# ANIMAL CROSSING COMPLETE-SERIES REFERENCE — AGENT READ FIRST

Authority: direct user instruction, 2026-10-02 JST
Canonical repo: DeeDee-Lab/Dolzore
Canonical issue: DeeDee-Lab/Dolzore#40
Consumer: DOLZORE game creation / life-sim / MMO / Unity agents
Status: ACTIVE_COMPREHENSIVE_RESEARCH_V1

## 0. Purpose

This directory studies the complete Animal Crossing / どうぶつの森 lineage as a game-development reference.

The target is not to clone Animal Crossing.

Use the corpus to learn:
- real-time life simulation;
- persistent towns/islands;
- villager schedules and social presence;
- parallel self-directed goals;
- long-term collection;
- seasonal/event content;
- interior/exterior customization;
- player-authored patterns;
- world-state persistence;
- low-pressure economies;
- environmental maintenance/ecology;
- social/online amplification without making multiplayer mandatory;
- very large content-volume pipelines;
- generational evolution from N64 to Switch 2/mobile;
- data decomposition visible in public decompilation/save-format research.

## 1. Evidence classes

Every important fact must keep one of:
- OFFICIAL_CURRENT
- OFFICIAL_HISTORICAL
- CREATOR_INTERVIEW
- OFFICIAL_MANUAL
- COMMUNITY_DECOMPILATION
- COMMUNITY_SAVE_RESEARCH
- COMMUNITY_REVERSE_ENGINEERING
- VISUAL_OBSERVATION
- INFERENCE
- DOLZORE_DESIGN_RECOMMENDATION

Never silently upgrade reverse-engineered tool structure into "Nintendo source architecture."

## 2. Copyright / source-fidelity boundary

Do not commit or redistribute:
- Nintendo ROMs/disc images;
- extracted textures/models/audio;
- full dialogue databases;
- copyrighted character art;
- copied map layouts;
- game save data not owned by the user;
- proprietary executable bodies.

May preserve:
- exact public source URLs/repository commit IDs;
- structure names and code-level architecture from public CC0/GPL research;
- exact field names/sizes/offsets publicly documented by reverse engineering;
- system behavior/facts;
- game-design patterns;
- original DOLZORE implementation recommendations.

When user-owned local copies become available, use the global source-fidelity policy:
RAW_SOURCE -> VERIFIED_FACT -> INTERPRETATION.

## 3. Series scope

Main lineage:
1. どうぶつの森 / Animal Forest — Nintendo 64
2. どうぶつの森+ — GameCube
3. Animal Crossing — international GameCube lineage
4. どうぶつの森e+ — GameCube
5. おいでよ どうぶつの森 / Wild World — Nintendo DS
6. 街へいこうよ どうぶつの森 / City Folk / Let's Go to the City — Wii
7. とびだせ どうぶつの森 / New Leaf — Nintendo 3DS
8. とびだせ どうぶつの森 amiibo+ / Welcome amiibo — 3DS update/reissue
9. Animal Crossing: New Horizons — Nintendo Switch
10. Happy Home Paradise — New Horizons DLC
11. Animal Crossing: New Horizons – Nintendo Switch 2 Edition / 3.x update line

Spinoff / adjacent:
- Animal Crossing Plaza — Wii U
- Animal Crossing: Happy Home Designer — 3DS
- Animal Crossing: amiibo Festival — Wii U
- Animal Crossing: Pocket Camp — smart devices, original service ended 2024
- Animal Crossing: Pocket Camp Complete — paid offline-oriented successor

Regional/version variants are tracked when they materially change data/content/systems.

## 4. Canonical reading order

1. AGENT_READ_FIRST.md
2. VERIFIED_FACT_RECORD_SCHEMA_V1.json
3. FACT_CATALOG_INDEX.json
4. every facts/*.jsonl listed by FACT_CATALOG_INDEX.json
5. SERIES_MATRIX_V1.json
6. TIME_SEASON_OFFLINE_SIMULATION.md
7. VILLAGER_SOCIAL_SIMULATION.md
8. WORLD_GENERATION_ECOLOGY.md
9. ECONOMY_COLLECTION_PROGRESSION.md
10. HOUSING_CUSTOMIZATION_CONTENT_PIPELINE.md
11. SOCIAL_MULTIPLAYER_ARCHITECTURE.md
12. AUDIO_ENVIRONMENT_GRAMMAR.md
13. SAVE_DATA_GENERATION_MATRIX.md
14. CORE_DESIGN_DOCTRINE.md
15. INTERNAL_ARCHITECTURE_V1.md
16. DOLZORE_IMPLEMENTATION_CHECKLIST.md
17. SOURCE_INDEX.md
18. RESEARCH_STATUS.json
19. Issue #40 latest checkpoint

Exact facts precede interpretation. If an interpretation conflicts with an exact fact or source record, the exact fact/source wins.

## 5. High-confidence series design facts

### Real-world time is foundational
Official DS/Wii material states that time flows in the same rhythm as the real world and seasons/scenery change accordingly.

### Player chooses their own goal
Official/creator material repeatedly emphasizes no single forced route: fishing, collecting, decorating, paying debt, socializing and other activities can become the player's own evaluation axis.

### Solitary play is complete; connection amplifies it
City Folk creator discussion explicitly says the game is designed to be enjoyable alone, while communication makes it more interesting and can increase motivation.

### Villager behavior/dialogue is a dedicated system
City Folk's sequence director explicitly owned animal behavior, dialogue specifications and messages.

### Customization expanded generation by generation
Home interior -> plants/trees -> town public works/ordinances -> island-scale placement/terraforming/crafting -> dedicated home-design products and DLC.

### Content volume is itself a design feature
New Leaf developers describe the importance of producing a huge amount of furniture, residents, small seasonal hooks and combinations, and that new staff learned by playing earlier entries that "volume" is essential to the experience.

## 6. Interpretation rule

Do not reduce the reference to "cozy."
The reusable engine is:
REAL TIME
+ persistent place
+ many small independent systems
+ resident simulation
+ collection/completion
+ personalization
+ slow material change
+ seasonal surprise
+ optional social sharing
+ huge combinatorial content library.

## 7. DOLZORE relationship

Animal Crossing research should strengthen DOLZORE's:
- living town;
- non-combat life path;
- NPC schedules;
- fishing/gathering/crafting;
- housing;
- furnishing;
- player economy;
- day/night/season;
- social spaces;
- asynchronous shared features;
- optional public events;
- long-term return motivation.

It must not overwrite:
- newest direct user instruction;
- GAME_CANONICAL_DIRECTION;
- HANDOFF_CURRENT_STATE;
- FF11-derived combat/RPG architecture;
- MOTHER2 research on town composition/atmosphere.

## 8. Resume protocol

After timeout/session change:
1. read RESEARCH_STATUS.json;
2. read Issue #40 latest checkpoint;
3. read only the source/domain files needed for the next delta;
4. append findings; do not restart from zero;
5. checkpoint source IDs/commit IDs and exact next action.
