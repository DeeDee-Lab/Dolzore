# DOLZORE MUSIC — Reuse EXISTING website only

User's exact correction on 2026-10-23: 「過去にHPはつくっています。今閉鎖していますが存在します」.

ORIGINAL WEB DESIGN is still present in this same GitHub repository (DeeDee-Lab/Dolzore), NOT a new website to build:
- docs/music/index.html : original complete pixel-art jukebox, 60 track titles and preview UI
- docs/site.js and docs/styles.css : original JavaScript/music controls and design
- docs/assets/dolzore-jukebox.png : original visual
- docs/data/tracks.json : original 60-entry catalog, 200 JPY per track, existing Stripe purchase URLs
- docs/legal, terms, privacy, refund, tokusho, disclosure, support : original disclosure and support pages
- music-only-site-20260928 : archived original development branch

GITHUB PAGES OLD SITE was closed by user's explicit decision (2026-09-30); docs/index.html is intentionally a closure page. Never expose the older Unity town or re-enable GitHub Pages by accident. Railway HP is retired (2026-10-04). User explicitly forbids Railway now.

Existing new empty Netlify PROJECT is only an ISOLATED HOST DESTINATION, never the source or a redesign:
name: dolzore-music-store
siteId: d1edeac2-5299-4b69-9442-653d1dcbbc53
admin: https://app.netlify.com/projects/dolzore-music-store
This project is empty until its first actual deploy (not yet verified).
Do not overwrite dolzore-official-recovery or dolzore-unity-walk-demo.

## Packaging of original sources for Netlify
- Git repository: DeeDee-Lab/Dolzore
- Base directory: music-shop
- Config: music-shop/netlify.toml
- Build command: python3 build.py
- Publish directory relative to base: dist
- Local isolated verification: python3 music-shop/build.py; python3 -m unittest discover -s music-shop -p 'test_*.py' -v
- Root of exported output points to original docs/music/index.html.
- Export whitelist excludes old town/Canvas and unrelated products; does not include paid master audio.
- Temporary public-preview safety: the exported purchase control is disabled until license, Stripe payment and fulfillment tests prove purchase is safe. Catalog retains original Stripe URLs unchanged.
- Source docs/* site content is UNCHANGED by the adapter; only the export is transformed.

## Cutover acceptance
1. Build and test original HTML, CSS, JS, pixel artwork and 60 canonical records.
2. Test real 6 sample media and mark the 54 missing previews correctly.
3. Verify separate royalty/licensing rights for each original Picsart source and standalone MP3 sale.
4. Verify existing Stripe Payment Link live price and purchase fulfillment/receipt/download refund support.
5. Verify legal disclosure, mobile/desktop, HTTPS and generated live Netlify URL.
6. Obtain user acceptance for production purchase mode.
7. Open sales and only then update social profiles/ads according to design/PUBLIC_URL_MIGRATION_MANIFEST.md.

Status at 2026-10-23: an isolated pull request is preparing original-website reuse. No production source switch and NO VERIFIED Netlify DEPLOY. GitHub writes are not evidence of deployment. No Railway.

PDCA PLAN recover ORIGINAL already built HP; DO export adapter and regression tests; CHECK build/preview/checkout/license gates; ACT deploy verified existing HP and selectively unfreeze purchased track delivery.

REUSE_ORIGINAL_WEBSITE=true / NO_REDESIGN=true / RAILWAY_FORBIDDEN=true / GITHUB_PAGES_OLD_CLOSED=true / NETLIFY_FIRST_DEPLOY_PENDING=true
