# MOTHER2 TECHNICAL ARCHITECTURE FINDINGS

Authority: DOLZORE MOTHER2 research issue #26
Evidence class for implementation internals: COMMUNITY_REVERSE_ENGINEERING unless otherwise stated.
Primary technical reference: pk-hack/CoilSnake.
Purpose: understand structural decomposition, NOT recover proprietary Nintendo/HAL code.

## 0. Interpretation boundary

CoilSnake is a community ROM-hacking tool and therefore demonstrates a reverse-engineered data model.
It is NOT proof that original source code was architected with identical classes/modules.

Use it to understand:
- separable data domains;
- storage relationships;
- content reuse;
- practical editor/data models.

Do not copy ROM data or proprietary assets.

## 1. Map grid

Observed in CoilSnake MapModule:
- MAP_WIDTH = 256
- MAP_HEIGHT = 320
- map entries are read/written as a large grid;
- local tileset bits are associated with map entries;
- sector tables independently store visual/gameplay metadata.

Interpretation:
MOTHER2 supports a broad connected map through a large data grid rather than requiring one isolated scene per town.

DOLZORE:
Do not reproduce dimensions.
Use additive sectors/chunks with stable global coordinates.

## 2. Sector metadata

CoilSnake sector aggregation exposes fields including:
- Tileset
- Palette
- Music
- Teleport
- Town Map
- Setting
- Item
- Town Map Arrow
- Town Map Image
- Town Map X/Y

Design lesson:
A spatial sector is not just tile graphics. It owns semantic presentation/runtime metadata.

DOLZORE SectorDefinition should include:
- sector_id;
- world_bounds;
- visual_theme;
- palette/lighting;
- audio_zone;
- world_flags;
- minimap metadata;
- encounter profile;
- environment tags;
- streaming priority.

## 3. Tileset / minitile / arrangement structure

CoilSnake model:
- 8x8 graphic minitiles;
- tool constructs drawing tilesets;
- each EbTileset model uses 896 minitiles;
- 1024 arrangements/collision entries in its model;
- palette association is data-driven.

Important:
These are reverse-engineered/tool representation facts, not DOLZORE design targets.

Reusable principle:
small material -> larger composed tile -> map placement.

Unity:
- source pixel material;
- TileBase;
- RuleTile/custom tile;
- Prefab brush;
- district tile palette.

## 4. Collision is separate from graphics

CoilSnake Tileset model stores collision information separately from the graphic arrangement.

This is a binding DOLZORE lesson:
never infer collision from visible texture.

Recommended collision categories:
- walkable;
- hard_block;
- ledge;
- water;
- soft_obstacle;
- door;
- interactable_edge;
- hazard;
- nav_cost;
- jumpable_small_obstacle.

## 5. Map event changes

CoilSnake MapEventModule exposes event-flag-driven tile changes.

Reusable principle:
World state mutates a base map.

DOLZORE:
WorldStatePatch:
- patch_id;
- condition;
- tile_overrides[];
- prefab_enable[];
- prefab_disable[];
- collision_overrides[];
- nav_overrides[];
- audio_overrides[];
- NPC_schedule_overrides[];
- rollback/persistence behavior.

## 6. Sprite placement separated from sprite art

MapSpriteModule / model.eb.map_sprites:
map sprite placement is a distinct domain.

DOLZORE:
NPCDefinition != NPCSpawn/Placement.

NPCPlacement:
- npc_id;
- sector_id;
- position;
- facing;
- schedule;
- spawn_conditions;
- event_override.

This allows the same character definition to move by story/time without duplicating art.

## 7. Sprite groups

CoilSnake SpriteGroup model exposes:
- multiple sprite size classes;
- palette reference;
- sprite width/height;
- separate N/S and E/W collision dimensions;
- directional compilation/order;
- horizontal flip reuse;
- swim flags.

Reusable lessons:
- collision footprint can vary by facing;
- mirroring saves art budget;
- sprite art should be reusable independent of map;
- special locomotion state can be metadata.

Unity:
SpriteProfile
- animation clips;
- mirrored directions allowed;
- collider_by_facing;
- special locomotion flags;
- palette/material profile.

## 8. Doors / transitions

DoorModule stores door groups and destinations separately.

DOLZORE TransitionData:
- trigger volume;
- interaction requirement;
- destination world/scene/sector;
- destination anchor;
- transition effect;
- audio policy;
- required flag;
- one-way;
- camera policy.

Do not hardcode scene name into door sprite.

## 9. Enemy group placement

MapEnemyModule separates:
- enemy-group definitions;
- map placement/group selection.

Reusable principle:
Enemy composition and geographic distribution are different systems.

DOLZORE:
EncounterSet defines who can appear.
EncounterZone defines where/how.

## 10. Map music / event flags

MapMusic model allows event-flag/music pairs.

Reusable principle:
The same physical area can have different music under world state.

DOLZORE AudioZone:
- default state;
- story override;
- time override;
- battle return behavior;
- anomaly override.

## 11. Palette/event palette

Reverse-engineered map palettes can have event variants and sprite palette metadata.

Reusable principle:
Visual state may be changed without replacing all geometry.

DOLZORE:
Lighting/MaterialState profiles can switch:
- color grade;
- tile material;
- sprite material;
- post-processing;
- ambient light;
- fog/overlay.

## 12. Battle background data system

CoilSnake BattleBgModule:
- graphic tiles are 8x8;
- arrangements are modeled as 32x32;
- palette data separate;
- scroll table separate;
- distortion table separate.

