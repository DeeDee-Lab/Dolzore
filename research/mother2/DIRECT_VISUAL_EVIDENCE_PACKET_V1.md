# MOTHER2 DIRECT VISUAL EVIDENCE PACKET V1

Authority: direct user correction, 2026-09-30 JST
Evidence class: VISUAL_OBSERVATION + USER_SUPPLIED_REFERENCE + DERIVED_MEASUREMENT
Canonical issue: DeeDee-Lab/Dolzore#26

## Why this packet exists

The previous research corpus preserved many valid high-level principles but compressed the visual evidence too aggressively.

That caused a production failure:
an Agent could read phrases such as "small readable characters", "ordinary town", "broad color masses", and "roads as navigation surfaces" yet still build a FirstTown that was visibly far from the user's intended reference.

This packet preserves the concrete evidence needed to prevent that loss.

## Reference set

### Reference A — user-supplied MOTHER2 city screenshot

User supplied in ChatGPT on 2026-09-30.

Visible source in screenshot:
`https://x.com/mother2_linebot/status/1563335627265155074`

Scene description:
- dense urban/commercial street;
- multiple adjoining/tightly spaced building masses;
- several buildings read as two- or three-story symbolic facades;
- pale high-value street/sidewalk surfaces;
- vivid green verge/vegetation;
- many NPCs and vehicles distributed through the viewport;
- frequent facade windows/signage/roof edges;
- road does not visually dominate half the viewport;
- character sprites are compact relative to building height;
- road/building/green areas interleave instead of large empty rectangular zones;
- several diagonal/sloped edge cues are present despite the symbolic tile style.

Copyright rule:
Do not store/copy the source image body, map geometry, sprite pixels, exact palette or building arrangement.
Retain source identifier + derived measurements/observations only.

### Current DOLZORE comparison image

Current FirstTown screenshot reviewed by the user:
`game/unity/Dolzore/BuildArtifacts/first-town.png`

Green source lineage at time of comparison:
`rebuild/unity-first-town-20260929`
generated source SHA:
`fcb9e81348084c6f60268dfb4a23e824287ea488`

User assessment:
- "全く違う"
- cityscape differs;
- characters differ;
- previous color correction was insufficient.

## Direct measured comparison

Measurements below are approximate derived screen statistics from the supplied viewport crops.
They are evidence for design calibration, not extracted MOTHER2 assets.

### Current DOLZORE

Dominant clustered screen colors:
- dark/cool road family ~31.4%
- yellow walkway family ~20.6%
- green grass family ~10.5%
- plaza/tan family ~7.7%

Interpretation:
- top two ground/surface colors exceed 50% of the visible world;
- large road/sidewalk rectangles dominate composition;
- architecture, trees and characters become sparse islands;
- the city reads as a road diagram with buildings placed around it.

Earlier V3 measurement:
- one road gray family ~29%;
- beige walkway ~18%;
- grass ~8%;
- top three colors exceeded 55%.

### Supplied MOTHER2 reference screenshot

Approximate dominant clustered families:
- pale gray-green street ~21.6%
- pale cream/light paving ~17.5%
- vivid green ~13.4%
- deeper green ~7.9%
- remaining area is more widely distributed across architecture/accent colors.

Interpretation:
- no single dark road family dominates the image;
- architecture and vegetation occupy much more of the local visual field;
- pale streets act as connective tissue rather than the focal mass;
- green strips/trees are visually strong;
- facades contribute many medium-sized color blocks.

### Edge / geometry comparison

Approximate diagonal-edge fraction:
- current DOLZORE ~2.1%
- reference screenshot ~8.3%

Current DOLZORE:
- overwhelming 0/90-degree line dominance;
- large rectangular intersections;
- long uninterrupted horizontal/vertical road bands.

Reference:
- more roof/facade/street edge variation;
- diagonal/sloped visual cues;
- denser local edge changes.

## Concrete visual mismatches

### 1. Road occupancy
Current:
road/sidewalk area is too large and too visually heavy.

Reference relationship:
streets are readable but share the viewport with dense architecture/greenery.

Implementation rule:
Reduce continuous road surface width/area and increase bordering visual mass.

