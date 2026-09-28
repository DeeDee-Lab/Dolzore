#!/usr/bin/env python3
from __future__ import annotations
import argparse, concurrent.futures, json, re, time
from datetime import datetime, timezone
from pathlib import Path
from urllib.parse import quote, quote_plus, urljoin, urlparse

import requests
from bs4 import BeautifulSoup

HEADERS={"User-Agent":"Mozilla/5.0 (compatible; DOLZORE-SmartBuy-GitHub/1.0)"}
MODELS={
 "BenQ TK700STi":{"terms":["tk700sti"],"strong":75000,"consider":90000,"fit":True},
 "BenQ TK705STi":{"terms":["tk705sti"],"strong":130000,"consider":145000,"fit":True},
 "Optoma UHD35STx":{"terms":["uhd35stx"],"strong":80000,"consider":100000,"fit":True},
 "BenQ TK710STi":{"terms":["tk710sti"],"strong":160000,"consider":180000,"fit":True},
 "ViewSonic X10-4K":{"terms":["x10-4k","x10 4k"],"strong":60000,"consider":65000,"fit":False},
 "XGIMI AURA":{"terms":["xgimi aura","aura xm03a"],"strong":90000,"consider":100000,"fit":False},
}
MARKETS={
 "mercari":("メルカリ","jp.mercari.com"),
 "yahoo_auction":("Yahoo!オークション","auctions.yahoo.co.jp"),
 "amazon":("Amazon","amazon.co.jp"),
 "rakuten":("楽天市場","rakuten.co.jp"),
 "hardoff":("ハードオフ","hardoff.co.jp"),
 "secondstreet":("セカンドストリート","2ndstreet.jp"),
 "yahoo_flea":("Yahoo!フリマ","paypayfleamarket.yahoo.co.jp"),
 "rakuma":("ラクマ","fril.jp"),
 "yahoo_shopping":("Yahoo!ショッピング","shopping.yahoo.co.jp"),
}
NEW={"amazon","rakuten","yahoo_shopping"}
USED=set(MARKETS)-NEW
BAD=("交換ランプ","互換ランプ","ランプモジュール","プロジェクター用ランプ","スタンド","スクリーン","リモコン","ケーブル","金具","部品","ジャンク","故障","投影不可","投影不良")

def compact(v): return re.sub(r"[^a-z0-9ぁ-んァ-ヶ一-龠々ー]+","",str(v or "").lower())
def model_match(title):
    t=compact(title)
    for name,cfg in MODELS.items():
        if any(compact(x) in t for x in cfg["terms"]):
            return name,cfg
    return None,None

def direct_url(market,q):
    qp=quote_plus(q)
    return {
      "yahoo_shopping":f"https://shopping.yahoo.co.jp/search?p={qp}",
      "rakuten":f"https://search.rakuten.co.jp/search/mall/{quote(q)}/",
      "yahoo_auction":f"https://auctions.yahoo.co.jp/search/search?p={qp}",
      "mercari":f"https://jp.mercari.com/search?keyword={qp}&status=on_sale",
      "yahoo_flea":f"https://paypayfleamarket.yahoo.co.jp/search/{quote(q)}",
      "rakuma":f"https://fril.jp/s?query={qp}",
      "hardoff":f"https://netmall.hardoff.co.jp/search/?q={qp}",
      "secondstreet":f"https://www.2ndstreet.jp/search?keyword={qp}",
      "amazon":f"https://www.amazon.co.jp/s?k={qp}",
    }[market]

def individual(market, raw):
    try:
        u=urlparse(raw); h=(u.hostname or "").lower(); p=u.path.lower()
        if market=="mercari": return h.endswith("mercari.com") and p.startswith("/item/")
        if market=="yahoo_flea": return h=="paypayfleamarket.yahoo.co.jp" and p.startswith("/item/")
        if market=="yahoo_auction": return h=="page.auctions.yahoo.co.jp" or (h=="auctions.yahoo.co.jp" and p.startswith("/jp/auction/"))
        if market=="amazon": return "amazon.co.jp" in h and ("/dp/" in p or "/gp/product/" in p)
        if market=="rakuten": return h=="item.rakuten.co.jp" and len([x for x in p.split("/") if x])>=2
        if market=="hardoff": return "hardoff.co.jp" in h and "/product/" in p
        if market=="secondstreet": return "2ndstreet.jp" in h and "/goods/detail/" in p
        if market=="rakuma": return h=="item.fril.jp" and bool(p.strip("/"))
        if market=="yahoo_shopping": return h.endswith("shopping.yahoo.co.jp") and "/search" not in p and len(p)>8
    except Exception:
        return False
    return False

def parse_price(text):
    vals=[]
    for m in re.finditer(r"(?:¥|￥)\s*([0-9][0-9,]{2,})|([0-9][0-9,]{2,})\s*円",text or ""):
        raw=(m.group(1) or m.group(2) or "").replace(",","")
        try:
            n=int(raw)
            if 1000<=n<=2000000: vals.append(n)
        except Exception: pass
    return min(vals) if vals else None

