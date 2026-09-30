# FF11 RAW SOURCE HANDOFF CONTRACT

Authority: direct user instruction 2026-09-30 JST
Canonical repo: DeeDee-Lab/Dolzore
Canonical issue: DeeDee-Lab/Dolzore#25
Status: MANDATORY

## 0. Absolute rule

For FF11 research and all downstream game-creation agents:

**Do not replace source information with a summary, abstraction, paraphrase, normalization, recompression, resize, redraw, OCR-only surrogate, translated substitute, or interpretation.**

Information handoff is ordered:

1. RAW_SOURCE
2. VERIFIED_FACT
3. INTERPRETATION

A consumer must not use layer 3 as a substitute for layers 1–2.

## 1. RAW_SOURCE layer

### 1.1 User-owned / user-supplied artifacts
When the source itself is owned/supplied by the user and technically available:
- preserve original bytes;
- preserve original filename;
- preserve original format;
- do not resize;
- do not recompress;
- do not transcode;
- do not redraw;
- do not replace an image with OCR;
- do not convert audio/video into another codec for handoff;
- compute SHA-256;
- record byte size;
- record source path/id;
- record acquisition time.

### 1.2 Installed FF11 third-party protected assets
FF11 DAT/images/audio/models/text/executable bodies are third-party copyrighted material.

Do **not** copy those asset bodies into a shared GitHub repository or chat.

To preserve exact fidelity without redistribution, store:
- host;
- exact local path;
- relative FF11 path;
- SHA-256;
- byte size;
- last-write time;
- extension/type;
- build/version context;
- source scan ID.

Authorized agents operating on the same source host must read the original file bytes directly from that path.

The pointer+hash is the handoff identity. A compressed screenshot, preview, OCR result, extracted image, transcoded audio, or prose description is **not** the RAW_SOURCE.

## 2. VERIFIED_FACT layer

Store exact observed/source-stated values.

Examples:
- job name;
- job ID;
- magic name;
- magic ID;
- level requirement;
- MP cost;
- recast;
- cast time;
- element;
- skill category;
- weapon skill;
- TP values;
- race;
- base stat;
- stat rank/grade;
- skill cap;
- formula;
- coefficient;
- cap/floor;
- resist tier;
- affinity;
- skillchain property;
- magic burst property;
- macro syntax;
- macro line;
- command keyword;
- equipment modifier;
- status ID;
- status potency;
- music/resource ID if observed;
- area/zone ID;
- client file/resource ID.

Rules:
- preserve raw spelling/case;
- preserve exact numeric value;
- preserve unit;
- preserve condition/context;
- preserve source version/date;
- preserve source identity;
- preserve evidence class;
- never overwrite raw_value with a derived value.

If a normalized/translated/calculated value is useful, add it in a separate field.

## 3. INTERPRETATION layer

Interpretation must:
- be stored separately;
- cite source IDs;
- be explicitly marked as interpretation/recommendation;
- never overwrite RAW_SOURCE or VERIFIED_FACT;
- never be presented as retail fact.

Existing files such as:
- AGENT_READ_FIRST.md
- FF11_REFERENCE_SPEC_V1.json
contain interpretation and synthesis.
They remain useful but are NOT raw source truth.

## 4. FF11 factual domains to preserve exactly

The raw fact catalog must cover, without abstraction:

### Character / progression
- races;
- gender/creation options where source-observed;
- levels;
- item level where applicable;
- experience/limit point/master-related values when sourced;
- HP/MP/stat values;
- STR/DEX/VIT/AGI/INT/MND/CHR;
- combat/magic skills and caps;
- job points/master levels/merit-style systems when sourced.

### Jobs
- every job;
- job IDs;
- unlock/level conditions when sourced;
- job traits;
- job abilities;
- recast/charge systems;
- main/support interactions;
- job-specific resources;
- job gifts/traits/bonuses where sourced.

