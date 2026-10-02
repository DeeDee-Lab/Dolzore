# DOLZORE LIVING-TOWN IMPLEMENTATION CHECKLIST — DERIVED FROM ANIMAL CROSSING RESEARCH

These are original DOLZORE implementation requirements, not Nintendo specifications.

## A. World clock
[ ] Authoritative TimeService exists.
[ ] Day rollover is a discrete transaction.
[ ] Weekday/season/event calendar is data driven.
[ ] Lighting/audio/services/NPC schedules subscribe to time state.
[ ] Offline catch-up is deterministic.
[ ] Changing device clock cannot corrupt persistent state.

## B. NPC life
[ ] NPCDefinition separate from NPC runtime object.
[ ] DailySchedule data exists.
[ ] Home/work/leisure anchors exist.
[ ] Activity changes dialogue context.
[ ] Relationship state persists.
[ ] NPC can remember selected past events.
[ ] Move-in/out is explicit state, not despawn.
[ ] Event schedule can override normal schedule.

## C. Dialogue
[ ] Personality is not the only selector.
[ ] Selectors include time/activity/weather/relationship/event/location/repetition.
[ ] Recent-topic cooldown prevents obvious repetition.
[ ] Localization intent/variables stored separately from rendered text.
[ ] NPC activity and dialogue cannot contradict each other.

## D. World
[ ] Persistent public object placement.
[ ] Stable IDs for buildings/plots/objects.
[ ] Terrain separated from decoration.
[ ] Ecology state has growth/decay rules.
[ ] Weather/season alters spawns and visuals.
[ ] Public-space permissions differ from private housing.

## E. Collections
[ ] Fish/insect/etc. spawn table is data-driven.
[ ] Spawn rule supports time/month/season/weather/location/rarity.
[ ] Collection record persists independently of inventory.
[ ] Museum/journal display is derived from collection state.
[ ] Selling/crafting does not delete discovery record.

## F. Housing
[ ] House layout stored as structured placement data.
[ ] Furniture catalog separate from owned copies.
[ ] Decoration mode supports grid/snap and free-enough expression.
[ ] Exterior and interior permissions differ.
[ ] NPC homes can be generated from personality/identity templates.

## G. Items/content scale
[ ] Item data is database-driven.
[ ] Variants/customization data separate from base item.
[ ] Tags support room/theme/shop/event/craft/use.
[ ] Batch import/validation tooling exists.
[ ] Duplicate IDs and broken references fail CI.
[ ] Art/content can scale to thousands without code changes.

## H. Economy
[ ] General currency ledger.
[ ] Shop stock generation separated from UI.
[ ] Daily stock rotation.
[ ] Buy/sell prices are data.
[ ] Building/project costs are data.
[ ] Craft materials have sources/sinks.
[ ] Avoid economy loops that require real-money purchases for power.

## I. Seasons/events
[ ] EventDefinition has recurrence and locale/calendar policy.
[ ] Event can alter NPC schedule/dialogue/music/spawns/shops/decor.
[ ] Event assets/content versioned.
[ ] Missed event does not corrupt normal-world state.
[ ] Real-world regional differences are configurable.

## J. Social
[ ] Core life loop works offline/solo.
[ ] Visiting does not hand ownership authority to guests.
[ ] Guest permissions explicit.
[ ] Trade/gift transaction authoritative.
[ ] Player-created designs preserve author/provenance IDs.
[ ] Async sharing possible before large MMO dependency.

## K. Audio
[ ] Hour/time-state music architecture considered.
[ ] Area ambience reacts to weather/time.
[ ] Footsteps/material audio.
[ ] Residents/object sounds are spatial.
[ ] Event music overrides are data-driven.
[ ] Silence/low-music periods allowed.

## L. Save schema
[ ] save_schema_version stored.
[ ] migration path tested.
[ ] static content IDs stable.
[ ] no scene/GameObject pointers in durable save.
[ ] backup/recovery plan.
[ ] corruption validation/checksum.
[ ] migrations are idempotent.
[ ] unknown/new fields do not silently reset unrelated systems.

## M. Return motivation
[ ] short loop: 5–15 min useful visit.
[ ] daily loop.
[ ] weekly loop.
[ ] seasonal loop.
[ ] long collection loop.
[ ] resident relationship loop.
[ ] decorating loop.
[ ] no single loop is mandatory for all players.

## N. Anti-clone
Reject if:
[ ] villagers visually imitate Nintendo characters;
[ ] exact personality/dialogue archetypes are copied;
[ ] Bell/debt/shop values are copied;
[ ] island/village topology matches a reference;
[ ] exact furniture/catalog content copied;
[ ] hourly music imitates Nintendo melodies;
[ ] holiday implementation recreates proprietary content;
[ ] UI presentation is recognizably cloned.
