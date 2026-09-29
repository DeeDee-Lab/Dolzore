# MOTHER2 NPC / LIFE / SOCIAL TEXTURE GRAMMAR

Authority: DOLZORE MOTHER2 research issue #26
Important evidence distinction:
MOTHER2 demonstrates rich NPC dialogue/placement and state reactions.
This file's schedule simulation recommendations are DOLZORE_DESIGN_RECOMMENDATION, not a claim that MOTHER2 ran modern NPC schedules.

## 1. NPCs are the town's sensor network

A town feels different because residents:
- comment on local conditions;
- occupy meaningful public/private places;
- provide useful and useless information;
- react to strange events;
- continue to have personal concerns.

DOLZORE:
NPCs should report the world indirectly through their lives, not only quest markers.

## 2. NPC functional taxonomy

Use a balanced population:

- SERVICE: performs utility.
- ORIENT: tells player about place.
- LOCAL: represents everyday resident.
- RELATION: connected to another NPC.
- RUMOR: incomplete information.
- COMIC: social release.
- UNEASE: makes player uncomfortable/alerts to problem.
- STORY: required plot.
- MEMORY: optional memorable interaction.
- MOBILE: traveler/wanderer.
- AUTHORITY: civic/institutional role.
- CHILD/YOUTH: different worldview.
- OUTSIDER: visitor/tourist.

One NPC can carry 2–3 roles.

## 3. Avoid quest-terminal population

Reject a district where most visible NPCs:
- have markers;
- issue tasks;
- wait forever in one pose;
- disappear after reward;
- speak only about player mission.

At least half of ambient social space should feel independent of the player's current objective.

This is a DOLZORE production recommendation.

## 4. Placement tells story

NPC location should answer:
- why are they here?
- who are they near?
- what can they see?
- what are they doing?
- what changed because of the local incident?

Placement is narrative data.

## 5. Social clusters

NPCs in groups can communicate:
- event/crowd;
- queue;
- gossip;
- family;
- work crew;
- conflict;
- leisure;
- authority barrier.

Use clusters to alter route/navigation as well as dialogue.

## 6. Movement profiles

DOLZORE original BehaviorProfile:
- stationary_service;
- stationary_watch;
- short_wander;
- route_patrol;
- commute;
- follow_target;
- social_cluster;
- flee_threat;
- investigate;
- scripted_scene;
- indoor_outdoor_cycle.

Movement speed and pause patterns should differ by role.

## 7. Schedule model

For DOLZORE:
NPCSchedule:
- time block;
- location anchor;
- behavior profile;
- dialogue set;
- world-state conditions;
- fallback location;
- event interruption.

Start simple:
DAY / EVENING / EVENT.
Do not implement complex 24-hour simulation before first town quality is accepted.

## 8. Crisis transforms society

A crisis should alter:
- who is outdoors;
- service availability;
- crowd location;
- authority presence;
- ambient dialogue;
- enemy incursion;
- lighting/audio.

After resolution, town should visibly relax/change.

## 9. Repeated NPC reuse

MOTHER2 used compact art resources and residents could share/reuse graphical material.

DOLZORE can reuse base sprite construction, but preserve identity through:
- head/hair;
- top/lower color blocks;
- prop;
- behavior;
- location;
- dialogue role.

A reused body is acceptable; a reused person is not.

## 10. Relationship graph

NPCs become more believable when lines refer to other actual residents.

NPCRelation:
- family;
- coworker;
- neighbor;
- customer;
- rival;
- friend;
- authority;
- rumor target.

Town authoring tool should surface orphan NPCs with zero relations.

## 11. Rumor consistency

Residents may be wrong, but wrongness should be authored.

RumorData:
- claim;
- speaker belief;
- factual truth;
- story state;
- propagation;
- later correction.

This permits mystery without accidental continuity errors.

## 12. Social discomfort

Reference tone includes rude, embarrassing or belittling interactions alongside warmth.

DOLZORE:
Not every NPC should praise or validate the player.
Use:
- impatience;
- skepticism;
- teasing;
- class/status difference;
- misunderstanding;
- indifference.

But avoid cruelty as default tone.

## 13. Children and adults should not speak from same worldview

Differentiate by:
- what they notice;
- vocabulary;
- confidence;
- practical knowledge;
- fear;
- humor;
- authority.

Do not use caricature dialect everywhere.

## 14. NPC utility fast-path

Flavor must not harm repeat usability.

For frequent service NPCs:
first interaction -> full characterization;
repeat -> short service path;
optional talk -> deeper lines.

## 15. Post-event reactions

For every local event, author reaction sets for:
- directly affected NPC;
- authority/service NPC;
- unrelated local;
- child/youth;
- odd/comic perspective.

This creates multiple interpretations of the same event.

## 16. Optional invisible depth

Not all authored relationships need quests.
A player may notice:
- two NPCs move closer after event;
- shop changes item;
- worker takes break;
- traveler leaves;
- sign changes;
- music source changes.

These small persistent changes are powerful memory anchors.

## 17. Unity implementation

NPCDefinition:
- identity;
- visual profile;
- behavior;
- relation tags;
- dialogue sets;
- service capability;
- save state.

NPCSchedule:
- state/time slots;
- anchors;
- override priority.

NPCSpawner/Director:
- reads WorldState;
- selects schedule;
- handles sector load/unload;
- restores persistent state.

## 18. QA metrics

Original DOLZORE metrics:
- named NPCs with at least 1 relation;
- percentage with post-event variant;
- percentage serving no quest function;
- repeated-talk variants;
- visible social clusters;
- schedule/state changes;
- duplicated visual profiles.

Use metrics as under-authoring alarms, not score targets.

## 19. Sources

Miura interview:
https://www.1101.com/n/s/mother_project/miura_akihiko/index.html

2013 revival conversation:
https://www.1101.com/mother_project/entry/archives/mother2_wiiu/

Nintendo official screenshots:
https://www.nintendo.co.jp/n08/a2uj/mother2/screen/index.html

CoilSnake map sprite placement (technical reference):
https://github.com/pk-hack/CoilSnake
