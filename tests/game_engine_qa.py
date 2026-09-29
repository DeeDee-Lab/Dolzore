from pathlib import Path
import base64
from playwright.sync_api import sync_playwright

BASE="http://127.0.0.1:8765/game/"
OUT=Path("qa-game-engine")
OUT.mkdir(exist_ok=True)

def shot(page,name,full=False):
    path=OUT/name
    page.screenshot(path=str(path),full_page=full)
    data=base64.b64encode(path.read_bytes()).decode("ascii")
    print(f"DOLZORE_ENGINE_SCREENSHOT_B64_BEGIN:{name}")
    print(data)
    print(f"DOLZORE_ENGINE_SCREENSHOT_B64_END:{name}")

def state(page):
    return page.evaluate("window.__DOLZORE_GAME__.state()")

def main():
    with sync_playwright() as p:
        browser=p.chromium.launch(headless=True)

        desktop=browser.new_context(viewport={"width":1280,"height":900})
        page=desktop.new_page()
        page.goto(BASE,wait_until="load")
        page.wait_for_function("window.__DOLZORE_GAME__ && window.__DOLZORE_GAME__.ready === true")
        s=state(page)
        assert s["worldWidth"]==1280 and s["worldHeight"]==960, s

        # Player moves.
        before=s["x"]
        page.keyboard.down("d")
        page.wait_for_timeout(420)
        page.keyboard.up("d")
        after=state(page)["x"]
        assert after > before + 20, (before,after)

        # BAR collision: place player below BAR and push upward.
        page.evaluate("window.__DOLZORE_GAME__.teleport(568, 360)")
        page.keyboard.down("w")
        page.wait_for_timeout(650)
        page.keyboard.up("w")
        after_bar=state(page)
        assert after_bar["y"] >= 338, after_bar

        # Jump remains available.
        page.keyboard.press("Space")
        page.wait_for_timeout(100)

        # Full map.
        page.keyboard.press("m")
        page.wait_for_timeout(120)
        assert state(page)["mapOpen"] is True
        shot(page,"desktop-map.png")
        page.keyboard.press("m")
        assert state(page)["mapOpen"] is False

        # Status.
        page.keyboard.press("c")
        page.wait_for_timeout(120)
        assert state(page)["statusOpen"] is True
        shot(page,"desktop-status.png")
        page.keyboard.press("c")
        assert state(page)["statusOpen"] is False

        # Main visual near central street.
        page.evaluate("window.__DOLZORE_GAME__.teleport(620, 410)")
        page.wait_for_timeout(160)
        shot(page,"desktop-central.png")
        desktop.close()

        mobile=browser.new_context(viewport={"width":390,"height":844},is_mobile=True,has_touch=True)
        m=mobile.new_page()
        m.goto(BASE,wait_until="load")
        m.wait_for_function("window.__DOLZORE_GAME__ && window.__DOLZORE_GAME__.ready === true")
        assert m.locator("#mobile-controls").count()==1
        assert m.locator("[data-action=map]").count()==1
        assert m.locator("[data-action=jump]").count()==1
        assert m.locator("[data-action=interact]").count()==1
        m.evaluate("window.__DOLZORE_GAME__.teleport(512, 760)")
        m.wait_for_timeout(120)
        shot(m,"mobile-riverside.png",full=True)
        mobile.close()

        browser.close()

    print("GAME_ENGINE_QA_PASS")
    print("PHASER_BOOT=PASS")
    print("WORLD_1280x960=PASS")
    print("PLAYER_MOVEMENT=PASS")
    print("BUILDING_COLLISION=PASS")
    print("MAP_UI=PASS")
    print("STATUS_UI=PASS")
    print("MOBILE_CONTROLS=PASS")

if __name__=="__main__":
    main()
