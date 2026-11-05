#!/usr/bin/env python3
"""One-off used 14in gaming laptop market scan, independent of Railway.

Public marketplace pages only. No authentication, checkout, owner data or paid API.
Positive price and buy controls are NOT proof of real-time stock or hardware quality.
"""
from __future__ import annotations

import argparse
import concurrent.futures
import json
import os
import re
from collections import Counter
from datetime import datetime, timezone
from pathlib import Path
from urllib.parse import urlparse

import requests
from bs4 import BeautifulSoup
import update_market_snapshot as market

ROOT = Path(__file__).resolve().parents[1]
TARGET_PATH = ROOT / "docs/data/targets/used-gaming-laptop-14-rtx4060.json"
OUTPUT_PATH = ROOT / "docs/data/smartbuy-gaming-laptops.json"
MODEL_ALIASES = {
    "ASUS ROG Zephyrus G14": ("zephyrus g14", "ga403", "ga402"),
    "ASUS TUF Gaming A14": ("tuf gaming a14", "tuf a14", "fa401"),
    "HP OMEN Transcend 14": ("omen transcend 14", "omen transcend14", "14-fb"),
    "Lenovo Legion Slim 5 14": ("legion slim 5 14", "legion slim 14", "14aph8"),
}
REJECT_CONDITION = (
    "ジャンク", "難あり", "液晶割れ", "画面割れ", "画面にひび", "ひび割れ",
    "不具合", "故障", "起動しない", "動作未確認", "動作保証なし",
    "現状品", "部品取り", "破損", "欠品あり", "ssdなし", "通電のみ",
)
BUY_ACTION = market.BUY_TOKENS
SOLD = market.SOLD_TOKENS
USED_MARKETS = ("mercari", "yahoo_flea", "hardoff", "secondstreet", "yahoo_auction", "rakuma")
SEED_URLS = (
    ("mercari", "https://jp.mercari.com/item/m53080545807"),
    ("mercari", "https://jp.mercari.com/item/m92953765124"),
    ("mercari", "https://jp.mercari.com/item/m11682504561"),
)

def compact(s):
    return re.sub(r"[^a-z0-9]+", "", str(s or "").lower())

def identify_model(text):
    t = compact(text)
    for model, aliases in MODEL_ALIASES.items():
        if any(compact(a) in t for a in aliases):
            return model
    return None

def parse_gpu(text):
    matches = re.findall(r"(?:geforce\s*)?rtx[\s\-]*(4060|4070|4080|4090|5060|5070\s*ti|5070|5080|5090|4050|3050|3060|3070|3080)\b", text, re.I)
    if not matches:
        return None
    return matches[0].upper().replace(" ", "")

def read_ram(text):
    a = re.search(r"(?:メモリ|RAM|memory)[\s:：=]*(?:容量[\s:：]*)?(\d{1,3})\s*GB", text, re.I)
    if a:
        return int(a.group(1))
    a = re.search(r"(\d{1,3})\s*GB[\s　]*(?:RAM|メモリ|memory)(?!カード)", text, re.I)
    return int(a.group(1)) if a else None

def read_ssd(text):
    matches = []
    forms = (
        r"(?:SSD|ストレージ|Storage)[\s:：=]*(?:容量[\s:：]*)?(\d+(?:\.\d+)?)\s*(TB|GB)",
        r"(\d+(?:\.\d+)?)\s*(TB|GB)[\s　]*(?:SSD|NVMe)",
        r"(?:SSD|NVMe)[^\n。]{0,14}?(\d+(?:\.\d+)?)\s*(TB|GB)",
    )
    for pattern in forms:
        for m in re.finditer(pattern, text, re.I):
            qty = float(m.group(1))
            gb = round(qty * (1024 if m.group(2).upper() == "TB" else 1))
            if 128 <= gb <= 8192:
                matches.append(gb)
    return max(matches) if matches else None

def read_weight(text):
    a = re.search(r"(?:本体重量|本体質量|重量|質量|weight|約)[\s:：=]*(\d(?:\.\d{1,3})?)\s*(?:kg|㎏|キログラム)\b", text, re.I)
    return float(a.group(1)) if a else None

