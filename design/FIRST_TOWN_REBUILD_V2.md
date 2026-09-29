# DOLZORE FIRST TOWN REBUILD v2
Authority: 2026-09-29 JST
Status: current public town REJECTED as visual/world-design baseline

## 0. Decision

The currently deployed first town is NOT accepted as the production-quality foundation.

It proved controls and deployment, but fails as a town:
- too frontal/flat;
- buildings read as isolated boxes;
- street network is too shallow;
- no real district structure;
- no meaningful vertical/depth hierarchy;
- too few landmarks;
- environment density is low;
- buildings do not yet feel lived in;
- town does not yet reward wandering.

Do not expand the MMO/RPG world directly from this map.

Build a replacement first town on branch:
`redesign/first-town-v2-20260929`

Keep the public current town only as a temporary playable prototype until v2 replaces it.

---

# 1. Reference-study conclusions

## MOTHER2 — what matters structurally

Official Nintendo screenshots show ordinary modern streets used as adventure space:
- roads are real navigable space, not background decoration;
- sidewalks, curbs, lamp posts, parked vehicles, signs, trees and storefronts establish scale;
- towns feel ordinary first, strange second;
- NPCs and unusual events coexist with mundane infrastructure;
- exploration moves naturally between streets, buildings, open ground and odd side places.

Important lesson:
**Do not build "three shops on one horizontal stage." Build a town block network.**

MOTHER2 also begins from a calm town and grows outward into a world-spanning journey.
The world feeling comes from moving through connected places rather than selecting isolated menu locations.

## FFXI — what matters structurally

FFXI established multiple starting nations, each with its own identity and adventure.
Its city structure is functionally divided into named districts/ports/markets.

Lessons:
- one city needs multiple functional districts;
- city identity comes from economy, culture, routes and NPC purpose;
- different entrances should lead to different player routines;
- transport and district boundaries help players build mental maps;
- starting city is not only visual—it defines early life/job/economy patterns.

## FFXIV — what matters structurally

FFXIV city-states also use strong district/layer identities:
- Limsa Lominsa Upper / Lower Decks
- Old / New Gridania
- Ul'dah Steps of Nald / Steps of Thal

Official Lodestone shop data demonstrates that important services are distributed across these named city districts.

Lessons:
- vertical/layered geography can create identity;
- landmarks + districts + services create navigation;
- markets/guilds/transport/social spaces should not all sit on one street;
- a city should support routine behavior even when the player is not questing.

---

# 2. Open-world decision

## Fiction
One connected world.

## Player experience
Open-world feeling:
- walk out of town toward river/road/fields;
- no "select destination" menu for normal movement;
- interiors entered from physical doors;
- multiple routes and shortcuts;
- world state persists.

## Technical implementation
**Open-region / seamless-region architecture**, not one monolithic infinite map.

Each large region:
- streams/scenes internally;
- can be hosted as a separate MMO shard/zone later;
- has natural boundaries such as road, station, bridge, tunnel, forest pass, time-layer seam.

Benefits:
- browser memory stays manageable;
- asset loading stays manageable;
- MMO player counts can be distributed;
- time-layer versions can be instanced;
- housing/story interiors can be instanced;
- dense content is prioritized over empty land.

Rule:
**The player should feel "open world"; the server does not need to be one giant zone.**

---

# 3. First town dimensions

Native tile:
16×16 px

Player:
32×40 px

Target first-town exterior:
approximately 96×72 tiles
= 1536×1152 logical pixels

Viewport:
480×270 logical pixels

Camera:
scrolling / follow-player

This makes the first town roughly 3×4 viewport areas rather than one screen.

---

# 4. First-town districts

## A. Residential Hill
Purpose:
- player's initial home;
- tutorial-safe streets;
- neighbors;
- small park;
- first odd detail.

Landmarks:
- player home
- old water tower
- hill overlook
- neighborhood notice pole

Tone:
quiet, domestic, safe.

## B. Central Main Street
Purpose:
primary social/economic route.

Contains:
- first BAR
- general store
- small restaurant/cafe
- local service shop
- bench/plaza
- public notice board

BAR is the canonical in-world music purchase location.

## C. Market / Workshop Lane
Purpose:
future crafting/economy foundation.

Contains:
- repair workshop
- secondhand stall
- food stall
- storage/back alley
- future player market access point

Visual identity:
denser props, boxes, awnings, delivery carts, signs.

## D. JOURNAL / Civic Corner
Purpose:
information/research/story.

Contains:
- JOURNAL office/board
- archive room later
- town map
- local records
- YUZU routines

## E. Riverside
Purpose:
first life-skill area.

