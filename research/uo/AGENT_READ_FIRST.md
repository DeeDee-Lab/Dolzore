# ULTIMA ONLINE RESEARCH — AGENT READ FIRST

Authority: direct user instruction + scope corrections, 2026-10-01 JST
Canonical issue: DeeDee-Lab/Dolzore#27

## 0. Primary objective
Analyze ULTIMA ONLINE ITSELF from the actual official online-distributed client/data set plus official/public technical and gameplay sources.

DOLZORE design application is SECONDARY. Never replace UO evidence collection with an explanation of how DOLZORE should imitate UO.

The user explicitly states there is no UO client installed on any of their PCs. A local installation is NOT required and must NOT be waited for. The current official patch distribution is the primary current-client acquisition lane.

## 1. Mandatory no-summary handoff
A prose summary is never the canonical handoff.
Downstream agents must be able to return to:
- the exact official distribution manifest or exact CDN provenance record;
- the exact observed binary hash/size/version record;
- the exact source URL; or
- the exact public repository + path + Git blob SHA.

Preserve exact names, values, IDs, offsets, conditions, client versions, publishes, eras and rulesets. Unknown means UNKNOWN; do not infer missing raw data.

## 2. Canonical evidence layers
1. OFFICIAL_DISTRIBUTION_RAW_METADATA — exact product/package manifests and distribution metadata retrieved from the live UO patch infrastructure.
2. OFFICIAL_DISTRIBUTION_BINARY_OBSERVATION — bytes retrieved from the official CDN for analysis, with SHA-256/size/version evidence; commercial bytes are not republished.
3. OFFICIAL_CURRENT / OFFICIAL_HISTORICAL — UO/Broadsword pages and publishes.
4. OFFICIAL_SANCTIONED_WEB_CLIENT — Broadsword-sanctioned ClassicUO Web Client documentation and observed delivery behavior.
5. OPEN_SOURCE_CLIENT_IMPLEMENTATION — ClassicUO desktop and client-file readers.
6. PUBLIC_PATCH_PROTOCOL_REFERENCE — public patch/manifest tooling; verify observations against official distribution before promotion.
7. OPEN_SOURCE_EMULATION_REFERENCE — ModernUO/ServUO/RunUO; never proof of retail server internals.
8. COMMUNITY_RESEARCH — measurements/reverse engineering; must remain attributed.
9. DOLZORE_DESIGN_RECOMMENDATION — downstream interpretation only.

## 3. Current client corpus
Read:
- research/uo/online/current_patch/summary.json
- research/uo/online/current_patch/product_manifest.json
- research/uo/online/current_patch/unpacked_filenames.txt
- research/uo/online/current_patch/unpacked_files_unique.jsonl
- research/uo/online/current_patch/pack_entries_unique.jsonl
- research/uo/online/current_patch/manifest_sources.jsonl
- research/uo/online/current_patch/probe_file_hashes.json
- research/uo/client/ONLINE_DISTRIBUTION_STATUS.json
- research/uo/client/SCAN_STATUS.json

The official online distribution has been harvested. Local-PC scan status remains false because no local installation exists; that is no longer a blocker.

For every binary observation preserve, where available:
relative distribution path/name, manifest attributes, transfer bytes, final bytes, SHA-256, client/version metadata, acquisition source, and archive/member metadata.

Canonical commercial image/audio/map bytes are not transcoded or committed. Any PNG/JPEG/WebP export, screenshot, transcoded audio or rendered map is a derivative only and must point back to original distribution provenance/hash.

## 4. Current technical source spine
Read:
- research/uo/sources/TECHNICAL_SOURCE_LEDGER.jsonl
- research/uo/formats/FORMAT_SOURCE_MAP_V1.jsonl
- research/uo/client/KNOWN_CLIENT_FILE_INVENTORY_UOFIDDLER.json
- research/uo/CLIENT_CORPUS_ACCEPTANCE.md

Then inspect the referenced source file itself when exact implementation detail matters.

## 5. All-domain collection
Client/data, patch/distribution, world, map/facets, rendering, UI/gumps, input/macros/targeting, items/containers, mobiles/NPCs, animation, sound/music, localization, stats/skills, combat, magic, equipment, death/corpses/loot, crime/notoriety, PvP/PvE, resources/crafting, economy/vendors/trade, housing, pets/taming, AI/spawns, social/guild/party/chat, travel/ships, quests/events, persistence/decay, networking/protocol, version/history and operations are all in scope.

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
Network/protocol research must separate public packet formats from authentication secrets and live credentials.

## 8. Read order
1. this file
2. CLIENT_CORPUS_ACCEPTANCE.md
3. client/ONLINE_DISTRIBUTION_STATUS.json
4. online/current_patch/summary.json
5. online/current_patch/product_manifest.json
6. online/current_patch/unpacked_filenames.txt
7. online/current_patch/manifest_sources.jsonl
8. online/current_patch/probe_file_hashes.json
9. formats/FORMAT_SOURCE_MAP_V1.jsonl
10. sources/TECHNICAL_SOURCE_LEDGER.jsonl
11. SOURCE_INDEX.md
12. ERA_MATRIX.md
13. SYSTEM_INVENTORY.md
14. facts/*.jsonl
15. referenced implementations as needed
16. latest checkpoints on issue #27
17. only after evidence: DOLZORE_STRUCTURAL_APPLICATION.md

## 9. Anti-loop
Never restart from memory. Append new official-distribution evidence, hashes, manifests, format mappings and contradictions.
Issue #27 remains open until the acceptance lanes are complete.
Never wait for a local client installation unless the user later explicitly requests that lane.

UO_CLIENT_FORENSIC_SCOPE_V3_ONLINE_PRIMARY=true
