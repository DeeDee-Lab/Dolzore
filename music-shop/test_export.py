from __future__ import annotations

import json
import tempfile
from pathlib import Path
import unittest

ROOT = Path(__file__).resolve().parents[1]
SRC = ROOT / "docs"
OUT = ROOT / "music-shop" / "dist"


class ExistingMusicShopExportTest(unittest.TestCase):
    @classmethod
    def setUpClass(cls) -> None:
        import sys
        sys.path.insert(0, str(ROOT / "music-shop"))
        from build import build
        build()

    def test_existing_sources_are_reused_and_not_replaced(self):
        src = (SRC / "music/index.html").read_text(encoding="utf-8")
        exported = (OUT / "music/index.html").read_text(encoding="utf-8")
        self.assertIn("data-jukebox", src)
        self.assertIn("data-jukebox", exported)
        self.assertIn("dolzore-jukebox.png", exported)
        self.assertIn("DOLZORE", exported)

    def test_sixty_canonical_purchase_rows_are_preserved(self):
        original = json.loads((SRC / "data/tracks.json").read_text(encoding="utf-8"))
        exported = json.loads((OUT / "data/tracks.json").read_text(encoding="utf-8"))
        self.assertEqual(original, exported)
        self.assertEqual(len(exported["tracks"]), 60)
        self.assertEqual({x["priceYen"] for x in exported["tracks"]}, {200})
        self.assertEqual(sum(bool(x.get("previewReady")) for x in exported["tracks"]), 6)

    def test_original_stripe_purchase_links_are_enabled(self):
        music = (OUT / "music/index.html").read_text(encoding="utf-8")
        script = (OUT / "site.js").read_text(encoding="utf-8")
        catalog = json.loads((OUT / "data/tracks.json").read_text(encoding="utf-8"))
        self.assertIn('data-purchase href="#"', music)
        self.assertIn("BUY THIS TRACK", music)
        self.assertIn("purchase.href=track.purchaseUrl;", script)
        self.assertNotIn('aria-disabled="true"', music)
        self.assertNotIn("販売再開準備中", music)
        self.assertEqual(len({t["purchaseUrl"] for t in catalog["tracks"]}), 60)
        self.assertTrue(all(t["purchaseUrl"].startswith("https://buy.stripe.com/")
                            for t in catalog["tracks"]))

    def test_only_whitelisted_site_content_and_no_full_audio(self):
        self.assertFalse((OUT / "town-game.js").exists())
        self.assertFalse((OUT / "journal").exists())
        self.assertFalse((OUT / "business").exists())
        self.assertFalse((OUT / "apps").exists())
        self.assertFalse((OUT / "about").exists())
        self.assertTrue((OUT / "legal/index.html").exists())
        self.assertTrue((OUT / "tokusho/index.html").exists())
        self.assertFalse(any(
            f.suffix.lower() in {".mp3", ".flac", ".wav", ".m4a"}
            for f in OUT.rglob("*") if f.is_file()
        ))

    def test_legacy_closure_and_search_controls_are_preserved(self):
        self.assertIn("Closed", (SRC / "index.html").read_text(encoding="utf-8"))
        self.assertIn("index,follow", (OUT / "music/index.html").read_text(encoding="utf-8"))
        self.assertIn("Allow: /", (OUT / "robots.txt").read_text(encoding="utf-8"))
        for p in OUT.rglob("*.html"):
            self.assertNotIn('href="../journal/"', p.read_text(encoding="utf-8"))

    def test_null_disc_no_longer_crashes_preview_audio(self):
        js = (OUT / "site.js").read_text(encoding="utf-8")
        self.assertIn("disc?.classList", js)
        self.assertNotIn("disc.classList", js)


if __name__ == "__main__":
    unittest.main()
