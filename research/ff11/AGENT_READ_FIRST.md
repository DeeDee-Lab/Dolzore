# CRITICAL — SOURCE FIDELITY ORDER (2026-09-30 DIRECT USER AUTHORITY)

Before reading the rest of this document, read these in order:

1. `research/ff11/RAW_SOURCE_HANDOFF_CONTRACT.md`
2. `research/ff11/RAW_SOURCE_INDEX.json`
3. `research/ff11/VERIFIED_FACT_RECORD_SCHEMA_V1.json`
4. `research/ff11/FF11_VERIFIED_FACT_CATALOG_V1.jsonl` when present
5. only then this document and `FF11_REFERENCE_SPEC_V1.json`

This file is an **INTERPRETATION / SYNTHESIS layer**. It is not the raw source of truth.

If this file conflicts with RAW_SOURCE or VERIFIED_FACT, RAW_SOURCE / VERIFIED_FACT wins and this file must be corrected.

Do not hand another agent only this summary.

---

# FF11 REFERENCE — AGENT READ FIRST

Authority date: 2026-09-29 JST  
Consumer: DOLZORE game creation / Unity / MMO architecture agents  
Canonical research issue: DeeDee-Lab/Dolzore#25  
DPC transport issue: DeeDee-Lab/Automation#602

## 0. Non-negotiable interpretation rules

This research is for **mechanics/architecture learning**, not cloning FINAL FANTASY XI.

Do not copy:
- Square Enix character designs, names, logos, maps, zone layouts, quest text, dialogue, story, music, sound, textures, models, animation data, UI artwork/layout, proprietary job/ability names, or DAT contents.
- Retail executable code or packet payloads into DOLZORE.
- Third-party emulator/addon code unless its license is independently reviewed and the intended use is compatible.

Allowed target:
- learn system decomposition, interaction patterns, pacing, economy structure, party-role design, horizontal progression, world segmentation, solo-to-party bridging, and operational architecture;
- then implement original DOLZORE mechanics, data, code, visuals, names, formulas, maps, and content.

## 1. Evidence classes

Every fact below MUST retain one of these classes:

- `OFFICIAL_CURRENT`: current or still-live Square Enix / PlayOnline source.
- `OFFICIAL_HISTORICAL`: Square Enix source describing a historical design or formula; useful but not assumed unchanged today.
- `COMMUNITY_RESEARCH`: community-tested mechanics; useful for modeling but not retail-authoritative.
- `OPEN_SOURCE_EMULATION_REFERENCE`: LandSandBoat or similar implementation. It demonstrates one viable architecture, NOT Square Enix retail internals.
- `CLIENT_COMPATIBILITY_RESEARCH`: Windower/resource tooling observations about the installed client/data/protocol surface; NOT server-authoritative truth.
- `DPC_STATIC_OBSERVED`: directly observed from the user's installed DPC client by the read-only scanner.
- `DPC_RUNTIME_OBSERVED`: directly measured from a live DPC client session.
- `INFERENCE`: reasoned conclusion from evidence; never silently upgrade to fact.
- `SERVER_SIDE_UNKNOWN`: cannot be proven from installed client files alone.

DPC evidence is currently **pending** because the DPC GitHub runner is not accepting jobs. Do not fabricate `DPC_STATIC_OBSERVED` fields until Automation run 36574471567 or a successor actually completes.

## 2. Current service state

Evidence: `OFFICIAL_CURRENT`.

FFXI is still actively maintained in 2026. The official site lists a September 2026 version update, and the September update continued a multi-round Trust/alter-ego upgrade program. The official 2026 development policy explicitly emphasizes expanded solo play and strengthening alter egos.

Implementation lesson:
- FFXI is not a frozen 2002 design. When studying it, separate the long-lived core loop from later accessibility/solo systems.
- DOLZORE should preserve a deep party-oriented core while supporting solo continuity through original companion systems rather than requiring population density at every progression tier.

