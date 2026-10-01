# UO SYSTEM INVENTORY — ALL-DOMAIN RESEARCH MAP

Authority: DeeDee-Lab/Dolzore#27
Purpose: ensure no major UO structural domain disappears from handoff.
Status vocabulary: CONFIRMED_V1 / PARTIAL / SOURCE_PENDING / RETAIL_INTERNAL_UNKNOWN.

## 1. Product / world topology
- account -> shard/server selection: CONFIRMED_V1
- shard identity and ruleset identity: CONFIRMED_V1
- facets / parallel rule environments: CONFIRMED_V1
- regions / towns / wilderness / dungeons: PARTIAL
- tiles / terrain / statics / multis: OPEN_SOURCE_REFERENCE_CONFIRMED; retail internal representation RETAIL_INTERNAL_UNKNOWN
- public world vs house/private/instanced boundaries: PARTIAL
- server boundaries / seamless movement behavior: PARTIAL
- day/night/weather/season systems: SOURCE_PENDING
- world-state persistence and restart/save semantics: OPEN_SOURCE_REFERENCE_CONFIRMED; retail internals RETAIL_INTERNAL_UNKNOWN

## 2. Account / character identity
- account/shard character relationship: PARTIAL
- character name/body/appearance: SOURCE_PENDING
- race/gargoyle/elf/human era additions: SOURCE_PENDING
- persistent character identity: CONFIRMED_V1
- stat/skill state: CONFIRMED_V1 current
- titles/fame/karma/reputation: CONFIRMED_V1

## 3. Stats / skills / progression
- STR/DEX/INT: CONFIRMED_V1 current
- HP/mana/stamina derivation: CONFIRMED_V1 current
- total stat cap / individual stat cap: CONFIRMED_V1 current; historical deltas PARTIAL
- skill list and skill-cap system: PARTIAL
- raise/lock/lower controls: CONFIRMED_V1 current
- primary/secondary stat associations per skill: CONFIRMED_V1 current
- skill gain algorithms: SOURCE_PENDING / community research separation required
- power scroll/stat scroll systems: PARTIAL
- templates/classless skill allocation philosophy: PARTIAL

## 4. Combat
- melee/ranged attack flow: PARTIAL
- attack speed / swing delay: SOURCE_PENDING
- hit chance: SOURCE_PENDING
- damage formulas: ERA_SENSITIVE / SOURCE_PENDING
- armor/resistances: PARTIAL
- special moves / weapon abilities: PARTIAL
- stamina effects: PARTIAL
- poison / disease / status: SOURCE_PENDING
- healing/bandages: SOURCE_PENDING
- PvE monster combat: PARTIAL
- PvP combat: PARTIAL and RULESET_SENSITIVE
- AoS combat formula discontinuity: CONFIRMED_V1

## 5. Magic
- Magery 64 spells / eight circles: CONFIRMED_V1 current
- reagents/mana/casting requirements: PARTIAL
- spell formulas: PARTIAL
- spellbooks/scrolls: PARTIAL
- Necromancy/Chivalry/other magic systems: PARTIAL
- travel magic Recall/Gate/Mark: SOURCE_PENDING
- resist/reflect/dispel/paralyze mechanics: SOURCE_PENDING

## 6. Death / corpse / resurrection
- corpse creation and item consequence: PARTIAL / RULESET_SENSITIVE
- item insurance: CONFIRMED_V1 modern normal ruleset
- Siege no-insurance exception: CONFIRMED_V1
- New Legacy death difference: CONFIRMED_V1 scoped
- resurrection methods / ghost interaction: SOURCE_PENDING
- corpse ownership / looting rights: PARTIAL / ERA_SENSITIVE
- murderer death penalties: PARTIAL

## 7. Crime / notoriety / reputation
- criminal acts: SOURCE_PENDING
- murderer counts / red status: CONFIRMED_V1 current
- short/long murder count decay: CONFIRMED_V1 current
- fame/karma: CONFIRMED_V1 current
- stealing/snooping: SOURCE_PENDING
- guards/guard zones: SOURCE_PENDING
- innocent/criminal/murderer visual notoriety: PARTIAL
- faction/guild war consent: PARTIAL

