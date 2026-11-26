"""Three pinned Dolzore visual QA candidates; use an approved isolated checkout."""
import argparse
from contextlib import contextmanager
from functools import partial
import hashlib
from http.server import SimpleHTTPRequestHandler, ThreadingHTTPServer
import json
import os
from pathlib import Path
import re
import shutil
import subprocess
import sys
from threading import Thread
import time

PROFILES = {
    'first-town': ('first-town-qa.yml', '3bd0a7c0940371c84b4e3bff96842574619d9212',
                   'first_town_qa.py', 'f7b9aae6ee02c8bc2cd9620d552bb12bbad8e625', 'qa-first-town', 1200),
    'slice': ('slice-qa-cloud.yml', '34075da936902ca9b4b7a61b71cdff5a73b95356',
              'game_slice_qa.py', 'c6055ad07847637d3029e674deba6fd136a9dc3f', 'qa-slice1', 1200),
    'visual': ('visual-qa.yml', 'f9552866d210c5009b768e6d9d1936ad3f51e834',
               'visual_qa.py', '2bbb50ea8de98f9cc1e72ffbc4b9b9f926bd5c2a', 'qa-screenshots', 1500),
}
PREVIEWS = {'desktop-home-preview.jpg', 'desktop-jukebox-preview.jpg', 'desktop-journal-preview.jpg',
            'mobile-home-preview.jpg', 'mobile-music-preview.jpg', 'mobile-journal-preview.jpg'}


def blob(data):
    return hashlib.sha1(b'blob ' + str(len(data)).encode() + b'\0' + data).hexdigest()


def clean_env():
    allowed = {'PATH', 'SYSTEMROOT', 'WINDIR', 'TEMP', 'TMP', 'TMPDIR', 'PATHEXT', 'COMSPEC', 'LANG'}
    env = {k: v for k, v in os.environ.items() if k.upper() in allowed}
    env.update(PYTHONNOUSERSITE='1', PYTHONDONTWRITEBYTECODE='1', PYTHONUTF8='1',
               GIT_CONFIG_NOSYSTEM='1', GIT_CONFIG_GLOBAL=os.devnull)
    return env


@contextmanager
def local_site(docs, port=8765):
    # A static site must not expose files reached through symlinks.
    if docs.is_symlink() or any(p.is_symlink() for p in docs.rglob("*")):
        raise ValueError("UNSAFE_SITE_SYMLINK")
    # Own the bound socket; never kill or reuse another process's listener.
    server = ThreadingHTTPServer(('127.0.0.1', port), partial(SimpleHTTPRequestHandler, directory=str(docs)))
    thread = Thread(target=server.serve_forever, daemon=True)
    thread.start()
    try:
        yield server.server_address[1]
    finally:
        server.shutdown()
        server.server_close()
        thread.join()


def collect_artifacts(root, output, target, profile, *, require_complete=True):
    source = root / output
    if source.is_symlink() or not source.is_dir() or source.resolve().parent != root:
        raise ValueError('QA_OUTPUT_MISSING_OR_UNSAFE')
    items = sorted(source.iterdir())
    if not items or any(p.is_symlink() or not p.is_file() for p in items):
        raise ValueError('QA_ARTIFACTS_MISSING_OR_UNSAFE')
    if require_complete and profile == 'visual' and not PREVIEWS <= {p.name for p in items}:
        raise ValueError('REQUIRED_PREVIEWS_MISSING')
    target.mkdir(exist_ok=False)
    rows = []
    for p in items:
        dest = target / p.name
        shutil.copyfile(p, dest)
        rows.append({'name': p.name, 'bytes': dest.stat().st_size,
                     'sha256': hashlib.sha256(dest.read_bytes()).hexdigest()})
        if rows[-1]['bytes'] == 0:
            raise ValueError('EMPTY_QA_ARTIFACT')
    return rows


