# MOTHER2 DIALOGUE / WORLDVIEW / "AIR" GRAMMAR

Authority: DOLZORE MOTHER2 research issue #26
Purpose: extract conversational/worldbuilding mechanics without imitating Shigesato Itoi's prose.

## 1. Core warning: "MOTHER-like writing" is NOT a pile of weird jokes

CREATOR_INTERVIEW / Itoi discussions:
Attempts to imitate the writing can become visibly jokey or self-conscious. The series' tone contains silliness, sincerity, embarrassment, fear, ordinary life and emotional weight together.

DOLZORE rule:
Never give a writer the instruction "write like Itoi."
Instead define:
- speaker;
- social position;
- immediate need;
- what the speaker notices;
- what they avoid saying;
- emotional temperature;
- comic distance;
- world-state knowledge;
- optional subtext.

The resulting voice must be original to DOLZORE.

## 2. The "air" is a ratio, not a genre

MOTHER2's atmosphere is produced by coexistence of:
- normal modern life;
- childish vulnerability;
- adult absurdity;
- mundane services;
- threat;
- embarrassment;
- bodily inconvenience;
- homesickness/loneliness;
- warm community;
- surreal interruption;
- high-stakes cosmic danger.

A useful DOLZORE model is not "comedy + RPG."
It is an authored mixture of emotional channels.

For every region, maintain an AtmosphereProfile:
- normality: 0..5;
- humor: 0..5;
- discomfort: 0..5;
- tenderness: 0..5;
- mystery: 0..5;
- danger: 0..5;
- loneliness: 0..5;
- social_warmth: 0..5;
- surrealism: 0..5;
- nostalgia: 0..5.

Do not keep all channels high.

## 3. Ordinary systems make surreal systems believable

MOTHER2 places extraordinary events inside a world that still contains:
- homes;
- phones;
- hotels;
- shops;
- food;
- transport;
- money;
- mundane adults;
- local services;
- social awkwardness.

Reusable principle:
Before asking the player to accept supernatural/anomalous events, establish a mundane baseline.

DOLZORE worldwriting:
For each town, first define:
- what people eat;
- how they travel;
- where they work;
- how they relax;
- where children/young people gather;
- what residents complain about;
- what local business exists;
- what the town is proud of;
- what residents consider "normal."

Only then define the local anomaly.

## 4. NPC dialogue is stateful content, not decoration

CREATOR_INTERVIEW / Miura:
Scenario work included conditional dialogue for unusual states and extensive post-event/endgame conversations. Some lines could be seen only in rare conditions.

DOLZORE DialogueVariant dimensions:
- story_state;
- local_event_state;
- time_of_day;
- weather;
- party_composition;
- player_status;
- first/repeat interaction;
- quest_state;
- relationship_state;
- rare_flag;
- postgame_state.

NPCDialogueSet:
- greeting;
- base_lines[];
- repeat_chain[];
- event_variants{};
- rare_variant;
- farewell;
- ambient_bark;
- post_event_reflection.

## 5. Repeated talk can escalate

CREATOR_INTERVIEW / development retrospective:
Some tiny scenes reward repeated interaction; a small joke or repeated exchange can turn into warmth or a different emotional register.

DOLZORE rule:
Do not make repeated interaction only return "same line."

RepeatInteractionPolicy:
- repeat 1: normal;
- repeat 2: acknowledgment of repetition;
- repeat 3+: optional change;
- rare final: secret/character reveal/mini-event.

Not every NPC needs this. Use it selectively to teach players that curiosity can be rewarded.

## 6. Dialogue density matters

Official script publication shows MOTHER2's text volume is substantial; this supports the observation that conversational richness is part of the product, not filler.

Do not copy or reconstruct the original script.

DOLZORE production metric:
Track:
- unique_dialogue_nodes;
- stateful_variants;
- optional_lines;
- post_event_lines;
- repeat_interaction_lines;
- NPCs_with_personal_detail;
- lines_per_district.

Quality is not raw count, but count reveals under-authored districts.

## 7. Every NPC needs a local point of view

NPCs should not exist only to:
- give quest;
- explain controls;
- deliver lore.

Each NPC should have at least one:
- occupation;
- worry;
- habit;
- preference;
- social relation;
- local opinion;
- misunderstanding;
- physical task;
- rumor source.

This creates a town that feels inhabited rather than populated by UI terminals.

## 8. Information delivery rule

Separate dialogue functions:
- ORIENT: where/what;
- WORLD: everyday knowledge;
- CHARACTER: reveals speaker;
- QUEST: actionable;
- ATMOSPHERE: mood;
- HUMOR: release/oddity;
- FORESHADOW: future;
- REFLECT: reacts to past;
- SECRET: optional reward.

A single line can have multiple functions.

Do not make all mandatory information dependent on one joke or obscure optional interaction.

## 9. Humor taxonomy for original writing

Use categories rather than imitation.

### Misalignment
Speaker treats important thing as mundane, or mundane thing as important.

### Over-specificity
Unexpectedly precise practical concern.

### Social friction
Politeness, awkwardness, bragging, impatience, insecurity.

### Consequence inversion
Player expects reward/praise; gets an ordinary or mildly inconvenient response.

### Object perspective
An item/sign/environment has an unexpected but internally consistent framing.

### Repetition transformation
Same setup changes after repeated talk or world-state shift.

### Bureaucratic absurdity
Systems/procedures meet extraordinary situations.

### Earnest oddity
Character sincerely believes/does something unusual.

Rule:
Humor must preserve character truth. Never add nonsense only because "MOTHER is weird."

## 10. Vulnerability

Creator discussions emphasize that protagonists can be mocked, uncomfortable, homesick, ill or otherwise imperfect.

