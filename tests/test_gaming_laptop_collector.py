from __future__ import annotations
import json
import sys
import unittest
from pathlib import Path
from unittest.mock import patch

ROOT=Path(__file__).resolve().parents[1]
sys.path.insert(0,str(ROOT/"scripts"))
import collect_gaming_laptops as scan

TARGET=json.loads((ROOT/"docs/data/targets/used-gaming-laptop-14-rtx4060.json").read_text(encoding="utf-8"))

class Response:
    status_code=200
    url="https://jp.mercari.com/item/m11223344556"
    def __init__(self, body): self.text=body

def listing(gpu="RTX 4060 Laptop GPU",ram=32,ssd="1TB",weight="1.5",price=120000,sold="",size="14.0型"):
    return (f'<html><head><meta property="og:title" content="ASUS ROG Zephyrus G14 GA403UV"><script type="application/ld+json">'
            f'{{"offers":{{"price":{price}}}}}</script></head><body><main>中古 ゲーミングノート 本体 購入する '
            f'{gpu} メモリ{ram}GB SSD {ssd} {size} 重量{weight}kg {sold}</main></body></html>')

class Tests(unittest.TestCase):
    def inspect(self, html):
        class Session:
            @staticmethod
            def get(*args,**kwargs):
                return Response(html)
        return scan.match_listing("mercari", Response.url, TARGET, session=Session())

    def test_complete_under_budget_is_consider_not_buy(self):
        r=self.inspect(listing())
        self.assertEqual(r["decision"],"CONSIDER")
        self.assertEqual(r["priceJPY"],120000)
        self.assertEqual(r["gpu"],"4060")
        self.assertEqual(r["memoryGB"],32)
        self.assertEqual(r["ssdGB"],1024)
        self.assertEqual(r["screenInches"],14.0)
        self.assertEqual(r["weightKg"],1.5)
        self.assertEqual(r["inventory"],"unconfirmed_at_checkout")

    def test_gpu_below_requirement_rejected(self):
        self.assertEqual(self.inspect(listing(gpu="RTX 3050 Laptop GPU"))["rejected"],"gpu_below_minimum")

    def test_overweight_rejected(self):
        self.assertEqual(self.inspect(listing(weight="1.75"))["rejected"],"too_heavy")

    def test_ram_too_small_rejected(self):
        self.assertEqual(self.inspect(listing(ram=8))["rejected"],"insufficient_memory")

    def test_ssd_too_small_rejected(self):
        self.assertEqual(self.inspect(listing(ssd="256GB"))["rejected"],"insufficient_ssd")

    def test_sold_and_damage_rejected(self):
        self.assertEqual(self.inspect(listing(sold="売り切れました"))["rejected"],"sold")
        self.assertEqual(self.inspect(listing(sold="液晶割れ"))["rejected"],"damaged_or_untested")

    def test_unknown_weight_hold(self):
        r=self.inspect(listing(weight="unknown "))
        self.assertEqual(r["decision"],"HOLD")
        self.assertTrue(any("重量" in reason for reason in r["reasons"]))

    def test_over_budget_wait(self):
        r=self.inspect(listing(price=160000))
        self.assertEqual(r["decision"],"WAIT")

    def test_wrong_model_rejected(self):
        r=self.inspect(listing().replace("Zephyrus G14 GA403UV","TUF F15 FA507"))
        self.assertEqual(r["rejected"],"wrong_model")

    def test_target_is_only_used_and_accepts_exact_requirements(self):
        self.assertEqual(TARGET["condition"],"used")
        self.assertEqual(TARGET["priceJPY"],{"min":80000,"max":130000})
        self.assertEqual(TARGET["maxWeightKg"],1.7)
        self.assertEqual(TARGET["memoryGB"]["min"],16)
        self.assertEqual(TARGET["ssdGB"]["min"],512)
        self.assertEqual(len(TARGET["models"]),4)
        self.assertTrue(TARGET["policy"]["neverAutoBuy"])

if __name__=="__main__":unittest.main()