Sources:
- https://www.playonline.com/ff11us/
- https://www.playonline.com/ff11us/polnews/news27767.shtml
- https://www.playonline.com/pcd2/topics/ff11us/detail/40038/detail.html
- https://www.playonline.com/ff11us/topics/backnumber/202602/topics_all.html

## 3. Character identity and job model

Evidence: `OFFICIAL_CURRENT`.

Official guide facts:
- Character creation separates race, job, gender, face, and body size.
- Five races are documented in the guide.
- Six initial jobs are documented.
- Jobs can be changed.
- A support-job system lets a character effectively combine a current main job with a secondary job role.
- Additional jobs become available later.

DOLZORE lesson:
- Separate permanent character identity from changeable combat vocation.
- Make vocation switching a progression asset rather than forcing rerolls.
- A sub-role/sub-job layer creates combinatorial build depth without requiring dozens of fully independent classes.
- Do NOT reuse FFXI race/job names. DOLZORE needs original species/culture/vocation naming and original balance rules.

Source:
- https://www.playonline.com/ff11us/guide/system/index.html?pageID=system

## 4. Battle loop: TP -> Weapon Skill -> coordinated combo

Evidence: `OFFICIAL_CURRENT`.

Official guide facts:
- Job abilities are learned as the current job develops.
- Dealing or taking damage builds Tactical Points (TP).
- Sufficient TP enables weapon skills tied to the equipped weapon.
- Timing multiple weapon skills can create Skillchains.
- Correctly timed magic can interact with those chains as Magic Bursts.

Structural interpretation:
`basic combat activity -> resource accumulation -> player-chosen burst action -> inter-player timing window -> amplified coordinated payoff`

DOLZORE lesson:
- The important reusable idea is not the name “TP” or the exact formulas.
- Preserve a readable build-spend cadence and party timing opportunities.
- Implement original resource names, thresholds, combo grammar, elements, timing windows, and effects.
- Party coordination should create output greater than independent button rotation, but must remain understandable in the HUD.

Source:
- https://www.playonline.com/ff11us/guide/system/battle.html

## 5. Party-role depth

Evidence: `COMMUNITY_RESEARCH`.

Widely documented current community structure:
- a normal party is generally six characters;
- an alliance can be three six-character parties (18);
- common functional roles include tank, healer, damage, support, and hybrid combinations.

Do not treat community role templates as mandatory retail rules; they are play-practice observations.

DOLZORE lesson:
- Design around explicit role contribution, not just damage rankings.
- Support/control/resource-management roles need visible value.
- Early DOLZORE realtime groups should likely target a smaller-to-moderate party size, while keeping the data model extensible to multi-party public events.
- Do not lock the first Unity vertical slice to MMO party plumbing.

Source:
- https://www.bg-wiki.com/ffxi/Jobs_%26_party_roles

## 6. Enmity / threat system

Evidence: `OFFICIAL_HISTORICAL` (Square Enix producer explanation, 2013).

Documented conceptual model:
- monsters compare quantified enmity across players;
- two decay classes were described:
  - time-volatile enmity, which decays with time;
  - damage-volatile enmity, which drops when the player takes damage;
- actions were described as direct/indirect and fixed/effect-dependent;
- healing/enhancement can create indirect enmity;
- the monster targets the player with the highest combined enmity value.

Historical numeric example from the same official explanation:
- time-volatile decay baseline was described as 60 per second;
- Provoke was described as 1800 time-volatile enmity, therefore decaying over 30 seconds;
- effect-dependent damage example:
  - time-volatile = 240 * damage / standard_damage
  - damage-volatile = 80 * damage / standard_damage
- the post explicitly discussed planned adjustments, so numbers MUST NOT be assumed current.

DOLZORE lesson:
- Use multiple threat components with different decay behavior rather than one monotonically increasing “aggro score”.
- Make tank control, healing risk, burst damage risk, positioning, and threat-drop tools strategically legible.
- Create original formulas and tune from simulation; never copy historical constants blindly.

Source:
- https://forum.square-enix.com/ffxi/threads/threads/30629-Enmity-System-Explanation-and-Planned-Adjustments?mode=hybrid

## 7. Attack cadence and TP mathematics

