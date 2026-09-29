from pathlib import Path
import base64
from playwright.sync_api import sync_playwright

BASE="http://127.0.0.1:8765"
OUT=Path("qa-first-town")
OUT.mkdir(exist_ok=True)

DISTRICTS=[
    ("RESIDENTIAL HILL",(360,350)),
    ("CENTRAL MAIN STREET",(700,500)),
    ("MARKET / WORKSHOP",(1100,300)),
    ("CIVIC / JOURNAL",(450,740)),
    ("RIVERSIDE",(565,860)),
    ("STATION / EAST GATE",(1330,835)),
    ("BACK ALLEY",(845,300)),
]

def no_overflow(page,label):
    sw=page.evaluate("document.documentElement.scrollWidth")
    iw=page.evaluate("window.innerWidth")
    assert sw <= iw + 1, (label,sw,iw)

def shot(page,name,full_page=False):
    path=OUT/name
    page.screenshot(path=str(path),full_page=full_page)
    data=base64.b64encode(path.read_bytes()).decode("ascii")
    print(f"FIRST_TOWN_SCREENSHOT_B64_BEGIN:{name}")
    print(data)
    print(f"FIRST_TOWN_SCREENSHOT_B64_END:{name}")

def main():
    with sync_playwright() as p:
        browser=p.chromium.launch(headless=True)
        desktop=browser.new_context(viewport={"width":1280,"height":900})
        page=desktop.new_page()
        page.goto(BASE+"/",wait_until="load")
        page.wait_for_function("window.__DOLZORE_WORLD__ && window.__DOLZORE_WORLD__.ready === true")

        state=page.evaluate("window.__DOLZORE_WORLD__.getState()")
        assert state["world"]["width"] == 1536
        assert state["world"]["height"] == 1152
        assert state["world"]["viewportWidth"] == 480
        assert state["world"]["viewportHeight"] == 270
        assert state["district"] == "RESIDENTIAL HILL"

        place_ids={p["id"] for p in state["places"]}
        for required in ("bar","journal","river","station","market","home","alley"):
            assert required in place_ids

        for district,(x,y) in DISTRICTS:
            page.evaluate("([x,y])=>window.__DOLZORE_WORLD__.teleport(x,y)",[x,y])
            page.wait_for_timeout(80)
            s=page.evaluate("window.__DOLZORE_WORLD__.getState()")
            assert s["district"] == district, (district,s["district"],x,y)
            badge=page.locator("[data-district-name]").text_content().strip()
            assert badge == district, (district,badge)
            safe=district.lower().replace(" / ","-").replace(" ","-")
            shot(page,f"district-{safe}.png")

        page.evaluate("window.__DOLZORE_WORLD__.teleport(1330,835)")
        s=page.evaluate("window.__DOLZORE_WORLD__.getState()")
        assert s["camera"]["x"] > 700
        assert s["camera"]["y"] > 500

        page.evaluate("window.__DOLZORE_WORLD__.teleport(700,500)")
        before=page.evaluate("window.__DOLZORE_WORLD__.getState().player.x")
        page.keyboard.down("ArrowRight")
        page.wait_for_timeout(350)
        page.keyboard.up("ArrowRight")
        after=page.evaluate("window.__DOLZORE_WORLD__.getState().player.x")
        assert after > before + 5, (before,after)

        page.keyboard.press("Space")
        page.wait_for_timeout(130)
        jump=page.evaluate("window.__DOLZORE_WORLD__.getState().player")
        assert jump["jumpActive"] is True
        assert jump["jumpHeight"] > 3

        assert page.locator("[data-bgm-toggle]").count() == 1
        assert page.locator("[data-action]").count() == 1
        no_overflow(page,"desktop")
        shot(page,"desktop-first-town.png",full_page=True)
        desktop.close()

        mobile=browser.new_context(viewport={"width":390,"height":844},is_mobile=True)
        m=mobile.new_page()
        m.goto(BASE+"/",wait_until="load")
        m.wait_for_function("window.__DOLZORE_WORLD__ && window.__DOLZORE_WORLD__.ready === true")
        assert m.locator("[data-move]").count() == 4
        assert m.locator("[data-jump]").count() == 1
        assert m.locator("[data-action]").count() == 1
        assert m.locator("[data-bgm-toggle]").count() == 1
        m.evaluate("window.__DOLZORE_WORLD__.teleport(565,860)")
        m.wait_for_timeout(80)
        assert m.evaluate("window.__DOLZORE_WORLD__.getState().district") == "RIVERSIDE"
        no_overflow(m,"mobile")
        shot(m,"mobile-first-town.png",full_page=True)
        mobile.close()
        browser.close()

    print("FIRST_TOWN_QA_PASS")
    print("WORLD_SIZE_1536x1152=PASS")
    print("SCROLLING_CAMERA=PASS")
    print("DISTRICTS_7=PASS")
    print("LANDMARKS_BAR_JOURNAL_RIVER_STATION=PASS")
    print("MOVE_JUMP_BGM_CONTROLS=PASS")
    print("MOBILE_CONTROLS=PASS")
    print("HORIZONTAL_OVERFLOW=0")

if __name__=="__main__":
    main()
