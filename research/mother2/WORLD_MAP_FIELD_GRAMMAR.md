# MOTHER2 WORLD / MAP / FIELD GRAMMAR

Authority: DOLZORE MOTHER2 research issue #26
Evidence policy: see AGENT_READ_FIRST.md
Purpose: derive implementation-grade world-design rules without copying MOTHER2 maps.

## 1. Core finding: place concept precedes geometry

Evidence: CREATOR_INTERVIEW — Akihiko Miura, 2026.

The development process began from town-by-town concepts and events. Those concepts were discussed and converted into map, dialogue, status and event specifications before the final game program was stable.

Implementation consequence:
A map must not begin as a blank Tilemap filled with streets. It begins as a semantic contract.

For every DOLZORE town/region create a TownConcept asset containing:
- emotional_role;
- everyday_function;
- anomaly_or_tension;
- mandatory_story_function;
- optional_memory_anchors[];
- local_population_types[];
- local_food/leisure/culture[];
- audio_identity;
- visual_identity;
- traversal_identity;
- danger_profile;
- transition_in;
- transition_out;
- post_event_state_changes[].

Unity recommendation:
TownConcept should be a ScriptableObject or data asset consumed by level-authoring validation.

## 2. World topology: connected journey, segmented implementation

Evidence:
- OFFICIAL_NINTENDO/manual: field travel, town maps, transport and visible encounters.
- CREATOR_INTERVIEW: road-movie travel intent.
- COMMUNITY_REVERSE_ENGINEERING / CoilSnake:
  - map model exposes a 256 x 320 map-entry grid;
  - independent sector metadata includes tileset, palette, music, town-map settings and other flags;
  - doors, map sprites, enemies and tile events are separate data domains.

Interpretation:
The player experiences a broad connected journey, while implementation is data-partitioned.

DOLZORE architecture:
World
  -> Region
     -> Sector
        -> VisualLayer
        -> CollisionLayer
        -> NavigationLayer
        -> EncounterLayer
        -> NPCPlacementLayer
        -> Trigger/EventLayer
        -> AudioZone
        -> Lighting/PaletteState
        -> TownMapMetadata

Do not bake gameplay behavior into a single raster image.

## 3. Town grammar

MOTHER2's towns are memorable because their function, route structure and social texture differ. The reusable pattern is not any specific street layout.

Each DOLZORE town should include at least these authored roles:

### 3.1 Arrival threshold
A transition that announces a new identity through at least three simultaneous channels:
- road/environment change;
- palette/lighting shift;
- music or ambience shift;
- population composition shift;
- signage/architecture change;
- enemy pressure change.

### 3.2 Civic/functional spine
A route connecting ordinary services and recognisable public functions.
Purpose:
- orientation;
- believable daily life;
- safe navigation;
- landmark memory.

### 3.3 Social pocket
A place where several NPCs can be encountered without forcing story progress.
Examples for DOLZORE:
- plaza;
- riverside seating;
- market lane;
- station forecourt;
- schoolyard;
- bar frontage;
- workshop court.

### 3.4 Oddity node
One location that violates the normal rhythm but remains internally coherent.
It may be:
- an architectural anomaly;
- a behaviorally strange crowd;
- a local ritual;
- unusual lighting;
- an unexpected object interaction;
- a subtle spatial rule change.

Rule:
The oddity should not simply be "random." It must have a local reason or recurring motif.

### 3.5 Quiet edge
A low-density edge between activity clusters.
Functions:
- decompression;
- anticipation;
- music breathing room;
- environmental storytelling;
- transition into danger or another town.

### 3.6 Hidden/optional memory anchor
A scene or interaction with no required mechanical reward.
Goal:
create player-specific memories.

DOLZORE validation:
Every town must contain at least 3 optional memory anchors before final art lock.

## 4. Road and traversal grammar

Evidence: CREATOR_INTERVIEW describes a road-movie sensibility: scenery + food + rest + music create the trip.

Derived rule:
Roads are not dead connectors. They are authored pacing corridors.

A road segment should normally contain a sequence such as:
1. exit cue from previous area;
2. widening/narrowing rhythm;
3. one visual landmark;
4. one minor optional interaction;
5. controlled encounter pressure;
6. change in ambient/audio texture;
7. preview of destination identity;
8. arrival threshold.

