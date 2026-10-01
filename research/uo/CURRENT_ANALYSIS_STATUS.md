# UO CURRENT ANALYSIS STATUS

Authority: DeeDee-Lab/Dolzore#27
Audit date: 2026-10-01 JST

## Executive status
FULL_UO_ANALYSIS_COMPLETE=false
CURRENT_OFFICIAL_DISTRIBUTION_HARVEST_COMPLETE=true
ALL_BINARY_FORMATS_DECODED=false
CLIENT_ARCHITECTURE_FULLY_MAPPED=false
NETWORK_FLOW_FULLY_MAPPED=false
FULL_GAME_SYSTEM_CORPUS_COMPLETE=false
FULL_ERA_RULESET_MATRIX_COMPLETE=false
GAME_AGENT_HANDOFF_READY_FOR_PARTIAL_USE=true
GAME_AGENT_HANDOFF_READY_FOR_COMPLETE_UO_REPRODUCTION=false

## What is complete
- Current official Classic Client distribution snapshot acquisition from live patch infrastructure.
- Raw product/package/sub-manifest preservation for the acquired snapshot.
- 480 unique loose distribution records and 44,200 unique pack-entry records.
- Current distributed client.exe version/hash observation.
- Initial exact format maps for UOP/MYP, map blocks, statics/staidx, tiledata, hues/radarcol, multis, skills.
- ClassicUO asset-loader source registry.
- 119 incoming packet ID -> handler mappings.
- 255 baseline packet-length entries plus version-dependent override assignments.
- Initial gameplay/system fact corpus and era/source discipline.

## What is not complete
The following are explicitly still incomplete and MUST NOT be represented as finished:
- art and gump pixel payload decoding across all live variants;
- animation frame/sequence/body/equipment mappings across all live variants;
- cliloc/string dictionary/localization exact binary decoding;
- sound/music/light/texture/font/multimap/verdata/tileart exact formats;
- all DEF/CFG/DAT/TXT auxiliary formats and semantics;
- all 480 loose files mapped to a verified decoder/semantic role;
- all 44,200 pack entries mapped to their logical resource identity;
- outgoing packet constructor map and all extended/subcommand protocols;
- login -> shard -> character -> world protocol/state transition;
- full Item/Mobile/Container/Object lifecycle;
- complete UI/gump/target/macro/input behavior;
- complete rendering/sorting/animation/audio runtime behavior;
- complete current game-system rules;
- complete historical rules for 1997/T2A/Renaissance/pre-AoS/AoS/modern/Siege/New Legacy;
- exact retail server internals that are not publicly observable.

## Preservation status
### Preserved byte-for-byte
- official product manifest raw bytes;
- fetched package/sub-manifest raw bytes;
- decompressed manifest XML as a derivative with its own SHA-256.

### Preserved as exact machine-readable records
- manifest attributes;
- filenames;
- file sizes;
- transfer/compression metadata;
- CDN provenance;
- pack-entry hashes/IDs;
- selected downloaded-binary final SHA-256/size/version observations;
- exact Git repository/path/blob SHA for open-source decoder/client implementation evidence;
- packet IDs and handler names extracted from cited source blobs.

### NOT stored as original bytes in GitHub
Commercial UO client binaries, maps, art, animation, music, sounds and other copyrighted assets are not republished into this repository. For these, the corpus keeps official distribution provenance, exact metadata/hash where observed, and decoder/source references. Therefore the statement "all original UO asset bytes are preserved in GitHub" is FALSE.

## Game-agent rule
The game agent may use the corpus now for verified partial implementation/research, but it MUST:
1. read AGENT_READ_FIRST.md first;
2. inspect CURRENT_ANALYSIS_STATUS.md before treating a domain as complete;
3. use raw manifests/JSONL/source blob references rather than prose summaries;
4. treat SOURCE_PENDING/PARTIAL/UNKNOWN domains as unfinished;
5. never infer retail server internals from emulator code;
6. never treat DOLZORE design interpretation as UO source evidence.

Issue #27 remains the canonical completion ledger.