## 8. Items / inventory / containers
- item entity identity: OPEN_SOURCE_REFERENCE_CONFIRMED
- backpack/container nesting: PARTIAL
- weight/carrying: CONFIRMED_V1 current item property
- equip slots/layers: SOURCE_PENDING
- item stackability: SOURCE_PENDING
- durability/current-max durability: CONFIRMED_V1 current
- repair: PARTIAL
- blessed/insured/newbie historical concepts: PARTIAL / ERA_SENSITIVE
- loot generation/magic properties: PARTIAL / ERA_SENSITIVE
- item decay: PARTIAL
- secure containers/lockdowns: CONFIRMED_V1 housing context

## 9. Gathering / resources
- mining/ore: PARTIAL
- lumberjacking/logs/boards: CONFIRMED_V1 current
- fishing: SOURCE_PENDING
- cotton/flax/wool -> cloth: CONFIRMED_V1 current
- leather/hides: PARTIAL
- resource respawn/depletion algorithms: RETAIL_INTERNAL_UNKNOWN unless official source found
- regional/facet resource incentives: PARTIAL

## 10. Crafting / production
- blacksmithing: CONFIRMED_V1 current
- tailoring: CONFIRMED_V1 current
- tinkering: PARTIAL
- carpentry: PARTIAL
- inscription: PARTIAL
- alchemy: PARTIAL
- cooking: PARTIAL
- fletching/bowcraft: PARTIAL
- imbuing/enhancing/reforging: PARTIAL
- Bulk Order Deeds: CONFIRMED_V1 current
- tool quality/exceptional/resource modifiers: SOURCE_PENDING
- repair/crafting consumption economy: PARTIAL

## 11. Economy / commerce
- NPC shopkeepers buy/sell: PARTIAL
- direct trade: SOURCE_PENDING
- bank/bank box: PARTIAL
- player vendors: CONFIRMED_V1 current
- vendor inventory/pricing/fees/search: CONFIRMED_V1 current
- commission vendors/stewards: CONFIRMED_V1 current
- commodity deeds: PARTIAL
- house-based shops: CONFIRMED_V1
- gold sinks/faucets: SOURCE_PENDING
- auction/global market systems if present by era: SOURCE_PENDING
- anti-dupe/transaction ledger retail internals: RETAIL_INTERNAL_UNKNOWN

## 12. Housing / property
- house placement: CONFIRMED_V1 current
- public/private house state: CONFIRMED_V1
- owner/co-owner/friend/access roles: CONFIRMED_V1
- lockdowns: CONFIRMED_V1
- secure containers: CONFIRMED_V1
- player vendors in houses: CONFIRMED_V1
- custom house construction/commit: CONFIRMED_V1
- house transfer/demolish: CONFIRMED_V1
- house decay/refresh history: PARTIAL / ERA_SENSITIVE
- one-house-per-account history: PARTIAL
- bulletin boards/barkeeps: PARTIAL historical

## 13. Pets / taming
- animal taming: CONFIRMED_V1 current
- animal lore: CONFIRMED_V1 current
- veterinary: PARTIAL
- control chance/loyalty: PARTIAL
- follower/control slots: PARTIAL
- pet training/progression: CONFIRMED_V1 current
- stables: SOURCE_PENDING
- pet death/resurrection: SOURCE_PENDING
- transfer/trade: PARTIAL

## 14. NPC / AI / ecology
- mobiles representing players/NPCs/creatures in ModernUO reference: OPEN_SOURCE_REFERENCE_CONFIRMED
- vendors/restocking: OPEN_SOURCE_REFERENCE_CONFIRMED; retail rules PARTIAL
- guards: SOURCE_PENDING
- healers: SOURCE_PENDING
- bankers: SOURCE_PENDING
- monster AI: historical change confirmed, exact retail logic RETAIL_INTERNAL_UNKNOWN
- spawn regions/generators: OPEN_SOURCE_REFERENCE_PARTIAL
- ecology/resource coupling: SOURCE_PENDING

