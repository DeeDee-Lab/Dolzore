# ULTIMA ONLINE RESEARCH — AGENT READ FIRST

Authority: direct user instruction + scope correction, 2026-10-01 JST
Canonical issue: DeeDee-Lab/Dolzore#27

## 0. Primary objective
Analyze ULTIMA ONLINE ITSELF from the actual client/data files where lawfully available and from official/public technical sources.

DOLZORE design application is SECONDARY. Never replace UO evidence collection with an explanation of how DOLZORE should imitate UO.

## 1. Mandatory no-summary handoff
A prose summary is never the canonical handoff.
Downstream agents must be able to return to:
- the exact local client file + SHA-256, or
- the exact source URL, or
- the exact public repository + path + Git blob SHA.

Preserve exact names, values, IDs, offsets, conditions, client versions, publishes, eras and rulesets. Unknown means UNKNOWN; do not infer missing raw data.

## 2. Canonical evidence layers
1. LOCAL_CLIENT_RAW — user/lawfully acquired actual client files, byte-preserved.
2. OFFICIAL_CURRENT / OFFICIAL_HISTORICAL — UO/Broadsword pages and publishes.
3. OPEN_SOURCE_CLIENT_IMPLEMENTATION — ClassicUO and client-file readers.
4. PUBLIC_PATCH_PROTOCOL_REFERENCE — public patch/manifest tooling; observations require verification.
5. OPEN_SOURCE_EMULATION_REFERENCE — ModernUO/ServUO/RunUO; never proof of retail server internals.
6. COMMUNITY_RESEARCH — measurements/reverse engineering; must remain attributed.
7. DOLZORE_DESIGN_RECOMMENDATION — downstream interpretation only.

## 3. Client corpus is mandatory
Read research/uo/client/SCAN_STATUS.json first.
Until actual_client_scan_performed=true, NEVER say the real local UO client has been analyzed.

For every actual client file capture:
relative_path, bytes, timestamps, SHA-256, type/signature, client version, acquisition source, and archive-member metadata where relevant are mandatory.

Canonical image/audio/map assets are the original bytes. PNG/JPEG/WebP exports, screenshots, thumbnails, transcoded audio and rendered maps are derivatives only.

## 4. Current technical source spine
Read:
- research/uo/sources/TECHNICAL_SOURCE_LEDGER.jsonl
- research/uo/client/KNOWN_CLIENT_FILE_INVENTORY_UOFIDDLER.json
- research/uo/CLIENT_CORPUS_ACCEPTANCE.md

Then inspect the referenced source file itself when exact implementation detail matters.

The current ClassicUO UOFileManager exposes loaders for Animations, AnimData, Arts, Maps, Clilocs, Gumps, Fonts, Hues, TileData, Multis, Skills, Texmaps, Speeches, Lights, Sounds, MultiMaps, Verdata, Professions, TileArt and StringDictionary. Treat this as an implementation observation tied to the referenced blob, not as a timeless retail specification.

## 5. All-domain collection
Client/data, world, map/facets, rendering, UI/gumps, input/macros/targeting, items/containers, mobiles/NPCs, animation, sound/music, localization, stats/skills, combat, magic, equipment, death/corpses/loot, crime/notoriety, PvP/PvE, resources/crafting, economy/vendors/trade, housing, pets/taming, AI/spawns, social/guild/party/chat, travel/ships, quests/events, persistence/decay, networking/protocol, patch/version/history and operations are all in scope.

## 6. Era/version separation
At minimum distinguish:
LAUNCH_1997
T2A_1998
RENAISSANCE_2000
PRE_AOS_2000_2002
AGE_OF_SHADOWS_2003_PLUS
MODERN_PRODUCTION
SIEGE_PERILOUS_RULESET
NEW_LEGACY_SEASONAL
and exact client versions when client behavior/files differ.

## 7. Sensitive capture handling
Never commit passwords, session tokens, account secrets or equivalent authentication material.
If an unmodified raw packet/log capture contains such values, retain raw locally in a protected evidence location, hash it, and create a separately labeled sanitized derivative for agent inspection.

## 8. Read order
1. this file
2. CLIENT_CORPUS_ACCEPTANCE.md
3. client/SCAN_STATUS.json
4. sources/TECHNICAL_SOURCE_LEDGER.jsonl
5. client/KNOWN_CLIENT_FILE_INVENTORY_UOFIDDLER.json
6. SOURCE_INDEX.md
7. ERA_MATRIX.md
8. SYSTEM_INVENTORY.md
9. facts/*.jsonl
10. referenced client/source implementations as needed
11. latest checkpoints on issue #27
12. only after evidence: DOLZORE_STRUCTURAL_APPLICATION.md

## 9. Anti-loop
Never restart from memory. Append new evidence, hashes, manifests and contradictions.
Issue #27 remains open until the acceptance lanes are complete.

UO_CLIENT_FORENSIC_SCOPE_V2=true
