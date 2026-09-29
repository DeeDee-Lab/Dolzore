# MOTHER2 MUSIC ROLE MATRIX

Authority: DOLZORE MOTHER2 research issue #26
Evidence: OFFICIAL_NINTENDO soundtrack listing + CREATOR_INTERVIEW
Purpose: identify functional music roles without copying compositions, melodies, instrumentation or arrangement.

## 0. Core finding

Official soundtrack categorization and creator interviews show that music identity is attached not only to generic "town/battle/dungeon" buckets, but to:
- individual towns/regions;
- travel modes;
- services such as hospital/hotel;
- performance/venue culture;
- special regions;
- recurring macro-memory material;
- ending/reflection.

This supports a fine-grained AudioRole system.

## 1. Functional role classes

### Town identity
Purpose:
- immediate location recognition;
- social tone;
- repeat-visit memory.

DOLZORE:
Each major town may have its own theme or variant family, but avoid overproduction before town identity is visually/gameplay proven.

### Transit identity
Reference soundtrack includes bicycle-specific music.

Design principle:
Travel mode can change mood enough to justify musical state.

DOLZORE:
- walking;
- bicycle/vehicle;
- train/bus;
- boat;
- special fast travel;
can have overlays or alternate states.

### Service identity
Reference listing includes hospital/hotel-specific music.

Design principle:
Repeated utility spaces benefit from strong audio identity because players revisit them often.

DOLZORE:
Potential service music states:
- clinic;
- inn/lodging;
- bar;
- workshop;
- market;
- transport terminal.

Do not create a full song for every room; use motif/ambience variants where efficient.

### Venue / performance identity
Reference listing includes a blues/live-performance context.

DOLZORE:
Music venues should contain diegetic music and social behavior.
BAR/JUKEBOX already fits this.

### Climate/region identity
Reference listing distinguishes cold region, resort, distant culture, etc.

DOLZORE:
Region music should encode:
- climate;
- culture;
- travel fatigue;
- recovery value;
- anomaly level.

### Battle / experimental identity
Creator interviews describe using more experimental or "mania" material for battles/strange contexts.

DOLZORE:
Reserve aggressive processing/timbral novelty for high-salience states rather than saturating all exploration.

### Memory / macro-objective identity
Reference uses recurring melody material as a major adventure/memory structure.

DOLZORE:
Use original recurring motifs tied to:
- character;
- place;
- ZURE phenomenon;
- relationship;
- episode callback.

Do not copy the number/eight-melody structure.

### Ending / reflection
Official soundtrack includes late reflective/ending material.

DOLZORE:
Ending music should transform previously established original motifs where earned.

## 2. Audio state hierarchy

MusicState priority example:
1. critical scripted scene;
2. battle;
3. anomaly;
4. vehicle/travel;
5. interior/service;
6. district/town;
7. region default.

Use priority + crossfade rules, not random AudioSource swaps.

## 3. Motif reuse

Motif reuse can connect:
- home -> memory;
- town -> ending;
- character -> reunion;
- anomaly -> resolved state.

Variation dimensions:
- tempo;
- instrumentation;
- register;
- harmony;
- rhythm;
- texture;
- density.

All motifs must be original.

## 4. Diegetic / non-diegetic split

DOLZORE should distinguish:
- score heard by player only;
- in-world radio/jukebox/live performance;
- environment emitters.

BAR/JUKEBOX:
- diegetic preview/purchase system;
- must not become mandatory for progression;
- volume/spatialization should change with player distance if in-world.

## 5. Transitional micro-cues

Not every transition needs a whole new track.

Use:
- arrival sting;
- service jingle;
- discovery cue;
- danger warning;
- memory cue;
- victory/result cue;
- state-change sting.

Keep these short and reusable.

## 6. Music and pacing

Tag every track/state:
- energy 0..5;
- warmth 0..5;
- tension 0..5;
- strangeness 0..5;
- density 0..5;
- nostalgia 0..5;
- loop_fatigue_risk 0..5.

World planner can compare adjacent zones and reject flat audio pacing.

## 7. Original first-town recommendation

For DOLZORE first Unity town, do NOT commission dozens of full tracks yet.

Minimum viable authored sound set:
- town-day core theme;
- quiet-edge/river ambience or music-light state;
- BAR interior/jukebox diegetic state;
- JOURNAL interior state;
- anomaly/event override;
- evening/night variant or filtered arrangement;
- 6–10 core SFX families;
- footsteps by surface;
- interaction/door/UI cues.

This is an original scope recommendation.

## 8. Acceptance gates

Fail if:
- all districts reuse one full-volume loop;
- entering a service restarts town music from zero every time;
- vehicle state has no sonic feedback;
- important anomaly has only visual change;
- music masks dialogue/UI SFX;
- loop point is audible;
- reference melodies/samples are present;
- "retro" instrumentation is used as substitute for composition quality.

## 9. Sources

Nintendo MOTHER 1+2 soundtrack listing:
https://www.nintendo.co.jp/n08/a2uj/sound/index.html

Keiichi Suzuki 2024:
https://www.1101.com/n/s/mother_project/keiichi_suzuki2024/index.html

MOTHER music interviews:
https://www.1101.com/mother_project/entry/archives/MOTHER_music/

Akihiko Miura 2026:
https://www.1101.com/n/s/mother_project/miura_akihiko/index.html
