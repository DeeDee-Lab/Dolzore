"""Local one-shot laptop scan carrier. No purchase, push or schedule changes.

Run only in a separately approved, disposable checkout. The local lock is not
an OS sandbox or a global writer lock. Collector network access requires HQ approval.
"""
from __future__ import annotations

import argparse
import hashlib
import importlib.metadata
import json
import os
import re
import stat
import subprocess
import sys
import tempfile
import time
import unittest
from datetime import datetime, timezone, timedelta
from pathlib import Path
from urllib.parse import urlsplit

BASE = "1db115626e657eab5d171a34fd09bf4bbdfc6034"
SCRIPT = "scripts/collect_gaming_laptops.py"
TARGET = "docs/data/targets/used-gaming-laptop-14-rtx4060.json"
OUTPUT = "docs/data/smartbuy-gaming-laptops.json"
PINS = {
    ".github/workflows/collect-gaming-laptops-once.yml": "003c1ace15c0ebb6e0b0367739a50b745a45ae96",
    SCRIPT: "a9d38223e81eb0d605e6532b10d6259d434ce2cd",
    "scripts/update_market_snapshot.py": "27365acd158d598bbbdb6f1fed64051486a8ebcd",
    "tests/test_gaming_laptop_collector.py": "02e34a30700cbba9d0f9b8a353a45e18f326cb61",
    TARGET: "190d8ecd6d8e02761671cbaee90ec250bc5082bf",
    OUTPUT: "eff39e2b29dd13d15a0dd10fa8170f046ce001a3",
}
LOCK = ".smartbuy-laptop-one-shot.lock"
LIMIT = 8 * 1024 * 1024


def need(ok, message):
    if not ok:
        raise ValueError(message)


def digest(data):
    return hashlib.sha256(data).hexdigest()


def blob(data):
    return hashlib.sha1(f"blob {len(data)}\0".encode() + data).hexdigest()


def pairs(items):
    result = {}
    for key, value in items:
        need(key not in result, "duplicate JSON key")
        result[key] = value
    return result


def reject_constant(_):
    raise ValueError("nonfinite JSON")


def load(data):
    return json.loads(data, object_pairs_hook=pairs, parse_constant=reject_constant)


def read(path):
    need(stat.S_ISREG(path.lstat().st_mode), "regular file required")
    flags = os.O_RDONLY | getattr(os, "O_BINARY", 0) | getattr(os, "O_NOFOLLOW", 0) | getattr(os, "O_NONBLOCK", 0)
    with os.fdopen(os.open(path, flags), "rb") as handle:
        info = os.fstat(handle.fileno())
        need(stat.S_ISREG(info.st_mode) and info.st_size <= LIMIT, "input size or type")
        data = handle.read(LIMIT + 1)
    need(len(data) <= LIMIT, "input too large")
    return data


def write_once(path, data):
    with path.open("xb") as handle:
        handle.write(data)
        handle.flush()
        os.fsync(handle.fileno())
    need(read(path) == data, "evidence readback mismatch")


def dump(value):
    return (json.dumps(value, sort_keys=True, ensure_ascii=True, allow_nan=False) + "\n").encode()


def plan(event):
    need(event in ("pull_request", "push", "workflow_dispatch"), "one-shot event only; no schedule")
    return event != "pull_request"


