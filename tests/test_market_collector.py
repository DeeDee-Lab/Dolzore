import importlib.util
import pathlib
import unittest

from bs4 import BeautifulSoup

ROOT = pathlib.Path(__file__).resolve().parents[1]
spec = importlib.util.spec_from_file_location(
    "market_collector", ROOT / "scripts" / "update_market_snapshot.py"
)
collector = importlib.util.module_from_spec(spec)
spec.loader.exec_module(collector)


class TrustedPriceExtractionTests(unittest.TestCase):
    def test_amazon_buybox_beats_coupon_amount(self):
        html = """
        <html><body>
          <div>12,000円OFFクーポン</div>
          <div id="corePriceDisplay_desktop_feature_div">
            <span class="a-price"><span class="a-offscreen">￥209,800</span></span>
          </div>
        </body></html>
        """
        soup = BeautifulSoup(html, "html.parser")
        cfg = collector.MODELS["BenQ TK705STi"]
        price, source = collector.extract_trusted_price(
            "amazon", soup, soup.get_text(" ", strip=True), cfg
        )
        self.assertEqual(price, 209800)
        self.assertEqual(source, "amazon_buybox")

    def test_amazon_coupon_only_is_rejected(self):
        html = "<html><body><div>12,000円OFFクーポン</div></body></html>"
        soup = BeautifulSoup(html, "html.parser")
        cfg = collector.MODELS["BenQ TK705STi"]
        price, source = collector.extract_trusted_price(
            "amazon", soup, soup.get_text(" ", strip=True), cfg
        )
        self.assertIsNone(price)
        self.assertEqual(source, "unverified_amazon_price")

    def test_jsonld_offer_is_accepted_when_plausible(self):
        html = """
        <script type="application/ld+json">
        {"@type":"Product","offers":{"@type":"Offer","price":"197890"}}
        </script>
        """
        soup = BeautifulSoup(html, "html.parser")
        cfg = collector.MODELS["BenQ TK710STi"]
        price, source = collector.extract_trusted_price("amazon", soup, "", cfg)
        self.assertEqual(price, 197890)
        self.assertEqual(source, "jsonld_offer")

    def test_implausible_new_price_is_rejected_even_if_structured(self):
        html = """
        <script type="application/ld+json">
        {"@type":"Product","offers":{"@type":"Offer","price":"12000"}}
        </script>
        """
        soup = BeautifulSoup(html, "html.parser")
        cfg = collector.MODELS["BenQ TK705STi"]
        price, source = collector.extract_trusted_price("amazon", soup, "", cfg)
        self.assertIsNone(price)
        self.assertEqual(source, "unverified_amazon_price")

    def test_used_page_text_still_allowed_above_model_floor(self):
        soup = BeautifulSoup("<html><body>販売価格 75,000円</body></html>", "html.parser")
        cfg = collector.MODELS["BenQ TK700STi"]
        price, source = collector.extract_trusted_price(
            "yahoo_flea", soup, soup.get_text(" ", strip=True), cfg
        )
        self.assertEqual(price, 75000)
        self.assertEqual(source, "used_page_text")

    def test_used_price_below_floor_is_rejected(self):
        soup = BeautifulSoup("<html><body>販売価格 12,000円</body></html>", "html.parser")
        cfg = collector.MODELS["BenQ TK700STi"]
        price, source = collector.extract_trusted_price(
            "yahoo_flea", soup, soup.get_text(" ", strip=True), cfg
        )
        self.assertIsNone(price)
        self.assertEqual(source, "unverified_used_price")


if __name__ == "__main__":
    unittest.main()