Related community implementations show time-varying scanline/sine-style distortions.

Reusable architecture:
BasePattern + LayerMotion + Distortion + PaletteState.

Unity shader implementation:
BattleBackdropLayer:
- texture;
- uv_scale;
- uv_scroll;
- distortion_mode;
- amplitude;
- frequency;
- phase_speed;
- palette/material;
- blend;
- accessibility variant.

Do not reproduce exact original formulas/images.

## 13. Battle sprite size classes

CoilSnake model defines several battle sprite dimensions:
- 32x32;
- 64x32;
- 32x64;
- 64x64;
- 128x64;
- 128x128.

Lesson:
Enemy art supports different visual weights/silhouettes.

DOLZORE:
Do not normalize all enemies to identical on-screen dimensions.
Use threat-role-specific visual framing.

## 14. Module/domain separation observed in CoilSnake

Relevant modules include:
- Map
- Tilesets
- MapSprites
- SpriteGroups
- MapEnemy
- MapEvent
- Door
- MapMusic
- BattleBg
- Enemy
- Music
- Font
- WindowGraphics
- TownMapIcon
- Swirl
- Animation

Interpretation:
Even under SNES constraints, content is decomposed into many separately addressable domains.

DOLZORE Unity should follow a similarly data-oriented separation, without copying implementation.

## 15. Field-to-battle transition

Official manual:
field contact begins battle and encounter direction changes advantage.

CoilSnake includes SwirlModule, supporting distinct transition-effect data.

DOLZORE:
EncounterTransition should be a dedicated state with:
- field freeze policy;
- contact advantage;
- camera transition;
- effect selection;
- audio transition;
- battle context package.

BattleContext:
- enemy_set;
- field_position;
- direction_advantage;
- biome;
- world_state;
- preemptive flags;
- escape context.

## 16. Instant/trivial resolution

Official/creator evidence:
weak encounters may resolve without entering full battle display; creator interview says battle calculation is still performed.

DOLZORE:
FastResolveEvaluator:
1. build battle context;
2. simulate bounded outcome using original DOLZORE rules;
3. if deterministic safe victory and no special event:
   - resolve quickly;
   - grant results;
   - play compact feedback;
4. otherwise transition to battle.

Do not copy MOTHER2 thresholds/formula.

## 17. Data-oriented Unity design

Recommended scriptable/data assets:

WorldDefinition
RegionDefinition
SectorDefinition
TownConcept
TilesetTheme
PaletteLightingState
WorldStatePatch
NPCDefinition
NPCPlacement
SpriteProfile
ColliderProfile
EncounterSet
EncounterZone
TransitionData
AudioZoneData
DialogueSet
LandmarkData
BattleBackdropDefinition
BattleContextRules

Runtime services:
WorldStreamer
WorldStateService
SaveService
NPCScheduler
EncounterDirector
InteractionSystem
AudioStateMachine
DialogueSystem
TransitionSystem
BattleBridge

## 18. Save-state implications

Stable IDs are mandatory.

Do not save:
- GameObject references;
- scene hierarchy positions only.

Save:
- world_state_flags;
- entity persistent state by stable ID;
- quest state;
- NPC schedule overrides;
- discovered landmarks;
- inventory;
- player state/anchor;
- time;
- audio preference;
- optional memory-anchor completion.

## 19. Authoring tools

Build validation/editor tools early:
- missing collider detector;
- mandatory-route reachability test;
- door destination validator;
- duplicate stable-ID detector;
- orphan dialogue key detector;
- encounter safe-radius visualizer;
- audio-zone overlap visualizer;
- state-patch preview;
- minimap landmark validator.

The original reference demonstrates that content density depends on strong specification/data organization.

## 20. Performance

MOTHER2-era constraints encouraged reuse.
For modern DOLZORE/WebGL:
- tilemap batching;
- sprite atlas;
- addressables only when useful;
- pooled ambient NPCs/effects;
- bounded audio voices;
- chunked loading;
- shader complexity budget;
- mobile memory budget;
- deterministic unload/reload state.

Do not use modern hardware as an excuse for unmanaged content.

## 21. Anti-copy technical boundary

Allowed:
- learn that data domains are separated;
- use general tilemap/sector/state architecture;
- use original procedural shader effects;
- use original data-driven encounter/transition systems.

Forbidden:
- ROM extraction into production;
- original tile/sprite/palette/music data;
- decompiled game code;
- exact battle background images;
- exact distortion constants copied from reverse engineering;
- exact map geometry.

## 22. Technical source anchors

Community reverse engineering:
- https://github.com/pk-hack/CoilSnake

Relevant files:
- coilsnake/modules/eb/MapModule.py
- coilsnake/modules/eb/TilesetModule.py
- coilsnake/model/eb/map_tilesets.py
- coilsnake/modules/eb/MapSpriteModule.py
- coilsnake/model/eb/map_sprites.py
- coilsnake/modules/eb/SpriteGroupModule.py
- coilsnake/model/eb/sprites.py
- coilsnake/modules/eb/MapEnemyModule.py
- coilsnake/modules/eb/MapEventModule.py
- coilsnake/modules/eb/DoorModule.py
- coilsnake/modules/eb/MapMusicModule.py
- coilsnake/model/eb/map_music.py
- coilsnake/modules/eb/BattleBgModule.py
- coilsnake/modules/eb/SwirlModule.py

Official gameplay:
- https://www.nintendo.co.jp/data/software/manual/man_jbbj.pdf
