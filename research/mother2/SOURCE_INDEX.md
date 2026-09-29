# MOTHER2 REFERENCE SOURCE INDEX

This file records source authority and the design questions each source supports.
Do not mirror copyrighted media; keep links and derived notes only.

## Primary / official

### Nintendo — MOTHER2 Wii U Virtual Console page
https://www.nintendo.co.jp/wiiu/software/vc/jbbj/index.html
Supports:
- 1994 release origin;
- setting in 199X;
- world-travel structure;
- party/adventure premise;
- eight-melody macro objective.

### Nintendo — Wii U electronic manual
https://www.nintendo.co.jp/data/software/manual/man_jbbj.pdf
Supports:
- field commands;
- contextual talk/check input;
- town map access;
- visible field enemy contact;
- contact direction changes advantage;
- weak-enemy instant resolution;
- battle command categories;
- phone-based save framing;
- hotels/home recovery.

### Nintendo — MOTHER 1+2 MOTHER2 screenshots/archive
https://www.nintendo.co.jp/n08/a2uj/mother2/screen/index.html
Supports:
- connected everyday town/road visual language;
- bicycle;
- cemetery;
- companions;
- strange residents;
- bazaar;
- photography;
- delivery service;
- field/battle presentation.

### Nintendo — 2013 revival message
https://www.nintendo.co.jp/wiiu/software/vc/jbbj/message/index.html
Supports:
- creator framing of MOTHER as a play-space filled with small memorable objects and discoveries;
- player-specific memory formation.

## Creator / developer interviews

### Hobonichi — Akihiko Miura: "The person who wrote the MOTHER2 specification" (2026)
Index:
https://www.1101.com/n/s/mother_project/miura_akihiko/index.html

Part 1:
https://www.1101.com/n/s/mother_project/miura_akihiko/2026-08-27.html
Supports:
- town-concept-first process;
- map/dialogue/status specification work;
- long pre-programming design incubation;
- art/game-design responsibility split.

Part 2:
https://www.1101.com/n/s/mother_project/miura_akihiko/2026-08-28.html
Supports:
- visible/symbol encounters;
- analog/drum HP concept;
- back-attack/contact-angle mechanic;
- late Miyamoto input and implementation complexity.

Part 3:
https://www.1101.com/n/s/mother_project/miura_akihiko/2026-08-29.html
Supports:
- detailed conditional dialogue;
- large amount of endgame/post-battle NPC conversation;
- specification handoff to Iwata/HAL;
- scenario dictation workflow.

Part 4:
https://www.1101.com/n/s/mother_project/miura_akihiko/2026-08-30.html
Supports:
- food variety as travel experience;
- road-movie concept;
- deliberate pleasure/rest area before harsh progression;
- delayed use of player name for late emotional payoff.

Part 5:
https://www.1101.com/n/s/mother_project/miura_akihiko/2026-08-31.html
Supports:
- human-centered design;
- team-wide internalization of creator intent;
- homesickness-style emotional status design;
- localization as recreation of effect rather than literal word substitution.

### Hobonichi — Koichi Oyama interview (2003)
https://www.1101.com/mother_project/entry/archives/MOTHER_them/oyama.html
Supports:
- art direction;
- deliberate non-realism;
- 8x8 minitile asset reuse;
- shared signage glyph/material reuse;
- difficulty of contiguous-world art;
- Onett as a foundational map;
- Magicant as a freer non-everyday art space;
- tension between concrete art and language imagination.

### Hobonichi — MOTHER2 Himitsu book extra interview (2024)
https://www.1101.com/n/s/mother_project/mother2_himitsu_book/2024-11-28.html
Supports:
- map assembly as a tile/material puzzle;
- vehicle art compromises;
- Moonside visual transformation by palette/outline manipulation;
- repeated-talk dialogue escalation and hidden small scenes;
- ending production under constrained remaining assets.

