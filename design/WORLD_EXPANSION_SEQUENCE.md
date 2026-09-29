# DOLZORE WORLD — INCREMENTAL EXPANSION SEQUENCE
Authority: 2026-09-29 JST

This document turns the RPG master plan into deployable public slices.

Rule:
Every slice must be independently playable, QA'd and deployed.
Do NOT hold five features in a hidden branch waiting for a "complete game."

---

## Slice 0 — current public baseline
Already public:
- one walkable town screen
- four named residents
- talk / enter
- HTML MUSIC / JOURNAL / SUPPORT / LEGAL
- basic collision

Known defects:
- character art still provisional
- no jump
- no BGM
- no interiors
- exterior architecture / shadows need later redraw

---

## Slice 1 — CHARACTER + JUMP + SOUND
Ship first.

Adds:
- 32×40 core character art
- stronger unique silhouettes
- Space jump
- mobile JUMP
- original procedural town BGM
- BGM ON/OFF
- persistent BGM preference
- updated resident dialogue based on character bible

Acceptance:
- all 4 recognizable at normal desktop and mobile scale
- jump visible > 10 px
- shadow shrinks while jumping
- Enter still interacts
- BGM does not autoplay before browser allows
- first input starts audio when BGM is ON
- toggle survives reload

Public result:
same town, but it feels more alive.

---

## Slice 2 — FIRST BAR + JUKEBOX SALES
Ship independently after Slice 1.

Exterior change:
- the current MUSIC building is replaced by the first town BAR.
- BAR is a real scene transition.

Interior:
- bar counter
- stools
- two small tables
- jukebox
- record sleeves / speakers / posters
- bartender / MELO presence

Interaction:
- Enter at jukebox opens the 60-track selector inside the game
- preview stays 20 sec
- track cards show BEST FOR + concrete video-use description
- BUY opens the exact Stripe checkout URL
- no separate web sales page is used
- exit door returns to the exact exterior doorway

Sales rule:
- canonical purchase path = WORLD -> BAR -> JUKEBOX -> TRACK -> STRIPE
- /music/ must not contain purchase buttons

Acceptance:
- enter / exit without page reload if possible
- no loss of BGM preference
- MELO has 5 interior lines
- one optional inspectable object

---

## Slice 3 — JOURNAL INTERIOR

Interior:
- bulletin board
- desk
- shelves
- wall map
- YUZU

Interaction:
- board shows current article titles
- selecting an article opens normal JOURNAL HTML
- desk has one optional YUZU note
- exit returns outside

Acceptance:
- article data comes from current articles JSON
- no duplicated hard-coded title list
- YUZU state line changes after reading one article

---

## Slice 4 — CAFE INTERIOR

No commerce.

Purpose:
world-building.

Interior:
- counter
- three tables
- window
- radio
- plant
- rotating unnamed NPC

Adds:
- room-specific BGM variation
- 3 inspectable props
- first "odd but ordinary" NPC conversation

Acceptance:
- CAFE is useful even with no product
- no checkout / paid non-music lane

---

## Slice 5 — EAST ROAD / RIVERSIDE

First actual world expansion beyond one screen.

Adds:
- camera scrolling
- east exit from town
- short connecting road
- riverside
- bench
- footbridge
- one hidden path
- one new unnamed NPC

Purpose:
prove that DOLZORE is a connected place, not a menu screen.

Acceptance:
- transition feels continuous
- player position persists
- returning reaches the same town edge

---

## Slice 6 — EPISODE 0 QUEST

Quest:
"Meet the town."

Steps:
1. talk to MELO
2. talk to YUZU
3. talk to PON
4. inspect MUSIC jukebox
5. inspect JOURNAL board

Reward:
- notebook / map menu unlock

Adds:
- quest state
- first state-dependent dialogue
- completion message

---

## Slice 7 — SAVE / NOTEBOOK

Adds:
- localStorage save
- location
- player coordinates
- quest flags
- BGM preference
- discovered locations
- notebook UI

No login required.

---

## Slice 8 — EVENING STATE

Same town, new state:
- darker palette
- windows light up
- NPC positions change
- BGM changes
- some dialogue changes
- CAFE becomes busier

This is cheaper and richer than immediately making a huge new map.

---

## Slice 9 — STATION / SECOND DISTRICT

Adds:
- a second town BAR with different interior palette / resident / recommended jukebox set;
- the same global catalog is available, but recommendations differ by location.



Adds:
- station
- bus stop
- small second commercial/residential area
- 3 new NPCs
- route between districts

Transport remains original and does not imitate MOTHER buses/bicycles mechanically or visually.

---

## Slice 10 — EPISODE 1: SILENT JUKEBOX

First multi-step story.

The MUSIC jukebox powers on but its mechanism cannot complete a selection.

Player:
- asks MELO
- gets a note from YUZU
- follows PON's detour
- finds three ordinary mechanical parts / clues
- restores mechanism

No purchase requirement.

Purpose:
combine world exploration, character relationships and music.

---

## Slice 11 — FISHING v1

Adds:
- first fishing rod;
- 4–6 fish species;
- location/time/bait conditions;
- tension-based catch interaction;
- fish log;
- NPC sell value;
- local personal-best records.

No public ranking yet; local-only while server authority is absent.

## Slice 12 — COOKING v1

Adds:
- 5–8 recipes;
- caught fish as ingredients;
- food quality;
- exploration/combat preparation effects;
- recipe book.

## Slice 13 — LOCAL HOUSING ROOM

Adds:
- personal instanced room;
- furniture placement;
- fish trophy / aquarium slot;
- storage;
- no server dependency yet.

## Slice 14 — INVENTORY / EQUIPMENT / LEVEL FOUNDATION

Adds:
- BODY / SHOES / CHARM / TOOL;
- local inventory;
- level/XP;
- weapon/profession progression placeholders;
- no battle required yet.

## Slice 15 — COMBAT MVP

Adds:
- 3 initial combat jobs;
- first enemy ecology set;
- weapon skills;
- resonance magic;
- dangerous field zone;
- first party-oriented boss prototype.

## Slice 16+ — broader RPG / social systems

After exploration feels genuinely good:
- inventory
- richer quest graph
- day/evening
- transport
- larger map
- environmental puzzles
- only later: battle prototype

---

# Slice release discipline

For each slice:

1. branch from fresh main
2. define exact acceptance checks
3. implement one slice only
4. desktop + mobile QA
5. screenshot review
6. merge
7. Pages deploy
8. fresh public readback
9. checkpoint in Dolzore #3 + AgentHub #238
10. next slice

Never:
- keep user waiting for "the full game"
- mix three unfinished slices
- call branch-only work complete
- push placeholder characters/art to public main

Current active slice:
**Slice 1 — CHARACTER + JUMP + SOUND**