def read_screen(text):
    a = re.search(r"\b(1[3-6](?:\.\d)?)\s*(?:インチ|型|inch|in\b|[″\"]) ", text + " ", re.I)
    if not a:
        a = re.search(r"\b(1[3-6](?:\.\d)?)\s*(?:インチ|型|inch|in\b|[″\"])", text, re.I)
    return float(a.group(1)) if a else None

def match_listing(market_name, url, target, session=requests):
    """Return an audit record for an exact per-item page, or a rejection reason."""
    if not market.individual_url(market_name, url):
        return {"rejected": "not_individual_url"}
    try:
        response = session.get(url, headers=market.HEADERS, timeout=12, allow_redirects=True)
        if response.status_code != 200 or not market.individual_url(market_name, response.url):
            return {"rejected": "unreadable_detail", "httpStatus": response.status_code}
        soup = BeautifulSoup(response.text[:900000], "html.parser")
        tag = soup.find("meta", property="og:title")
        title = (tag.get("content") if tag else None) or (soup.title.get_text(" ", strip=True) if soup.title else "")
        title = str(title)[:250]
        meta_description = soup.find("meta", attrs={"name": "description"})
        description = (meta_description.get("content") if meta_description else "") or ""
        main = soup.find("main")
        main_text = (main or soup.body or soup).get_text(" ", strip=True)[:20000]
        content = (title + " " + description + " " + main_text)[:26000]
        lowered = content.lower()
        model = identify_model(title + " " + description)
        if not model:
            return {"rejected": "wrong_model"}
        if any(s in lowered for s in SOLD):
            return {"rejected": "sold"}
        if any(s in lowered for s in REJECT_CONDITION):
            return {"rejected": "damaged_or_untested"}
        if not any(s in lowered for s in BUY_ACTION):
            return {"rejected": "no_buy_action"}
        price = market.parse_jsonld_price(soup) or market.parse_price(content)
        if price is None:
            return {"rejected": "price_unknown"}
        gpu = parse_gpu(content)
        ram = read_ram(content)
        ssd = read_ssd(content)
        weight = read_weight(content)
        screen = read_screen(content)
        # The model family signals a 14-inch nominal laptop, but is NOT proof of exact screen spec.
        reasons = []
        if gpu not in {g.upper().replace(" ", "") for g in target["gpu"]["acceptedLaptopGpuModels"]}:
            if gpu:
                return {"rejected": "gpu_below_minimum", "gpu": gpu}
            reasons.append("GPUの4060 Laptop以上を個別仕様で確認できない")
        if ram is None:
            reasons.append("メモリ容量の個別確認が必要")
        elif ram < target["memoryGB"]["min"]:
            return {"rejected": "insufficient_memory", "memoryGB": ram}
        if ssd is None:
            reasons.append("SSD容量の個別確認が必要")
        elif ssd < target["ssdGB"]["min"]:
            return {"rejected": "insufficient_ssd", "ssdGB": ssd}
        if weight is None:
            reasons.append("本体重量1.7kg以下の個別確認が必要")
        elif weight > target["maxWeightKg"]:
            return {"rejected": "too_heavy", "weightKg": weight}
        if screen is None:
            reasons.append("画面サイズの個別確認が必要")
        elif not target["screenInches"]["min"] <= screen <= target["screenInches"]["max"]:
            return {"rejected": "screen_size_mismatch", "screenInches": screen}

        if not target["priceJPY"]["min"] <= price <= target["priceJPY"]["max"]:
            decision = "WAIT"
            reasons.insert(0, "価格帯（8万〜13万円）外")
        elif reasons:
            decision = "HOLD"
        else:
            decision = "CONSIDER"
            reasons = ["出品記載の仕様は条件内。注文時の在庫・実機動作・送料確認が必要"]
        return {
            "model": model,
            "title": title,
            "priceJPY": int(price),
            "seller": market.MARKETS[market_name][0],
            "market": market_name,
            "url": response.url,
            "decision": decision,
            "gpu": gpu, "memoryGB": ram, "ssdGB": ssd,
            "screenInches": screen, "weightKg": weight,
            "condition": "used_listing_unverified",
            "inventory": "unconfirmed_at_checkout",
            "reasons": reasons,
            "verifiedAt": datetime.now(timezone.utc).isoformat(),
        }
    except requests.RequestException as exc:
        return {"rejected": "network_error", "errorType": type(exc).__name__}
    except Exception as exc:
        return {"rejected": "parse_error", "errorType": type(exc).__name__}

