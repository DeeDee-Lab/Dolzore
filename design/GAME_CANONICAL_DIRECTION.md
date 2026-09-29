# DOLZORE GAME CANONICAL DIRECTION

Authority: 2026-09-30 JST  
Resume phrase: **Gameひきついで**

This file is the short canonical handoff for the game itself.  
Read this before re-analyzing engine, art direction, combat math, or progression.

## 0. One-line product rule

**Presentation: original DOLZORE 16-bit-inspired town RPG learning from MOTHER2's high-level readability, warmth, ordinary-town exploration, and everyday/strange contrast.**

**Simulation: deep FFXI-derived RPG foundation with original DOLZORE names, content, data, tuning and formulas where retail-exact values are not proven.**

Unity 2D is the active game engine.

## 1. Copyright/originality boundary

Learn from MOTHER2:
- small readable characters;
- ordinary modern town used as adventure space;
- roads, sidewalks, storefronts, signs, trees and NPCs establishing scale;
- warm approachable presentation;
- mundane places mixed with strange details;
- strong town landmarks and wandering.

Do NOT copy:
- maps or street layouts;
- characters/sprites;
- exact UI/menu/dialogue layout;
- dialogue or story;
- music or sound;
- logo;
- exact palette sheets;
- protected objects or quest content.

Learn from FFXI:
- stat/dependency architecture;
- character identity vs changeable jobs;
- main/sub job depth;
- weapon/magic skill progression;
- build/spend combat cadence;
- party timing/chain payoff;
- attack/defense, accuracy/evasion and partial-resist math pipelines;
- threat/enmity components;
- gear/loadout preparation;
- horizontal progression;
- level sync/mentor principle;
- economy/life loops;
- zone/server decomposition.

Do NOT copy:
- Square Enix characters/races/lore;
- job/ability/spell names that are distinctive/proprietary;
- exact maps/UI/art/audio/text;
- DAT content;
- server code/protocol payloads;
- unverified retail constants represented as facts.

## 2. Internal base stats

DOLZORE internal simulation reserves the FFXI-style core stat set:

- HP
- MP
- STR
- DEX
- VIT
- AGI
- INT
- MND
- CHR

Player-facing aliases may remain:
- HEART = HP presentation
- FOCUS = MP/resource presentation

The UI alias must not remove the internal stat model.

## 3. Lineage / race-stat architecture

DOLZORE uses five original lineage profiles matching the broad mechanical roles of the five-race FFXI model:

1. BALANCED
   - no major weakness;
   - baseline HP/MP and attributes.

2. VANGUARD
   - stronger HP/STR/MND;
   - weaker DEX/AGI/INT.

3. MYSTIC
   - high MP/INT and good AGI;
   - lower HP/STR/VIT.

4. AGILE
   - high DEX/AGI;
   - moderate INT;
   - lower VIT/CHR.

5. STALWART
   - very high HP/VIT and high STR;
   - low MP and lower INT/AGI.

These are original DOLZORE lineages. Names/visual cultures are not final.

All lineage coefficients are data-driven and replaceable.  
Do not hard-code them into gameplay scripts.

## 4. Vocation / support-vocation architecture

Permanent character identity is independent of combat vocation.

Every character may hold multiple vocation levels.

Runtime state:
- main vocation;
- support vocation;
- main vocation level;
- support vocation native level;
- support vocation effective level.

Support-vocation effective level:
**min(native support level, floor(main vocation level / 2))**

This mirrors the documented structural behavior of FFXI's support-job concept while DOLZORE uses original vocation names and balance.

Initial DOLZORE role set remains:
- WARDEN
- STRIKER
- WEAVER
- LANTERN
- TRACE
- ECHO

Each vocation defines:
- base stat modifiers;
- weapon skill ranks;
- magic skill ranks;
- traits;
- active techniques;
- spell access;
- role;
- solo tools;
- party contribution.

## 5. Skill-rank architecture

Combat and magic competence are separate from character level.

Skill ranks:
- A+
- A
- A-
- B+
- B
- B-
- C+
- C
- C-
- D
- E
- F
- NONE

Each vocation declares a rank for every relevant skill family.

Current skill families reserve:
- blade
- heavy
- polearm
- shield
- ranged
- focus_tool
- evasion
- guard
- parry
- elemental/resonance magic
- restoration magic
- enhancement magic
- disruption/enfeebling magic
- dark/shade magic
- light/revelation magic

Rank determines skill cap by level through one isolated function/table.

## 6. Accuracy / evasion model

Reference structure:
- accuracy derives from DEX + current weapon skill + explicit bonuses;
- evasion derives from AGI + evasion skill + explicit bonuses;
- hit rate is based on Accuracy - Evasion and level correction;
- hit rate is clamped.

Current DOLZORE reference implementation:
- DEX contribution = floor(DEX × 0.75)
- skill contribution is piecewise:
  - <=200: 1.00 per skill
  - 201-400: 0.90
  - 401-600: 0.80
  - >600: 0.90
- base hit rate = 75%
- each 2 points of accuracy/evasion difference changes hit rate by ~1 percentage point
- higher-target-level correction reduces hit rate
- default clamp: 5% to 95%

