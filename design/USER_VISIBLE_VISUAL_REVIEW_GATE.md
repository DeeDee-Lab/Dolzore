# USER-VISIBLE VISUAL REVIEW GATE — MANDATORY

Authority: direct user correction, 2026-10-01 JST
Scope: all DOLZORE user-visible visual artifacts, including FirstTown, avatars, UI, interiors, maps, battle screens and promotional screenshots.
Status: HARD GATE

## 0. Rule

NO visual artifact may be presented to the user as a candidate, improvement, completion, preview, or "current result" merely because:
- Unity build succeeds;
- route/collision tests pass;
- WebGL builds;
- automated image metrics pass;
- source compiles;
- an agent claims it looks better.

Before user-visible presentation, the responsible agent/assistant MUST open the actual generated image/artifact bytes and visually inspect them.

## 1. Required order

1. Generate artifact.
2. Obtain exact artifact from the successful run/build.
3. Open the actual screenshot/image.
4. Open/read the canonical original-reference evidence required by the task.
5. Perform direct visual comparison.
6. Record concrete mismatches.
7. Decide PASS or FAIL.
8. If FAIL:
   - do not present it as a candidate;
   - return to implementation;
   - preserve failure evidence/checkpoint;
   - only show it to the user if the user explicitly asks to see rejected/debug output.
9. If PASS:
   - persist a visual-review receipt;
   - only then may it be presented as a candidate for user acceptance.

TECHNICAL_PASS != VISUAL_PASS
VISUAL_PASS != USER_ACCEPTED

## 2. FirstTown mandatory references

Before FirstTown visual review:
- research/mother2/DIRECT_VISUAL_EVIDENCE_PACKET_V1.md
- research/mother2/GAMEPEDIA_TOWN_IMAGE_SOURCE_MANIFEST_V1.json
- research/mother2/FIRST_TOWN_AUTHORING_CONTRACT_V1.json
- design/AVATAR_DRESSUP_SYSTEM_V2.md
- newest explicit user visual corrections
- actual generated FirstTown screenshot

Original reference images/source URLs are authoritative where available. Summaries alone are insufficient.

## 3. FirstTown review dimensions

The reviewer must explicitly inspect:
- road/street visual share;
- building density;
- building vertical mass;
- facade/detail rhythm;
- green/vegetation integration;
- empty-space ratio;
- diagonal/sloped edge variety;
- NPC density and clustering;
- character-to-building/door scale;
- character silhouette/appeal;
- crosswalk/road-marking alignment;
- composition focal hierarchy;
- whether the frame reads first as a town rather than a road/blockout;
- whether visual quality is materially improved over the previous rejected version.

A regression in any major dimension blocks presentation.

## 4. Required visual-review receipt

Persist a machine-readable receipt containing:
- artifact_path
- artifact_sha256 or workflow artifact digest
- source_commit
- workflow_run_id
- reference_packet_ids
- actual_image_opened = true
- reviewer
- review_timestamp
- mismatch_list[]
- regression_vs_previous
- decision = PASS|FAIL
- user_visible_ready = true|false
- user_accepted = false until explicit user acceptance

No receipt means no user-visible candidate.

## 5. Anti-loop

If two consecutive versions fail for the same dominant visual reason, do not make a third small positional/color patch.
Change the production method or base art/layout architecture.

For FirstTown specifically:
- stop stacking V3/V6/V7-style corrective layers when the base composition is wrong;
- do not optimize only for collision and route validity;
- rebuild visual composition from an authored town plan/reference comparison when required.

## 6. Reporting

When a build is technically green but visually failed, report internally:
TECHNICAL_PASS=true
VISUAL_PASS=false
USER_VISIBLE_READY=false

Do not send the failed screenshot to the user as a candidate.

## 7. Current V7 ruling

Artifact:
Unity run 36723025197
source commit 093f3b0330ac9577640f3cd3506b7b99ae906206
generated head 6133014139a7a945ba372e9385de306030d67286

Technical result:
PASS

Visual review:
FAIL

Reasons:
- road/intersection still dominates the frame;
- building density and continuity remain too low;
- repeated simple detached-building grammar;
- insufficient vertical/facade mass;
- large rectangular green/empty areas;
- NPCs remain sparse/placed rather than socially clustered;
- characters remain weak relative to desired world quality;
- composition reads as a test/blockout map, not a convincing authored town;
- overall visual result does not meet the user's stated reference/quality bar.

Therefore:
USER_VISIBLE_READY=false
USER_ACCEPTED=false

This V7 image is retained only as rejected evidence.