## 15. Quests / exploration / public events
- treasure maps: CONFIRMED_V1
- champion spawns: CONFIRMED_V1
- quests: PARTIAL
- escorts: SOURCE_PENDING
- dungeon keys/boss loops: SOURCE_PENDING
- seasonal/live events: PARTIAL
- world event persistence: SOURCE_PENDING

## 16. Social systems
- proximity speech: CONFIRMED_V1
- yell/whisper/emote: CONFIRMED_V1
- party: CONFIRMED_V1
- party leadership/invite: CONFIRMED_V1
- guilds: PARTIAL current; 2004 redesign historical confirmed
- alliances: PARTIAL
- guild wars: SOURCE_PENDING
- global/chat channels: CONFIRMED_V1 current
- friends/ignore/report: SOURCE_PENDING
- player bulletin boards: PARTIAL historical

## 17. Travel / geography
- walking/running: PARTIAL
- moongates: SOURCE_PENDING
- Recall/Gate/Mark: SOURCE_PENDING
- mounts: PARTIAL
- ships/boats: CONFIRMED_V1 current
- ship security/crew/storage/navigation: CONFIRMED_V1 current
- facet transitions: PARTIAL
- server boundary crossing: OPEN_SOURCE_REFERENCE_PARTIAL; retail internals unknown

## 18. UI / input / macro / targeting
- isometric field view: HISTORICAL_VISUAL_REFERENCE
- paperdoll: CONFIRMED_V1 current classic client
- backpack/container gumps: CONFIRMED_V1 current classic client
- radar/map: CONFIRMED_V1 current classic client
- journal/chat: PARTIAL
- drag/drop object interaction: SOURCE_PENDING
- double/single click interaction: PARTIAL
- targeting cursor / last target: CONFIRMED_V1 macro vocabulary
- last object / last spell / attack macros: CONFIRMED_V1
- equipment macros/actions: CONFIRMED_V1
- Classic vs Enhanced client differences: PARTIAL

## 19. Persistence / server architecture
- persistent World concept: OPEN_SOURCE_REFERENCE_CONFIRMED
- persistent Mobile and Item entities: OPEN_SOURCE_REFERENCE_CONFIRMED
- map spatial queries: OPEN_SOURCE_REFERENCE_CONFIRMED
- serialization/versioned entity saves: OPEN_SOURCE_REFERENCE_CONFIRMED
- autosave/archive: OPEN_SOURCE_REFERENCE_CONFIRMED
- retail persistence/database topology: RETAIL_INTERNAL_UNKNOWN
- retail shard process topology: RETAIL_INTERNAL_UNKNOWN
- retail save cadence/failure recovery: RETAIL_INTERNAL_UNKNOWN

## 20. Networking
- stateful network session abstraction in ModernUO: OPEN_SOURCE_REFERENCE_CONFIRMED
- movement network handling in ModernUO: OPEN_SOURCE_REFERENCE_CONFIRMED
- binary packet framework in ModernUO: OPEN_SOURCE_REFERENCE_CONFIRMED
- retail UO protocol/server implementation: RETAIL_INTERNAL_UNKNOWN
- anti-cheat/exploit detection internals: RETAIL_INTERNAL_UNKNOWN

## 21. Operations / content evolution
- publish/update model: CONFIRMED_V1
- shard-specific publishes/rules: CONFIRMED_V1
- historical rules supersession: CONFIRMED_V1
- live events/maintenance/rollback operations: PARTIAL
- client patch + server publish split historically: CONFIRMED_V1 for Publish 18 example
- observability/admin tooling retail internals: RETAIL_INTERNAL_UNKNOWN

## 22. DOLZORE integration status
- UO as major macro-structure reference: LOCKED_BY_USER_AUTHORITY
- FFXI combat/stat/vocation foundation: RETAINED_UNLESS_EXPLICITLY_CHANGED
- MOTHER2 presentation/readability reference: RETAINED
- direct copying of UO expression/assets/maps/text: PROHIBITED
- source/era/evidence preservation: MANDATORY

SYSTEM_INVENTORY_V1=true
