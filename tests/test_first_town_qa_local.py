import os
from pathlib import Path
import tempfile
import unittest
from unittest.mock import patch
from urllib.request import urlopen

from scripts import first_town_qa_local as carrier


class CarrierTests(unittest.TestCase):
    def test_actual_loopback_server_and_collision(self):
        with tempfile.TemporaryDirectory() as tmp:
            docs = Path(tmp).resolve()
            (docs / "index.html").write_text("fixture", encoding="utf-8")
            with carrier.local_site(docs, port=0) as port:
                self.assertEqual(urlopen(f"http://127.0.0.1:{port}/", timeout=3).read(), b"fixture")
                with self.assertRaises(OSError):
                    with carrier.local_site(docs, port=port):
                        self.fail("must not take over occupied port")
            with self.assertRaises(OSError):
                urlopen(f"http://127.0.0.1:{port}/", timeout=1)

    def test_exact_screenshot_set_and_markers(self):
        with tempfile.TemporaryDirectory() as tmp:
            folder = Path(tmp)
            for name in carrier.SHOTS:
                (folder / name).write_bytes(b"\x89PNG\r\n\x1a\nfixture")
            markers = "\n".join(sorted(carrier.MARKERS))
            receipt = carrier.screenshot_receipt(folder, markers)
            self.assertEqual(set(receipt), carrier.SHOTS)
            self.assertEqual(len(receipt), 9)
            with self.assertRaisesRegex(ValueError, "MARKERS"):
                carrier.screenshot_receipt(folder, "FIRST_TOWN_QA_PASS")
            (folder / "extra.png").touch()
            with self.assertRaisesRegex(ValueError, "SET_MISMATCH"):
                carrier.screenshot_receipt(folder, markers)
            (folder / "extra.png").unlink()
            (folder / "desktop-first-town.png").write_bytes(b"not a png")
            with self.assertRaisesRegex(ValueError, "INVALID_SCREENSHOT"):
                carrier.screenshot_receipt(folder, markers)

    def test_unapproved_execution_refuses_before_io(self):
        with self.assertRaisesRegex(ValueError, "APPROVED_OSK222"):
            carrier.run("missing", "0" * 40, "missing", "missing", "missing")

    def test_no_inherited_credentials_or_browser_override(self):
        with patch.dict(os.environ, {"GITHUB_TOKEN": "fixture", "PLAYWRIGHT_BROWSERS_PATH": "other", "GIT_ALLOW_PROTOCOL": "https"}):
            env = carrier.clean_env(Path("fixture-home"), Path("approved-browser"))
        self.assertNotIn("GITHUB_TOKEN", env)
        self.assertEqual(env["PLAYWRIGHT_BROWSERS_PATH"], "approved-browser")
        self.assertEqual(env["GIT_ALLOW_PROTOCOL"], "")
