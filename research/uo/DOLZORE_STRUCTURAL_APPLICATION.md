# UO -> DOLZORE STRUCTURAL APPLICATION

Authority: direct user instruction 2026-10-01 JST / issue #27
Evidence boundary: this file contains DOLZORE_DESIGN_RECOMMENDATION, not UO facts.

## 0. Layer assignment

DOLZORE must not merge all reference games into one undifferentiated clone.

MOTHER2 layer:
- exterior presentation/readability;
- ordinary-town adventure atmosphere;
- symbolic 16-bit-inspired visual grammar;
- everyday/strange contrast.

FFXI layer:
- deep combat/stat dependency;
- lineage/vocation/support-vocation structure;
- weapon/magic skill progression;
- physical/magic damage pipeline;
- cooperative timing and threat;
- loadout and horizontal-progression depth.

UO layer:
- persistent shared-world sandbox macrostructure;
- persistent world objects and containers;
- player property/housing/security;
- gathering/crafting/player-commerce loops;
- skill-driven life activity freedom;
- crime/notoriety/death/risk structure;
- pets/taming;
- proximity/social/guild/party communication;
- open-world travel/ships;
- public world events;
- shard/facet/ruleset separation;
- world/server persistence thinking.

FFXI combat core remains active unless a later explicit user decision replaces it.

## 1. Canonical world hierarchy proposal

Account
 -> Shard / World
    -> Ruleset Profile
       -> Region
          -> Spatial Cell / Stream Chunk
             -> Persistent Entity

Persistent Entity families:
- Character
- NPC / Creature
- Item
- Container
- Resource Node
- House / Property
- Ship / Vehicle
- Vendor
- Event Controller
- Portal / Travel Node

Private/public state must be explicit metadata, not inferred from Unity scene names.

## 2. Ruleset profiles

UO historical evidence proves that world geography/risk rules can be separated by facet/ruleset.
DOLZORE should support data-driven rule profiles from the beginning.

Possible DOLZORE rule dimensions:
- player-vs-player permission;
- criminal action permission;
- corpse/loot access;
- item-loss policy;
- resource abundance;
- guard protection;
- housing placement;
- event eligibility;
- fast-travel restrictions;
- market restrictions.

Do not reproduce Felucca/Trammel names, maps, art or exact rule numbers.

## 3. Character progression

Keep FFXI-derived vocation/combat identity.
Add a separate UO-inspired activity-skill layer for life/sandbox actions.

Example independent activity skills:
- mining;
- forestry;
- fishing;
- tailoring;
- smithing;
- carpentry;
- cooking;
- animal handling;
- commerce;
- navigation;
- identification/lore;
- medicine.

These are original DOLZORE names/data and must not silently change the combat skill-rank architecture.

## 4. Persistent object and container model

Every economically meaningful object should have a stable entity identity when persistence begins.
Inventory ownership must be separate from visual scene placement.
Containers should support nested contents with authoritative ownership/access rules.

Minimum item-state dimensions to reserve:
- entity_id;
- template_id;
- quantity;
- owner_id or world placement;
- parent_container_id;
- durability/current condition;
- bound/insured/protected policy if DOLZORE later adopts one;
- access/security state;
- crafted_by;
- provenance/event source;
- creation timestamp/version;
- rule-profile metadata where needed.

## 5. Housing/property as a platform

Housing should not be decorative-only.
Reserve:
- owner/co-owner/friend/visitor roles;
- public/private mode;
- storage and secure containers;
- object lockdown/placement;
- customization state;
- player shop/vendor attachment;
- bulletin/social attachment;
- transfer/demolition/decay policy;
- instancing or public-world placement strategy.

Housing can become a durable content platform for cosmetics, social retention, crafting and commerce.

## 6. Economy loop

World resources
 -> gathering
 -> processing
 -> crafting
 -> item condition/consumption
 -> repair/replacement
 -> direct trade / player vendor / centralized discovery
 -> gold/resource sinks
 -> renewed resource demand

Transactions must become server-authoritative before real-money-value-like scarcity or multiplayer trading matters.
Do not make the Unity client authoritative for inventory, currency, vendor inventory or trade result.

## 7. Death / crime / risk

DOLZORE should model death and crime as ruleset-controlled state machines rather than hard-coded map exceptions.

Separate:
- legal status/notoriety;
- criminal action event;
- victim/attacker relation;
- guard response;
- murder/repeat-offender counters if adopted;
- corpse ownership/access;
- item-loss/protection policy;
- resurrection state;
- penalties/recovery.

Do not copy UO thresholds/names as DOLZORE defaults without an explicit balance decision.

## 8. Social structure

Reserve channel classes:
- proximity say;
- proximity whisper;
- proximity yell;
- emote;
- party;
- guild;
- alliance/community;
- system/event;
- global where enabled.

Social permissions and moderation metadata should be server-side.

## 9. Pets / taming / companions

Treat tame/companion entities as persistent actors with:
- owner/control relationship;
- loyalty/obedience state if used;
- capacity/follower budget;
- training/progression;
- stable/storage state;
- transfer permissions;
- death/recovery policy;
- combat and non-combat roles.

This must coexist with any FFXI-inspired party/companion system without conflating them.

## 10. World events

Adopt the structural idea of location-bound escalating/public activity without copying UO champion content.
Event Controller should own:
- activation condition;
- region;
- phase/wave;
- spawn/objectives;
- participant tracking;
- reward entitlement;
- completion/cooldown;
- persistence across restart where intended.

## 11. Travel and ships

Treat ships/vehicles as persistent world entities rather than cutscene-only travel when this layer is implemented.
Reserve:
- owner;
- crew/access;
- cargo/container state;
- position/region;
- movement/navigation;
- docking/decay/recovery;
- event/combat hooks.

## 12. Server-authority boundary

Unity client:
- input intent;
- rendering;
- local prediction/interpolation as needed;
- UI/presentation.

Authoritative server:
- character persistent state;
- world entities;
- inventories/containers;
- item ownership;
- resources;
- combat result once multiplayer is active;
- currency/trades/vendors;
- housing/property;
- crime/notoriety;
- pets;
- world-event state.

## 13. Staged implementation aligned to existing DOLZORE roadmap

Stage 0 — current single-player persistent slice:
- use stable IDs/data models now;
- no unnecessary MMO infrastructure.

Stage 1 — asynchronous shared world:
- server-authoritative account/character/world-event/economy slices;
- no continuous position sync required.

Stage 2 — shared town shard:
- presence/movement/social;
- authoritative inventory/property/economy.

Stage 3 — public events/parties:
- region event controllers;
- participant/reward state.

Stage 4 — larger MMO only after demand:
- scale shard/region processes;
- operational partitioning;
- persistence/recovery/observability.

## 14. Immediate design impact

Before implementing combat content, DOLZORE data schemas should now reserve UO-derived macrostructure requirements.
Do not rewrite accepted First Town art or FFXI math simply to imitate UO.
Use UO to make the world persistent, ownable, social, economic and consequence-bearing.

DOLZORE_UO_STRUCTURAL_LAYER_V1=true
