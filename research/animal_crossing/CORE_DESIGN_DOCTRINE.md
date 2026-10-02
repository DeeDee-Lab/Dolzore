# ANIMAL CROSSING CORE DESIGN DOCTRINE

Authority: research/animal_crossing/
Evidence: Nintendo official product pages + Iwata Asks + public technical research.

## 1. The game has no single mandatory "win" axis

City Folk creator discussion explicitly frames one-player enjoyment around self-chosen goals: completion, room design, and other personal evaluation axes.

Reusable principle:
A life-sim world should support several parallel value systems that do not invalidate each other.

Recommended DOLZORE life paths:
- resident relationships;
- collection;
- fishing;
- gardening;
- gathering;
- crafting;
- cooking;
- house/interior;
- town appearance;
- trade/economy;
- exploration;
- local events;
- photography/journal;
- fashion;
- music/social venue.

Do not convert every activity into combat-power progression.

## 2. Real-world clock is a content scheduler, not a cosmetic clock

Official DS/Wii descriptions:
- time advances like reality;
- morning/day/night matter;
- seasons alter scenery;
- seasonal events occur.

New Leaf creator discussion shows why this matters:
Developers wanted to accommodate late-working players without destroying the meaning of real-time day/night, leading to ordinances that shift opening hours.

DOLZORE TimeService should therefore be a first-class authoritative system:
- world date/time;
- timezone policy;
- season;
- weekday;
- holiday/event schedule;
- sun/lighting phase;
- shop/service schedules;
- NPC schedule windows;
- spawning/collection windows;
- music state;
- weather state.

Do not make time merely a shader parameter.

## 3. Design for absence

A core life-sim property is that the world conceptually continues while the player is away.

Persistence categories:
- elapsed real time;
- weeds/plant/ecology change;
- shop inventory rotation;
- resident availability;
- mail/deliveries;
- event transitions;
- collection spawn windows;
- relationship memory;
- house/town state;
- moving-in/out state.

For DOLZORE, process long offline durations as deterministic catch-up transactions rather than running the whole simulation continuously.

## 4. Villagers are scheduled social actors

Public N64 decompilation contains dedicated NPC schedule and walking systems, including separate schedule state, field/house schedule modes and timed goal tables.

City Folk credits/creator interview explicitly says the sequence director handled animal behavior and dialogue specifications.

DOLZORE villager architecture:
VillagerDefinition
+ PersonalityProfile
+ RelationshipState
+ DailySchedule
+ ActivityPlanner
+ DialogueContext
+ HomeDefinition
+ Inventory/wardrobe
+ MemoryFlags
+ EventParticipation
+ MoveInOutState.

Never make dialogue and movement independent random systems; current activity should affect what a resident says.

## 5. Role and character identity reinforce each other

New Leaf developers describe selecting animal species to fit the function/shop, while established characters can change jobs across installments and still retain identity.

Principle:
Character identity = stable core traits + role-specific contextual behavior.

DOLZORE recurring NPCs may change occupation/location over time without being rewritten as different people.

## 6. "Volume" creates individuality

New Leaf developers explicitly describe:
- large resident roster;
- continuous furniture production;
- resident-specific homes;
- many small seasonal hooks;
- staff recognizing through play that sheer content volume is essential.

This is not "content for content's sake."
Large catalogs create combinatorics:
resident × house × furniture × season × clothing × conversation × event × player design.

DOLZORE needs data-driven authoring tools; handcrafted code per content item will not scale.

## 7. Small hooks make one world support many people

Iwata Asks New Leaf describes intentionally placing many small hooks because different players latch onto different details.

A life-sim should contain:
- useful hooks;
- collection hooks;
- aesthetic hooks;
- relationship hooks;
- hidden/rare hooks;
- seasonal hooks;
- absurd/tiny hooks;
- creation hooks;
- sharing hooks.

Do not force every player to see every hook.

## 8. Customization expands outward over generations

Series arc:
home room
-> clothing/custom patterns
-> landscaping
-> public works
-> town rules
-> dedicated home design
-> campsite design
-> outdoor furniture
-> terrain/river/cliff authoring
-> resort/facility design.

Design lesson:
Customization becomes powerful when the editable scope expands but rules remain legible.

DOLZORE authoring hierarchy:
Player appearance
< room
< house
< lot
< neighborhood
< town public spaces
< shared-event decoration.

Use permissions and costs per layer.

## 9. A persistent place becomes valuable because change is slow

Debt/home upgrades, shop development, tree growth, collections, resident changes and events are intentionally spread over time.

Avoid collapsing every progression into one-session completion.

DOLZORE:
Use short, medium and long horizons simultaneously:
minutes: catch/craft/talk;
hours: shop rotations/tasks;
days: building/project completion;
weeks: relationships/collections;
seasons: ecology/events;
months: town identity/history.

## 10. Maintenance creates attachment when bounded

Examples across the series:
- weeds;
- flowers;
- tree growth;
- grass wear/path emergence;
- home state;
- museum completion;
- resident relationships.

Maintenance must produce visible history, not chores without meaning.

Use "maintenance pressure" carefully:
too low -> world feels static;
too high -> guilt/obligation.

## 11. Ecology is a collection engine

Fishing/bugs/fossils/plants connect:
- season;
- time;
- location;
- weather;
- rarity;
- museum;
- selling economy;
- resident requests;
- decoration.

DOLZORE organism/content record should separate:
SpeciesDefinition
SpawnRule[]
CollectionRecord
Museum/JournalRecord
SellValue
CraftUse
SocialQuestTags.

## 12. Economy is soft progression

Classic loop:
small activities -> Bells/resources -> house/items/services -> more personalization.

New Horizons adds:
materials -> DIY -> tools/furniture/construction.

Keep currencies orthogonal:
- general money;
- social/event token;
- crafting materials;
- long-term civic currency/reputation;
but avoid redundant currencies.

## 13. Single-player first, connectivity second

City Folk interview explicitly states:
- game is designed to be enjoyable alone;
- connection makes it better;
- anticipation of future connection can itself motivate creation.

DOLZORE rule:
No core life feature should require simultaneous multiplayer.
Then add:
- visiting;
- gifting;
- marketplace;
- photo/share;
- asynchronous guest book;
- co-presence events;
- realtime shard activities.

## 14. Sharing changes creation behavior

Once players know a visitor may see their town/home, private creation becomes social expression.

Architecture consequence:
Store provenance and ownership for:
- furniture placement;
- custom pattern;
- public decoration;
- player-created message;
- shared snapshot.

## 15. Localization is a system concern

City Folk developers note enormous text volume and culture-specific adjustments.

Do not store dialogue as anonymous strings only.

Dialogue line metadata should include:
- speaker/personality;
- activity;
- time/season;
- relationship tier;
- topic;
- event;
- localization intent;
- grammatical variables;
- item/species/place references;
- repeat frequency/cooldown.

## 16. Design pillars for DOLZORE

Adopt as principles:
- living time;
- many independent pleasures;
- residents with routines;
- slow visible history;
- huge combinatorial content;
- collection/ecology;
- personalization;
- optional connection;
- world state after absence.

Do not copy:
- character designs/names;
- village/island layouts;
- exact economy/prices;
- item catalogs;
- dialogue;
- music;
- UI;
- holiday implementations;
- exact villager personality scripts.
