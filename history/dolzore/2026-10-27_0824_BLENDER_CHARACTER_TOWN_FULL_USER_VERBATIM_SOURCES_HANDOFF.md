# DOLZORE Blender / GAME — 2026-10-27 08:24 JST FULL CONTEXT & TASK HANDOFF (append-only, non-abstracted primary sources)

## Scope & fidelity
All **four user messages** in this DOLZORE-specific continuation are reproduced verbatim in order below. This archive includes the exact original source contents of the two current GitHub design directives, two issue bodies, the seven complete Issue #37 historical comments, all currently available #47 comments, and the relevant five #3 comments. Other prior ChatGPT conversations not available as explicit raw messages are **not** claimed to have been reproduced; other 83 Issue #3 comments remain in their original source thread, not erased. User request is to prevent loss and restart/reanalysis; original files are unchanged.

## Exact current user message sequence (DO NOT ALTER)
### Message 1
~~~~~text
以前ゲームを作成するプロジェクトがありました。。unityを利用していたのですが、全くうまく行きませんでした。blenderだとどうですか？
~~~~~

### Message 2
~~~~~text
それで担当に指示だして
~~~~~

### Message 3
~~~~~text
1分以上の思考の場合は、タイムアウト対策で常時全履歴AgentHub(GD含む)に保存。思考の巻き戻り防止。
~~~~~

### Message 4
~~~~~text
引継ぎ準備して、上限が近いです。情報は抽象化せず正確に詳細にAgentHub(GD)にも全文保存。
~~~~~


## Task current canonical pointers and proven state
- Work Owner issue https://github.com/DeeDee-Lab/Dolzore/issues/47  (existing, open). Do not create duplicate.
- Original full 9,689-char directive https://github.com/DeeDee-Lab/Dolzore/blob/main/design/2026-10-27_BLENDER_CHARACTER_TOWN_ART_PIPELINE_DIRECT_EXECUTION.md (exact GitHub blob c100f4a546eba3d361373f7e17b10dcd496d9e4a).
- Later full 5,167-char style and safety correction https://github.com/DeeDee-Lab/Dolzore/blob/main/design/2026-10-27_BLENDER_PRODUCTION_OWNER_CORRECTION_STYLE_AUTHORITY_AND_RESTART_CHECKPOINT.md (exact GitHub blob c7797c32b7537d10723a6b3e64124a0b459c0f51). **Explicit correction supersedes 2.5-to-3-head production shape from older issue; official FirstTown model 6.5-to-7.5 heads, stylized realistic FF11-like consistent with BAR street quarter.** Never overwrite historical instructions; later specific correction wins.
- User rejection proof https://github.com/DeeDee-Lab/Dolzore/issues/37#issuecomment-5945834235 ; follow-up https://github.com/DeeDee-Lab/Dolzore/issues/37#issuecomment-5954806491 . Unity 6000.5.1f1 WebGL compiled, input and CharacterController demonstrated, but visual result rejected as coarse primitive geometry, low-quality face/hair/costume, poor rigging and unconvincing proportions. Technology compile is not artistic acceptance.
- Town failure proof full postmortem: design/FIRSTTOWN_V7_POSTMORTEM_20261001.md, source blob 37fdbf294997343de0d478b4346568c08816c792; visual gate design/USER_VISIBLE_VISUAL_REVIEW_GATE.md source blob 180afe3ba2c1ea04e7129d857c12b8b8885eaf0e. Never show candidate without opening original actual screenshot/video and direct comparing visual rejection.
- No verified new original Blender .blend or GLB / rendered 3-direction PNG / Idle Walk Turn clip / BAR street 4 view model or walkthrough / same GLB Unity/Godot A/B; no actual PostgreSQL agent Task/Lease/Attempt CLAIM/ACK proven. This is **NOT production-complete**. Do not treat Issue#47, GD material, file source, enabled automation as physical agent execution. REAL_BLENDER_ASSET_COUNT=0_VERIFIED / OWNER_CLAIM_ACK=UNVERIFIED.
- Core technical decision: Blender makes authentic 3D sculpting + rig + animated assets; runtime comparison retains existing Unity and non-destructively tests Godot; no blanket migration yet. Preserve RPG math and first town assets / prior HTML/CSS/WASM / existing Netlify 60 music sale catalog / Stripe, closed Railway URL and all existing user assets. Never modify commercial site without a new separate user authorization. No public deployment.
- AgentHub #20 contains current actions/runner policy and Dolzore latest relevant safety commit e37c8469c03ab0e444947efde4da20a76664c61b prohibited OSK222 self-hosted runner. Older GitHub Actions success/blanket credits claim != current per-project authorization. Don't start old OSK222 runner or desktop front takeover without fresh approved safe contract. DC, RDC, Tailscale, Railway, foreground black-window, new paid API, 6th seat forbidden.
- Required next actual steps: Owner use existing 5-seat AgentHub authorized Task/Lease/Attempt with claimed/retrieved ACK (when possible) -> identify approved **headless** Blender-capable machine in READ-ONLY step -> Builder A deliver original **6.5-to-7.5-head** character real .blend .glb and front/3-quarter/back PNG SHA -> actual Armature Idle Walk Turn exported clips and video -> Builder B deliver real single BAR street-block `.blend` `.glb` with real architectural density, collision and 4 renders + walkthrough -> Inspector independently open actual bytes, reject failed visual artifacts (no cheap primitive placeholders, no dominant road grid); Runtime import SAME GLB into existing Unity and Godot with equal camera/input/lighting and measure playable real results -> user acceptance separate. Evidence IDs, exact filename bytes, SHA, frame visuals, task ACK, QA and blockers logged; no same-method third patch after two artistic failures.
- Existing Google Drive protected archive found through search: https://docs.google.com/document/d/1Kol39aFO4ds33sd0TU3Bkppgg_KZkpi5NlBrUHOHIoQ/edit . Verified its previous 13 source-block / 34,138-char preservation existed BEFORE newest user message; append new delta to **same** existing doc, read back, no rewrite of prior block.
- Prior explicit AgentHub GitHub write attempt to #239/#177/history/dolzore was safety-blocked as recorded in Issue#47 comment 6429834227. Do not claim success there unless fresh readback; do not bypass or change account permissions. The Dolzore repo main plus the connected DOLZORE-owned GD are accessible durable sources and AgentHub Owner has pointers from Issue #47. Recording in Dolzore repo is not equivalent to writing AgentHub own private GitHub history.

## Verbatim original source materials, no abstracted substitutions

### BEGIN CANONICAL_9689_CHAR_DIRECTIVE
ORIGINAL_URL https://github.com/DeeDee-Lab/Dolzore/blob/main/design/2026-10-27_BLENDER_CHARACTER_TOWN_ART_PIPELINE_DIRECT_EXECUTION.md
CHARACTER_LENGTH 9689
~~~~~text
# DOLZORE — Blender造形先行・主人公1体と街1区画の実制作／Unity・Godot比較（直接命令の全文、2026-10-27 JST）

## 1. 直近ユーザー原文（省略・言い換え禁止）

> 以前ゲームを作成するプロジェクトがありました。。unityを利用していたのですが、全くうまく行きませんでした。blenderだとどうですか？

それに対し、「Blenderでキャラクター1体と街の1区画を制作し、Unity継続とGodot比較を実動作で判断」という提案を提示した後の追加原文：

> それで担当に指示だして

**目的：このゲーム制作担当に具体的な制作行動を開始させることであり、検討会・ツール紹介・単なる研究に戻らない。Blenderを制作ツールとして正式な限定パイロットに採用し、Unityからの全面移行は結果が出るまで決定しない。** User's original style: MOTHER2の温かい町の読みやすさ・生活感（保護表現は模倣しない）、FF11風の奥深い内側のRPG、オリジナルDOLZORE、将来のRPG拡張性を守る。

## 2. 2026-10-27に確認した過去の本物の失敗・制限

- Character issue https://github.com/DeeDee-Lab/Dolzore/issues/37 はOPEN。Unity 6000.5.1f1のコンパイル、WebGL、CharacterController、キーボード/HTML D-padの入力検証は技術面で成功記録がある。だから「Unityでは移動すら実装できなかった」と誤記するな。
- #37 comment https://github.com/DeeDee-Lab/Dolzore/issues/37#issuecomment-5945834235 に2026-10-02 USER VISUAL REJECTION。原因：円柱や球などのprimitive結合、粗い髪束/顔、衣装/金具の装飾不足、平板な光、2.5〜3頭身級の魅力なし、root transformの擬似歩行、髪/服の二次動作欠落。実体として不合格。
- #37 comment https://github.com/DeeDee-Lab/Dolzore/issues/37#issuecomment-5954806491 に新しいKayKit等の低ポリ系素材はユーザー要望の品質を満たさないと明記。既存のUnity asset proofをそのまま「新完成品」に出さない。
- 町の品質受入は https://github.com/DeeDee-Lab/Dolzore/blob/main/design/USER_VISIBLE_VISUAL_REVIEW_GATE.md と https://github.com/DeeDee-Lab/Dolzore/blob/main/design/FIRSTTOWN_V7_POSTMORTEM_20261001.md 。V7/他の技術成功・画像不合格の理由：単調な長い道路、孤立した箱建物、緑の矩形、道路偏重、手作業の構図なし、積み重なった小修正。第3回の同一方法パッチ禁止。現物画像を見ずにユーザーへ出すことを禁止。
- 2026-10-03町の改善履歴から相対的によいbaselineはrun37074718843 / commit2c3611919afd81e20f0e1669d2068b904aa68438。これをbefore/after比較用に保全する。技術に成功していても、その絵を完成と判定したわけではない。
- https://github.com/DeeDee-Lab/Dolzore/blob/main/design/GAME_CANONICAL_DIRECTION.md の外見/内部RPG/5属性/職業/生活/オープンリージョン構造を保持。Blenderを**描画/リグ/アニメ・3D制作ツール**として用い、Godot/Unityいずれかはゲームロジック・衝突・カメラ・UI・セーブを担当させる。Blender単体をMMO完成エンジンとして扱わない。
- 現在の音源販売は既存Netlify https://dolzore-music-store.netlify.app/music/ で運用されており、音楽販売・Stripe/Preview/catalog/法律/現在の公開サイトは本ゲーム実験でいっさい変更しない。Railwayへ戻さず、停止済GitHub Pages/旧HPを復活しない。公開切替は本番承認なしに行わない。

