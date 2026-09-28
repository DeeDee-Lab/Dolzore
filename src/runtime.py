import json, os, re, html, urllib.request
from http.server import BaseHTTPRequestHandler, ThreadingHTTPServer
from urllib.parse import urlparse

PORT = int(os.environ.get("PORT", "8080"))
SMARTBUY = os.environ.get("SMARTBUY_API_URL", "https://smartbuy-live-production.up.railway.app/api/latest")
LEGACY = "https://dolzore.lovable.app"
INSTAGRAM = "https://www.instagram.com/dolzoreofficial/"
BASE = os.path.dirname(os.path.abspath(__file__))

PRODUCTS = [
 {"name":"BenQ TK705STi","badge":"短焦点・ゲーム","line":"150インチを約2.66m。4K/60Hz 5msで、短い部屋と速いゲームの両方に強い。","joke":"壁まで近い。判断は速い。値札だけ少し遠い。","specs":["4K UHD","3000 ANSI lm","4LED","投写比0.8","4K60 5ms"],"strong":130000,"consider":145000,"src":"https://www.benq.com/ja-jp/projector/cinema/tk705sti/spec.html"},
 {"name":"BenQ TK710STi","badge":"レーザー・短焦点","line":"3200 ANSI lmのレーザー。150インチを約2.29〜2.76mで狙える上位候補。","joke":"壁まで近い。予算にはちょっと遠い。","specs":["4K UHD","3200 ANSI lm","Laser","投写比0.69–0.83","4K60 16.7ms"],"strong":160000,"consider":180000,"src":"https://www.benq.com/ja-jp/projector/gaming/tk710sti/spec.html"},
 {"name":"BenQ TK700STi","badge":"中古の本命候補","line":"生産終了後も4K・3000 ANSI lm・16.7msが効く。中古価格次第でかなり面白い。","joke":"スペックはまだ現役。ランプだけは年齢を隠せません。","specs":["4K UHD","3000 ANSI lm","Lamp","投写比0.9–1.08","4K60 16.7ms"],"strong":75000,"consider":90000,"src":"https://www.benq.com/ja-jp/projector/gaming/tk700sti/spec.html","deep":True},
 {"name":"Optoma UHD35STx","badge":"かなり短焦点","line":"短い投写距離と低遅延を狙うゲーム寄り候補。中古で8万円前後なら注目。","joke":"部屋は短くてOK。探す時間は少し長め。","specs":["4K UHD","短焦点","ゲーム向け","SmartBuy監視"],"strong":80000,"consider":100000,"src":"https://www.optoma.com/"},
 {"name":"ViewSonic X10-4K","badge":"映画・デザイン","line":"約1.77mで100型。4K LEDとHarman Kardonで、映画を気軽にまとめやすい。","joke":"映画は得意。反射神経勝負は少し休憩。","specs":["4K UHD","2400 LED lm","LED","100型 約1.77m","最大200型"],"strong":60000,"consider":65000,"src":"https://www.viewsonic.com/jp/products/projectors/X10-4K"},
 {"name":"XGIMI AURA","badge":"超短焦点・映画","line":"0.233:1の超短焦点。壁際設置で80〜150型を狙える4Kレーザー。","joke":"壁との距離はほぼゼロ。存在感はゼロではありません。","specs":["4K UHD","1800 ISO lm","Laser","投写比0.233:1","80–150型"],"strong":90000,"consider":100000,"src":"https://global.xgimi.com/products/aura"},
 {"name":"XGIMI HORIZON S Max","badge":"現行スマート4K","line":"3100 ISO lm、4K、40〜200型。自動補正を含めて設置の手間を減らしやすい。","joke":"賢いです。部屋の奥行きまでは増やしてくれません。","specs":["4K UHD","3100 ISO lm","投写比1.2:1","40–200型","Android TV"],"src":"https://jp.xgimi.com/products/horizon-s-max"},
 {"name":"JMGO N1S Ultra 4K","badge":"3色レーザー","line":"2800 ISO lmの3色レーザー。4K・最大180型で色と設置自由度を狙う。","joke":"首はよく回る。部屋の壁は動きません。","specs":["4K UHD","2800 ISO lm","3色Laser","投写比1.2:1","最大180型"],"src":"https://jmgo.jp/products/jmgo-n1s-ultra-4k"},
 {"name":"Epson EH-LS800","badge":"超短焦点・150型","line":"150型でも投写距離約54cmの超短焦点。大画面を壁際で成立させやすい。","joke":"投写距離は短い。本体の横幅は遠慮しません。","specs":["超短焦点","Laser","150型 約54cm","2.1ch speaker"],"src":"https://www2.epson.jp/support/manual/EHLS800W_USERS_GUIDE_JA_R100.PDF"},
 {"name":"BenQ X3100i","badge":"ゲーム・画質","line":"3300 ANSI lm、4LED、4K/60Hz 16.7ms。色とゲーム性能の両方を狙う。","joke":"性能は盛っています。投写距離だけは節約しません。","specs":["4K UHD","3300 ANSI lm","4LED","投写比1.15–1.50","4K60 16.7ms"],"src":"https://www.benq.com/ja-jp/projector/gaming/x3100i/spec.html"},
]

