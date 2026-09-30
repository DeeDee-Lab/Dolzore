# DOLZORE AVATAR CUSTOMIZATION SYSTEM

Authority: 2026-09-30 JST
Engine: Unity 2D
Status: active implementation

## Goal

DOLZORE characters must be appealing enough that players want to:
- choose a look;
- change outfits;
- collect clothing;
- try hair/accessory combinations;
- recognize themselves and other players at small field scale.

Customization is cosmetic identity.
It must never alter:
- character ID;
- race/lineage stats;
- vocation/job;
- equipment combat math;
- FF11-derived internal progression.

## Visual hierarchy

A field avatar must read in this order:
1. overall silhouette;
2. hair/head mass;
3. torso/clothing mass;
4. lower-body silhouette;
5. signature accessory;
6. accent item;
7. face pixels.

Color alone is never enough to distinguish major characters.

## Native target

- authoring canvas: 32×40 px
- field display uses pixel-perfect nearest-neighbor rendering
- head occupies a large but not dominant portion of total silhouette
- collider remains independent from hair/clothes/accessories
- dark-indigo outline separates avatar from all district palettes

## Modular appearance data

Saved independently:
- skin tone;
- body shape;
- face style;
- hair style;
- hair color;
- top style;
- top color;
- bottom style;
- bottom color;
- shoe style;
- shoe color;
- accessory style/color;
- accent style/color.

Initial style families:
- 8 hair silhouettes;
- 6 tops;
- 5 bottoms;
- 4 shoes;
- 8+ accessories;
- 6+ accent items.

The combinatorial system must support many looks without requiring a new hand-authored full sprite sheet for every outfit.

## Direction and movement

Avatar rendering contract:
- Down
- Left
- Right
- Up
- 2 walk phases minimum
- idle frame
- later: talk / surprise / sit / carry / inspect

Clothing and hair must preserve identity across all directions.

## Canonical core presets

SORA:
- soft forelock;
- short mint jacket;
- tapered charcoal trousers;
- cream diagonal strap;
- coral pouch.

MELO:
- asymmetric bob;
- burgundy overshirt;
- deep-plum lower silhouette;
- record bag;
- amber hair clip.

YUZU:
- wide wavy hair;
- mustard cardigan;
- long olive lower silhouette;
- notebook;
- red bookmark accent.

PON:
- tousled blue-gray hair;
- blue jacket;
- wider lower stance;
- square tan backpack;
- orange scarf.

These are presets of the same customization system, not separate rendering technology.

## Attractive-avatar quality rules

Reject a revision if:
- all hairstyles share the same outer contour;
- all tops are rectangle recolors;
- accessories disappear at field scale;
- face pixels carry most identity;
- all bodies have one silhouette;
- outfit variation is visible only in RGB values;
- skin/hair/clothes blend into common town backgrounds;
- character scale is inconsistent with doors/cars/benches.

## Curated color policy

Use curated swatches rather than unrestricted RGB as the default UI.

Reason:
- combinations stay visually coherent;
- players still get substantial variety;
- field contrast remains predictable;
- screenshot quality is more consistent.

Advanced custom color can be considered later.

## Runtime architecture

Current implementation:
- `AvatarAppearanceData`
- `AvatarPresets`
- `AvatarPixelComposer`
- `AvatarRuntimeRenderer`
- editor `AvatarAssetGenerator`

Player save:
`PlayerPersistentStateData.appearance`

Appearance state is independent from RPG stats.

## Street-marking correction

Crosswalks must not be free-positioned sprites.

Canonical rule:
- road markings live on a dedicated `Road Markings` Tilemap;
- crosswalk cells align exactly with road cells;
- markings carry no collision;
- road/collision topology remains separate.

## Review artifacts

Every meaningful avatar pass should produce:
1. FirstTown screenshot at gameplay scale;
2. avatar showcase at enlarged nearest-neighbor scale;
3. core-four silhouette comparison;
4. wardrobe variation sample.

## Current implementation checkpoint

Active branch:
`rebuild/unity-first-town-20260929`

Implementation commits in progress:
- modular avatar runtime/data model;
- editor avatar generator;
- appearance saved independently of stats;
- town actors migrated to avatar presets;
- crosswalks migrated to road-grid Tilemap.

Do not call the avatar quality complete until the generated showcase and field screenshot are visually reviewed.