## 3. 最初のユーザーが実際に目で見て歩ける受入範囲

**Deliverable A（主人公1体）**: DOLZOREオリジナルの明るい小柄なファンタジー冒険者。#37の制作希望を受け継ぎ、基本シルエットは頭2.5〜3頭身、独自の表情/目/髪を持ち、キャラクターだけで見分けがつく。方向の暫定参照として金髪＋白・青・金のローブ/軽装＋杖またはクリスタルを使ってよい。ただし外部作品の顔・衣装・リグ・骨格の複製は禁止。過去の不採用パーツは品質反面教師としてのみ使う。正面/背面/左右斜め/歩行近景で見て成立する独自の面構成・色・質感を作る。**primitive-only造形は禁止**。別名/名前やデザイン変更は承認があるまで実験内部に限る。

- Blender `.blend`実ファイルに、命名済みメッシュ、独立した髪束層、顔/眼/眉/口、胴/袖/装飾、手足、装備アクセサリ、材質を持つ。鏡像メッシュ破綻、隙間や貫通、長距離/近距離での目と輪郭の読めなさ、くっついた棒状の手足は不合格。
- 任意のtoon/cel/hand-paintedスタイルを実際に比較する。色と明度が混濁しないよう一貫したマテリアル・輪郭線と照明を作る。描画エンジン独立のtexture set (base color/emissive/必要ならnormal)をエクスポートできるようにし、独自Blender shader依存だけのアートで他エンジンで崩れないこと。
- 変形可能なArmatureをリグとして作る。Idle、Walk、Turn 3アクションを最低基準（可能ならRunも別）。歩行ではかかと/つま先/接地、頭/肩の揺れ、服と髪の自然な二次動作、アクセサリ追従をテスト。Animation sourceはBlenderのNLA/actionに残し、GLB/glTFへ同名clipを保存。骨の不一致・T-pose・足滑りはFAIL。
- 実ファイル：character.blend, character.glb, source textures, renders front/3q/back/side, actual walk animation clip（動画） , export_manifest.json, license/source ledger, sha256 manifest。Blender版/エクスポート引数/ポリ数・テクスチャサイズ・rig bonesは実測値を記録、偽の数値を埋めない。

**Deliverable B（街1区画）**: DOLZORE FirstTown全96×72タイルまたは全世界を最初から造らない。実用1区画として**Central Main StreetのBAR近くの一角**を選び、歩き回りの小体験として閉じる。道が画面全体の50%以上を支配しないのを目標としつつ、構図で歩道・店先・ベンチ・花壇・樹木・街灯・看板・視線の先のランドマークが自然な読み順になるよう実構図A/B。町の原本の地理・district conceptに矛盾しないよう整合させる。

- 建築3タイプ以上（BARの外観と店舗/住宅/工房等）、少なくとも連続する建物2棟の高低/屋根/窓/軒/看板に差異をつけ、道路一枚＋四角いハウス3個を再生産しない。外壁材/窓枠/看板/店舗灯りにテクスチャとスケールをつけ、遠景/中景/近景が同時に成立する。
- 地面は路面/歩道/縁石/石畳・芝等を目的に沿って分ける。過去に不合格となった正方形の広い緑地と全平坦道路は使用しない。密度は視認性・歩行可能幅を犠牲にしない。NPC用の安全なスペース/道幅を確保。
- 正確なcollisionは表示meshと独立した名前付き簡易shapeで書き出す。画面外へ落下、建物の壁へすり抜け、BAR入口への不通、無限閉塞、同一区画内のループ移動不能はFAIL。
- Environment.blend, Environment.glb/構造が伝わる別エクスポート、texture/source, light preset, 4方向と3/4視点のスクリーンショット, one-block camera path clip, sha256/licensing manifest を作る。演出は出力動画だけでなく、ユーザーが入力して動ける状態を必ず後続エンジンで確認。

**Deliverable C（両エンジン比較）**:
- **同じGLBアセット、同じmap、同じcamera framing、同じライト基準、同じ解像度、同じプレイヤー操作条件**で、既存Unity 3D side branch と最小Godot side branchを用意。Godot採用を先に確定しない。安全に動作可能な既存Unity build pipeline/runner/ライセンスを調べ、実行資源がなければ負荷を避けて別ランナー利用。GodotバージョンおよびWeb renderer/compatibility適合は実環境で計測し、モバイルWebGL/WebGPU/GLXに一般的な断定をしない。
- どちらも WASD/矢印＝歩行、カメラ追従、Idle/Walk/Turnの本当のアニメーション、簡単なNPC1体/店入口を示す（NPCは暫定素材と明示）。カメラは街を読みやすい同角度からスタート、両エンジンの視覚変化（色/影/透明/リグ/物体サイズ）をbefore/afterに並べる。
- 指標：同一スクリーン構図、実フレームレート、frame time、初回ロード所要、ビルドサイズ、各操作成功、collisionルート成功、アセット差し替え容易性、カメラ/見た目の正確さ、音なしでも理解できるUI。値は実測してJSONへ。比較不能はUNKNOWNにし架空のFPSを作らない。
- 最初の成果は**ローカル/許可範囲の動くデモ**。公共Web URLを無断作らない。Web向け出力が重すぎる場合は正直に記録し、比較のためのネイティブ版を使う。GitHub Actionsの成功はスクリーンの美観合格ではない。

## 4. 担当別の絶対作業順（既存AgentHub論理役割・5席枠内）

**既存DOLZOREゲームOwner:** 実際に https://github.com/DeeDee-Lab/Dolzore/issues/37 の原文/全5コメントと https://github.com/DeeDee-Lab/Dolzore/blob/main/design/GAME_CANONICAL_DIRECTION.md / FIRST_TOWN_REBUILD.md / USER_VISIBLE_VISUAL_REVIEW_GATE.md を読み、Unityで起きた「技術合格なのに品質不合格」の原因を短絡的に覆さない。既存の5席の中へ具体Taskを作り、承認済みAgentHub Task/Lease/Attempt/ACKを取得できたらIDと承認経路を投稿。**Issueコメントのみでは自動的にWorkerを起動した証拠ではない**。別のビルダーや架空の第6席を立てない。

**Builder A / Character Art:** 1) 過去の不合格キャラの実レンダーとvisual referenceを照合; 2) キャラ造形の2方向のデザイン候補をBlender sceneに作成; 3) face/hair/clothing silhouetteの良い候補から一体を完成; 4) export可能なrigとIdle/Walk/Turnを仕上げ、4方向静止画と回転表示、歩行クリップを実生成; 5) 中間時点でInspectorのrender feedbackを受けて外見を修正; 6) `.blend`と`.glb`の実バイト/hashをGitHub LFSまたは承認済みprivate artifactへ記録。構想図だけで終わらない。

**Builder B / Environmental Art:** 1) FirstTownの現行地理/道路を安全に閲覧; 2) BAR近くの1区画と建物正面から決定; 3) block layoutと3段階の密度・マテリアルを別バージョンでBlender制作; 4) 実材質/ライティング/外壁装飾、植栽、街具を追加; 5) 動線とcollisionを分離; 6) exports/renders/真正source ledgerを作成。画像生成だけではBLEND_MODEL_PASSにならない。

**Integration / Builder available within existing pool:** character.glb + block.glbを既存Unity側とGodot最小比較レーンに投入。両側で同条件のwalk, turn, collision, camera, performance検査。新ゲームエンジンを大規模インストールする場合は既存マシンのGPU/空きストレージ/ライセンス/動作中アプリ負荷を**読み取り専用で**把握、問題なければ承認済みヘッドレスRunner/Workflowだけで行う。ユーザーPCの画面前面・マウス・キーボード・ターミナルを乗っ取らない。

**Independent Inspector:** 以下3軸を別判定 (i) Visual: 現物PNG/動画を実際に開き、旧失敗とのA/Bと目/顔/髪/細かな服飾/キャラの魅力/街の自然な構図/塗り、スクショUI、(ii) Technical: blend/glb開封、アクションimport、幾何エラー/テクスチャ欠損/負荷、(iii) Game: 経路・入力・fps、を評価。**全部の評価が証跡付きPASSになるまで USER_VISIBLE_READY=false**。品質FAILなら座標変更パッチを積むのではなく、原画・造形・街の構図へ戻す具体指摘を出す。ひたすら研究して出力を止める行為も禁止。

**Runtime Verifier:** 承認済みホスト上の実BLEND/GLB/画像/動画/Unity build/Godot projectの本物を再取得、サイズ・SHA照合→headless load→保存・再読み取り→入力実プレイ。CI成功・エクスポート成功は本人の視覚合格とは違う。証拠には `scene_build_id / model_shas / clip_shas / before_vs_after_render_paths / viewport_screens / evidence_owner / input_route / host / commit / inspector_status` を記録。

## 5. 合格条件／成果物と提出基準（設計説明や単なる生成スクリプトでは不合格）

**Gate 0 — authority/source:** 既存`DeeDee-Lab/Dolzore` GitHub main ソースと、過去の不合格スクリーン・デモの実物を固定。最新本番音楽販売URLと旧サイトClosure Guardを保護。mainはGitHub source-of-truth、実画像・blob artifactは適正なGitHub LFS/Release/Actions artifactまたは既存許可プライベート保管先に保存。社内ファイルシステムやGoogle Driveへの無断保存は禁止。誤ったpublic URLを使わない。

**Gate 1 — Blender proof:** レンダーではなく `.blend`と`.glb`実ファイル/Blender版・実readback・書出しログ・アニメーションclipsを伴う。保存/解凍・簡易mesh review・headless開封・3方向の見た目。実体なし=FAIL_ARTIFACT_MISSING。