Evidence: `COMMUNITY_RESEARCH`.

Community testing documents:
- weapon Delay is strongly tied to time between attack rounds;
- an approximate conversion near Delay/60 seconds is commonly used, with observed nuance;
- modified weapon delay affects TP-per-hit;
- Haste and actual delay-reduction mechanics have separate categories/caps;
- reducing actual weapon delay through systems such as Dual Wield/Martial Arts can affect TP generation differently from Haste.

DOLZORE lesson:
- Weapon speed should be a first-class build dimension linked to resource generation, animation feel, interrupt windows, and burst frequency.
- Avoid a hidden web of unrelated caps. If DOLZORE adopts multiple speed modifiers, expose the effective value and category limits in UI/tooltips.
- Use an original normalized timing system rather than reproducing FFXI's exact delay/TP equations.

Sources:
- https://www.bg-wiki.com/ffxi/Delay
- https://www.bg-wiki.com/ffxi/Attack_Speed
- https://www.bg-wiki.com/ffxi/Tactical_Points

## 8. Level Sync / mixed-level cooperation

Evidence: `OFFICIAL_HISTORICAL` (2008 feature announcement).

Official design:
- party leader designates a target player;
- higher-level members temporarily scale down to the target;
- players can earn experience while synchronized;
- the original announcement set a minimum target level of 10;
- same-area restriction was specified;
- deactivation had a 30-second buffer;
- high-level gear was scaled or had some properties suppressed while level-restricted.

DOLZORE lesson:
- A persistent MMO should actively remove level-gap friction between friends.
- DOLZORE should implement an original “mentor/sync” contract:
  - deterministic effective-level scaling;
  - reward anti-exploit rules;
  - ability/gear normalization;
  - transparent UI showing native vs effective stats.
- Treat the 2008 numbers as historical reference, not current FFXI proof.

Source:
- https://www.playonline.com/pcd2/topics/ff11us/detail/3599/detail.html

## 9. Quest / mission / territorial world systems

Evidence: `OFFICIAL_CURRENT` guide.

Official conceptual separation:
- quests: requests from individuals, shops, or organizations, usually optional;
- missions: nation-related assignments with stronger institutional/story framing;
- conquest: nations compete over regions; region control feeds influence/economic effects and player rewards.

DOLZORE lesson:
Use separate content channels instead of one giant “quest” bucket:
- local requests / relationship tasks;
- main-faction or story operations;
- persistent regional/public-state activities.
This supports both authored narrative and living-world consequences.

Source:
- https://www.playonline.com/ff11us/guide/system/quests.html

## 10. Economy and life systems

Evidence: `OFFICIAL_CURRENT` guide.

Official systems include:
- secure direct player trade;
- auction-house selling/bidding;
- player Bazaar sales;
- crystal synthesis/crafting;
- fishing with skill progression and cooking linkage;
- multiple world transportation modes.

DOLZORE lesson:
- economy and life content must be first-class, matching existing DOLZORE doctrine.
- Keep at least three market interaction modes conceptually distinct:
  1. direct trade,
  2. centralized listing/auction market,
  3. player-personal storefront/bazaar.
- Connect gathering -> crafting -> consumables/equipment -> combat/life demand.
- Build server-authoritative transaction ledgers from the beginning when multiplayer economy starts.
- Use original auction rules, currencies, recipes, fish, resources, world transport and UI.

Source:
- https://www.playonline.com/ff11us/guide/system/other.html

## 11. Housing / private-space pattern

Evidence: `OFFICIAL_HISTORICAL`.

Mog House was designed as a private player space and later gained guest invitation support.

DOLZORE lesson:
- Housing should be technically instanceable/private by default while still permitting controlled social visitation.
- This fits DOLZORE's staged MMO architecture: private housing does not need to consume always-live public-zone simulation.

Source:
- https://www.playonline.com/pcd2/topics/ff11us/detail/2507/detail.html

## 12. Equipment-set / macro persistence

Evidence: `OFFICIAL_HISTORICAL`.

The 2015 official update planning explicitly included server-side retention of equipment-set macros and display of party-member status effects.

