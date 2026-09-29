# DOLZORE STORY / LEVEL / EQUIPMENT / SAVE ARCHITECTURE
Authority: 2026-09-29 JST

## 1. Core decision

DOLZORE can become a full RPG without a dedicated game server at the beginning.

The game is designed as:
- static client game on GitHub Pages;
- story/items/maps/equipment definitions shipped as static data;
- local save in IndexedDB;
- small preferences in localStorage;
- optional cloud save added later.

Music purchases remain separate from gameplay progression.
Buying music must never grant levels, battle power or exclusive progression.

---

# 2. Capacity reality

Level, equipment and story state are tiny.

Typical per-player save:
- level / XP / stats: < 1 KB
- inventory 100–300 items: a few KB
- equipment: < 1 KB
- quest flags: a few KB
- NPC/world flags: 5–30 KB
- discovered map state: 5–30 KB
- settings: < 1 KB

Practical target:
20–80 KB per save slot.

What actually consumes space:
1. music
2. sound effects
3. pixel-art tiles / character sheets
4. large maps
5. cutscene art

Therefore game-state architecture should not be distorted to save bytes.

---

# 3. Save architecture

## Phase 1 — local only

localStorage:
- BGM on/off
- volume
- language
- control settings
- last save slot id

IndexedDB:
- save slots
- player progression
- inventory
- equipment
- quests
- NPC flags
- map discovery
- story state

Save schema:
```json
{
  "version": 1,
  "slot": 1,
  "player": {
    "level": 1,
    "xp": 0,
    "hp": 24,
    "maxHp": 24,
    "focus": 8,
    "maxFocus": 8,
    "position": {"scene":"town01","x":252,"y":184},
    "equipment": {
      "body": "mint_jacket",
      "shoes": "canvas_sneakers",
      "charm": null,
      "tool": "field_pouch"
    }
  },
  "inventory": [],
  "quests": {},
  "worldFlags": {},
  "npcFlags": {},
  "discovered": [],
  "playTimeSec": 0
}
```

Advantages:
- zero server cost
- works offline after assets are loaded
- instant save
- simple
- perfect for early development

Limitations:
- save belongs to that browser/device
- browser data deletion removes the save
- client data can be edited by the user

---

# 4. Optional cloud save later

Do not add cloud infrastructure until one of these becomes real:
- users ask for cross-device saves;
- accounts are introduced;
- leaderboard/achievements need authority;
- multiplayer/social features exist;
- cheating matters;
- a large number of public players exists.

Preferred candidates:

## Option A — Cloudflare Workers + D1

Good fit for:
- tiny save records
- simple REST API
- low cost
- static Pages-style deployment

Current 2026 Free-tier reference:
- D1: 500 MB per database
- D1: 5 GB storage per account
- D1: 5 million rows read/day
- D1: 100,000 rows written/day
- Workers: 100,000 requests/day

Example:
at 40 KB compressed save/player:
500 MB holds roughly 12,000 full saves in one D1 database before considering indexes/overhead.

Recommended D1 table:
```
players
  id
  created_at

saves
  player_id
  slot
  version
  save_json
  updated_at

achievements
  player_id
  achievement_id
  unlocked_at
```

## Option B — Supabase

Good fit if we want:
- authentication quickly
- Postgres
- cloud saves
- admin tooling

Current 2026 Free plan:
- 500 MB database per active project
- 1 GB Storage
- 50,000 MAU
- 500,000 Edge Function invocations
- 5 GB egress

Supabase is easier when account/login becomes important.
Cloudflare is leaner when the backend is only a small save API.

## Recommendation

Phase 1:
**NO GAME SERVER.**

Phase 2:
If cloud saves become necessary, choose:
**Supabase for auth-first**
or
**Cloudflare D1 for lean save-API-first**.

Do not introduce both.

---

# 5. Story premise

Working title:
## DOLZORE — まちの音が、少しずれる。

The town is ordinary.
No chosen-one prophecy.
No obvious world-ending villain at the beginning.

SORA has recently begun spending time in DOLZORE.
MELO notices a sound from the BAR jukebox that should not be there.
YUZU notices town records that disagree by one small detail.
PON knows a road that sometimes seems shorter than it was yesterday.

At first these are unrelated everyday oddities.

Then the four discover a recurring phenomenon:

## "ズレ"

A ZURE is not a monster or magic spell.
It is a small disagreement between:
- sound and source;
- map and street;
- memory and record;
- time and clock;
- object and owner.

Examples:
- a jukebox label names a song that nobody remembers adding;
- a bus-stop timetable contains a stop that does not exist;
- the river reflection shows a streetlight that is not there;
- a cafe radio reports tomorrow's weather as yesterday's;
- a photo contains a bench that appeared only the following week.

The first half of the story treats these as local mysteries.
Later, the player learns that the ZUREs connect several towns.

The central question is not:
"Who will destroy the world?"

It is:
"Why is the world remembering slightly different versions of itself?"

