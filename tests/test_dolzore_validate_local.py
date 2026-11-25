"""Local fixture tests exercising actual unittest subprocesses; no live site access."""
import hashlib
import json
from pathlib import Path
import subprocess
import sys
import tempfile
import unittest

sys.path.insert(0,str(Path(__file__).resolve().parents[1]/'scripts'))
import dolzore_validate_local as ci

WORKFLOW='''name: Validate DOLZORE music-only site

on:
  pull_request:
    branches: [main]
  push:
    branches: [music-only-site-20260928]
    paths:
      - "docs/**"
      - "tests/**"
      - "content/articles/**"
      - ".github/workflows/validate-site.yml"

permissions:
  contents: read

jobs:
  contracts:
    runs-on: ubuntu-latest
    timeout-minutes: 15
    steps:
      - uses: actions/checkout@v4
      - name: Run public-site contract tests
        run: python -m unittest discover -s tests -p "test_*.py" -v
'''


class Contract(unittest.TestCase):
    def setUp(self):
        self.temp=tempfile.TemporaryDirectory(); self.addCleanup(self.temp.cleanup)
        base=Path(self.temp.name); self.root=base/'checkout'; self.root.mkdir()
        self.evidence=base/'evidence'
        self.git('init','-q'); self.git('config','user.name','fixture'); self.git('config','user.email','fixture@example.invalid')
        path=self.root/ci.WORKFLOW; path.parent.mkdir(parents=True); path.write_text(WORKFLOW)
        (self.root/'tests').mkdir(); self.testfile=self.root/'tests/test_example.py'
        self.sha=self.fixture('self.assertTrue(True)')

    def git(self,*args):
        return subprocess.check_output(['git',*args],cwd=self.root,text=True).strip()

    def commit(self):
        self.git('add','.'); self.git('commit','-qm','fixture'); return self.git('rev-parse','HEAD')

    def fixture(self,body):
        self.testfile.write_text('import unittest\nclass Example(unittest.TestCase):\n    def test_example(self):\n        '+body+'\n')
        return self.commit()

    def run_ci(self,sha=None,timeout=5):
        return ci.run(self.root,sha or self.sha,self.evidence,sys.executable,timeout)

    def result(self):
        return json.loads((self.evidence/'receipt.json').read_text())

    def test_real_success(self):
        self.assertEqual(self.run_ci(),0)
        r=self.result(); self.assertEqual(r['status'],'PASS'); self.assertEqual(r['tests_run'],1)
        self.assertEqual(r['log_sha256'],hashlib.sha256((self.evidence/'unittest.log').read_bytes()).hexdigest())

    def test_real_failure(self):
        self.sha=self.fixture('self.fail("fixture")')
        self.assertEqual(self.run_ci(),1); self.assertEqual(self.result()['status'],'FAIL')

    def test_zero_tests_rejected(self):
        self.testfile.write_text('import unittest\n'); self.sha=self.commit()
        self.assertEqual(self.run_ci(),1); self.assertEqual(self.result()['error'],'NO_EXECUTED_TESTS')

    def test_timeout(self):
        self.sha=self.fixture('import time; time.sleep(5)')
        self.assertEqual(self.run_ci(timeout=1),1); self.assertEqual(self.result()['error'],'TimeoutExpired')

    def test_mutation(self):
        self.sha=self.fixture('from pathlib import Path; Path("tests/test_example.py").write_text("changed")')
        self.assertEqual(self.run_ci(),1); self.assertEqual(self.result()['error'],'TRACKED_SOURCE_CHANGED')

    def test_dirty(self):
        (self.root/'untracked').touch()
        with self.assertRaisesRegex(ValueError,'DIRTY_CHECKOUT'): self.run_ci()

    def test_wrong_commit(self):
        with self.assertRaisesRegex(ValueError,'CHECKOUT_MISMATCH'): self.run_ci('a'*40)

    def test_drift(self):
        (self.root/ci.WORKFLOW).write_text('changed'); self.sha=self.commit()
        with self.assertRaisesRegex(ValueError,'WORKFLOW_DRIFT'): self.run_ci()

    def test_existing_evidence(self):
        self.evidence.mkdir(); (self.evidence/'receipt.json').write_text('keep')
        with self.assertRaises(FileExistsError): self.run_ci()
        self.assertEqual((self.evidence/'receipt.json').read_text(),'keep')

    def test_original_blob(self):
        self.assertEqual(ci.blob(WORKFLOW.encode()),ci.WORKFLOW_BLOB)


if __name__=='__main__': unittest.main()
