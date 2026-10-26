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
