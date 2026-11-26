"""Offline carrier for First Town QA; requires Muse-approved isolated browser runtime."""
import argparse
from contextlib import contextmanager
import functools
import hashlib
from http.server import SimpleHTTPRequestHandler, ThreadingHTTPServer
import json
import os
from pathlib import Path
import re
import socket
import subprocess
import sys
import threading
import time

WORKFLOW = ".github/workflows/first-town-qa.yml"
QA = "tests/first_town_qa.py"
PINS = {WORKFLOW: "3bd0a7c0940371c84b4e3bff96842574619d9212",
        QA: "f7b9aae6ee02c8bc2cd9620d552bb12bbad8e625"}
CONTRACT = ["-m", "unittest", "discover", "-s", "tests", "-p", "test_*.py", "-v"]
SHOTS = {"district-" + name + ".png" for name in (
    "residential-hill", "central-main-street", "market-workshop", "civic-journal",
    "riverside", "station-east-gate", "back-alley")} | {"desktop-first-town.png", "mobile-first-town.png"}
MARKERS = {"FIRST_TOWN_QA_PASS", "WORLD_SIZE_1536x1152=PASS", "SCROLLING_CAMERA=PASS",
           "DISTRICTS_7=PASS", "LANDMARKS_BAR_JOURNAL_RIVER_STATION=PASS",
           "MOVE_JUMP_BGM_CONTROLS=PASS", "MOBILE_CONTROLS=PASS", "HORIZONTAL_OVERFLOW=0"}


def blob(data):
    return hashlib.sha1(f"blob {len(data)}\0".encode() + data).hexdigest()


def digest(data):
    return hashlib.sha256(data).hexdigest()


def clean_env(home, browsers):
    allowed = {"PATH", "SYSTEMROOT", "WINDIR", "TEMP", "TMP", "TMPDIR", "PATHEXT", "COMSPEC", "LANG"}
    env = {key: value for key, value in os.environ.items() if key.upper() in allowed}
    env.update(HOME=str(home), USERPROFILE=str(home), PYTHONNOUSERSITE="1", PYTHONDONTWRITEBYTECODE="1",
               PYTHONUTF8="1", PLAYWRIGHT_BROWSERS_PATH=str(browsers), GIT_CONFIG_NOSYSTEM="1",
               GIT_CONFIG_GLOBAL=os.devnull, GIT_ALLOW_PROTOCOL="", GIT_NO_REPLACE_OBJECTS="1",
               GIT_NO_LAZY_FETCH="1", GIT_TERMINAL_PROMPT="0")
    return env


def checkout(root, sha, env, *, pristine):
    def git(*args):
        return subprocess.check_output(["git", "-C", str(root), *args], env=env, timeout=30)
    if not re.fullmatch(r"[0-9a-f]{40}", sha) or Path(os.fsdecode(git("rev-parse", "--show-toplevel").rstrip())).resolve() != root or git("rev-parse", "HEAD").decode().strip() != sha:
        raise ValueError("CHECKOUT_MISMATCH")
    if pristine and git("status", "--porcelain", "--untracked-files=all", "--ignored"):
        raise ValueError("CHECKOUT_NOT_PRISTINE")
    if git("diff", "--name-only", "HEAD"):
        raise ValueError("TRACKED_SOURCE_CHANGED")
    for path, expected in PINS.items():
        file = root / path
        data = git("show", f"{sha}:{path}")
        if file.resolve() != file or blob(data) != expected or file.read_bytes() != data:
            raise ValueError("SOURCE_PIN_MISMATCH:" + path)


class ExclusiveServer(ThreadingHTTPServer):
    allow_reuse_address = False
    allow_reuse_port = False

    def server_bind(self):
        if os.name == "nt":
            self.socket.setsockopt(socket.SOL_SOCKET, socket.SO_EXCLUSIVEADDRUSE, 1)
        super().server_bind()


@contextmanager
def local_site(docs, port=8765):
    if docs.resolve() != docs or not docs.is_dir():
        raise ValueError("INVALID_DOCS_DIRECTORY")
    handler = functools.partial(SimpleHTTPRequestHandler, directory=str(docs))
    server = ExclusiveServer(("127.0.0.1", port), handler)
    thread = threading.Thread(target=server.serve_forever, daemon=True)
    thread.start()
    try:
        yield server.server_address[1]
    finally:
        server.shutdown()
        server.server_close()
        thread.join(timeout=5)


def screenshot_receipt(folder, qa_log):
    if folder.is_symlink() or not folder.is_dir() or set(qa_log.splitlines()) & MARKERS != MARKERS:
        raise ValueError("QA_MARKERS_OR_DIRECTORY_MISSING")
    if {p.name for p in folder.iterdir()} != SHOTS:
        raise ValueError("SCREENSHOT_SET_MISMATCH")
    result = {}
    for name in sorted(SHOTS):
        file = folder / name
        if file.is_symlink() or not file.is_file() or not 8 < file.stat().st_size <= 32 * 1024 * 1024:
            raise ValueError("INVALID_SCREENSHOT:" + name)
        data = file.read_bytes()
        if not data.startswith(b"\x89PNG\r\n\x1a\n"):
            raise ValueError("INVALID_SCREENSHOT:" + name)
        result[name] = digest(data)
    return result