NEW_MARKETS={"amazon","rakuten","yahoo_shopping"}
USED_MARKETS={"mercari","yahoo_auction","hardoff","secondstreet","yahoo_flea","rakuma"}
BAD=("交換ランプ","互換ランプ","ランプモジュール","プロジェクター用ランプ","スタンド","スクリーン","リモコン","ケーブル","金具","部品","ジャンク","故障","投影不可","投影不良")

def e(v): return html.escape(str(v if v is not None else ""))
def yen(v):
    try: return "¥{:,.0f}".format(float(v))
    except: return "—"

def art(label, idx=0):
    colors=["#a9694b","#65737e","#8b7355","#5f7766","#7f6579","#5f6f8b"]
    c=colors[idx%len(colors)]
    return f'''<svg class="projector-art" viewBox="0 0 560 320" role="img" aria-label="{e(label)}の抽象イラスト"><rect x="24" y="28" width="512" height="264" rx="24" fill="{c}" opacity=".09"/><rect x="112" y="106" width="336" height="116" rx="22" fill="#f8f4ec" stroke="#2d2a26" stroke-width="4"/><circle cx="366" cy="164" r="38" fill="#24211e"/><circle cx="366" cy="164" r="24" fill="{c}"/><circle cx="366" cy="164" r="12" fill="#f7ede1"/><rect x="142" y="136" width="112" height="10" rx="5" fill="#2d2a26" opacity=".65"/><rect x="142" y="160" width="86" height="8" rx="4" fill="#2d2a26" opacity=".35"/><path d="M93 235h374" stroke="#2d2a26" stroke-width="4" stroke-linecap="round" opacity=".16"/></svg>'''

def layout(title, desc, body):
    return f'''<!doctype html><html lang="ja"><head><meta charset="utf-8"><meta name="viewport" content="width=device-width,initial-scale=1"><title>{e(title)}</title><meta name="description" content="{e(desc)}"><link rel="stylesheet" href="/styles.css"></head><body><header class="site-header"><a class="brand" href="/">DOLZORE<span>ドルゾーレ</span></a><nav><a href="{LEGACY}/business">Business</a><a href="{LEGACY}/creator">Creator</a><a href="{LEGACY}/apps">Apps</a><a class="active" href="/buying-guide">Buying Guide</a></nav></header><main>{body}</main><footer><div><strong>DOLZORE</strong><p>調べる時間を、選ぶ時間に変える。</p></div><div class="footer-links"><a href="{LEGACY}/about">About</a><a href="{LEGACY}/privacy">Privacy</a><a href="{LEGACY}/terms">Terms</a><a href="{INSTAGRAM}" target="_blank" rel="noopener noreferrer">Instagram</a></div></footer><script src="/site.js" defer></script></body></html>'''

