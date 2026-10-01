# DOLZORE DECISION LOG
Authority: 2026-09-29 JST
Purpose: prevent goal drift, repeated rethinking and loss of project intent.

This file records only decisions that are already agreed or explicitly superseded.
Future work must append new decisions here when they materially change product direction, architecture, game rules, commercial rules or release order.

---

## Operating rule

Every material decision must record:
- DATE/TIME
- DECISION
- WHY
- WHAT IT SUPERSEDES (if any)
- IMPLEMENTATION IMPACT
- NEXT ACTION

Do not rely on chat memory alone.

For long-running work:
- checkpoint before a long/risky implementation phase;
- checkpoint after each major implementation milestone;
- checkpoint before branch merge/deployment;
- checkpoint after public/runtime verification;
- if work may exceed a session/tool timeout, checkpoint intermediate state before continuing.

A task is not "complete" because design/code/tests exist.
For public/runtime work:
`DECISION -> IMPLEMENT -> QA -> MERGE -> DEPLOY -> FRESH PUBLIC READBACK -> CHECKPOINT`

---

# Current locked decisions

## 2026-09-29 — Current first town rejected
DECISION:
The current public first town is an operation prototype, NOT an accepted visual/world-design foundation.

WHY:
It proves movement/jump/BGM but fails town quality, density, district structure, landmarking, perspective and spatial richness.

SUPERSEDES:
Previous "Slice 1 first town complete/accepted" wording.

IMPLEMENTATION IMPACT:
Do not expand the world directly from the current one-screen town.

NEXT:
Rebuild first town.

---

## 2026-09-29 — First town architecture
DECISION:
Build a larger scrolling first town with multiple districts and loops.

Locked districts:
- Residential Hill
- Central Main Street
- Market / Workshop Lane
- JOURNAL / Civic Corner
- Riverside
- Station / East Gate
- optional Back Alley / strange pocket

WHY:
The town must feel like a real place, not three buildings on one road.

IMPLEMENTATION IMPACT:
Target map about 96×72 tiles / 1536×1152 logical px.
Viewport remains 480×270 initially.
Use camera scrolling.

NEXT:
Build exterior foundation before adding more world regions.

---

## 2026-09-29 — Open-world model
DECISION:
DOLZORE uses **open-region architecture**.

PLAYER EXPERIENCE:
The world feels connected and open.
Players physically walk between places rather than choosing destinations from a normal travel menu.

TECHNICAL MODEL:
Large seamless regions/zones internally.
Interiors, housing, story instances and time layers may be instanced.
MMO shards may split regions when needed.

WHY:
Combines dense authored world design with browser/MMO scalability.

SUPERSEDES:
A single monolithic world map concept.

---

## 2026-09-29 — Incremental public expansion
DECISION:
Do not wait for the entire RPG to be complete.
Ship the world in stable playable slices.

WHY:
The user wants to watch and use the world while it grows.

RULE:
Every slice must preserve playability.
Do not mix many unfinished slices.

---

## 2026-09-29 — Music commerce
DECISION:
Music is the only paid product category.

Canonical purchase flow:
`WORLD -> town BAR -> JUKEBOX -> 20s preview -> exact Stripe checkout`

Every town eventually gets a BAR.

The standalone /music/ page must stop being a purchase page once the first BAR purchase flow is live.

WHY:
Commerce should be part of the world rather than a disconnected shop page.

RULE:
Music purchases never grant combat power, XP, equipment stats or progression.

---

## 2026-09-29 — BAR role
DECISION:
BARs are:
- jukebox/music purchase locations;
- social hubs;
- rumor hubs;
- party meeting points;
- world-building spaces.

Each town BAR may have:
- unique name;
- bartender;
- interior;
- palette;
- NPCs;
- local recommendations;
- same global music catalog.

---

## 2026-09-29 — Character architecture
DECISION:
Players eventually get character creation.

SORA becomes a default starter preset, not a mandatory appearance.

Canonical story NPCs:
- MELO
- YUZU
- PON

Character creator future fields:
- name
- skin palette
- body silhouette
- face/eyes
- hair/hair color
- top/bottom/shoes
- accessory
- bag/tool
- controlled color palettes

