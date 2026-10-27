"""Isolated DOLZORE public projector market collector contract tests; no live network."""
from __future__ import annotations

import importlib.util
import json
import tempfile
import unittest
from pathlib import Path
from unittest.mock import patch

ROOT = Path(__file__).resolve().parents[1]
SPEC = importlib.util.spec_from_file_location(
    "market_snapshot", ROOT / "scripts" / "update_market_snapshot.py")
market = importlib.util.module_from_spec(SPEC)
SPEC.loader.exec_module(market)


class PublicMarketSnapshotTests(unittest.TestCase):
    def test_models_cover_smartbuy_targets(self):
        for model in ("BenQ TK700STi", "BenQ TK705STi", "BenQ X500i",
                      "NOMVDIC P1000", "Optoma GT2160HDR", "BenQ W2720i"):
            self.assertIn(model, market.MODELS)

    def test_model_matching_keeps_short_and_long_ids_distinct(self):
        self.assertEqual(market.match_model("P1000 4K 短焦点")[0], "NOMVDIC P1000")
        self.assertEqual(market.match_model("TK705STi 4K 本体")[0], "BenQ TK705STi")
        self.assertIsNone(market.match_model("TK705i 4K 本体")[0])

    def test_sold_listing_not_returned_as_available(self):
        class Response:
            url = "https://paypayfleamarket.yahoo.co.jp/item/z123123123"
            status_code = 200
            text = ('<html><head><meta property="og:title" content="BenQ TK700STi">'
                    '<script type="application/ld+json">{"offers":{"price":69000}}</script></head>'
                    '<body>購入手続きへ 売り切れました</body></html>')
        with patch.object(market.requests, "get", return_value=Response()):
            self.assertIsNone(market.inspect_listing("yahoo_flea", Response.url, "TK700STi"))

    def test_all_market_failures_preserve_published_json(self):
        with tempfile.TemporaryDirectory() as temp:
            out = Path(temp) / "market.json"
            original = '{"generatedAt":"2026-09-30T00:00:00+00:00"}'
            out.write_text(original, encoding="utf-8")
            with patch.object(market, "collect_one", return_value=None):
                with self.assertRaisesRegex(RuntimeError, "NO_VERIFIED_DETAIL_LISTINGS"):
                    market.main(out)
            self.assertEqual(out.read_text(encoding="utf-8"), original)


if __name__ == "__main__":
    unittest.main()
