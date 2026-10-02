# ANIMAL CROSSING — SAVE / INTERNAL DATA GENERATION MATRIX

Authority: DeeDee-Lab/Dolzore#40
Evidence layer: public reverse engineering unless marked official.

## 1. N64 — Animal Forest

Primary reverse source:
zeldaret/af @ 4ddba04604ee7b4c4cfc0b64f8ee4d094bb385be

Existing research found:
- persistent Save struct is modeled separately from runtime CommonData;
- NPC schedule/runtime walking data exists outside/alongside persistent state;
- explicit time/environment renewal boundaries exist.

Meaning:
persistent state != full runtime simulation image.

## 2. GameCube — Animal Crossing

Primary reverse source:
ACreTeam/ac-decomp @ 09ca8e8b5b24e6ab44047ee980cf0088ad7ecb4c

Repo is a GameCube decompilation targeting a specific USA revision.
Version scope must be retained for every exact fact.

Research domains to keep separate:
- save/memory card;
- actor/NPC;
- field/acre;
- events;
- mail;
- house;
- shops/economy;
- calendar/weather;
- audio/UI.

## 3. Cross-generation save research — ACSE

Cuyler36/ACSE @ 23563b168f4afd705aa3b48da807978644c715df

Public project exposes explicit domains:
- Saves/Checksums
- Players
- Town/Acres
- Town/Buildings
- Town/Island
- NativeFruit
- Shops
- Villagers
- AnimalMemories
- Weather
- Quests

This is strong evidence that save editing/research across supported generations requires version-specific modular parsing.

## 4. New Leaf

marcrobledo/acnl-editor @ b76e89a7346b18600893c54adf8ca2a7845974e3

Use as separate 3DS save research line.
Do not merge offsets with older ACSE models without explicit version proof.

New Leaf design-level persistent domains include:
- town;
- mayor/player;
- public works;
- ordinance;
- residents;
- house;
- inventory/catalog;
- design;
- island;
- event/progression.

Exact offsets remain version-scoped research.

## 5. New Horizons

kwsch/NHSE @ cb0745415945776f73375bf0a434a8babf059307

Current public code structure includes:
Save/
- Files
- Meta
- Offsets

Structures/
- Building
- Designs
- Item
- Mail
- Map
- Records
- Villager
- RecipeList
- TurnipStonk
- RNG helper

Editing/
- FieldItem
- Inventory
- ItemRequest

Encryption/
Hashing/

This is a much more explicit versioned-save architecture than early games.

## 6. Exact public NHSE examples

At current research commit:
- MainSaveOffsets declares PlayerCount = 8.
- MainSaveOffsets declares VillagerCount = 10.
- Villager1 is documented for game updates 1.0 through 1.4 with SIZE = 0x12AB0.
- Villager2 is documented starting at update 1.5 with SIZE = 0x13230.
- GSaveMemory SIZE = 0x5F0.
- VillagerHouse1 SIZE = 0x1D4 and ItemCount = 36.
- VillagerHouse2 SIZE = 0x12E8.
- TurnipStonk / GSaveShopKabu SIZE = 0x44.
- MainSave exposes Hemisphere using the WeatherArea offset.

These are PUBLIC_REVERSE_ENGINEERING facts for NHSE's supported format model, not official Nintendo documentation.

## 7. Versioning lesson

Never define one monolithic SaveData class forever.

DOLZORE:
SaveHeader
- schema_version;
- build_version;
- migration_version;
- world_id;
- created_at;
- saved_at;
- checksum/hash metadata.

Persistent modules:
- World;
- Player;
- NPC;
- Housing;
- Economy;
- Collection;
- Calendar;
- Mail;
- Market;
- Crafting;
- Social;
- CustomDesigns.

Use explicit migrations.

## 8. Integrity

Public editors contain checksum/hashing/encryption layers.
For DOLZORE:
- local saves: integrity hash + backup generations;
- server saves: DB transaction + revision;
- multiplayer: authoritative ledger.

## 9. User-owned analysis

If the user later supplies their own save/dump:
RAW_SOURCE
-> hash and preserve original
-> parse read-only copy
-> record version
-> create VERIFIED_FACT catalog
-> never overwrite source file during analysis.

## 10. Anti-copy

Do not embed Nintendo save layouts or structs directly in DOLZORE.
Use them to learn:
- modular persistence;
- version migrations;
- separation of runtime/persistent state;
- checksums;
- explicit content IDs;
- dedicated market/social/villager state.