**Gate 2 — Visual Review:** 高品質なキャラ/街の実出力を旧#37不合格画像と比較。横並びして顔/衣装/髪/輪郭/手足/明度/歩行と街の路地/立体感/生活感/店の個性/街の色温度を目視でコメント。ここでVisual FAILなら**ユーザーに新候補として見せる前に制作へ戻す**。実画像を開かずに合格不可。少なくとも案A/Bが実際の別レンダーとして存在。

**Gate 3 — Motion:** 特にWalkが足を滑らせない、腕/杖/服が破綻しない。idle→walk→stop→turn→walkを1分以上繰返して破綻/キャラ床への埋没を検査。UnityとGodot両方に入れてもボーン/材質が消えないこと。

**Gate 4 — Playable:** 全画面直線/対角/角/店入口まで歩け、予期せぬ道遮断なし。Unity/Godotで入力、カメラ、FPSの実比較。簡易静止レンダーを『プレイ可能』と呼ばない。

**Gate 5 — User handoff:** 技術PASS、独立InspectorのVisual PASS、実動作/ログPASSならユーザーへ実画像・比較動画・操作できる保存先を提供し、ユーザー本人の採否は別記。本人承認前に公開サイトへ上書きしない。Unity残留/Godot移行決定は比較成果を見てから。

**必要な出力ファイル（一例、存在しないうちは命名を実体化しない）：**
- `characters/dolzore_adventurer.blend`、`characters/dolzore_adventurer.glb`
- `town/central_main_street_bar_block.blend`、`town/central_main_street_bar_block.glb`
- `reviews/character_front_threequarter_back.png`、`reviews/character_idle_walk_turn.mp4`、`reviews/town_threequarter_A_B.png`、`reviews/one_block_walkthrough.mp4`
- `comparison/unity/`、`comparison/godot/`、`comparison/metrics.json`、`comparison/input_collision_test.json`
- `source/LICENSES_AND_ART_PROVENANCE.md`、`source/sha256_manifest.json`、`source/USER_VISIBLE_VISUAL_REVIEW_RECEIPT.md`、`source/OWNER_RUNTIME_ACK.md`

## 6. 停止・変更・削除禁止の境界

- 今回は**Blender-first限定検証**。ユーザーが今回指示したのはゲーム全資産の一括移行・全削除・公開差替えではない。既存UnityビルドとC#ルールエンジンを保持し、Godot移植は短い比較プロトタイプの範囲に限定する。
- 現行音楽販売のNetlify、60曲カタログ、既存Stripe決済、法務ページ、問い合わせ、既存Webフロント、本番HPを変更禁止。Railwayでの再公開/旧GitHub Pagesの再開禁止。
- 著作物の流用、FFXI/マザー2/Hero Wars/どうぶつの森等のキャラ・マップ・画像をそのまま抽出・模写することを禁止。権利とライセンスの原本添付。
- 新規高額・従量課金サービス、外部AIでの社外秘原本漏えいは禁止。新AgentHub席/ジョブ枠/端末操作権限を増やさない。
- Remote Desktop Commander/“DC”/Tailscale/可視黒窓/前面奪取は禁止。無断Actionsの無制限ループ、NASの一斉再マップ等も禁止。
- ひとつの方法で連続2回同じ視覚的失敗なら工法（造形/原案/構図）を変更。古い生成済みprimitiveに細かな道路建物追加を続けない。ユーザーへ失敗候補の画像をプレゼンしない。

## 7. 持続監査とPDCAの原文保存

毎回 `git_head, task_id, real_owner_claim_ack, blender_version, engine_import_version, machine_name, license_assets, created_files_sha, visual_pass, gameplay_pass, actual_video_sha, encountered_errors, reason_of_fail, exact_fix_order, next_action` を残す。**計画/コメントだけでは3Dの実ファイル0・動画0・プレイ0**という実情を報告。回収可能な中間成果（造形済みモデルや比較レンダー）を安全なGitHubブランチ/PRとartifactに継続保全。対象Issuesの原コメントは削除・圧縮せず引用元を保持。

**最優先実作業順序（依頼直後）：** (1) 既存AgentHub実受領の確認 (2) Blenderを動かす認可された既存マシン・Runner・必要依存を読み取り専用調査 (3) 第一実造形キャラクター＋3方向PNGを実生成 (4) Blender rig とWalk実生成 (5) BAR区画の3D原型＋構図比較 (6) UnityとGodot GLB A/Bプレイ (7) Inspector目視・Runtime実機合格 (8) ユーザーへ成果物の実ファイルと違いを報告。

`BLENDER_FIRST_PILOT_AUTHORIZED=true; FULL_ENGINE_MIGRATION_NOT_AUTHORIZED=true; CHARACTER_REAL_BLEND=NOT_YET_VERIFIED; TOWN_REAL_BLEND=NOT_YET_VERIFIED; GODOT_COMPARISON=NOT_YET_RUN; UNITY_LEGACY_PRESERVED=true; USER_VISIBLE_VISUAL_GATE_REQUIRED=true; NETLIFY_MUSIC_UNCHANGED=true; ACTUAL_AGENTHUB_ACK_UNVERIFIED=true`

~~~~~
### END CANONICAL_9689_CHAR_DIRECTIVE

### BEGIN CANONICAL_5167_CHAR_LATEST_CORRECTION
ORIGINAL_URL https://github.com/DeeDee-Lab/Dolzore/blob/main/design/2026-10-27_BLENDER_PRODUCTION_OWNER_CORRECTION_STYLE_AUTHORITY_AND_RESTART_CHECKPOINT.md
CHARACTER_LENGTH 5167
~~~~~text
# DOLZORE — Blender実制作・正本体型衝突是正・AgentHub/GD保存要求（2026-10-27 JST）

## 新しいユーザー直接命令（改変・省略禁止）

> 以前ゲームを作成するプロジェクトがありました。。unityを利用していたのですが、全くうまく行きませんでした。blenderだとどうですか？
> それで担当に指示だして
> 1分以上の思考の場合は、タイムアウト対策で常時全履歴AgentHub(GD含む)に保存。思考の巻き戻り防止。

## 実制作はすでに正式起票済み。重複Task/検討会を増やさない

- Owner指示の直接作業Issue: https://github.com/DeeDee-Lab/Dolzore/issues/47 （open, 2026-10-26 21:19:23Zに起票、現地2026-10-27）。
- 既に作成済みの9,689文字・元の依頼原文・キャラ/街/モーション/Unity-Godot比較/5席役割/検査をすべて含む詳細原本：https://github.com/DeeDee-Lab/Dolzore/blob/main/design/2026-10-27_BLENDER_CHARACTER_TOWN_ART_PIPELINE_DIRECT_EXECUTION.md （main commit 0d67e8863c589c6a6fafc93672d2e636ab293c03）。**ここを省略・要約して別の一般的AI指示へ置き換えない。**
- 旧ユーザー不合格と技術PASSを分離した一次証拠：https://github.com/DeeDee-Lab/Dolzore/issues/37#issuecomment-5945834235 、https://github.com/DeeDee-Lab/Dolzore/issues/37#issuecomment-5954806491 。
- 街全体のユーザー直指示による方向修正：https://github.com/DeeDee-Lab/Dolzore/issues/3#issuecomment-5973706588 。詳細では『chibi約3頭身と半写実3D街の混合は不合格』『標準プレイヤー/NPCは概ね6.5〜7.5頭身、FF11方向に寄せた統一stylized-realistic素材/光』と明記されている。
- visual hard gate: https://github.com/DeeDee-Lab/Dolzore/blob/main/design/USER_VISIBLE_VISUAL_REVIEW_GATE.md 、V7失敗：https://github.com/DeeDee-Lab/Dolzore/blob/main/design/FIRSTTOWN_V7_POSTMORTEM_20261001.md 。

## 【P0是正】Issue #47の主人公体型が既存の新しいユーザー修正と矛盾する

ISSUE47 BODY / original directive draft currently says 『オリジナル2.5〜3頭身のプレイヤー1体』『Hero-Wars-like chibi』。これは2026-10-02単体のUnity『Lunaria Walkable Character』の視覚改善案を踏まえた内容だが、**2026-10-03のDOLZORE街全体の実ユーザー却下・最新に適用すべき6.5〜7.5頭身の決定**を反映していない。
**修正命令**：製品DOLZORE FirstTownへ実際に統合する『標準キャラクター1体』は6.5〜7.5頭身のオリジナルstylized-realistic方向で制作し、顔・目・髪・衣装・装飾・骨格・本物のIdle/Walk/Turnを高品質にしてBlender `.blend` / `.glb` /実動画/3方向PNGを出す。BAR街区も同じ統一スタイル・材質・光に揃える。2.5〜3頭身キャラクターの実験を行うなら**明確に separate rejected-reference style experiment** と記し、勝手に本番候補・街のプレイヤーとして採用しない。
**例外/確認**：ユーザーが今後『街全体もchibiに戻す』と直接指示した時のみ体型・街のスタイルを変更可。今回の『Blenderならどうか』『それで担当に指示だして』という表現だけで2026-10-03の明確な却下を撤回したとは扱わない。
**修正箇所**：#47 Issue bodyの元文は改ざん削除せず、本補足を最新指示として付けて担当が起動前に読む。既存9,689字指示は歴史原本として永久保存する。Owner/Builder A/B/Inspectorは必ず『6.5〜7.5頭身／非chibi統一アートスタイル』を手元の実タスクに適用し、スタイルを未確認のまま造形開始して完成扱いしない。

## 正確な制作実装（既存9,689字正本を省略・変更せず追加指定）

