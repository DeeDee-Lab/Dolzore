from __future__ import annotations
import json
from pathlib import Path
from playwright.sync_api import sync_playwright, TimeoutError as PlaywrightTimeoutError

ROOT = Path(__file__).resolve().parents[1]
OUT = ROOT / "qa-screenshots"
BASE = "http://127.0.0.1:8765"

def no_overflow(page, label: str) -> None:
    ok = page.evaluate("document.documentElement.scrollWidth <= window.innerWidth + 1")
    if not ok:
        sw = page.evaluate("document.documentElement.scrollWidth")
        iw = page.evaluate("window.innerWidth")
        raise AssertionError(f"{label}: horizontal overflow scrollWidth={sw} innerWidth={iw}")

def screenshot(page, name: str) -> None:
    page.screenshot(path=str(OUT / name), full_page=True)

def preview(page, name: str) -> None:
    page.screenshot(path=str(OUT / name), full_page=False, type="jpeg", quality=45)

def main() -> None:
    OUT.mkdir(exist_ok=True)
    tracks = json.loads((ROOT / "docs/data/tracks.json").read_text(encoding="utf-8"))["tracks"]
    weekend = next(t for t in tracks if t["id"] == "BGM-005")
    bright = next(t for t in tracks if t["id"] == "BGM-001")

    with sync_playwright() as p:
        browser = p.chromium.launch(headless=True)

        desktop = browser.new_context(viewport={"width": 1440, "height": 1000})
        page = desktop.new_page()
        page.goto(BASE + "/", wait_until="load")
        page.wait_for_timeout(800)
        assert "DOLZORE" in page.title()
        assert page.locator("text=ここは DOLZORE。").count() == 1
        assert page.locator("text=いま買えるものは、音楽だけ。").count() == 1
        assert page.locator('a[href="music/"]').count() >= 1
        assert page.locator("nav").get_by_text("ABOUT", exact=True).count() == 0
        assert page.locator(".town-pixel-art").count() == 1
        no_overflow(page, "desktop home")
        screenshot(page, "desktop-home.png")
        preview(page, "desktop-home-preview.jpg")

        page.goto(BASE + "/music/", wait_until="load")
        page.wait_for_function("document.querySelectorAll('.track-choice').length === 60")
        assert page.locator(".track-choice").count() == 60
        assert page.locator(".jukebox-machine-v4").count() == 1
        assert page.locator(".jukebox-pixel-art").count() == 1
        assert page.locator(".songbook-v4").count() == 1
        no_overflow(page, "desktop music")

        search = page.locator("[data-track-search]")
        search.fill("Weekend Drive")
        page.wait_for_function("document.querySelectorAll('.track-choice').length === 1")
        page.locator(".track-choice").click()
        assert page.locator("[data-purchase]").get_attribute("href") == weekend["purchaseUrl"]
        assert page.locator("[data-track-title]").text_content().strip() == "Weekend Drive"
        screenshot(page, "desktop-jukebox-weekend-drive.png")
        preview(page, "desktop-jukebox-preview.jpg")

        page.locator("[data-play]").click()
        try:
            page.wait_for_function(
                "document.querySelector('[data-audio]').currentTime > 0.15",
                timeout=15000,
            )
        except PlaywrightTimeoutError:
            status = page.locator("[data-status]").text_content()
            src = page.locator("[data-audio]").get_attribute("src")
            raise AssertionError(f"proven preview failed to play; status={status!r} src={src!r}")
        assert "試聴中" in page.locator("[data-status]").text_content()
        page.locator("[data-stop]").click()

        search.fill("Bright Morning")
        page.wait_for_function("document.querySelectorAll('.track-choice').length === 1")
        page.locator(".track-choice").click()
        assert page.locator("[data-purchase]").get_attribute("href") == bright["purchaseUrl"]
        page.locator("[data-play]").click()
        assert "準備中" in page.locator("[data-status]").text_content()
        audio_src = page.locator("[data-audio]").get_attribute("src")
        assert not audio_src, f"pending preview must not invent a source: {audio_src}"
        desktop.close()

        mobile = browser.new_context(viewport={"width": 390, "height": 844}, is_mobile=True)
        m = mobile.new_page()
        m.goto(BASE + "/", wait_until="load")
        m.wait_for_timeout(500)
        no_overflow(m, "mobile home")
        screenshot(m, "mobile-home.png")
        preview(m, "mobile-home-preview.jpg")

        m.goto(BASE + "/music/", wait_until="load")
        m.wait_for_function("document.querySelectorAll('.track-choice').length === 60")
        no_overflow(m, "mobile music")
        screenshot(m, "mobile-music.png")
        preview(m, "mobile-music-preview.jpg")

        for path in ("/journal/", "/journal/projector-150inch-4k-2026/", "/support/", "/legal/", "/privacy/", "/terms/", "/refund/", "/tokusho/", "/disclosure/"):
            m.goto(BASE + path, wait_until="load")
            assert m.locator("body").count() == 1
            no_overflow(m, "mobile " + path.strip("/"))

        m.goto(BASE + "/journal/projector-150inch-4k-2026/", wait_until="load")
        m.wait_for_function("document.querySelectorAll('[data-product-card]').length === 10")
        assert m.locator("[data-product-card]").count() == 10
        assert m.locator("[data-smartbuy-market]").count() == 1
        assert m.locator("[data-market-targets] .market-target-card").count() >= 6

        # Official manufacturer photos are primary; pixel art is fallback.
        m.wait_for_timeout(12000)
        photo_state = m.locator(".official-product-photo").evaluate_all(
            "(els)=>els.map(i=>({src:i.src,complete:i.complete,naturalWidth:i.naturalWidth,naturalHeight:i.naturalHeight}))"
        )
        loaded_photos = sum(1 for x in photo_state if x["complete"] and x["naturalWidth"] > 0)
        print("OFFICIAL_PHOTO_STATE="+json.dumps(photo_state,ensure_ascii=False))
        print(f"OFFICIAL_PRODUCT_PHOTO_COUNT={loaded_photos}")
        assert loaded_photos >= 8, photo_state

        # Product filters must visibly reduce and restore the card set.
        m.locator('[data-filter="short"]').click()
        m.wait_for_timeout(100)
        visible_short = m.locator("[data-product-card]:visible").count()
        assert 1 <= visible_short < 10, visible_short
        m.locator('[data-filter="all"]').click()
        assert m.locator("[data-product-card]:visible").count() == 10

        # Throw calculator must react to screen-size changes.
        before = m.locator("[data-throw-result]").text_content()
        m.locator("[data-screen-size]").evaluate("(el)=>{el.value='100';el.dispatchEvent(new Event('input',{bubbles:true}))}")
        after = m.locator("[data-throw-result]").text_content()
        assert before != after, (before, after)

        # Market target/search affordances must remain clickable even when strict candidates are zero.
        assert m.locator(".market-search-links a").count() >= 6
        assert m.locator(".product-actions a").count() >= 20
        no_overflow(m, "mobile projector journal")
        screenshot(m, "mobile-projector-journal.png")
        preview(m, "mobile-projector-journal-preview.jpg")
        mobile.close()
        browser.close()

    print("VISUAL_QA_PASS")
    print("TRACK_BUTTONS=60")
    print("PROVEN_PREVIEW_PLAYBACK=BGM-005_PASS")
    print("PENDING_PREVIEW_FAIL_CLOSED=BGM-001_PASS")
    print("DESKTOP_HOME=PASS")
    print("DESKTOP_MUSIC=PASS")
    print("MOBILE_HOME=PASS")
    print("MOBILE_MUSIC=PASS")
    print("MOBILE_PROJECTOR_JOURNAL=PASS")
    print("PROJECTOR_FILTERS=PASS")
    print("THROW_CALCULATOR=PASS")
    print("MARKET_AFFORDANCES=PASS")
    print("OFFICIAL_PRODUCT_PHOTOS=PASS")
    print("HORIZONTAL_OVERFLOW=0")

if __name__ == "__main__":
    main()
