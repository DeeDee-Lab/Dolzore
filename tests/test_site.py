import importlib.util
import pathlib
import unittest

ROOT = pathlib.Path(__file__).resolve().parents[1]
spec = importlib.util.spec_from_file_location("dolzore_runtime", ROOT / "src" / "runtime.py")
runtime = importlib.util.module_from_spec(spec)
spec.loader.exec_module(runtime)


class RecommendationGateTests(unittest.TestCase):
    def base(self):
        return {
            "target_category": "projector",
            "matched_priority_model": "BenQ TK700STi",
            "fit_status": "ELIGIBLE",
            "live_status": "live",
            "detail_verification_status": "verified",
            "price": 75000,
            "price_status": "strong_buy",
            "market": "yahoo_flea",
            "title": "BenQ TK700STi 4K プロジェクター 美品",
            "url": "https://paypayfleamarket.yahoo.co.jp/item/z123456789",
        }

    def test_valid_individual_listing_passes(self):
        self.assertTrue(runtime.eligible(self.base()))

    def test_search_page_is_rejected(self):
        row = self.base()
        row["url"] = "https://paypayfleamarket.yahoo.co.jp/search/BenQ%20TK700STi"
        self.assertFalse(runtime.eligible(row))

    def test_sold_listing_is_rejected(self):
        row = self.base()
        row["live_status"] = "sold"
        self.assertFalse(runtime.eligible(row))

    def test_accessory_is_rejected(self):
        row = self.base()
        row["title"] = "BenQ TK700STi 交換ランプ"
        self.assertFalse(runtime.eligible(row))

    def test_low_price_is_rejected(self):
        row = self.base()
        row["price"] = 500
        self.assertFalse(runtime.eligible(row))

    def test_unknown_price_status_is_rejected(self):
        row = self.base()
        row["price_status"] = "unknown"
        self.assertFalse(runtime.eligible(row))


class PublicSiteContractTests(unittest.TestCase):
    def test_static_pages_exist(self):
        expected = [
            ROOT / "docs" / "index.html",
            ROOT / "docs" / "buying-guide" / "index.html",
            ROOT / "docs" / "buying-guide" / "projectors" / "index.html",
            ROOT / "docs" / "buying-guide" / "projectors" / "tk700sti" / "index.html",
        ]
        for path in expected:
            self.assertTrue(path.exists(), str(path))

    def test_projector_page_has_ten_cards(self):
        text = (ROOT / "docs" / "buying-guide" / "projectors" / "index.html").read_text(encoding="utf-8")
        self.assertEqual(text.count('class="product-card"'), 10)
        self.assertIn("人気だけで決めない。", text)

    def test_tk700_page_has_required_sections(self):
        text = (ROOT / "docs" / "buying-guide" / "projectors" / "tk700sti" / "index.html").read_text(encoding="utf-8")
        for needle in (
            "中古なら、まだかなりアリ。",
            "こんな人向け",
            "こんな人は別候補",
            "150インチ設置イメージ",
            "中古は、値札より白画面を見る。",
            "SmartBuy",
            "実機レビューではなく",
        ):
            self.assertIn(needle, text)


if __name__ == "__main__":
    unittest.main()