1. Ownerは既存ゲームアセットとUnity demoの現在のsha、Issue#3/#37/#47、style correction、最も良いaccepted referenceに対する不合格箇所を鮮度付きで確認。既存AgentHub five-seat Owner/BuilderA/BuilderB/Inspector/Runtimeの本物のTask/Lease/Attemptへ実作業を登録。実CLAIM+ACKを取れた場合だけ受領と記録。Issue commentだけではWorkerが自動起動した証拠なし。
2. Builder A最初の成果は **本物のBlenderファイル** `character.blend`, `character.glb`、正面/3/4/背面の高解像度実レンダーPNG＋マテリアル/テクスチャ/骨構造/対応Blender版のmanifest。円柱・箱・棒の組み合わせだけは禁止。キャラ全長6.5〜7.5頭身・顔つき・衣装の情報量・自然な関節を実画像で判断し、旧メッシュとのA/B実比較。ソース出典と商用利用権を明記。
3. Builder A次に首/腕/腰/膝/足首/表情/衣装の本物Armature、Idle/Walk/Turn 3clip（名前一致）+glTF export・walk.mp4（正面/斜め/側面）。脚が滑る/髪や衣装が身体を突抜ける/ウェイト破綻/T-pose恒常はFAIL。GPU/VRAM/ライセンスの足りないローカルPCで無理に起動しない。
4. Builder Bは Central Main Street のBAR周辺1区画を、街用統一スタイルで3種以上の建物・自然な連続する街路/路地/生活感/植栽/店頭照明/看板/家具/マテリアルを持つBlender `town.blend`/`town.glb` + 4方向PNG + walk-through.mp4として制作。大きな四角路面/孤立した箱家屋/V7のcoordinate-patch loopは不合格。BARまで歩ける経路と独立collisionプロキシを残す。音楽/Stripe本番には一切触れない。
5. Integrationは実存在する同一の`character.glb`/`town.glb`を、現在実行許可が確認された各Unity/Godot検証支線に入れてWASD/矢印・アニメ/カメラ/衝突・実測fps/load/build sizeをJSONで比較。Godot本採用は調査後にユーザーが決定することであり、既存Unity本体/C# internal FF11-derived rules/data/公開サイトを無断削除/上書きしない。
6. Inspectorは生成済みの実PNG/MP4と元#37とFirstTown V7失敗画像を**実際に開いて**比べ、具体的な顔・輪郭/髪/衣装/腕脚/自然な動き、街の密度/統一世界観/道路割合/建築シルエット/ライティングを採点。 `TECHNICAL_PASS != VISUAL_PASS != USER_ACCEPTED` を徹底。Visual FAILなら具体的修正と別の構図・造形方法に切替。失敗現物をユーザー向け新完成品として提示しない。
7. Runtimeはblend/glb実バイト・license/readback SHA・実アニメとUnity/Godot入出力を実際に開いてQA。まだ未生成なら `REAL_BLEND=0, REAL_GLB=0, REAL_WALK_VIDEO=0, UNITY_GODOT_AB=0, PG_ACK=UNVERIFIED` と証拠を記録し、『指示登録完了＝ゲーム制作完了』は厳禁。

## 実行制約 — 新旧矛盾は最新の権限証明で解決

- `DeeDee-Lab/Dolzore/AGENTS.md` のDC/RDC絶対禁止とCodex/Claude Codeの**read-only**を守る。内部Owner/Builderのみが正規の生成/実装を実施。外部coding agentに自動生成・コード変更を代理委託しない。
- AgentHub #20の現行正本ではActions利用禁止の記述が残り、Dolzore main `e37c8469c03ab0e444947efde4da20a76664c61b` の最新コミットは `policy(ci): stop prohibited OSK222 self-hosted runner [skip ci]`。**旧OSK222 runnerやActionsの過去成功を最新の実行許可と誤認しない。** ユーザーから個別にActions利用可能とされた別の文脈もあるため、実行時に現在の当該project workflow/runner安全契約と承認ルートを確定できる場合のみ非対話ジョブを実行。実証不可なら該当ホストは起動せず、ソース保全・他の明示承認済み実行基盤を探索。
- 絶対にDC/RDC/Tailscale/黒窓/前面奪取/無断public release/Railway/無断従量課金・利用者ソフト削除/音楽60曲・Stripe/Netlify・既存Main Gameの破壊行為は禁止。

## 毎回の履歴保存と巻戻り防止

**ユーザー原文全文、直近発見の実ハッシュ・URL・artifact IDs、Issue#47原本文書（9,689文字）と本補足、過去#37/#3の原コメント、task state（CLAIM/ACK有無）をGitHub AgentHub history正本に全文でcheckpoint。** 更新後には専用Google Docsへ同じ本文を全文挿入し、start/end一意マーカー+原文テキスト非改行一致をreadback検証。議論の途中段階で作成されていない実3D資産のSHAを創作しない。進展があれば毎回追記し、すでに検証済みの発見から再分析し直さない。
現時点で確定できる受入状態は Issue #47 / 指示書 / 正本commitの**存在のみ**。新Blender `.blend`, `.glb`, 3D walkthrough, Unity/Godot同条件比較の実生成証明はなし。Ownerの実PG Task/Lease/Attempt CLAIM/ACKなし。最初の再開地点は **6.5〜7.5頭身の体型是正が本物の担当Taskに反映されたか／既存認可済みBlenderを誰が実起動可能か**、次にcharacter.blend/glb＋前/3/4/後の本物PNG/manifest/SHAを取得できるか。

STATUS: `BLENDER_DIRECTIVE_GITHUB_READBACK=PASS / NEW_USER_HISTORY_RULE=REGISTERED / STYLE_CONFLICT=CORRECTED_IN_ADDENDUM / ACTUAL_AGENT_ACK=UNVERIFIED / REAL_3D_MEDIA_OUTPUT=0_VERIFIED / OLD_GAME_MUSIC_SITE_UNCHANGED=true`.

~~~~~
### END CANONICAL_5167_CHAR_LATEST_CORRECTION

### BEGIN ISSUE47_ORIGINAL_BODY
ORIGINAL_URL https://github.com/DeeDee-Lab/Dolzore/issues/47
CHARACTER_LENGTH 2105
~~~~~text
<!-- DOLZORE_BLENDER_FIRST_OWNER_ORDER_20261027 -->
# ユーザー直接承認：DOLZOREをBlenderで造形先行再制作（既存Unityは保全しGodotと比較）

**直接ユーザー原文（変更・要約しない）**
> 以前ゲームを作成するプロジェクトがありました。。unityを利用していたのですが、全くうまく行きませんでした。blenderだとどうですか？
> それで担当に指示だして

**実作業の省略なし原本（9,689文字・GitHub全文readback済）**：https://github.com/DeeDee-Lab/Dolzore/blob/main/design/2026-10-27_BLENDER_CHARACTER_TOWN_ART_PIPELINE_DIRECT_EXECUTION.md

**本Issueの責任者は既存DOLZOREゲームOwner。** 既存AgentHub５席のOwner/Builder A/B/Inspector/Runtimeの実Task/Lease/Attemptへ割り当て（本Issueへのコメントだけをworker受領ACKと扱うな）。#37のUnity歩行デモが実動作PASSながら見た目をユーザーから却下された根本原因を修正する。既存Assets・再生可能なUnity WebGL buildは保持し、旧不合格モデルをGold/完成物と言うな。

### Do now — 成果物のある4つの独立レーン
- [ ] **Blenderキャラクター原本**：オリジナル2.5〜3頭身のプレイヤー1体、顔/瞳/髪束/衣装/装飾/装備を手製造形。制作シーン **.blend** + **.glb** + 前面/斜め/背面の本物PNG（primitive-only不可）。プロジェクトは既存ライセンス下で実開封できること。
- [ ] **Blenderアニメ原本**：Armature＋Idle / Walk / Turnの3実アクション、ルート移動/歩行・髪や衣装の2次動作を調整し、実歩行動画・glTF clipを実生成。T-pose/足滑り/エクスポート後骨欠落不可。
- [ ] **FirstTown BAR付近の1区画**：異なる3種以上の建築、軒/窓/舗装/看板/植栽/街具、歩ける通路、独立collision。 **.blend + .glb + 4方向実レンダー＋短い移動動画**。過去の箱建物・道路ばかり・矩形緑地は禁止。町全体や他の街はまだ作らない。
- [ ] **Unity vs Godot同一モデル実比較**：同じ2つのGLB・同一画角・カメラ距離・同一操作・ライティングで実インポート。WASD/矢印歩行、アニメ/衝突/カメラ、ビルドサイズ/実FPS/Load等を計測。Web公開は承認があるまで禁止。どちらかを先に勝者と宣言しない。

### 絶対の承認・安全ゲート
1. 既存 Issue #37 comment5945834235/5954806491、FirstTown失敗ポストモーテム、visual review gateと現在のgame canonをfresh-read。
2. 初回はBlender実行可否・使用可能GPU/空きストレージ/認可済Runnerを読み取り専用確認。前面操作/RDC/DC/Tailscale/新たな従量課金/API使用/未承認公開一切禁止。
3. technical PASS と artistic VISUAL PASS を別に記録。実際の画像/動画を**開いて**現物の旧失敗との差を評価。失敗作品を次のユーザー向け候補として出すな。
4. GitHub issueの進捗だけで完成と言わない。実.blend/glb/MP4/スクリーンショット/ライセンス/sha256/実ゲーム入力ログを保護保存し、Inspector & Runtimeの読戻しを得る。ユーザーの最終承認は別。
5. **Music-only Netlify/60曲/Stripe/法務/現行HPと停止済旧HPは触らない**。既存ゲームRPG internalの実装とUnity sourceを破壊・一括置換しない。

**直ちに提出すべき最初の実成果**：character.blendとcharacter.glb＋正面/背面/斜めPNGの実byte/SHA、実生成runと失敗なら具体的修正；次が歩行、町の一角、Unity/Godot比較。提案書の追加だけで数日停滞せず作ること。

**最初の状態（Issue起票時）：** 詳細仕様はGitHubに保存済み、**実Blender造形0件、実Godot比較0件、AgentHubの実CLAIM/ACK未確認**。実証のないものは完成と扱わない。

References: #37 / #3; AgentHub DeeDee-Lab/TECBUILD-Agent-Hub#239・#177。

~~~~~
### END ISSUE47_ORIGINAL_BODY

### BEGIN ISSUE37_ORIGINAL_BODY
ORIGINAL_URL https://github.com/DeeDee-Lab/Dolzore/issues/37
CHARACTER_LENGTH 769
~~~~~text
# Goal
Create one actual Unity character that the user can control directly in WebGL.

