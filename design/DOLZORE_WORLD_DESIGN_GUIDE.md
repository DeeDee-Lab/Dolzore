# DOLZORE WORLD DESIGN GUIDE

Last authority: 2026-09-29 JST

This file is a binding design guard for the public DOLZORE website.

## 1. Product truth

- Public paid product: **music only**.
- Public IA: **WORLD / MUSIC / JOURNAL / SUPPORT / LEGAL**.
- No ABOUT navigation.
- Journal may contain editorial research, but must not recreate a non-music DOLZORE checkout lane.

## 2. Reference intent

High-level references:
- Nintendo official MOTHER2 presentation / screenshots:
  https://topics.nintendo.co.jp/article/7a5ddbdb-dac7-468b-9c6c-3014dc7cb0b6
- HOBONICHI MOTHER PROJECT development interview:
  https://www.1101.com/n/s/mother_project/mother2_himitsu_book/2024-11-27.html
  https://www.1101.com/n/s/mother_project/mother2_himitsu_book/2024-11-28.html
- nob-sakuma.com for authored browsing / personality only.

These are **reference material, not source assets**.
Never copy characters, maps, sprites, dialogue, music, logos, exact UI, or page layouts.

## 3. The key distinction: retro != cheap

DOLZORE must not look like:
- flat CSS rectangles enlarged until they look blocky;
- generic "retro game" templates;
- children's construction-paper illustrations;
- giant pixel-font marketing headlines;
- random pastel colors with no material/shadow system;
- an ordinary product grid with pixel borders.

Pixel art must be **drawn on a low-resolution grid** and enlarged with nearest-neighbour rendering.

Current canonical raster assets:
- `docs/assets/dolzore-town.png`
- `docs/assets/dolzore-jukebox.png`

They are generated from:
- `scripts/render_pixel_assets.py`

## 4. Pixel-art construction rules

### Environment
A WORLD scene should include enough environmental information to feel inhabited:
- road + sidewalk / curb;
- multiple buildings with different functions;
- shop signage;
- vehicles;
- trees / vegetation;
- street furniture (bench, lamp, mailbox, vending machine, signs, etc.);
- tiny people / NPC scale cues;
- material-specific shadow/highlight;
- small asymmetry and incidental detail.

### Palette
Use restrained color ramps.
Each material should normally have:
- dark outline / deep shade;
- body color;
- lighter plane;
- optional highlight.

Do not solve depth with browser gradients as the primary art technique.

### Outlines
Dark main outlines are deliberate.
Do not outline every decorative detail equally.
Foreground/interactable silhouettes need the strongest separation.

### Scaling
Raster pixel art:
- authored at low resolution;
- enlarged with nearest-neighbour;
- CSS: `image-rendering: pixelated` / `crisp-edges`.

## 5. Typography

Pixel font is an accent:
- signage;
- labels;
- small UI;
- track number;
- menu language.

Large Japanese copy and long reading text should use a readable rounded/grotesque Japanese font.

Do not make the whole page a pixel font showcase.

## 6. Copy voice

DOLZORE copy must be:
- concrete;
- short;
- warm;
- lightly unusual when useful;
- understandable on first read.

It must NOT be pseudo-poetic or self-consciously "quirky".

Forbidden / retired copy:
- "音のとなりに、ことばを置く"
- "音のとなりに、読みものを"
- obsolete business-brand story about reducing repetitive work / app products
- copy explaining stopped non-music sales on public pages

Preferred pattern:
- say where the visitor is;
- say what they can do;
- give one small human detail;
- stop.

Example structure (not mandatory copy):
- "ここは DOLZORE。"
- "音楽屋と、掲示板のある小さな町です。"
- "いま買えるものは、音楽だけ。"
- "気になるほうから、どうぞ。"

Do not imitate Shigesato Itoi's exact prose or reproduce MOTHER dialogue.

## 7. MUSIC / jukebox

The machine must read immediately as a jukebox.

Required visual cues:
- arched cabinet;
- illuminated side tubes;
- glass/record chamber;
- physical song-label / ticket area;
- selection buttons;
- speaker grille;
- material contrast (wood/red body, metal/chrome/cream trim, glass, grille).

The 60-track selector should feel like physical selection labels, not an admin dashboard.

Functional truth remains:
- 60 tracks;
- 1 track ¥200;
- exact Stripe URLs;
- 20-second preview only where a proven preview exists;
- pending previews fail closed;
- no full paid audio in public repo/site.

## 8. JOURNAL

Journal is a town notice board / reading surface.

It should answer immediately:
"What is here?"
Not:
"What does this metaphor mean?"

Article cards may look like pinned paper/cards, but reading quality takes priority over decoration.

## 9. Acceptance gate

A visual redesign is not accepted from source code alone.

Required:
1. desktop browser screenshot;
2. mobile browser screenshot;
3. public URL fresh readback;
4. music interaction regression;
5. no horizontal overflow;
6. user-visible deployment.

If the result looks "retro because it is crude", the design has failed.