def live_section(model="all"):
    return f'''<section class="section live-section" data-smartbuy-live data-model="{e(model)}"><div class="live-head"><div><p class="eyebrow">SmartBuy LIVE</p><h2>市場は、いまどうなっている？</h2></div><span class="live-dot">LIVE</span></div><p class="lead small">個別商品・本体・型番一致・販売中・価格正常・適合確認を全部通ったものだけを表示します。0件なら、0件と出します。</p><div class="live-meta">読み込み中…</div><div class="live-columns"><div><h3>中古の買い候補</h3><div class="live-used live-list"></div></div><div><h3>新品の候補</h3><div class="live-new live-list"></div></div></div><p class="disclaimer">販売状態・価格・商品状態は各販売ページで最終確認してください。SmartBuyは購入判断の補助です。</p></section>'''

def page_home():
    cards=[
      ("Business","検収・照合・RAGなど、仕事の困りごとを整える。",LEGACY+"/business","既存サイト"),
      ("Creator","動画編集の選曲時間を短くするCreator BGM。",LEGACY+"/creator","既存サイト"),
      ("Apps","公開プロトタイプと開発中アプリ。",LEGACY+"/apps","既存サイト"),
      ("Buying Guide","商品の特徴・新品/中古相場・買い時をSmartBuy LIVEと一緒に見る。","/buying-guide","新しい入口")]
    c="".join([f'<a class="entrance-card" href="{u}"><span>{tag}</span><h3>{n}</h3><p>{d}</p><b>見る →</b></a>' for n,d,u,tag in cards])
    return layout("DOLZORE | Business・Creator・Buying Guide","DOLZORE公式。Business、Creator、Apps、Buying Guide / SmartBuy LIVE。",
      f'''<section class="hero"><p class="eyebrow">Digital products · Research · Buying intelligence</p><h1>仕組みをつくり、<br>選ぶ時間を取り戻す。</h1><div class="rule"></div><p class="lead">DOLZOREは、仕事を整えるBusiness、動画編集を助けるCreator、公開アプリ、そして商品を理解して買い時まで見るBuying Guideを育てています。</p></section><section class="section"><p class="eyebrow">Four entrances</p><h2>目的から入ってください。</h2><div class="entrance-grid">{c}</div></section><section class="section warm"><p class="eyebrow">Buying Guide</p><h2>人気だけで決めない。<br>部屋と財布と用途で決める。</h2><p class="lead">最初のカテゴリはプロジェクター。スペック、150インチ設置、ゲーム遅延、新品と中古の差、いま販売中の候補まで一画面で追います。</p><a class="button" href="/buying-guide/projectors">プロジェクターを見る</a></section>''')

def page_guide():
    return layout("DOLZORE Buying Guide | 買う前に、ちゃんと知る","商品を理解し、新品・中古相場とSmartBuy LIVEで買い時まで確認するDOLZORE Buying Guide。",
      f'''<section class="hero compact"><p class="eyebrow">DOLZORE Buying Guide</p><h1>買う前に、ちゃんと知る。</h1><div class="rule"></div><p class="lead">価格だけでも、スペックだけでも決めない。DOLZORE編集部リサーチとSmartBuy LIVEで「自分の条件なら今買う意味があるか」まで整理します。</p></section><section class="section"><div class="category-hero">{art("プロジェクター",1)}<div><span class="pill">公開中</span><h2>Projectors</h2><p>4K、短焦点、150インチ、ゲーム。似て見えて、部屋に置くとぜんぜん違う。主要候補10機種から始めます。</p><a class="button" href="/buying-guide/projectors">10機種を見る</a></div></div></section><section class="section warm"><p class="eyebrow">How it works</p><div class="three"><div><b>1</b><h3>理解する</h3><p>仕様・設置・弱点を図で確認。</p></div><div><b>2</b><h3>比べる</h3><p>新品・中古・後継まで横断。</p></div><div><b>3</b><h3>今を見る</h3><p>SmartBuy LIVEで販売中候補だけ確認。</p></div></div><p class="note">Camera / PC / Audioはまだ提供中ではありません。</p></section>''')

