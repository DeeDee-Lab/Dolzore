# DOLZORE Journal — article handoff contract

This directory is the GitHub handoff boundary for the Agent responsible for DOLZORE website articles.

## Current public rule
The public website is MUSIC + JOURNAL + About/Support/Legal. Articles are editorial content, not a path for re-enabling stopped non-music product sales. Editorial product research, buying guides, and non-affiliate market snapshots are allowed inside Journal when they are clearly identified as research and do not create a DOLZORE checkout/sales lane for those products.

## Ownership
- Article Agent owns article research/copy and article-specific factual citations.
- Website implementation owns layout, navigation, design system, accessibility and rendering.
- Do not overwrite the jukebox/music catalog while editing articles.

## Handoff
Until a richer generator is added, update `docs/data/articles.json` with:
- `title`
- `excerpt`
- `date` (YYYY-MM-DD)
- `url` (relative path under `journal/`)

Create the article page under:
`docs/journal/<slug>/index.html`

Use the shared `../../styles.css` or the correct relative path and preserve the DOLZORE world navigation.

## Truthfulness
Do not invent publication dates, interviews, test results, product usage or personal experiences.
Do not copy text/layout/art/assets from reference sites.
