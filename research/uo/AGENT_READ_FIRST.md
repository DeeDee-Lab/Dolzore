# ULTIMA ONLINE REFERENCE — AGENT READ FIRST

Authority: direct user instruction, 2026-10-01 JST
Consumer: DOLZORE game creation / Unity / shared-world / MMO / economy / life-system agents
Canonical research issue: DeeDee-Lab/Dolzore#27

## 0. Mandatory handoff rule

UO research is an evidence corpus first and a design input second.

Do not pass a shortened prose summary to another agent as a substitute for the evidence corpus.
Do not merge facts from different UO eras/rulesets into one timeless specification.
Do not silently change values, labels, conditions, publish dates, facet/ruleset scope, or source class.

For every factual record preserve, when available:
- fact_id
- domain
- field_name
- raw_value
- raw_value_type
- unit_or_encoding
- conditions
- era
- publish_or_ruleset
- source_id
- source_type
- source_location
- source_version_or_date
- evidence_class
- observed_at_or_source_date
- retrieved_at
- extraction_method
- source_sha256_if_file_backed

Interpretation belongs in separate DOLZORE application documents.

## 1. Copyright / source preservation rule

The user requires information and images to be passed without lossy reinterpretation.

For third-party copyrighted pages, books, images, audio, maps, sprites, UI art, manuals and game assets:
- do not republish the copyrighted body into this repository;
- preserve the exact source page URL;
- preserve the exact original-media URL when it can be lawfully referenced;
- preserve file metadata/hash only when the file is user-owned or otherwise lawfully available for local analysis;
- do not substitute a compressed thumbnail as the canonical visual source;
- do not treat a rewritten description as a replacement for the original image/text.

Atomic factual values such as numbers, dates, names of systems, formulas documented by sources, and rules can be stored as structured facts with provenance.

## 2. Evidence classes

Use exactly one primary evidence class per record:

- OFFICIAL_CURRENT
  Current official UO/Broadsword documentation or current official publish notes.

- OFFICIAL_HISTORICAL
  Official historical publish notes, archived official rules, or dated official material.

- OFFICIAL_NEW_LEGACY
  Official New Legacy rules. Never mix these into production-shard rules without explicit scope.

- HISTORICAL_PRIMARY_SOURCE
  Period primary material such as an original guide/manual/interview where rights allow factual extraction.

- THIRD_PARTY_HISTORICAL
  Historical screenshot/archive/reference not controlled by the current UO team.

- COMMUNITY_RESEARCH
  Community-measured formulas/behavior. Must not be promoted to official fact.

- OPEN_SOURCE_EMULATION_REFERENCE
  ModernUO/ServUO/other emulator architecture. This is implementation reference only and is NOT evidence of retail server internals.

- DOLZORE_DESIGN_RECOMMENDATION
  A DOLZORE design decision derived from evidence. Never serialize it as a UO fact.

## 3. Era separation is mandatory

At minimum distinguish:

1. LAUNCH_1997
2. T2A_1998
3. RENAISSANCE_2000
4. PRE_AOS_2000_2002
5. AGE_OF_SHADOWS_2003_PLUS
6. MODERN_PRODUCTION
7. NEW_LEGACY_SEASONAL
8. SIEGE_PERILOUS_RULESET where rules differ

A fact with unknown era must say UNKNOWN_ERA. Do not guess.

## 4. DOLZORE reference-layer contract

DOLZORE uses three distinct reference layers.

MOTHER2 reference layer:
- field readability
- warm ordinary-town presentation
- symbolic 16-bit-inspired visual grammar
- everyday/strange contrast

FFXI reference layer:
- deep stat dependency
- vocation/support-vocation structure
- weapon/magic skill progression
- staged physical/magic formulas
- build-spend cadence
- cooperative timing
- threat/enmity
- loadout preparation
- horizontal progression

UO reference layer:
- persistent shared-world sandbox structure
- class-light / skill-driven freedom as a structural reference
- world objects and containers as persistent gameplay entities
- player housing and private/public property
- gathering -> crafting -> goods -> player commerce loops
- player vendors and market discovery
- crime / notoriety / murder / risk rules
- corpse/death/loot consequence structure
- social proximity, guild, party and chat layers
- pets/taming and non-combat life roles
- open-world travel, ships and geography
- shard/facet/ruleset separation
- server-authoritative persistent world and event/state thinking

UO does NOT automatically replace the existing FFXI-derived combat core.
Any replacement must be an explicit later user decision.

## 5. What must never be copied

Do not copy into DOLZORE:
- UO maps/town layouts/dungeon layouts;
- sprites, tiles, animations, UI art or gumps;
- music/audio;
- dialogue/text/lore;
- named characters/factions/creatures when distinctive;
- exact branded spell/skill presentation;
- retail client/server protocol payloads;
- emulator code unless license compatibility is explicitly reviewed and user directs reuse.

Learn system structure; create original DOLZORE expression/content/data.

## 6. Agent read order

1. this file
2. SOURCE_INDEX.md
3. ERA_MATRIX.md
4. SYSTEM_INVENTORY.md
5. facts/*.jsonl
6. OPEN_SOURCE_SERVER_REFERENCE.md
7. DOLZORE_STRUCTURAL_APPLICATION.md
8. design/GAME_CANONICAL_DIRECTION.md
9. design/SHARED_WORLD_MMO_ARCHITECTURE.md
10. research/ff11/AGENT_READ_FIRST.md
11. research/mother2/AGENT_READ_FIRST.md
12. latest comments in issue #27

## 7. Anti-loop rule

Never restart UO research from zero.
Append new evidence and deltas.
If a source contradicts an older source:
- keep both records;
- add era/publish/ruleset scope;
- mark supersession only when the later official source clearly supersedes the earlier one.

## 8. Current checkpoint

UO_RESEARCH_V1_FRAMEWORK_ACTIVE=true
CANONICAL_ISSUE=27
BRANCH=research/uo-structural-v1
