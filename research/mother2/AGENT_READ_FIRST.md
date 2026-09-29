# MOTHER2 REFERENCE — AGENT READ FIRST

Authority: direct user instruction, 2026-09-29 JST
Canonical research issue: DeeDee-Lab/Dolzore#26
Consumer: DOLZORE game creation agents
Status: ACTIVE_RESEARCH_V1

## 0. Purpose

This directory is a design-research asset, not a ROM extraction archive.

Use MOTHER2 / EarthBound to learn:
- world/field composition;
- town identity and travel rhythm;
- environmental density and landmarking;
- character/sprite readability;
- encounter presentation;
- battle urgency and audiovisual feedback;
- music as place identity;
- dialogue density, humor and emotional contrast;
- mundane/surreal worldbuilding;
- memory-anchor design;
- efficient content reuse under constrained art budgets;
- how to turn those principles into an original Unity game.

Do not copy protected expression.

## 1. Evidence classes

Every important statement should be treated as one of:

- OFFICIAL_NINTENDO — Nintendo product/manual/archive material.
- CREATOR_INTERVIEW — developer/creator testimony from Hobonichi or equivalent primary interview.
- OFFICIAL_ARCHIVE — official historical project pages or anniversary material.
- COMMUNITY_REVERSE_ENGINEERING — technical facts inferred from tools such as CoilSnake or battle-background research.
- COMMUNITY_REFERENCE — fan/reference material useful for enumeration or cross-checking only.
- VISUAL_OBSERVATION — direct observation of screenshots; descriptive, not source-code proof.
- INFERENCE — synthesis across evidence; never silently upgrade to fact.
- DOLZORE_DESIGN_RECOMMENDATION — an original implementation decision inspired by the research.

## 2. Copyright / originality boundary

Never copy into DOLZORE:
- original maps or traced layouts;
- original sprite sheets, character silhouettes, costumes or pixel arrangements;
- original music, samples, melody, bass line or arrangement;
- original dialogue/scripts or reconstructed full text;
- original UI/battle background artwork;
- proprietary enemy/character/place/item names;
- decompiled code or ROM data.

Allowed research output:
- topology;
- proportions;
- density;
- transition grammar;
- state-machine ideas;
- systemic patterns;
- timing/pacing categories;
- palette/function observations;
- collision and tile architecture concepts;
- role/function taxonomies;
- original implementation recommendations.

## 3. High-confidence development facts

### 3.1 Town-first design process
CREATOR_INTERVIEW:
Game designer Akihiko Miura states that Shigesato Itoi first wrote town-by-town concepts and what happens in each town on large sheets. The team discussed how to turn those concepts into a game; Miura converted them into specifications. While the base program was delayed, the team kept refining maps, dialogue and unusual status effects.

Implication:
MOTHER2's richness was not created by decorating a generic map after mechanics were done. Place concept, events, dialogue, systems and art were co-designed.

DOLZORE rule:
For each town, write a TOWN CONCEPT CONTRACT before final map art:
1. emotional role;
2. everyday-life role;
3. anomaly/oddity;
4. mandatory story beat;
5. optional memory anchors;
6. local NPC types;
7. local food/leisure/culture;
8. audio identity;
9. visual grammar;
10. exit/transition rhythm.

### 3.2 Visible enemies and contact advantage
OFFICIAL_MANUAL + CREATOR_INTERVIEW:
MOTHER2 deliberately moved to visible enemies on the field. Contact direction changes encounter advantage/disadvantage. Very weak enemies can be resolved without opening the normal battle screen.

DOLZORE rule:
Exploration and combat are one continuous risk loop. Field positioning must matter before battle starts. Trivial battles should be collapsible/fast-resolved to avoid grinding friction.

### 3.3 Rolling HP creates time pressure inside turn-based combat
CREATOR_INTERVIEW:
Miura explicitly identifies the analog/drum-style HP display as his specification.

Design interpretation:
Damage is not merely a discrete number change; it creates a short reaction window. This adds urgency to a command-driven battle without converting it to action combat.

DOLZORE rule:
If adopting this principle, implement an original delayed-consequence health model or other time-sensitive recovery window, not the same visual drum meter.