Technical direction:
layered 32×40 paper-doll sprites.

---

## 2026-09-29 — Core character writing
DECISION:
MELO / YUZU / PON remain authored characters with persistent personalities, relationships, routines and story roles.

Detailed current character authority:
`design/CHARACTER_BIBLE_V1.md`

---

## 2026-09-29 — Story premise
DECISION:
Working story direction:
**DOLZORE — ずれた時刻の向こう側**

Core phenomenon:
**ズレ / ZURE**

Small contradictions between:
- sound and source;
- map and street;
- memory and records;
- object and owner;
- one time layer and another.

Time-layer direction:
- PRESENT
- OLD TOWN
- FAR TOWN
- BLANK HOUR
- later origin layer

WHY:
Allows ordinary-town mystery to grow into time-spanning adventure without copying existing stories.

---

## 2026-09-29 — Living-world priority
DECISION:
DOLZORE must be enjoyable without story progression or combat.

First-class non-combat systems:
- fishing
- cooking
- gathering
- crafting
- housing
- decorating
- trading/economy
- BAR/social activity
- collecting
- exploration
- rankings later
- community events

A player who spends months fishing/cooking/housing is playing correctly.

---

## 2026-09-29 — Hardcore RPG depth
DECISION:
Friendly/light visual presentation, heavy systemic depth.

Combat goals:
- preparation;
- role identity;
- enemy knowledge;
- positional judgment;
- resource management;
- gear sets;
- magic/skills;
- party cooperation;
- retreat/failure decisions.

Long-term references are high-level system qualities only:
- role/preparation depth
- encounter mechanics
- enemy observation/ecology
- economy/exploration/trade
- tension/resource management

Do not copy proprietary classes, enemies, maps, dialogue, UI or mechanics directly.

---

## 2026-09-29 — Progression model
DECISION:
Do not let character level dominate all progression.

Progression dimensions may include:
- character level
- job level
- weapon proficiency
- profession skill
- equipment sets
- knowledge/discovery
- crafting/fishing mastery

Horizontal progression is important.

---

## 2026-09-29 — Life systems
DECISION:
Fishing is a profession-level system, not a one-click timer.

Fishing variables:
- location
- water type
- time
- weather
- bait
- hook
- rod/line
- player skill
- fish behavior
- time layer

Caught fish can be:
- sold
- traded later
- cooked
- used as bait
- displayed
- donated
- used in quests

Cooking:
uses regional ingredients and supports exploration/combat preparation.

Housing:
major social/endgame system, beginning with local instanced housing and later shared visiting.

---

## 2026-09-29 — MMO direction
DECISION:
MMO is possible, but grow in stages.

Stages:
0. single-player persistent RPG
1. asynchronous shared-world features
2. small real-time town shards
3. public shared events / BAR social spaces
4. larger MMO only after demand proves it

Shared:
- town exteriors
- BARs
- roads
- public events
- social presence

Instanced:
- story-critical decisions
- time-layer choices
- puzzles
- endings
- some boss content

---

## 2026-09-29 — Backend direction
DECISION:
No dedicated game server is needed for early single-player development.

Early:
- GitHub Pages client
- IndexedDB save
- localStorage settings

Later online authority:
preferred direction:
- Cloudflare Workers
- Durable Objects/WebSockets
- D1 persistence

Server-authoritative before public competitive/economic use:
- fishing rankings
- tradable inventory
- player market
- shared housing state
- realtime combat

---

## 2026-09-29 — Current top implementation priority
DECISION:
**FIRST TOWN comes before BAR interior, fishing, combat, second town or MMO implementation.**

WHY:
The world foundation is not yet visually/spatially acceptable.

NEXT ACTION:
Build first-town exterior:
- larger scrolling map
- coherent 3/4 projection
- districts
- loops
- landmarks
- consistent lighting/shadows
- physically reachable BAR/JOURNAL/river/station

Only after the first-town exterior reaches acceptance:
add BAR interior as next slice.


---

## 2026-09-29 — First town foundation deployed
DECISION:
The rebuilt first-town foundation is now public and becomes the canonical world base.

