# DOLZORE SHARED-WORLD / MMO ARCHITECTURE
Authority: 2026-09-29 JST

## 0. Decision

DOLZORE may grow from a single-player browser RPG into a shared-world online RPG and eventually an MMO.

Do NOT jump directly to a massive single-world server.

Expansion path:
1. single-player persistent RPG;
2. asynchronous shared-world features;
3. small real-time town shards;
4. shared public events + parties;
5. only after demand proves it: larger MMO infrastructure.

The goal is an original DOLZORE world:
- warm everyday towns;
- strange local residents;
- time-layer exploration;
- cause/effect across eras;
- personal character creation;
- BARs as social/music hubs.

No MOTHER/EarthBound or Chrono Trigger characters, sprites, dialogue, maps, melodies, named mechanics or exact story beats are copied.

---

# 1. Story direction — everyday town × time-spanning adventure

Working story title:
## DOLZORE — ずれた時刻の向こう側

### Core phenomenon: ZURE / ズレ

A ZURE begins as a tiny contradiction:
- a song label dated next week;
- a street number that differs between two official records;
- a reflection showing a lamp that does not exist;
- a person remembering a store that has never opened;
- a BAR receipt printed decades too early.

At first the four core characters believe the problems are local.

Then the player discovers that some ZUREs are not mistakes in information.

They are overlaps between different versions of time.

---

# 2. Time layers

Instead of a conventional time machine, DOLZORE uses **Time Layers**.

A place can briefly align with another version of itself.
Players cross through environmental triggers called **Seams / 継ぎ目**.

This keeps time travel physical and tied to exploration.

## Layer A — PRESENT
The current DOLZORE world.

Characteristics:
- ordinary town
- BAR / JOURNAL / CAFE
- local rumors
- first ZUREs

## Layer B — OLD TOWN
Roughly 30 years earlier.

The same streets exist but:
- buildings have different owners;
- roads are incomplete;
- younger versions of older NPCs appear;
- objects that are gone in the present still exist.

Cause/effect examples:
- repair a sign in the past -> hidden route clue survives in the present;
- leave a notebook -> YUZU finds an old record in the present;
- move a bench -> future river path changes.

## Layer C — FAR TOWN
Roughly 70 years later.

The town is quieter, denser and partly overgrown.

Important rule:
Future is not purely dystopian.
Some things are better.
Some are gone.
Some ordinary items are treated as history.

## Layer D — BLANK HOUR / 空白時
A short unstable layer that belongs to no normal calendar.

Characteristics:
- familiar places with missing details;
- clocks do not advance;
- no ordinary NPC schedules;
- audio behaves differently;
- certain ZUREs can only be resolved here.

This becomes the most mysterious layer.

## Layer E — FIRST MORNING
Late-story origin layer.
Only introduced after the player understands the consequences of changing time.

---

# 3. Main story arc

## ACT 1 — LOCAL ODDITIES
Town-scale.
Player meets MELO, YUZU and PON.
First BAR jukebox produces an impossible track label.

## ACT 2 — THE FIRST SEAM
A ZURE at riverside briefly opens OLD TOWN.
Player learns that changing a small object can affect PRESENT.

## ACT 3 — SECOND TOWN
Travel to a second town.
It has another BAR and a different ZURE pattern.
Evidence proves the phenomenon is regional, not local.

## ACT 4 — FAR TOWN
The party reaches a future layer.
They discover records about a project that attempted to preserve disappearing towns by recording not only images and sound, but complete states of places.

## ACT 5 — THE ARCHIVE PROBLEM
The preservation system did not simply store memories.
It began keeping multiple incompatible versions of the world active.

ZUREs are collisions between those versions.

## ACT 6 — WHO GETS TO BE THE REAL VERSION?
The conflict is not simply defeating an evil person.

The player must deal with:
- people who only exist in one layer;
- locations that survive only because another version disappeared;
- memories that are false in one timeline but real in another.

The final choices concern coexistence, preservation and loss.

MMO/shared-world events can occur around large Seams without changing each player's private story ending.

---

# 4. Character creation

## Player role

