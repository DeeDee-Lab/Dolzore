# FIRSTTOWN V7 POSTMORTEM — 2026-10-01

## Incident

The user explicitly required that FirstTown be visually checked before being shown.
Canonical checkpoint already stated:
1. if build PASS, inspect generated screenshot manually;
2. compare against MOTHER2 Direct Visual Evidence + original references;
3. do not claim visual acceptance until the screenshot itself is acceptable.

The implementation/run passed technically, but the mandatory manual visual gate was not completed before the output was surfaced to the user.

This is a process violation.

## Root causes

### R1 — Technical acceptance was stronger than visual acceptance in the execution path

Automated verifiers check:
- route reachability;
- colliders;
- building overlap;
- road marking placement;
- HUD presence/binding;
- build/WebGL success.

They do not fail on:
- unattractive composition;
- road dominance;
- weak architecture;
- character appeal;
- mismatch to reference density;
- poor town atmosphere.

Therefore a bad-looking scene can be fully green.

### R2 — Patch-stack architecture

FirstTownBuilder applies:
- FirstTownVisualEnhancer
- FirstTownVisualRebuildV3
- FirstTownVisualRebuildV6
- FirstTownVisualRebuildV7

This is iterative correction on an old generated base rather than clean visual reconstruction from the latest evidence.

Consequence:
old assumptions survive and each version rearranges symptoms.

### R3 — Code-first geometric generation

V7 directly constructs long rectangular roads using Fill(), places primary buildings at numeric coordinates, adds a decorative rear row, controlled trees/bushes, and moves people to fixed coordinates.

This optimizes deterministic placement and validation, not organic/authored city composition.

### R4 — Research was advisory, not executable acceptance

The repository contains strong reference research and Direct Visual Evidence.
However, the build pipeline did not require a visual-review receipt proving the generated screenshot was compared to those references.

Therefore "research exists" did not mean "research constrained output."

### R5 — Failure to obey explicit pre-presentation instruction

This is the most important process failure.
The checkpoint explicitly required manual screenshot inspection after PASS.
The screenshot was not used as a blocking gate before user-visible presentation.

The user's instruction was not ambiguous.

## Why more information made the result worse

More research produced more constraints, while the implementation remained a simple code-generated blockout.
Instead of changing the production method, the agent attempted to satisfy new constraints through:
- road resizing;
- building relocation;
- scale changes;
- decorative backdrop rows;
- controlled greenery;
- fixed NPC relocation;
- stricter collision verification.

This increased technical regularity while reducing visual naturalness.

The information itself was not the cause.
The translation pipeline from evidence -> art direction -> authored composition -> visual QA was missing.

## Corrective architecture

REFERENCE EVIDENCE
-> visual target ranges / art direction
-> authored town block plan
-> actual art/layout production
-> technical validation
-> generate screenshot
-> mandatory actual-image visual review
-> comparison receipt
-> if FAIL: iterate without user presentation
-> if PASS: user-visible candidate
-> explicit user acceptance

## Stop conditions

Do not continue V8 as another small coordinate/color patch if the underlying town still uses the same road-dominant blockout grammar.

Before next candidate:
- choose a materially different composition method;
- reduce road dominance at the planning level;
- author denser facade blocks and varied building silhouettes;
- integrate greenery at edges rather than as rectangles;
- rebuild characters/avatar presentation to meet appeal bar;
- create social NPC clusters;
- compare screenshot before presentation.

## PDCA

PLAN:
Make visual quality and reference fit a hard gate, separate from technical correctness.

DO:
Create mandatory USER_VISIBLE_VISUAL_REVIEW_GATE and mark V7 VISUAL_FAIL.

CHECK:
Confirmed prior checkpoint already contained the required manual review instruction; confirmed automated QA could all pass without visual-quality checks; confirmed V7 code is patch-stack + coordinate/block generation.

ACT:
No future FirstTown candidate may be shown without actual-image review receipt PASS. Two same-reason failures force method change, not another small patch.

## Truth

USER_INSTRUCTION_CLEAR=true
USER_INSTRUCTION_CAUSED_FAILURE=false
PROCESS_VIOLATION=true
TECHNICAL_PASS_V7=true
VISUAL_PASS_V7=false
USER_VISIBLE_READY_V7=false
V7_USER_ACCEPTED=false
SMALL_PATCH_LOOP_FORBIDDEN=true
