# DOLZORE FIRST TOWN — TOWN CONCEPT CONTRACT

Authority: 2026-09-30 JST
Research input: `research/mother2/`
Implementation engine: Unity 2D
Internal RPG authority: FF11-derived DOLZORE core

This contract applies high-level MOTHER2 research without copying protected maps, sprites, palettes, UI, dialogue, music, or exact event composition.

## Emotional role

A friendly everyday town that feels safe enough to wander without objectives, while quietly introducing the first signs that reality is slightly misaligned.

The player should first understand the town as a place people actually live.

Only after that baseline is readable should ZURE feel strange.

## Everyday function

The town is simultaneously:
- home neighborhood;
- ordinary shopping/service district;
- BAR/music meeting place;
- JOURNAL/information district;
- workshop/market lane;
- riverside leisure area;
- station/east-gate travel node.

The player should be able to log in and walk around without combat or quest progression.

## Local anomaly

The anomaly is subtle:
ordinary objects, schedules, sounds, reflections, or routes occasionally disagree with remembered reality.

Do not make the entire town visually surreal.
Normality is the contrast mechanism.

## Mandatory story function

The First Town teaches:
- movement;
- interaction;
- map/landmark navigation;
- interior boundary logic;
- recurring residents;
- the first BAR/JOURNAL relationship;
- first ZURE observation later.

## Population types

Required visible social texture:
- SORA/player;
- MELO;
- YUZU;
- PON;
- worker/service resident;
- child/youth;
- older established resident;
- visitor/commuter;
- odd-but-locally-coherent resident.

At least half of visible residents should not exist merely to issue quests.

## Visual identity

High-level target:
- readable 16-bit-inspired symbolic world;
- small characters relative to architecture;
- strong dark silhouette edges;
- broad color blocks over microtexture;
- roof/front/side-plane building grammar;
- ordinary streets and sidewalks;
- lawns, fences, trees, parked vehicles, signs, benches, utilities;
- district identity survives with HUD/labels hidden;
- coherent world scale between character/door/bench/car/tree.

Do not trace or reproduce MOTHER2 geometry or sprite arrangements.

## Density plan

Residential Hill:
D1 lived-in.
Lawns, fences, mail, bicycles, trees, neighbors.

Central Main Street:
D2 social.
BAR/shops, benches, parked vehicles, signs, recurring NPCs.

Market/Workshop:
D2 practical.
Industrial/service props, worker population, loading/storage cues.

Civic/JOURNAL:
D2 information/social.
Public facade, seating, map/signage, residents with town knowledge.

Riverside:
D0-D1 recovery edge.
Large tree, bench, water sound later, sparse interactions.

Station/East Gate:
D1-D2 transition.
Transit props, commuter/visitor population, clear world-exit cue.

Back Alley / Strange Pocket:
D0-D1 optional oddity.
Cat, vending machine, quiet prop cluster, later ZURE clue.

## Traversal identity

Required:
- primary loop;
- secondary route;
- optional spur;
- alternate return/shortcut where practical;
- no pixel-perfect mandatory movement;
- BAR/JOURNAL/RIVERSIDE/STATION must remain reachable through actual collision.

Current collider-aware acceptance is implemented and produces:
`BuildArtifacts/first-town-acceptance.json`.

## Memory anchors

At least three:
1. residential water tower / overlook identity;
2. BAR frontage + social pocket;
3. riverside landmark tree / quiet bench.

Additional:
- odd cat / quiet vending pocket;
- station clock/sign;
- civic/JOURNAL facade.

## Audio identity

Planned states:
- TOWN_DAY;
- QUIET_EDGE_OR_RIVER;
- BAR interior/diegetic jukebox;
- JOURNAL interior;
- ANOMALY override.

Do not use one generic loop for every place.

## Arrival experience

Player begins in a readable lived-in residential/main-street transition.
Within one camera view:
- one clear road;
- one landmark;
- at least one resident;
- one optional prop interaction candidate.

## Exit experience

Station/east gate visually foreshadows expansion.
The player should understand where the larger world continues before activating the transition.

## Post-event state

At least two channels must change:
- NPC placement/dialogue;
- audio;
- props/lighting/service availability.

## Current implementation checkpoint

Latest validated exterior pass:
- Unity run `36636292647 = SUCCESS`
- generated source branch commit after run: `5e1b945f0cd2801417aa0f81111359764d27dc2a`
- route acceptance: BAR/JOURNAL/RIVERSIDE/STATION PASS
- WebGL PASS
- generated-source preservation PASS
- V3 visual grammar: active
- silhouette outline pass: active

This is the current production direction, not a final art lock.
