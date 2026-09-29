# DOLZORE RPG MASTER PLAN
Authority: 2026-09-29 JST
Status: architectural reset after visual review

## 0. Product definition

DOLZORE WORLD is no longer treated as a decorative website hero.

Target:
A browser-native, original 16-bit-inspired town RPG that gradually grows into a full game, while preserving:
- music remains the only paid product category;
- all music purchasing happens inside in-game BAR jukeboxes;
- JOURNAL remains editorial/read-only content;
- SUPPORT / LEGAL as normal web pages;
- SEO-friendly static pages outside the game canvas.

High-level reference:
- connected everyday-world feeling;
- readable small characters;
- town exploration;
- humorous/strange NPCs;
- ordinary places with unexpected details;
- strong pixel-art composition.

Never copy MOTHER/EarthBound:
- characters;
- sprites;
- maps;
- exact dialogue;
- melodies;
- battle UI;
- logos;
- proprietary names or objects.

## 1. Current failure analysis

### A. Perspective is inconsistent
Current buildings are mostly front elevations.
Right-side dark extrusions read as accidental shadows rather than architectural side planes.

### B. Shadows are wrong
Current large purple/dark blocks:
- have no single world light source;
- use inconsistent length;
- compete with building silhouettes;
- make buildings look pasted onto the grass.

New rule:
- primary light direction = upper-left;
- only short contact/cast shadows down-right;
- building side planes are material-specific darker colors, NOT black/purple shadow slabs;
- characters get 2–5 px soft/contact shadows only.

### C. Character silhouettes are too weak
Current residents read as colored human-shaped pixels, not memorable people.

New character standard:
- 32×40 native sprite;
- head ~40% of total visual mass;
- distinct hairstyle contour;
- visible face pixels;
- 2–3 clothing layers;
- one signature accessory;
- 4 directions × 2 walk frames;
- jump frame derived from current direction;
- silhouette must remain identifiable at 1× native canvas scale.

### D. Town has no systemic depth
Current map is a decorated screen.
A real RPG needs:
- collision;
- doors;
- interiors;
- talk targets;
- scene transitions;
- persistent state;
- audio;
- quests;
- items;
- time/area changes;
- camera/scrolling.

## 1.1 Living-world / hardcore gameplay authority

Canonical depth design:
`design/LIVING_WORLD_HARDCORE_RPG_DOCTRINE.md`

Binding principles:
- non-combat life/social play is a first-class path;
- fishing/cooking/crafting/housing/market/social play must be enjoyable without story progression;
- combat is preparation-heavy, role-driven, knowledge-driven and difficult;
- horizontal progression and multiple gear sets matter;
- named enemies, ecology, jobs, magic and weapon mastery are long-term core systems;
- real-money music purchases never grant game power.

## 2. Engine architecture

### Decision
Move from ad-hoc static homepage logic toward a proper game layer.

Recommended target stack:
- TypeScript
- Phaser 3
- Vite build
- static deployment to GitHub Pages
- no server required for core RPG
- localStorage / IndexedDB for saves

Migration sequence:
1. keep current Canvas prototype while interaction rules are proven;
2. build Phaser shell in parallel under `game/`;
3. migrate WORLD to Phaser once feature parity is reached;
4. keep MUSIC/JOURNAL/LEGAL static HTML for SEO and purchases.

### Repository layout

```
game/
  src/
    main.ts
    scenes/
      BootScene.ts
      TownScene.ts
      MusicInteriorScene.ts
      JournalInteriorScene.ts
      CafeInteriorScene.ts
      RiverScene.ts
      NightStreetScene.ts
      BattleScene.ts
    systems/
      InputSystem.ts
      InteractionSystem.ts
      DialogueSystem.ts
      AudioSystem.ts
      SaveSystem.ts
      QuestSystem.ts
      InventorySystem.ts
      CollisionSystem.ts
    entities/
      Player.ts
      Npc.ts
      Door.ts
      Interactable.ts
    data/
      characters.ts
      dialogue.ts
      maps.ts
      quests.ts
      audio.ts
  public/
    assets/
      tiles/
      characters/
      interiors/
      audio/
      sfx/
docs/
  game/     <- production build
  music/
  journal/
  support/
  legal/
```

## 3. World projection / art grammar

### Camera
- 3/4 top-down town view;
- NOT pure front elevation;
- NOT true isometric diamond;
- continuous connected street map.

### Native rendering
- logical canvas: 480×270 initially;
- camera expands to maps larger than viewport;
- nearest-neighbour scaling;
- integer camera positions whenever possible.