All constants live in a rules profile, not in encounter scripts.

## 7. Physical damage pipeline

DOLZORE follows an FFXI-like staged structure:

1. weapon base damage;
2. fSTR-like STR-vs-VIT adjustment;
3. WSC-like stat contribution from the technique;
4. fTP-like technique multiplier;
5. attack/defense ratio;
6. pDIF-like bounded variance;
7. critical adjustment;
8. situational bonuses/reductions;
9. final integer damage.

Conceptual form:

base = floor((weaponDamage + fSTR + WSC) × fTP)

ratio = Attack / Defense

pDIF = bounded ratio-derived multiplier with controlled variance

damage = floor(base × pDIF × critical × situational)

Exact FFXI retail constants are not claimed unless separately proven.

## 8. Magic damage pipeline

DOLZORE uses an FFXI-like staged magic model:

1. spell base power;
2. dSTAT-like caster-vs-target stat term;
3. magic skill / spell coefficient contribution;
4. resist tier;
5. affinity / environmental modifier;
6. cooperative burst modifier;
7. magic attack / magic defense ratio;
8. target-specific magic damage adjustment;
9. final integer damage.

Default resist tiers:
- full: 1.0
- half: 0.5
- quarter: 0.25
- eighth: 0.125

Exact spell coefficients are data.

## 9. Build-spend / chain combat

Combat must support:
- basic/auto attack cadence;
- resource accumulation;
- weapon technique execution;
- cooperative timing windows;
- chain properties;
- compatible spell burst windows.

DOLZORE uses original names, properties, elements, windows and effects.

Current DOLZORE element/resonance vocabulary may include:
- Heat
- Flow
- Gale
- Stone
- Light
- Shade
- Pulse
- Stillness

## 10. Threat / enmity

Threat is not one permanent number.

Reserve at least:
- volatile/time-decaying threat;
- durable/damage-loss or event-decaying threat.

Threat-generating relations:
- direct;
- indirect;
- fixed;
- effect-dependent.

Healing/support can create indirect threat.

Enemies target according to their AI policy using total threat plus ecology/behavior rules.

Historical FFXI constants are references only, not retail-current truth.

## 11. Equipment / loadout philosophy

Named loadouts are first-class persistent objects.

Power should be horizontal as well as vertical.

Players may maintain:
- attack set;
- accuracy set;
- resistance set;
- magic set;
- recovery set;
- gathering/fishing/crafting sets later.

Inventory ownership and active equipment state are separate.

## 12. Exterior visual target

The player should initially see:
- friendly pixel-town presentation;
- streets that look lived in;
- small readable characters;
- distinct homes/shops/public buildings;
- trees, signs, benches, utility objects, small animals and NPC movement;
- optional strange details;
- clear landmarks before map usage.

The player should NOT see:
- obvious FFXI-like interface density on the field;
- cloned MOTHER2 buildings/characters/UI;
- giant debug labels;
- grid-generator-looking roads;
- repeated recolored box buildings.

Depth belongs mostly inside systems and menus; field presentation stays approachable.

## 13. Current Unity architecture already green

The active Unity First Town foundation already contains:
- Tilemaps;
- physical collision;
- player Rigidbody2D/Collider2D;
- camera follow;
- HUD/minimap shell;
- stable region/entity IDs;
- public/interior/private-instance boundaries;
- persistent player model;
- inventory/equipment/loadout model;
- WebGL green build.

Latest known green FF11-derived architecture validation:
run `36588077904`.

Generated-source proof at that checkpoint:
commit `fdd8437217565fbf88029bc835439f649d41ef5b`.

## 14. Next implementation order

Do NOT restart engines or rebuild Canvas.

1. lock this canonical direction;
2. implement internal lineage/base-stat engine;
3. implement vocation/sub-vocation/skill-rank database;
4. implement reusable physical/magic/accuracy/threat math library;
5. add deterministic unit/reference tests;
6. keep combat disabled in gameplay until First Town visual quality is acceptable;
7. simultaneously continue First Town visual improvement:
   - distinct architecture;
   - better SORA/NPC sprites;
   - narrower/authored streets;
   - props and living-world density;
   - no giant world labels;
   - real WebGL UI review.

## 15. Resume protocol

If chat context is lost and user says:

**Gameひきついで**

the next agent must:
1. read this file;
2. read `design/HANDOFF_CURRENT_STATE.md`;
3. read `design/DECISION_LOG.md`;
4. read `design/FF11_DERIVED_UNITY_ARCHITECTURE.md`;
5. read `research/ff11/AGENT_READ_FIRST.md`;
6. read `research/ff11/FF11_REFERENCE_SPEC_V1.json`;
7. read latest Dolzore Issue #3 and #25 checkpoints;
8. fresh-read active Unity branch;
9. continue from the newest green run/commit;
10. never restart analysis from zero unless the newest user authority explicitly changes direction.

Priority conflict order:
1. newest explicit user instruction;
2. this canonical file;
3. latest DECISION_LOG entry;
4. HANDOFF_CURRENT_STATE;
5. older design docs.

`GAME_CANONICAL_READY=true`