# Acceptance
- Must be a real Unity scene/build, not a generated still image.
- Character is built from Unity meshes/materials, not a pasted character screenshot.
- WASD and arrow-key walking.
- Visible walk animation while moving.
- Camera follows the character.
- WebGL build opens directly into the walk demo.
- Publish to a URL the user can operate in-browser.
- Source and build lineage preserved in GitHub.

# Visual target
Original DOLZORE chibi fantasy mage: oversized head, compact body, blonde hair, blue/white/gold outfit, staff/crystal accent. Do not copy Hero Wars assets/characters.

# PDCA
Record build failures, fixes, receipts, published URL, and final commit here.
~~~~~
### END ISSUE37_ORIGINAL_BODY

### BEGIN ISSUE37_COMMENT_5943990146
ORIGINAL_URL https://github.com/DeeDee-Lab/Dolzore/issues/37#issuecomment-5943990146
CHARACTER_LENGTH 661
~~~~~text
## PDCA checkpoint — run #1

- run: https://github.com/DeeDee-Lab/Dolzore/actions/runs/36951893626
- head: `ea7287a3c2e34735c30bd52b8062264de187dd70`
- result: FAILURE
- jobs created: 0
- Unity compile started: false
- character build started: false

Diagnosis: failure occurred at workflow definition/startup level, before a runner job or Unity process existed. Therefore this is not a Unity-character implementation failure.

Action: reduce the workflow to a minimal runner/Unity build lane first, validate that GitHub creates a job, then re-add publish/PDCA steps after the Unity build itself is proven. No character source is being discarded or regenerated.
~~~~~
### END ISSUE37_COMMENT_5943990146

### BEGIN ISSUE37_COMMENT_5944224938
ORIGINAL_URL https://github.com/DeeDee-Lab/Dolzore/issues/37#issuecomment-5944224938
CHARACTER_LENGTH 862
~~~~~text
## PDCA checkpoint — browser movement validation

### PASS
- Unity 6000.5.1f1 project creation: PASS
- Unity compilation: PASS
- Unity WebGL build: PASS
- acceptance receipt: PASS
- 3D procedural character render in a live browser: PASS
- no fatal WebGL errors: PASS

### NOT PASS YET
Automated browser validation could not make WASD / Arrow input visibly move the character, even after adding canvas-focus handling.

This is not being accepted as complete.

### Action
Add a second, browser-independent control lane:
- visible HTML D-pad buttons outside the WebGL canvas;
- button pointer input calls Unity C# via WebGL SendMessage;
- movement still occurs inside the actual Unity CharacterController;
- keyboard input remains available;
- rebuild and revalidate movement visually.

Current playable-build claim remains **NOT FINAL** until movement is verified.
~~~~~
### END ISSUE37_COMMENT_5944224938

### BEGIN ISSUE37_COMMENT_5944786449
ORIGINAL_URL https://github.com/DeeDee-Lab/Dolzore/issues/37#issuecomment-5944786449
CHARACTER_LENGTH 1771
~~~~~text
## 2026-10-02 JST — playable local Unity WebGL package checkpoint

### P
User requested an actual Unity character they can operate, with walking only.

### D
- Reused accepted Unity 6 WebGL build run #36953875618 (SUCCESS).
- Character is `Lunaria Walkable Character`, built from Unity meshes/materials; no character image texture.
- Movement remains inside Unity `CharacterController` via `HeroStyleWalkCharacter.cs`.
- Browser D-pad calls Unity C# through WebGL `SendMessage`:
  - `SetExternalInput`
  - `ClearExternalInput`
  - `Nudge`
- PR #41 merged the D-pad-enabled published artifact to main: `57ea121e268ceec5cb333c107f8db0f57e03bd44`.
- GitHub Pages was NOT re-enabled because main intentionally contains the Public Site Closure Guard.
- Created a standalone local-run package with a PowerShell HTTP server so the user can launch the actual WebGL build without installing Unity.

### C
Package contains:
- WalkDemo.wasm ~41 MB
- WalkDemo.data ~8.6 MB
- WalkDemo.framework.js
- WalkDemo.loader.js
- patched index.html with browser D-pad
- START_DEMO.bat
- serve.ps1 with correct `application/wasm` MIME
- Japanese instructions

ZIP SHA-256:
`dd03fde624024a8a5acc0dc8114474a84dba6ce44a49632deca727f6f9d2f6bd`

### Incident / correction
An attempt to move D-pad injection into `WalkDemoBuilder.cs` introduced C# string escaping compile errors in run #36956538619. That source change was reverted to the last compile-green builder in commit `17720601feed408f5c5b5c7df6be65cfecfd4103`. The working D-pad remains in the publish/package layer and calls the already compiled Unity C# external-input methods.

### A
User can now test walking locally by extracting the ZIP and double-clicking `START_DEMO.bat`. Public hosting remains separate from the closed-site policy.
~~~~~
### END ISSUE37_COMMENT_5944786449

### BEGIN ISSUE37_COMMENT_5945834235
ORIGINAL_URL https://github.com/DeeDee-Lab/Dolzore/issues/37#issuecomment-5945834235
CHARACTER_LENGTH 1954
~~~~~text
## 2026-10-02 JST — USER VISUAL REJECTION / quality reset

User tested the actual Unity WebGL walk demo and rejected the visual quality as far too low.

### What passed
- actual Unity runtime
- WebGL launch
- live movement
- CharacterController
- browser controls

### What failed
The character does NOT meet the requested visual quality target represented by the Hero Wars reference.

Observed causes:
- primitive/procedural geometry is visibly prototype-grade;
- no authored high-quality face illustration/texture treatment;
- hair is built from coarse primitive masses instead of layered designed strands;
- costume lacks detailed silhouette, ornament, fabric treatment, filigree and readable accessory hierarchy;
- Standard-material lighting is too flat and toy-like;
- proportions/silhouette do not match the appealing 2.5–3-head chibi target;
- gait is transform-driven proof animation, not polished authored skeletal animation;
- secondary motion for hair/clothing/accessories is insufficient;
- environment presentation further exposes the prototype look.

### Acceptance reset
VISUAL_ACCEPTANCE=false
TECHNICAL_MOVEMENT_PROOF=true

Do NOT present the current procedural-primitive character as the target DOLZORE production-quality avatar.

### Next production direction
For the Hero-Wars-like quality target, use a high-quality original DOLZORE character art pipeline inside Unity:
1. original character master art;
2. production part separation (face/hair/front-back/torso/arms/legs/accessories/weapon);
3. skeletal rig / mesh deformation;
4. authored idle + walk cycles;
5. layered hair/clothing secondary motion;
6. Unity shader/VFX integration;
7. build and browser-test the real rigged character.

Preferred visual lane: premium 2D/2.5D skeletal character in Unity rather than primitive 3D geometry, unless an authored high-quality 3D model is available.

The next demo must be rejected automatically if it is only primitive-mesh proof art.
~~~~~
### END ISSUE37_COMMENT_5945834235

### BEGIN ISSUE37_COMMENT_5954806491
ORIGINAL_URL https://github.com/DeeDee-Lab/Dolzore/issues/37#issuecomment-5954806491
CHARACTER_LENGTH 807
~~~~~text
## 2026-10-02 23:36 JST — fresh quality-status audit

Fresh-checked Issue #37, `feature/unity-walkable-chibi`, and the newer `rebuild/firsttown-ff11-fantasy-20261002` successful run #37017800701.

### Result
HIGH_QUALITY_CHARACTER_COMPLETE=false
VISUAL_ACCEPTANCE=false
TECHNICAL_MOVEMENT_PROOF=true

The newer FirstTown run successfully adds animated KayKit Adventurers and denser fantasy-town assets, but its preview still shows low-poly/stylized game-kit characters. It does **not** meet the user's Hero-Wars-level character-quality target and must not be substituted for the rejected walk-demo character.

No production-quality 2D/2.5D skeletal DOLZORE character with authored face/hair/costume, rigged walk cycle, secondary motion, and Unity integration has been completed yet.

Issue #37 remains OPEN.
~~~~~
### END ISSUE37_COMMENT_5954806491

### BEGIN ISSUE37_COMMENT_6427588400
ORIGINAL_URL https://github.com/DeeDee-Lab/Dolzore/issues/37#issuecomment-6427588400
CHARACTER_LENGTH 1631
~~~~~text
<!-- DOLZORE_BLENDER_CHARACTER_TOWN_PILOT_OWNER_HANDOFF_20261027 -->

## 【新しい直接ユーザー指示】Blender造形で品質不合格を突破、現行Unityは保護

原文：
> 以前ゲームを作成するプロジェクトがありました。。unityを利用していたのですが、全くうまく行きませんでした。blenderだとどうですか？
> それで担当に指示だして

**新規制作Issue（実作業owner pointer）**：https://github.com/DeeDee-Lab/Dolzore/issues/47

**原文と工法・全作業・失敗原因・素材・実データ品質・A/B・署名ログの9,689文字GitHub正本**：https://github.com/DeeDee-Lab/Dolzore/blob/main/design/2026-10-27_BLENDER_CHARACTER_TOWN_ART_PIPELINE_DIRECT_EXECUTION.md

既存 #37 comment 5945834235 / 5954806491 の user rejected character（primitive, hair, face, costume, no skeletal walk）と FirstTown V7 postmortem（geometry-first、道路偏重、低密度、見た目を見ないまま技術合格）を前提に、Blenderで**独自の高品質2.5〜3頭身キャラクター1体＋Central Main Street BARの街1区画**を制作。rig Idle/Walk/Turn、blend+GLB、使用許諾、PNG前横後、歩行動画を実生成。次に**同一GLB/同一カメラ/同一操作で既存Unity vs Godotの実動作**を比較。Godotへの全面移行は未承認。先に元Unity資産や既存RPG rulesを上書き・削除しない。

作業割当は現在の既存AgentHub 5席内（Owner、Builder A/B、Inspector、Runtime）。OwnerがTask/Lease/Attemptを実割当・実ACK読戻し、Builder Aキャラクター、Builder B街、Inspector本物実レンダー/実動画の見た目照合、Runtime実GLB import・入力・Webまたはlocal acceptance。**Issueへの指示は実AgentHub ACKではない**。最初にcharacter.blend/glb+前斜背3レンダーの実SHAを提出し、作業開始を数日計画文書だけでごまかさない。