### Tile grid
- base tile: 16×16;
- architecture macro-grid: 32×32;
- character collision footprint: 14×10 approx;
- door: 24–32 px wide;
- road lane: 48–64 px;
- sidewalk: 16–24 px.

### Buildings
Every building gets:
1. roof plane;
2. front wall;
3. visible side plane;
4. eave;
5. door;
6. windows;
7. signage as crisp UI layer when text is required;
8. very short cast/contact shadow.

No giant purple shadow duplicates.

### Environment density
Each screen needs:
- 3–5 primary structures;
- 5–10 environmental props;
- 2–5 NPCs;
- 1 memorable oddity;
- at least one optional interaction.

Props:
- benches;
- hydrants;
- utility poles;
- signs;
- trash cans;
- vending machine;
- bicycles;
- parked car;
- mailbox;
- flowers/weeds;
- puddles;
- newspaper;
- small animals.

## 4. Four original core characters

### SORA — player
Silhouette:
- soft dark hair with one lifted forelock;
- mint short jacket;
- cream shoulder strap;
- warm-coral pouch;
- charcoal trousers;
- off-white sneakers.

Personality:
quietly curious; reacts to oddities with short observations.

### MELO — music resident
Silhouette:
- auburn asymmetric bob;
- dark rose overshirt;
- cream inner shirt;
- amber square hair clip;
- record-sleeve bag.

Personality:
listens before speaking; describes sounds with concrete images.

### YUZU — journal resident
Silhouette:
- dark wavy hair;
- mustard cardigan;
- olive long lower silhouette;
- off-white notebook held at side.

Personality:
collects facts, prices, small local rumors.

### PON — wanderer
Silhouette:
- slate-blue tousled hair;
- deep blue jacket;
- vivid orange scarf;
- box-shaped tan backpack.

Personality:
always walking somewhere; frequently changes destination.

Rule:
No character may resemble a MOTHER/EarthBound cast silhouette, outfit, hat, facial arrangement, or color blocking.

## 5. Controls

Desktop:
- Arrow / WASD = move
- Space = jump
- Enter = talk / enter / inspect
- Shift = run (Phase 2)
- Esc = menu (Phase 3)

Mobile:
- D-pad
- TALK / ENTER
- JUMP
- BGM toggle

Jump:
Phase 1:
- visual/physical hop;
- vertical offset curve;
- shadow shrinks;
- collision still enforced.

Phase 2:
- can clear curb / puddle / small 1-tile obstacle;
- cannot pass walls or NPCs.

## 6. Interior system

### BAR / JUKEBOX
Every town gets one BAR.
The BAR is the canonical music-discovery and purchase location.

Interior baseline:
- bar counter;
- stools / small tables;
- lighting unique to the town;
- large classic jukebox;
- speakers / record sleeves / posters;
- one bartender or recurring resident;
- optional town-specific NPCs.

Interact with jukebox:
- opens the in-game 60-track selector;
- 20-second preview only where a proven sample exists;
- scene-first discovery;
- purchase button opens the exact Stripe checkout URL;
- no separate web sales page is required.

Town variation:
- each BAR has its own name, palette, resident dialogue and recommended playlist;
- the same global 60-track catalog may be filtered/recommended differently by town.

### JOURNAL
Interior:
- bulletin board;
- desk;
- shelves;
- map;
- YUZU interaction.

Interact with board:
- shows newest article titles;
- selecting one opens static JOURNAL article.

### CAFE
No sales.
Purpose:
world-building and NPC dialogue.

Interior:
- counter;
- tables;
- window;
- radio;
- one rotating NPC.

Exit:
walk onto door tile + Enter -> return to exterior at previous door.

## 7. Audio system

Browser restriction:
audio may not autoplay before first user gesture.

Behavior:
- BGM setting shown as ON by default;
- actual audio starts on first movement/action/click;
- visible BGM ON/OFF toggle;
- setting saved to localStorage.

Original Town Theme:
- 92–104 BPM;
- simple warm square/triangle bass;
- bell/electric-piano-like lead;
- light syncopated percussion;
- 45–75 sec loop;
- cheerful but slightly odd harmonic turn;
- entirely original melody.

Area themes:
- Town Day
- BAR interior / jukebox purchase
- JOURNAL interior
- CAFE or other non-sales world-building interiors
- Town Evening
- River / outskirts

SFX:
- step;
- jump;
- door;
- confirm;
- cancel;
- dialogue blip;
- jukebox click.