### Magic
- every spell/ability record available from the source;
- name/ID;
- magic type/category;
- element;
- MP cost;
- cast/recast;
- level/job availability;
- range/target;
- skill;
- status/effect references;
- potency/duration/formula when source-proven;
- resist/accuracy rules when source-proven.

### Physical combat
- weapon types;
- weapon skills;
- skill ranks/caps;
- delay;
- TP gain;
- attack;
- defense;
- accuracy;
- evasion;
- critical;
- fSTR;
- WSC;
- fTP;
- pDIF;
- multi-attack;
- haste/delay-reduction categories;
- dual wield/martial arts effects;
- level correction where source-proven;
- damage formulas and caps/floors.

### Magic damage / resist
- dSTAT terms;
- base damage;
- multipliers;
- magic attack/defense;
- affinity;
- day/weather;
- magic accuracy/evasion;
- resist states/tiers;
- elemental resistance;
- magic burst;
- target adjustment;
- caps/floors;
- source-specific historical/current distinctions.

### Skillchains / bursts
- weapon-skill properties;
- skillchain levels/properties;
- valid property combinations;
- timing windows when source-proven;
- resulting properties;
- damage multipliers when source-proven;
- magic burst compatibility/bonuses when source-proven.

### Enmity
- cumulative/volatile components where sourced;
- action values;
- decay rules;
- damage/heal interactions;
- caps;
- job-specific enmity modifiers.

### Equipment / modifiers
- stat modifiers;
- latent/conditional modifiers;
- haste;
- store TP;
- dual wield;
- magic accuracy/attack;
- skill bonuses;
- weapon-skill damage;
- skillchain bonus;
- magic burst bonus;
- damage taken / resistance;
- set bonuses;
- augments;
- exact source text/value where legally and technically permissible.

### Macros / commands
- macro command syntax;
- command keywords;
- target selectors;
- waits;
- equipment-set commands;
- text commands;
- behavior/limits;
- line count/character limits when sourced;
- exact raw syntax.

### Battle music / audio metadata
- exact file/resource pointers and hashes from installed client;
- exact IDs/names only where legitimately exposed as factual metadata;
- no copying or redistributing audio bodies;
- consumer on source host reads original bytes in place if authorized.

### Client/resource data
- exact file paths;
- resource IDs;
- DAT paths;
- file hashes;
- sizes/timestamps;
- client version/build context.

## 5. No-loss image/audio rule

For user-supplied/shareable images/audio:
- byte-for-byte original only.

For installed FF11 protected images/audio:
- no extracted/copy asset body in shared repo;
- use original local path + SHA-256 + size + version;
- authorized same-host agent reads original bytes;
- no preview is accepted as source truth.

## 6. Required record shape

Every VERIFIED_FACT record must contain:
- fact_id
- domain
- field_name
- raw_value
- raw_value_type
- unit_or_encoding
- conditions
- source_id
- source_type
- source_location
- source_version
- evidence_class
- observed_at_or_source_date
- extraction_method
- source_sha256_if_file_backed

Optional additional fields may exist but may not replace raw_value.

## 7. Consumer rule

Before implementing or changing any FF11-derived system:
1. read this contract;
2. load RAW_SOURCE_INDEX.json;
3. load `FACT_CATALOG_INDEX.json`;
4. load every `facts/*.jsonl` listed by that index;
5. if file-backed, validate source SHA-256 before using it;
6. only then read interpretation/recommendation files;
7. if interpretation conflicts with raw/verified source, raw/verified source wins and the interpretation must be corrected.

## 8. Historical correction

Prior FF11 research summarized systems for DOLZORE implementation.
That work is preserved as historical interpretation but is no longer sufficient as the handoff source.

This contract supersedes any workflow that hands another agent only:
- a summary;
- a paraphrase;
- a transformed image;
- compressed media;
- a normalized formula;
- an inferred rule without provenance.

## 9. Continuity

Long-running extraction must checkpoint:
- scan_id;
- source host/version;
- completed domains;
- exact source IDs/hashes;
- last processed record;
- failures;
- next exact record/domain.

Resume from checkpoint. Do not restart from zero.
