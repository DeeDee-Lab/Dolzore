# MOTHER2 AUDIO / ATMOSPHERE GRAMMAR

Authority: DOLZORE MOTHER2 research issue #26
Purpose: learn how sound constructs place and memory without copying music.

## 1. Core finding: music is assigned by dramatic fit, not only by category

CREATOR_INTERVIEW — Keiichi Suzuki:
Music creation included discussion of where a piece fit best. More accessible/pop material could serve primary scenes while more experimental material could be used in battles or stranger contexts.

Design rule:
Do not begin with "Town Track / Dungeon Track / Battle Track."
Begin with scene function.

DOLZORE AudioRole values:
- HOME;
- ARRIVAL;
- CIVIC_DAY;
- SOCIAL_REST;
- COMIC_ODDITY;
- UNEASE;
- WILDERNESS;
- NIGHT;
- DISCOVERY;
- BATTLE_LOW;
- BATTLE_HIGH;
- ANOMALY;
- AFTERMATH;
- MEMORY;
- ENDING_REFLECTION.

A track may satisfy multiple roles.

## 2. Music as location identity

Evidence: creator interviews describe music, scenery, food and travel as one road-movie experience.

DOLZORE rule:
Every major town/region gets an AudioIdentity:
- emotional keywords;
- tempo range;
- rhythmic density;
- harmonic tension profile;
- instrumentation family;
- ambience bed;
- transition policy;
- event variants.

Do not imitate specific MOTHER2 genres or melodies. Select original influences that fit DOLZORE's own culture.

## 3. Contrast is more important than uniform brand sound

MOTHER2's remembered atmosphere emerges from stylistic variety rather than one continuous genre.

Reusable principle:
A soundtrack can change idiom aggressively while maintaining identity through:
- recurring motifs created for DOLZORE;
- consistent sound palette families;
- shared production texture;
- recurring ambience;
- controlled transition logic.

DOLZORE should avoid making every region the same "retro JRPG" arrangement.

## 4. Rest before stress

CREATOR_INTERVIEW:
A pleasurable destination was intentionally positioned before a harsher progression segment, and its music participated in that comfort.

DOLZORE pacing:
Use audio to communicate recovery_value.

Area metadata:
- stress_level;
- recovery_value;
- music_energy;
- ambience_density;
- dissonance;
- silence_ratio.

Before a high-stress arc, deliberately consider a low-stress music/space segment.

## 5. Experimental sound belongs where the fiction can support it

Creator interviews describe adventurous/experimental material being useful for battles and unusual contexts.

DOLZORE:
Reserve the widest timbral/processing departures for:
- ZURE anomalies;
- high-threat battles;
- dream/time layers;
- corrupted spaces;
- rare encounters.

This makes strange audio meaningful rather than constant.

## 6. Hardware-constraint lesson

CREATOR_INTERVIEW:
The MOTHER music team describes enjoying the creative pressure of hardware/channel/capacity limits.

Reusable production principle:
Constraints can create identity.

Original DOLZORE audio constraint proposal:
- define a limited recurring instrument palette per cultural region;
- limit simultaneous attention-grabbing layers;
- reserve full-spectrum/high-layer arrangements for rare peaks;
- design loop points explicitly;
- maintain a lightweight WebGL/mobile mix profile.

Do not artificially reproduce SNES channel limitations unless intentionally chosen.

## 7. Loop design

Ambient game music must survive repetition.

DOLZORE loop contract:
- no obvious "song ended" cadence unless scene demands;
- intro may be separate from loop body;
- loop body should tolerate 3–10 repetitions;
- avoid melodic density that exhausts the player during exploration;
- maintain alternate intensity stems where needed;
- store sample-accurate loop metadata.

## 8. Music state transitions

Transition types:
- hard cut for shock;
- short crossfade for geographic transition;
- bar-aligned transition where stems support it;
- ambience-first bridge;
- motif carryover;
- silence gap.

AudioZoneData:
- track_id;
- ambience_id;
- entry_transition;
- exit_transition;
- priority;
- ducking;
- event_overrides[];
- time_overrides[];
- combat_overlay_policy.

## 9. Ambience

The "air" of a place cannot come from BGM alone.

Every major DOLZORE area should specify:
- far ambience;
- near loopable ambience;
- intermittent one-shots;
- human activity;
- mechanical sources;
- weather;
- wildlife;
- silence zones.

Examples of categories, not copied content:
- distant traffic;
- river;
- insects;
- station announcements;
- bar interior murmur;
- workshop machinery;
- electrical hum.

## 10. Object-bound sound

World objects should own sound where possible:
- vending machine hum;
- neon buzz;
- water;
- door;
- sign creak;
- jukebox;
- machinery.

This reinforces spatial identity and navigation.

## 11. Footsteps

Footstep sound is a world-material sensor.

SurfaceProfile:
- asphalt;
- tile;
- wood;
- dirt;
- grass;
- gravel;
- metal;
- shallow water.

Each profile:
- clip family;
- pitch/volume variation;
- step interval;
- wet/dry modifier;
- indoor/outdoor reverb send.

## 12. Dialogue sound

Use an original subtle dialogue feedback system.

Requirements:
- readable with sound off;
- speaker/category variations may exist;
- should not become constant high-frequency irritation;
- accessibility toggle;
- no imitation of MOTHER2's exact blips.

## 13. Battle audio

Battle music intensity should reflect encounter meaning, not only "battle = one song."

Possible original classification:
- trivial;
- standard;
- dangerous;
- elite;
- boss;
- anomaly;
- scripted emotional conflict.

Weak/trivial field encounters that auto-resolve should not incur a long battle-music transition.

## 14. Memory motifs

A location or relationship can gain a motif only after the player has a reason to care.

Original DOLZORE motif system:
- motif_id;
- associated_character/location/theme;
- first_exposure;
- transformed_variants;
- instrumentation_variants;
- final_recall_condition.

Do not mimic MOTHER2's melodies or interval structures.

## 15. Silence

Silence is a designed state.

Use silence or near-silence for:
- anticipation;
- post-event reflection;
- uncanny transition;
- isolating a line of dialogue;
- revealing environmental sound.

Do not fill every space with continuous music.

## 16. Audio implementation for Unity

Recommended:
AudioManager
AudioZone
MusicStateMachine
AmbienceEmitter
SurfaceFootstepSystem
SfxPool
Snapshot/Mixer profiles
AccessibilityAudioSettings

For WebGL/mobile:
- bounded simultaneous voices;
- compressed streaming for long music;
- preloaded critical UI SFX;
- user-gesture unlock handling;
- save BGM/SFX settings.

## 17. Audio acceptance gates

Fail if:
- every town sounds interchangeable;
- BGM ignores story state;
- transitions repeatedly restart tracks abruptly without intent;
- ambience exists only as global stereo loop;
- battle music triggers for instantly resolved trivial enemies;
- strange spaces rely only on louder music;
- licensed/reference music has unclear rights;
- any MOTHER2 melody/sample/arrangement is copied.

## 18. Source anchors

Primary:
- Keiichi Suzuki 2024:
  https://www.1101.com/n/s/mother_project/keiichi_suzuki2024/index.html
- MOTHER music interviews:
  https://www.1101.com/mother_project/entry/archives/MOTHER_music/
- Akihiko Miura 2026:
  https://www.1101.com/n/s/mother_project/miura_akihiko/index.html
