# FF11 INTERNAL RULES — APPLIED TO DOLZORE

Authority: 2026-09-30 JST

Purpose:
Record exactly which FFXI-derived mechanics are now implemented in DOLZORE, the evidence level, and where DOLZORE intentionally uses original tunable values.

This is a mechanics reference, not a license to copy FINAL FANTASY XI content.

## Evidence rule

- Official facts may define architecture.
- Community-researched formulas may be implemented as reference mechanics when clearly marked.
- Historical official formulas are not claimed as current retail truth.
- DPC-installed-client observations remain pending until the authorized DPC runner completes.
- Retail server code/AI/RNG/database internals remain unknown.

## Applied rules

### Character / lineage

Implemented:
- HP / MP / STR / DEX / VIT / AGI / INT / MND / CHR internal stat model.
- five original DOLZORE lineage profiles preserving the broad five-race balance pattern:
  - balanced;
  - STR/MND/HP leaning;
  - MP/INT leaning;
  - DEX/AGI leaning;
  - HP/VIT/STR leaning.

DOLZORE lineage names, culture, appearance and coefficients are original/tunable.

### Main/support vocation

Implemented:
`supportEffectiveLevel = min(nativeSupportLevel, floor(mainLevel / 2))`

Evidence:
official Square Enix support documentation confirms the support-job maximum is half the main-job level.

DOLZORE vocation names are original.

### Skill ranks

Implemented:
A+ / A / A- / B+ / B / B- / C+ / C / C- / D / E / F / NONE.

Early level 1-30 cap tables are isolated in `InternalCombatMath.cs`.

The implementation uses the documented rank progression shape for early levels.

### Accuracy

Implemented:
`Accuracy = floor(DEX * 0.75) + AccuracyFromSkill + flat bonuses`

Skill conversion:
- <=200: 1.00 per skill
- 201-400: 0.90
- 401-600: 0.80
- >600: 0.90

### Evasion

Implemented:
`Evasion = floor(AGI * 0.5) + EvasionFromSkill + flat bonuses`

### Hit rate

Implemented:
`HitRate% = 75 + floor((Accuracy - Evasion)/2) - 2*dLVL`

DOLZORE current default clamp:
20%-95%.

Weapon-specific/player-system caps remain data-driven for later expansion.

### Attack

Implemented reference:
`Attack = 8 + CombatSkill + STR + flat bonus`

### Defense

Implemented level/VIT staged reference formula:
- level 1-50;
- 51-60;
- 61-89;
- 90+.

Armor defense remains an additive data term.

### fSTR

Implemented:
piecewise STR - target VIT conversion with weapon-rank lower/upper caps.

### Weapon technique base damage

Implemented:
`floor((WeaponDamage + fSTR + WSC) * fTP)`

WSC is a data-driven weighted sum of STR/DEX/VIT/AGI/INT/MND/CHR.

fTP supports linear interpolation between 1000 / 2000 / 3000 resource anchor values.

### Attack/Defense ratio and pDIF-like variance

Implemented:
- Attack / Defense ratio;
- target-level correction;
- critical wRatio shift;
- documented lower/upper piecewise curves;
- weapon-class pDIF caps;
- qRatio interpolation;
- final 1.00-1.05 randomizer.

All random samples are injectable, enabling deterministic testing.

### Critical hit

Implemented reference:
- base 5%;
- melee dDEX (attacker DEX - target AGI) contribution bands;
- explicit bonus term.

### Delay -> technique resource

Implemented player delay/resource formula with the documented piecewise delay bands.

Reference execution threshold:
1000 resource for a weapon technique.

DOLZORE may present this resource with an original name.

### Magic damage

Implemented staged pipeline:
1. base power;
2. dSTAT term;
3. flat magic-damage term;
4. resist state;
5. affinity/environment;
6. cooperative burst;
7. burst bonus;
8. day/weather/world modifier;
9. MAB/MDB ratio;
10. target magic damage adjustment.

Each stage floors where appropriate.

### Partial resist

Implemented:
- full = 1.0
- half = 0.5
- quarter = 0.25
- eighth = 0.125

### Magic accuracy

Implemented foundation:
- relevant magic skill;
- flat magic accuracy;
- explicit dSTAT contribution term.

Magic Accuracy Skill remains 1:1 in the reference model.

### Cooperative chain / magic burst

Implemented math foundation:
- no chain: 1.0
- 2-stage: 1.35
- each additional stage: +0.10

DOLZORE chain property names and elements are original.

Current DOLZORE resonance vocabulary:
Heat / Flow / Gale / Stone / Light / Shade / Pulse / Stillness.

### Threat / enmity

Implemented two-component state:
- volatile threat;
- durable threat.

Historical reference damage contribution:
- volatile: `floor(240 * damage / standardDamage)`
- durable: `floor(80 * damage / standardDamage)`

Historical volatile decay reference:
60 / second.

These constants are explicitly tunable and are not claimed as verified 2026 retail FFXI values.

## Not yet activated in live gameplay

The math exists, but combat is intentionally not enabled in the First Town yet.

Still pending gameplay integration:
- actual enemies;
- target lock;
- attack rounds;
- spells;
- techniques;
- chain windows;
- party roles;
- threat-driven AI;
- equipment stat aggregation;
- combat log.

This prevents combat code from distracting from the current First Town visual-quality gate.

## Files

Canonical Unity source:
- `Assets/Scripts/InternalRpgRulesData.cs`
- `Assets/Scripts/InternalCombatMath.cs`
- `Assets/Scripts/CoreRpgModels.cs`
- `Assets/Editor/InternalRulesSelfTest.cs`

Canonical game direction:
- `design/GAME_CANONICAL_DIRECTION.md`

## Pending FF11 evidence

Still not proven from DPC/current retail server:
- exact installed DPC client build and DAT topology;
- exact current race growth tables at every level/job;
- every current spell coefficient;
- every current job modifier;
- retail AI/RNG;
- retail authoritative server formulas/databases;
- auction/anti-cheat internals.

Do not fill these gaps by pretending inference is observed fact.

When better evidence arrives, update tables/constants without changing the overall architecture.
