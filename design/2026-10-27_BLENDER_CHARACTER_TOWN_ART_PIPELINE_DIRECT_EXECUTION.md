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
