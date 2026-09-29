# MOTHER2 GAMEPLAY / UI / BATTLE PRESENTATION GRAMMAR

Authority: DOLZORE MOTHER2 research issue #26
Purpose: extract presentation and interaction structure without copying MOTHER2 UI, commands, graphics or formulas.

## 1. Field and battle are visually distinct states

OFFICIAL_MANUAL:
- field screen presents world navigation plus command/status information;
- touching a visible field monster starts battle;
- contact direction can alter advantage;
- weak enemies may be resolved without entering the normal battle screen.

Design consequence:
Battle transition must preserve context from field contact.

DOLZORE BattleContext should carry:
- initiating enemy;
- player/enemy facing;
- contact advantage;
- world sector;
- encounter zone;
- time/weather;
- story flags;
- trivial-resolution eligibility;
- escape context.

## 2. Context-sensitive field interaction

OFFICIAL_MANUAL:
A context action can execute talk/check depending on situation.

Reusable principle:
Reduce interaction friction by mapping common world verbs to one context-aware interaction input.

DOLZORE InteractionResolver priority example:
1. critical scripted interaction;
2. NPC talk;
3. door/transition;
4. object inspect/use;
5. environmental affordance;
6. fallback/no-op prompt.

UI must show expected action before activation when ambiguity matters.

## 3. Separate field command layer from direct interaction

MOTHER2 supports both direct contextual interaction and a broader command/menu layer.

Reusable principle:
Do not require opening a menu for every conversation/object interaction, but preserve a menu for:
- inventory;
- equipment;
- character status;
- abilities;
- map;
- settings;
- quests/notebook.

DOLZORE should use an original menu hierarchy and visual language.

## 4. Turn-based battle with temporal pressure

CREATOR_INTERVIEW:
The rolling/drum HP system can keep decreasing after damage, allowing fast recovery to intervene before the final value is reached.

Design lesson:
A command RPG can contain real-time consequence windows without becoming reflex-action combat.

Original DOLZORE options:
- delayed_damage_queue;
- recoverable_wound_buffer;
- "stability" countdown;
- rhythm/resource stabilization;
- party rescue interrupt.

Do NOT reproduce the drum numerals or exact timing.

## 5. Player-facing health state

Information hierarchy:
1. can this character act?
2. current survivability trend;
3. resource availability;
4. status conditions;
5. queued/delayed consequence;
6. command readiness.

If using delayed damage, UI must distinguish:
- displayed current HP;
- pending loss;
- stabilized amount;
- critical threshold.

Accessibility:
- no essential information conveyed only through animation speed;
- optional reduced-motion mode;
- high-contrast critical state;
- audio cue optional, not required.

## 6. Command roles

OFFICIAL_MANUAL shows a small set of common battle command roles:
- basic attack;
- special/resource ability;
- item;
- defend;
- auto;
- escape;
and notes character command sets can differ.

Reusable principle:
Keep common commands legible while allowing character/job identity through special command slots.

DOLZORE command taxonomy:
- ATTACK;
- SKILL;
- MAGIC/ABILITY;
- ITEM;
- DEFEND;
- TACTIC;
- ESCAPE;
- CONTEXT_SPECIAL.

Names and mechanics must remain original.

## 7. Character-specific commands

A party member should not feel different only because stats differ.

Character/Job command identity can come from:
- unique resource;
- unique targeting;
- item interaction;
- stance;
- preparation;
- timing;
- support/control;
- field-battle crossover ability.

This aligns with DOLZORE's existing deep-job direction and is not a requirement to copy MOTHER2 character mechanics.

## 8. Auto battle

OFFICIAL_MANUAL includes an automatic battle option.

Reusable principle:
Provide a way to reduce repetitive low-stakes command entry, but never make optimal play opaque.

DOLZORE options:
- auto for trivial encounters;
- user-defined tactics;
- repeat last action;
- party AI profiles.

If added, UI must clearly show active automation and allow immediate cancel.

## 9. Trivial encounter resolution

Primary evidence:
- official manual: weak enemies can finish instantly without the battle screen;
- Miura interview: internal battle calculation still determines whether instant resolution applies.

DOLZORE FastResolve contract:
- never skip boss/script encounters;
- never skip if meaningful status/resource risk exists;
- calculate rewards/loot consistently;
- show compact result feedback;
- preserve quest/kill triggers;
- allow player setting to disable if desired.

