# DOLZORE AVATAR / DRESS-UP SYSTEM V2

Authority: 2026-09-30 JST
Resume phrase: **Gameひきついで**

## Product goal

Avatar customization is a first-class long-term attraction for DOLZORE.

The character system must support players who enjoy:
- choosing hair / body / face;
- coordinating tops / bottoms / shoes;
- collecting accessories;
- changing colors;
- building social identity through appearance;
- keeping a recognizable personal silhouette at small field scale.

The system must remain original DOLZORE art. It may learn high-level small-sprite readability principles from MOTHER2 research but must not copy protected characters, costumes, sprites, palettes, poses, or pixel arrangements.

## Canonical implementation

Unity source:
- `game/unity/bootstrap/Assets/Scripts/AvatarSystem.cs`
- `game/unity/bootstrap/Assets/Editor/AvatarAssetGenerator.cs`
- `game/unity/bootstrap/Assets/Editor/AvatarQualityVerifier.cs`

Do not create a second competing avatar renderer.

Earlier duplicate experimental files were removed. The canonical path is the existing `AvatarSystem.cs` pipeline.

## Native format

- 32 x 40 pixel source canvas
- four directions: down / left / right / up
- two walk frames per direction
- eight directional/movement states
- Point filtering / pixel rendering
- appearance data separate from physics collider
- runtime renderer changes facing from player movement

## Appearance dimensions

`AvatarAppearanceData` stores:
- bodyShape
- faceStyle
- hairStyle
- topStyle
- bottomStyle
- shoeStyle
- accessoryStyle
- accentStyle
- skin color
- hair color
- top color
- bottom color
- shoe color
- accessory color
- accent color

Current combinatorial space before color variants:

`3 * 4 * 8 * 6 * 5 * 4 * 9 * 7 = 725,760`

Color variants increase the effective space far beyond this.

## Canonical core profiles

### SORA
- soft rounded dark hair
- mint short jacket
- dark tapered trousers
- off-white sneakers
- cream diagonal strap
- coral pouch
- visual behavior: balanced, curious, neutral expression

### MELO
- asymmetric auburn bob
- burgundy / rose layered upper body
- skirt / leggings silhouette
- record-sleeve bag
- amber detail
- back-facing bag placement must remain visibly different from front

### YUZU
- broad wavy dark hair
- mustard cardigan
- long olive lower silhouette
- off-white notebook
- calm expression

### PON
- tousled blue/slate hair
- blue short jacket
- orange scarf
- cargo/tapered lower silhouette
- large tan backpack
- widest motion/silhouette contrast among the core four

## V5 quality changes

The old avatar art was rejected because it looked like the same blocky person with color swaps.

V5 implemented:
- tapered shoulder-to-waist torso instead of rectangle body;
- smaller, more readable shoes;
- separate silhouettes for pants / skirt+leggings / long skirt / cargo / shorts;
- stronger sleeve/hand poses;
- one-pixel eyes instead of square black eye blocks;
- eyebrows / nose-shadow / cheek highlights;
- expression variants;
- stronger hairstyle mass differences;
- ground/contact shadow;
- distinct front/side/back treatment;
- back-specific hair shading;
- back-specific bag position for MELO;
- movement-frame asymmetry.

## Automated quality gates

Latest green run:
`36662901682 = SUCCESS`

Generated-source commit:
`fcb9e81348084c6f60268dfb4a23e824287ea488`

Avatar quality receipt:
`BuildArtifacts/avatar-quality.json`

Current green checks:
- SORA silhouette unique
- MELO silhouette unique
- YUZU silhouette unique
- PON silhouette unique
- down vs side visual difference
- down vs up visual difference
- walk-frame visual difference
- head/whole-body pixel ratio
- canonical hair/top/bottom/accessory differences
- wardrobe combination-count floor

Measured head ratios:
- SORA 0.404
- MELO 0.441
- YUZU 0.450
- PON 0.389

Representative motion differences:
- SORA down/side 94 px, down/up 63 px, walk 98 px
- MELO down/side 70 px, down/up 231 px, walk 76 px
- YUZU down/side 65 px, down/up 56 px, walk 38 px
- PON down/side 72 px, down/up 283 px, walk 84 px

## Wardrobe review artifact

Unity generates:
`BuildArtifacts/avatar-showcase.png`

It currently displays:
- SORA
- MELO
- YUZU
- PON
- sporty sample
- street sample
- soft sample
- utility sample

The showcase must remain part of visual QA whenever avatar grammar changes.

## Crosswalk / street alignment

Crosswalks are no longer free-positioned decorative sprites.

Current implementation:
- dedicated `Road Markings` Tilemap
- horizontal / vertical crosswalk marking tiles
- placed in road grid coordinates
- `FirstTownAcceptanceVerifier.AssertRoadMarkings()` verifies every marking cell also contains road
- acceptance receipt is only written after that assertion succeeds

This prevents the visible off-road drift seen in the rejected screenshot.

## Next avatar finish lane

Do not expand random cosmetics before the base character appeal is accepted.

Next:
1. in-game character creator / wardrobe UI;
2. save/load appearance using the existing persistent player state;
3. color selection with curated palette families;
4. hair thumbnails and outfit previews;
5. equip / unequip accessory preview;
6. rotate/facing preview;
7. live walk preview;
8. additional facial/skin/body options;
9. emotes / idle poses;
10. collectible cosmetic inventory only after the editor flow feels good.

Quality principle:
**A player should want the base avatar before cosmetics are used as reward.**