def page_projectors():
    cards=[]
    for i,p in enumerate(PRODUCTS):
        chips="".join([f"<span>{e(x)}</span>" for x in p["specs"]])
        buy=f'<div class="buyline">中古目安 <strong>{yen(p.get("strong"))}以下</strong>で注目 / {yen(p.get("consider"))}まで検討</div>' if p.get("strong") else ""
        deep='<a class="text-link" href="/buying-guide/projectors/tk700sti">深掘りガイド →</a>' if p.get("deep") else '<span class="muted">深掘りガイド準備中</span>'
        cards.append(f'''<article class="product-card">{art(p["name"],i)}<div class="product-body"><div class="card-top"><span class="pill">{e(p["badge"])}</span><small>{i+1:02d}</small></div><h3>{e(p["name"])}</h3><p class="one-line">{e(p["line"])}</p><div class="spec-chips">{chips}</div>{buy}<p class="joke">“ {e(p["joke"])} ”</p><div class="card-actions">{deep}<a class="source-link" href="{e(p["src"])}" target="_blank" rel="noopener noreferrer nofollow">公式情報 ↗</a></div></div></article>''')
    return layout("プロジェクターおすすめ候補10機種 | DOLZORE Buying Guide","4K・短焦点・150インチ・ゲームを軸に主要プロジェクター10機種を整理。SmartBuy LIVEの新品・中古候補も確認できます。",
      f'''<section class="hero compact"><p class="eyebrow">Projector Guide · 10 candidates</p><h1>人気だけで決めない。<br>部屋と財布と用途で決める。</h1><div class="rule"></div><p class="lead">順位を断定するページではありません。用途ごとの主要候補を並べています。</p></section><section class="section"><div class="section-head"><div><p class="eyebrow">Editor selections</p><h2>まず追う10機種</h2></div><p>DOLZORE編集部リサーチ。実機レビューではありません。</p></div><div class="product-grid">{"".join(cards)}</div></section>{live_section()}</section><section class="section warm instagram"><p class="eyebrow">Instagram × DOLZORE</p><h2>1分版はInstagram、<br>深掘りとLIVE相場はDOLZORE。</h2><a class="button outline" href="{INSTAGRAM}" target="_blank" rel="noopener noreferrer">@dolzoreofficial</a></section>''')

