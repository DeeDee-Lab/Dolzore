# DOLZORE HANDOFF — CURRENT STATE
# 2026-10-01 HARD USER-VISIBLE VISUAL GATE

Direct-user correction:
**Do not show the user a visual candidate before opening the actual generated artifact and checking it visually.**

Mandatory authority:
- `design/USER_VISIBLE_VISUAL_REVIEW_GATE.md`
- `design/FIRSTTOWN_V7_POSTMORTEM_20261001.md`

For FirstTown and every DOLZORE user-visible visual:
1. technical/build PASS is not visual PASS;
2. open the exact generated image/artifact bytes;
3. compare directly with required original/reference evidence;
4. record concrete mismatches;
5. persist visual-review receipt;
6. if visual decision is FAIL, do not present it to the user as a candidate;
7. only a visual PASS may be shown for user acceptance;
8. explicit user acceptance remains separate.

Current FirstTown V7 ruling:
- run `36723025197`: TECHNICAL_PASS
- VISUAL_PASS=false
- USER_VISIBLE_READY=false
- USER_ACCEPTED=false
- V7 is rejected evidence, not a candidate.

The user instruction was already clear before V7. Failure to execute the manual visual review was a process violation.
A third same-method small corrective patch after repeated visual failure is forbidden; change the visual production method/base composition.

---

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


---

# 12. 2026-09-30 latest game implementation proof

Canonical exterior/internal split:
- Exterior: original DOLZORE presentation using MOTHER2 high-level town-RPG lessons.
- Internal: FFXI-derived deep RPG simulation.

Latest full green validation:
- Unity run: `36593013783 = SUCCESS`
- generated Unity source: `aa2af226102c22659944ffc07aff526310638ee4`
- internal rule tests: PASS
- lineage/vocation grade tests: PASS
- FirstTown scene generation: PASS
- stable region/entity/interior contract: PASS
- WebGL: PASS
- artifact: PASS
- generated-source preservation: PASS

The generated-source preservation loop was permanently changed from stale patch replay to:
Unity output snapshot -> fresh remote reset -> snapshot overlay -> stage -> commit -> push with retry.

Resume phrase remains:
**Gameひきついで**

Next work after resume:
1. do NOT change engines;
2. do NOT re-derive internal architecture;
3. improve First Town visual quality toward the canonical exterior direction;
4. keep combat math dormant until town presentation/interaction quality passes;
5. when combat starts, use the existing internal formula engine rather than inventing a new one.


---

# 13. 2026-09-30 Mother2 research -> FirstTown V3 implementation

Canonical Mother2 research is now production-consumed.

Read before further FirstTown visual iteration:
- `research/mother2/AGENT_READ_FIRST.md`
- `research/mother2/FIRST_TOWN_AUTHORING_CONTRACT_V1.json`
- `research/mother2/WORLD_MAP_FIELD_GRAMMAR.md`
- `research/mother2/VISUAL_CHARACTER_EFFECT_GRAMMAR.md`
- `research/mother2/NPC_LIFE_SOCIAL_TEXTURE_GRAMMAR.md`
- `research/mother2/DIFFERENTIATION_GUARDRAILS.md`
- `design/FIRST_TOWN_TOWN_CONCEPT.md`

Implemented V3 changes:
- smaller field-character scale;
- stronger silhouette outlines;
- brighter broad palette families;
- narrower authored road composition;
- roof-dominant 3/4 building treatment;
- residential lawns/fences/trees;
- main-street social pocket;
- ordinary props/vehicles/utilities;
- quiet riverside edge;
- oddity pocket;
- named residents;
- giant world labels remain removed.

Latest green proof:
- Unity run `36636292647 = SUCCESS`
- active generated-source commit `5e1b945f0cd2801417aa0f81111359764d27dc2a`
- BAR route PASS steps=19
- JOURNAL route PASS steps=30
- RIVERSIDE route PASS steps=47
- STATION route PASS steps=82
- HUD object/runtime binding contract PASS
- WebGL PASS
- generated-source preservation PASS

The requested visual direction is interpreted as:
**strongly apply the MOTHER2-derived design grammar, but never clone protected maps/sprites/UI/dialogue/music/exact palette layouts.**

FF11-derived internal simulation remains frozen/green underneath this visual work.

Resume remains:
**Gameひきついで**


---

# 14. 2026-09-30 Avatar / Color / Crosswalk quality checkpoint

User rejected:
- misaligned crosswalk;
- low-quality/unattractive characters;
- weak basis for dress-up/avatar play;
- dull color direction.

Permanent fixes:
- crosswalks moved from free-positioned SpriteRenderers to `Road Markings` Tilemap;
- every marking cell is automatically checked to have an underlying Road tile;
- 27 current marking cells PASS;
- player appearance is persistent data independent of stats/jobs;
- modular avatar engine added;
- SORA/MELO/YUZU/PON rebuilt as distinct avatar presets;
- enlarged wardrobe showcase added to CI;
- silhouette hashes for core four must differ;
- sleeves/hands/hair highlights/faces polished;
- palette shifted from neutral gray/beige/olive dominance to blue-violet/cream/spring-green/cyan/high-chroma architecture.

Latest green:
- run `36656034405 = SUCCESS`
- generated source `82510581568d9b868b06a5a89e4fb9db30e8c907`
- BAR/JOURNAL/RIVERSIDE/STATION route PASS
- HUD binding PASS
- ROAD MARKINGS ALIGNED PASS
- AVATAR QUALITY PASS
- WebGL PASS
- source preservation PASS

Read:
- `design/AVATAR_CUSTOMIZATION_SYSTEM.md`
- `design/FIRST_TOWN_COLOR_CONTRACT.md`

