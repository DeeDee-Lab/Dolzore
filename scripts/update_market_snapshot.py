#!/usr/bin/env python3
from __future__ import annotations

import argparse
import concurrent.futures
import json
import re
from datetime import datetime, timezone
from pathlib import Path
from urllib.parse import quote, quote_plus, unquote, urljoin, urlparse

import requests
from bs4 import BeautifulSoup

HEADERS={"User-Agent":"Mozilla/5.0 (compatible; DOLZORE-MarketBot/1.0; public editorial snapshot)"}

MODELS={
    "BenQ TK700STi":{"terms":["tk700sti"],"strong":75000,"consider":90000},
    "BenQ TK705STi":{"terms":["tk705sti"],"strong":130000,"consider":145000},
    "BenQ TK710STi":{"terms":["tk710sti"],"strong":160000,"consider":180000},
    "Optoma UHD35STx":{"terms":["uhd35stx"],"strong":80000,"consider":100000},
    "ViewSonic X10-4K":{"terms":["x10-4k","x104k"],"strong":60000,"consider":65000},
    "XGIMI AURA":{"terms":["xgimiaura","auraxm03a"],"strong":90000,"consider":100000},
    "XGIMI HORIZON S Max":{"terms":["horizonsmax","horizon s max"],"strong":None,"consider":None},
    "JMGO N1S Ultra 4K":{"terms":["n1sultra4k","n1s ultra 4k"],"strong":None,"consider":None},
    "Epson EH-LS800":{"terms":["ehls800","eh-ls800"],"strong":None,"consider":None},
    "BenQ X3100i":{"terms":["x3100i"],"strong":None,"consider":None},
}

MARKETS={
    "mercari":("メルカリ","jp.mercari.com","used"),
    "yahoo_auction":("Yahoo!オークション","auctions.yahoo.co.jp","used"),
    "hardoff":("ハードオフ","hardoff.co.jp","used"),
    "secondstreet":("セカンドストリート","2ndstreet.jp","used"),
    "yahoo_flea":("Yahoo!フリマ","paypayfleamarket.yahoo.co.jp","used"),
    "rakuma":("ラクマ","fril.jp","used"),
    "amazon":("Amazon","amazon.co.jp","new"),
    "rakuten":("楽天市場","rakuten.co.jp","new"),
    "yahoo_shopping":("Yahoo!ショッピング","shopping.yahoo.co.jp","new"),
}

BAD_TERMS=(
    "交換ランプ","互換ランプ","ランプモジュール","プロジェクター用ランプ",
    "スタンド","スクリーン","リモコン","ケーブル","金具","部品",
    "ジャンク","故障","投影不可","投影不良","映らない","電源のみ",
)
BAD_BODY_TERMS=(
    "使用できない","使用出来ない","修理できる方","修理出来る方",
    "メーカーに修理","要修理","動作未確認","現状品","部品取り",
)
SOLD_TOKENS=("売り切れ","売却済み","sold out","取引完了","販売終了","この商品は削除","売れました")
BUY_TOKENS=("購入手続きへ","購入する","今すぐ購入","カートに入れる","入札する","落札する","購入できます")


def compact(v):
    return re.sub(r"[^a-z0-9ぁ-んァ-ヶ一-龠々ー]+","",str(v or "").lower())


def match_model(title):
    t=compact(title)
    for name,cfg in MODELS.items():
        if any(compact(term) in t for term in cfg["terms"]):
            return name,cfg
    return None,None


def direct_url(market,query):
    q=quote_plus(query)
    return {
        "mercari":f"https://jp.mercari.com/search?keyword={q}&status=on_sale",
        "yahoo_auction":f"https://auctions.yahoo.co.jp/search/search?p={q}",
        "hardoff":f"https://netmall.hardoff.co.jp/search/?q={q}",
        "secondstreet":f"https://www.2ndstreet.jp/search?keyword={q}",
        "yahoo_flea":f"https://paypayfleamarket.yahoo.co.jp/search/{quote(query)}",
        "rakuma":f"https://fril.jp/s?query={q}",
        "amazon":f"https://www.amazon.co.jp/s?k={q}",
        "rakuten":f"https://search.rakuten.co.jp/search/mall/{quote(query)}/",
        "yahoo_shopping":f"https://shopping.yahoo.co.jp/search?p={q}",
    }[market]


