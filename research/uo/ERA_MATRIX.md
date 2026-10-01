# UO ERA / RULESET MATRIX

Authority: DeeDee-Lab/Dolzore#27
Purpose: prevent cross-era rule contamination.

## Rule

Every UO factual record MUST include era and ruleset/publish scope.
A later rule must not overwrite an older rule merely because it is current.
An unknown launch-era value remains UNKNOWN/PENDING until a dated source is collected.

## LAUNCH_1997
Verified anchor: Ultima Online launch date 1997-09-24.
Current corpus status: PARTIAL.
Do not infer 1997 skill caps, combat formulae, death rules, housing limits, crime thresholds, vendor behavior or server internals from current UO.
Source: UO-OFFICIAL-LAUNCH-2026.

## T2A_1998
Verified anchor: official 1998-10-24 Second Age publish introduced/expanded Lost Lands content including Papua, Delucia and additional locations/creatures.
Current corpus status: PARTIAL.
Source: UO-T2A-1998-10-24.

## RENAISSANCE_2000
Verified structural discontinuity: Publish 5 duplicated the existing lands into Felucca and Trammel rule environments.
Verified scope notes: Felucca retained/enhanced PvP rules; Trammel used consent-oriented PvP; bank boxes were shared; maps/runes became facet-specific; Siege Perilous did not receive Trammel.
Current corpus status: CORE_STRUCTURE_CONFIRMED / detailed rules PARTIAL.
Source: UO-RENAISSANCE-PUBLISH5.

## PRE_AOS_2000_2002
Verified anchor: continued corpse/looting/PvP iteration after Renaissance.
Verified Publish 16 anchor: individual stat cap changed from 100 to 125; Felucca risk/reward incentives included resource/fame/karma effects.
Current corpus status: PARTIAL.
Sources: UO-PUBLISH6-LOOTING, UO-PUBLISH16-FELUCCA.

## AGE_OF_SHADOWS_2003_PLUS
Verified structural discontinuity: Publish 17 / Age of Shadows.
Official historical source explicitly states that virtually all combat formulae changed to accommodate new systems.
Verified associated changes include the Item Insurance option, conversion/removal of older magic-item patterns, creature stat/skill/resistance/damage/loot changes, NPC shop-price changes, and AoS-era house customization fixes.
Do not combine PRE_AOS combat/item/death facts with AOS_PLUS without scope.
Sources:
- https://uo.com/wiki/ultima-online-wiki/technical/previous-publishes/2003-2/publish-17-1-age-of-shadows/
- https://uo.com/wiki/ultima-online-wiki/technical/previous-publishes/2003-2/publish-18-main-29th-may/
- https://uo.com/wiki/ultima-online-wiki/technical/previous-publishes/2003-2/publish-17-3-age-of-shadows/

## MODERN_PRODUCTION
Current official UO wiki/playguide rules.
Examples already verified in V1:
- STR / DEX / INT and derived HP/mana/stamina rules;
- current total skill cap and stat-cap mechanics;
- murder-count / red-status system;
- fame / karma;
- insurance on normal production rulesets;
- housing, lockdowns, secure containers, vendors and access levels;
- player vendors;
- crafting/gathering skill loops;
- animal taming/lore/training;
- party and communication channels;
- ships;
- champion spawns and treasure maps;
- Classic/Enhanced client UI/macro verbs.
Current corpus status: BROAD V1 COVERAGE / not exhaustive.

## NEW_LEGACY_SEASONAL
Separate official ruleset. Never merge with MODERN_PRODUCTION.
Verified V1 differences include:
- seasonal shard framing;
- no item insurance;
- equipped items preserved on death under documented beta rules;
- backpack contents move to corpse under documented beta rules;
- player-crafting emphasis.
Current corpus status: PARTIAL.
Sources: UO-NEW-LEGACY-FAQ, UO-NEW-LEGACY-BETA-FAQ, UO-NEW-LEGACY-GETTING-STARTED.

## SIEGE / MUGEN RULESET OVERLAY
Orthogonal ruleset differences exist across multiple eras.
Verified V1 examples:
- current insurance page excludes Siege Perilous from item insurance;
- official 2003 AoS Siege/Mugen publish applied AoS features with ruleset-specific differences, including Malas operating with Felucca-style PvP+ rules.
Source:
- UO-INSURANCE
- https://uo.com/wiki/ultima-online-wiki/technical/previous-publishes/2003-2/publish-17-5-age-of-shadows/

## Required next historical evidence

- launch-era manual/guide facts with date/version;
- exact pre-T2A and T2A stat/skill/death/crime/housing/economy rules;
- Renaissance exact PvP/crime/corpse/house differences;
- AoS complete item/combat/property/housing/magic changes;
- major later expansion/publish deltas;
- current production vs Siege/Mugen vs New Legacy differences.

ERA_MATRIX_V1=true
