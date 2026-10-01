# DOLZORE FirstTown 2.5D comparison prototype

Date: 2026-10-01 JST

## Purpose

Build a narrow comparison slice before any engine-direction decision.

This prototype does **not** replace the canonical 2D Unity branch and does **not** declare a 3D migration complete.

## Scope

- central intersection only;
- fixed three-quarter / orthographic 3D camera;
- original low-poly DOLZORE town forms;
- 6 buildings;
- 4 correctly positioned crosswalk groups around the intersection, not ladder-like markings in the intersection center;
- trees, benches, street lamps, fences, signs and cars;
- 1 controllable player and 4 NPC figures;
- WebGL comparison build;
- screenshot artifact for direct 2D-vs-2.5D visual review.

## Visual reference authority

The user supplied a screenshot on 2026-10-01 showing a 3D interpretation of a MOTHER2 town.

Use only high-level lessons visible in that reference:
- fixed readable three-quarter camera;
- compact low-poly town scale;
- strong building/road height separation;
- frequent street furniture and vegetation;
- bright but shaded materials;
- low-head-height character silhouettes;
- clear road hierarchy and intersections.

Do **not** copy protected MOTHER2 maps, exact building placement, models, textures, characters, signs, UI, palette values or other protected expression.

The user-supplied third-party screenshot is not duplicated into the public repository.

## Decision gate

Do not choose 2D vs 2.5D from theory.

Compare:
1. current rejected 2D FirstTown screenshot;
2. generated `first-town-25d.png`;
3. implementation burden;
4. character/avatar extensibility;
5. town authoring speed;
6. WebGL performance.

No full migration until the user-visible comparison is materially better.


## 2026-10-01 quality benchmark checkpoint

Current branch: `prototype/first-town-25d-20261001`

Latest full green run:
- GitHub Actions run `36802886825 = SUCCESS`
- baseline 2.5D generation: PASS
- quality benchmark generation: PASS
- quality receipt verification: PASS
- quality visual artifact upload: PASS
- quality WebGL build: PASS

Quality benchmark scene:
- `Assets/Scenes/FirstTown25DQualityBenchmark.unity`
- screenshot: `BuildArtifacts/first-town-25d-quality.png`
- receipt: `BuildArtifacts/first-town-25d-quality-receipt.json`
- WebGL: `Builds/WebGL25DQuality/`

Implemented quality changes:
- deterministic procedural textures for grass, road, sidewalk, stucco, brick, roofs and wood;
- layered building facades with foundations, corner posts, fascia, framed windows, doors, porches, steps, signs, roof ridges and chimneys;
- side-wall windows to avoid blank box facades;
- authored street props including planters, benches, lamps, hydrant, trash can, bike rack, mailbox, street sign, utility cabinet, drains and manhole;
- three distinct original buildings: BAR 13, MOON CAFE, CLOCK HOUSE;
- three original chibi character silhouettes with backpack/bag/scarf identity cues;
- fixed perspective three-quarter camera;
- stronger directional lighting and reduced ambient wash;
- hard static-object road-intrusion QA;
- manual screenshot review remains mandatory after CI.

Important truth:
- this is materially better than the original primitive 2.5D blockout;
- it is still a quality benchmark, not the completed FirstTown;
- do not expand to Town 2 or claim production visual acceptance yet;
- next step is to reuse this authored 2.5D pipeline across the rest of FirstTown while continuing character/model polish and district identity work.

`FIRST_TOWN_25D_DIRECTION_ACTIVE=true`
`FIRST_TOWN_25D_QUALITY_BENCHMARK_GREEN=true`
`FIRST_TOWN_COMPLETE=false`
`NO_FALSE_COMPLETION=true`