### Hobonichi — MOTHER music interviews (2003)
https://www.1101.com/mother_project/entry/archives/MOTHER_music/
Supports:
- mass music production for MOTHER2;
- non-game musical influences;
- sound texture as world/battle/dungeon identity;
- implementation checked on real hardware;
- emotional "after-feeling" as a composition target.

### Hobonichi — Keiichi Suzuki music interview (2024)
https://www.1101.com/n/s/mother_project/keiichi_suzuki2024/index.html
Supports:
- mixture of outsider and specialist perspectives;
- creative use of hardware constraints;
- music composed for how it is heard in the game, not as isolated songs.

### Hobonichi — Itoi / Iwata MOTHER2 revival conversation (2013)
https://www.1101.com/mother_project/entry/archives/mother2_wiiu/
Supports:
- breadth of audience;
- player-specific memory anchors;
- mixture of moving, silly, uncomfortable and trivial moments;
- development rescue/rearchitecture context.

### Hobonichi — Itoi dialogue interview (2003)
https://www.1101.com/mother_project/entry/archives/MOTHER/06.html
Supports:
- difference between true tone and obvious imitation/parody;
- rejection of visible forced jokes;
- importance of exact language judgment.

## Technical community reverse engineering

### pk-hack / CoilSnake
https://github.com/pk-hack/CoilSnake
Evidence class: COMMUNITY_REVERSE_ENGINEERING
Supports structural decomposition, not Nintendo-authored design intent.

Relevant modules:
- MapModule.py
- TilesetModule.py
- MapSpriteModule.py
- SpriteGroupModule.py
- MapEnemyModule.py
- MapEventModule.py
- DoorModule.py
- MapMusicModule.py
- BattleBgModule.py

Observed architecture:
- global map grid represented as 256 x 320 map entries;
- sector metadata carries tileset, palette, music and setting information;
- map art uses 8x8 graphics units assembled/reused into larger arrangements;
- collision data is stored separately from the visible arrangement;
- event-driven tile changes exist;
- map sprite placement is separate data;
- enemy group placement is separate data;
- doors/transitions are separate data;
- map music can be event-flag dependent;
- battle backgrounds are tile/palette/arrangement data with separate scroll/distortion tables;
- field sprite groups have separate palette and directional/collision metadata.

### EarthBound battle-background reverse engineering
Reference implementation families:
https://github.com/gjtorikian/Earthbound-Battle-Backgrounds-JS
https://github.com/SourceCode/earthbound-battle-backgrounds
Supports:
- layered battle backgrounds;
- palette + tile arrangement reuse;
- scanline/sine-based translation/compression families;
- time-varying distortion rather than frame-by-frame authored animation.

Treat exact counts/formulas as technical-reference facts, not required DOLZORE targets.

## Secondary community reference

EarthBound Wiki / Starmen.net may be used to enumerate locations, formulas or rare behavior.
Never treat community pages as stronger evidence than official/manual/developer testimony.


## Additional official system/reference pages

### Nintendo — MOTHER 1+2 soundtrack listing
https://www.nintendo.co.jp/n08/a2uj/sound/index.html
Supports:
- location-specific town themes;
- travel-mode music;
- service/interior music;
- venue/performance music;
- region identity;
- recurring memory/ending music roles.
Do not reproduce or transcribe compositions.

### Nintendo — MOTHER2 enemy archive
https://www.nintendo.co.jp/n08/a2uj/mother2/monster/index.html
Supports:
- enemy-concept range from people/creatures to everyday/abstract oddities;
- ordinary-world concepts becoming combat content.
Use only as concept taxonomy; do not copy enemy identities.

### Nintendo — MOTHER2 item archive
https://www.nintendo.co.jp/n08/a2uj/mother2/item/index.html
Supports:
- ordinary-looking objects carrying battle/equipment functions;
- character-specific equipment context.
Do not copy item identities or data.

### Nintendo — MOTHER2 character archive
https://www.nintendo.co.jp/n08/a2uj/mother2/hero/index.html
Supports:
- party member differentiation by background/role/ability framing.
Do not copy characters, costumes or silhouettes.
