# ANIMAL CROSSING — AUDIO / ENVIRONMENT GRAMMAR

Authority: DeeDee-Lab/Dolzore#40

## 1. Why audio matters

Animal Crossing ties place to time more strongly than most life sims.
A useful study axis is:
world time
-> music state
-> ambience
-> wildlife
-> weather
-> interior audio.

The exact Nintendo compositions are protected and must not be copied.

## 2. Hourly/period identity

DOLZORE AudioState should be driven by:
- hour/daypart;
- weather;
- season;
- district;
- event;
- indoor/outdoor;
- crowd/social state.

Rather than one endless town loop, use multiple related states.

## 3. Layered ambience

Ambience buses:
- wind;
- water;
- vegetation;
- insects;
- birds;
- rain/snow;
- town activity;
- interior room tone;
- mechanical props.

One-shot emitters:
- doors;
- sign;
- river;
- shop bell;
- furniture;
- tool action;
- resident reaction.

## 4. Character voice language

Animal Crossing's stylized vocalization demonstrates a principle:
dialogue can have audio identity without full voiced language.

DOLZORE should invent its own system:
- per-speaker timbre;
- syllable/phoneme blips;
- prosody based on punctuation/emotion;
- accessibility volume toggle;
- no imitation of Nintendo's exact synthesis/timbres.

## 5. Weather transitions

Audio should transition before/with visual weather:
clear
-> wind
-> drizzle
-> rain
-> storm
and back.

Use crossfades/state machine rather than abrupt clip replacement.

## 6. Interior identity

Shops, museum-like spaces, homes, public facilities and venues require distinct sound profiles.

Interior audio communicates:
- safety;
- commerce;
- culture;
- quiet;
- density;
- event state.

## 7. Silence

Quiet nighttime/outskirts states are important.
Do not fill every space with full-frequency music.

## 8. Implementation

AudioDirector
- receives TimeService;
- WeatherService;
- WorldState;
- Region/District;
- InteriorState;
- EventState.

Outputs:
- score stem/state;
- ambience snapshot;
- emitter population;
- reverb;
- wildlife activity;
- UI/voice mix.

## 9. QA

Test all combinations:
hour × weather × district × interior × event.
Prevent transition thrashing and repeated restarts.

## 10. Anti-copy

Do not reproduce:
- hourly melodies;
- K.K.-style songs;
- resident voice samples;
- UI jingles;
- event music.
Use only the state-driven audio architecture principle.
