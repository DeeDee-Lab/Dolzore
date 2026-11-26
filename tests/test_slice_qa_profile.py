from pathlib import Path
import tempfile
import unittest

from scripts import first_town_qa_local as carrier


class SliceProfileTests(unittest.TestCase):
    def test_original_output_directory_and_explicit_pins(self):
        spec = carrier.get_profile("slice1")
        self.assertEqual(spec["output"], "qa-slice1")
        self.assertEqual(spec["qa"], "tests/game_slice_qa.py")
        self.assertEqual(spec["pins"], {
            ".github/workflows/slice-qa-cloud.yml": "34075da936902ca9b4b7a61b71cdff5a73b95356",
            "tests/game_slice_qa.py": "c6055ad07847637d3029e674deba6fd136a9dc3f"})
        self.assertEqual(carrier.get_profile("first-town")["pins"], carrier.PINS)

    def test_exact_slice_screenshots_and_marker_set(self):
        spec = carrier.get_profile("slice1")
        with tempfile.TemporaryDirectory() as tmp:
            folder = Path(tmp)
            for name in spec["shots"]:
                (folder / name).write_bytes(b"\x89PNG\r\n\x1a\nfixture")
            markers = "\n".join(sorted(spec["markers"]))
            result = carrier.screenshot_receipt(folder, markers, shots=spec["shots"], markers=spec["markers"])
            self.assertEqual(set(result), {"desktop-slice1.png", "mobile-slice1.png"})
            with self.assertRaisesRegex(ValueError, "MARKERS"):
                carrier.screenshot_receipt(folder, markers)
            (folder / "mobile-slice1.png").unlink()
            with self.assertRaisesRegex(ValueError, "SET_MISMATCH"):
                carrier.screenshot_receipt(folder, markers, shots=spec["shots"], markers=spec["markers"])

    def test_unknown_profile_cannot_supply_executable(self):
        for name in (None, "../other.py", "visual-qa", ""):
            with self.assertRaisesRegex(ValueError, "UNKNOWN_QA_PROFILE"):
                carrier.get_profile(name)

    def test_slice_still_requires_approved_runtime(self):
        with self.assertRaisesRegex(ValueError, "APPROVED_OSK222"):
            carrier.run("missing", "0" * 40, "missing", "missing", "missing", profile="slice1")