### 3.4 Road-movie structure
CREATOR_INTERVIEW:
Miura describes insisting on many foods and destination-specific mood because scenery, food, rest and music make the journey feel like a road movie. He also describes creating a pleasurable resort-like segment immediately before harsher travel.

DOLZORE rule:
Pacing must include intentional comfort peaks before difficult arcs. Towns are not only utility hubs; they are emotional rest states.

### 3.5 Place identity through excessive specific detail
CREATOR_INTERVIEW + OFFICIAL_ARCHIVE:
The project invested in details that are inefficient by strict gameplay logic: food variety, hotels, photography, delivery, transport, shops, odd NPCs, signs, jokes and optional conversations.

DOLZORE rule:
A memorable town needs a surplus of non-essential but coherent detail. Do not optimize all content toward rewards.

### 3.6 Dialogue is state-aware and post-story life continues
CREATOR_INTERVIEW:
Scenario implementation included conditional lines for unusual states and many post-final-battle conversations. Writers intentionally prepared lines that most players may never see.

DOLZORE rule:
NPC dialogue should be a state matrix rather than a single line:
- base;
- before/after local incident;
- party/character context;
- time/weather;
- repeated-talk escalation;
- rare condition;
- endgame/post-quest reflection.

### 3.7 Art is deliberately stylized, not realistic
CREATOR_INTERVIEW:
Art director Koichi Oyama described the graphics as intentionally non-realistic in details such as road logic, plant ecology and urban realism. He also described extensive reuse of small tile materials and the difficulty of keeping a large contiguous world visually varied.

DOLZORE rule:
Prioritize symbolic readability and emotional truth over literal civil-engineering realism, but keep internal visual rules coherent.

### 3.8 The connected-world illusion is technically expensive
CREATOR_INTERVIEW + COMMUNITY_REVERSE_ENGINEERING:
MOTHER2 often keeps large regions visually contiguous instead of switching to isolated town screens. Technical research shows map sectors independently carry tileset, palette, music, settings and town-map metadata.

DOLZORE rule:
Build one logical connected world, but divide it into authored streaming/sector units with independent palette, music, encounter and event state.

### 3.9 Music is scene design
CREATOR_INTERVIEW:
The music team produced a very large quantity of material, drew from non-game musical culture, and tested music in the running game. Interviews describe mood, texture and after-feeling as primary concerns.

DOLZORE rule:
Do not assign one generic town track and one battle track. Music must be part of location/event identity and can deliberately create contrast with visuals or combat.

## 4. Canonical reading order

1. AGENT_READ_FIRST.md
2. MOTHER2_REFERENCE_SPEC_V1.json
3. WORLD_MAP_FIELD_GRAMMAR.md
4. LOCATION_EXPERIENCE_ARCHETYPE_MATRIX.md
5. VISUAL_CHARACTER_EFFECT_GRAMMAR.md
6. AUDIO_ATMOSPHERE_GRAMMAR.md
7. DIALOGUE_WORLDVIEW_GRAMMAR.md
8. GAMEPLAY_UI_BATTLE_PRESENTATION_GRAMMAR.md
9. TECHNICAL_ARCHITECTURE_FINDINGS.md
10. DOLZORE_UNITY_IMPLEMENTATION_CHECKLIST.md
11. SOURCE_INDEX.md
12. RESEARCH_STATUS.json
13. Dolzore issue #26 latest checkpoint

## 5. Translation into DOLZORE

MOTHER2 should influence DOLZORE at the level of:
- town authoring method;
- density of optional interactions;
- connected-road travel;
- readable silhouettes;
- symbolic environment art;
- emotional contrast;
- visible field threats;
- fast trivial encounter handling;
- authored audiovisual identity;
- layered dialogue state;
- mundane life mixed with strangeness.

MOTHER2 must NOT determine:
- DOLZORE map layouts;
- specific characters;
- exact proportions or palettes;
- exact battle UI;
- exact HP implementation;
- names;
- story beats;
- melodies;
- dialogue voice.

DOLZORE remains an original world.

## 6. Resume protocol

On interruption:
1. read RESEARCH_STATUS.json;
2. read issue #26 latest checkpoint;
3. do not repeat already-consumed source research;
4. add only evidence deltas;
5. upgrade INFERENCE only when new primary/technical evidence supports it.