This lets the game grow slowly from one town to multiple connected towns.

---

# 6. Story arcs

## Prologue — DOLZORE Town

Learn:
- move
- jump
- talk
- inspect
- enter buildings

Meet:
- MELO
- YUZU
- PON

End:
A BAR jukebox plays one note after the power switch is turned off.

---

## Episode 1 — The Silent Jukebox

The first town BAR's jukebox can light up but cannot complete a selection.

MELO hears three mechanical sounds missing from its startup sequence.

Player explores:
- BAR
- JOURNAL
- CAFE
- street
- riverside later

Three clues are ordinary objects, not magical relics.

Result:
jukebox works again.

But one track label appears with no matching audio file.
It is dated one week in the future.

This becomes the first confirmed ZURE.

---

## Episode 2 — The Wrong Address

YUZU finds three documents that list the same building at different street numbers.

At night, one number leads to a narrow road that is not present during the day.

Introduces:
- evening town state
- first temporary route
- first larger story flag

---

## Episode 3 — Riverside Signal

A small portable radio picks up a repeating sequence near the river.

PON remembers hearing it in another district.

Introduces:
- second outdoor area
- signal-based environmental puzzle
- first journey beyond town center

---

## Episode 4 — The Second Town

The four reach another town.

It has:
- its own BAR
- its own BAR theme
- different recommended jukebox tracks
- new residents
- a different kind of ZURE

The global story begins to connect.

---

# 7. Level system

Do not make levels the reason to grind random encounters.

Initial cap:
**LV 30**

Early release cap:
**LV 10**

XP sources:
- story quests
- exploration discoveries
- optional NPC quests
- later battles
- environmental puzzle completion

No XP from:
- buying music
- clicking ads
- repeating trivial interactions

Core stats:

## HEART
Physical durability / HP.

## FOCUS
Resource for abilities / techniques.

## POWER
Physical / action strength.

## GUARD
Damage resistance.

## SPEED
Turn order / movement-related mechanics later.

## SENSE
Observation / unusual-interaction stat.
Can reveal optional dialogue, clues or hidden objects.

## LUCK
Small probability modifier only.
Never dominate progression.

Level-up:
- HP / Focus rise consistently
- other stats grow by character profile
- occasional choice point every few levels

SORA should remain balanced.

---

# 8. Equipment system

Equipment should matter before combat exists.

Slots:

## BODY
Examples:
- Mint Jacket
- Rain Shell
- Work Apron

Effects:
- defense later
- rain/weather interactions
- dialogue/world flags in rare cases

## SHOES
Examples:
- Canvas Sneakers
- Trail Shoes
- Rubber Boots

Effects:
- run speed
- jump recovery
- puddle/mud behavior
- later combat speed

## CHARM
Examples:
- Bent Coin
- Small Bell
- Bus Token
- Glass Bead

Effects:
- Sense / Luck
- unusual event triggers
- no huge raw-stat bonuses

## TOOL
Examples:
- Field Pouch
- Flashlight
- Pocket Radio
- Small Toolkit

Effects:
exploration ability rather than combat power.

This gives equipment value immediately.

---

# 9. Item philosophy

Avoid 200 meaningless items.

Item categories:
- key item
- consumable
- equipment
- clue
- collection / keepsake

Early game target:
- 10–15 equipment pieces
- 8–12 consumables
- 15–25 key/clue objects
- a small set of keepsakes

Expand only when systems need them.

---

# 10. Battle timing

Battle system is NOT required for levels/equipment to exist.

Levels can begin through quests/exploration.

Battle should enter after:
- movement is fun
- interiors work
- second area exists
- dialogue/quest/save systems are stable

This prevents battle code from swallowing development before the world feels alive.

---

# 11. Server decision gate

Stay serverless/local until we have:
- at least several public game slices;
- real user saves worth syncing;
- a reason for accounts.

At that point choose one backend only.

Decision heuristic:

Use **Cloudflare D1** if:
- login is simple/optional;
- save API is small;
- low ops is priority.

Use **Supabase** if:
- full user accounts matter;
- auth/email/social login are needed;
- admin/database inspection matters.

---

# 12. Capacity budget

Target static package budget:

Phase 1:
< 20 MB

First town + interiors:
< 50 MB

Several towns:
< 150 MB

Larger RPG:
keep under 400–600 MB if possible.

GitHub Pages published sites have a 1 GB maximum, so asset discipline matters long before story/save data does.

Audio budget:
- prefer small looped OGG/Opus assets
- reuse SFX
- avoid dozens of uncompressed WAV files

Pixel art budget:
very small compared with audio.

---

# 13. Incremental release order

1. Character/JUMP/BGM
2. First BAR + jukebox purchase
3. JOURNAL interior
4. CAFE interior
5. first quest/prologue
6. save system
7. level + equipment menu
8. riverside
9. evening state
10. second town + second BAR
11. Episode 1
12. only then consider battle prototype

This order intentionally lets players watch the world grow.
