# ANIMAL CROSSING — HOUSING / CUSTOMIZATION / CONTENT PIPELINE

Authority: DeeDee-Lab/Dolzore#40

## 1. Generational expansion

Customization scope grows roughly:
room
-> multiple rooms / exterior
-> custom clothing/patterns
-> town public works
-> dedicated client-home design
-> campsite
-> outdoor island placement
-> terraforming
-> resort/facility design.

This is one of the clearest long-term series evolutions.

## 2. Data model

ItemDefinition:
- stable ID;
- category;
- dimensions;
- placement rules;
- interaction;
- variants;
- material/theme tags;
- buy/sell/craft metadata;
- indoor/outdoor;
- animation/audio;
- localization.

PlacedObject:
- item ID;
- variant/customization;
- world/room;
- position;
- rotation;
- surface/slot;
- owner;
- placed_at;
- state.

## 3. Room system

RoomDefinition:
- dimensions;
- wall/floor/ceiling;
- grid/continuous placement;
- doors/windows;
- lighting;
- fixed architecture;
- object list;
- design score tags;
- screenshot/share metadata.

## 4. Client brief design

HHD/HHP shows that furnishing becomes progression when NPC clients provide themed constraints.

Original DOLZORE client brief:
- must-have functions;
- desired tags;
- disliked tags;
- room function;
- optional bonus constraints;
- no single exact solution.

This supports creativity better than fixed puzzle answers.

## 5. Public facilities

Designing shops/schools/restaurants creates civic authorship.
DOLZORE can reuse the principle through:
- guild hall;
- inn/bar;
- workshop;
- public square;
- market;
- community house.

## 6. Content production scale

At thousands of items, manual one-off code is impossible.

Required pipeline:
spreadsheet/DB authoring
-> schema validation
-> localization keys
-> asset validation
-> collision/placement bounds
-> theme tags
-> icon generation
-> catalog index
-> runtime bundle.

## 7. Variants

Keep base item identity separate from:
- color/material;
- pattern;
- condition;
- season;
- crafted quality;
- owner customization.

This multiplies expressive breadth without multiplying core code.

## 8. UX

Essential:
- preview before placement;
- undo/redo;
- move/rotate;
- multi-select where appropriate;
- search/filter;
- recent/favorite;
- storage;
- copy room/zone templates;
- mouse/touch/controller parity.

Switch 2 Edition adding mouse controls for room/custom design/bulletin-board tasks reinforces that high-precision authoring benefits from pointer input.

## 9. DOLZORE

Adopt:
- housing as first-class path;
- public-space authorship;
- client design jobs;
- large tagged catalog.

Do not copy Nintendo furniture, art, exact scoring rules, room layouts or UI.