def run(root, sha, evidence, python, browsers, *, approved=False, timeout=1200):
    if not approved or os.name != "nt" or os.getenv("COMPUTERNAME") != "OSK222" or not 1 <= timeout <= 1200:
        raise ValueError("APPROVED_OSK222_BROWSER_ISOLATION_REQUIRED")
    root, evidence, python, browsers = (Path(p).resolve() for p in (root, evidence, python, browsers))
    if root.is_relative_to(evidence) or evidence.is_relative_to(root) or any(p.is_relative_to(root) or not p.exists() for p in (python, browsers)):
        raise ValueError("EXTERNAL_EVIDENCE_AND_TOOLCHAIN_REQUIRED")
    env = clean_env(evidence, browsers)
    checkout(root, sha, env, pristine=True)
    version = subprocess.check_output([str(python), "--version"], env=env, timeout=10).decode().strip()
    if version != "Python 3.12.10":
        raise ValueError("APPROVED_PYTHON_VERSION_REQUIRED")
    evidence.mkdir(exist_ok=False)
    receipt = {"schema": "dolzore-first-town-local/v1", "source_sha": sha, "pins": PINS,
               "python_version": version, "steps": [], "status": "FAIL", "runtime_acceptance": "NOT_ESTABLISHED",
               "migration_complete": False, "actions_disable_authorized": False}
    deadline = time.monotonic() + timeout

    def step(name, args):
        remaining = deadline - time.monotonic()
        if remaining <= 0:
            raise ValueError("TIMEOUT")
        log = evidence / (name + ".log")
        with log.open("xb") as out:
            rc = subprocess.run([str(python), *args], cwd=root, env=env, stdout=out,
                                stderr=subprocess.STDOUT, timeout=remaining, check=False).returncode
        data = log.read_bytes()
        receipt["steps"].append({"name": name, "returncode": rc, "log_sha256": digest(data)})
        if rc:
            raise ValueError(name + "_FAILED:" + str(rc))
        return data.decode("utf-8", errors="replace")

    try:
        contract = step("contract", CONTRACT)
        counts = re.findall(r"^Ran (\d+) tests? in [0-9.]+s\s*$", contract, re.M)
        if not counts or int(counts[-1]) == 0:
            raise ValueError("NO_EXECUTED_CONTRACT_TESTS")
        checkout(root, sha, env, pristine=False)
        if (root / "qa-first-town").exists():
            raise ValueError("PREEXISTING_SCREENSHOTS")
        with local_site(root / "docs"):
            qa_log = step("first-town", [QA])
        fingerprints = screenshot_receipt(root / "qa-first-town", qa_log)
        checkout(root, sha, env, pristine=False)
        shots = evidence / "screenshots"
        shots.mkdir()
        for name, expected in fingerprints.items():
            data = (root / "qa-first-town" / name).read_bytes()
            if digest(data) != expected:
                raise ValueError("SCREENSHOT_DRIFT")
            with (shots / name).open("xb") as out:
                out.write(data)
            if digest((shots / name).read_bytes()) != expected:
                raise ValueError("SCREENSHOT_READBACK_MISMATCH")
        receipt.update(status="PASS", screenshots=fingerprints, contract_tests=int(counts[-1]))
    except (OSError, ValueError, subprocess.SubprocessError) as exc:
        receipt["error"] = str(exc)
    finally:
        payload = (json.dumps(receipt, sort_keys=True, indent=2) + "\n").encode()
        with (evidence / "receipt.json").open("xb") as out:
            out.write(payload)
            out.flush()
            os.fsync(out.fileno())
        if (evidence / "receipt.json").read_bytes() != payload:
            raise OSError("RECEIPT_READBACK_MISMATCH")
    return int(receipt["status"] != "PASS")


def main():
    parser = argparse.ArgumentParser(description=__doc__)
    for arg in ("checkout", "evidence", "python", "browsers"):
        parser.add_argument("--" + arg, required=True, type=Path)
    parser.add_argument("--approved-sha", required=True)
    parser.add_argument("--run-isolated", action="store_true")
    args = parser.parse_args()
    try:
        return run(args.checkout, args.approved_sha, args.evidence, args.python, args.browsers, approved=args.run_isolated)
    except (OSError, ValueError, subprocess.SubprocessError) as exc:
        print("FIRST_TOWN_LOCAL_REJECTED:" + str(exc), file=sys.stderr)
        return 2


if __name__ == "__main__":
    raise SystemExit(main())
