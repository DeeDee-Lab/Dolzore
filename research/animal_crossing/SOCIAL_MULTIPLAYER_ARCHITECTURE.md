# ANIMAL CROSSING — SOCIAL / MULTIPLAYER ARCHITECTURE

Authority: DeeDee-Lab/Dolzore#40

## 1. Series principle

"Visiting" evolves across the entire lineage:
- N64 Controller Pak;
- GameCube Memory Card/GBA-related features;
- DS local wireless/Wi-Fi;
- Wii online/WiiConnect24/WiiSpeak;
- 3DS local/internet/streetpass/showcase;
- Switch local/online island visiting;
- Switch 2 larger online sessions and GameChat/CameraPlay.

Wii developer interview explicitly calls going out/visiting a core experience.

## 2. Critical design principle

Single-player life must be complete.
Connectivity should amplify motivation and expression, not hold core progress hostage.

## 3. Social layers

### Asynchronous traces
- letters;
- gifts;
- guest book;
- design share;
- market listings;
- screenshots;
- visited-home snapshots;
- resident stories about visitors.

### Delayed exchange
- item delivery;
- marketplace;
- town exports/imports;
- shared projects.

### Realtime visit
- host-authoritative town;
- guest permissions;
- shared activities;
- emotes/chat;
- temporary item interactions.

## 4. Authority model

For realtime shared town:
Host/Server owns:
- world topology;
- persistent object placement;
- resident state;
- economy transactions;
- item ownership changes;
- time/event state.

Client sends intents:
- move;
- interact;
- pick/drop;
- use tool;
- emote;
- chat.

Never let client-side Unity objects directly become authoritative persistent state.

## 5. Permission model

Guest capabilities should be tiered:
- view only;
- collect limited resources;
- place temporary object;
- trade;
- edit public zone;
- edit private/home zone.

Friend status does not automatically mean world-edit permission.

## 6. Social presence

Switch 2 Edition adds:
- up to 12 online players under the Edition conditions;
- CameraPlay;
- GameChat;
- microphone-based resident search.

Reusable lesson:
social presence can be layered onto an already-complete life simulation.

## 7. DOLZORE staging

1. persistent single-player town;
2. asynchronous social traces;
3. visiting read-only/limited;
4. trading/gifting;
5. small realtime town shard;
6. shared public events;
7. editable cooperative spaces only after authorization model is mature.

## 8. Abuse/safety

Need:
- report/block;
- chat filtering;
- owner rollback;
- placement permission;
- trade confirmation;
- transaction ledger;
- rate limits;
- snapshot restore for vandalism.

## 9. Anti-copy

Do not copy:
- friend-code UX;
- airport/gate fiction;
- dream-address naming;
- exact visit ceremonies.
Use original world fiction and protocol.