Resume phrase:
**Gameひきついで**


---

# 14. 2026-09-30 Avatar / Dress-up V5 green checkpoint

The user rejected the prior characters as unattractive and identified crosswalk misalignment in a screenshot.

Canonical avatar implementation is now:
- `game/unity/bootstrap/Assets/Scripts/AvatarSystem.cs`
- `game/unity/bootstrap/Assets/Editor/AvatarAssetGenerator.cs`
- `game/unity/bootstrap/Assets/Editor/AvatarQualityVerifier.cs`
- `design/AVATAR_DRESSUP_SYSTEM_V2.md`

Do not recreate a parallel avatar renderer.

V5 changes:
- improved body proportions / torso taper;
- improved face/eyes/brows;
- stronger hair silhouettes;
- distinct lower-body silhouettes;
- improved shoes/hands/arms;
- contact shadow;
- stronger front/side/back distinction;
- stronger walk-frame distinction;
- MELO back hair / record-bag placement fix after QA failure;
- 8-look wardrobe showcase generated every town art pass;
- stronger automated avatar QA.

Latest green proof:
- Unity run: `36662901682 = SUCCESS`
- generated source: `fcb9e81348084c6f60268dfb4a23e824287ea488`
- avatar quality: PASS
- wardrobe combinations before color variants: 725,760
- WebGL: PASS
- generated-source preservation: PASS

Street/crosswalk:
- use `Road Markings` Tilemap only;
- no free-positioned crosswalk decoration;
- road-marking acceptance is executed before FirstTown acceptance receipt can be written.

Next avatar work:
**in-game wardrobe / character-creator UI**, not a second avatar rendering architecture.

Resume:
**Gameひきついで**


---

# 15. 2026-09-30 permanent detailed-handoff rule + Mother2 research correction

Global permanent user rule:
**Agent-to-Agent transfer may not be abstract-only.**

Before continuing any substantial DOLZORE work, successors must preserve/read:
- exact user authority;
- exact repo/branch/SHA/Issue/comment/run/artifact references;
- source/evidence inventory;
- concrete measurements/observations;
- evidence -> decision -> implementation mapping;
- rejected outputs/failures;
- unknown/unmeasured domains;
- acceptance criteria/results;
- exact resume point.

Global canonical policy:
- `DeeDee-Lab/TECBUILD-Agent-Hub/canonical/agenthub/DETAILED_HANDOFF_EVIDENCE_POLICY.md`
- constraint `global.detailed-handoff-evidence.v1`
- AgentHub Issue #20 top authority contains the binding rule.

For Mother2-derived visual work specifically:
1. read `research/mother2/AGENT_READ_FIRST.md`;
2. immediately read `research/mother2/DIRECT_VISUAL_EVIDENCE_PACKET_V1.md`;
3. then read the structured/grammar documents;
4. never claim visual alignment from abstract principles alone;
5. preserve representative source identifiers and quantitative comparisons.

Mother2 research state was corrected:
`STRUCTURAL_REFERENCE_V1_READY_QUANTITATIVE_VISUAL_RESEARCH_ACTIVE`

Important truth:
- structural research is strong;
- quantitative visual research is NOT complete;
- the prior `COMPREHENSIVE_REFERENCE_V1_READY` wording overstated visual completeness.

Current correction evidence:
- direct visual packet commit `82d370a5679470489f822dfa1ad7d166550f84a8`
- read-first correction `1cbb54dc00f66f34d31ac3927698a5c2fd3f7162`
- research-status correction `bcb7d4a042afa73af57451b7a8f5e80b5238ab0e`
- issue #26 correction checkpoint `5904118796`

The user must never have to explain this again.

Resume phrase remains:
**Gameひきついで**


---

# 16. 2026-09-30 permanent original-image transfer rule

Direct-user permanent rule:
**Images must be handed off as originals, not abstracted/summarized/compressed replacements.**

Global canonical:
- AgentHub: `canonical/agenthub/ORIGINAL_IMAGE_TRANSFER_POLICY.md`
- AI-Supervisor: `policies/ORIGINAL_IMAGE_TRANSFER_POLICY.md`
- constraint: `global.original-image-transfer.v1`

Rules:
- original bytes unchanged when technically/legally preservable;
- derivative never replaces original;
- thumbnails/compressed/crops/OCR/descriptions are derivative evidence only;
- user-provided/owned images preserve original asset/reference;
- third-party copyrighted images use exact original image URL + source page + identifier instead of unlawful repo duplication;
- multi-image research requires an Image Source Manifest;
- user must not have to resend an image because a predecessor only summarized it.

Mother2/Gamepedia corpus:
- `research/mother2/GAMEPEDIA_TOWN_IMAGE_SOURCE_MANIFEST_V1.json`
- 15 town/field pages
- 70 exact original image URLs
- manifest commit `ffdc3eb33e47d2d28ed28c356edf97f2a71c9eb9`

Before visual implementation:
1. read original-image policy;
2. read Direct Visual Evidence Packet;
3. read Gamepedia Image Source Manifest;
4. inspect original image references;
5. only then use measurements/summaries as secondary data.

Resume phrase remains:
**Gameひきついで**


---

# 17. 2026-09-30 22:35 JST — master checkpoint pointer

Resume phrase:
**Gameひきついで**

Before acting, read:
`design/GAME_MASTER_CHECKPOINT_20260930_2235.md`

Checkpoint commit:
`c7b1f913b47cab5074063f6a7c6022d3ee8a2fd6`

It contains the current website recovery/migration state, FirstTown V7 exact run/error/fix state, Mother2 original-image evidence requirements, avatar state, and exact next actions.

`GAME_HANDOFF_READY=true`
`USER_RESTATEMENT_REQUIRED=false`