def individual_url(market,raw):
    try:
        u=urlparse(raw); host=(u.hostname or "").lower(); p=u.path.lower()
        if market=="mercari": return host.endswith("mercari.com") and p.startswith("/item/")
        if market=="yahoo_flea": return host=="paypayfleamarket.yahoo.co.jp" and p.startswith("/item/")
        if market=="yahoo_auction": return host=="page.auctions.yahoo.co.jp" or (host=="auctions.yahoo.co.jp" and p.startswith("/jp/auction/"))
        if market=="hardoff": return "hardoff.co.jp" in host and "/product/" in p
        if market=="secondstreet": return "2ndstreet.jp" in host and "/goods/detail/" in p
        if market=="rakuma": return host=="item.fril.jp" and bool(p.strip("/"))
        if market=="amazon": return "amazon.co.jp" in host and ("/dp/" in p or "/gp/product/" in p)
        if market=="rakuten": return host=="item.rakuten.co.jp" and len([x for x in p.split("/") if x])>=2
        if market=="yahoo_shopping": return host.endswith("shopping.yahoo.co.jp") and "/search" not in p and len(p)>8
    except Exception:
        return False
    return False


def unwrap(href):
    if not href: return ""
    if "uddg=" in href:
        m=re.search(r"uddg=([^&]+)",href)
        if m: return unquote(m.group(1))
    return href


def parse_price(text):
    vals=[]
    for m in re.finditer(r"(?:¥|￥)\s*([0-9][0-9,]{2,})|([0-9][0-9,]{2,})\s*円",text or ""):
        raw=(m.group(1) or m.group(2) or "").replace(",","")
        try:
            n=int(raw)
            if 10000<=n<=2000000: vals.append(n)
        except Exception:
            pass
    return min(vals) if vals else None


def parse_og_image(soup, base_url):
    for attrs in (
        {"property":"og:image"},
        {"property":"og:image:secure_url"},
        {"name":"twitter:image"},
    ):
        tag=soup.find("meta",attrs=attrs)
        if tag and tag.get("content"):
            url=urljoin(base_url,tag["content"])
            try:
                u=urlparse(url)
                if u.scheme=="https" and u.netloc:
                    return url
            except Exception:
                pass
    return None


def parse_jsonld_price(soup):
    for script in soup.find_all("script",type="application/ld+json")[:24]:
        try:
            obj=json.loads(script.string or "{}")
            stack=obj if isinstance(obj,list) else [obj]
            for row in stack:
                if not isinstance(row,dict): continue
                offers=row.get("offers")
                if isinstance(offers,list): offers=offers[0] if offers else None
                if isinstance(offers,dict) and offers.get("price") is not None:
                    n=int(float(str(offers["price"]).replace(",","")))
                    if 10000<=n<=2000000: return n
        except Exception:
            pass
    return None


def inspect_listing(market,url,fallback_title):
    try:
        r=requests.get(url,headers=HEADERS,timeout=10,allow_redirects=True)
        if r.status_code>=400 or not individual_url(market,r.url): return None
        soup=BeautifulSoup(r.text[:900000],"html.parser")
        meta=soup.find("meta",property="og:title")
        title=(meta.get("content") if meta else None) or (soup.title.get_text(" ",strip=True) if soup.title else fallback_title)
        title=str(title)[:240]
        if any(x.lower() in title.lower() for x in BAD_TERMS): return None
        model,cfg=match_model(title)
        if not model: return None

        text=soup.get_text(" ",strip=True)[:220000]
        lower=text.lower()
        if any(x in lower for x in SOLD_TOKENS): return None
        if any(x.lower() in lower for x in BAD_BODY_TERMS): return None

        # Consumer-to-consumer pages may remain indexed after sale. Require a positive buy action.
        if market in {"mercari","yahoo_flea","rakuma","yahoo_auction"} and not any(x in lower for x in BUY_TOKENS):
            return None

        price=parse_jsonld_price(soup) or parse_price(text)
        if price is None: return None
        image_url=parse_og_image(soup,r.url)

        lane=MARKETS[market][2]
        if lane=="used":
            strong=cfg.get("strong")
            consider=cfg.get("consider")
            if strong is not None and price<=strong:
                price_status="strong_buy"
            elif consider is not None and price<=consider:
                price_status="consider"
            elif consider is not None:
                price_status="over_target"
            else:
                price_status="market_price"
        else:
            price_status="new_market"

        return {
            "title":title,
            "model":model,
            "market":market,
            "marketLabel":MARKETS[market][0],
            "price":price,
            "priceStatus":price_status,
            "imageUrl":image_url,
            "url":r.url,
        }
    except Exception:
        return None


