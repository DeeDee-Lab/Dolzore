# FirstTown Stylized-Real Reset

Authority: direct user rejection of current public 3D art direction, 2026-10-03/04 JST
Branch: rebuild/firsttown-stylized-real-20261004
Status: ACTIVE

## Rejection reason
The rejected build mixed:
- roughly 3-head/chibi body proportions,
- non-cute faces/body language,
- semi-real medieval/fantasy architecture,
- low-poly/toy-like surface treatment.

That combination produced an uncanny middle state. Adding more props does not solve it.

## New target
DOLZORE FirstTown moves to a coherent FF11-like stylized-real direction.

### Character
- production baseline: approximately 6.5–7.5 head humanoid proportions;
- no chibi baseline;
- readable but restrained facial stylization;
- fantasy outfits and silhouettes;
- consistent skeleton/animation scale;
- player/NPC share one visual realism band.

### Environment
- no bright toy-village palette;
- muted stone / plaster / timber / iron / cloth materials;
- low/mid-poly geometry with texture-driven detail;
- stronger real-world scale relationships;
- streets, doors, stairs and furniture sized against the new humanoid baseline;
- modular reuse remains important, but repeated kit pieces must not read as prefab spam.

### FF11 lessons retained
- lightweight reusable geometry;
- texture-driven richness;
- selective detail density;
- strong silhouettes;
- streaming / culling / asset budget discipline.

### Visual acceptance
Reject before user review if any of these are true:
- character reads as chibi without deliberate cute art direction;
- character realism and town realism are visibly mismatched;
- town reads as a toy/model kit;
- saturated primary roofs/grass dominate the frame;
- a single asset pack identity is immediately obvious;
- character-to-door / character-to-road scale feels wrong;
- screenshot looks like a prototype asset test instead of a commercial RPG scene.

## Baseline rule
The prior public preview is REJECTED and cannot become the main HP release.

The new branch starts from the stronger pre-regression composition baseline and replaces the art grammar, not merely the prop count.

CURRENT_PUBLIC_3D_VISUAL_REJECTED=true
STYLIZED_REAL_RESET_ACTIVE=true
TARGET_CHARACTER_HEAD_RATIO=6.5_TO_7.5
CHIBI_BASELINE_ALLOWED=false
FIRST_TOWN_COMPLETE=false
NO_FALSE_COMPLETION=true
