# DOLZORE

Public website and Buying Guide source for **DOLZORE**.

This repository is the canonical public-site codebase. Internal automation, private SmartBuy configuration, secrets, personal data, and unrelated projects do not belong here.

## Current scope

- DOLZORE transition home
- Buying Guide
- Projector guide with 10 initial editor-selected candidates
- BenQ TK700STi deep guide
- SmartBuy market snapshot presentation
- Strict recommendation eligibility logic
- Static GitHub Pages source in `docs/`

## Repository layout

```text
docs/                         Static public site for GitHub Pages
  buying-guide/
  data/smartbuy-projectors.json
  styles.css
  site.js
scripts/
  update_market_snapshot.py   Public-safe market snapshot collector
src/
  runtime.py                  Reference eligibility/page logic
tests/
  test_site.py                Recommendation and public-site contracts
```

## Publishing

The site is designed to publish directly from:

- Branch: `main`
- Folder: `/docs`

GitHub Pages can serve this repository without Lovable or Railway.

While the repository remains private, Pages / GitHub-hosted Actions availability depends on the GitHub plan and account usage policy. Do not make any engineering repository public just to publish DOLZORE. If free public Pages and standard hosted runners are desired, this dedicated repository can be made public after verifying that it contains public-safe material only.

## Market snapshot

The public collector can be run with:

```bash
python -m pip install requests beautifulsoup4
python scripts/update_market_snapshot.py --output docs/data/smartbuy-projectors.json
```

The collector fails closed:

- individual item page required
- exact supported projector model required
- SOLD / unavailable listings excluded
- accessories / parts / junk / failures excluded
- suspicious low prices excluded
- C2C listings require positive live-sale evidence
- no result is better than inventing a recommendation

## Important product rule

A high internal score is **not** enough to recommend a product.

Public recommendation eligibility requires a separate validation gate. If zero listings pass, the site must show zero candidates rather than manufacture a top pick.

## Migration boundary

The existing `https://dolzore.lovable.app` site remains available during migration. Business / Creator / Apps links continue to point there until those sections are deliberately migrated.

## Long-task checkpointing

Material implementation phases are recorded in GitHub Issues/PRs so work can resume safely after timeouts or provider interruptions.
