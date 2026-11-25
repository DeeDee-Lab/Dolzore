"""No-network fixture tests; no real music assets or publication."""
import os
from pathlib import Path
import sys
import tempfile
import time
import unittest

sys.path.insert(0,str(Path(__file__).resolve().parents[1]/'scripts'))
import music_preview_local_ci as ci


class Contract(unittest.TestCase):
    def setUp(self):
        self.tmp=tempfile.TemporaryDirectory(); self.addCleanup(self.tmp.cleanup)
        self.root=Path(self.tmp.name); self.shop=self.root/'music-shop'; self.shop.mkdir()
        self.dist=self.shop/'dist'; self.evidence=self.root/'evidence'; self.evidence.mkdir()
        self.receipt={'steps':[],'status':'FAIL'}
        (self.shop/'build.py').write_text("from pathlib import Path\nfor name in ('music','tokusho'):\n p=Path('music-shop/dist')/name\n p.mkdir(parents=True,exist_ok=True)\n (p/'index.html').write_text('fixture')\n")
        (self.shop/'test_fixture.py').write_text('import unittest\nclass Fixture(unittest.TestCase):\n def test_one(self): self.assertTrue(True)\n')

    def execute(self,timeout=5):
        ci.execute(sys.executable,self.root,self.evidence,{'PATH':os.environ['PATH']},time.monotonic()+timeout,self.receipt)

    def test_actual_build_and_tests(self):
        self.execute(); self.assertEqual(self.receipt['status'],'PASS'); self.assertEqual(self.receipt['output_guard'],'PASS')
        self.assertEqual([r['name'] for r in self.receipt['steps']],['build','test'])
        self.assertTrue(all(r['log_sha256'] for r in self.receipt['steps']))

    def test_build_failure_stops(self):
        (self.shop/'build.py').write_text('raise SystemExit(2)'); self.execute()
        self.assertEqual(self.receipt['failed_step'],'build'); self.assertFalse((self.evidence/'test.log').exists())

    def test_empty_suite_rejected(self):
        (self.shop/'test_fixture.py').unlink(); self.execute()
        self.assertEqual(self.receipt['failed_step'],'test'); self.assertEqual(self.receipt['steps'][-1]['error'],'NO_EXECUTED_TESTS')

    def test_timeout(self):
        (self.shop/'build.py').write_text('import time; time.sleep(5)'); self.execute(.1)
        self.assertEqual(self.receipt['steps'][0]['error'],'TimeoutExpired')

    def test_paid_extension_case_insensitive(self):
        self.execute(); (self.dist/'paid.MP3').touch()
        with self.assertRaisesRegex(ValueError,'PAID_SOURCE_AUDIO_EXTENSION'): ci.verify_output(self.dist)

    def test_missing_legal_route(self):
        self.execute(); (self.dist/'tokusho/index.html').unlink()
        with self.assertRaisesRegex(ValueError,'MISSING_REQUIRED_EXPORT'): ci.verify_output(self.dist)

    def test_symlink_refused(self):
        self.execute(); (self.dist/'link').symlink_to(self.evidence,target_is_directory=True)
        with self.assertRaisesRegex(ValueError,'UNSAFE_EXPORT_PATH'): ci.verify_output(self.dist)

    def test_missing_dist(self):
        with self.assertRaisesRegex(ValueError,'INVALID_DIST'): ci.verify_output(self.dist)


if __name__=='__main__':
    unittest.main()