Avoid:
- long uniform corridors;
- identical prop spacing;
- enemies distributed at constant density;
- hard cuts between every town;
- decorative road with no social/environmental content.

## 5. Field enemy integration

Evidence:
- OFFICIAL_MANUAL: enemies are visible on the field; contact begins battle; contact direction can affect advantage; weak enemies may resolve immediately.
- CREATOR_INTERVIEW: visible encounter design was intentional and weak-enemy skip was added to remove low-value battle friction.

Reusable principle:
Enemy placement is part of navigation, not a separate random system.

DOLZORE EncounterZone data:
- encounter_set_id;
- population_budget;
- spawn_surfaces;
- safe_margin_from_doors;
- pursuit_profile;
- retreat_profile;
- contact_advantage_rule;
- trivial_resolution_rule;
- respawn_policy;
- story_flag_overrides;
- day/night overrides.

Map design requirement:
The player must be able to read and make route choices around threats in ordinary field spaces.

Do not copy MOTHER2 encounter formulas. Build original DOLZORE threat/readability rules.

## 6. Density modulation

The key atmosphere is density contrast, not maximum density.

Use four density classes:
- D0 quiet: landscape and breathing room;
- D1 lived-in: sparse residents/props;
- D2 social: multiple NPCs, services, signs and interactions;
- D3 event: deliberately busy or strange.

Do not sustain D3 for long periods.

DOLZORE first-town target recommendation, not MOTHER2 measurement:
Within a typical 20–35 second walking interval, aim for one meaningful change among:
- NPC;
- landmark;
- interactable;
- route choice;
- audio/ambience;
- micro-event;
- environmental joke;
- vista.

This is an original pacing target to prevent dead space.

## 7. Landmarks and orientation

Landmarks should be multi-channel:
- silhouette;
- color family;
- local props;
- sound/ambience;
- NPC topic;
- route position.

A landmark is stronger when residents refer to it and gameplay routes depend on it.

Unity implementation:
Each LandmarkData asset:
- landmark_id;
- world_position;
- district_id;
- minimap_icon;
- visibility_radius;
- discovery_radius;
- audio_affordance;
- associated_dialogue_tags[];
- route_weight;
- quest_hooks[];
- day_state;
- night_state.

## 8. Building and interior relationship

MOTHER2's everyday world gains credibility from mundane functions: hotels, shops, homes, civic sites, entertainment and strange exceptions.

Reusable rule:
Exterior architecture must signal probable interior function before entering.

DOLZORE building contract:
- facade_semantic;
- door_location;
- public/private;
- expected_open_state;
- interior_scene_id;
- NPC_schedule_links;
- ambient_source;
- signage_policy;
- after_event_variant.

Do not create doors that look enterable but are permanently inert unless the visual language distinguishes them.

## 9. Interiors

Interiors should be compact semantic spaces, not miniature overworlds.

Each interior needs:
- entrance focal point;
- service/interaction focal point;
- 1–3 optional details;
- NPC position logic;
- clear exit;
- state variant if story changes it.

Original recommendation:
Use consistent interior camera scale but allow different footprint sizes. Avoid copying exact MOTHER2 room shapes or furniture placement.

## 10. Transition design

Transition classes:
- continuous walk;
- doorway;
- vehicle;
- narrative cut;
- special-space distortion;
- fast travel/teleport;
- state-change same-space transition.

A transition should change at least one of:
- collision topology;
- encounter rules;
- audio;
- palette/lighting;
- NPC population;
- available interactions;
- camera behavior.

Special-space transitions should feel qualitatively different from normal doors.

## 11. Dynamic map state

Evidence: COMMUNITY_REVERSE_ENGINEERING / CoilSnake exposes event-driven map tile changes and event palettes.

Reusable principle:
A town is not one immutable tilemap.

DOLZORE state layers:
- Base;
- StoryEvent;
- TimeOfDay;
- Weather;
- Restoration/Damage;
- Festival/PublicEvent;
- PlayerOwnedChanges.

Implementation:
Prefer additive state overlays and authored change sets rather than duplicating entire scenes.

## 12. Palette and mood transformation

