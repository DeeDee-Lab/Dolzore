# DOLZORE Character System v1

Authority: 2026-09-29 redesign branch

## Reference analysis

Nintendo's official MOTHER2 screenshots show that small town characters remain readable because:
- the head is relatively large compared with the body;
- hair/hat and clothing create a strong silhouette;
- characters use a few large color masses rather than noisy detail;
- feet and cast shadow anchor the sprite to the road;
- every person shares the same world scale as cars, sidewalks, doors and trees.

The goal is to learn those high-level principles only. Do not copy MOTHER/EarthBound characters, outfits, faces, poses, palette, dialogue, or sprites.

## DOLZORE residents

### SORA — protagonist
- role: the visitor/player
- silhouette: rounded dark hair, short jacket, small diagonal shoulder bag
- colors: deep brown hair / teal jacket / cream bag / charcoal trousers / coral shoe accent
- personality: curious, quiet, moves first and asks later
- recognizer: diagonal cream bag crossing the torso

### MELO — music resident
- role: stands near MUSIC
- silhouette: short auburn hair with one asymmetric side tuft
- colors: burgundy outerwear / warm red / amber accessory / dark skirt-or-shorts silhouette
- personality: notices small differences in sound; direct and friendly
- recognizer: amber hair clip + burgundy block

### YUZU — journal resident
- role: stands near JOURNAL
- silhouette: dark wavy hair, mustard cardigan, long green lower silhouette
- colors: mustard / olive / charcoal / off-white
- personality: collects notes and odd facts
- recognizer: mustard upper body + olive lower body

### PON — wandering resident
- role: walks around the road
- silhouette: blue-violet hair, orange scarf, small backpack
- colors: slate blue / orange / navy / tan
- personality: always going somewhere, sometimes forgets where
- recognizer: bright orange scarf and square backpack

## Sprite constraints

- native sprite size: 24×32 px
- 4 directions: down / left / right / up
- 2 walk frames per direction
- 8 frames per character
- shadow drawn separately by the game renderer
- no facial detail smaller than 1px
- no black rectangle-body placeholders
- no copied baseball cap/red-yellow stripe/pink dress/glasses-labcoat/prince silhouette combinations from MOTHER2

## World scale

- internal game canvas: 480×270
- character native display: 24×32
- typical door opening: 22–28 px wide
- car body: ~45–55 px wide
- sidewalk: ~20 px deep
- road: ~72–84 px deep

## Building text

Never bake MUSIC / JOURNAL / CAFE text into low-resolution raster art.
Building names are crisp HTML overlays positioned over the game canvas.

## Acceptance

A character pass fails if, at normal browser zoom:
- a resident looks like a stain/dot;
- two residents cannot be distinguished by silhouette/color;
- a resident is out of scale with a door/car;
- building labels blur;
- the player cannot visibly move at least 24 px using keyboard or mobile controls.