Reusable principle:
A hero becomes human through non-heroic states.

DOLZORE may use original vulnerability systems:
- fatigue;
- homesickness/longing analogue with original name/mechanics;
- embarrassment/social stress;
- injury consequences;
- fear;
- isolation;
- weather discomfort.

Do not reproduce MOTHER2's exact statuses.

## 11. Emotional contrast

Do not isolate "serious story" from "funny NPCs."

A stronger pattern:
ordinary -> funny -> uncomfortable -> useful -> warm -> strange.

The player's emotional state can turn within one district.

DOLZORE scene review:
For every 30–60 minute content unit, list emotional beats. Reject units that are a flat single tone unless intentionally designed as such.

## 12. Memory anchors

Iwata/Itoi retrospective emphasizes that different players remember different fragments: music, words, scenes, unpleasant sounds, silliness, emotional moments.

DOLZORE MemoryAnchor asset:
- id;
- type: dialogue / visual / sound / interaction / place / relationship;
- mandatory_or_optional;
- setup;
- payoff;
- revisit_variant;
- sensory_channels[];
- no_reward_required;
- screenshotable_moment;
- later_callback_id.

A game world should contain more anchors than any single player will consume.

## 13. Environmental writing

Not all worldwriting should be spoken.

Channels:
- signs;
- object inspection;
- product names;
- architecture;
- shop inventory;
- sounds;
- NPC placement;
- empty space;
- repeated props;
- local food;
- transit;
- posters/notices.

DOLZORE rule:
Lore should emerge from life patterns, not only codex exposition.

## 14. Signs and object interaction

Signs are cheap high-density characterization tools.

ObjectInteraction:
- useful;
- mundane;
- misleading;
- funny;
- emotional;
- state-changing;
- secret.

Avoid making every object produce a joke. Variety is required.

## 15. Local food / consumption

CREATOR_INTERVIEW / Miura:
Food variety was deliberately tied to travel experience.

DOLZORE use:
Each major region should have:
- common food;
- local specialty;
- cheap snack;
- comfort meal;
- odd local item;
- one food tied to an NPC or memory anchor.

Food can communicate culture even when mechanically simple.

## 16. Player naming and personal data as delayed emotional tools

CREATOR_INTERVIEW describes delayed use of player-provided naming information for an emotional payoff.

Reusable principle:
Information entered much earlier can be recalled later, when earned.

DOLZORE:
If player provides names/preferences:
- use sparingly;
- establish consent/context;
- do not overuse;
- reserve one or two strong callbacks.

Never copy the exact MOTHER2 structure/payoff.

## 17. Post-event life

NPCs should react after the crisis.

Required for major DOLZORE incidents:
- at least 3 local reaction clusters;
- one practical reaction;
- one emotional reaction;
- one changed routine;
- one optional follow-up;
- persistent environmental change if appropriate.

This makes the town feel like a place, not a stage reset.

## 18. Endgame/postgame conversation principle

If the player saves the world, residents should not remain frozen at pre-ending state.

DOLZORE plan:
Build a PostEvent/PostGame dialogue layer from the start, even if first slice has only one local incident.

## 19. Localization policy

CREATOR_INTERVIEW:
Localization aimed to recreate intended effect, not mechanically translate every phrase.

DOLZORE localization contract:
Store:
- semantic_intent;
- emotional_intent;
- joke_mechanism;
- required_gameplay_fact;
- cultural_dependency;
- max_width/context.

Localizer may rewrite wording while preserving these contracts.

## 20. Dialogue authoring data model

DialogueNode:
- node_id;
- speaker_id;
- text_key;
- semantic_functions[];
- emotion;
- conditions[];
- priority;
- choices[];
- effects[];
- next;
- voice_sfx_profile;
- localization_notes;
- analytics_tag.

DialogueSet:
- owner_id;
- variants[];
- repeat_policy;
- fallback_node;
- last_changed_version.

## 21. Style constraints for DOLZORE

Original DOLZORE voice should be:
- concise;
- concrete;
- observant;
- emotionally honest;
- capable of oddity without telegraphing "this is a joke";
- locally grounded;
- different by speaker age/job/personality.

Avoid:
- fake-poetic slogans everywhere;
- nonstop quirky one-liners;
- self-aware retro-game jokes;
- copied cadence from MOTHER;
- exposition dumps;
- every NPC praising the player;
- every NPC being hostile;
- identical speech length.

## 22. Worldview formulation

MOTHER2's reusable worldview principle:
The extraordinary does not replace ordinary life. It intrudes into it.

DOLZORE original worldview:
Define normal life first, then let ZURE distort:
- schedule;
- memory;
- relationships;
- space;
- sound;
- objects;
- economics;
- weather/time;
- perception.

The anomaly becomes stronger because the baseline is tangible.

## 23. Content QA

For each district:
- at least 3 dialogue functions represented;
- at least 1 NPC with non-quest life detail;
- at least 1 state-changing line after an event;
- at least 1 optional object interaction;
- not all humor;
- not all lore;
- no copied MOTHER wording/cadence;
- mandatory information has reliable fallback;
- localization intent notes exist for difficult lines.

## 24. Source anchors

Primary / creator:
- Miura interview:
  https://www.1101.com/n/s/mother_project/miura_akihiko/index.html
- MOTHER2 revival conversation:
  https://www.1101.com/mother_project/entry/archives/mother2_wiiu/
- Itoi MOTHER interviews:
  https://www.1101.com/mother_project/entry/archives/MOTHER/
- MOTHER2 development retrospective:
  https://www.1101.com/n/s/mother_project/mother2_himitsu_book/
- Official script-book metadata (used only as evidence of text volume, not copied):
  https://www.1101.com/mother_project/items/book_motherscripts.html
