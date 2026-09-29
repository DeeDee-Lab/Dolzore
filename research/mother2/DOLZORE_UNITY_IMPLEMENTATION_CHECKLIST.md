# DOLZORE UNITY IMPLEMENTATION CHECKLIST DERIVED FROM MOTHER2 RESEARCH

Authority: DOLZORE MOTHER2 research issue #26
Status: actionable first-town checklist
Important: these are DOLZORE original implementation requirements, not MOTHER2 specifications.

## A. Before opening Tile Palette

[ ] Write TownConcept.
[ ] Define emotional role.
[ ] Define everyday function.
[ ] Define local anomaly.
[ ] Define normal resident routines.
[ ] Define food/leisure/culture.
[ ] Define audio identity.
[ ] Define arrival/exit experience.
[ ] Define 3+ optional memory anchors.
[ ] Define post-event state.

## B. Blockout

[ ] Draw district graph first.
[ ] Place 3–6 major landmarks.
[ ] Establish primary loop.
[ ] Establish optional spur.
[ ] Add at least one shortcut/alternate return where scale permits.
[ ] Mark quiet edge.
[ ] Mark social pocket.
[ ] Mark danger/encounter boundary.
[ ] Mark mandatory route.
[ ] Validate mandatory route automatically.

## C. Unity layers

[ ] Visual Ground Tilemap.
[ ] Visual Structure Tilemap.
[ ] Collision Tilemap.
[ ] Navigation/Cost layer.
[ ] Interaction volumes.
[ ] Door/Transition volumes.
[ ] NPC placement layer.
[ ] Encounter zones.
[ ] Audio zones.
[ ] State-change overlays.

## D. World scale

[ ] Standard character reference.
[ ] Standard door.
[ ] Standard road lane.
[ ] Standard sidewalk.
[ ] Standard bench.
[ ] Standard car/vehicle.
[ ] Standard tree.
[ ] Character/world comparison screenshot.

## E. Characters

For each major NPC:
[ ] role;
[ ] silhouette thumbnails;
[ ] grayscale silhouette pass;
[ ] color block pass;
[ ] signature accessory/shape;
[ ] movement profile;
[ ] collider footprint separate from sprite;
[ ] interaction anchor;
[ ] base dialogue;
[ ] state-change dialogue;
[ ] rare/repeat interaction.

## F. Town life

[ ] worker-type NPC;
[ ] child/youth-type NPC;
[ ] older/established resident;
[ ] visitor/tourist/wanderer;
[ ] service worker;
[ ] practical resident;
[ ] odd resident whose behavior is locally coherent;
[ ] at least one NPC with no quest purpose.

## G. Props/interactions

[ ] orientation prop;
[ ] daily-life prop;
[ ] route-shaping prop;
[ ] optional inspection;
[ ] environmental humor/oddity;
[ ] audio-emitting prop;
[ ] no meaningless prop clutter.

## H. Audio

[ ] town AudioIdentity;
[ ] arrival transition;
[ ] normal day state;
[ ] quiet-edge ambience;
[ ] social-pocket ambience;
[ ] interior ambience;
[ ] anomaly state;
[ ] night/event variant plan;
[ ] object-bound emitters;
[ ] material footsteps;
[ ] intentional silence location;
[ ] WebGL/mobile voice budget.

## I. Dialogue

[ ] speaker-local viewpoint;
[ ] base line;
[ ] repeated-talk policy;
[ ] post-event variant;
[ ] optional world line;
[ ] practical/orientation fallback;
[ ] no nonstop joke writing;
[ ] no imitation of MOTHER/Itoi cadence;
[ ] localization intent metadata.

## J. Encounters

[ ] visible enemies only where appropriate;
[ ] safe margin near doors/critical NPCs;
[ ] readable pursuit;
[ ] route choice around threats;
[ ] contact context preserved;
[ ] trivial-resolve prototype later;
[ ] no battle for every decorative creature.

## K. UI/map

[ ] player status hierarchy;
[ ] context interaction prompt;
[ ] minimap;
[ ] town map;
[ ] player marker;
[ ] landmark discovery;
[ ] district label;
[ ] no copied MOTHER2 window/border/layout.

## L. World state

[ ] base state;
[ ] incident state;
[ ] after-incident state;
[ ] night/event possibility;
[ ] NPC schedule changes;
[ ] audio changes;
[ ] optional visual patch;
[ ] save persistence by stable ID.

## M. Memory anchors

At least three before town acceptance:
[ ] dialogue anchor;
[ ] environmental/visual anchor;
[ ] sound/interaction anchor.

Each anchor:
[ ] can be optional;
[ ] does not need loot;
[ ] has setup;
[ ] has distinctive sensory channel;
[ ] has revisit/callback potential.

## N. Quality review

Reject town if:
[ ] routes are blocked/trapped;
[ ] districts cannot be identified without labels;
[ ] NPCs look interchangeable;
[ ] town has one emotional tone;
[ ] all interactions are rewards/quests;
[ ] no ordinary life is visible;
[ ] oddity feels random rather than local;
[ ] audio is generic/one-loop only;
[ ] collision is derived from visible art;
[ ] map lacks state response;
[ ] protected MOTHER2 expression is recognizable.

## O. Production order

1. TownConcept.
2. District graph.
3. Blockout/collision.
4. Player controller/camera.
5. Landmark architecture.
6. Character scale bible.
7. NPC placements.
8. HUD/minimap.
9. Interaction/dialogue.
10. Audio zones.
11. Optional memory anchors.
12. State-change pass.
13. Encounter zones.
14. visual polish.
15. desktop/mobile/WebGL QA.
16. user visual review.

Do not start full combat/MMO/second-town production before the first-town foundation is accepted.
