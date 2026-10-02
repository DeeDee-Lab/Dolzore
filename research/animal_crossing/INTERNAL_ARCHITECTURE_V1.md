# ANIMAL CROSSING INTERNAL ARCHITECTURE FINDINGS V1

Authority: DeeDee-Lab/Dolzore#40
Important: decomp/save-editor structures are community reverse engineering unless explicitly marked official.

## 0. Why multiple generations matter

Animal Crossing's system architecture is unusually useful to study across hardware generations because the same conceptual product survives:
N64 -> GameCube -> DS -> Wii -> 3DS -> mobile -> Switch.

Study invariants separately from storage/layout changes.

## 1. Nintendo 64 — zeldaret/af

Repository:
https://github.com/zeldaret/af

Repository states it is a work-in-progress decompilation of どうぶつの森 / Animal Forest.
It rebuilds from a user-supplied base ROM; it is not a PC port.

### Observed source-domain separation
Public decomp contains dedicated modules including:
- m_npc
- m_npc_schedule
- m_npc_walk
- m_time
- environment code
- house/room systems
- quest/event systems
- scene tables
- common persistent data.

### Save
Observed in include/m_common_data.h:
- struct Save is documented with size 0x10000 in the current decomp.
- CommonData embeds Save and runtime-only state separately.
- CommonData includes NPC schedule/runtime walking/event state after persistent save region.

Architectural lesson:
Persistent authoritative state and runtime simulation state are separable.

### NPC schedules
Observed:
- src/code/m_npc_schedule.c
- include/m_npc_schedule.h
- src/code/m_npc_walk.c
- common_data.npcSchedule[ANIMAL_NUM_MAX]

The decomp contains time-based schedule entries and modes such as field vs in-house.
Do not infer every villager shares one identical table; multiple profiles/tables exist.

### Time/environment
include/m_time.h exposes renewal categories including WEATHER and DAILY and calendar term structures.

Architectural lesson:
daily renewal, weather renewal and calendar logic are explicit simulation boundaries.

### Houses
Persistent save data contains player home structures and environment/scene code references NPC house scenes separately from outdoor field scenes.

## 2. GameCube — ACreTeam/ac-decomp

Repository:
https://github.com/ACreTeam/ac-decomp

Current README:
- decompilation of Animal Crossing for Nintendo GameCube;
- supported version listed as GAFE01_00 Rev 0 USA;
- repository intentionally contains no game assets or assembly;
- original game copy required.

Use this as the highest-value structural reference for the localized GameCube generation.

Research targets:
- field/acre generation;
- actor framework;
- animal data;
- schedule/walk logic;
- event/calendar;
- mail;
- house/furniture;
- shop/economy;
- save/checksum/memory-card structures;
- audio/environment;
- localization layer.

Do not assume N64 offsets/struct sizes carry over unchanged.

## 3. Nintendo DS — Wild World

Research lines:
- Universal-Team/WildEdit
- Universal-Team/ACWW-Web-SaveEditor
- linked ACWW_Research

What save editors prove at minimum:
Wild World save state contains independently editable domains for:
- town/map;
- players/appearance;
- pockets/items;
- patterns;
- residents/villagers;
- letters/mail-related data in specialized tooling.

Official game material confirms both local wireless and internet village visits.

Design implication:
Town simulation remains persistable as a structured data object portable enough for network visits and save editing.

## 4. Wii — City Folk

Research:
https://github.com/mattgj/actoolkit

ACToolkit's documented editable domains include:
- Town
- Acre
- Grass
- House
- Pocket
- Drawer
- Lost & found
- Recycle bin
- Nook's store
- Emotion
- Appearance
- Wallet
- ABD
- points
- resident/town names
- gate style

This is strong reverse-engineering evidence that the Wii save separates major life-sim subsystems into addressable state regions.

### Grass/path ecology
The existence of a dedicated grass editor corresponds to City Folk's visible grass-wear/path system.
Treat exact decay/growth equations as pending until extracted from higher-confidence research.

## 5. Nintendo 3DS — New Leaf / Welcome amiibo / Happy Home Designer

Research:
- Universal-Team/LeafEdit
- LeafEdit-Core

README confirms LeafEdit covers:
- Wild World
- New Leaf
- New Leaf Welcome amiibo
- Happy Home Designer.

Documented editor screens/features include:
- acre editor;
- map editor;
- town editor;
- player editor;
- pocket/item editor;
- pattern editor/viewer;
- villager editor;
- villager inventory/items;
- badge data;
- appearance;
- palette tools.

