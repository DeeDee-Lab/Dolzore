import hashlib
import json
import subprocess

import pytest
from scripts import render_pixel_assets_local as carrier


@pytest.fixture
def checkout(tmp_path, monkeypatch):
    root = tmp_path / "source"
    scripts = root / "scripts"
    scripts.mkdir(parents=True)
    render = ("from PIL import Image\nfrom pathlib import Path\n"
              "out=Path('docs/assets');out.mkdir(parents=True)\n"
              f"for name,size in {carrier.ASSETS!r}.items():\n"
              "    Image.new('RGB',size).save(out/name)\n")
    (scripts / "render_pixel_assets.py").write_text(render)
    (scripts / "verify_pixel_assets.py").write_text("assert True\n")
    monkeypatch.setattr(carrier, "SOURCE", {p.name: carrier.blob(p.read_bytes()) for p in scripts.iterdir()})
    subprocess.run(["git", "init", "-q", str(root)], check=True)
    subprocess.run(["git", "-C", str(root), "add", "."], check=True)
    subprocess.run(["git", "-C", str(root), "-c", "user.name=Test", "-c", "user.email=test@example.invalid", "commit", "-qm", "fixture"], check=True)
    sha = subprocess.check_output(["git", "-C", str(root), "rev-parse", "HEAD"], text=True).strip()
    return root, sha, tmp_path / "evidence"


def test_render_readback(checkout):
    root, sha, evidence = checkout
    assert carrier.execute(*checkout) == 0
    assert not (root / "docs").exists()
    manifest = json.loads((evidence / "manifest.json").read_text())
    assert manifest["checkout_sha"] == sha
    assert set(manifest["assets"]) == set(carrier.ASSETS)
    for name, item in manifest["assets"].items():
        assert hashlib.sha256((evidence / name).read_bytes()).hexdigest() == item["sha256"]


def test_unreviewed_script(checkout, monkeypatch):
    monkeypatch.setattr(carrier, "SOURCE", {"render_pixel_assets.py": "0" * 40})
    with pytest.raises(ValueError, match="unreviewed script"):
        carrier.execute(*checkout)
    assert not checkout[-1].exists()


def test_wrong_sha(checkout):
    root, _, evidence = checkout
    with pytest.raises(ValueError, match="checkout mismatch"):
        carrier.execute(root, "0" * 40, evidence)


def test_dirty(checkout):
    (checkout[0] / "dirty").touch()
    with pytest.raises(ValueError, match="not clean"):
        carrier.execute(*checkout)


def test_invalid_asset_dimensions(checkout, monkeypatch):
    monkeypatch.setattr(carrier, "ASSETS", {**carrier.ASSETS, "dolzore-town.png": (1, 1)})
    with pytest.raises(ValueError, match="invalid asset"):
        carrier.execute(*checkout)
    assert not checkout[-1].exists()
