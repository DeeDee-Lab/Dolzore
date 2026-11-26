"""Stage independently acquired Dolzore writer output; never collect or publish.

Both profiles must use the same private staging root. The local lock protects
staging only; it is not a distributed collector or production writer lock.
"""
from __future__ import annotations
import argparse
import hashlib
import json
import os
import re
import stat
import tempfile
import unittest
from datetime import datetime, timezone, timedelta
from pathlib import Path
from urllib.parse import urlsplit

PINS = {
    "market": {"workflow": "500b3e5477516792405bfb7044842402769994ed", "source": "27365acd158d598bbbdb6f1fed64051486a8ebcd", "path": "docs/data/smartbuy-projectors.json"},
    "media": {"workflow": "bf4b616b9038f88a42eb1998a31382aa2f2d4311", "source": "75efdcd3e2e827f9a4524679f80cef58fbfda94e", "path": "docs/data/product-media.json"},
}
BASE = "1db115626e657eab5d171a34fd09bf4bbdfc6034"
LOCK = ".dolzore-data-writers.lock"
LIMIT = 16 * 1024 * 1024


def sha(data):
    return hashlib.sha256(data).hexdigest()


def pairs(items):
    result = {}
    for k, v in items:
        if k in result:
            raise ValueError("duplicate JSON key")
        result[k] = v
    return result


def reject_constant(_):
    raise ValueError("nonfinite JSON")


def load(data):
    result = json.loads(data, object_pairs_hook=pairs, parse_constant=reject_constant)
    if not isinstance(result, dict):
        raise ValueError("JSON object required")
    return result


def aware(value):
    stamp = datetime.fromisoformat(value)
    if stamp.tzinfo is None or stamp.utcoffset() is None:
        raise ValueError("timezone-aware timestamp required")
    return stamp


def https(value):
    if not isinstance(value, str):
        return False
    u = urlsplit(value)
    return u.scheme == "https" and bool(u.hostname) and u.username is None and u.password is None


def validate(profile, old, new, now):
    a, b = load(old), load(new)
    if profile == "market":
        age = (now - aware(b["generatedAt"])).total_seconds()
        if not -120 <= age <= 3600 or type(b.get("schemaVersion")) is not int or b["schemaVersion"] != 3:
            raise ValueError("market freshness or schema")
        models = b.get("trackedModels")
        if not isinstance(models, list) or not models or any(not isinstance(x, str) or not x for x in models):
            raise ValueError("market tracked models")
        for key in ("used", "observedUsed", "newItems", "modelSummaries"):
            if not isinstance(b.get(key), list):
                raise ValueError("market list: " + key)
        if not b["observedUsed"] and not b["newItems"]:
            raise ValueError("no observed listings")
    elif profile == "media":
        if a.get("schemaVersion") != 1 or b.get("schemaVersion") != 1:
            raise ValueError("media schema")
        ap, bp = a.get("products"), b.get("products")
        if not isinstance(ap, list) or not isinstance(bp, list) or len(ap) != len(bp) or not ap:
            raise ValueError("media product set")
        if {k:v for k,v in a.items() if k not in ("updatedAt", "products")} != {k:v for k,v in b.items() if k not in ("updatedAt", "products")}:
            raise ValueError("media metadata changed")
        changed = False
        for x, y in zip(ap, bp):
            if not isinstance(x, dict) or not isinstance(y, dict) or {k:v for k,v in x.items() if k != "imageUrl"} != {k:v for k,v in y.items() if k != "imageUrl"}:
                raise ValueError("media identity changed")
            if x.get("imageUrl") != y.get("imageUrl"):
                if not https(y.get("imageUrl")):
                    raise ValueError("media URL not HTTPS")
                changed = True
        if not changed and old != new:
            raise ValueError("media no-change must preserve bytes")
        if changed and not -120 <= (now - aware(b["updatedAt"])).total_seconds() <= 3600:
            raise ValueError("media update timestamp")
    else:
        raise ValueError("unknown profile")


def read(path):
    if not stat.S_ISREG(path.lstat().st_mode):
        raise ValueError("input not regular file")
    flags = os.O_RDONLY | getattr(os, "O_NOFOLLOW", 0) | getattr(os, "O_BINARY", 0) | getattr(os, "O_NONBLOCK", 0)
    with os.fdopen(os.open(path, flags), "rb") as f:
        info = os.fstat(f.fileno())
        if not stat.S_ISREG(info.st_mode) or info.st_size > LIMIT:
            raise ValueError("input size or type")
        data = f.read(LIMIT + 1)
    if len(data) > LIMIT:
        raise ValueError("input too large")
    return data


def write_once(path, data):
    with path.open("xb") as f:
        f.write(data); f.flush(); os.fsync(f.fileno())
    if read(path) != data:
        raise ValueError("stage readback mismatch")