def page_tk700():
    facts=[("解像度","4K UHD 3840×2160"),("明るさ","3000 ANSIルーメン"),("光源","ランプ"),("投写比","0.9〜1.08"),("4K/60Hz","16.7ms"),("150インチ目安","約2.99〜3.59m")]
    fact="".join([f"<div><span>{a}</span><strong>{b}</strong></div>" for a,b in facts])
    checks=["白画面でドット・色ムラ・内部埃を確認","ランプ使用時間と交換履歴を確認","HDMI入力・4K表示・リモコンを確認","ファン異音や高温時の停止がないか確認","Android TVドングル等の付属品を確認"]
    ck="".join([f"<li><span>{i+1:02d}</span>{e(x)}</li>" for i,x in enumerate(checks)])
    rows=[("TK700STi","3000 ANSI","Lamp","0.9–1.08","16.7ms","中古7.5万円以下で強い"),("TK705STi","3000 ANSI","4LED","0.8","5ms","短焦点・低遅延の現行本命"),("TK710STi","3200 ANSI","Laser","0.69–0.83","16.7ms","レーザー寿命を重視")]
    table="".join(["<tr>"+"".join([f"<td>{e(v)}</td>" for v in r])+"</tr>" for r in rows])
    return layout("BenQ TK700STiは2026年でも買い？中古相場と後継比較 | DOLZORE","TK700STiの4K・3000 ANSI・投写距離・遅延・中古チェックポイントを解説。SmartBuy LIVEで現在候補も確認。",
      f'''<section class="hero product-hero"><div><p class="eyebrow">Deep Guide · BenQ TK700STi</p><span class="pill">DOLZORE編集部リサーチ</span><h1>中古なら、まだかなりアリ。</h1><div class="rule"></div><p class="lead">生産終了後も4K・3000 ANSI lm・16.7ms。新品在庫を追うより、状態の良い中古を買い価格で拾う方がSmartBuy向きです。</p><p class="joke hero-joke">“ スペックはまだ現役。ランプだけは年齢を隠せません。 ”</p></div>{art("BenQ TK700STi",2)}</section><section class="section split"><div><p class="eyebrow">30-second verdict</p><h2>こんな人向け</h2><ul class="good-list"><li>○ 150インチを約3m台で設置したい</li><li>○ 4K映画・PC・ゲームを1台でまとめたい</li><li>○ 現行短焦点4Kより予算を抑えたい</li></ul></div><div><p class="eyebrow">Not for everyone</p><h2>こんな人は別候補</h2><ul class="bad-list"><li>△ ランプ交換や中古確認を避けたい</li><li>△ 壁際設置の超短焦点が必須</li><li>△ 4K60で5ms級を求める</li></ul></div></section><section class="section warm"><p class="eyebrow">Key specs</p><h2>数字は少なく、意味は大きく。</h2><div class="fact-grid">{fact}</div><div class="visual-grid"><div class="visual-card"><h3>150インチ設置イメージ</h3><div class="room-visual"><div class="screen"><span>150"</span></div><div class="beam"></div><div class="device"></div><p>約2.99〜3.59m</p></div></div><div class="visual-card"><h3>4K / 60Hz</h3><div class="latency"><span style="width:67%"></span><b>16.7ms</b></div><p>5ms級のTK705STiには負けますが、中古価格差で勝負できます。</p></div></div></section><section class="section"><p class="eyebrow">Used checklist</p><h2>中古は、値札より白画面を見る。</h2><ol class="check-list">{ck}</ol></section><section class="section warm"><p class="eyebrow">Successors</p><h2>旧型が安い理由と、新型が高い理由。</h2><div class="table-wrap"><table><thead><tr><th>機種</th><th>明るさ</th><th>光源</th><th>投写比</th><th>4K60</th><th>編集メモ</th></tr></thead><tbody>{table}</tbody></table></div></section>{live_section("BenQ TK700STi")}<section class="section sources"><p class="eyebrow">Sources</p><h2>仕様の根拠</h2><div><a href="https://www.benq.com/ja-jp/projector/gaming/tk700sti/spec.html" target="_blank" rel="noopener noreferrer nofollow">TK700STi 公式仕様 ↗</a><a href="https://www.benq.com/ja-jp/projector/cinema/tk705sti/spec.html" target="_blank" rel="noopener noreferrer nofollow">TK705STi 公式仕様 ↗</a><a href="https://www.benq.com/ja-jp/projector/gaming/tk710sti/spec.html" target="_blank" rel="noopener noreferrer nofollow">TK710STi 公式仕様 ↗</a></div><p class="note">実機レビューではなく、メーカー公開仕様とSmartBuy市場データをもとにした編集部リサーチです。</p></section>''')

def is_item(market, raw):
    try:
        u=urlparse(raw); host=u.hostname or ""; p=u.path.lower()
        if market=="mercari": return host.endswith("mercari.com") and p.startswith("/item/")
        if market=="yahoo_flea": return host=="paypayfleamarket.yahoo.co.jp" and p.startswith("/item/")
        if market=="yahoo_auction": return host=="page.auctions.yahoo.co.jp" or (host=="auctions.yahoo.co.jp" and p.startswith("/jp/auction/"))
        if market=="amazon": return "amazon.co.jp" in host and ("/dp/" in p or "/gp/product/" in p)
        if market=="rakuten": return host=="item.rakuten.co.jp" and len([x for x in p.split("/") if x])>=2
        if market=="hardoff": return "hardoff.co.jp" in host and "/product/" in p
        if market=="secondstreet": return "2ndstreet.jp" in host and "/goods/detail/" in p
        if market=="rakuma": return host=="item.fril.jp" and p.strip("/")!=""
        if market=="yahoo_shopping": return host.endswith("shopping.yahoo.co.jp") and "/search" not in p and len(p)>8
    except: return False
    return False

