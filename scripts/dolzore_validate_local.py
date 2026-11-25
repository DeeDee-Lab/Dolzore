"""Local replacement for validate-site.yml, in an approved disposable checkout."""
import argparse
import hashlib
import json
import os
from pathlib import Path
import re
import subprocess
import sys

WORKFLOW = '.github/workflows/validate-site.yml'
WORKFLOW_BLOB = '1043fd200b54327f2fc12bdd3632290136f9a0f0'
ARGS = ['-m','unittest','discover','-s','tests','-p','test_*.py','-v']


def blob(data):
    return hashlib.sha1(b'blob ' + str(len(data)).encode() + b'\0' + data).hexdigest()


def run(root, sha, evidence, python, timeout=900):
    root, evidence, python = (Path(x).resolve() for x in (root,evidence,python))
    if not re.fullmatch('[0-9a-f]{40}',sha) or not 1 <= timeout <= 900:
        raise ValueError('INVALID_SHA_OR_TIMEOUT')
    if evidence.is_relative_to(root) or root.is_relative_to(evidence) or python.is_relative_to(root):
        raise ValueError('EXTERNAL_EVIDENCE_AND_TOOLCHAIN_REQUIRED')
    allowed = {'PATH','SYSTEMROOT','WINDIR','TEMP','TMP','TMPDIR','PATHEXT','COMSPEC','LANG'}
    env = {k:v for k,v in os.environ.items() if k.upper() in allowed}
    env.update(PYTHONNOUSERSITE='1',PYTHONDONTWRITEBYTECODE='1',PYTHONUTF8='1',
               GIT_CONFIG_NOSYSTEM='1',GIT_CONFIG_GLOBAL=os.devnull)
    def git(*args):
        return subprocess.check_output(['git',*args],cwd=root,env=env,text=True).strip()
    if Path(git('rev-parse','--show-toplevel')).resolve()!=root or git('rev-parse','HEAD')!=sha:
        raise ValueError('CHECKOUT_MISMATCH')
    if git('status','--porcelain=v1','--untracked-files=all','--ignored'):
        raise ValueError('DIRTY_CHECKOUT')
    workflow = root / WORKFLOW
    if workflow.resolve()!=workflow or blob(workflow.read_bytes())!=WORKFLOW_BLOB:
        raise ValueError('WORKFLOW_DRIFT')
    testdir=root/'tests'
    if testdir.resolve()!=testdir or not testdir.is_dir():
        raise ValueError('INVALID_TEST_DIRECTORY')
    version=subprocess.check_output([str(python),'--version'],env=env,text=True).strip()
    evidence.mkdir(exist_ok=False)
    home=evidence/'home'; home.mkdir()
    env.update(HOME=str(home),USERPROFILE=str(home),CI='true')
    log=evidence/'unittest.log'
    receipt={'schema':'dolzore-validate-local/v1','source_sha':sha,'workflow_blob':WORKFLOW_BLOB,
             'python_version':version,'argv':['python',*ARGS],'status':'ERROR','returncode':None,
             'tests_run':None,'actions_disable_authorized':False}
    try:
        with log.open('xb') as out:
            p=subprocess.run([str(python),*ARGS],cwd=root,env=env,stdout=out,stderr=subprocess.STDOUT,timeout=timeout)
        receipt['returncode']=p.returncode
        text=log.read_text(encoding='utf-8',errors='replace')
        summaries=re.findall(r'^Ran (\d+) tests? in [0-9.]+s\s*$',text,re.M)
        if summaries and int(summaries[-1])==0:
            receipt.update(status='FAIL',error='NO_EXECUTED_TESTS',tests_run=0)
        elif p.returncode!=0:
            receipt['status']='FAIL'
        elif not summaries:
            receipt.update(status='FAIL',error='NO_EXECUTED_TESTS')
        else:
            receipt.update(status='PASS',tests_run=int(summaries[-1]))
        if git('rev-parse','HEAD')!=sha or git('diff','--name-only','HEAD'):
            receipt.update(status='FAIL',error='TRACKED_SOURCE_CHANGED')
    except (OSError,subprocess.SubprocessError) as exc:
        receipt.update(status='ERROR',error=type(exc).__name__)
    receipt['log_sha256']=hashlib.sha256(log.read_bytes()).hexdigest() if log.is_file() else None
    with (evidence/'receipt.json').open('x',encoding='utf-8') as out:
        json.dump(receipt,out,indent=2); out.write('\n')
    return 0 if receipt['status']=='PASS' else 1


def main():
    p=argparse.ArgumentParser(description=__doc__)
    p.add_argument('--approved-sha',required=True)
    p.add_argument('--evidence',required=True,type=Path)
    p.add_argument('--python',required=True,type=Path)
    p.add_argument('--timeout',type=int,default=900)
    p.add_argument('--run-isolated',action='store_true',help='Requires separately approved isolation; not a sandbox')
    a=p.parse_args()
    if not a.run_isolated:
        p.error('separately approved isolation required')
    return run(Path(__file__).resolve().parents[1],a.approved_sha,a.evidence,a.python,a.timeout)


if __name__=='__main__':
    sys.exit(main())