def run(profile, root, sha, evidence, python, browser_cache):
    root, evidence, python, browser_cache = map(lambda p: Path(p).resolve(), (root, evidence, python, browser_cache))
    workflow, workflow_blob, script, script_blob, output, timeout = PROFILES[profile]
    if profile == 'visual' and os.environ.get('COMPUTERNAME', '').upper() != 'OSK222':
        raise ValueError('DOLZORE_WRONG_HOST')
    if not re.fullmatch('[0-9a-f]{40}', sha):
        raise ValueError('INVALID_SHA')
    if any(p.is_relative_to(root) or root.is_relative_to(p) for p in (evidence, python, browser_cache)):
        raise ValueError('EXTERNAL_TOOLCHAIN_AND_EVIDENCE_REQUIRED')
    if not browser_cache.is_dir() or evidence.exists():
        raise ValueError('APPROVED_BROWSER_CACHE_AND_NEW_EVIDENCE_REQUIRED')
    env = clean_env()
    def git(*args):
        return subprocess.check_output(['git', *args], cwd=root, env=env, text=True).strip()
    if Path(git('rev-parse', '--show-toplevel')).resolve() != root or git('rev-parse', 'HEAD') != sha:
        raise ValueError('CHECKOUT_MISMATCH')
    if git('status', '--porcelain=v1', '--untracked-files=all', '--ignored'):
        raise ValueError('DIRTY_CHECKOUT')
    for rel, expected in (('.github/workflows/'+workflow, workflow_blob), ('tests/'+script, script_blob)):
        p = root / rel
        if p.resolve() != p or blob(p.read_bytes()) != expected:
            raise ValueError('SOURCE_DRIFT:'+rel)
    if (root/output).exists() or (root/output).is_symlink() or not (root/'docs').is_dir() or (root/'docs').is_symlink():
        raise ValueError('UNSAFE_OR_STALE_SITE_INPUT')
    evidence.mkdir(exist_ok=False)
    home = evidence/'home'; home.mkdir()
    env.update(HOME=str(home), USERPROFILE=str(home), PLAYWRIGHT_BROWSERS_PATH=str(browser_cache), CI='true')
    receipt = {'schema':'dolzore-visual-qa-local/v1','profile':profile,'source_sha':sha,'workflow_blob':workflow_blob,
               'qa_script_blob':script_blob,'steps':[],'status':'FAIL','actions_disable_authorized':False,
               'production_authorized':False,'runtime_acceptance':'NOT_ESTABLISHED',
               'browser_setup':'PREPROVISIONED_BY_APPROVED_RUNNER_NO_DOWNLOAD',
               'output_dir':output,'original_fingerprint_dir_mismatch':profile=='slice'}
    deadline = time.monotonic()+timeout
    def step(name, args):
        log = evidence/(name+'.log'); row={'name':name,'returncode':None}; receipt['steps'].append(row)
        try:
            remaining = deadline-time.monotonic()
            if remaining <= 0: raise subprocess.TimeoutExpired(args,0)
            with log.open('xb') as f:
                row['returncode']=subprocess.run([str(python),*args],cwd=root,env=env,stdout=f,stderr=subprocess.STDOUT,timeout=remaining).returncode
            if name=='contracts':
                counts=re.findall(r'^Ran (\d+) tests? in ',log.read_text(encoding='utf-8',errors='replace'),re.M)
                if not counts or int(counts[-1])==0: raise ValueError('NO_EXECUTED_TESTS')
            if row['returncode']!=0: raise ValueError('STEP_FAILED:'+name)
        finally:
            row['log_sha256']=hashlib.sha256(log.read_bytes()).hexdigest() if log.is_file() else None
    try:
        step('contracts',['-m','unittest','discover','-s','tests','-p','test_*.py','-v'])
        step('playwright-version',['-m','playwright','--version'])
        with local_site(root/'docs'):
            step('browser-qa',['tests/'+script])
        receipt['artifacts']=collect_artifacts(root,output,evidence/'screenshots',profile)
        if git('rev-parse','HEAD')!=sha or git('diff','--name-only','HEAD'):
            raise ValueError('TRACKED_SOURCE_CHANGED')
        receipt['status']='PASS'
    except (ValueError,OSError,subprocess.SubprocessError) as exc:
        receipt['error']=str(exc)
    finally:
        # Like the original always() fingerprint step, retain partial output on failure.
        if receipt['status']=='FAIL' and (root/output).exists() and not (evidence/'screenshots').exists():
            try:
                receipt['partial_artifacts']=collect_artifacts(root,output,evidence/'screenshots',profile,require_complete=False)
            except (ValueError,OSError) as exc:
                receipt['partial_artifact_error']=str(exc)
        with (evidence/'receipt.json').open('x',encoding='utf-8') as f:
            json.dump(receipt,f,indent=2); f.write('\n')
    return 0 if receipt['status']=='PASS' else 1


def main():
    p=argparse.ArgumentParser(description=__doc__)
    p.add_argument('--profile',required=True,choices=PROFILES)
    p.add_argument('--approved-sha',required=True)
    for n in ('evidence','python','browser-cache'):
        p.add_argument('--'+n,required=True,type=Path)
    p.add_argument('--run-isolated',action='store_true',help='Requires separately approved disposable checkout and network isolation; not a sandbox')
    a=p.parse_args()
    if not a.run_isolated: p.error('separately approved isolation required')
    return run(a.profile,Path(__file__).resolve().parents[1],a.approved_sha,a.evidence,a.python,a.browser_cache)


if __name__=='__main__':
    sys.exit(main())