def detail(market,url,fallback):
    try:
        r=requests.get(url,headers=HEADERS,timeout=10,allow_redirects=True)
        if not individual(market,r.url): return None
        soup=BeautifulSoup(r.text[:900000],"html.parser")
        meta=soup.find("meta",property="og:title")
        title=(meta.get("content") if meta else None) or (soup.title.get_text(" ",strip=True) if soup.title else fallback)
        title=str(title)[:240]
        if any(x.lower() in title.lower() for x in BAD): return None
        model,cfg=model_match(title)
        if not model or not cfg.get("fit"): return None
        text=soup.get_text(" ",strip=True)[:220000]
        price=None
        for script in soup.find_all("script",type="application/ld+json")[:20]:
            try:
                obj=json.loads(script.string or "{}")
                stack=obj if isinstance(obj,list) else [obj]
                for row in stack:
                    if not isinstance(row,dict): continue
                    offers=row.get("offers")
                    if isinstance(offers,list): offers=offers[0] if offers else None
                    if isinstance(offers,dict) and offers.get("price") is not None:
                        price=int(float(str(offers["price"]).replace(",",""))); break
                if price is not None: break
            except Exception: pass
        if price is None: price=parse_price(text)
        if price is None or price<10000 or price>2000000: return None
        lower=text.lower()
        sold=("売り切れ","売却済み","sold out","取引完了","販売終了","この商品は削除","売れました")
        live_tokens=("購入手続きへ","購入する","今すぐ購入","カートに入れる","入札する","落札する","購入できます")
        if r.status_code>=400 or any(x in lower for x in sold): return None
        if market in {"mercari","yahoo_flea","rakuma","yahoo_auction"} and not any(x in lower for x in live_tokens): return None
        if market in NEW:
            status="acceptable"
        elif price<=cfg["strong"]: status="strong_buy"
        elif price<=cfg["consider"]: status="consider"
        else: return None
        return {"title":title,"model":model,"market":market,"marketLabel":MARKETS[market][0],"price":price,"priceStatus":status,"capturedAt":datetime.now(timezone.utc).isoformat(),"url":r.url}
    except Exception:
        return None

def search_one(model,market):
    label,domain=MARKETS[market]
    url=direct_url(market,model)
    links=[]
    try:
        r=requests.get(url,headers=HEADERS,timeout=10)
        soup=BeautifulSoup(r.text,"html.parser")
        for a in soup.select("a[href]"):
            href=urljoin(r.url,a.get("href"))
            if individual(market,href):
                links.append((href,a.get_text(" ",strip=True) or model))
                if len(links)>=3: break
    except Exception: pass
    if not links:
        try:
            q=quote_plus(f"site:{domain} {model}")
            r=requests.get(f"https://html.duckduckgo.com/html/?q={q}",headers=HEADERS,timeout=10)
            soup=BeautifulSoup(r.text,"html.parser")
            for a in soup.select("a[href]"):
                href=a.get("href") or ""
                m=re.search(r"uddg=([^&]+)",href)
                if m:
                    from urllib.parse import unquote
                    href=unquote(m.group(1))
                if individual(market,href):
                    links.append((href,a.get_text(" ",strip=True) or model))
                    if len(links)>=2: break
        except Exception: pass
    for href,title in links:
        row=detail(market,href,title)
        if row and row["model"]==model: return row
    return None

def main(out):
    jobs=[]
    rows=[]
    with concurrent.futures.ThreadPoolExecutor(max_workers=12) as ex:
        for model in MODELS:
            for market in MARKETS:
                jobs.append(ex.submit(search_one,model,market))
        for fut in concurrent.futures.as_completed(jobs):
            row=fut.result()
            if row: rows.append(row)
    best={}
    for r in rows:
        best[r["url"]]=r
    rows=list(best.values())
    order={"strong_buy":0,"consider":1,"acceptable":2}
    rows.sort(key=lambda r:(order.get(r["priceStatus"],9),r["price"]))
    payload={
      "ok":True,
      "generatedAt":datetime.now(timezone.utc).isoformat(),
      "observedProjectorRows":len(rows),
      "used":[r for r in rows if r["market"] in USED][:10],
      "newItems":[r for r in rows if r["market"] in NEW][:10],
      "source":"github-actions-direct-market-collection",
      "note":"Only strict verified individual listings are published."
    }
    out.parent.mkdir(parents=True,exist_ok=True)
    out.write_text(json.dumps(payload,ensure_ascii=False,indent=2),encoding="utf-8")
    print(json.dumps({"used":len(payload["used"]),"new":len(payload["newItems"])},ensure_ascii=False))

if __name__=="__main__":
    ap=argparse.ArgumentParser(); ap.add_argument("--output",required=True)
    args=ap.parse_args(); main(Path(args.output))