Evidence: CREATOR_INTERVIEW / 2024 production discussion describes efficient transformation of familiar graphic material using palette/outline changes for a strange-world effect.

Reusable principle:
A known place can feel alien through controlled transformation of:
- palette;
- lighting;
- outlines/material response;
- ambience;
- music;
- NPC behavior;
- collision/event semantics.

DOLZORE must create its own visual language; do not reproduce Moonside colors or geometry.

## 13. Town map / minimap

Evidence:
- OFFICIAL_MANUAL includes town-map access.
- CoilSnake sector metadata includes town-map image, arrow and coordinates.

DOLZORE implementation:
Two distinct products:
1. Minimap: immediate navigation.
2. Town Map: semantic overview with landmarks/districts.

Town map should simplify geometry rather than mirror every tile.
Required:
- player marker;
- discovered landmarks;
- district boundaries;
- important services;
- quest markers only when fictionally justified;
- optional route notes.

## 14. Collision

Evidence: COMMUNITY_REVERSE_ENGINEERING separates tile arrangements from collision data.

Binding DOLZORE rule:
Collision must never be authored implicitly from visible tile color/shape.

Unity:
- Visual Tilemap;
- Ground/Navigation Tilemap;
- Collision Tilemap;
- Interaction volumes;
- Door/transition volumes.

Acceptance test:
Every mandatory route must pass automated reachability validation.

## 15. Route topology quality gates

For each town:
- at least one primary loop;
- at least one optional spur;
- at least one shortcut or alternate return route where scale permits;
- no mandatory destination reachable only by pixel-perfect movement;
- no decorative collision trap;
- no NPC placement that permanently blocks the critical path;
- all exits signaled visually before transition;
- major landmarks visible/foreshadowed from multiple approaches where appropriate.

These are DOLZORE recommendations, not claims about MOTHER2 exact topology.

## 16. Pacing curve

Town/field pacing should alternate:
comfort -> curiosity -> tension -> relief -> discovery -> stronger tension -> recovery.

Evidence: Miura explicitly describes placing a pleasurable resort-like experience before harsher travel.

DOLZORE world planner should assign every major area:
- stress_level 0..5;
- navigation_complexity 0..5;
- encounter_pressure 0..5;
- social_density 0..5;
- mystery_level 0..5;
- recovery_value 0..5.

Adjacent major areas should not all have identical profiles.

## 17. Practical Unity data contract

Recommended assets:
WorldDefinition
RegionDefinition
SectorDefinition
TownConcept
DistrictDefinition
LandmarkData
BuildingData
InteriorDefinition
EncounterZone
NPCSchedule
WorldStateLayer
AudioZoneData
TransitionData
MapDisplayData

Scene composition:
- persistent bootstrap scene;
- additive region/sector scenes;
- addressable asset bundles later if world scale requires;
- Tilemap layers separated by responsibility;
- deterministic stable IDs for save state.

## 18. What the game-creation agent should copy conceptually

COPY AS PRINCIPLE:
- semantic town concept before geometry;
- connected-journey illusion;
- contrast in density;
- everyday functions + coherent oddities;
- field-visible threats;
- optional memory anchors;
- stateful places;
- audio/palette as spatial identity;
- mundane travel as gameplay.

DO NOT COPY:
- specific town layouts;
- road bends;
- building placement;
- cemetery/market/etc. arrangement;
- names;
- exact landmarks;
- visual palettes;
- exact event sequence.

## 19. Source anchors

Primary:
- Nintendo MOTHER2 Wii U manual:
  https://www.nintendo.co.jp/data/software/manual/man_jbbj.pdf
- Nintendo official MOTHER2 pages:
  https://www.nintendo.co.jp/wiiu/software/vc/jbbj/index.html
- Hobonichi Akihiko Miura 2026:
  https://www.1101.com/n/s/mother_project/miura_akihiko/index.html
- Hobonichi Koichi Oyama:
  https://www.1101.com/mother_project/entry/archives/MOTHER_them/oyama.html
- Hobonichi MOTHER2 30th anniversary development material discussion:
  https://www.1101.com/n/s/mother_project/mother2_himitsu_book/

Technical:
- CoilSnake:
  https://github.com/pk-hack/CoilSnake
