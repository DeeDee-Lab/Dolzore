# MOTHER2 VISUAL / CHARACTER / EFFECT GRAMMAR

Authority: DOLZORE MOTHER2 research issue #26
Purpose: extract visual-system rules for original Unity implementation.

## 1. Visual thesis

MOTHER2's visual effectiveness does not come from high pixel detail. It comes from:
- strong symbolic readability;
- disciplined color masses;
- memorable silhouettes;
- consistent scale relationships;
- deliberate non-realism;
- high reuse of compact art material;
- local exceptions used for emotional emphasis.

CREATOR_INTERVIEW / Koichi Oyama:
The art direction intentionally did not chase literal realism. Roads, plants and urban details could be simplified or ecologically inconsistent if the screen read well.

DOLZORE rule:
"Believable" means internally coherent and readable, not physically exact.

## 2. Sprite role hierarchy

Do not give every resident equal visual complexity.

Recommended original DOLZORE tiers:

### Tier A — player / principal cast
- strongest silhouette;
- unique accessory;
- broad animation set;
- multi-state emotional/event variants;
- highest palette isolation from environment.

### Tier B — recurring named NPC
- recognizable silhouette and color block;
- limited directional animation;
- one signature prop or garment mass;
- state variants as needed.

### Tier C — local resident
- reusable body grammar;
- changed hair/head/upper/lower color masses;
- small behavior animation;
- readable occupation/social role.

### Tier D — ambient object/creature
- highly economical frames;
- motion/placement creates identity.

This is an original production taxonomy.

## 3. Technical sprite facts from community reverse engineering

CoilSnake's field sprite model shows:
- sprite groups separated from map placement;
- palettes are separately referenced;
- sprite groups contain width/height and directional assets;
- North/South and East/West collision dimensions are stored separately;
- horizontal mirroring can reuse art;
- multiple sprite size classes exist rather than one universal canvas.

Interpretation:
Character appearance, collision and placement are data-driven and decoupled.

DOLZORE Unity:
CharacterDefinition
  - sprite_profile
  - palette/material profile
  - animation_profile
  - collider_profile_by_facing
  - interaction_anchor
  - shadow_profile
  - behavior_profile

Do not tie collider automatically to sprite bounds.

## 4. Readability at field scale

A good small sprite should be identified first by:
1. overall silhouette;
2. head/hair/hat mass;
3. torso color block;
4. accessory/prop;
5. movement signature;
6. facial pixels last.

Do not depend on tiny facial details for identity.

DOLZORE first-town acceptance:
At gameplay zoom, major NPCs should be distinguishable in a grayscale silhouette test and in a color-block test.

## 5. Character-to-world scale

Existing DOLZORE design already treats door/car/sidewalk/character scale as one shared system.

MOTHER2 reference principle:
The world reads because people, cars, houses, street furniture and road widths occupy a coherent symbolic scale.

Unity validation:
For every character sheet, capture comparison frames next to:
- standard door;
- road lane;
- bench;
- vehicle;
- tree;
- shop counter.

Reject assets that require one-off world rescaling.

## 6. Animation grammar

MOTHER2-era sprite production demonstrates that identity can survive with few frames when:
- pose silhouettes are clear;
- facing change is readable;
- feet/body alternation is rhythmic;
- motion speed matches step cadence;
- animation is synchronized with world displacement.

DOLZORE recommendations:
- use minimal walk animation only if cadence looks intentional;
- separate idle from walk;
- support direction-aware interactions;
- reserve special animations for memorable actions;
- avoid adding high-frame-count animation that clashes with low-frequency world art.

Exact MOTHER2 frame counts are not a DOLZORE target.

## 7. Color strategy

Use palette families per district, not arbitrary object-by-object colors.

Recommended hierarchy:
- ground/background family;
- architecture family;
- vegetation family;
- signage/accent family;
- resident palette family;
- interactive highlight family.

Characters require local contrast against each district.

Avoid copying MOTHER2's specific greens, reds, neons or character colors.

## 8. Tile-art economy

CREATOR_INTERVIEW + COMMUNITY_REVERSE_ENGINEERING:
- art director discusses reuse of very small graphic materials;
- CoilSnake models 8x8 graphic minitiles assembled into larger arrangements;
- collisions are associated separately.

Reusable production principle:
Create a small high-quality vocabulary and compose it, instead of drawing every block uniquely.

DOLZORE Unity asset hierarchy:
8/16px micro-materials
 -> 16/32px tiles
 -> modular architecture pieces
 -> prefabs
 -> district compositions
 -> authored exceptions.

Even if DOLZORE uses 16x16 as its authoring base, preserve modular reuse.

## 9. Reuse without sameness

Asset reuse should be hidden by:
- composition;
- palette family;
- prop clusters;
- facade rhythm;
- roof/eave variation;
- vegetation;
- signs;
- NPC population;
- lighting;
- audio;
- state changes.

