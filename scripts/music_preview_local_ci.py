"""Offline preview CI; never publish or access paid audio."""
import argparse
import hashlib
import json
import os
from pathlib import Path
import re
import subprocess
import sys
import time

WORKFLOW = '.github/workflows/original-music-hp-preview-test.yml'
BLOB = 'f606805c8cb7125ab441f029bd742a40d160be53'
AUDIO = {'.mp3', '.wav', '.flac', '.m4a'}


def verify_output(dist):
    if dist.is_symlink() or not dist.is_dir():
        raise ValueError('INVALID_DIST')
    for path in dist.rglob('*'):
        if path.is_symlink() or not (path.is_file() or path.is_dir()):
            raise ValueError('UNSAFE_EXPORT_PATH')
        if path.is_file() and path.suffix.lower() in AUDIO:
            raise ValueError('PAID_SOURCE_AUDIO_EXTENSION')
    for name in ('music/index.html', 'tokusho/index.html'):
        if not (dist / name).is_file():
            raise ValueError('MISSING_REQUIRED_EXPORT:' + name)


def execute(python, root, evidence, env, deadline, receipt):
    steps = [('build', ['music-shop/build.py']),
             ('test', ['-m', 'unittest', 'discover', '-s', 'music-shop', '-p', 'test_*.py', '-v'])]
    for name, args in steps:
        row = {'name': name, 'argv': ['python', *args], 'returncode': None}
        receipt['steps'].append(row)
        log = evidence / (name + '.log')
        try:
            remaining = deadline - time.monotonic()
            if remaining <= 0:
                raise subprocess.TimeoutExpired(args, 0)
            with log.open('xb') as out:
                row['returncode'] = subprocess.run([str(python), *args], cwd=root, env=env, stdout=out,
                                                  stderr=subprocess.STDOUT, timeout=remaining).returncode
            if name == 'test':
                counts = re.findall(r'^Ran (\d+) tests? in ', log.read_text(encoding='utf-8', errors='replace'), re.M)
                if not counts or int(counts[-1]) == 0:
                    row['error'] = 'NO_EXECUTED_TESTS'
        except (OSError, subprocess.SubprocessError) as exc:
            row['error'] = type(exc).__name__
        row['log_sha256'] = hashlib.sha256(log.read_bytes()).hexdigest() if log.is_file() else None
        if row.get('error') or row['returncode'] != 0:
            receipt.update(status='FAIL', failed_step=name)
            return
    try:
        verify_output(root / 'music-shop/dist')
        receipt.update(status='PASS', output_guard='PASS')
    except (OSError, ValueError) as exc:
        receipt.update(status='FAIL', failed_step='output_guard', error=str(exc))


def run(root, sha, evidence, python, timeout=900):
    root, evidence, python = (Path(p).resolve() for p in (root, evidence, python))
    if not re.fullmatch('[0-9a-f]{40}', sha) or not 1 <= timeout <= 900:
        raise ValueError('INVALID_SHA_OR_TIMEOUT')
    if evidence.is_relative_to(root) or root.is_relative_to(evidence) or python.is_relative_to(root):
        raise ValueError('EXTERNAL_EVIDENCE_AND_TOOLCHAIN_REQUIRED')
    allowed = {'PATH','SYSTEMROOT','WINDIR','TEMP','TMP','TMPDIR','PATHEXT','COMSPEC','LANG'}
    env = {k:v for k,v in os.environ.items() if k.upper() in allowed}
    env.update(PYTHONNOUSERSITE='1', PYTHONDONTWRITEBYTECODE='1', PYTHONUTF8='1',
               GIT_CONFIG_NOSYSTEM='1', GIT_CONFIG_GLOBAL=os.devnull)
    def git(*args):
        return subprocess.check_output(['git', *args], cwd=root, env=env, text=True).strip()
    if Path(git('rev-parse', '--show-toplevel')).resolve() != root or git('rev-parse', 'HEAD') != sha:
        raise ValueError('CHECKOUT_MISMATCH')
    if git('status', '--porcelain=v1', '--untracked-files=all', '--ignored'):
        raise ValueError('DIRTY_CHECKOUT')
    workflow = root / WORKFLOW
    data = workflow.read_bytes()
    if workflow.resolve() != workflow or hashlib.sha1(b'blob '+str(len(data)).encode()+b'\0'+data).hexdigest() != BLOB:
        raise ValueError('WORKFLOW_DRIFT')
    shop = root / 'music-shop'
    if shop.resolve() != shop or (shop / 'dist').exists() or (shop / 'dist').is_symlink():
        raise ValueError('FRESH_DISPOSABLE_EXPORT_REQUIRED')
    version = subprocess.check_output([str(python), '--version'], env=env, text=True).strip()
    if not re.fullmatch(r'Python 3\.12\.\d+', version):
        raise ValueError('PYTHON312_REQUIRED')
    evidence.mkdir(exist_ok=False)
    home = evidence / 'home'; home.mkdir()
    env.update(HOME=str(home), USERPROFILE=str(home), CI='true')
    receipt = {'schema':'dolzore-music-preview-local/v1', 'source_sha':sha, 'workflow_blob':BLOB,
               'python_version':version, 'steps':[], 'status':'FAIL', 'publication_status':'NOT_EXECUTED',
               'actions_disable_authorized':False}
    execute(python, root, evidence, env, time.monotonic()+timeout, receipt)
    try:
        if git('rev-parse','HEAD') != sha or git('diff','--name-only','HEAD'):
            receipt.update(status='FAIL', error='TRACKED_SOURCE_CHANGED')
    except (OSError, subprocess.SubprocessError) as exc:
        receipt.update(status='FAIL', error=type(exc).__name__)
    with (evidence/'receipt.json').open('x',encoding='utf-8') as out:
        json.dump(receipt,out,indent=2); out.write('\n')
    return 0 if receipt['status']=='PASS' else 1


def main():
    p = argparse.ArgumentParser(description=__doc__)
    p.add_argument('--approved-sha',required=True)
    p.add_argument('--evidence',required=True,type=Path)
    p.add_argument('--python',required=True,type=Path)
    p.add_argument('--run-isolated',action='store_true',help='Requires approved disposable isolation; not a sandbox')
    a = p.parse_args()
    if not a.run_isolated:
        p.error('separately approved isolation required')
    return run(Path(__file__).resolve().parents[1],a.approved_sha,a.evidence,a.python)


if __name__ == '__main__':
    sys.exit(main())