PUBLIC FOUNDATION:
- 1536×1152 logical world
- 480×270 scrolling camera
- Residential Hill
- Central Main Street
- Market / Workshop
- Civic / JOURNAL
- Riverside
- Station / East Gate
- Back Alley
- BAR / JOURNAL / Riverside / Station landmark points
- Arrow/WASD movement
- Space jump
- Enter interaction
- BGM ON/OFF
- mobile controls

WHY:
The old one-screen prototype was too shallow to support the planned RPG/MMO world.
The new foundation creates an expandable open-region structure.

STATUS:
PUBLIC FOUNDATION, NOT FINAL TOWN.
Do not freeze visual design.
Continue adding interiors, residents, fishing, story and world detail on top of this map.

PROOF:
- merge commit: `9e77ef8894f882aa4d774ecb26806b5d14e3fc56`
- first-town QA main run: `36538543897` SUCCESS
- Pages run: `36538543369` SUCCESS
- fresh public readback: PASS

SUPERSEDES:
The rejected one-screen town as canonical world foundation.

NEXT ACTION:
Implement the first BAR interior and in-game jukebox purchase path while preserving the first-town map and regression QA.


---

## 2026-09-29 — Full game-engine rebuild

DECISION:
The current Canvas/direct-draw game implementation is rejected as the quality foundation.

Replace the game layer with:
- Phaser 3
- TypeScript
- Vite
- Tiled-compatible map data / Tiled authoring workflow
- layered HUD
- minimap/world map
- proper collision/object layers
- data-driven NPC/event/interaction definitions
- save-ready player/stat model

Free/CC0 asset policy:
- use CC0 assets as production-quality base/reference where appropriate;
- Kenney RPG Urban Pack is approved as a temporary high-quality urban base;
- DOLZORE-specific character art, landmarks and key buildings must become original;
- every third-party asset must have source/license recorded.

WHY:
The current approach caps visual quality and creates fragile collision/layout behavior.
The user explicitly rejects the current art, character design, blocked routes, lack of map UI and lack of status UI.

SUPERSEDES:
- further incremental patching of the current Canvas town as the primary production path.

IMPLEMENTATION IMPACT:
- existing Canvas game remains only as temporary public prototype until the Phaser build passes acceptance;
- JOURNAL / SUPPORT / LEGAL remain static HTML;
- music purchase direction remains BAR -> JUKEBOX -> preview -> Stripe;
- current story/MMO/life-system decisions remain valid;
- first town is rebuilt in the new engine instead of copied pixel-for-pixel.

FIRST-TOWN QUALITY GATE:
- no trapped/blocked mandatory path;
- collision comes from authored collision/object layers;
- minimap always available;
- player HUD shows name, level, HP/HEART, FOCUS, current job placeholder and zone;
- town has distinct districts and landmarks;
- no generic box-house repetition;
- readable, coherent character sprites;
- keyboard + mobile controls;
- camera follow;
- interaction prompt;
- BGM control;
- desktop/mobile QA;
- screenshot visual review before public replacement.

NEXT ACTION:
Build new Phaser/Tiled first-town vertical slice on branch:
`rebuild/game-engine-20260929`


---

## 2026-09-29 — Unity game-layer migration

DECISION:
The DOLZORE game/world layer will migrate from the current hand-coded Canvas + generated-background prototype to Unity.

WHY:
The target is no longer a lightweight interactive homepage. It is a long-lived high-quality RPG/shared-world game with:
- authored pixel-art towns;
- scrolling/open-region world;
- character creation;
- interiors;
- minimap/status/UI;
- fishing/cooking/housing;
- deep jobs/magic/skills/combat;
- eventual realtime multiplayer/MMO.

The current Canvas/Pillow approach proved controls and deployment but is a bottleneck for visual quality, level iteration, collision authoring, animation, UI and large-world content.

IMPLEMENTATION IMPACT:
- current public Canvas town becomes a temporary prototype/reference only;
- do not keep investing in its art as the production game foundation;
- WORLD/game client moves to Unity 2D;
- JOURNAL / SUPPORT / LEGAL and other SEO/editorial surfaces remain normal web pages;
- BAR jukebox commerce remains an in-world game interaction that opens exact Stripe checkout;
- online/MMO authority remains a separate backend concern and is not solved by Unity alone.