Do not solve variety by making hundreds of unique low-quality tiles.

## 10. Architecture

Symbolic architecture communicates function with:
- silhouette;
- entrance treatment;
- window rhythm;
- signage;
- local prop cluster;
- roof/eave shape;
- foreground boundary.

DOLZORE rule:
Each service type gets a visual semantic, but each town can reinterpret it.

## 11. Environmental props

Props should have one of five purposes:
- orient;
- imply life/use;
- block/shape route;
- invite interaction;
- deliver humor/oddity.

A prop with none of these functions is visual noise.

Recommended prop-cluster approach:
Create authored clusters such as:
- curb + drain + weeds;
- bench + bin + poster;
- bicycle + wall sign;
- vending machine + crates;
- utility pole + cable shadow.

Never copy exact MOTHER2 prop compositions.

## 12. Special-space visual transformation

CREATOR_INTERVIEW:
The development team could produce a dramatic alternate-world feel by transforming familiar material instead of drawing an entirely new asset library.

Reusable principle:
For DOLZORE's time-layer/ZURE spaces, transform familiar geometry through:
- palette remap;
- material/shader change;
- edge treatment;
- selective sprite substitution;
- animated overlay;
- parallax/warping;
- altered prop semantics;
- changed audio.

Do not mimic Moonside.

## 13. Battle presentation

COMMUNITY_REVERSE_ENGINEERING:
Battle backgrounds are composed from graphic tiles, palette/arrangement data and separate scrolling/distortion parameters. Reverse-engineering projects reproduce them using layered procedural/scanline-style distortions rather than frame-by-frame full-screen animation.

Design lesson:
High-impact battle motion can be generated from compact parameterized visual systems.

DOLZORE battle-background architecture:
BattleBackdropDefinition
- base_texture_family;
- layer_count;
- palette_profile;
- uv_scroll[];
- distortion_curve[];
- pulse[];
- event_reactivity;
- intensity;
- accessibility_reduced_motion_variant.

Do not use MOTHER2 patterns, palettes or exact distortion formulas.

## 14. Effect grammar

Separate effects into semantic families:

### Confirm/positive
Short, clean, high-contrast.
Use for:
- successful interaction;
- heal;
- discovery;
- item gain.

### Threat/damage
Fast onset, readable center, short decay.
Never obscure state-critical UI.

### Psychic/magic-like
DOLZORE must use original visual language.
Possible original directions:
- waveform;
- geometric resonance;
- ink/smoke;
- chromatic splitting;
- time offset/ZURE echo.

### World anomaly
Prefer environment-wide material/palette/geometry response over generic particle spam.

## 15. UI relationship to graphics

MOTHER2 uses strong window/UI contrast against decorative field/battle imagery.

Reusable principle:
Gameplay information should remain legible even when the world is visually strange.

DOLZORE:
- reserve stable UI value ranges;
- use consistent typography;
- never let battle effects mask critical health/resources;
- allow world art to be expressive while HUD remains functional.

Do not reproduce exact MOTHER2 windows, borders, type arrangement or menu positions.

## 16. Shadows

MOTHER2 is a stylized reference, not a physically based lighting target.

DOLZORE currently requires:
- coherent single world light direction;
- short contact/cast shadows;
- separate building side planes from shadows;
- character contact shadows.

Do not derive shadow geometry from MOTHER2 screenshots literally.

## 17. Character design workflow

For each important DOLZORE character:
1. written role;
2. 6 silhouette thumbnails;
3. grayscale selection;
4. color block pass;
5. gameplay-scale sprite;
6. directional walk;
7. door/car/world scale test;
8. interaction pose;
9. night/palette test;
10. screenshot review.

Do not start from a MOTHER2 sprite and alter it.

## 18. Quality gates

Fail visual pass if:
- major residents share nearly identical silhouettes;
- identity disappears without facial pixels;
- character is out of scale with architecture;
- palette blends into common backgrounds;
- collider follows hair/hat shape instead of body footprint;
- effects obscure state information;
- special spaces are only recolors with no behavioral/audio change;
- reused architecture becomes obvious repetition;
- final art depends on protected reference assets.

## 19. Source anchors

Primary:
- Hobonichi Koichi Oyama:
  https://www.1101.com/mother_project/entry/archives/MOTHER_them/oyama.html
- Hobonichi MOTHER2 30th anniversary materials:
  https://www.1101.com/n/s/mother_project/mother2_himitsu_book/
- Nintendo official screenshots:
  https://www.nintendo.co.jp/n08/a2uj/mother2/screen/index.html

Technical:
- CoilSnake SpriteGroupModule / model.eb.sprites
- CoilSnake TilesetModule / model.eb.map_tilesets
- CoilSnake BattleBgModule
  https://github.com/pk-hack/CoilSnake
