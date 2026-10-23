#!/usr/bin/env python3
"""Export ONLY the existing DOLZORE music shop to isolated Netlify hosting.

Preserves the original website files under docs/, never re-enables the
previously closed GitHub Pages / Unity legacy world, and never publishes
full paid MP3s. Existing Stripe checkout links remain authoritative and
are used directly without creating new payment pages.
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
        '<meta name="robots" content="index,follow">'
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

    # Restore the site's ORIGINAL buy controls and exact Stripe links, without
    # creating or altering checkout accounts, prices or digital delivery.
    if '<a class="purchase-v6" data-purchase href="#"' not in music_html:
        raise AssertionError("Original Stripe purchase CTA changed")

    # The music-only recovery must not point visitors into unexported Journal
    # pages or the retired world. Modify ONLY the generated files.
    music_html = re.sub(r'<a[^>]+href="../journal/"[^>]*>[^<]*</a>', '', music_html)
    music.write_text(music_html, encoding="utf-8")
    for name in LEGAL_ROUTES:
        legal_file = OUTPUT / name / "index.html"
        legal_html = legal_file.read_text(encoding="utf-8")
        legal_html = re.sub(r'<a[^>]+href="../journal/"[^>]*>[^<]*</a>', '', legal_html)
        legal_file.write_text(legal_html, encoding="utf-8")

    script = OUTPUT / "site.js"
    source_js = script.read_text(encoding="utf-8")
    # Existing main original shop is missing the optional animation disc
    # on its HTML screen. Guard null instead of crashing on playback.
    source_js = source_js.replace("disc.classList", "disc?.classList")
    if "purchase.href=track.purchaseUrl;" not in source_js:
        raise AssertionError("Existing Stripe purchase binding missing")
    script.write_text(source_js, encoding="utf-8")

    for p in OUTPUT.rglob("*"):
        if p.is_file() and p.suffix.lower() in {".mp3", ".wav", ".flac", ".m4a"}:
            raise AssertionError(f"Unexpected public audio master: {p}")
    print(f"REUSING ORIGINAL HP docs/music: {len(tracks)} tracks; "
          f"{sum(bool(t.get('previewReady')) for t in tracks)} existing preview flags")
    print(f"READY_FOR_DEPLOYMENT={OUTPUT} / original_Stripe_links=ENABLED")


if __name__ == "__main__":
    build()