DOLZORE lesson:
- Gear/loadout switching is a strategic system, not merely inventory UX.
- If DOLZORE supports gear sets, store named loadouts as data objects separate from raw inventory ownership.
- For multiplayer, synchronize only authoritative equipment state; user-defined shortcut organization can be account/profile data.

Source:
- https://www.playonline.com/ff11us/topics/backnumber/201507/topics_all.html

## 13. Client data / DAT research surface

Evidence: `CLIENT_COMPATIBILITY_RESEARCH`.

Windower's Resources documentation states that its Resource Extractor parses many resources from FFXI DAT files and supplements some categories with manually maintained fixes. It also warns that client updates can change structures and break extraction.

Implications:
- DPC DAT presence can reveal client resource taxonomy and IDs.
- DAT contents do NOT prove server-side formulas or authoritative rules.
- Extractors must be version-bound and schema-tested.
- For DOLZORE research, prefer metadata and independently derived taxonomy; do not commit extracted copyrighted art/audio/text/model data.

Sources:
- https://github.com/Windower/Resources
- https://github.com/Windower/ResourceExtractor

## 14. Client/server protocol surface

Evidence: `CLIENT_COMPATIBILITY_RESEARCH`.

Windower packet metadata documents client-facing categories including:
- connect/zone-in/zone-out;
- position/state updates;
- target/action requests;
- PC/NPC updates;
- job info;
- inventory counts/updates/assignment;
- trade;
- chat;
- party invitations and other UI/system state.

This proves that the client participates in a stateful zone/session protocol. It does NOT prove Square Enix's internal server implementation.

DOLZORE lesson:
Model network contracts by domain:
- session/auth;
- zone transition;
- movement/state replication;
- entity visibility;
- combat intent/result;
- inventory/equipment;
- party/social;
- transactions/economy.
Do not make Unity scene objects the source of truth for persistent MMO state.

Sources:
- https://github.com/Windower/Lua/blob/dev/addons/libs/packets/data.lua
- https://github.com/Windower/Lua/blob/dev/addons/libs/packets/fields.lua

## 15. Open-source emulator architecture reference

Evidence: `OPEN_SOURCE_EMULATION_REFERENCE`.

LandSandBoat is explicitly an open-source FFXI server emulator. Its development documentation shows one workable architecture:
- packet handlers receive client requests;
- zones own entity logic;
- zones can be inactive until a player enters and stop active work when empty;
- repeated task scheduling handles world/time/cleanup/persistence duties;
- C++ core plus Lua scripting and SQL-backed state are used.

IMPORTANT:
This is an emulator architecture, not evidence that retail FFXI servers are implemented the same way.

DOLZORE lesson:
- Region/zone processes should be activation-aware.
- Separate high-frequency simulation ticks from low-frequency world/persistence jobs.
- Separate engine code from authored content scripting/data.
- Persist world/player state independently of Unity clients.
- Small town shards and region servers can sleep when empty to control cost.

Sources:
- https://github.com/LandSandBoat/server/wiki/Development-Server-Startup-Tutorial
- https://github.com/LandSandBoat/server/blob/base/src/map/zone_entities.cpp

## 16. DPC direct-observation plan

Current status: `BLOCKED_DPC_RUNNER`.

Prepared scanner:
- Dolzore source commit: `73e9e36140e748a7d6966b1ab59225a9c4914a4f`
- Automation copy commit: `8cc42893737d4688ea0d973f609d028d92a98a93`
- Automation issue-trigger workflow corrected through commit: `4ba9ac30d6c43c700c3a9c4bd2cecbcd1c304bb2`
- canonical DPC transport issue: `DeeDee-Lab/Automation#602`
- latest scan run: `36574471567`

When DPC runner reconnects, scanner will collect:
- discovered install/product metadata;
- exact selected FF11 installation root with user-path redaction;
- complete relative file-path/size/timestamp manifest;
- extension counts/bytes;
- top-level and ROM/DAT topology;
- EXE/DLL version resources;
- EXE/DLL SHA-256;
- Authenticode status;
- expected user configuration directory metadata only.

