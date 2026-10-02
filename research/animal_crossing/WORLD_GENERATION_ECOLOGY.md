# ANIMAL CROSSING — WORLD GENERATION / ECOLOGY

Authority: DeeDee-Lab/Dolzore#40

## 1. Cross-generation arc

Early games:
- generated/shared village identity;
- acre-based field concepts visible in public save research;
- native fruit;
- fixed civic/service topology with variation.

New Leaf:
- player civic authority increases;
- public works modify town.

New Horizons:
- island bootstrap;
- outdoor object placement;
- paths;
- river/cliff terraforming;
- building relocation;
- large player-authored world state.

## 2. Public reverse-engineering evidence

ACSE town model explicitly separates:
- Acres;
- Buildings;
- Island;
- NativeFruit;
- Shops;
- TownBuildings.

NHSE separates:
- Map structures;
- Building structures;
- field item editing;
- save offsets;
- hemisphere/weather state.

This supports a world model where topology, buildings, objects and ecology are separate persistent domains.

## 3. Original generation pipeline

Seed
-> choose macro topology
-> coast/boundary
-> river/water
-> elevation
-> districts/acres/chunks
-> civic anchors
-> residential lots
-> vegetation/native resource
-> traversal validation
-> collectible/ecology spawn zones
-> player-editable zones
-> save stable IDs.

Never generate final town from unconstrained random noise.

## 4. Chunk model

Use fixed stable chunks/sectors for:
- persistence;
- streaming;
- ecology updates;
- object ownership;
- navigation rebuild;
- multiplayer delta replication.

Chunk state:
- terrain;
- elevation;
- water;
- placed objects;
- vegetation;
- collectibles;
- dig spots;
- path/material;
- permissions;
- revision.

## 5. Terrain editing

New Horizons proves high-value player authoring from:
- placement;
- paths;
- water edits;
- cliff/elevation edits.

Original DOLZORE terraforming should use constraints:
- protected civic anchors;
- minimum route width;
- water connectivity rules;
- bridge/slope safety;
- navmesh regeneration;
- invalid-building overlap prevention;
- rollback transaction.

## 6. Ecology

Plant/collection world state should depend on:
- species;
- season;
- hemisphere;
- biome;
- time;
- weather;
- local density;
- regrowth timers;
- player actions.

Do not model plants as decorative static prefabs.

## 7. Trees / flowers / weeds

Persistent plant record:
- species;
- growth stage;
- planted_at;
- last_growth_update;
- health/water state;
- genetics/variant if used;
- fruit state;
- location;
- owner/source;
- event modifiers.

Weeds can act as absence/history signal but must not create punitive cleanup burden.

## 8. Spawn ecology

Separate:
SpeciesDefinition
from
SpawnRule.

SpawnRule dimensions:
- month/season;
- time range;
- weather;
- habitat;
- water type/height;
- biome;
- rarity;
- population cap;
- player progression;
- event override.

This is the engine behind long-term collection loops.

## 9. Persistent world history

A good life-sim map shows history:
- moved residents;
- grown trees;
- built structures;
- paths;
- seasonal decoration;
- museum progress;
- player-made designs.

Do not reset world appearance between sessions.

## 10. DOLZORE use

Animal Crossing should inform DOLZORE's living-town layer:
- long-term visible change;
- ecology;
- lots/housing;
- player decoration;
- schedule-sensitive resources.

Do not copy exact acre layouts, island shapes, tree/flower catalogs or terrain visuals.