def validate(report, target, now, *, local=True):
    need(isinstance(report, dict) and isinstance(target, dict), "objects required")
    need(type(report.get("schemaVersion")) is int and report["schemaVersion"] == 1 and report.get("targetId") == "used-gaming-laptop-14-rtx4060" and report.get("target") == target, "target identity")
    need(target.get("policy", {}).get("neverAutoBuy") is True and target.get("condition") == "used", "target policy")
    need(type(report.get("verifiedBuyCount")) is int and report["verifiedBuyCount"] == 0, "BUY forbidden")
    if local:
        need(report.get("runId") is None and report.get("runUrl") is None, "false Actions identity")
    stamp = datetime.fromisoformat(report["generatedAt"])
    need(stamp.tzinfo is not None and stamp.utcoffset() is not None and -120 <= (now - stamp).total_seconds() < 900, "report freshness")
    results = report.get("results")
    observed, qualified, inspected = (report.get(k) for k in ("observedCount", "qualifiedForConsiderCount", "detailPagesInspected"))
    need(all(type(x) is int and x >= 0 for x in (observed, qualified, inspected)), "invalid counts")
    need(inspected >= observed >= qualified and isinstance(results, list) and len(results) == min(observed, 10), "result counts")
    rejected = report.get("rejections")
    need(isinstance(rejected, dict) and all(type(v) is int and v >= 0 for v in rejected.values()) and sum(rejected.values()) + observed == inspected, "inspection count mismatch")
    need(report.get("status") == ("completed_market_observation" if observed else "scanned_no_matching_individual_listings"), "report status")
    seen_consider = 0
    for row in results:
        need(isinstance(row, dict), "invalid row")
        url = urlsplit(row["url"])
        need(url.scheme == "https" and bool(url.hostname) and url.username is None and url.password is None, "invalid listing URL")
        need(row.get("market") in target["sellerMarkets"] and row.get("model") in target["models"] and row.get("inventory") == "unconfirmed_at_checkout" and row.get("condition") == "used_listing_unverified", "unverified listing boundary")
        price = row.get("priceJPY")
        need(type(price) is int and price > 0, "invalid price")
        gpu = row.get("gpu")
        need(gpu is None or gpu in [x.upper().replace(" ", "") for x in target["gpu"]["acceptedLaptopGpuModels"]], "GPU out of target")
        for key, low, high in (("memoryGB", target["memoryGB"]["min"], float("inf")), ("ssdGB", target["ssdGB"]["min"], float("inf")), ("screenInches", target["screenInches"]["min"], target["screenInches"]["max"]), ("weightKg", 0, target["maxWeightKg"])):
            value = row.get(key)
            need(value is None or (type(value) in (int, float) and low <= value <= high and value > 0), "hardware out of target: " + key)
        complete = all(row.get(k) is not None for k in ("gpu", "memoryGB", "ssdGB", "screenInches", "weightKg"))
        in_price = target["priceJPY"]["min"] <= price <= target["priceJPY"]["max"]
        decision = "WAIT" if not in_price else "CONSIDER" if complete else "HOLD"
        need(row.get("decision") == decision, "false decision")
        seen_consider += decision == "CONSIDER"
    need(seen_consider == min(qualified, len(results)), "consider count mismatch")
    return {"observed": observed, "qualified": qualified, "inspected": inspected, "verifiedBuyCount": 0}


