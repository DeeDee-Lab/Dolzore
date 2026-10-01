# UO CLIENT / GAME FORENSIC CORPUS — ACCEPTANCE CONTRACT

Authority: direct user corrections, 2026-10-01 JST
Canonical issue: #27

## Primary objective
Analyze Ultima Online itself. DOLZORE application is a downstream consumer, not the research target.

The user has confirmed that no UO client is installed on their PCs. Therefore the current official internet distribution is the canonical acquisition lane. A local installed copy is not an acceptance dependency.

## Lane A — current official client distribution
Completion requires a reproducible harvest of the live official Classic Client distribution containing:
- exact product manifest bytes + SHA-256;
- exact manifest repository and file repository recorded as provenance;
- every package/sub-manifest raw byte copy + SHA-256;
- decompressed manifest XML + SHA-256;
- every distributed loose filename and exact manifest attributes;
- every UOP pack entry and exact manifest attributes;
- exact transfer/uncompressed size accounting;
- client.exe final byte size + SHA-256 + PE/version observation;
- direct acquisition procedure that can be rerun without a local PC.

Current state: LIVE DISTRIBUTION HARVEST COMPLETED for the 2026-10-01 snapshot. Continue as versioned/delta evidence, not as a one-time timeless truth.

## Lane B — binary/file/data formats
Every discovered file family must map to:
- exact observed live filename(s);
- magic/header/index structure where known;
- decoder/reader implementation source with repository/path/blob SHA;
- observed client-version applicability;
- unknown fields explicitly marked unknown;
- independent decoder comparison where possible.

Required families include at minimum:
MUL, IDX, UOP/MYP, DEF, CFG, DAT, cliloc/localization, map, statics, diffs, facet, tiledata, tileart, art, texmaps, animations, animation data/sequence/frame, gumps, multis, hues, radar colors, light, sound, music, fonts, skills/groups, body/equipment mappings, speech and string dictionaries.

## Lane C — observable client architecture
Collect source/provenance for:
patch/update; asset loading; render/world scene; UI/gumps; paperdoll/containers; movement; target cursor; macros/input; audio; localization; object/mobile/item lifecycle; network receive/send; packet table; login/shard/character flow; version/encryption handling; profile/config persistence.

## Lane D — game systems
Collect rules and observed behavior, versioned by era/ruleset:
world/facets/regions, characters, stats, skills, combat, magic, equipment, inventory, death/corpses/loot, crime/notoriety, PvP/PvE, resources, crafting, economy, vendors/trade, housing, pets/taming, NPC/AI/spawns, guild/party/chat, travel/ships, quests/events, reputation, persistence/decay.

## Lane E — version/history
Never combine 1997/T2A/Renaissance/pre-AoS/AoS/modern/Siege/New Legacy into one timeless rule set.
For current distribution evidence, keep retrieval timestamp and actual binary version together.

## Raw preservation
- Store exact official distribution manifest bytes when practical and lawful.
- Store exact source URL/repository/path/blob SHA.
- Commercial UO art/audio/maps/binaries are not republished to the repository; store hashes, exact size, manifest metadata, and acquisition provenance.
- Do not resize/recompress/re-encode a commercial asset and call that derivative the canonical source.
- When an asset must be visually inspected, preserve a pointer to the original distributed file/hash and label any rendered/exported image DERIVATIVE.
- If a network capture contains authentication secrets, do not commit those secrets.

## Completion guard
Do not close #27 because a prose overview or DOLZORE design document exists.
Lane A completion does not mean full UO analysis is complete; B/C/D/E must also reach their acceptance checks.
