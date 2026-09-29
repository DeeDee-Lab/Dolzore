# DOLZORE HANDOFF — CURRENT STATE
Authority: 2026-09-29 20:53 JST
Purpose: allow the next agent to resume from one short instruction without re-deriving project intent.

## 0. One-line handoff

**Do not continue polishing the current Canvas town or the old Phaser scaffold.  
The active production direction is: build the first town again as a high-quality Unity 2D vertical slice.**

Active branch:
`rebuild/unity-first-town-20260929`

Main at handoff:
`4f5bcfe6925d7763b3c5cb3e5ccf55a18dd6b2de`

---

# 1. Current public state

The GitHub Pages WORLD currently exposes a playable Canvas prototype.

It has:
- movement;
- jump;
- BGM toggle;
- scrolling town;
- multiple named districts.

However, the user has explicitly REJECTED its quality.

Reasons:
- visual quality looks amateur/child-made;
- character design is unacceptable;
- repeated/simple house design;
- collision can create blocked routes;
- no proper map/minimap UI;
- no real status HUD;
- insufficient game-grade town design;
- not acceptable as the production game foundation.

Therefore:
**PUBLIC CANVAS TOWN = TEMPORARY PROTOTYPE ONLY.**

Do not call it complete or accepted.

---

# 2. Superseded technical paths

## Hand-coded Canvas/Pillow
Status:
**REJECTED as production foundation.**

Keep only as temporary public reference until Unity replacement is accepted.

## Phaser 3 / TypeScript / Vite scaffold
Branch:
`rebuild/game-engine-20260929`

Status:
**SUPERSEDED / DEPRECATED.**

That branch may contain useful experiments, but the latest authority is Unity.
Do not resume Phaser implementation unless the user explicitly reverses the Unity decision.

---

# 3. Active production architecture

## Game/world layer
Unity 2D.

Initial Unity direction:
- Unity Personal while eligible;
- 2D project;
- Pixel Perfect Camera;
- Tilemap / Tile Palette;
- Input System;
- UI Toolkit or appropriate Unity UI for HUD/map;
- Addressables when asset scale justifies it;
- GitHub remains canonical source control.

## Web/editorial surfaces
Remain normal web pages:
- JOURNAL
- SUPPORT
- LEGAL
- SEO/editorial content

## Music commerce
Still locked:
`WORLD -> town BAR -> JUKEBOX -> 20 sec preview -> exact Stripe checkout`

Standalone MUSIC page must not remain the canonical purchase path after the BAR flow is live.

---

# 4. Immediate user authority — quality reset

The latest user instruction rejects the current game quality and requires a FULL REBUILD.

Mandatory quality targets:
- high-quality authored town;
- substantially better character design;
- no mandatory route blocked by buildings/collision;
- proper map/minimap UI;
- proper player status display;
- stronger RPG HUD;
- coherent district/town planning;
- better visual hierarchy;
- believable streets, props and landmarks;
- use suitable free tools/assets where legally allowed;
- record licenses/sources for third-party assets.

Reference learning:
Study high-level world/town/system qualities from:
- MOTHER2
- FFXI
- FFXIV

Also preserve previously stated inspiration from:
- Monster Hunter
- Wizardry
- Uncharted Waters Online

Do NOT copy protected:
- characters
- maps
- dialogue
- music
- UI
- exact quests
- proprietary class/skill names
- protected art/assets

Learn structure, density, roles, preparation depth, city functions and long-term-system quality.

---

# 5. First Unity vertical-slice acceptance gate

The first town must not replace the current prototype until all are true.

## World / town
- coherent authored street plan;
- multiple recognizable districts;
- clear landmarks;
- walkable loops and shortcuts;
- no trapped mandatory route;
- collision authored separately from visual art;
- physical access to BAR, JOURNAL, riverside and station/east exit;
- interiors can be added incrementally after exterior foundation.

## Player / character
- professional-quality readable protagonist/avatar;
- current SORA becomes default future character-creator preset, not permanently mandatory;
- MELO / YUZU / PON remain authored story NPCs;
- animation must not look like placeholder rectangles.

## HUD
Always-available or context-appropriate UI for:
- player name;
- Level;
- HEART/HP;
- FOCUS;
- current job placeholder;
- current zone/district;
- BGM state;
- interaction prompt.

## Map
- minimap;
- expandable town map;
- player marker;
- key landmarks;
- undiscovered/known handling later.

## Controls
- keyboard;
- mobile/touch path planned;
- jump only if it remains mechanically useful;
- interact;
- camera behavior.

## QA
- desktop;
- mobile/web target if Unity WebGL is the delivery target;
- collision-path validation;
- screenshot visual review;
- no branch-only completion claim;
- public replacement only after user-visible quality passes.

---

# 6. Art/tool policy

Use free resources/tools when they materially improve quality.

Allowed direction:
- Unity built-in 2D tools;
- Tiled only if still useful for asset/map planning, but Unity Tilemap is primary;
- CC0/free commercial-use asset packs as reference/base only when license permits;
- record source + license in repository;
- key DOLZORE characters, landmarks, BAR identity and signature art should become original.

The previous note approved Kenney-style CC0 urban assets as a possible temporary base/reference.
Do not let temporary generic assets become the final DOLZORE identity.

---

# 7. Locked game design decisions that must survive the engine rebuild

## World model
Open-region architecture:
- player experience feels connected/open;
- technical regions/shards may be separated;
- interiors/time layers/housing can be instanced.

## Story
Working direction:
**DOLZORE — ずれた時刻の向こう側**

Core phenomenon:
**ズレ / ZURE**

