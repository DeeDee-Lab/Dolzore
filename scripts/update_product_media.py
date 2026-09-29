#!/usr/bin/env python3
from __future__ import annotations

import argparse
import json
from datetime import datetime, timezone
from pathlib import Path
from urllib.parse import urljoin, urlparse

import requests
from bs4 import BeautifulSoup

HEADERS={"User-Agent":"Mozilla/5.0 (compatible; DOLZORE-MediaBot/1.0; official product media resolver)"}

def valid_https(url: str) -> bool:
    try:
        u=urlparse(url)
        return u.scheme=="https" and bool(u.netloc)
    except Exception:
        return False

def resolve_image(source_url: str):
    try:
        r=requests.get(source_url,headers=HEADERS,timeout=12,allow_redirects=True)
        if r.status_code>=400:
            return None
        soup=BeautifulSoup(r.text[:1200000],"html.parser")
        candidates=[]
        for attrs in (
            {"property":"og:image"},
            {"property":"og:image:secure_url"},
            {"name":"twitter:image"},
            {"name":"twitter:image:src"},
        ):
            tag=soup.find("meta",attrs=attrs)
            if tag and tag.get("content"):
                candidates.append(urljoin(r.url,tag["content"]))
        for img in soup.find_all("img",src=True)[:80]:
            alt=(img.get("alt") or "").lower()
            if any(k in alt for k in ("projector","プロジェクター","tk700","tk705","tk710","x10","aura","eh-ls800","x3100","n1s","horizon","uhd35")):
                candidates.append(urljoin(r.url,img["src"]))
        for url in candidates:
            if valid_https(url):
                return url
    except Exception:
        return None
    return None

def stable_view(data):
    return [(x.get("model"),x.get("sourceUrl"),x.get("imageUrl")) for x in data.get("products",[])]

def main(path: Path):
    data=json.loads(path.read_text(encoding="utf-8"))
    before=stable_view(data)
    resolved=0
    for row in data.get("products",[]):
        image=resolve_image(row.get("sourceUrl",""))
        if image:
            row["imageUrl"]=image
            resolved+=1
    data["updatedAt"]=datetime.now(timezone.utc).isoformat()
    after=stable_view(data)
    if before==after:
        print(f"PRODUCT_MEDIA_UNCHANGED resolved={resolved}")
        return
    path.write_text(json.dumps(data,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
    print(f"PRODUCT_MEDIA_UPDATED resolved={resolved}")

if __name__=="__main__":
    ap=argparse.ArgumentParser()
    ap.add_argument("--file",required=True)
    a=ap.parse_args()
    main(Path(a.file))
