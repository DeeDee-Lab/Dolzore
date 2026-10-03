# GAME ANALYSIS HANDOFF — CANONICAL ENTRY

Trigger phrase: **ゲーム分析ひきついで**

Authority: direct user continuity instruction
Canonical repo: `DeeDee-Lab/Dolzore`

## Mandatory resume rule

When the user starts a new chat with **「ゲーム分析ひきついで」**:

1. Fresh-read this file first.
2. Do NOT ask the user to repeat project details.
3. Fresh-read each project's `RESEARCH_STATUS.json`.
4. Fresh-read the latest comments on the canonical issue.
5. Resume only from each project's latest exact next action.
6. Do not redo completed research.
7. Preserve source-fidelity rules: RAW SOURCE / exact facts before interpretation.
8. For long work, checkpoint material progress to the canonical issue and status files.

---

# 1. MOTHER2 / EarthBound research

Canonical path:
`research/mother2/`

Canonical issue:
`DeeDee-Lab/Dolzore#26`

Start reading:
1. `research/mother2/AGENT_READ_FIRST.md`
2. `research/mother2/DIRECT_VISUAL_EVIDENCE_PACKET_V1.md`
3. `research/mother2/GAMEPEDIA_TOWN_IMAGE_SOURCE_MANIFEST_V1.json`
4. `research/mother2/MOTHER2_REFERENCE_SPEC_V1.json`
5. `research/mother2/FIRST_TOWN_AUTHORING_CONTRACT_V1.json`
6. `research/mother2/RESEARCH_STATUS.json`
7. Issue #26 latest checkpoint

Current truth:
- Structural reference V1 is ready.
- Quantitative visual research is NOT complete.
- Direct visual evidence is mandatory; abstract-only handoff is forbidden.
- Original third-party image bodies are not republished; canonical source URLs are preserved.
- Gamepedia corpus currently records 15 town/field pages and 70 exact original media URLs.
- User-visible DOLZORE visuals have a hard gate:
  GENERATE -> OPEN ACTUAL ARTIFACT -> COMPARE REFERENCES -> RECORD MISMATCHES -> VISUAL PASS/FAIL -> only PASS may be shown as candidate.
- Technical pass is NOT visual pass and is NOT user acceptance.

Next exact action:
- expand representative original-image evidence;
- measure street/building/character/greenery/palette ratios without replacing source evidence;
- derive DOLZORE original target ranges;
- compare FirstTown output against source corpus before presentation.

---

# 2. FINAL FANTASY XI research

Canonical path:
`research/ff11/`

Canonical issue:
`DeeDee-Lab/Dolzore#25`

Mandatory source-fidelity reading order:
1. `research/ff11/RAW_SOURCE_HANDOFF_CONTRACT.md`
2. `research/ff11/RAW_SOURCE_INDEX.json`
3. `research/ff11/VERIFIED_FACT_RECORD_SCHEMA_V1.json`
4. `research/ff11/FACT_CATALOG_INDEX.json`
5. every `research/ff11/facts/*.jsonl` listed by the index
6. only then `AGENT_READ_FIRST.md` and `FF11_REFERENCE_SPEC_V1.json`
7. Issue #25 latest checkpoint

Absolute rule:
- Do not replace FF11 source information with summaries, normalization, paraphrases, resized/compressed images, transcoded media, or inferred substitutes.
- Exact values/IDs/formulas/syntax stay exact.
- Protected FF11 asset bodies are not redistributed; use exact original local path + hash + size + version and authorized same-host read-in-place.
- Interpretation is downstream of RAW_SOURCE / VERIFIED_FACT.

SPC source identity:
- Host: `SOWCIEPC`
- FF11 root: `C:\Program Files (x86)\PlayOnline\SquareEnix\FINAL FANTASY XI`
- files: 65,329
- total bytes: 14,860,167,302
- DPC_USED=false for current SPC quantification lane
- DC/RDC_USED=false
- game mutation/input=false

Latest verified render-data checkpoint on Issue #25 includes:
- race skeleton/animation container observations;
- partial HumeM geometry;
- partial texture census;
- partial object/placement geometry;
- authored LOD fields;
- source-parser discrepancy tracking.
Exhaustive render budgets remain false.