Time-layer direction:
- PRESENT
- OLD TOWN
- FAR TOWN
- BLANK HOUR
- later origin layer

## Player character
- character creation planned;
- SORA = starter/default preset;
- MELO/YUZU/PON = canonical major NPCs.

## Life systems
First-class, not side content:
- fishing;
- cooking;
- gathering;
- crafting;
- housing;
- decoration;
- trade/economy;
- collecting;
- social play;
- rankings later.

## Combat
Deep/hardcore direction:
- preparation;
- jobs;
- magic;
- skills;
- gear sets;
- enemy knowledge;
- resource management;
- meaningful party roles;
- retreat/failure decisions;
- horizontal progression.

## MMO
Grow in stages:
1. single-player persistent RPG;
2. asynchronous shared features;
3. small realtime town shards;
4. public shared events;
5. larger MMO only after demand proves it.

Backend later:
- Cloudflare Workers;
- Durable Objects/WebSockets;
- D1 persistence.

No dedicated game server is required for the first Unity single-player town slice.

---

# 8. Immediate next action for the next agent

Do NOT spend the next turn re-analyzing the whole project.

Start from:
`rebuild/unity-first-town-20260929`

Immediate implementation sequence:
1. create Unity 2D project structure in repo;
2. define project settings / pixel-perfect baseline;
3. build first-town blockout with Tilemap;
4. create separate collision layer;
5. create first usable player controller;
6. add camera follow;
7. add HUD shell with Level/HEART/FOCUS/Job/Zone;
8. add minimap shell;
9. import only legally documented temporary/free art needed for the vertical slice;
10. produce first screenshot/preview;
11. checkpoint;
12. continue visual iteration until user quality bar is reached.

Do not start:
- second town;
- fishing implementation;
- combat implementation;
- MMO networking;
- BAR commerce implementation

until the first Unity town foundation is visually and structurally acceptable.

---

# 9. Canonical authority files

Read these before implementation:
- `design/DECISION_LOG.md`
- `design/FIRST_TOWN_REBUILD.md`
- `design/CHARACTER_BIBLE_V1.md`
- `design/DOLZORE_RPG_MASTER_PLAN.md`
- `design/STORY_LEVEL_EQUIPMENT_SAVE_ARCHITECTURE.md`
- `design/SHARED_WORLD_MMO_ARCHITECTURE.md`
- `design/LIVING_WORLD_HARDCORE_RPG_DOCTRINE.md`
- this file

When conflicts exist:
1. newest explicit user decision;
2. DECISION_LOG latest applicable entry;
3. this HANDOFF current state;
4. older design documents.

---

# 10. Handoff status

`HANDOFF_READY=true`

The next agent should be able to resume from:
**「収益化担当 / DOLZOREを引き継いで」**
or simply:
**「引き継いで」**

and continue at the Unity first-town vertical slice without asking the user to repeat requirements.


---

# 11. 2026-09-30 CANONICAL GAME DIRECTION UPDATE

Direct-user authority supersedes older ambiguity.

## Resume phrase

If the user says:

**Gameひきついで**

do not ask them to restate the project.

Immediately read:
1. `design/GAME_CANONICAL_DIRECTION.md`
2. this file
3. `design/DECISION_LOG.md`
4. `design/FF11_DERIVED_UNITY_ARCHITECTURE.md`
5. `research/ff11/AGENT_READ_FIRST.md`
6. `research/ff11/FF11_REFERENCE_SPEC_V1.json`
7. latest Issue #3 / Issue #25 checkpoints
8. active Unity branch head and latest green workflow

## Presentation authority

Exterior/presentation target:
**original DOLZORE game using MOTHER2 high-level town-RPG lessons.**

Meaning:
- warm readable 16-bit-inspired town;
- small expressive characters;
- everyday roads/houses/shops as real exploration space;
- readable landmarks;
- dense props and NPC life;
- ordinary/strange contrast;
- simple approachable field presentation.

Do not copy protected MOTHER2:
maps / sprites / characters / dialogue / UI / music / logo / exact palette/layout.

## Internal simulation authority

Internal design target:
**FFXI-derived deep RPG systems.**

Locked internal model:
- HP / MP
- STR / DEX / VIT / AGI / INT / MND / CHR
- 5 differentiated original DOLZORE lineage profiles
- main vocation + support vocation
- support effective level capped at half main level
- weapon/magic skill ranks
- skill caps by level
- Accuracy / Evasion / Hit Rate math
- Attack / Defense
- fSTR
- WSC / fTP
- Attack/Defense Ratio -> pDIF
- critical chance/damage structure
- TP-like build/spend resource based on weapon delay
- weapon technique / chain / magic-burst architecture
- magic damage with dSTAT / resist / affinity / burst / MAB-MDB / target adjustment
- volatile + durable threat
- named equipment loadouts
- horizontal progression
- preparation/enemy knowledge emphasis
- life/economy systems and staged MMO remain binding

Current-retail constants that are not actually proven are NOT to be represented as observed truth.
All uncertain values live in data/tuning tables so they can be replaced without architecture changes.

## Active implementation

Canonical detailed rule:
`design/GAME_CANONICAL_DIRECTION.md`

New Unity source:
- `Assets/Scripts/InternalRpgRulesData.cs`
- `Assets/Scripts/InternalCombatMath.cs`
- `Assets/Editor/InternalRulesSelfTest.cs`

Current game work remains on:
`rebuild/unity-first-town-20260929`

Do not reopen:
- Canvas as production engine;
- Phaser as active production engine;
- engine-selection debate.

Do not expand to second town before First Town visual acceptance.

`GAME_HANDOFF_READY=true`
`GAME_RESUME_PHRASE=Gameひきついで`
