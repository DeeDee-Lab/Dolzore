# MOTHER2 ENEMY / ITEM / SERVICE SYSTEM GRAMMAR

Authority: DOLZORE MOTHER2 research issue #26
Purpose: extract how ordinary life is converted into RPG systems without copying characters, enemies, items or names.

## 1. Enemy concept principle

OFFICIAL_NINTENDO enemy material demonstrates a wide conceptual range:
- human troublemakers;
- biological/dirty everyday threats;
- folklore/rumor creatures;
- abstract art-like entities;
- ordinary things made hostile or absurd.

Reusable principle:
Enemy design does not need a single fantasy taxonomy. The shared rule can be "what this world can make strange."

DOLZORE enemy concept sources:
- daily object;
- local occupation;
- urban infrastructure;
- weather/ecology;
- rumor;
- social behavior;
- ZURE anomaly;
- machine/tool;
- memory artifact;
- wild creature.

Do not copy MOTHER2 enemy names, silhouettes, jokes or object choices one-for-one.

## 2. Enemy = mechanics + local culture

An enemy should communicate at least two dimensions:
- mechanical role;
- regional/worldview role.

EnemyDefinition:
- combat_role;
- silhouette_role;
- biome/culture tag;
- behavior verb;
- signature status/effect;
- field behavior;
- social/environmental origin;
- rarity;
- audiovisual profile.

This prevents enemies from becoming generic stat bags.

## 3. Field behavior matters

Visible encounters mean enemy identity begins before battle.

FieldBehaviorProfile:
- idle;
- wander;
- patrol;
- avoid player;
- chase;
- ambush;
- block route;
- cluster;
- protect object;
- flee when player is strong;
- react to time/world state.

A harmless-looking entity may still be strange, but threat readability must remain fair.

## 4. Strength communicated without floating levels

Original DOLZORE techniques:
- movement confidence;
- pursuit distance;
- group size;
- field animation;
- territory;
- audio cue;
- party reaction;
- scan/knowledge system.

Do not require exact numeric labels over every field enemy.

## 5. Trivial enemy handling

When the player massively outclasses an ordinary threat, repeated full battle transitions create friction.

DOLZORE:
Use FastResolveEvaluator or enemy avoidance/flee behavior.
Preserve:
- rewards;
- quest counters;
- drops;
- state hooks;
- special exceptions.

## 6. Item principle: mundane object can be mechanically meaningful

OFFICIAL_NINTENDO item examples show ordinary-looking objects can have battle/equipment function.

Reusable principle:
Do not separate "serious RPG items" from "world props" too sharply.

DOLZORE categories:
- food;
- medicine;
- clothing;
- tools;
- hobby object;
- machine;
- document;
- key object;
- keepsake;
- crafting material;
- oddity.

Mechanics should emerge from fiction where possible.

## 7. Food as worldbuilding

CREATOR_INTERVIEW / Miura:
Food variety was deliberately tied to travel and place experience.

DOLZORE FoodDefinition:
- region;
- price tier;
- availability;
- recovery/stat effect;
- description intent;
- cultural tag;
- favorite/dislike hooks;
- NPC reference hooks.

Every region should have food that says something about the place.

## 8. Equipment should reveal character

Do not design every party member around the same generic sword/armor slots.

DOLZORE:
EquipmentCategory can vary by job/culture.
Visual equipment need not always be shown on field sprite if pixel budget/readability would suffer.

Item data:
- allowed jobs/characters;
- mechanical role;
- world origin;
- shop/loot/craft sources;
- rarity;
- inspect text;
- resale/trade policy.

## 9. Services embedded in fiction

OFFICIAL_MANUAL/ARCHIVE examples:
- home/hotel recovery;
- telephone-based save framing;
- public phone with small usage cost;
- map acquired in world;
- delivery service;
- transport such as bicycle;
- photography/optional event.

Reusable principle:
System UI can be diegetically anchored in world institutions.

DOLZORE service categories:
- lodging/recovery;
- banking/storage;
- transport;
- courier;
- map/information;
- repair;
- crafting;
- clinic;
- social venue;
- photo/memory;
- bar/jukebox;
- journal/news.

Not every backend function needs a magical menu.

## 10. Save system vs save fiction

Technical save must be robust and modern.
Fictional save interaction can still provide character/world flavor.

DOLZORE:
- autosave for safety;
- manual save where appropriate;
- fictional "check-in" interaction can trigger dialogue/state feedback;
- never risk player data for nostalgia.

## 11. Money/economy tone

MOTHER2's modern-life setting makes prices, shops and services feel like part of daily life rather than medieval abstractions.

DOLZORE:
Use economy to communicate class/local culture carefully:
- cheap/common options;
- premium/tourist options;
- local specialty;
- service fees;
- availability differences.

Avoid tedious ATM/administrative friction unless it adds meaningful choice.

## 12. Inventory friction

If inventory capacity is intentionally limited:
- player needs storage/courier solutions;
- the world can respond through services;
- inconvenience becomes a system relationship.

DOLZORE decision:
Do not copy exact inventory limits.
Choose friction based on whether preparation depth is desired.

## 13. Optional service memory anchors

Low-stakes services can become memorable because of:
- operator personality;
- unusual process;
- repeated call/reaction;
- world-state change;
- sound cue.

Design principle:
Utility NPCs need personality but should remain efficient on repeat use.

First interaction may be flavorful; repeated service should allow fast path.

## 14. Systemic everyday-world checklist

For each town ask:
- where can player recover?
- where can player buy/sell?
- how does player learn local information?
- how does player travel?
- where do residents relax?
- what is delivered/moved?
- what service feels unique?
- what is expensive/cheap here?
- what becomes unavailable during crisis?
- what changes after resolution?

## 15. Anti-copy boundary

Do not reproduce:
- reference enemy concepts one-for-one;
- specific joke items;
- exact service dialogue;
- exact telephone/save structure;
- exact shop inventories/prices;
- exact transport sequence.

Learn:
ordinary life can carry mechanical systems.

## 16. Sources

Nintendo official enemy archive:
https://www.nintendo.co.jp/n08/a2uj/mother2/monster/index.html

Nintendo official item archive:
https://www.nintendo.co.jp/n08/a2uj/mother2/item/index.html

Nintendo official screenshots:
https://www.nintendo.co.jp/n08/a2uj/mother2/screen/index.html

Nintendo manual:
https://www.nintendo.co.jp/data/software/manual/man_jbbj.pdf

Miura interview:
https://www.1101.com/n/s/mother_project/miura_akihiko/index.html