def eligible(r):
    try: price=float(r.get("price"))
    except: return False
    title=str(r.get("title","")).lower(); market=str(r.get("market",""))
    return str(r.get("target_category","")).lower()=="projector" and bool(r.get("matched_priority_model")) and r.get("fit_status")=="ELIGIBLE" and r.get("live_status")=="live" and r.get("detail_verification_status")=="verified" and price>=10000 and r.get("price_status") in ("strong_buy","consider","acceptable") and not any(x.lower() in title for x in BAD) and is_item(market,r.get("url",""))

def smartbuy():
    try:
        req=urllib.request.Request(SMARTBUY,headers={"Accept":"application/json","User-Agent":"DOLZORE-SmartBuy-Public/0.1"})
        with urllib.request.urlopen(req,timeout=8) as resp: data=json.loads(resp.read().decode())
        rows=data.get("results") if isinstance(data,dict) else []
        rows=rows if isinstance(rows,list) else []
        ok=[r for r in rows if eligible(r)]
        order={"strong_buy":0,"consider":1,"acceptable":2}
        ok.sort(key=lambda r:(order.get(r.get("price_status"),9),float(r.get("price",10**18))))
        def pub(r): return {"title":str(r.get("title",""))[:180],"model":str(r.get("matched_priority_model","")),"market":str(r.get("market","")),"marketLabel":str(r.get("market_label") or r.get("market") or ""),"price":float(r.get("price")),"priceStatus":str(r.get("price_status","")),"capturedAt":r.get("captured_at"),"url":str(r.get("url",""))}
        return {"ok":True,"generatedAt":data.get("generated_at"),"observedProjectorRows":sum(1 for r in rows if str(r.get("target_category","")).lower()=="projector"),"used":[pub(r) for r in ok if str(r.get("market","")) in USED_MARKETS][:10],"newItems":[pub(r) for r in ok if str(r.get("market","")) in NEW_MARKETS][:10]}
    except Exception as ex:
        return {"ok":False,"error":"LIVE市場データを一時取得できません。","generatedAt":None,"observedProjectorRows":0,"used":[],"newItems":[]}

def asset(name):
    env={"styles.css":"DOLZORE_CSS","site.js":"DOLZORE_JS"}[name]
    v=os.environ.get(env)
    if v: return v.encode()
    p=os.path.join(BASE,"public",name)
    return open(p,"rb").read() if os.path.exists(p) else b""

class H(BaseHTTPRequestHandler):
    def sendb(self,status,body,ctype):
        b=body.encode("utf-8") if isinstance(body,str) else body
        self.send_response(status); self.send_header("Content-Type",ctype); self.send_header("Content-Length",str(len(b))); self.send_header("Cache-Control","no-store"); self.end_headers(); self.wfile.write(b)
    def do_GET(self):
        p=self.path.split("?",1)[0].rstrip("/") or "/"
        if p=="/health": return self.sendb(200,json.dumps({"ok":True,"service":"dolzore-web","version":"0.1.0"}),"application/json")
        if p=="/styles.css": return self.sendb(200,asset("styles.css"),"text/css; charset=utf-8")
        if p=="/site.js": return self.sendb(200,asset("site.js"),"application/javascript; charset=utf-8")
        if p=="/api/smartbuy/projectors": return self.sendb(200,json.dumps(smartbuy(),ensure_ascii=False),"application/json; charset=utf-8")
        pages={"/":page_home,"/buying-guide":page_guide,"/buying-guide/projectors":page_projectors,"/buying-guide/projectors/tk700sti":page_tk700}
        if p in pages: return self.sendb(200,pages[p](),"text/html; charset=utf-8")
        return self.sendb(404,layout("404 | DOLZORE","ページが見つかりません。",'<section class="hero compact"><p class="eyebrow">404</p><h1>このページは、まだここにいません。</h1><a class="button" href="/buying-guide">Buying Guideへ</a></section>'),"text/html; charset=utf-8")
    def log_message(self,fmt,*args): print(fmt%args,flush=True)

def main():
    print("DOLZORE_WEB_BOOT",PORT,flush=True)
    ThreadingHTTPServer(("0.0.0.0",PORT),H).serve_forever()

if __name__ == "__main__":
    main()