Scanner explicitly does NOT:
- copy executable bodies;
- copy DAT contents;
- read user config content;
- read credentials;
- capture account/chat/private data.

Expected durable outputs:
- `research/ff11/dpc/LATEST_SCAN.json`
- `research/ff11/dpc/dpc_static_inventory.json`
- `research/ff11/dpc/dpc_pe_binaries.json`
- `research/ff11/dpc/dpc_dat_topology.csv`
- `research/ff11/dpc/dpc_file_manifest.tsv[.gz]`
- `research/ff11/dpc/dpc_static_observations.md`

## 17. Current DPC blocker and recovery truth

Observed 2026-09-29:
- DPC self-hosted scan run `36574471567` remains queued.
- Existing six-layer recovery policy was followed through the established peer rescue workflow.
- rescue run `36574620463`:
  - SowciePC job `109426924586`: executed and failed querying the DPC fixed task with Windows “Access is denied.”
  - Motoca rescue job `109426924229`: queued at last readback.
- Therefore **do not claim DPC is powered off**. The proven failure is execution/control reachability, not power state.
- Do not replay the same SPC scheduled-task method; unchanged repeated failure is an anti-loop violation.
- Do not use Desktop Commander / Remote Desktop Commander / DC.

## 18. DOLZORE implementation priorities derived from FF11 study

These are `DOLZORE_DESIGN_RECOMMENDATION`, not FFXI facts.

Priority A — relevant before/soon after the Unity first-town slice:
1. Character model cleanly separates identity from vocation/job state.
2. HUD data model reserves player name, level, HEART/HP, FOCUS/resource, vocation, zone, buffs/debuffs and target.
3. World is divided into stable region/zone IDs independent of Unity scene filenames.
4. Interaction targets use durable entity IDs, not object names.
5. Inventory/equipment/loadouts are pure data models, not scene-only state.
6. Public-world, private interior, and later housing instance boundaries are explicit.
7. Game-time/world-state service is separate from rendering.

Priority B — combat prototype:
1. paced auto/basic attack cadence;
2. build-spend combat resource;
3. weapon/skill burst actions;
4. cooperative timing/chain system;
5. two-component or multi-component threat model;
6. readable tank/heal/support/DD contribution;
7. equipment/loadout switching;
8. enemy knowledge and preparation as meaningful advantage.

Priority C — persistent RPG/life layer:
1. fishing, gathering, crafting, cooking as linked economies;
2. secure trade ledger;
3. centralized market plus player bazaar/storefront concept;
4. optional local requests vs faction/story missions vs public regional state;
5. companion/Trust-like ORIGINAL solo-support system;
6. mentor/level-sync system for friends at different progression points.

Priority D — staged MMO:
1. authoritative server-side player/inventory/economy/combat results;
2. zone/region activation and sleep;
3. event-driven persistence plus periodic checkpoints;
4. party first, then multi-party public events;
5. shard/instance boundaries invisible to authored world design where possible.

## 19. What remains unknown until DPC/runtime analysis

Do not guess:
- exact installed client version/build metadata on DPC;
- exact ROM directory count/size/layout on this installation;
- exact executable/DLL set and hashes;
- actual graphics/config state;
- local asset/resource taxonomy present on this build;
- runtime zone transition timing;
- client frame/input timing;
- live packet cadence;
- UI state machine details;
- exact current retail enmity/damage/TP formulas;
- server AI decision code;
- loot/drop RNG implementation;
- authoritative persistence/database topology;
- auction server internals;
- anti-cheat internals.

These are either `DPC_*_PENDING` or `SERVER_SIDE_UNKNOWN`.

## 20. Resume rule

On every new agent/session:
1. read this file;
2. read `research/ff11/FF11_REFERENCE_SPEC_V1.json`;
3. read `research/ff11/DPC_SCAN_STATUS.json`;
4. read Dolzore issue #25 latest comment;
5. if DPC evidence exists, prefer newest `DPC_STATIC_OBSERVED` over guesses;
6. never restart research from zero;
7. append only new evidence/deltas.