Contains:
- riverbank
- fishing spots
- bridge
- bench
- drainage channel
- reeds/trees
- first hidden path

Later:
Fishing Slice begins here.

## F. Station / East Gate
Purpose:
future world expansion.

Contains:
- station/bus-like original transit hub
- road out of town
- freight/storage
- route to second district/region

This becomes the physical connection to the larger world.

## G. Back Alley / Strange Pocket
Purpose:
MOTHER-like ordinary/odd contrast without copying assets.

Contains:
- vending machine
- graffiti/stickers
- dead-end door
- cat/animal
- odd NPC
- first ZURE clue

Must feel optional.

---

# 5. Town topology

The town needs loops.

Primary loop:
Residential Hill -> Main Street -> Civic Corner -> Riverside -> Residential Hill

Secondary loop:
Main Street -> Market Lane -> Station -> Riverside

Shortcuts:
- alley connector
- stairs/ramp
- behind-BAR path
- riverside footpath

Avoid:
- single corridor;
- all buildings facing one horizontal road;
- every service visible at once.

---

# 6. Visual grammar

Projection:
3/4 top-down.

Light:
upper-left.

Buildings:
- roof plane;
- front plane;
- side plane;
- eave;
- short cast/contact shadow;
- material-specific shading.

Street:
- curb;
- sidewalk;
- lane marking where appropriate;
- driveways;
- gutters/drains;
- crosswalk only where logical.

Scale:
- doors match 32×40 characters;
- cars match street width;
- benches/tables match body size;
- signs remain readable via UI/overlay when text is required.

No giant duplicate-purple shadows.

---

# 7. Environment-density target

Per viewport:
- 1–3 primary structures;
- 3–7 props;
- 1–4 NPCs;
- vegetation variation;
- at least one optional interaction;
- one clear navigation landmark.

Town total:
- 12–18 meaningful exterior buildings/structures;
- 8–12 enterable over time;
- 20+ NPC positions/routines over time;
- 4–6 major landmarks.

Not all interiors must ship in v2 initial release.

---

# 8. Landmark design

The first town needs memorable orientation points.

Initial landmarks:
1. water tower / hill marker
2. BAR neon/sign
3. large riverside tree
4. bridge
5. station clock/sign
6. civic/JOURNAL facade

A player should navigate by landmarks before opening a map.

---

# 9. NPC placement philosophy

NPCs are inhabitants, not decoration.

Initial v2 target:
- SORA/player preset
- MELO
- YUZU
- PON
- 4 named secondary residents
- 6–10 unnamed ambient residents

Each named resident gets:
- home/work location;
- daytime position;
- evening position later;
- relationship links;
- reason to be in that district.

---

# 10. First-town life before combat

The town must already be enjoyable without combat.

Initial interactions:
- talk;
- inspect signs/props;
- BAR;
- JOURNAL;
- CAFE;
- river/future fishing spots;
- bench/sit later;
- vendor browsing later;
- environmental oddities.

A player should be able to spend time wandering without a quest marker.

---

# 11. First-town v2 release slices

## v2.1 — Exterior foundation
- scrolling map
- all district roads/paths
- coherent building footprints
- landmarks
- collision
- current 4 core characters
- jump/BGM retained

## v2.2 — First BAR
- enter/exit
- bartender/MELO
- jukebox
- music purchase inside game
- standalone /music/ purchase removed simultaneously

## v2.3 — JOURNAL + CAFE interiors
- two interiors
- NPC routines
- inspectables

## v2.4 — Riverside life
- fishing spots
- first fishing prototype
- bridge/hidden path

## v2.5 — Station / east exit
- physical route to next region
- no second town yet

Only after v2.5 do we expand to the second town.

---

# 12. Acceptance criteria for replacing current town

Current public town remains REJECTED until v2 meets:

- scrolling map larger than viewport;
- at least 5 visually distinct districts/areas;
- road/path loops;
- coherent building perspective/light;
- no ambiguous giant shadows;
- 4 clear landmarks;
- player can reach BAR/JOURNAL/river/station physically;
- 4 core characters remain readable;
- desktop/mobile controls work;
- no horizontal overflow;
- screenshot review;
- public deployment proof.

Only then label "first town accepted."

---

# 13. Copyright / originality guard

Learn from:
- spatial density;
- district identity;
- navigation structure;
- ordinary/strange contrast;
- service distribution;
- landmark use.

Do NOT copy:
- maps;
- exact street layouts;
- buildings;
- characters;
- dialogue;
- names;
- UI;
- music;
- quests.

DOLZORE town must become recognizable on its own.