### 2. Street value/color
Current:
cool blue-violet road and saturated yellow/cream walkways became a dominant stylization.

Reference relationship:
road/street is high-value, low-to-moderate saturation, gray-green/cream leaning.

Implementation rule:
DOLZORE must use an original pale street family with similar value/saturation relationships, not the previous dark road.

### 3. Building density
Current:
detached buildings with large gaps.

Reference relationship:
commercial/urban sections show tighter facade spacing and continuous town-wall rhythm.

Implementation rule:
Use denser block rows and background/decorative multi-story facade masses.

### 4. Building vertical mass
Current:
many buildings read as low one-story detached shops/houses.

Reference relationship:
several facades read as taller urban buildings relative to people.

Implementation rule:
Increase symbolic building height and window/floor rhythm in commercial/civic zones.

### 5. Character/world scale
Current:
characters still read too tall/large relative to the city.

Reference relationship:
characters are compact symbols inside much larger architectural masses.

Implementation rule:
Reduce world-display size and keep large-head readability without tall full-body proportions.

### 6. NPC density
Current:
residents are isolated and evenly spaced.

Reference relationship:
people cluster around services/street activity while other areas stay quieter.

Implementation rule:
Use social clusters, not evenly distributed NPC dots.

### 7. Facade/detail density
Current:
large simple wall/roof masses with low internal rhythm.

Reference relationship:
windows, signs, awnings, doors, roof lines and neighboring buildings create frequent readable edges.

Implementation rule:
Increase medium-scale facade rhythm before adding microtexture.

### 8. Green integration
Current:
green is often a large rectangular background band.

Reference relationship:
green strips, trees, shrubs and park edges break architecture/street boundaries.

Implementation rule:
Interleave green elements with street/facade edges.

### 9. Crosswalk
Current rejected screenshot:
crosswalk visually drifted/misaligned and was overlarge.

Permanent implementation rule:
crosswalks belong to a dedicated Road Markings Tilemap;
every marking cell must have road below it;
crossing should be one correctly oriented strip, not an arbitrary 3x3 decorative matrix.

### 10. Character appeal
Current rejected earlier avatar:
same-body/color-swap impression.

Permanent avatar rule:
silhouette, hairstyle, lower-body shape, accessory, face, front/back treatment and movement must carry identity before palette.

Canonical avatar spec:
`design/AVATAR_DRESSUP_SYSTEM_V2.md`

## Implementation acceptance before claiming "MOTHER2 research applied"

An Agent may NOT claim this reference is applied merely because:
- AGENT_READ_FIRST was read;
- grammar docs were read;
- a bright palette was chosen;
- route tests pass;
- WebGL builds.

Required:
1. current screenshot;
2. representative reference packet;
3. direct mismatch list;
4. approximate quantitative comparison;
5. explicit implementation changes for the largest mismatches;
6. post-change screenshot review;
7. remaining differences recorded.

## Current research truth

Structural/reference research:
READY.

Quantitative visual calibration:
ACTIVE / NOT EXHAUSTIVE.

Frame-by-frame / broad screenshot corpus measurement:
NOT COMPLETE.

Therefore the previous label `COMPREHENSIVE_REFERENCE_V1_READY_CONTINUOUS_RESEARCH` must not be interpreted as "visual similarity research complete."

## Next exact research actions

1. Add 6–12 representative official/reference town screenshots as source identifiers, not copied assets.
2. For each, record scene type and visual purpose.
3. Measure approximate:
   - road/street share;
   - vegetation share;
   - architecture share;
   - character-to-door/building scale;
   - buildings per viewport;
   - NPCs per viewport;
   - dominant value/saturation families;
   - diagonal/edge mix;
   - empty-space ratio.
4. Create target ranges for original DOLZORE urban/residential/riverside screens.
5. Require production Agents to compare generated screenshots to these ranges.
6. Keep all protected expression original.

`STRUCTURAL_RESEARCH_READY=true`
`QUANTITATIVE_VISUAL_RESEARCH_INCOMPLETE=true`
`ABSTRACT_ONLY_VISUAL_HANDOFF_FORBIDDEN=true`
`DIRECT_VISUAL_EVIDENCE_REQUIRED=true`
