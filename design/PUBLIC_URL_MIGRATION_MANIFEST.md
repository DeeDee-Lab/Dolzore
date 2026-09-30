# DOLZORE PUBLIC URL MIGRATION MANIFEST

Authority: direct user instruction, 2026-09-30 JST

## Permanent migration rule

If DOLZORE changes its canonical public website URL, the migration is not complete until every known public/marketing/advertising/reference surface has been checked and updated.

Do not change external links until the new canonical URL is live and independently verified.

## Cutover gate

Before any external URL replacement:
1. new canonical URL resolves publicly;
2. homepage loads successfully;
3. representative routes load;
4. no 404/502;
5. content is the intended new HP;
6. SSL works;
7. mobile/desktop basic display verified;
8. canonical URL recorded here.

Current canonical replacement URL:
`https://dolzore-web-runtime-production.up.railway.app/`

Current known broken/new-host state:
- `https://dolzore-official.lovable.app/` = 404
- `https://dolzore.lovable.app/` = legacy/closure host; must never become canonical again

## Social / advertising inventory

### Metricool brand
- brand label: `dolzoreofficial`
- brand id: `6845577`
- timezone: `Asia/Tokyo`

Connected networks:
- Facebook: id `1341212082404928`
- Instagram: `dolzoreofficial`
- TikTok: `dolzoredolzore`
- YouTube: `UC5-Ya1fXY8tYaD3doZBg_cg`

Scheduled posts:
- 0 scheduled posts found for 2026-09-30 through 2027-12-31
- therefore no scheduled-post URL migration currently required

### YouTube
Public channel:
`https://www.youtube.com/channel/UC5-Ya1fXY8tYaD3doZBg_cg`

Migration checks required:
- channel profile links / website;
- About text;
- channel description;
- video descriptions;
- Shorts descriptions;
- pinned comments where used;
- end-screen/card destinations where relevant;
- ad campaign destination URLs if configured outside YouTube channel UI.

Public fetch did not expose a current website URL, so authenticated/manual profile inspection remains required before cutover completion.

### Instagram
Public profile:
`https://www.instagram.com/dolzoreofficial/`

Migration checks required:
- profile website/link;
- bio;
- link-in-bio destination;
- pinned post captions/comments if old URL appears;
- ad destination URLs in Meta Ads.

Public unauthenticated fetch did not expose a website URL; authenticated profile inspection remains required.

### TikTok
Public profile:
`https://www.tiktok.com/@dolzoredolzore`

Public state observed:
- no bio shown publicly at audit time.

Migration checks required:
- profile website field;
- bio;
- ad destination URLs;
- pinned video captions/comments if old URL appears.

### Facebook
Metricool-connected page id:
`1341212082404928`

Public unauthenticated fetch requires login.

Migration checks required:
- Page website field;
- About/contact website;
- pinned post;
- CTA button destination;
- Meta Ads destination URLs.

### X / Twitter
Metricool:
- NOT connected in current brand settings.

Public check:
- guessed `https://x.com/dolzoreofficial` returned 404.

Migration status:
- account/handle must be identified separately;
- if an X profile or X Ads account exists, website/profile/pinned post/ad destination must be updated.

Do not mark X migration complete until the actual account identity is confirmed.

## GitHub / code inventory

Organization-wide exact search for `dolzore.lovable.app` found 9 references.

### DeeDee-Lab/Automation
- `docs/404.html`
- `docs/index.html`
- `docs/buying-guide/index.html`
- `docs/buying-guide/projectors/index.html`
- `docs/buying-guide/projectors/tk700sti/index.html`
- `projects/dolzore-web/content.mjs`
- `projects/dolzore-web/runtime.py`
- `projects/dolzore-web/test_runtime.py`

### DeeDee-Lab/Dolzore
- `tests/test_site.py`

Exact search for `dolzore-official.lovable.app` found 0 GitHub code references.

All 9 old-domain references must be replaced or intentionally removed after the new URL is verified.

## Public web search

Current broad web search found no indexed exact references for:
- `dolzore.lovable.app`
- `dolzore-official.lovable.app`

This does NOT prove there are no non-indexed registrations.

## Additional external surfaces to inspect

Before migration is closed, inspect:
- Google Business Profile if used;
- Google Ads if used;
- Meta Ads;
- YouTube Ads / Google Ads destination URLs;
- X Ads if used;
- TikTok Ads if used;
- Instagram/Facebook profile links;
- creator/profile directories;
- GitHub repo About/homepage fields;
- README badges/links;
- Linktree/SmartLinks/landing pages if used;
- payment/checkout thank-you and return URLs where applicable;
- Stripe product/payment links if they contain site return URLs;
- QR codes;
- email signatures/templates;
- newsletters;
- app store/listing links;
- press/media kits;
- analytics property default URLs;
- Search Console/Bing Webmaster registrations;
- robots/sitemap/canonical/OG URLs;
- webhook/callback/redirect allowlists;
- any external site manually registered by DOLZORE.

## Migration execution order

1. establish and verify the new canonical URL;
2. update site-internal canonical/OG/sitemap/robots links;
3. replace GitHub/code references;
4. update official social profile links;
5. update pinned/fixed posts and descriptions;
6. update advertising destination URLs;
7. update external directories/profile listings;
8. update payment/callback/analytics/search registrations;
9. search the public web again for old URL;
10. open old URL and new URL manually;
11. verify no public traffic path still points to the retired URL.

## Completion rule

Migration is complete only when:
- new URL is live and verified;
- old URL is retired/blank/redirected according to policy;
- every inventory item is checked;
- search for old URL returns no actionable remaining references;
- all modified public links have been manually opened after update;
- a final migration receipt is saved.

`PUBLIC_URL_MIGRATION_PENDING=true`
`NEW_CANONICAL_URL_VERIFIED=true`
`EXTERNAL_LINKS_NOT_YET_CUT_OVER=true`
`NO_FALSE_COMPLETION=true`


## 2026-09-30 emergency new-site recovery — externally verified

Verified canonical replacement candidate:
`https://dolzore-web-runtime-production.up.railway.app/`

External acceptance performed against the actual public internet endpoint:
- `/` = PASS
- `/business` = PASS
- `/creator` = PASS
- `/apps` = PASS
- `/buying-guide` = PASS
- `/about` = PASS
- `/privacy` = PASS
- `/terms` = PASS
- `/health` = PASS / OK
- no 404/502 on the tested canonical routes
- homepage contains DOLZORE new-site content, not source code or recovery placeholder

Railway deployment proof:
- service: `dolzore-web-runtime`
- deployment: `5041f5ad-b97d-4ce7-bce0-6d9aae9c30b3`
- runtime: 1/1 replica online
- start marker: `DOLZORE_SITE_READY 8080`

Important:
This is the verified replacement public URL used for URL-migration execution.
The old Lovable URL remains retired and must not become canonical again.

Next:
execute all migration inventory items and only then set `PUBLIC_URL_MIGRATION_PENDING=false`.