Architectural significance:
By New Leaf, customization and persistent town state are rich enough that editor architecture benefits from separate domain objects rather than one opaque save blob.

Official New Leaf design adds:
- mayor authority;
- public works;
- ordinances;
- shopping district;
- broader exterior personalization.

DOLZORE mapping:
PublicWorldState must separate:
- terrain/acre;
- placed public structures;
- rule/ordinance state;
- service progression;
- resident state;
- player state.

## 6. New Horizons — kwsch/NHSE

Repository:
https://github.com/kwsch/NHSE

README states:
- Animal Crossing: New Horizons save editor;
- edits save data dumped from Switch;
- tool itself does not dump saves.

### Versioned save layouts
NHSE contains version-specific MainSaveOffsets classes and a revision checker.

Observed FileHeaderInfo fields:
- offset 0x00: Major (uint)
- 0x04: Minor (uint)
- 0x08: Unk1 (ushort)
- 0x0A: HeaderRevision (ushort)
- 0x0C: Unk2 (ushort)
- 0x0E: SaveRevision (ushort)

Important lesson:
Save schema migrations are first-class. Never hardcode one save layout for a long-lived live-updated game.

### Main save domains visible in code
Public NHSE code exposes accessors/editors for domains including:
- Villagers
- Villager houses
- DesignPattern / DesignPatternPRO
- map/island-related state
- items/inventory/storage
- player-related state
- turnips/economy-related state
and many other versioned offsets.

Exact current field coverage must be read against the specific NHSE commit and supported game revision.

### Custom designs
MainSave accessors expose normal and PRO design patterns.
Version-specific offset classes move LandMyDesign across game revisions.

Architectural lesson:
Player-authored content should be stored as independent versioned records and not embedded in UI/render state.

## 7. New Horizons 3.x / Switch 2

Official current state:
- Switch 2 Edition released 2026-01-15.
- Free 3.0 update shared by Switch/Switch 2.
- Nintendo support lists 3.0.3 on 2026-04-29.

Research warning:
NHSE or any reverse-engineered offset set must be checked for explicit 3.x support before treating older offsets as current.

## 8. Pocket Camp -> Pocket Camp Complete

Original Pocket Camp was a service product:
- campsite management;
- material/craft loops;
- seasonal live events;
- social/mobile service features.

Service ended 2024-11-29 JST.
Pocket Camp Complete launched 2024-12-02 as a paid app without additional in-app purchases and brings previously released content forward.

Official Complete description:
- over 10,000 items;
- monthly seasonal events;
- fishing/bugs/fruit;
- villagers/campsite;
- Complete Tickets;
- QR-based Camper Cards;
- Whistle Pass.

Architecture lesson:
Live-service content can be migrated to a deterministic local catalog/event rotation if server dependencies are deliberately separated.

## 9. Cross-generation internal invariants

Across reverse-engineered generations, keep these as independent logical stores:
- Clock/Calendar
- World/Town/Island
- Terrain/Acre/Chunk
- Player
- House/Room
- Villager
- Villager schedule/activity
- Relationship/memory
- Item catalog
- Inventory/storage
- Placed objects
- Economy/shop
- Collection museum/journal
- Events/holidays
- Mail/messages
- Custom designs
- Service/building progression
- Settings/version/schema.

## 10. DOLZORE persistent-state architecture

Suggested:
WorldSave
  metadata/schema_version
  world_clock
  environment
  region_states[]
  town_state
  public_projects[]
  placed_objects[]
  ecology_state
  economy_state
  collection_state
  event_state

PlayerSave
  identity
  progression
  inventory
  equipment
  currencies
  house
  designs
  discoveries
  relationships
  mail
  settings

NPCSave
  npc_id
  home_id
  relationship[]
  current_schedule_state
  move_state
  memories/flags
  possessions/clothing
  event_state

Static content must remain outside save data and be referenced by stable IDs.

## 11. Offline catch-up

Do NOT simulate every missed second.

Store:
- last_simulated_time
- daily counters
- event version
- deterministic seeds where needed.

On login:
1. calculate elapsed boundaries;
2. process day rollover;
3. process season/event transitions;
4. process ecology growth/decay;
5. process shops/stock;
6. process NPC relationship/move state;
7. process mail/deliveries;
8. spawn current-time activities.

Must be deterministic/idempotent.

## 12. Source warning

Decompilation/source names are reverse-engineered names and may not equal Nintendo's original internal symbol names.
Save-editor field naming can reflect tool authors' abstractions.
Preserve source repo + commit when recording an exact field or offset.