def run(checkout, evidence, lock_path, lock_sha, run_id, event, approved_egress=False):
    collect = plan(event)
    need(not collect or approved_egress, "separate egress approval required")
    checkout, evidence, lock_path = (Path(p).resolve(strict=True) for p in (checkout, evidence, lock_path))
    need(re.fullmatch(r"[A-Za-z0-9_-]{1,80}", run_id) and checkout.is_dir() and evidence.is_dir(), "invalid run identity")
    need(not evidence.is_relative_to(checkout) and not checkout.is_relative_to(evidence) and not lock_path.is_relative_to(checkout), "separate private evidence and dependency lock required")
    need(sys.version_info[:2] == (3, 11) and not Path(sys.executable).resolve().is_relative_to(checkout), "external Python 3.11 required")
    env = {k: v for k, v in os.environ.items() if not k.upper().startswith(("GIT_", "GITHUB_", "PYTHON", "PIP_"))}
    env.update(GIT_NO_REPLACE_OBJECTS="1", GIT_NO_LAZY_FETCH="1", GIT_TERMINAL_PROMPT="0", GIT_OPTIONAL_LOCKS="0", PYTHONUTF8="1", PYTHONNOUSERSITE="1", PYTHONDONTWRITEBYTECODE="1")
    def git(*args):
        return subprocess.check_output(["git", "-c", "protocol.allow=never", "-C", str(checkout), *args], env=env, timeout=30)
    def clean():
        need(git("rev-parse", "HEAD").decode().strip() == BASE and not git("status", "--porcelain", "--untracked-files=all"), "pinned clean checkout required")
    lock = evidence / LOCK
    lock.mkdir()
    uncertain = False
    directory = None
    try:
        need(Path(os.fsdecode(git("rev-parse", "--show-toplevel").strip())).resolve() == checkout and (checkout / ".git").is_dir() and not (checkout / ".git/objects/info/alternates").exists(), "standalone checkout required")
        clean()
        for path, expected in PINS.items():
            file = checkout / path
            need(file.resolve() == file and blob(read(file)) == expected and blob(git("show", f"{BASE}:{path}")) == expected, "source pin drift: " + path)
        dep = read(lock_path)
        need(digest(dep) == lock_sha, "dependency lock hash mismatch")
        approved = load(dep)
        need(isinstance(approved, dict) and set(approved) == {"requests", "beautifulsoup4"} and all(importlib.metadata.version(k) == v for k, v in approved.items()), "approved dependency versions mismatch")
        destination = evidence / run_id
        destination.mkdir()
        directory = destination
        write_once(directory / "intent.json", dump({"schema": "dolzore-laptop-one-shot-intent/v1", "run_id": run_id, "event": event, "source_commit": BASE, "dependency_lock_sha256": lock_sha}))
        original = read(checkout / OUTPUT)
        write_once(directory / "previous.json", original)
        target = load(read(checkout / TARGET))
        def step(name, args, deadline):
            nonlocal uncertain
            seconds = deadline - time.monotonic()
            need(seconds > 0, "phase deadline")
            try:
                with (directory / (name + ".log")).open("xb") as log:
                    result = subprocess.run([sys.executable, *args], cwd=checkout, env=env, stdout=log, stderr=subprocess.STDOUT, timeout=seconds, check=False)
                    log.flush(); os.fsync(log.fileno())
            except (subprocess.TimeoutExpired, OSError):
                uncertain = True
                raise
            if result.returncode < 0:
                uncertain = True
            need(result.returncode == 0, "STEP_FAILED:" + name)
            return read(directory / (name + ".log")).decode("utf-8", errors="replace")
        deadline = time.monotonic() + 360
        step("compile", ["-m", "py_compile", SCRIPT], deadline)
        log = step("contracts", ["-m", "unittest", "discover", "-s", "tests", "-p", "test_gaming_laptop_collector.py", "-v"], deadline)
        need(re.search(r"Ran [1-9][0-9]* tests?\b", log) and re.search(r"^OK[ \t]*\r?$", log, re.M) and not re.search(r"\b(?:skipped|expected failures|unexpected successes)=", log), "contracts did not all pass")
        clean()
        receipt = {"schema": "dolzore-laptop-one-shot/v1", "run_id": run_id, "event": event, "source_commit": BASE, "previous_sha256": digest(original), "collection_executed": collect, "publication_authorized": False, "remote_write": False, "runtime_acceptance": "NOT_ESTABLISHED"}
        if collect:
            output = directory / "candidate.json"
            need(not output.exists(), "candidate already exists")
            step("collect", [SCRIPT, "--target", TARGET, "--output", str(output)], time.monotonic() + 1080)
            data = read(output)
            receipt.update(state="LOCAL_CANDIDATE_PREPARED", candidate_sha256=digest(data), validation=validate(load(data), target, datetime.now(timezone.utc)))
            need(read(output) == data, "candidate drift")
        else:
            receipt.update(state="CI_ONLY_PASS")
        clean()
        need(read(checkout / OUTPUT) == original, "published bytes changed")
        write_once(directory / "receipt.json", dump(receipt))
        return receipt
    except Exception as exc:
        if directory is not None:
            try:
                write_once(directory / "failure.json", dump({"state": "FAILED", "error_type": type(exc).__name__, "lock_retained": uncertain, "publication_authorized": False}))
            except OSError:
                pass
        raise
    finally:
        if not uncertain:
            lock.rmdir()


