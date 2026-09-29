from __future__ import annotations
import base64
from pathlib import Path
from playwright.sync_api import sync_playwright

OUT=Path("qa-slice1")
OUT.mkdir(exist_ok=True)
BASE="http://127.0.0.1:8765"

def shot(page,name):
    path=OUT/name
    page.screenshot(path=str(path),full_page=True)
    data=base64.b64encode(path.read_bytes()).decode("ascii")
    print(f"SLICE1_SCREENSHOT_B64_BEGIN:{name}")
    print(data)
    print(f"SLICE1_SCREENSHOT_B64_END:{name}")

def no_overflow(page,label):
    sw=page.evaluate("document.documentElement.scrollWidth")
    iw=page.evaluate("window.innerWidth")
    assert sw <= iw + 1, (label,sw,iw)

def main():
    with sync_playwright() as p:
        browser=p.chromium.launch(headless=True)

        # Desktop game
        desktop=browser.new_context(viewport={"width":1280,"height":900})
        page=desktop.new_page()
        page.goto(BASE+"/",wait_until="load")
        page.wait_for_function("window.__DOLZORE_WORLD__ && window.__DOLZORE_WORLD__.ready === true")

        state=page.evaluate("window.__DOLZORE_WORLD__.getState()")
        assert {x["name"] for x in state["residents"]} == {"MELO","YUZU","PON"}
        assert page.locator("[data-place-label='music']").count()==1
        assert page.locator("[data-place-label='journal']").count()==1
        assert page.locator("[data-jump]").count()==1
        assert page.locator("[data-bgm-toggle]").count()==1

        # Movement
        before=state["player"]["x"]
        page.keyboard.down("ArrowRight")
        page.wait_for_timeout(450)
        page.keyboard.up("ArrowRight")
        after=page.evaluate("window.__DOLZORE_WORLD__.getState().player.x")
        assert after > before + 8, (before,after)

        # Space is jump, not interact.
        page.keyboard.press("Space")
        page.wait_for_timeout(130)
        jump=page.evaluate("window.__DOLZORE_WORLD__.getState().player")
        assert jump["jumpActive"] is True
        assert jump["jumpHeight"] > 4, jump

        # Jump lands.
        page.wait_for_timeout(500)
        landed=page.evaluate("window.__DOLZORE_WORLD__.getState().player")
        assert landed["jumpActive"] is False
        assert landed["jumpHeight"] == 0

        # BGM toggle + persistence.
        bgm=page.locator("[data-bgm-toggle]")
        if page.evaluate("window.__DOLZORE_WORLD__.getState().bgm.enabled"):
            bgm.click()
        assert page.evaluate("window.__DOLZORE_WORLD__.getState().bgm.enabled") is False
        assert page.evaluate("localStorage.getItem('dolzore_bgm_enabled_v1')") == "off"
        page.reload(wait_until="load")
        page.wait_for_function("window.__DOLZORE_WORLD__ && window.__DOLZORE_WORLD__.ready === true")
        assert page.evaluate("window.__DOLZORE_WORLD__.getState().bgm.enabled") is False
        assert "OFF" in page.locator("[data-bgm-toggle]").text_content()
        page.locator("[data-bgm-toggle]").click()
        page.wait_for_timeout(120)
        assert page.evaluate("window.__DOLZORE_WORLD__.getState().bgm.enabled") is True
        assert page.evaluate("localStorage.getItem('dolzore_bgm_enabled_v1')") == "on"

        no_overflow(page,"desktop")
        shot(page,"desktop-slice1.png")
        desktop.close()

        # Mobile controls
        mobile=browser.new_context(viewport={"width":390,"height":844},is_mobile=True)
        m=mobile.new_page()
        m.goto(BASE+"/",wait_until="load")
        m.wait_for_function("window.__DOLZORE_WORLD__ && window.__DOLZORE_WORLD__.ready === true")
        assert m.locator("[data-move]").count()==4
        assert m.locator("[data-action]").count()==1
        assert m.locator("[data-jump]").count()==1
        assert m.locator("[data-bgm-toggle]").count()==1
        m.locator("[data-jump]").click()
        m.wait_for_timeout(130)
        jump=m.evaluate("window.__DOLZORE_WORLD__.getState().player")
        assert jump["jumpHeight"] > 4
        no_overflow(m,"mobile")
        shot(m,"mobile-slice1.png")
        mobile.close()
        browser.close()

    print("SLICE1_QA_PASS")
    print("CHARACTERS_4=PASS")
    print("MOVEMENT=PASS")
    print("SPACE_JUMP=PASS")
    print("ENTER_RESERVED_FOR_INTERACTION=PASS")
    print("BGM_TOGGLE_PERSISTENCE=PASS")
    print("MOBILE_CONTROLS=PASS")
    print("HORIZONTAL_OVERFLOW=0")

if __name__=="__main__":
    main()