SORA becomes the **default starter preset**, not a mandatory fixed appearance.

Players can:
- keep SORA preset;
- rename;
- customize appearance;
- change outfit;
- choose colors.

Story NPCs refer to the chosen player name.

MELO / YUZU / PON remain canonical shared NPCs.

## Character creator v1

Player chooses:

### Identity
- name
- display name
- optional pronoun / reference style
- no gender-locked clothing

### Body / face
- skin tone palette
- 3 body silhouettes kept subtle at pixel scale
- face set
- eye style

### Hair
- 12 initial hairstyles
- 12–16 hair colors

### Clothes
- top
- bottoms
- shoes
- outer layer

### Accessory
- glasses
- hat
- scarf
- hair accessory
- small jewelry

### Bag/tool
- pouch
- backpack
- notebook bag
- record bag
- none

### Colors
Use controlled palettes so combinations stay visually coherent.

---

# 5. Paper-doll sprite architecture

Do NOT pre-render every possible player combination.

Use layered sprite sheets with identical frame grids:

```
base/
hair/
face/
top/
bottom/
shoes/
accessory/
bag/
equipment/
```

Every layer uses:
- 32×40 native frame
- down walk 2
- left walk 2
- right walk 2
- up walk 2
- later jump / idle / talk

Runtime draws layers in order.

A player profile stores only IDs/colors, e.g.:

```json
{
  "name":"YOSHI",
  "skin":"skin_04",
  "hair":{"id":"hair_07","palette":"brown_03"},
  "face":"face_02",
  "top":{"id":"jacket_05","palette":"mint_02"},
  "bottom":"pants_03",
  "shoes":"shoe_02",
  "accessory":"glasses_01",
  "bag":"backpack_04"
}
```

This is tiny.
Character customization does not meaningfully increase save-data size.

---

# 6. Equipment appearance

Equipment can visually change the avatar.

Slots:
- BODY
- SHOES
- CHARM
- TOOL

Not every stat item needs a unique giant asset.

Use:
- palette variants
- small overlays
- accessory layers
- occasional full top/bottom sprite layers

This keeps asset size manageable.

---

# 7. Multiplayer stages

## Stage 0 — single-player
Current direction.
Everything local.

## Stage 1 — asynchronous shared world
Very cheap.

Features:
- see town population count
- community message board
- global discovered-ZURE count
- daily shared target
- ghost records / recent footprints

No continuous movement synchronization.

This can feel connected without MMO cost.

## Stage 2 — shared town shards
First real-time multiplayer.

Target:
- 12–30 visible players per town instance initially
- WebSocket presence
- position/direction/emote sync
- chat or preset phrases
- BAR social spaces
- parties

Story interiors remain instanced/private when necessary.

## Stage 3 — public events
Examples:
- a large Seam opens at riverside;
- town lights fail;
- unusual future NPC appears;
- BAR jukebox receives a temporary global track clue.

20–50 players can cooperate inside a shard.

## Stage 4 — MMO
Only after real player demand exists.

Features:
- account cloud saves
- friends
- guild/group equivalent
- world events
- larger shards
- matchmaking
- authoritative battle
- moderation
- anti-cheat
- analytics/operations

---

# 8. Why true MMO needs a backend

Single-player RPG:
no server required.

True real-time MMO:
a backend IS required.

Reasons:
- player position synchronization
- presence
- chat
- authoritative item ownership
- account/save sync
- anti-cheat
- party state
- shared events
- moderation

Peer-to-peer/WebRTC alone is not recommended for a persistent MMO:
- NAT/connectivity complexity
- host authority problems
- cheating
- persistence
- player churn
- moderation

---

# 9. Recommended realtime backend

## Preferred: Cloudflare Workers + Durable Objects + D1

Client:
- GitHub Pages / Phaser 3

Realtime:
- Cloudflare Durable Object per world shard / zone
- WebSocket connections
- Hibernation API where appropriate

Persistence:
- D1
- profile
- save
- inventory
- equipment
- quest state
- account linkage

Why:
Durable Objects are specifically suited to coordinating multiple clients and multiplayer/game-style realtime state.

### Free tier reality (2026)

