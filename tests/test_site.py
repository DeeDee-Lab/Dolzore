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

    def test_support_and_legal_surfaces_are_github_native(self):
        support=(DOCS/"support/index.html").read_text(encoding="utf-8")
        legal=(DOCS/"legal/index.html").read_text(encoding="utf-8")
        self.assertIn("Creator BGM",support)
        self.assertIn("instagram.com/dolzoreofficial",support)
        for route in ("privacy","terms","refund","tokusho","disclosure"):
            self.assertIn(f"../{route}/",legal)
            self.assertTrue((DOCS/route/"index.html").exists(),route)
        for p in (DOCS/"legal/index.html", DOCS/"privacy/index.html", DOCS/"terms/index.html", DOCS/"refund/index.html", DOCS/"tokusho/index.html", DOCS/"disclosure/index.html"):
            text=p.read_text(encoding="utf-8",errors="ignore")
            self.assertNotIn("dolzore.lovable.app",text,p)
            self.assertNotIn("Business Packs",text,p)
            self.assertNotIn("SmartBuy",text,p)
            self.assertNotIn("AI検収",text,p)

    def test_music_only_legal_truth(self):
        terms=(DOCS/"terms/index.html").read_text(encoding="utf-8")
        refund=(DOCS/"refund/index.html").read_text(encoding="utf-8")
        tokusho=(DOCS/"tokusho/index.html").read_text(encoding="utf-8")
        disclosure=(DOCS/"disclosure/index.html").read_text(encoding="utf-8")
        self.assertIn("現在一般販売している商品は、DOLZORE Creator BGM",terms)
        self.assertIn("DOLZORE Creator BGM",refund)
        self.assertIn("1曲 200円",tokusho)
        self.assertIn("現在の一般販売",disclosure)


    def test_all_public_html_excludes_stopped_sales_navigation(self):
        blocked=("Buying Guide","SmartBuy","Business Packs","/business","/apps","QA-001","QA-002","QA-003","QA-004")
        findings=[]
        for p in DOCS.rglob("*.html"):
            text=p.read_text(encoding="utf-8",errors="ignore")
            for marker in blocked:
                if marker.lower() in text.lower():
                    findings.append((str(p.relative_to(DOCS)),marker))
        self.assertEqual(findings,[],f"stopped public sales surface remains: {findings}")

    def test_sitemap_excludes_stopped_non_music_lanes(self):
        sitemap=(DOCS/"sitemap.xml").read_text(encoding="utf-8")
        for blocked in ("business","apps","buying-guide","smartbuy","qa"):
            self.assertNotIn(blocked,sitemap.lower())
        for required in ("/music/","/journal/","/support/","/legal/","/privacy/","/terms/","/refund/","/tokusho/","/disclosure/"):
            self.assertIn(required,sitemap)

if __name__=="__main__":
    unittest.main()