No MOTHER melody/samples/arrangement.

## 8. Dialogue system

Core rule:
short, concrete, human, slightly unexpected.

Structure:
```
speaker
1–3 short lines
optional response / action
```

Avoid:
- fake-poetic slogans;
- exact Itoi-style imitation;
- exposition dumps.

NPC roles:
- practical resident;
- strange resident;
- child;
- worker;
- tourist;
- recurring wanderer;
- shop-adjacent character;
- rumor character.

Each named NPC needs:
- purpose;
- recognizable silhouette;
- 3 base lines;
- 1 state-change line;
- 1 hidden/rare line.

## 9. Story structure

### Episode 0 — Town Introduction
Goal:
learn movement, jump, talk, interiors.

Quest:
meet MELO, YUZU, PON and inspect the town board.

Reward:
town map / notebook feature.

### Episode 1 — The Silent Jukebox
The jukebox has power but one internal mechanism is missing.
Player explores town and finds three mundane parts via NPC errands.
No purchase required.

Purpose:
teach exploration + interiors + optional music previews.

### Episode 2 — Tools District
Unlock JOURNAL-related district.
No paid non-music products.
Articles appear as research boards / reading material.

### Episode 3 — Riverside / Station
Larger connected world.
Introduce bicycle/bus-like original transport concept later.

### Episode 4 — Night Town
Same map, different NPC positions, lights, music and dialogue.

## 10. Quest / progression

Quest types:
- talk;
- inspect;
- carry item;
- find person;
- choose route;
- return item;
- solve environmental clue.

Avoid repetitive fetch spam.

Data model:
```
quest_id
state
steps[]
requirements[]
rewards[]
npc_state_changes[]
world_flags[]
```

## 11. Inventory / save

Inventory:
- notebook;
- town map;
- key items;
- cosmetic finds;
- no purchased music required for progress.

Save:
localStorage initially:
- player position;
- scene;
- quest flags;
- NPC flags;
- BGM preference;
- discovered locations.

Later migrate to IndexedDB if needed.

## 12. Battle system — later, not now

Do NOT build battles until town exploration is genuinely good.

Phase 6 concept:
- original turn-based encounters;
- visible field enemies;
- no rolling HP copy;
- no copied PSI/menu/battle background design.

Possible DOLZORE stats:
- HEART
- FOCUS
- SPARK
- RHYTHM

Possible enemies:
abstract daily-life disturbances / odd urban creatures.
Must be original.

## 13. Website + RPG coexistence

Important:
Do not turn everything into Canvas.

Keep:
- /music/ is NOT a checkout/sales page; it may remain only as a non-sales guide/archive or redirect into WORLD;
- music checkout is initiated only from BAR jukebox interactions inside the RPG;
- /journal/ HTML = SEO + articles;
- /legal/ HTML = legal;
- /support/ HTML = support.

Game:
- / = RPG entrance/world;
- eventually /game/ can host full engine if landing needs separation.

In-game interactions deep-link to static pages where appropriate.

## 14. Quality gates

### Visual
- no building-shadow ambiguity;
- no blurry raster text;
- no NPC-shaped stains;
- single projection/perspective rule;
- coherent light direction;
- minimum 4 readable main characters.

### Interaction
- walk;
- jump;
- talk;
- enter/exit interiors;
- BGM toggle;
- mobile controls;
- persistent settings.

### Production
- desktop screenshot;
- mobile screenshot;
- interaction QA;
- fresh public URL readback;
- no branch-only completion claims.

## 15. Development order

### Phase A — ART/ENGINE RESET
1. shadow and building projection guide;
2. 4 final character sheets;
3. exterior town redraw;
4. Space jump;
5. BGM toggle.

### Phase B — INTERIORS
6. MUSIC interior;
7. JOURNAL interior;
8. CAFE interior;
9. door/exit transitions.

### Phase C — WORLD LIFE
10. NPC dialogue system;
11. NPC schedules/wander;
12. inspectable props;
13. first quest.

### Phase D — GAME FOUNDATION
14. save;
15. inventory;
16. quest state;
17. map/camera scrolling;
18. second outdoor area.

### Phase E — FULL RPG
19. broader connected map;
20. transport;
21. day/evening state;
22. battle prototype;
23. narrative episodes.

## 16. Definition of success

The target is NOT:
"looks like a cheap clone of MOTHER2."

The target is:
"an original DOLZORE RPG that immediately feels like a carefully authored 16-bit town adventure: warm, odd, readable, explorable, and alive."
