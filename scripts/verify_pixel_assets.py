from pathlib import Path
from PIL import Image

EXPECTED={
    "docs/assets/dolzore-town.png":(960,540),
    "docs/assets/dolzore-jukebox.png":(480,720),
    "docs/assets/dolzore-town-game.png":(480,270),
    "docs/assets/dolzore-characters.png":(256,160),
    "docs/assets/dolzore-first-town.png":(1536,1152),
}
for raw,size in EXPECTED.items():
    p=Path(raw)
    assert p.exists(), f"MISSING_ASSET:{p}"
    with Image.open(p) as im:
        assert im.size==size, f"BAD_DIMENSIONS:{p}:{im.size}"
print("PIXEL_ASSET_DIMENSIONS_PASS")
