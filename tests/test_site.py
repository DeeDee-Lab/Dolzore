from __future__ import annotations
import json
from pathlib import Path
import unittest

ROOT=Path(__file__).resolve().parents[1]
DOCS=ROOT/"docs"

class MusicOnlySiteTests(unittest.TestCase):
    def test_track_catalog_is_exactly_60_and_music_only(self):
        data=json.loads((DOCS/"data/tracks.json").read_text(encoding="utf-8"))
        self.assertEqual(len(data["tracks"]),60)
        self.assertEqual(data["policy"]["saleScope"],"music-only")
        self.assertTrue(data["policy"]["sampleOnlyPublic"])
        self.assertFalse(data["policy"]["fullAudioPublic"])
        ids=[x["id"] for x in data["tracks"]]
        self.assertEqual(ids[0],"BGM-001")
        self.assertEqual(ids[-1],"BGM-060")
        self.assertEqual(len(set(ids)),60)

    def test_every_track_has_exact_purchase_and_sample_contract(self):
        data=json.loads((DOCS/"data/tracks.json").read_text(encoding="utf-8"))
        purchase_urls=[]
        ready=0
        for t in data["tracks"]:
            self.assertTrue(t["purchaseUrl"].startswith("https://buy.stripe.com/"))
            purchase_urls.append(t["purchaseUrl"])
            self.assertEqual(t["priceYen"],200)
            self.assertEqual(t["sampleSeconds"],20)
            self.assertRegex(t["sampleFile"],r"^bgm-\d{3}\.mp3$")
            if t.get("previewReady"):
                ready += 1
                self.assertTrue(str(t.get("previewUrl","")).startswith("https://"))
            else:
                self.assertFalse(t.get("previewUrl"))
        self.assertEqual(len(set(purchase_urls)),60)
        self.assertGreaterEqual(ready,6)

    def test_public_pages_do_not_reference_full_audio_source(self):
        for p in DOCS.rglob("*"):
            if not p.is_file() or p.suffix.lower() not in {".html",".js",".css",".json",".xml",".txt"}:
                continue
            text=p.read_text(encoding="utf-8",errors="ignore")
            self.assertNotIn("downloads"+"/"+"bgm",text,p)

    def test_home_and_music_surface_are_music_only(self):
        home=(DOCS/"index.html").read_text(encoding="utf-8")
        music=(DOCS/"music/index.html").read_text(encoding="utf-8")
        self.assertIn("いま売っているのは、音楽だけです",home)
        self.assertIn("data-jukebox",music)
        self.assertNotIn("Business",home)
        self.assertNotIn("Buying Guide",home)
        self.assertNotIn("SmartBuy",home)

    def test_support_and_legal_surfaces_preserve_current_routes(self):
        support=(DOCS/"support/index.html").read_text(encoding="utf-8")
        legal=(DOCS/"legal/index.html").read_text(encoding="utf-8")
        self.assertIn("Creator BGM",support)
        self.assertIn("instagram.com/dolzoreofficial",support)
        for route in ("privacy","terms","refund","tokusho"):
            self.assertIn(f"https://dolzore.lovable.app/{route}",legal)
        self.assertNotIn("Business Packs",legal)
        self.assertNotIn("SmartBuy",legal)

    def test_sitemap_excludes_stopped_non_music_lanes(self):
        sitemap=(DOCS/"sitemap.xml").read_text(encoding="utf-8")
        for blocked in ("business","apps","buying-guide","smartbuy","qa"):
            self.assertNotIn(blocked,sitemap.lower())
        for required in ("/music/","/journal/","/support/","/legal/"):
            self.assertIn(required,sitemap)

if __name__=="__main__":
    unittest.main()