2度続く同原因の不合格は造形・構図の制作方法を変え、旧primitive/矩形道路に飾りを足す方法を繰返すな。スクリーン実見を通過したときだけユーザーへ提示、本人最終受入は別。

**絶対保護**：現在の音楽販売Netlify/60曲/Stripe、閉鎖済旧HP、GitHub mainのRPG/Unity実物、PC前面、DC/RDC/Tailscale、無断新従量課金、無断第6席。Web公開・旧HP再開なし。原本はGitHub-first。実ブランチ/Run/SHA/遊べる成果物を確認してから完了報告。

CURRENT CHECKPOINT: Instruction issued & GitHub source file read back, new work issue created. REAL_BLENDER_BLEND=0, REAL_GLB=0, REAL_ENGINE_A_B=0, PG_CLAIM_ACK=UNVERIFIED. Do not confuse this with end-to-end execution.
~~~~~
### END ISSUE37_COMMENT_6427588400

### BEGIN ISSUE37_COMMENT_6429780647
ORIGINAL_URL https://github.com/DeeDee-Lab/Dolzore/issues/37#issuecomment-6429780647
CHARACTER_LENGTH 1843
~~~~~text
<!-- DOLZORE_BLENDER_STYLE_CONFLICT_6_5_TO_7_5_HEAD_20261027 -->

## 【既存Issue#47のP0正本補正／最優先の造形条件】

直接のユーザー原文（省略なし）：
> 以前ゲームを作成するプロジェクトがありました。。unityを利用していたのですが、全くうまく行きませんでした。blenderだとどうですか？
> それで担当に指示だして
> 1分以上の思考の場合は、タイムアウト対策で常時全履歴AgentHub(GD含む)に保存。思考の巻き戻り防止。

既存Issue #47の正式9,689文字原文は維持。ただし**「2.5〜3頭身の本番用主人公を制作」という部分は、2026-10-03の最新DOLZORE街全体に対する直接ユーザー却下 https://github.com/DeeDee-Lab/Dolzore/issues/3#issuecomment-5973706588 （Chibi 3頭身×半写実街のミスマッチ → 製品用人物6.5〜7.5頭身 + 統一stylized-realisticへ変更）と矛盾**。ユーザーの今回の「Blenderで」の指示は体型の再変更を明示していない。

**当該キャラクター製品制作は6.5〜7.5頭身で製作し、街区も一貫したFF11的stylized-realistic材質/光を合わせる。** Hero Wars風2.5〜3頭身版の試作は別の作風実験として明記する場合のみ可。本体への無断採用なし。この是正は元のIssue#47/9,689文字歴史原本を書換えない補足（後から担当が必ず読める最新版）。

**最新の5167文字の、省略禁止・タスク・素材・QA・GD保存・Runner安全条件・次再開地点の詳細補足**： https://github.com/DeeDee-Lab/Dolzore/blob/main/design/2026-10-27_BLENDER_PRODUCTION_OWNER_CORRECTION_STYLE_AUTHORITY_AND_RESTART_CHECKPOINT.md

同時にAgentHub#20現行本文ではActions禁止、Dolzore mainの`policy(ci): stop prohibited OSK222 self-hosted runner [skip ci]` commit e37c8469c03ab0e444947efde4da20a76664c61b が存在。古いOSK222 Runnerと過去CI-successの再利用許可を勝手に仮定しない。必ず実current owner authorizationを検証し、未確認なら実行禁止。DC/RDC/Codexコード改修の無断代理/PC前面/新高額課金/既存Netlify音楽HP改変は禁止。

Builder Aは**実 .blend/.glbと前3/4背PNG SHA → Armature+Idle/Walk/Turn MP4**、Builder Bは**BAR街区1区画の実.blend/.glb/4方位レンダー/撮影動画**、Inspectorは実画像を開いて実際に同じ世界観と旧FAIL差を独立判定、Runtimeは同じGLBをUnityとGodotに実インポートして入力/カメラ/衝突/実FPSを比較。**技術PASS≠見た目PASS≠ユーザー合格**。

GitHub Issueへ指示を出したことは真のAgentHub PG Task/Lease/Attempt受領ACKではない。現状 `REAL_BLENDER_ARTIFACTS=0_VERIFIED; ACTUAL_AGENT_CLAIM_ACK=UNVERIFIED; PUBLIC_RELEASE_NOT_AUTHORIZED=true`。5席内でOwnerが実dispatch→CLAIM/ACKをreadbackし、実ファイルSHA/検査ログだけ完成計上する。

長時間思考が必要なら当該指示原文・Issue#47起票全文・既存9,689文字原本・実際の不合格原コメント・今回の補足原本・Source SHA・PDCAを毎checkpointでAgentHub(GDを含む)原文全文として追記保存、保存後の読戻しを行うこと。過去履歴の要約による代用不可。
~~~~~
### END ISSUE37_COMMENT_6429780647

### BEGIN ISSUE3_COMMENT_5973706588
ORIGINAL_URL https://github.com/DeeDee-Lab/Dolzore/issues/3#issuecomment-5973706588
CHARACTER_LENGTH 1277
~~~~~text
## 2026-10-03 — Public 3D art-direction rejection

User rejected the current public 3D FirstTown for a specific coherence failure:

- character is around 3-head/chibi proportion but lacks deliberate cute/chibi facial and body language;
- town is semi-realistic medieval/fantasy in composition but still simplified/low-poly in asset treatment;
- the combination creates an uncanny middle state: neither cute stylized nor FF11/FF14-like stylized-realistic.

This is not a density problem and not fixable by adding more props.

Root correction:
1. remove chibi-proportion characters from the production/public visual target;
2. target roughly 6.5–7.5-head humanoid proportions for player/NPC baseline;
3. use one coherent stylized-realistic material/lighting language across character + architecture;
4. keep FF11 efficiency principles (low geometry, reuse, texture-driven detail) without copying FF11 assets;
5. do not continue polishing the current mismatched art direction;
6. current public preview must not be promoted to the main HP.

CURRENT_PUBLIC_3D_VISUAL_REJECTED=true
ART_DIRECTION_MISMATCH_CONFIRMED=true
TARGET_STYLE=FF11_LIKE_STYLIZED_REALISTIC
CHIBI_CHARACTER_BASELINE_REJECTED=true
MAIN_HP_PROMOTION_BLOCKED=true
FIRST_TOWN_COMPLETE=false
NO_FALSE_COMPLETION=true
~~~~~
### END ISSUE3_COMMENT_5973706588

### BEGIN ISSUE3_COMMENT_5979628527
ORIGINAL_URL https://github.com/DeeDee-Lab/Dolzore/issues/3#issuecomment-5979628527
CHARACTER_LENGTH 820
~~~~~text
2026-10-04 DIRECT USER CORRECTION — PUBLIC HP AUTHORITY

The URL `https://dolzore-web-runtime-production.up.railway.app/` is CLOSED/RETIRED. It is NOT the current DOLZORE HP and must never be returned as the current HP.

Previous canonical documents incorrectly still labeled it as the current/verified replacement URL. Those records are stale historical evidence and have now been overridden.

Current rule:
- CURRENT_CANONICAL_HP_URL=NONE
- RETIRED_RUNTIME_HP_URL=https://dolzore-web-runtime-production.up.railway.app/
- RETIRED_URL_MUST_NOT_BE_PRESENTED_AS_CURRENT=true
- NEW_HP_REQUIRES_FRESH_PUBLIC_VERIFICATION=true
- NEW_HP_REQUIRES_DIRECT_USER_APPROVAL=true

AgentHub portfolio authority for `dolzore-game` points to Issue #3, so successors must consume this correction before answering HP/publication questions.
~~~~~
### END ISSUE3_COMMENT_5979628527

### BEGIN ISSUE3_COMMENT_6005219155
ORIGINAL_URL https://github.com/DeeDee-Lab/Dolzore/issues/3#issuecomment-6005219155
CHARACTER_LENGTH 916
~~~~~text
PUBLIC HP RETIREMENT RUNTIME CHECK — 2026-10-06 JST

Direct-user authority remains: the former Railway DOLZORE HP must be closed and must not be presented as current.

Fresh Railway readback:
- service `dolzore-web-runtime` production: serviceDomains=[] / customDomains=[]
- service `dolzore-web-preview` production: serviceDomains=[] / customDomains=[]
- former `dolzore-web-runtime-production.up.railway.app` route was removed
- former preview Railway public route was also removed

Therefore the retired Railway web services currently have zero public domains. Reachability/existence of the underlying service must not be confused with current public-HP authority.

CURRENT_CANONICAL_HP_URL=NONE
RETIRED_RAILWAY_PUBLIC_DOMAINS_REMOVED=true
PUBLIC_DOMAIN_COUNT_RUNTIME=0
PUBLIC_DOMAIN_COUNT_PREVIEW=0
NEW_HP_REQUIRES_FRESH_PUBLIC_VERIFICATION=true
NEW_HP_REQUIRES_DIRECT_USER_APPROVAL=true
NO_STALE_URL_REPLAY=true
~~~~~
### END ISSUE3_COMMENT_6005219155

### BEGIN ISSUE3_COMMENT_5968473713
ORIGINAL_URL https://github.com/DeeDee-Lab/Dolzore/issues/3#issuecomment-5968473713
CHARACTER_LENGTH 1489
~~~~~text
2026-10-03 PUBLIC WRONG-SCENE INCIDENT — root cause and fix

User-visible public URL displayed the legacy 2D FirstTown instead of the intended FF11-style 3D FirstTown.

Root cause confirmed in ProjectBootstrap.BuildWebGL(): public WebGL scene list was [Title.unity, FirstTown3D.unity, FirstTownShell.unity]. Scene 0 was Title, whose flow could enter the legacy 2D town. The public payload itself was WebGL-capable, but the startup scene contract was wrong.

