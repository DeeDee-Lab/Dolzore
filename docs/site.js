(function(){
  const root=new URL(".",document.currentScript.src);
  const statusLabel={strong_buy:"買い価格",consider:"検討価格",acceptable:"条件内"};
  const yen=v=>Number.isFinite(Number(v))?"¥"+Number(v).toLocaleString("ja-JP"):"—";
  const fmt=v=>{if(!v)return "未同期";try{return new Intl.DateTimeFormat("ja-JP",{timeZone:"Asia/Tokyo",year:"numeric",month:"2-digit",day:"2-digit",hour:"2-digit",minute:"2-digit"}).format(new Date(v))}catch{return String(v)}};
  const esc=v=>String(v??"").replace(/[&<>"']/g,c=>({"&":"&amp;","<":"&lt;",">":"&gt;","\"":"&quot;","'":"&#39;"}[c]));
  const item=x=>'<div class="live-item"><div><a href="'+esc(x.url)+'" target="_blank" rel="noopener noreferrer nofollow">'+esc(x.title)+'</a><small>'+esc(x.marketLabel)+' · '+esc(x.model)+'</small></div><div class="live-price"><b>'+yen(x.price)+'</b><span class="live-tag">'+esc(statusLabel[x.priceStatus]||"確認済み")+'</span></div></div>';
  const list=(el,items)=>el.innerHTML=items.length?items.map(item).join(""):'<div class="live-empty">現在、厳格条件を通過した候補は0件です。無理に1位を作りません。</div>';
  async function load(box){const meta=box.querySelector(".live-meta"),u=box.querySelector(".live-used"),n=box.querySelector(".live-new"),model=box.dataset.model||"";try{const r=await fetch(new URL("data/smartbuy-projectors.json",root),{cache:"no-store"});const d=await r.json();let used=d.used||[],fresh=d.newItems||[];if(model){used=used.filter(x=>x.model===model);fresh=fresh.filter(x=>x.model===model)}meta.textContent="最終同期 "+fmt(d.generatedAt)+" / 中古"+used.length+"件・新品"+fresh.length+"件 / "+(d.source||"GitHub");list(u,used);list(n,fresh)}catch(e){meta.textContent="市場スナップショットを取得できません。静的ガイドはそのまま読めます。";list(u,[]);list(n,[])}}document.querySelectorAll("[data-smartbuy-live]").forEach(load);
})();