class Tests(unittest.TestCase):
    def setUp(self):
        self.now = datetime(2026, 11, 26, tzinfo=timezone.utc)
        self.target = {"targetId": "used-gaming-laptop-14-rtx4060", "condition": "used", "policy": {"neverAutoBuy": True}, "sellerMarkets": ["fixture"], "models": ["fixture"], "gpu": {"acceptedLaptopGpuModels": ["4060"]}, "memoryGB": {"min": 16}, "ssdGB": {"min": 512}, "screenInches": {"min": 13.5, "max": 14.5}, "maxWeightKg": 1.7, "priceJPY": {"min": 80000, "max": 130000}}
        row = {"url": "https://example.invalid/item/1", "market": "fixture", "model": "fixture", "inventory": "unconfirmed_at_checkout", "condition": "used_listing_unverified", "priceJPY": 100000, "gpu": "4060", "memoryGB": 16, "ssdGB": 512, "screenInches": 14, "weightKg": 1.5, "decision": "CONSIDER"}
        self.report = {"schemaVersion": 1, "targetId": self.target["targetId"], "target": self.target, "generatedAt": self.now.isoformat(), "verifiedBuyCount": 0, "runId": None, "runUrl": None, "observedCount": 1, "qualifiedForConsiderCount": 1, "detailPagesInspected": 2, "rejections": {"sold": 1}, "status": "completed_market_observation", "results": [row]}
    def check(self):
        return validate(self.report, self.target, self.now)
    def test_consider(self):
        self.assertEqual(self.check()["qualified"], 1)
    def test_hold(self):
        self.report["results"][0].update(weightKg=None, decision="HOLD"); self.report["qualifiedForConsiderCount"] = 0
        self.assertEqual(self.check()["qualified"], 0)
    def test_wait(self):
        self.report["results"][0].update(priceJPY=150000, decision="WAIT"); self.report["qualifiedForConsiderCount"] = 0
        self.check()
    def test_empty(self):
        self.report.update(observedCount=0, qualifiedForConsiderCount=0, results=[], rejections={"sold": 2}, status="scanned_no_matching_individual_listings")
        self.assertEqual(self.check()["observed"], 0)
    def test_buy(self):
        self.report["results"][0]["decision"] = "BUY"
        with self.assertRaises(ValueError): self.check()
    def test_unknown_as_consider(self):
        self.report["results"][0]["weightKg"] = None
        with self.assertRaises(ValueError): self.check()
    def test_wrong_hardware(self):
        self.report["results"][0]["memoryGB"] = 8
        with self.assertRaises(ValueError): self.check()
    def test_wrong_count(self):
        self.report["detailPagesInspected"] = 3
        with self.assertRaises(ValueError): self.check()
    def test_stale(self):
        self.report["generatedAt"] = (self.now - timedelta(seconds=900)).isoformat()
        with self.assertRaises(ValueError): self.check()
    def test_future(self):
        self.report["generatedAt"] = (self.now + timedelta(seconds=121)).isoformat()
        with self.assertRaises(ValueError): self.check()
    def test_false_actions_id(self):
        self.report["runId"] = "123"
        with self.assertRaises(ValueError): self.check()
    def test_unsafe_url(self):
        self.report["results"][0]["url"] = "https://user:pass@example.invalid/item"
        with self.assertRaises(ValueError): self.check()
    def test_no_recurring_or_pr_collection(self):
        self.assertFalse(plan("pull_request")); self.assertTrue(plan("workflow_dispatch"))
        with self.assertRaises(ValueError): plan("schedule")
    def test_real_exclusive_write(self):
        with tempfile.TemporaryDirectory() as root:
            path = Path(root) / "receipt.json"
            write_once(path, b"original")
            with self.assertRaises(FileExistsError): write_once(path, b"replacement")
            self.assertEqual(read(path), b"original")
    def test_duplicate_json(self):
        with self.assertRaises(ValueError): load(b'{"x":1,"x":2}')


def main():
    p = argparse.ArgumentParser(description=__doc__)
    p.add_argument("--selftest", action="store_true")
    p.add_argument("--approved-isolated", action="store_true")
    p.add_argument("--approved-egress", action="store_true")
    for name in ("checkout", "evidence", "dependency-lock", "dependency-lock-sha256", "run-id", "event"):
        p.add_argument("--" + name)
    a = p.parse_args()
    if a.selftest:
        return 0 if unittest.TextTestRunner(verbosity=2).run(unittest.defaultTestLoader.loadTestsFromTestCase(Tests)).wasSuccessful() else 1
    if not a.approved_isolated or not all((a.checkout, a.evidence, a.dependency_lock, a.dependency_lock_sha256, a.run_id, a.event)):
        p.error("approved isolated checkout and all inputs required")
    try:
        print(json.dumps(run(a.checkout, a.evidence, a.dependency_lock, a.dependency_lock_sha256, a.run_id, a.event, a.approved_egress), sort_keys=True))
    except (ValueError, OSError, KeyError, TypeError, subprocess.SubprocessError, importlib.metadata.PackageNotFoundError):
        print("DOLZORE_LAPTOP_REJECTED; retain evidence for HQ", file=sys.stderr)
        return 2
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