Fix applied on publish/firsttown-3d-20261003:
- public BuildWebGL scene list is now EXACTLY [Assets/Scenes/FirstTown3D.unity]
- legacy Title and FirstTownShell are excluded from the public release build
- build logs now assert:
  DOLZORE_PUBLIC_START_SCENE=Assets/Scenes/FirstTown3D.unity
  DOLZORE_PUBLIC_SCENE_COUNT=1
- workflow run 37117424828 = SUCCESS
- public payload verification: DOLZORE_PUBLIC_BUILD_FILES=4 and DOLZORE_PUBLIC_PAYLOAD=READY
- persisted release branch head: cf225ba2bbd946d6ec2a0da630abf3255d59c18d
- Railway deployment 8f7310aa-0824-4c5c-a661-c2183948c4ad = SUCCESS from commit cf225...
- main HP runtime remains untouched.

Anti-repeat rule:
A public build is not accepted merely because WebGL loads. Release builds must assert startup scene identity and scene count. Legacy scenes must not be packaged in the public 3D release candidate.

PUBLIC_WRONG_SCENE_ROOT_CAUSE_FIXED=true
PUBLIC_START_SCENE_FIRSTTOWN3D=true
PUBLIC_SCENE_COUNT=1
LEGACY_2D_EXCLUDED_FROM_PUBLIC_BUILD=true
NO_FALSE_COMPLETION=true
~~~~~
### END ISSUE3_COMMENT_5968473713

### BEGIN ISSUE3_COMMENT_5954379687
ORIGINAL_URL https://github.com/DeeDee-Lab/Dolzore/issues/3#issuecomment-5954379687
CHARACTER_LENGTH 1796
~~~~~text
## 2026-10-02 — FirstTown FF11-style quality polish checkpoint

Fresh manual review performed on the current real Unity screenshot from branch:
rebuild/firsttown-ff11-fantasy-20261002

Implemented in this polish lane:
- replaced modern/T-pose avatars with fantasy adventurer characters;
- added idle/run animator pipeline;
- narrowed roads and added pedestrian edge strips;
- lowered the raised ward wall and tightened street enclosure;
- replaced giant blank castle slabs with a detailed arch/pillar gate treatment;
- added denser lightweight street dressing: stalls, lamps, hedges, fences, banners, rocks;
- explicitly bound source atlas textures to external fantasy assets;
- added barrels, crates, sacks, weapon rack, wheelbarrow and other everyday props;
- added distant hills/mountains/trees for world depth;
- moved player/camera spawn deeper into the dense district;
- added more NPCs and more nearby buildings.

Current fresh visual result:
- materially better than the rejected industrial demo and first fantasy pass;
- T-pose problem removed;
- fantasy-town identity is visible;
- street enclosure and foreground density are improved;
- still not final production quality.

Remaining visible gaps:
- building kit still reads somewhat toy-like/CC0;
- facade/material complexity remains below FF11/FF14 target;
- plaza and streets need more surface variation/decal detail;
- NPC life/animation variety remains limited;
- vegetation and signage need more authored identity;
- no final public visual acceptance yet.

Current run 37017800701:
- 3D generation PASS
- preview verification PASS
- preview artifact uploaded
- WebGL build still in progress at checkpoint

FIRST_TOWN_FF11_POLISH_ACTIVE=true
FIRST_TOWN_3D_VISUAL_ACCEPTANCE=false
FIRST_TOWN_COMPLETE=false
NO_FALSE_COMPLETION=true
~~~~~
### END ISSUE3_COMMENT_5954379687

### BEGIN ISSUE47_COMMENT_6429779788
ORIGINAL_URL https://github.com/DeeDee-Lab/Dolzore/issues/47#issuecomment-6429779788
CHARACTER_LENGTH 1843
~~~~~text
<!-- DOLZORE_BLENDER_STYLE_CONFLICT_6_5_TO_7_5_HEAD_20261027 -->

## 【既存Issue#47のP0正本補正／最優先の造形条件】

直接のユーザー原文（省略なし）：
> 以前ゲームを作成するプロジェクトがありました。。unityを利用していたのですが、全くうまく行きませんでした。blenderだとどうですか？
> それで担当に指示だして
> 1分以上の思考の場合は、タイムアウト対策で常時全履歴AgentHub(GD含む)に保存。思考の巻き戻り防止。

既存Issue #47の正式9,689文字原文は維持。ただし**「2.5〜3頭身の本番用主人公を制作」という部分は、2026-10-03の最新DOLZORE街全体に対する直接ユーザー却下 https://github.com/DeeDee-Lab/Dolzore/issues/3#issuecomment-5973706588 （Chibi 3頭身×半写実街のミスマッチ → 製品用人物6.5〜7.5頭身 + 統一stylized-realisticへ変更）と矛盾**。ユーザーの今回の「Blenderで」の指示は体型の再変更を明示していない。

**当該キャラクター製品制作は6.5〜7.5頭身で製作し、街区も一貫したFF11的stylized-realistic材質/光を合わせる。** Hero Wars風2.5〜3頭身版の試作は別の作風実験として明記する場合のみ可。本体への無断採用なし。この是正は元のIssue#47/9,689文字歴史原本を書換えない補足（後から担当が必ず読める最新版）。

**最新の5167文字の、省略禁止・タスク・素材・QA・GD保存・Runner安全条件・次再開地点の詳細補足**： https://github.com/DeeDee-Lab/Dolzore/blob/main/design/2026-10-27_BLENDER_PRODUCTION_OWNER_CORRECTION_STYLE_AUTHORITY_AND_RESTART_CHECKPOINT.md

同時にAgentHub#20現行本文ではActions禁止、Dolzore mainの`policy(ci): stop prohibited OSK222 self-hosted runner [skip ci]` commit e37c8469c03ab0e444947efde4da20a76664c61b が存在。古いOSK222 Runnerと過去CI-successの再利用許可を勝手に仮定しない。必ず実current owner authorizationを検証し、未確認なら実行禁止。DC/RDC/Codexコード改修の無断代理/PC前面/新高額課金/既存Netlify音楽HP改変は禁止。

Builder Aは**実 .blend/.glbと前3/4背PNG SHA → Armature+Idle/Walk/Turn MP4**、Builder Bは**BAR街区1区画の実.blend/.glb/4方位レンダー/撮影動画**、Inspectorは実画像を開いて実際に同じ世界観と旧FAIL差を独立判定、Runtimeは同じGLBをUnityとGodotに実インポートして入力/カメラ/衝突/実FPSを比較。**技術PASS≠見た目PASS≠ユーザー合格**。

GitHub Issueへ指示を出したことは真のAgentHub PG Task/Lease/Attempt受領ACKではない。現状 `REAL_BLENDER_ARTIFACTS=0_VERIFIED; ACTUAL_AGENT_CLAIM_ACK=UNVERIFIED; PUBLIC_RELEASE_NOT_AUTHORIZED=true`。5席内でOwnerが実dispatch→CLAIM/ACKをreadbackし、実ファイルSHA/検査ログだけ完成計上する。

長時間思考が必要なら当該指示原文・Issue#47起票全文・既存9,689文字原本・実際の不合格原コメント・今回の補足原本・Source SHA・PDCAを毎checkpointでAgentHub(GDを含む)原文全文として追記保存、保存後の読戻しを行うこと。過去履歴の要約による代用不可。
~~~~~
### END ISSUE47_COMMENT_6429779788

### BEGIN ISSUE47_COMMENT_6429834227
ORIGINAL_URL https://github.com/DeeDee-Lab/Dolzore/issues/47#issuecomment-6429834227
CHARACTER_LENGTH 1237
~~~~~text
<!-- DOLZORE_GAME_BLENDER_GD_ARCHIVE_READBACK_20261027_0824 -->

## 原文全文・思考巻戻り防止の永続引継ぎレシート（2026-10-27 JST）

ユーザーの直接命令「1分以上の思考の場合は、タイムアウト対策で常時全履歴AgentHub(GD含む)に保存。思考の巻き戻り防止。」を**原文のまま保存**。

GitHub original source: 9,689文字の制作仕様 + 5,167文字の体型衝突是正文書 + Issue #47元本文2,105文字。関連Issue #37/#3/#47の10件元コメントを**全文**含み、13原文ブロックでGoogle Docsに保存（追加パラグラフによりDoc本体35,421文字）。

GD保存先（readback URL）: https://docs.google.com/document/d/1Kol39aFO4ds33sd0TU3Bkppgg_KZkpi5NlBrUHOHIoQ/edit

**読戻しで start/end 13件/13件、主原文3本の改行以外完全一致、原文ユーザーメッセージ及びスタイル是正を確認。** GitHub正本の元文字列は上書き・圧縮していない。Google Docsは空行を自動追加するため逐字・改行の最終正本はGitHub。

**AgentHub側の別書込みは、安全性チェックにより未達**：`TECBUILD-Agent-Hub#239`, `#177` のIssueコメント、`history/dolzore/...md` 新規作成はコネクタ保護機構により拒否。これらは `AGENTHUB_GITHUB_ARCHIVE=BLOCKED` と記録し、実保存成功を偽称しない。OwnerはIssue#47と前記Google Docsにある実原文全ブロックを読み、認可されたAgentHub記録経路で安全に索引・保存を進める。保護チェックを回避する書込みや別権限への勝手な変更は禁止。

**最優先次アクション**：Ownerは現行5席で実Task/Lease/Attempt/ACKを取得→安全に承認済Blender環境へ制作割当→6.5〜7.5頭身の実character.blend/glb正面/斜背レンダー＋歩行実動画→BAR一区画blend/glb＋歩行demo→同じ資産をUnity/Godot比較→現物視覚合格。元Unity/Netlify音楽販売/Stripe/ゲーム本番は触らない。

最新区分 `GITHUB_DOLZORE_DIRECTIVE=PASS; GD_FULL_ARCHIVE=READBACK_PASS; AGENTHUB_WRITE=BLOCKED; PG_AGENT_CLAIM_ACK=UNVERIFIED; NEW_REAL_3D_MEDIA=0_VERIFIED`。
~~~~~
### END ISSUE47_COMMENT_6429834227