def discover_links(model,market):
    _,domain,_=MARKETS[market]
    links=[]
    try:
        r=requests.get(direct_url(market,model),headers=HEADERS,timeout=10)
        soup=BeautifulSoup(r.text,"html.parser")
        for a in soup.select("a[href]"):
            href=urljoin(r.url,unwrap(a.get("href") or ""))
            if individual_url(market,href):
                links.append((href,a.get_text(" ",strip=True) or model))
                if len(links)>=3: break
    except Exception:
        pass
    if links: return links

    try:
        q=quote_plus(f"site:{domain} {model}")
        r=requests.get(f"https://html.duckduckgo.com/html/?q={q}",headers=HEADERS,timeout=10)
        soup=BeautifulSoup(r.text,"html.parser")
        for a in soup.select("a[href]"):
            href=unwrap(a.get("href") or "")
            if individual_url(market,href):
                links.append((href,a.get_text(" ",strip=True) or model))
                if len(links)>=2: break
    except Exception:
        pass
    return links


def collect_one(model,market):
    for href,title in discover_links(model,market):
        row=inspect_listing(market,href,title)
        if row and row["model"]==model:
            return row
    return None


def main(output:Path):
    rows=[]
    with concurrent.futures.ThreadPoolExecutor(max_workers=10) as pool:
        futs=[pool.submit(collect_one,model,market) for model in MODELS for market in MARKETS]
        for fut in concurrent.futures.as_completed(futs):
            row=fut.result()
            if row: rows.append(row)

    # De-duplicate exact listing URLs.
    unique={r["url"]:r for r in rows}
    rows=list(unique.values())
    rank={"strong_buy":0,"consider":1,"over_target":2,"market_price":3,"new_market":4}
    rows.sort(key=lambda r:(rank.get(r["priceStatus"],9),r["price"]))

    observed_used=[r for r in rows if MARKETS[r["market"]][2]=="used"]
    observed_new=[r for r in rows if MARKETS[r["market"]][2]=="new"]
    recommended_used=[r for r in observed_used if r["priceStatus"] in {"strong_buy","consider"}]

    model_summaries=[]
    for model,cfg in MODELS.items():
        used_for_model=sorted((r for r in observed_used if r["model"]==model),key=lambda r:r["price"])
        new_for_model=sorted((r for r in observed_new if r["model"]==model),key=lambda r:r["price"])
        model_summaries.append({
            "model":model,
            "strongBuy":cfg.get("strong"),
            "consider":cfg.get("consider"),
            "bestUsed":used_for_model[0] if used_for_model else None,
            "bestNew":new_for_model[0] if new_for_model else None,
        })

    payload={
        "schemaVersion":3,
        "generatedAt":datetime.now(timezone.utc).isoformat(),
        "source":"github-hosted-public-market-snapshot",
        "used":recommended_used[:10],
        "newItems":observed_new[:10],
        "observedUsed":observed_used[:30],
        "modelSummaries":model_summaries,
        "targets":[
            {
                "model":model,
                "strongBuy":cfg.get("strong"),
                "consider":cfg.get("consider"),
            }
            for model,cfg in MODELS.items()
        ],
        "trackedModels":list(MODELS),
        "note":"Verified live listings are retained even when above target so the page can show current market price and a WAIT decision. Recommendation remains separate."
    }

    output.parent.mkdir(parents=True,exist_ok=True)
    output.write_text(json.dumps(payload,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
    print(json.dumps({"used":len(payload["used"]),"new":len(payload["newItems"])},ensure_ascii=False))


if __name__=="__main__":
    ap=argparse.ArgumentParser()
    ap.add_argument("--output",required=True)
    args=ap.parse_args()
    main(Path(args.output))
