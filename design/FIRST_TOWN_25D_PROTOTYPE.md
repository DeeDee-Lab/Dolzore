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