Important:
`research/ff11/DPC_SCAN_STATUS.json` is the DPC-lane status only and is older than the latest Issue #25 SPC/render checkpoints. Do not treat it as the global FF11 latest state.

Current next exact action:
- continue source-fidelity exact extraction;
- repair/continue exhaustive SPC parser scan without repeating failed allocation path;
- ingest metadata-only artifacts;
- persist exact verified facts with source identity;
- complete requested domains including magic/jobs/skills/levels/stats/races/damage/modifiers/resist/skillchains/macros/audio/resource metadata without abstraction.

---

# 3. Animal Crossing / どうぶつの森 complete-series research

Canonical path:
`research/animal_crossing/`

Canonical issue:
`DeeDee-Lab/Dolzore#40`

Start reading:
1. `research/animal_crossing/AGENT_READ_FIRST.md`
2. `research/animal_crossing/VERIFIED_FACT_RECORD_SCHEMA_V1.json`
3. `research/animal_crossing/FACT_CATALOG_INDEX.json`
4. every listed `facts/*.jsonl`
5. domain architecture files
6. `research/animal_crossing/RESEARCH_STATUS.json`
7. Issue #40 latest checkpoint

Governance:
- Issue #40 is the single canonical owner.
- Issue #42 was an accidental duplicate and is closed; never revive it.

Current validated truth:
- comprehensive game-development domain corpus: ready
- exact fact catalog: useful
- latest synchronized validation: 1,098 exact fact records / 28 JSONL / parse PASS
- protected Nintendo asset bodies stored: false
- public-source exhaustion complete: false
- proprietary internal completeness: false
- unavailable proprietary/server/source-tree/runtime internals must be marked SOURCE_UNAVAILABLE, never guessed

Completed/high-depth areas include:
- N64 architecture/time/weather/schedules
- GameCube decomp baseline
- full GameCube annual event schedule
- full GameCube insect spawn tables
- full GameCube fish spawn tables
- spawn scheduler linkage
- GameCube audio state machine
- villager social/memory structures
- Wild World / New Leaf / Welcome amiibo save research
- New Leaf/WA system state and editor offset expressions
- City Folk save/toolkit structures
- HHD structures
- New Horizons 3.x save structures/offsets
- Pocket Camp Complete 221-table public save schema
- time/season/offline simulation
- social/villager simulation
- world generation/ecology
- economy/collection
- housing/customization
- multiplayer/social architecture
- audio/environment
- save-generation matrix

Current exact next actions:
1. continue remaining N64 public decomp structs/events/audio;
2. continue remaining GameCube actor/shop/item/mail/house tables;
3. complete remaining Wild World source-expression catalog;
4. City Folk ACSE cross-check;
5. New Leaf/WA remaining building/PWP/house algorithms;
6. New Horizons full public Structures manifest;
7. HHD remaining public structures;
8. Pocket Camp Complete cross-validation;
9. original Pocket Camp publicly available local model only;
10. Switch 2 official runtime/input feature deltas without private-engine claims.

Continuity rule:
- do not restart completed generations;
- append exact deltas only;
- rerun catalog recount after material batches;
- distinguish PUBLIC_SOURCE_EXHAUSTED from SOURCE_UNAVAILABLE.

---

# 4. FF14

Known deployment fact:
- FF14 is installed on both DPC and SPC.

Current state:
- standalone canonical `research/ff14/` corpus has not yet been established to the same maturity as FF11/MOTHER2/Animal Crossing.
- Do not silently treat FF11 findings as FF14 findings.
- If user asks to begin/continue FF14 analysis, create/fresh-read a dedicated canonical FF14 research path and preserve the same RAW_SOURCE -> VERIFIED_FACT -> INTERPRETATION layering.

---

# 5. Keyword routing

If the user's new-chat message is exactly or substantially:
- 「ゲーム分析ひきついで」
- 「ゲーム分析を引き継いで」
- 「前のゲーム解析を続けて」

Then:
- start from this file;
- report which game corpus is active if the user named one;
- if no game was named, recognize all three active corpora and continue the most recently active one (currently Animal Crossing), while keeping FF11 and MOTHER2 state intact;
- do not ask for a repeat of known requirements.

Current most-recent active corpus:
**Animal Crossing #40**

Global no-restart / no-false-completion rule applies.
