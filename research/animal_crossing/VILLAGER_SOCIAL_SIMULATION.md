# ANIMAL CROSSING — VILLAGER / SOCIAL SIMULATION

Authority: DeeDee-Lab/Dolzore#40

## 1. Evidence

City Folk developer interview identifies a dedicated sequence-director role for:
- animal behavior;
- dialogue specifications;
- message writing.

Public reverse engineering reinforces that villager state is not one flat record:
- N64 decomp has NPC schedule/walk systems.
- ACSE separates VillagerData and AnimalMemories.
- ACSE models an AnimalMemory array; current public code exposes seven memory slots in one supported generation path.
- NHSE models revisioned Villager objects and separate VillagerHouse structures.

## 2. Architectural split

Original DOLZORE model should separate:

VillagerDefinition
- stable identity
- species/body type
- personality
- visual profile
- default home style
- interests
- voice/audio profile

VillagerPersistentState
- relationship values
- memories
- catchphrase/greeting/custom state
- move state
- gifts/outfits
- home modifications
- player-origin/social provenance
- conversation cooldowns
- event flags

VillagerRuntimeState
- current schedule activity
- world position
- current target
- emotion/reaction
- dialogue context
- path state
- temporary needs

## 3. Schedules

Resident life should be composed from:
- wake/sleep profile;
- home/shop/outdoor activity windows;
- weather reaction;
- event participation;
- social destination;
- hobby activity;
- player-request interruption;
- move/visit state.

Do not hardcode every villager with a full bespoke timetable.
Use schedule archetypes + personality + event overrides.

## 4. Social memory

Memory should be event-based, not raw transcript storage.

MemoryEvent:
- memory_type;
- other_entity;
- timestamp;
- place;
- item/topic;
- magnitude;
- sentiment;
- expiry/decay;
- dialogue_tags.

Examples:
- received gift;
- player helped request;
- repeated neglect;
- birthday interaction;
- visit;
- letter;
- disagreement;
- shared event.

## 5. Relationship model

Avoid one visible "friendship XP bar" as the whole system.

Possible hidden components:
- familiarity;
- trust;
- affection;
- reciprocity;
- recent-contact;
- conflict;
- shared-history count.

Dialogue and behavior can query a compressed tier while deeper values remain internal.

## 6. Conversation selection

DialogueContext should include:
- speaker;
- listener;
- relationship tier;
- recent memory;
- current activity;
- current place;
- time;
- weather;
- season;
- event;
- player inventory/outfit;
- nearby NPCs;
- repeat count;
- recent topic cooldown.

Selection:
eligible pool -> remove cooldown conflicts -> weight by context/personality -> choose -> write memory/cooldown.

## 7. Moving in/out

Treat move state as a transaction/state machine:
Candidate
-> Invited/Selected
-> MoveScheduled
-> Packing
-> Resident
-> ConsideringMove
-> Leaving
-> HistoricalResident.

Preserve historical linkage so returning characters can reference prior residency.

## 8. House coupling

Villager home is persistent personal expression.
Separate:
- building lot;
- house shell/exterior;
- room layout;
- furniture placements;
- ownership/provenance;
- baseline design;
- player-gift modifications.

NHSE's separate VillagerHouse structures support the usefulness of this separation as a save-model pattern.

## 9. Population simulation

Town should have:
- active residents;
- special visitors;
- service NPCs;
- transient visitors;
- visiting players.

Do not run expensive high-frequency AI for every resident when offscreen.
Use:
- simulated schedule state while unloaded;
- instantiate near player;
- derive position from schedule anchors when loaded.

## 10. DOLZORE design target

Adopt:
- persistent residents;
- social memory;
- schedule-driven life;
- relationship-aware dialogue;
- home identity;
- move history.

Do not copy:
- Animal Crossing personality labels/scripts;
- catchphrases;
- resident names/species designs;
- exact friendship formulas;
- exact dialogue text.

## Source anchors

Official:
- https://www.nintendo.co.jp/wii/interview/ruuj/vol1/index.html

Reverse engineering:
- zeldaret/af NPC schedule/walk
- Cuyler36/ACSE @ 23563b168f4afd705aa3b48da807978644c715df
- kwsch/NHSE @ cb0745415945776f73375bf0a434a8babf059307
