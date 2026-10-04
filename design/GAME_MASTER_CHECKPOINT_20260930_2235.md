# 2026-10-04 DIRECT-USER OVERRIDE — PUBLIC HP STATE

The historical Railway URL below is CLOSED/RETIRED and is NOT the current DOLZORE HP:
`https://dolzore-web-runtime-production.up.railway.app/`

Do not return it as the current HP.
Do not infer a replacement URL.
A new canonical HP requires fresh public verification + direct-user approval.

`CURRENT_CANONICAL_HP_URL=NONE`
`RETIRED_RUNTIME_HP_URL=https://dolzore-web-runtime-production.up.railway.app/`
`RETIRED_URL_MUST_NOT_BE_PRESENTED_AS_CURRENT=true`

---

# GAME MASTER CHECKPOINT — 2026-09-30 22:35 JST

Resume phrase: **Gameひきついで**

## Read order
1. this file
2. design/HANDOFF_CURRENT_STATE.md
3. design/GAME_CANONICAL_DIRECTION.md
4. design/PUBLIC_URL_MIGRATION_MANIFEST.md
5. research/mother2/AGENT_READ_FIRST.md
6. research/mother2/DIRECT_VISUAL_EVIDENCE_PACKET_V1.md
7. research/mother2/GAMEPEDIA_TOWN_IMAGE_SOURCE_MANIFEST_V1.json
8. research/ff11/AGENT_READ_FIRST.md
9. latest Issue #3 and #26 checkpoints
10. active Unity branch rebuild/unity-first-town-20260929

Global policies:
- global.detailed-handoff-evidence.v1
- global.original-image-transfer.v1

## Public website
Verified new public URL:
https://dolzore-web-runtime-production.up.railway.app/

Fresh external check at checkpoint:
- / PASS
- /business PASS
- intended DOLZORE new-site content visible

Previously green routes:
- /creator
- /apps
- /buying-guide
- /about
- /privacy
- /terms
- /health

Railway evidence:
- project: dolzore-web
- service: dolzore-web-runtime
- deployment: 5041f5ad-b97d-4ce7-bce0-6d9aae9c30b3
- runtime marker: DOLZORE_SITE_READY 8080

Legacy Lovable:
https://dolzore.lovable.app/
- old business/marketing content retired
- currently closure tombstone + noindex
- do not make it canonical again

Old official Lovable alias:
https://dolzore-official.lovable.app/
- was 404 after shared-project incident
- not canonical

URL migration:
- design/PUBLIC_URL_MIGRATION_MANIFEST.md
- full SNS/ads/profile migration NOT complete
- Metricool brand: dolzoreofficial / id 6845577
- Facebook: 1341212082404928
- Instagram: dolzoreofficial
- TikTok: dolzoredolzore
- YouTube: UC5-Ya1fXY8tYaD3doZBg_cg
- X actual account unresolved
- old dolzore.lovable.app appears in 9 GitHub code references across Automation + Dolzore

## FirstTown
Engine: Unity 2D only.
Internal simulation: FF11-derived core remains fixed/green.
Visual input: Mother2 original-image evidence, not summaries alone.

Last technical green:
V6 run 36675601306 = SUCCESS

But V6 is visually rejected by user due:
- broken building placement
- overlap / composition failure
- still far from target visual

Therefore V6 is NOT accepted.

### V7
Active branch:
rebuild/unity-first-town-20260929

Key files:
- game/unity/bootstrap/Assets/Editor/FirstTownVisualRebuildV7.cs
- game/unity/bootstrap/Assets/Editor/FirstTownVisualPlacementVerifier.cs

V7 QA rejects:
- building collider on road
- primary building overlap
- building outside world bounds
- people inside building colliders

Previous V7 run:
36679845415 = FAILURE

Exact error:
DOLZORE_V7_BUILDING_ON_ROAD:BAR 13:cell=(-15, 6, 0):cellCenter=-14.50,6.50:bounds=[-15.15,6.35]-[-10.85,8.35]

Analysis:
bounds reflected stale pre-V7 physics transform state.

Fix:
commit 093f3b0330ac9577640f3cd3506b7b99ae906206
added Physics2D.SyncTransforms() before placement QA.

Current validation run at checkpoint:
36723025197 = IN_PROGRESS

Exact next action:
1. fresh-read run 36723025197
2. if FAIL, fix exact placement error; do not weaken verifier
3. if PASS, inspect generated screenshot manually
4. compare against Mother2 Direct Visual Evidence Packet + original Gamepedia references
5. do not claim visual acceptance until screenshot itself is acceptable

## Mother2 evidence
Current source corpus:
- 15 town/field pages
- 70 exact original media URLs
- original references are canonical
- thumbnail/compressed derivative is not canonical
- production Agents must inspect originals

## Avatar
Canonical pipeline:
- AvatarSystem.cs
- AvatarAssetGenerator.cs
- AvatarQualityVerifier.cs

Latest green avatar base:
36662901682 = SUCCESS

Wardrobe base combinations before color variants:
725,760

Do not create parallel avatar systems.

## Current truth flags
NEW_HP_PUBLICLY_AVAILABLE=true
NEW_HP_CORE_ROUTES_VERIFIED=true
FULL_EXTERNAL_URL_MIGRATION_COMPLETE=false
FIRST_TOWN_V7_ACTIVE=true
FIRST_TOWN_VISUAL_ACCEPTANCE=false
GAME_HANDOFF_READY=true
GAME_RESUME_PHRASE=Gameひきついで
NO_FALSE_COMPLETION=true