UNITY DIRECTION:
- Unity Personal while eligible;
- Unity 2D project;
- Pixel Perfect Camera;
- Tilemap/Tile Palette;
- Input System;
- Addressables when world assets grow;
- GitHub remains canonical source control.

ORIGINALITY:
Learn from high-level spatial/system qualities of reference RPGs, but do not copy protected characters, maps, dialogue, music, UI or other assets.

NEXT ACTION:
Freeze the current Canvas town as prototype.
Create a Unity first-town vertical slice before expanding BAR/fishing/combat/MMO implementation.


---

## 2026-09-30 — MOTHER2-high-level exterior / FF11-derived internal simulation

DECISION:
DOLZORE's permanent game identity is split deliberately:

### Exterior / presentation
Use original DOLZORE art/content while learning high-level presentation qualities from MOTHER2:
- warm readable 16-bit-inspired town;
- mundane streets/buildings used as adventure space;
- small expressive characters;
- everyday + strange contrast;
- wandering/landmarks/props/NPC life.

No protected maps, characters, sprites, UI layout, dialogue, music, logo or exact visual reproduction.

### Internal / systems
Use FFXI-derived internal RPG architecture:
- HP/MP + STR/DEX/VIT/AGI/INT/MND/CHR;
- five differentiated lineage-stat profiles;
- main/support vocation model;
- support effective level = min(native support level, floor(main level/2));
- skill ranks/caps;
- weapon/magic proficiency;
- accuracy/evasion/hit-rate;
- attack/defense ratio and pDIF-style physical damage pipeline;
- fSTR/WSC/fTP;
- TP-like delay-based build/spend resource;
- chain + burst cooperation;
- dSTAT/resist/MAB-MDB magic pipeline;
- multi-component threat;
- gear/loadout preparation and horizontal progression.

WHY:
The user wants an immediately approachable town adventure externally and deep, long-term, preparation-heavy RPG mechanics internally.

IMPLEMENTATION IMPACT:
- `design/GAME_CANONICAL_DIRECTION.md` is new canonical game identity authority.
- Unity internal rules become data-driven and testable.
- HEART/FOCUS remain player-facing aliases over internal HP/MP.
- Existing First Town Unity topology remains; visual art/composition improves separately.
- No unverified retail server values may be fabricated as facts.

RESUME:
A future agent must resume from **Gameひきついで** without requiring the user to repeat project context.

NEXT:
Finish Unity internal-rule compile/tests, then continue First Town visual-quality pass without enabling combat gameplay prematurely.


---

## 2026-10-01 — Ultima Online major structural reference layer

DECISION:
Ultima Online becomes a major structural reference layer for DOLZORE's persistent sandbox/shared-world architecture.

REFERENCE LAYER SPLIT:
- MOTHER2: exterior presentation/readability/ordinary-town atmosphere reference;
- FFXI: combat/stat/vocation/progression reference;
- UO: persistent world, object/container, property/housing, economy/crafting, player commerce, crime/notoriety/death risk, pets/taming, social communication, travel/ships, world events and shard/ruleset structure reference.

WHY:
The user explicitly requires UO to be analyzed across all elements and made usable as a primary structural foundation, while preserving source facts without lossy handoff.

EVIDENCE RULE:
- UO is not one timeless ruleset;
- every factual record carries era/ruleset/publish/source metadata;
- official current, official historical, New Legacy, community research and emulator implementation evidence remain separate;
- unknown retail internals remain unknown rather than inferred from ModernUO/ServUO;
- copyrighted source text/images/audio/assets are not republished as a substitute for originals; preserve original source/media references and provenance.

IMPLEMENTATION IMPACT:
- add `research/uo/` canonical corpus;
- reserve data-driven world/ruleset/property/economy/social/persistence boundaries now;
- do not replace the accepted FFXI-derived combat core unless later explicitly directed;
- do not redraw First Town into UO visual style;
- Unity remains active client/game engine.

NEXT ACTION:
Continue evidence ingestion by domain, append atomic JSONL facts, and implement only DOLZORE-original structures after evidence/design separation.
