# FF11-derived Unity architecture for DOLZORE

Authority: 2026-09-30 JST  
Reference source: `research/ff11/AGENT_READ_FIRST.md` and `research/ff11/FF11_REFERENCE_SPEC_V1.json`

This document records only the reusable architecture lessons applied to DOLZORE. It does not copy FINAL FANTASY XI names, maps, dialogue, art, formulas, data files, or proprietary implementation.

## Implemented now

### Character identity is permanent; vocation is changeable
`CharacterIdentityData` and `VocationStateData` are separate serialized models.

This lets one DOLZORE character change vocation without rerolling or replacing permanent identity.

### Stable world IDs are independent of Unity names
First Town uses `region.first_town.present`.

World entities use durable IDs such as:
- `entity.first_town.bar_13`
- `entity.first_town.journal`
- `entity.first_town.market_hall`
- `entity.first_town.east_station`

Unity GameObject names are presentation/editor labels only.

### Interactions target stable entity IDs
`InteractionAnchor` stores the stable target entity ID.

The player target model therefore survives GameObject renaming and future scene splitting.

### Persistent RPG state is pure data
`PlayerPersistentStateData` owns:
- identity
- vocation
- vitals
- status effects
- inventory
- loadouts/equipment slots
- active loadout
- current region/district
- current target entity

These models are deliberately independent of Tilemaps and scene object layout.

### Public / interior / private space boundaries are explicit
`WorldSpaceKind` defines:
- PublicRegion
- Interior
- PrivateInstance

`WorldPortal` reserves future transitions such as:
- First Town -> BAR interior
- First Town -> JOURNAL interior
- First Town -> Station interior
- First Town -> player-home private instance

No MMO backend is required yet; the boundary contract exists before networking.

### World time/state is separate from rendering
`WorldStateService` owns current region and world-time state.

Render objects do not become the authoritative world model.

### HUD is model-driven
The First Town HUD now reserves and reads:
- player name
- level
- HEART
- FOCUS
- vocation
- district/zone
- status state
- current target

## Deferred intentionally

Not implemented in this pass:
- combat build/spend resource
- coordinated chain/burst combat
- multi-component threat
- party/alliance networking
- server economy
- fishing/crafting runtime
- mentor/level-sync runtime
- companion system
- MMO authoritative persistence

These are later layers. Their data boundaries are not allowed to contaminate the current First Town vertical slice.

## Acceptance contract

This architecture pass is accepted only if:
1. Unity 6000.5.1f1 compiles it.
2. FirstTownShell regenerates.
3. stable region/entity IDs are serialized into the scene.
4. WebGL builds.
5. the existing playable First Town remains functional.
6. no current Canvas code is restored as the production game layer.
