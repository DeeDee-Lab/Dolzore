# UO CLIENT / GAME FORENSIC CORPUS — ACCEPTANCE CONTRACT

Authority: direct user correction, 2026-10-01 JST
Canonical issue: #27

## Primary objective
Analyze Ultima Online itself. DOLZORE application is a downstream consumer, not the research target.

## Lane A — actual client dataset
Completion requires a lawful Classic Client dataset to be available for inspection and an immutable manifest containing:
- exact client root and version;
- every relative path;
- exact byte length;
- file timestamps as observed;
- SHA-256 of every file;
- PE/version metadata for executables and DLLs;
- archive/member inventory for every UOP;
- no asset transcoding as canonical evidence.

Current state: NOT STARTED on a real installed dataset. Remote devices were unavailable at the latest check.

## Lane B — file/data formats
Every discovered file family must map to:
- exact observed filenames;
- magic/header/index structure where known;
- decoder/reader implementation source with repository/path/blob SHA;
- client-version applicability;
- unknown fields explicitly marked unknown.

Required families include at minimum:
MUL, IDX, UOP/MYP, DEF, CFG, DAT, cliloc/localization, map, statics, diffs, tiledata, art, texmaps, animations, animation data/sequence/frame, gumps, multis, hues, radar colors, light, sound, music, fonts, skills/groups, body/equipment mappings, speech, facet metadata.

## Lane C — observable client architecture
Collect source/provenance for:
asset loading; render/world scene; UI/gumps; paperdoll/containers; movement; target cursor; macros/input; audio; localization; object/mobile/item lifecycle; network receive/send; packet table; login/shard/character flow; version/encryption handling; profile/config persistence.

## Lane D — game systems
Collect rules and observed behavior, versioned by era/ruleset:
world/facets/regions, characters, stats, skills, combat, magic, equipment, inventory, death/corpses/loot, crime/notoriety, PvP/PvE, resources, crafting, economy, vendors/trade, housing, pets/taming, NPC/AI/spawns, guild/party/chat, travel/ships, quests/events, reputation, persistence/decay.

## Lane E — version/history
Never combine 1997/T2A/Renaissance/pre-AoS/AoS/modern/Siege/New Legacy into one timeless rule set.

## Raw preservation
- Store exact source URL or repository/path/blob SHA.
- Locally acquired binary assets remain byte-identical; never resize/recompress/re-encode the canonical file.
- Do not commit commercial UO art/audio/maps wholesale. Store hashes, metadata, lawful local path, and source provenance.
- If a network capture contains authentication secrets, preserve the unmodified raw capture only in a protected local evidence location and commit only its hash/metadata plus a separate sanitized derivative. Never pretend the derivative is the raw file.

## Completion guard
Do not close #27 because a prose overview or DOLZORE design document exists.
