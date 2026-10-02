# ANIMAL CROSSING — TIME / SEASON / OFFLINE SIMULATION

Authority: DeeDee-Lab/Dolzore#40
Evidence: Nintendo official + public reverse engineering
Purpose: implementation-grade life-sim timing model.

## 1. Series invariant

The original N64 official page explicitly says:
- the cartridge village has time like the real world;
- morning/day/night pass;
- seasons change trees/background;
- while the game is not running, villagers may write letters or move.

Wild World official material repeats real-life rhythm and seasons.

This establishes that the clock is not cosmetic. It drives world state.

## 2. Required conceptual clocks

Separate these clocks in any original implementation:

### RealClock
Wall-clock / trusted elapsed time.

### WorldClock
Game-facing date/time.

### SessionClock
Time since current login.

### SimulationCatchupClock
Used to process elapsed changes since previous persistence checkpoint.

### EventClock
Scheduled event windows.

### NPCScheduleClock
Resident activity windows.

### EcologyClock
Plants, weeds, spawn cycles, regeneration.

### MarketClock
Shop rotation / turnip-like weekly economy.

Do not make one DateTime value responsible for all systems.

## 3. Offline catch-up

Recommended deterministic pipeline:

load save
-> compare last_simulated_at with now
-> clamp/validate time jump
-> process day boundaries
-> process week/month/season boundaries
-> process resident move/social state
-> process mail/deliveries
-> process shops/market
-> process ecology
-> process events
-> persist new simulation checkpoint
-> enter live world

Do not replay every minute/hour individually for long absences.
Use boundary/event-based transactions.

## 4. Time manipulation resilience

Animal Crossing historically allows console-clock changes, so systems must tolerate non-monotonic local time.

DOLZORE recommendation:
Track separately:
- player-visible local world time;
- monotonic server/account progression time where exploitation matters;
- last observed wall time;
- last authoritative progression time.

For offline/single-player content, favor forgiveness over punishment.

## 5. Calendar layers

CalendarService should expose:
- minute/hour;
- weekday;
- day-of-month;
- month;
- season;
- hemisphere;
- special event;
- event preparation state;
- holiday override;
- shop/service schedule;
- sun phase;
- audio state.

New Horizons public save research explicitly exposes Hemisphere in MainSave, demonstrating hemisphere is persisted game state in that generation.

## 6. Granularity

Not all systems need per-frame or per-minute updates.

Suggested cadence:
- lighting/audio: minute/hour boundary;
- NPC schedule: activity window or event-driven;
- store inventory: daily;
- ecology: day boundary + local interactions;
- seasonal content: calendar boundary;
- long-term relationship/move state: daily/interaction driven;
- weekly market: half-day/daily windows;
- festivals: scheduled event windows.

## 7. Player lifestyle adaptation

New Leaf developer interview describes ordinances as a way to preserve real-time meaning while allowing players with different schedules to participate.

Reusable principle:
Do not solve schedule friction by removing the clock.
Offer world-policy modifiers:
- early-town schedule;
- late-town schedule;
- market/service shift;
- resident wake/sleep shift.

DOLZORE should make these explicit civic/world settings rather than hidden cheats.

## 8. Audio coupling

Time state should drive:
- hourly/period music;
- morning/evening ambience;
- insect/bird intensity;
- shop/interior audio;
- festival overrides;
- weather layers.

AudioState must subscribe to TimeService rather than independently polling OS time.

## 9. Save contract

Persist:
- world_datetime;
- last_simulated_at;
- last_daily_reset;
- last_weekly_reset;
- season/hemisphere;
- active event;
- event phase;
- market week state;
- ecology checkpoint;
- NPC schedule state only where necessary.

Derive transient state where possible.

## 10. QA

Test:
- same-day reconnect;
- midnight crossing;
- week crossing;
- month crossing;
- season crossing;
- DST/timezone shift;
- clock moved backward;
- clock moved forward 1 day / 30 days / 1 year;
- leap day;
- hemisphere seasonal inversion;
- event active during absence;
- resident move pending during absence.

## Source anchors

Official:
- https://www.nintendo.co.jp/n01/n64/software/nus_p_nafj/what/index.html
- https://www.nintendo.co.jp/ds/admj/what/index.html
- https://www.nintendo.co.jp/3ds/interview/egdj/vol1/index2.html

Reverse engineering:
- zeldaret/af @ 4ddba04604ee7b4c4cfc0b64f8ee4d094bb385be
- kwsch/NHSE @ cb0745415945776f73375bf0a434a8babf059307