## 10. Encounter advantage

Field contact direction affects battle advantage in the reference.

DOLZORE original implementation choices:
- rear contact -> initiative bonus;
- surprised contact -> delayed first command;
- terrain approach -> tactical modifier;
- stealth preparation -> positioning bonus.

Do not copy exact MOTHER2 color transition or advantage formulas.

## 11. Transition effect

Battle transitions serve four functions:
- acknowledge contact;
- communicate advantage/disadvantage;
- hide scene state change/loading;
- emotionally switch player from navigation to command focus.

DOLZORE transition should encode context visibly but use original:
- motion;
- color;
- geometry;
- sound.

Accessibility:
provide reduced-motion substitute.

## 12. Battle composition

The reference often prioritizes:
- enemy readable near center;
- battle background as expressive motion field;
- command information in stable UI zones;
- party vital state continuously visible.

Reusable rule:
Separate expressive combat art from stable information surfaces.

DOLZORE:
- center/arena = expressive;
- status/commands = stable;
- target state = explicit;
- effects = bounded so they never erase crucial UI.

## 13. Battle backgrounds

COMMUNITY_REVERSE_ENGINEERING:
Battle backgrounds can be generated from tile/palette layers plus scrolling/distortion parameters.

DOLZORE original BattleBackdropDefinition:
- biome family;
- threat family;
- layer material;
- procedural motion;
- palette state;
- intensity;
- boss override;
- reduced-motion override.

The background should express encounter mood, not duplicate literal field scenery.

## 14. Effect hierarchy

Effects should answer:
- what happened?
- to whom?
- with what strength/type?
- what persistent state changed?

Effect phases:
1. anticipation;
2. impact;
3. result;
4. persistent residue/status.

Small ordinary actions may skip anticipation to keep pacing fast.

## 15. Combat text

Avoid log spam.

Use separate layers:
- immediate numeric/state feedback;
- concise action result;
- expandable detailed log if needed.

For DOLZORE hardcore systems, an inspectable battle log may be valuable without turning every action into multi-line narration.

## 16. Status presentation

Reference includes unusual status concepts; creator interviews show status design extended beyond generic poison/sleep.

Reusable principle:
Status effects can express worldview and human vulnerability.

DOLZORE StatusDefinition:
- mechanical category;
- fiction label;
- field effect;
- battle effect;
- duration;
- cure routes;
- UI icon;
- animation;
- NPC/dialogue hooks;
- audio hooks.

Do not copy reference status names/mechanics.

## 17. Menu latency and interaction rhythm

Retro command interfaces succeed when:
- open is immediate;
- hierarchy is shallow;
- cursor state predictable;
- confirmation clear;
- frequent actions require few inputs.

Unity UI acceptance:
- no blocking animation before menu usability;
- controller/keyboard/touch navigation parity;
- last selection memory where safe;
- cancel consistently returns one level;
- critical commands require appropriate confirmation only.

## 18. UI visual originality

Reference learning:
High-contrast windows make dense information legible over expressive worlds.

DOLZORE MUST NOT copy:
- exact black-window treatment;
- border designs;
- type layout;
- command positions;
- HP drum appearance;
- battle background palette/pattern.

Build an original UI system based on:
- stable hierarchy;
- readable contrast;
- minimal obstruction;
- scalable typography;
- accessibility.

## 19. Field HUD

Existing DOLZORE authority requires a proper HUD.

Recommended:
Always/contextual:
- player;
- Level;
- HEART/HP;
- FOCUS/resource;
- job;
- zone/district;
- interaction prompt;
- BGM/audio state only if useful.

Do not display every RPG stat constantly.

## 20. Battle pacing metrics for DOLZORE

Original targets to measure, not MOTHER2 facts:
- command-open latency;
- average decision time;
- average animation lock;
- trivial encounter duration;
- standard encounter duration;
- boss turn cycle;
- time from lethal hit to rescue-window end if delayed-damage system exists.

Record telemetry during prototypes.

## 21. Source anchors

Official:
- Nintendo Wii U electronic manual:
  https://www.nintendo.co.jp/data/software/manual/man_jbbj.pdf

Creator:
- Akihiko Miura:
  https://www.1101.com/n/s/mother_project/miura_akihiko/2026-08-28.html

Technical:
- CoilSnake BattleBgModule / SwirlModule:
  https://github.com/pk-hack/CoilSnake
