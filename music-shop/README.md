# DOLZORE MUSIC — existing homepage relaunch (no redesign)

## User exact directions, 2026-10-23
- 「Dolzoreの音源販売ページを再開しましょう」
- 「Railwayを使用せずすすめてください。」
- 「過去にHPはつくっています。今閉鎖していますが存在します」
- 「決済ページを作成されています。HP公開すればそのまま運用できます。」

Original source lives in this SAME repo (DeeDee-Lab/Dolzore):
- docs/music/index.html: original retro JUKEBOX, NOT new layout
- docs/site.js, docs/styles.css, docs/assets/dolzore-jukebox.png: original JS/CSS/art
- docs/data/tracks.json: EXACT 60 BGM entries, 60 unique existing Stripe Payment Links and original ¥200 prices
- docs/legal, docs/terms, docs/privacy, docs/refund, docs/tokusho, docs/disclosure, docs/support: existing law/support
- Historical development branch: music-only-site-20260928
- Legacy docs/index.html intentionally displays Closed; original GitHub Pages URL was deactivated by owner order on 2026-09-30. DO NOT expose closed old town or undo this global closure.
- Railway original runtime also deliberately retired by owner 2026-10-04. NEVER use Railway.

## Reusing original HP without disclosing paid audio
The build script music-shop/build.py copies ONLY the original music, legal, CSS, JS, 60-track catalog and jukebox art to music-shop/dist. The root directs to the original /music/ page.
All 60 preexisting Stripe purchase URLs remain linked to the buy CTA in the exported site. NO new payment pages, accounts or prices are made. Existing six sample previews are kept, 54 remain not-yet-preview-ready, and paid original MP3 master files are not included.
The original docs/ directory is not overwritten. Export-only null DOM reference in the player gets a guard.

## Netlify independent hosting target (created; not yet deployed)
Site: dolzore-music-store
Netlify site ID: d1edeac2-5299-4b69-9442-653d1dcbbc53
Netlify admin: https://app.netlify.com/projects/dolzore-music-store
Expected public URL after deploy: https://dolzore-music-store.netlify.app/music/
Currently the Netlify site URL has been observed returning HTTP 404 (no deploy).
There are two other Netlify sites (dolzore-official-recovery, dolzore-unity-walk-demo) which must never be overwritten.

Setup to deploy via Netlify UI Git integration:
- Repository: DeeDee-Lab/Dolzore
- Production branch: main (after reviewed PR #45 merged)
- Base directory: music-shop
- Build command: python3 build.py
- Publish directory relative to base: dist
Netlify UI repo link does not require adding a new GitHub action Netlify API token or ZIP upload.

## Latest CI and failed deploy facts
Current branch PR: https://github.com/DeeDee-Lab/Dolzore/pull/45
Dedicated music build/test: all six passed (including original Stripe CTA and 60 distinct urls).
GitHub-hosted Ubuntu only; no self-hosted OSK222.
Actual Netlify production deploy attempt: GitHub Actions run 39914850771.
- Build and tests succeeded.
- Deploy failed: NETLIFY_DEPLOY_CREDENTIAL_MISSING.
- All three expected GitHub repository secrets (NETLIFY_AUTH_TOKEN, NETLIFY_TOKEN, NETLIFY_ACCESS_TOKEN) were empty.
- This failure does not imply missing Stripe checkout pages.
- GitHub Actions existing 4 baseline failures concern stale old homepage/index expectations and sensitive marker scanning. Separate triage; never falsely claim global CI success.
GitHub repo Netlify secret is optional if using Netlify's native Git integration.

## Post-deploy real verification
Verify public HTTPS 200, original visual juke/music page, 60 songs, six actual previews, 60 preexisting Stripe href destinations, legal/supported pages, desktop/mobile.
Two sampled Stripe checkout pages returned empty_content to browser fetch, so credit card purchase and fulfillment were NOT technically verified this turn; user directly assures payment pages already built. No further payment pages are to be created.
Do not change canonical social URLs until new site live and verified. See design/PUBLIC_URL_MIGRATION_MANIFEST.md.
Do not make legal guarantees about underlying third-party music model rights without proof; retain as a risk, not misstate it as a missing checkout page.

PDCA: P restore existing site; D isolated export and purchase controls; C six tests pass, production Netlify blocked by missing auth only; A link existing repo to Netlify site and verify live operation.
