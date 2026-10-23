#!/usr/bin/env python3
"""Export ONLY the existing DOLZORE music shop to isolated Netlify hosting.

Preserves the original website files under docs/, never re-enables the
previously closed GitHub Pages / Unity legacy world, and never publishes
full paid MP3s. The exported storefront is checkout-disabled until all
license and purchase-delivery acceptance checks have passed.
"""
from __future__ import annotations

import json
from pathlib import Path
import re
import shutil

ROOT = Path(__file__).resolve().parents[1]
DOCS = ROOT / "docs"
OUTPUT = Path(__file__).resolve().parent / "dist"
LEGAL_ROUTES = ("legal", "terms", "privacy", "refund", "tokusho", "disclosure", "support")


def build() -> None:
    manifest = json.loads((DOCS / "data" / "tracks.json").read_text(encoding="utf-8"))
    tracks = manifest["tracks"]
    assert manifest["policy"]["saleScope"] == "music-only"
    assert len(tracks) == 60
    assert [t["id"] for t in tracks] == [f"BGM-{n:03d}" for n in range(1, 61)]
    assert all(t["priceYen"] == 200 for t in tracks)
    assert all(t["purchaseUrl"].startswith("https://buy.stripe.com/") for t in tracks)
    assert all(t.get("previewUrl", "").startswith("https://") for t in tracks if t.get("previewReady"))
    assert not manifest["policy"]["fullAudioPublic"]

    if OUTPUT.exists():
        shutil.rmtree(OUTPUT)
    OUTPUT.mkdir(parents=True)

    paths = ["music/index.html", "styles.css", "site.js", "data/tracks.json",
             "assets/dolzore-jukebox.png"]
    paths += [f"{name}/index.html" for name in LEGAL_ROUTES]
    for rel in paths:
        source = DOCS / rel
        if not source.is_file():
            raise FileNotFoundError(f"Required original HP asset not found: {source}")
        target = OUTPUT / rel
        target.parent.mkdir(parents=True, exist_ok=True)
        shutil.copyfile(source, target)

    # The live old HP remains closed. Create a root that opens the ORIGINAL
    # music room and never serves the rejected Canvas / town prototype.
    (OUTPUT / "index.html").write_text(
        '<!doctype html><html lang="ja"><meta charset="utf-8">'
        '<meta name="viewport" content="width=device-width,initial-scale=1">'
        '<meta name="robots" content="noindex,nofollow">'
        '<meta http-equiv="refresh" content="0;url=/music/">'
        '<title>DOLZORE MUSIC</title>'
        '<a href="/music/">DOLZORE JUKEBOXを開く</a></html>',
        encoding="utf-8",
    )
    # Safe until final authorization; never advertise unfinished checkouts.
    (OUTPUT / "robots.txt").write_text(
        "User-agent: *\nDisallow: /\n", encoding="utf-8"
    )
    music = OUTPUT / "music/index.html"
    music_html = music.read_text(encoding="utf-8")
    old_cta = ('<a class="purchase-v6" data-purchase href="#" '
               'target="_blank" rel="noopener noreferrer">')
    if old_cta not in music_html:
        raise AssertionError("Original purchase control changed: review export guard")
    music_html = music_html.replace(
        old_cta, '<div class="purchase-v6" data-purchase aria-disabled="true">'
    ).replace(
        '<span>BUY THIS TRACK</span><strong>¥200</strong></a>',
        '<span>販売再開準備中</span><strong>¥200</strong></div>',
    )
    music_html = music_html.replace(
        "20秒試聴して1曲¥200で購入できます。",
        "20秒試聴できます。購入の再開は確認中です。",
    ).replace(
        "商用・収益化動画OK。",
        "ライセンス条件を最終確認中。",
    ).replace(
        "1曲¥200・買い切り、商用・収益化動画でも使えます。",
        "1曲¥200・買い切りでの販売再開を準備中です。",
    )
    music_html = music_html.replace(
        '<main class="music-v6-main" data-jukebox>',
        '<main class="music-v6-main" data-jukebox><p role="status" '
        'style="margin:1rem auto;max-width:1120px;padding:1rem;background:#fff2bd;'
        'color:#261c30">販売再開準備中：試聴・表示の確認ページです。'
        '購入後の提供方法と販売権利を確認するまで、決済は受け付けません。</p>',
    )
    music.write_text(music_html, encoding="utf-8")
    if '販売再開準備中' not in music_html:
        raise AssertionError("Not marked as a preview-only shop")

    script = OUTPUT / "site.js"
    source_js = script.read_text(encoding="utf-8")
    # Existing main original shop is missing the optional animation disc
    # on its HTML screen. Guard null instead of crashing on playback.
    source_js = source_js.replace("disc.classList", "disc?.classList")
    source_js = source_js.replace(
        "purchase.href=track.purchaseUrl;",
        'purchase.removeAttribute("href");'
        'purchase.setAttribute("aria-disabled","true");',
    )
    if "purchase.href=track.purchaseUrl;" in source_js:
        raise AssertionError("Live purchase binding escaped checkout guard")
    script.write_text(source_js, encoding="utf-8")

    for p in OUTPUT.rglob("*"):
        if p.is_file() and p.suffix.lower() in {".mp3", ".wav", ".flac", ".m4a"}:
            raise AssertionError(f"Unexpected public audio master: {p}")
    print(f"REUSING ORIGINAL HP docs/music: {len(tracks)} tracks; "
          f"{sum(bool(t.get('previewReady')) for t in tracks)} existing preview flags")
    print(f"READY_FOR_ISOLATED_PREVIEW={OUTPUT} / checkout=DISABLED")


if __name__ == "__main__":
    build()