Workers Free:
- 100,000 requests/day

Durable Objects Free:
- available with SQLite backend
- 100,000 requests/day
- 13,000 GB-s/day
- 5 GB account storage
- many objects/classes possible

D1 Free:
- 500 MB/database
- 5 GB/account
- 5M rows read/day
- 100k rows written/day

This is excellent for:
- development
- internal testing
- closed alpha
- small public experiments

It is NOT realistic to assume a popular realtime MMO remains free forever.

Movement messages must be aggressively optimized.

---

# 10. Realtime traffic strategy

Do NOT send 60 FPS network updates.

Client renders at 60 FPS.
Network state sends much less often.

Initial target:
- 4–8 movement updates/sec while moving
- 0 updates/sec while idle except heartbeat
- direction/action changes immediately
- interpolate remote players client-side

Area of interest:
send nearby players only.

Shard:
start 12–30 players.

This greatly reduces realtime traffic.

---

# 11. Alternative: Supabase

Supabase Realtime Free currently supports:
- 200 concurrent connections
- 100 messages/sec

Good for:
- prototype
- auth-first product
- simple presence/chat

Less ideal than Durable Objects for authoritative game-room logic.

Decision:
Prefer Cloudflare for realtime world authority.
Do not introduce both Cloudflare realtime and Supabase realtime initially.

---

# 12. Identity strategy

## Alpha
Guest identity.

Generate:
- player UUID
- local credential/token
- profile saved locally + backend when multiplayer is enabled

## Later
Account linking.

Possible:
- passkey
- email magic link
- OAuth

Do not require account registration before a new user can walk around the town.

---

# 13. Shared world vs story

Important architecture rule:

## Shared
- town exterior
- BAR
- public roads
- town events
- chat/emotes
- social NPC position where safe

## Instanced per player/party
- story-critical conversations
- time-layer decisions
- puzzle states
- boss/event states
- endings

This solves the classic MMO problem where every player needs to be the protagonist without destroying world consistency.

---

# 14. BAR network

Every town has a BAR.

BARs are:
- music purchase points;
- social hubs;
- rumor hubs;
- party meetup locations;
- time-layer clue locations.

Each BAR has:
- unique name
- owner/bartender
- palette/interior
- recommended tracks
- local rumor table
- same global music catalog

BAR jukebox purchase:
`BAR -> JUKEBOX -> PREVIEW -> BUY -> STRIPE`

Purchases never provide battle power.

---

# 15. MMO economy

Do not begin with player-to-player currency trading.

Early multiplayer economy:
- no tradable premium power
- music purchase stays external product purchase
- quest/equipment items bind to player initially
- cosmetics may become shareable later

Reason:
player trading introduces duping, fraud and server authority complexity early.

---

# 16. Recommended development sequence

## Current
Slice 1:
character + jump + BGM.

## Next
Slice 2:
first BAR + jukebox purchases.

## Then
Slice 3–8:
single-player RPG systems and first story.

## Shared-world Alpha A
Add asynchronous community state.

## Alpha B
Add 12-player town shard.

## Alpha C
30-player shard + BAR social hub.

## Beta
accounts + cloud save + parties + public ZURE event.

## MMO decision gate
Only then decide whether to scale toward hundreds/thousands of concurrent users.

---

# 17. Cost expectation

### Single-player / local save
≈ zero backend cost.

### Closed multiplayer alpha
Can plausibly fit in free Cloudflare limits if activity is small and network updates are controlled.

### Small public shared world
May still be cheap, but monitor request/message usage.

### Actual MMO
Expect paid infrastructure.
Cloudflare Workers Paid begins at a low monthly base price, but realtime usage and scale eventually become operational costs.

Do not make "must remain free" a permanent architectural requirement if the game succeeds.

Revenue can then pay for the backend.

---

# 18. Definition of MMO success

DOLZORE should not chase "thousands visible on one screen."

Success means:
- the world feels inhabited;
- friends can meet;
- strangers can appear naturally;
- BARs feel social;
- public events feel shared;
- personal story still matters;
- time-layer choices remain coherent;
- backend remains maintainable.
