# CURRENT SOURCE REFRESH — 2026-10-02

Canonical issue: DeeDee-Lab/Dolzore#40

Official origin / early lineage:
- https://www.nintendo.co.jp/n01/n64/software/nus_p_nafj/index.html — Animal Forest release/product facts.
- https://www.nintendo.co.jp/n01/n64/software/nus_p_nafj/what/index.html — real-world time/offline village behavior.
- https://www.nintendo.co.jp/ngc/gafj/index.html — どうぶつの森+ GameCube release/specs.
- https://www.nintendo.co.jp/ngc/gaej/index.html — どうぶつの森e+ release/specs.
- https://www.nintendo.co.jp/ds/admj/what/index.html — Wild World real-time/free-form life.
- https://www.nintendo.co.jp/ds/admj/tsushin/index.html — Wild World local/Wi-Fi visiting.

Current New Horizons lineage:
- https://www.nintendo.com/jp/games/switch2/acbaa/index.html — Switch 2 Edition features/release.
- https://en-americas-support.nintendo.com/app/answers/detail/a_id/49112 — update history; Ver.3.0.3 dated 2026-04-29.

Pinned public reverse-engineering commits used for exact facts:
- zeldaret/af @ 4ddba04604ee7b4c4cfc0b64f8ee4d094bb385be
- ACreTeam/ac-decomp @ 09ca8e8b5b24e6ab44047ee980cf0088ad7ecb4c
- Cuyler36/ACSE @ 23563b168f4afd705aa3b48da807978644c715df
- marcrobledo/acnl-editor @ b76e89a7346b18600893c54adf8ca2a7845974e3
- kwsch/NHSE @ cb0745415945776f73375bf0a434a8babf059307

# ANIMAL CROSSING SOURCE INDEX V1

## Nintendo official — design/game facts

### Wild World
https://www.nintendo.co.jp/ds/admj/what/index.html
Supports:
- real-world rhythm time;
- seasons;
- free-form life;
- fishing/home/debt/player-chosen play.

https://www.nintendo.co.jp/ds/admj/tsushin/index.html
Supports:
- local wireless and remote internet village visits.

### City Folk / Let's Go to the City
https://www.nintendo.co.jp/wii/ruuj/about/index.html
Supports:
- fictional village life;
- real-time day/night and seasons;
- events;
- resident interaction;
- fishing/fossils/discovery.

https://www.nintendo.co.jp/wii/interview/ruuj/vol1/index.html
Supports:
- production roles;
- animal behavior/dialogue as explicit sequence design;
- WiiConnect24/network focus;
- single-player complete, connectivity amplifies;
- huge localization/text burden.

### New Leaf
https://www.nintendo.co.jp/3ds/interview/egdj/vol1/index.html
Supports:
- deliberate reset of established formula;
- whole-town customization goal;
- mayor/public-works framing.

https://www.nintendo.co.jp/3ds/interview/egdj/vol1/index2.html
Supports:
- real-time clock retained as core;
- ordinances designed to adapt game to player lifestyle without removing day/night meaning.

https://www.nintendo.co.jp/3ds/interview/egdj/vol1/index3.html
Supports:
- role/animal-species identity matching;
- large furniture/resident production volume;
- 3D lighting/detail production burden.

https://www.nintendo.co.jp/3ds/interview/egdj/vol1/index5.html
Supports:
- many small hooks for different player interests.

https://www.nintendo.co.jp/3ds/interview/egdj/vol1/index6.html
Supports:
- content volume as essential series quality;
- character-specific house-part production and coherent combinations.

https://www.nintendo.co.jp/3ds/egdj/index.html
Supports:
- Welcome amiibo release/update lineage and 3DS online shutdown notice.

### Happy Home Designer
https://www.nintendo.co.jp/3ds/edhj/detail/index.html
Supports release/platform/amiibo/communication facts.

https://www.nintendo.co.jp/3ds/edhj/guidebook/index.html
Useful official guide metadata:
- 5,250+ item list;
- 387 client list;
- public facilities/home design/remake/custom-design domains.

### amiibo Festival
https://www.nintendo.co.jp/wiiu/aalj/detail/index.html
Supports:
- 2015-11-21;
- 1–4 player board-game spinoff;
- amiibo/card roles.

### Pocket Camp / Complete
https://support.nintendo.com/jp/information/2024/0822.html
Supports:
- original service ended 2024-11-29 JST;
- transition plan to paid app, no IAP, save transfer.

https://www.nintendo.com/jp/topics/article/ff99bc39-6fc9-42fc-8ec4-f25f5c37c502
Supports:
- Complete launch 2024-12-02;
- Custom Design import.

https://www.nintendo.com/us/whatsnew/mobilenews-animal-crossing-pocket-camp-complete-is-now-available/
Supports:
- 10,000+ items;
- seasonal events;
- fishing/bugs/fruit;
- campsite/resident/craft loops.

### New Horizons / Switch 2
https://www.nintendo.com/us/store/products/animal-crossing-new-horizons-us-109505/
Supports:
- release 2020-03-20;
- player counts;
- HHP DLC 2021-11-05.

https://www.nintendo.com/us/whatsnew/animal-crossing-new-horizons-sails-ashore-with-a-nintendo-switch-2-edition-and-free-content-update-jan-15/
Supports:
- Switch 2 Edition / free 3.0 feature direction.

https://en-americas-support.nintendo.com/app/answers/detail/a_id/49112
Supports:
- current update history; 3.0.3 dated 2026-04-29.

## Public reverse engineering / source research

### N64
https://github.com/zeldaret/af
COMMUNITY_DECOMPILATION.
Work-in-progress Animal Forest decompilation.

Key files identified:
- include/m_common_data.h
- include/m_time.h
- include/m_npc_schedule.h
- src/code/m_npc.c
- src/code/m_npc_schedule.c
- src/code/m_npc_walk.c
- src/code/m_kankyo.c
- src/code/m_start_data_init.c

### GameCube
https://github.com/ACreTeam/ac-decomp
COMMUNITY_DECOMPILATION.
README currently says GameCube Animal Crossing decomp, GAFE01_00 Rev 0 USA supported, no game assets included.

### Wild World
https://github.com/Universal-Team/WildEdit
https://github.com/Universal-Team/ACWW-Web-SaveEditor
COMMUNITY_SAVE_RESEARCH.

### New Leaf / Welcome amiibo / HHD
https://github.com/Universal-Team/LeafEdit
COMMUNITY_SAVE_RESEARCH.
LeafEdit README states support for WW, NL, NL Welcome amiibo, HHD.

### City Folk
https://github.com/mattgj/actoolkit
COMMUNITY_SAVE_RESEARCH.
README enumerates editable Town/Acre/Grass/House/Inventory/Shop/Resident/Economy-related domains.

### New Horizons
https://github.com/kwsch/NHSE
COMMUNITY_SAVE_RESEARCH.
README: edits user-dumped New Horizons save data.

Key code areas identified:
- NHSE.Core/Save/Meta/FileHeaderInfo.cs
- NHSE.Core/Save/Meta/RevisionChecker.cs
- NHSE.Core/Save/Files/MainSave.cs
- NHSE.Core/Save/Offsets/MainSaveOffsets*.cs
- DesignPattern structures
- Villager/house/map/item/player domains.

## Research caution
All community reverse engineering must retain repository + commit when exact offsets/field names are promoted into VERIFIED_FACT catalogs.