def discover_tasks():
    tasks = [(market_name, url) for market_name, url in SEED_URLS]
    counts = Counter()
    search_jobs = []
    for model in MODEL_ALIASES:
        for market_name in USED_MARKETS:
            query = model + " RTX 4060"
            if model.startswith("Lenovo"):
                query = "Legion Slim 5 14APH8 RTX4060"
            search_jobs.append((model, market_name, query))
    with concurrent.futures.ThreadPoolExecutor(max_workers=6) as pool:
        futures = {pool.submit(market.discover_links, query, vendor): vendor
                   for _, vendor, query in search_jobs}
        for f in concurrent.futures.as_completed(futures):
            vendor = futures[f]
            try:
                links = f.result()
                counts[vendor] += 1
                for link, _ in links[:3]:
                    tasks.append((vendor, link))
            except Exception:
                counts["discovery_error"] += 1
    return list(dict.fromkeys(tasks)), dict(counts)

def main(target_path=TARGET_PATH, output_path=OUTPUT_PATH):
    target = json.loads(Path(target_path).read_text(encoding="utf-8"))
    tasks, discovery = discover_tasks()
    results = []
    rejected = Counter()
    with concurrent.futures.ThreadPoolExecutor(max_workers=8) as pool:
        futures = [pool.submit(match_listing, vendor, url, target) for vendor, url in tasks]
        for f in concurrent.futures.as_completed(futures):
            row = f.result()
            if row.get("rejected"):
                rejected[row["rejected"]] += 1
            else:
                results.append(row)
    priority = {"CONSIDER": 0, "HOLD": 1, "WAIT": 2}
    results.sort(key=lambda r: (priority.get(r["decision"], 3), r["priceJPY"], -(r.get("memoryGB") or 0)))
    now = datetime.now(timezone.utc).isoformat()
    payload = {
        "schemaVersion": 1,
        "targetId": target["targetId"],
        "generatedAt": now,
        "source": "github-actions-one-shot-market-detail-scan",
        "runId": os.environ.get("GITHUB_RUN_ID"),
        "runUrl": "https://github.com/DeeDee-Lab/Dolzore/actions/runs/" + os.environ["GITHUB_RUN_ID"] if os.environ.get("GITHUB_RUN_ID") else None,
        "target": target,
        "requestedSites": list(USED_MARKETS),
        "discoveryRequests": discovery,
        "detailPagesInspected": len(tasks),
        "rejections": dict(rejected),
        "observedCount": len(results),
        "verifiedBuyCount": 0,
        "qualifiedForConsiderCount": sum(1 for r in results if r["decision"] == "CONSIDER"),
        "status": "scanned_no_matching_individual_listings" if not results else "completed_market_observation",
        "results": results[:target["policy"]["resultsLimit"]],
        "dataLimitations": [
            "Only individually fetched seller pages are scored. Index snippets do not prove active stock.",
            "HOLD means one or more required hardware specifications were not verifiable from that seller page.",
            "No automatic BUY: current checkout, return policy, battery, condition and shipping remain unverified.",
        ],
    }
    output_path = Path(output_path)
    output_path.parent.mkdir(parents=True, exist_ok=True)
    output_path.write_text(json.dumps(payload, ensure_ascii=False, indent=2) + "\n", encoding="utf-8")
    print(json.dumps({"status": payload["status"], "detailPagesInspected": len(tasks), "results": len(results), "qualified": payload["qualifiedForConsiderCount"], "rejections": dict(rejected)}, ensure_ascii=False), flush=True)
    return payload

if __name__ == "__main__":
    parser = argparse.ArgumentParser()
    parser.add_argument("--target", default=str(TARGET_PATH))
    parser.add_argument("--output", default=str(OUTPUT_PATH))
    options = parser.parse_args()
    main(Path(options.target), Path(options.output))
