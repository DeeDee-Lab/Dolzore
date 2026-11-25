"""Isolated render/verify carrier; never publishes or mutates the checkout."""
from __future__ import annotations

import argparse
import hashlib
import json
import subprocess
import sys
import tempfile
import time
from pathlib import Path

WORKFLOW_BLOB = "4b753758e873f12b4528158ba21c38102422d5b5"
SOURCE = {"render_pixel_assets.py": "699fed0ee3d9f712817548a5944f5543e7529d9f",
          "verify_pixel_assets.py": "85844452b392b386619bd61de5715d8bef57837d"}
ASSETS = {"dolzore-town.png": (960, 540), "dolzore-jukebox.png": (480, 720),
          "dolzore-town-game.png": (480, 270), "dolzore-characters.png": (256, 160),
          "dolzore-first-town.png": (1536, 1152)}


def blob(data: bytes) -> str:
    return hashlib.sha1(f"blob {len(data)}\0".encode() + data).hexdigest()


def execute(checkout: Path, approved_sha: str, evidence: Path) -> int:
    from PIL import Image

    checkout = checkout.resolve(strict=True)
    evidence = evidence.resolve()
    deadline = time.monotonic() + 900

    def git(*args: str) -> str:
        return subprocess.check_output(["git", "-C", str(checkout), *args], text=True, timeout=30).strip()

    if Path(git("rev-parse", "--show-toplevel")).resolve() != checkout or git("rev-parse", "HEAD") != approved_sha:
        raise ValueError("approved checkout mismatch")
    if git("status", "--porcelain", "--untracked-files=all"):
        raise ValueError("checkout is not clean")
    if evidence.is_relative_to(checkout):
        raise ValueError("evidence must be outside checkout")
    with tempfile.TemporaryDirectory(prefix="dolzore-render-") as temp:
        root = Path(temp)
        scripts = root / "scripts"
        scripts.mkdir()
        for name, expected in SOURCE.items():
            source = checkout / "scripts" / name
            if source.is_symlink() or not source.resolve().is_relative_to(checkout):
                raise ValueError("unsafe script path")
            data = source.read_bytes()
            if blob(data) != expected:
                raise ValueError(f"unreviewed script: {name}")
            (scripts / name).write_bytes(data)
        for name in SOURCE:
            remaining = deadline - time.monotonic()
            if remaining <= 0:
                raise TimeoutError("render deadline exceeded")
            subprocess.run([sys.executable, str(scripts / name)], cwd=root, check=True, timeout=remaining)
        assets = root / "docs" / "assets"
        if not assets.is_dir() or set(p.name for p in assets.iterdir()) != set(ASSETS):
            raise ValueError("unexpected output set")
        payloads = {}
        for name, size in ASSETS.items():
            path = assets / name
            if path.is_symlink() or not path.is_file():
                raise ValueError(f"unsafe asset: {name}")
            with Image.open(path) as image:
                if image.format != "PNG" or image.size != size:
                    raise ValueError(f"invalid asset: {name}")
                image.verify()
            payloads[name] = path.read_bytes()
        evidence.mkdir(parents=True, exist_ok=False)
        manifest = {"checkout_sha": approved_sha, "workflow_blob": WORKFLOW_BLOB,
                    "source_blobs": SOURCE, "assets": {}}
        for name, data in payloads.items():
            (evidence / name).write_bytes(data)
            if (evidence / name).read_bytes() != data:
                raise OSError(f"readback failed: {name}")
            manifest["assets"][name] = {"sha256": hashlib.sha256(data).hexdigest(), "bytes": len(data)}
        (evidence / "manifest.json").write_text(json.dumps(manifest, indent=2) + "\n", encoding="utf-8")
    return 0


def main() -> int:
    p = argparse.ArgumentParser(description=__doc__)
    p.add_argument("--checkout", required=True, type=Path)
    p.add_argument("--approved-sha", required=True)
    p.add_argument("--evidence-dir", required=True, type=Path)
    a = p.parse_args()
    try:
        return execute(a.checkout, a.approved_sha, a.evidence_dir)
    except (OSError, ValueError, ImportError, subprocess.SubprocessError) as exc:
        print(f"LOCAL_RENDER_FAILED: {exc}", file=sys.stderr)
        return 1


if __name__ == "__main__":
    raise SystemExit(main())