def stage(profile, root, original, candidate, run_id, expected_original, expected_candidate, now):
    root = Path(root).resolve(strict=True)
    original, candidate = Path(original).absolute(), Path(candidate).absolute()
    if profile not in PINS or not root.is_dir() or not re.fullmatch(r"[A-Za-z0-9_-]{1,80}", run_id):
        raise ValueError("invalid stage identity")
    if any(p.resolve().is_relative_to(root) for p in (original, candidate)):
        raise ValueError("stage root must be separate from inputs")
    lock = root / LOCK
    lock.mkdir()  # exclusive for both profiles; never delete a pre-existing lock
    try:
        old, new = read(original), read(candidate)
        if sha(old) != expected_original or sha(new) != expected_candidate:
            raise ValueError("independent input hash mismatch")
        validate(profile, old, new, now)
        out = root / run_id
        out.mkdir()  # immutable run identity; partial run requires HQ review
        write_once(out / "candidate.json", new)
        receipt = {"schema": "dolzore-shared-writer-stage/v1", "profile": profile, "run_id": run_id, "source_commit": BASE, "source_pins": PINS[profile], "original_sha256": sha(old), "candidate_sha256": sha(new), "bytes_changed": old != new, "stage_status": "STAGED", "collection_executed": False, "source_provenance_verified": False, "production_writer_authorized": False, "publication_authorized": False, "runtime_acceptance": "NOT_ESTABLISHED"}
        write_once(out / "receipt.json", (json.dumps(receipt, sort_keys=True) + "\n").encode())
        return receipt
    finally:
        lock.rmdir()


class StageTests(unittest.TestCase):
    def setUp(self):
        tmp = tempfile.TemporaryDirectory(); self.addCleanup(tmp.cleanup)
        base = Path(tmp.name); self.root = base / "stage"; self.root.mkdir()
        self.old, self.new = base / "old.json", base / "new.json"
        self.now = datetime(2026, 11, 26, tzinfo=timezone.utc)
        self.market = {"schemaVersion": 3, "generatedAt": self.now.isoformat(), "trackedModels": ["fixture"], "used": [], "observedUsed": [{}], "newItems": [], "modelSummaries": []}
        self.media = {"schemaVersion": 1, "updatedAt": self.now.isoformat(), "policy": "fixture", "products": [{"model": "fixture", "sourceUrl": "https://example.invalid/product", "imageUrl": "https://example.invalid/old"}]}
    def dump(self, x): return json.dumps(x, sort_keys=True).encode()
    def go(self, profile, old, new, run="run1", digest=None):
        self.old.write_bytes(old); self.new.write_bytes(new)
        return stage(profile, self.root, self.old, self.new, run, sha(old), digest or sha(new), self.now)
    def test_market_stage(self):
        data = self.dump(self.market); r = self.go("market", data, data)
        self.assertEqual((self.root / "run1/candidate.json").read_bytes(), data)
        self.assertFalse(r["publication_authorized"]); self.assertEqual(load((self.root / "run1/receipt.json").read_bytes()), r)
        self.assertFalse((self.root / LOCK).exists()); self.assertEqual(self.old.read_bytes(), data)
    def test_cross_profile_lock(self):
        (self.root / LOCK).mkdir(); data = self.dump(self.media)
        for p in ("market", "media"):
            with self.assertRaises(FileExistsError): self.go(p, data, data)
        self.assertTrue((self.root / LOCK).is_dir())
    def test_duplicate_run(self):
        data = self.dump(self.market); self.go("market", data, data)
        with self.assertRaises(FileExistsError): self.go("market", data, data)
    def test_market_stale(self):
        old = self.dump(self.market); self.market["generatedAt"] = (self.now - timedelta(seconds=3601)).isoformat()
        with self.assertRaises(ValueError): self.go("market", old, self.dump(self.market))
    def test_market_empty(self):
        self.market["observedUsed"] = []; data = self.dump(self.market)
        with self.assertRaises(ValueError): self.go("market", data, data)
    def test_wrong_hash(self):
        data = self.dump(self.market)
        with self.assertRaises(ValueError): self.go("market", data, data, digest="0" * 64)
    def test_media_change(self):
        old = self.dump(self.media); self.media["products"][0]["imageUrl"] = "https://example.invalid/new"
        self.assertTrue(self.go("media", old, self.dump(self.media))["bytes_changed"])
    def test_media_identity(self):
        old = self.dump(self.media); self.media["products"][0]["sourceUrl"] = "https://example.invalid/other"
        with self.assertRaises(ValueError): self.go("media", old, self.dump(self.media))
    def test_media_bad_url(self):
        old = self.dump(self.media); self.media["products"][0]["imageUrl"] = "http://example.invalid/new"
        with self.assertRaises(ValueError): self.go("media", old, self.dump(self.media))
    def test_media_nochange(self):
        data = self.dump(self.media); self.assertFalse(self.go("media", data, data)["bytes_changed"])
    def test_media_timestamp_only(self):
        old = self.dump(self.media); self.media["updatedAt"] = (self.now + timedelta(seconds=1)).isoformat()
        with self.assertRaises(ValueError): self.go("media", old, self.dump(self.media))
    def test_duplicate_key(self):
        with self.assertRaises(ValueError): load(b'{"x":1,"x":2}')


def main():
    p = argparse.ArgumentParser(description=__doc__)
    p.add_argument("--selftest", action="store_true")
    for name in ("profile", "root", "original", "candidate", "run-id", "expected-original", "expected-candidate"):
        p.add_argument("--" + name)
    a = p.parse_args()
    if a.selftest:
        return 0 if unittest.TextTestRunner(verbosity=2).run(unittest.defaultTestLoader.loadTestsFromTestCase(StageTests)).wasSuccessful() else 1
    if not all((a.profile, a.root, a.original, a.candidate, a.run_id, a.expected_original, a.expected_candidate)):
        p.error("stage requires all inputs and independent digests")
    try:
        print(json.dumps(stage(a.profile, a.root, a.original, a.candidate, a.run_id, a.expected_original, a.expected_candidate, datetime.now(timezone.utc)), sort_keys=True))
    except (OSError, ValueError, KeyError, TypeError, RecursionError):
        print("DOLZORE_STAGE_REJECTED; preserve any partial stage for HQ")
        return 2
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