### BEGIN VISUAL_REVIEW_GATE_FILE
ORIGINAL_URL https://github.com/DeeDee-Lab/Dolzore/blob/main/design/USER_VISIBLE_VISUAL_REVIEW_GATE.md
CHARACTER_LENGTH 4491
~~~~~text
# USER-VISIBLE VISUAL REVIEW GATE — MANDATORY

Authority: direct user correction, 2026-10-01 JST
Scope: all DOLZORE user-visible visual artifacts, including FirstTown, avatars, UI, interiors, maps, battle screens and promotional screenshots.
Status: HARD GATE

## 0. Rule

NO visual artifact may be presented to the user as a candidate, improvement, completion, preview, or "current result" merely because:
- Unity build succeeds;
- route/collision tests pass;
- WebGL builds;
- automated image metrics pass;
- source compiles;
- an agent claims it looks better.

Before user-visible presentation, the responsible agent/assistant MUST open the actual generated image/artifact bytes and visually inspect them.

## 1. Required order

1. Generate artifact.
2. Obtain exact artifact from the successful run/build.
3. Open the actual screenshot/image.
4. Open/read the canonical original-reference evidence required by the task.
5. Perform direct visual comparison.
6. Record concrete mismatches.
7. Decide PASS or FAIL.
8. If FAIL:
   - do not present it as a candidate;
   - return to implementation;
   - preserve failure evidence/checkpoint;
   - only show it to the user if the user explicitly asks to see rejected/debug output.
9. If PASS:
   - persist a visual-review receipt;
   - only then may it be presented as a candidate for user acceptance.

TECHNICAL_PASS != VISUAL_PASS
VISUAL_PASS != USER_ACCEPTED

## 2. FirstTown mandatory references

Before FirstTown visual review:
- research/mother2/DIRECT_VISUAL_EVIDENCE_PACKET_V1.md
- research/mother2/GAMEPEDIA_TOWN_IMAGE_SOURCE_MANIFEST_V1.json
- research/mother2/FIRST_TOWN_AUTHORING_CONTRACT_V1.json
- design/AVATAR_DRESSUP_SYSTEM_V2.md
- newest explicit user visual corrections
- actual generated FirstTown screenshot

Original reference images/source URLs are authoritative where available. Summaries alone are insufficient.

## 3. FirstTown review dimensions

The reviewer must explicitly inspect:
- road/street visual share;
- building density;
- building vertical mass;
- facade/detail rhythm;
- green/vegetation integration;
- empty-space ratio;
- diagonal/sloped edge variety;
- NPC density and clustering;
- character-to-building/door scale;
- character silhouette/appeal;
- crosswalk/road-marking alignment;
- composition focal hierarchy;
- whether the frame reads first as a town rather than a road/blockout;
- whether visual quality is materially improved over the previous rejected version.

A regression in any major dimension blocks presentation.

## 4. Required visual-review receipt

Persist a machine-readable receipt containing:
- artifact_path
- artifact_sha256 or workflow artifact digest
- source_commit
- workflow_run_id
- reference_packet_ids
- actual_image_opened = true
- reviewer
- review_timestamp
- mismatch_list[]
- regression_vs_previous
- decision = PASS|FAIL
- user_visible_ready = true|false
- user_accepted = false until explicit user acceptance

No receipt means no user-visible candidate.

## 5. Anti-loop

If two consecutive versions fail for the same dominant visual reason, do not make a third small positional/color patch.
Change the production method or base art/layout architecture.

For FirstTown specifically:
- stop stacking V3/V6/V7-style corrective layers when the base composition is wrong;
- do not optimize only for collision and route validity;
- rebuild visual composition from an authored town plan/reference comparison when required.

## 6. Reporting

When a build is technically green but visually failed, report internally:
TECHNICAL_PASS=true
VISUAL_PASS=false
USER_VISIBLE_READY=false

Do not send the failed screenshot to the user as a candidate.

## 7. Current V7 ruling

Artifact:
Unity run 36723025197
source commit 093f3b0330ac9577640f3cd3506b7b99ae906206
generated head 6133014139a7a945ba372e9385de306030d67286

Technical result:
PASS

Visual review:
FAIL

Reasons:
- road/intersection still dominates the frame;
- building density and continuity remain too low;
- repeated simple detached-building grammar;
- insufficient vertical/facade mass;
- large rectangular green/empty areas;
- NPCs remain sparse/placed rather than socially clustered;
- characters remain weak relative to desired world quality;
- composition reads as a test/blockout map, not a convincing authored town;
- overall visual result does not meet the user's stated reference/quality bar.

Therefore:
USER_VISIBLE_READY=false
USER_ACCEPTED=false

This V7 image is retained only as rejected evidence.

~~~~~
### END VISUAL_REVIEW_GATE_FILE

### BEGIN V7_POSTMORTEM_FILE
ORIGINAL_URL https://github.com/DeeDee-Lab/Dolzore/blob/main/design/FIRSTTOWN_V7_POSTMORTEM_20261001.md
CHARACTER_LENGTH 4731
~~~~~text
# FIRSTTOWN V7 POSTMORTEM — 2026-10-01

## Incident

The user explicitly required that FirstTown be visually checked before being shown.
Canonical checkpoint already stated:
1. if build PASS, inspect generated screenshot manually;
2. compare against MOTHER2 Direct Visual Evidence + original references;
3. do not claim visual acceptance until the screenshot itself is acceptable.

The implementation/run passed technically, but the mandatory manual visual gate was not completed before the output was surfaced to the user.

This is a process violation.

## Root causes

### R1 — Technical acceptance was stronger than visual acceptance in the execution path

Automated verifiers check:
- route reachability;
- colliders;
- building overlap;
- road marking placement;
- HUD presence/binding;
- build/WebGL success.

They do not fail on:
- unattractive composition;
- road dominance;
- weak architecture;
- character appeal;
- mismatch to reference density;
- poor town atmosphere.

Therefore a bad-looking scene can be fully green.

### R2 — Patch-stack architecture

FirstTownBuilder applies:
- FirstTownVisualEnhancer
- FirstTownVisualRebuildV3
- FirstTownVisualRebuildV6
- FirstTownVisualRebuildV7

This is iterative correction on an old generated base rather than clean visual reconstruction from the latest evidence.

Consequence:
old assumptions survive and each version rearranges symptoms.

### R3 — Code-first geometric generation

V7 directly constructs long rectangular roads using Fill(), places primary buildings at numeric coordinates, adds a decorative rear row, controlled trees/bushes, and moves people to fixed coordinates.

This optimizes deterministic placement and validation, not organic/authored city composition.

### R4 — Research was advisory, not executable acceptance

The repository contains strong reference research and Direct Visual Evidence.
However, the build pipeline did not require a visual-review receipt proving the generated screenshot was compared to those references.

Therefore "research exists" did not mean "research constrained output."

### R5 — Failure to obey explicit pre-presentation instruction

This is the most important process failure.
The checkpoint explicitly required manual screenshot inspection after PASS.
The screenshot was not used as a blocking gate before user-visible presentation.

The user's instruction was not ambiguous.

## Why more information made the result worse

More research produced more constraints, while the implementation remained a simple code-generated blockout.
Instead of changing the production method, the agent attempted to satisfy new constraints through:
- road resizing;
- building relocation;
- scale changes;
- decorative backdrop rows;
- controlled greenery;
- fixed NPC relocation;
- stricter collision verification.

This increased technical regularity while reducing visual naturalness.

The information itself was not the cause.
The translation pipeline from evidence -> art direction -> authored composition -> visual QA was missing.

## Corrective architecture

REFERENCE EVIDENCE
-> visual target ranges / art direction
-> authored town block plan
-> actual art/layout production
-> technical validation
-> generate screenshot
-> mandatory actual-image visual review
-> comparison receipt
-> if FAIL: iterate without user presentation
-> if PASS: user-visible candidate
-> explicit user acceptance

## Stop conditions

Do not continue V8 as another small coordinate/color patch if the underlying town still uses the same road-dominant blockout grammar.

Before next candidate:
- choose a materially different composition method;
- reduce road dominance at the planning level;
- author denser facade blocks and varied building silhouettes;
- integrate greenery at edges rather than as rectangles;
- rebuild characters/avatar presentation to meet appeal bar;
- create social NPC clusters;
- compare screenshot before presentation.

## PDCA

PLAN:
Make visual quality and reference fit a hard gate, separate from technical correctness.

DO:
Create mandatory USER_VISIBLE_VISUAL_REVIEW_GATE and mark V7 VISUAL_FAIL.

CHECK:
Confirmed prior checkpoint already contained the required manual review instruction; confirmed automated QA could all pass without visual-quality checks; confirmed V7 code is patch-stack + coordinate/block generation.

ACT:
No future FirstTown candidate may be shown without actual-image review receipt PASS. Two same-reason failures force method change, not another small patch.

## Truth

USER_INSTRUCTION_CLEAR=true
USER_INSTRUCTION_CAUSED_FAILURE=false
PROCESS_VIOLATION=true
TECHNICAL_PASS_V7=true
VISUAL_PASS_V7=false
USER_VISIBLE_READY_V7=false
V7_USER_ACCEPTED=false
SMALL_PATCH_LOOP_FORBIDDEN=true

~~~~~
### END V7_POSTMORTEM_FILE

## Continue exactly here / PDCA
P=Build one actually visually appealing character and one BAR quarter instead of all-town rebuild; D=pending approved execution resources and real creation; C=original GitHub task & reports already saved, prior failures verified; A=actual Builder/Runtime CLAIM/ACK and .blend/.glb first, not another plan. Report every new SHA/CI/art review and publish nothing rejected.

## Explicit non-completion
BLENDER_DIRECTIVE_ALREADY_ISSUED=true
HANDOFF_DELTA_USER_4_MESSAGES_CAPTURED=true
GITHUB_SOURCE_VERBATIM_COPIED=true
BLENDER_REAL_ARTIFACT_VERIFIED=false
AGENTHUB_PG_CLAIM_ACK_UNVERIFIED=true
BLENDER_GODOT_COMPARISON_VERIFIED=false
DOLZORE_NETLIFY_MUSIC_UNTOUCHED=true
PUBLIC_GAME_DEPLOY_NOT_AUTHORIZED=true