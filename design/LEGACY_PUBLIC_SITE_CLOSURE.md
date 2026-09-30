# Legacy Public Site Closure

Authority: direct user instruction, 2026-09-30 JST

## Decision

The rejected legacy DOLZORE GitHub Pages website must not remain publicly accessible while the Unity FirstTown replacement is still under review.

The website source/history is preserved in GitHub. Only the public Pages surface is closed.

## Closure implementation

Previous public URL:
`https://deedee-lab.github.io/Dolzore/`

Previous source:
`main:/docs`

The old `.github/workflows/pages.yml` previously reconfigured/republished `main:/docs`.

It was replaced by a closure guard that:
1. reads current Pages state;
2. calls GitHub Pages DELETE;
3. verifies the Pages API is disabled;
4. verifies the old public URL no longer serves the DOLZORE site.

Closure commit:
`c0cbb6aea1271420d75f8f9404172514ba30919a`

Closure workflow:
`36671300007 = SUCCESS`

The legacy GitHub-generated `pages build and deployment` run triggered by the same commit later ended:
`36671299041 = FAILURE`

That failure is expected after Pages was deleted; it did not restore the old site.

## Re-publication rule

Do NOT restore GitHub Pages merely because a new build exists.

Public replacement requires:
- Unity FirstTown visual acceptance;
- route/collision acceptance;
- WebGL acceptance;
- desktop/mobile review;
- no legacy Canvas/town prototype exposure;
- direct user approval for public cutover.

Until then:
`PUBLIC_DOLZORE_SITE_CLOSED=true`

## Source preservation

Do not delete `docs/` simply to hide the site.
It remains historical/source evidence and contains Journal/legal/music content that may be reused or separated later.

The public exposure is the closed edge, not the repository history.
