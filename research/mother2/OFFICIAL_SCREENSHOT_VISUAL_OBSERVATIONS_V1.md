# MOTHER2 OFFICIAL SCREENSHOT VISUAL OBSERVATIONS V1

Authority: DOLZORE MOTHER2 research issue #26
Evidence class: VISUAL_OBSERVATION + OFFICIAL_ARCHIVE
Source priority: Nintendo official screenshots/manual only.
No source images are stored in this repository.

## 0. Method

This pass observes composition and system presentation from official Nintendo screenshot/archive material.
It does NOT:
- trace maps;
- extract sprites;
- measure copyrighted pixel layouts;
- reproduce palettes;
- infer hidden implementation from appearance alone.

Technical claims remain in TECHNICAL_ARCHITECTURE_FINDINGS.md and are separately sourced from community reverse engineering.

## 1. Field composition

Official screenshots consistently show the player in a world where:
- architecture and vegetation occupy large, simple readable masses;
- roads/paths are visually dominant navigation surfaces;
- characters remain small relative to buildings but are still high-contrast/readable;
- environmental props and NPCs break up otherwise broad ground planes;
- the camera presents enough surrounding context to plan movement.

DOLZORE takeaway:
At gameplay zoom, the player should see:
- immediate route;
- at least one orientation cue;
- nearby interaction candidate;
- enough space around threats/NPCs to understand approach.

## 2. Everyday infrastructure is visible

Official archive examples include ordinary-town and travel functions such as:
- streets;
- bicycle travel;
- public/commercial areas;
- delivery/service behavior;
- photo event;
- bazaar/market-like space;
- homes/services.

The world does not communicate "RPG" only through fantasy architecture.

DOLZORE rule:
First town must visually support daily life before extraordinary systems dominate.

## 3. Scenic category changes are obvious

Official material juxtaposes:
- green town/countryside;
- night/dark or graveyard-like areas;
- cave/community spaces;
- market/bazaar spaces;
- abstract battle screens.

Even with compact pixel assets, scene identity changes strongly.

DOLZORE rule:
District identity should survive if UI labels are hidden.

Test:
Take a screenshot from each district with HUD removed. A reviewer should be able to classify the district by:
- ground/road grammar;
- architecture;
- prop family;
- population;
- lighting/palette;
- ambience concept.

## 4. Characters read as graphic symbols

Field characters are not rendered as realistic miniature humans.
They read through:
- head/body mass;
- limited color blocks;
- outline/contrast;
- consistent orientation;
- position and context.

DOLZORE implication:
Do not over-detail 24–40px-class sprites. Add detail only after silhouette and color-block read passes.

## 5. Party readability

When multiple party members appear, the world still remains legible because:
- characters occupy a narrow spatial train/cluster;
- silhouettes/colors differ;
- architecture is not overly noisy at the same scale.

DOLZORE:
If followers are shown:
- avoid stacking exact overlaps;
- keep formation predictable;
- ensure pathfinding does not create visual chaos;
- maintain sprite/background contrast.

## 6. Field enemy readability

Official material describes and shows enemies/odd figures existing directly in field space.

DOLZORE:
Field threats must be distinguishable from harmless residents/props by some combination of:
- motion;
- facing/attention;
- silhouette;
- animation;
- UI cue;
- audio;
- behavior radius.

Do not rely solely on color because of accessibility.

## 7. Interaction candidates are embedded in ordinary objects

Official manual explicitly supports checking signs/containers/objects.

DOLZORE:
World props should have interaction affordance without every object glowing.

Use:
- consistent proximity prompt;
- cursor/icon;
- animation or facing;
- subtle sound;
- semantic object categories.

## 8. UI occupies stable high-contrast zones

Official manual screenshots show command and status information presented in compact high-contrast boxes over field/battle imagery.

Observation:
The UI is visually distinct from the environment and preserves legibility.

DOLZORE:
Preserve the principle, not the appearance.
Use an original HUD/window system with:
- stable alignment;
- consistent typography;
- minimal overlap with action;
- scalable sizes;
- controller/touch focus state.

## 9. Battle composition

Official manual battle screenshot shows:
- enemy presentation separated from field map;
- abstract expressive background;
- command information;
- party health/resource panels.

Functional split:
- center/background = mood/action;
- windows/status = decision information.

DOLZORE:
Battle effects/backgrounds can be visually aggressive only if command/status reading remains stable.

## 10. Strong color contrast over fine texture

Across official material, area identity depends heavily on broad color/shape decisions rather than fine texture noise.

DOLZORE:
At first-town art review, perform:
- 25% scale screenshot;
- grayscale screenshot;
- blurred screenshot;
- silhouette-only character test.

If district and key landmarks vanish under these tests, the composition is too dependent on microdetail.

## 11. Map is not a photoreal city model

The reference visual language uses symbolic compression:
- shortened distances;
- simplified roads;
- selective buildings;
- exaggerated readability.

This aligns with creator testimony that literal realism was not the art priority.

DOLZORE:
Use believable topology, not GIS realism.
Prioritize:
- navigation;
- identity;
- pacing;
- scene composition;
- game functions.

## 12. Variety comes from combinations

Official screenshots show very different situations while maintaining the same base rendering language.

DOLZORE content economy:
Reuse:
- curb pieces;
- wall materials;
- vegetation;
- generic resident bodies;
- furniture;
- signage system.

Vary through:
- cluster composition;
- district palette;
- landmarks;
- NPC roles;
- event state;
- lighting;
- sound.

## 13. Visual oddity is anchored by normality

The stranger screenshots are effective because the game also shows ordinary roads, towns and services.

DOLZORE ZURE rule:
Before visually distorting a district, ensure the player has learned the normal version or an equally clear baseline.

## 14. Scene storytelling without dialogue

Screenshots communicate:
- obstruction;
- crowding;
- travel method;
- danger;
- community;
- market/social activity;
- unusual encounter;
without requiring a text dump.

DOLZORE scene review:
For each major scene, ask what a muted, dialogue-hidden screenshot communicates.

## 15. Original DOLZORE visual targets

These are recommendations, not MOTHER2 measurements:
- 1 strong landmark or compositional anchor per local screen/viewport-scale area where appropriate;
- no more than 2–3 equally dominant focal masses in one ordinary field view;
- at least 1 clear navigable ground/route shape;
- interactive prop clusters separated from pure decoration;
- major NPC readable by silhouette at gameplay zoom;
- enemy approach readable before contact;
- HUD critical values readable against every district.

## 16. Official source anchors

Nintendo MOTHER2 screen archive:
https://www.nintendo.co.jp/n08/a2uj/mother2/screen/index.html

Nintendo Wii U MOTHER2 page:
https://www.nintendo.co.jp/wiiu/software/vc/jbbj/index.html

Nintendo Wii U electronic manual:
https://www.nintendo.co.jp/data/software/manual/man_jbbj.pdf
