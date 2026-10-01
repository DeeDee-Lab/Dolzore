# FirstTown 3D Area Checkpoint

Updated: 2026-10-01 JST

## User direction

- Use the cross-city FF11 measurements to define a reusable DOLZORE town design standard.
- Switch production direction to true 3D.
- Build one area first and show the actual Unity result before expanding FirstTown.

## Completed

- Created branch: prototype/first-town-3d-area-20261001
- Recorded representative FF11 city spatial observations:
  - Bastok Markets
  - Southern San d'Oria
  - Windurst Waters
  - Lower Jeuno
- Created DOLZORE 3D Town Design Standard V1.
- Implemented true-3D Crossroads Quarter generator.
- Added controlled perspective third-person camera.
- Added dedicated Unity/OSK222/WebGL workflow.

## DOLZORE derived target

- area: 96m x 72m
- primary road: 7.2m
- secondary road: 5.4m
- sidewalk: 2.0m
- active interest spacing: 6-10m
- quiet edge spacing: 16-24m
- landmark spacing: 30-45m
- elevation change: 1.5-3.0m

## Area content

Crossroads Quarter:
- BAR 13
- JOURNAL Hall
- Corner Market
- Workshop
- Clock Court landmark
- raised civic terrace + stairs
- diagonal secondary road + service loop
- player + 7 NPCs
- trees, lamps, benches, planters, signs, service props
- true 3D colliders
- perspective camera

## Current finish lane

Workflow run 36825796440 is the first generation/build attempt.

Do not expand the town until:
1. Unity generation passes;
2. screenshot artifact is inspected manually;
3. obvious placement/rendering failures are fixed;
4. WebGL build passes;
5. the actual screenshot is shown to the user.

FIRST_TOWN_COMPLETE=false
NO_FALSE_COMPLETION=true
