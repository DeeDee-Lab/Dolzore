# DOLZORE Unity game layer

Canonical implementation branch: `rebuild/unity-first-town-20260929`.

This project is the production WORLD/game lane. The existing GitHub Pages Canvas town remains a rejected temporary prototype until the Unity first-town vertical slice passes source, build, visual and functional acceptance.

## Current slice

1. Deterministic Unity project bootstrap.
2. Original title screen with NEW GAME / CONTINUE / SETTINGS / BGM.
3. FIRST TOWN foundation with player movement, camera follow, separated collision, HUD and minimap shell.
4. GitHub self-hosted audit/build receipt before any public replacement.

UnityMCP is pinned as an editor bridge only. GitHub remains the canonical source/evidence plane; MCP is not a second control plane.

## Generated scenes

The committed Editor bootstrap creates `Assets/Scenes/Boot.unity` and `Assets/Scenes/FirstTown.unity` deterministically from committed C# source. CI regenerates them before validation/build. Generated scene/meta output is build material, not a separate authority.

## Truth gates

`UNITY_PROJECT_PRESENT=true` only means source exists.
`UNITY_FIRST_TOWN_SLICE_ACCEPTED=true` requires a real Unity editor/build receipt plus visual review and gameplay QA.
No BAR, fishing, combat, second town or MMO implementation is authorized before that gate.
