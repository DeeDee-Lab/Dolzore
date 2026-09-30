# DOLZORE FIRST TOWN COLOR CONTRACT

Authority: 2026-09-30 JST
Status: active visual authority

## Purpose

The First Town must use a bright, readable 16-bit-inspired color language compatible with the high-level MOTHER2 reference research while remaining original DOLZORE artwork.

Do not copy an exact MOTHER2 palette table or exact color placement.

## Failure found in V3

The rejected/criticized screenshot was dominated by:
- neutral mid gray road;
- muted beige sidewalk;
- olive grass;
- low-chroma architecture.

Measured from the 1920×1080 V3 screenshot:
- mean HSV saturation: ~0.265
- low-saturation pixels (<0.15): ~34.3%
- high-saturation pixels (>0.45): ~12.4%
- top exact colors:
  - neutral gray ~29.3%
  - muted beige ~18.3%
  - olive grass ~8.3%

This produced a dull, earthy impression inconsistent with the desired cheerful high-contrast town-RPG presentation.

## Current V4 direction

Ground/vegetation:
- spring yellow-green, not olive;
- brighter foliage highlights;
- enough grass area to make ordinary town life feel open.

Road:
- cool blue-violet road family;
- not photoreal asphalt gray;
- subtle texture only.

Sidewalk/plaza:
- light cream/yellow family;
- visually distinct from road and grass.

Water:
- cyan-blue family with bright ripple accents.

Outline:
- deep indigo rather than black/charcoal.

Architecture:
- high-chroma but controlled facade/roof families;
- blue;
- coral/red;
- yellow/cream;
- mint/green;
- lavender/purple;
- cyan accents.

Props/avatars:
- deliberate accent colors;
- palette isolation from common backgrounds.

## Current measured result

Latest avatar/palette screenshot from Unity:
- mean HSV saturation: ~0.442
- low-saturation pixels (<0.15): ~1.1%
- high-saturation pixels (>0.45): ~61.3%

Dominant field colors are now:
- cool blue-violet road;
- bright cream/yellow sidewalk;
- spring green grass;
- deep indigo outlines;
- cyan water;
- varied architecture accents.

These are DOLZORE-specific values, not copied MOTHER2 palette values.

## Quality rule

Future art revisions fail if they drift back toward:
- majority neutral gray/brown;
- all buildings sharing one muted palette;
- characters blending into road/grass;
- excessive beige occupying most walkable space;
- fine texture replacing broad color-block readability.

## Latest proof

Unity run:
`36656034405 = SUCCESS`

Generated branch source:
`82510581568d9b868b06a5a89e4fb9db30e8c907`

The same run also proves:
- avatar quality PASS;
- road-marking alignment PASS;
- mandatory route PASS;
- WebGL PASS;
- generated-source preservation PASS.
