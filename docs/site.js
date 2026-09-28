(function(){
  const script=document.currentScript;
  const root=new URL(".",script.src);

  const pad=n=>String(Math.max(0,Math.floor(n))).padStart(2,"0");
  const timeText=s=>`${pad(s/60)}:${pad(s%60)}`;

  async function readJson(path){
    const r=await fetch(new URL(path,root),{cache:"no-store"});
    if(!r.ok) throw new Error(`HTTP ${r.status}`);
    return r.json();
  }

  function initJukebox(){
    const page=document.querySelector("[data-jukebox]");
    if(!page) return;
    const audio=page.querySelector("[data-audio]");
    const selector=page.querySelector("[data-track-selector]");
    const search=page.querySelector("[data-track-search]");
    const number=page.querySelector("[data-track-number]");
    const title=page.querySelector("[data-track-title]");
    const useCase=page.querySelector("[data-track-usecase]");
    const moment=page.querySelector("[data-track-moment]");
    const status=page.querySelector("[data-status]");
    const purchase=page.querySelector("[data-purchase]");
    const progress=page.querySelector("[data-progress]");
    const time=page.querySelector("[data-time]");
    const disc=page.querySelector("[data-record-disc]");
    const play=page.querySelector("[data-play]");
    let tracks=[],filtered=[],currentIndex=0,loadedId=null;

    const current=()=>filtered[currentIndex]||tracks[0];

    function setStatus(msg){status.textContent=msg}
    function updateButtons(){
      selector.querySelectorAll(".track-choice").forEach(btn=>btn.classList.toggle("active",btn.dataset.id===current()?.id));
    }
    function selectByTrack(track){
      const idx=filtered.findIndex(t=>t.id===track.id);
      if(idx>=0) currentIndex=idx;
      number.textContent=track.number;
      title.textContent=track.title;
      useCase.textContent=track.useCase;
      moment.textContent=track.moment;
      purchase.href=track.purchaseUrl;
      purchase.setAttribute("aria-label",`${track.title}を¥200で購入`);
      progress.max=track.sampleSeconds||20;progress.value=0;
      time.textContent=`00:00 / 00:${pad(track.sampleSeconds||20)}`;
      if(!audio.paused){audio.pause();disc.classList.remove("playing");play.textContent="▶"}
      audio.removeAttribute("src");audio.load();loadedId=null;
      setStatus(`${track.number} をセットしました。PLAYで20秒きけます。`);
      updateButtons();
    }
    function render(){
      selector.innerHTML="";
      filtered.forEach(track=>{
        const b=document.createElement("button");
        b.type="button";b.className="track-choice";b.dataset.id=track.id;
        b.innerHTML=`<span class="n">${track.number}</span><strong></strong><small></small>`;
        b.classList.toggle("preview-ready",!!track.previewReady);
        b.querySelector("strong").textContent=track.title;
        b.querySelector("small").textContent=track.useCase + (track.previewReady ? " · 20秒きけます" : " · 試聴は準備中");
        b.addEventListener("click",()=>selectByTrack(track));
        selector.appendChild(b);
      });
      currentIndex=Math.min(currentIndex,Math.max(0,filtered.length-1));
      if(filtered.length) selectByTrack(filtered[currentIndex]);
      else setStatus("一致する曲がありません。検索語を変えてください。");
    }
    function ensureAudio(){
      const track=current();if(!track)return false;
      if(!track.previewReady){
        setStatus("この番号の試聴は、まだ準備中です。購入リンクは使えます。");
        return false;
      }
      if(loadedId!==track.id){
        audio.src=track.previewUrl || new URL(`audio/samples/${track.sampleFile}`,root).href;
        loadedId=track.id;
      }
      return true;
    }
    async function togglePlay(){
      if(!ensureAudio()) return;
      if(audio.paused){
        try{await audio.play();disc.classList.add("playing");play.textContent="Ⅱ";setStatus(`${current().title} を試聴中です。`)}
        catch(e){disc.classList.remove("playing");play.textContent="▶";setStatus("うまく再生できませんでした。少し時間をおいて試してください。")}
      }else{audio.pause();disc.classList.remove("playing");play.textContent="▶";setStatus("一時停止しました。")}
    }
    function stopAudio(){audio.pause();audio.currentTime=0;disc.classList.remove("playing");play.textContent="▶";setStatus("停止しました。")}
    function move(delta){
      if(!filtered.length)return;
      currentIndex=(currentIndex+delta+filtered.length)%filtered.length;
      selectByTrack(filtered[currentIndex]);
      selector.querySelector(`[data-id="${current().id}"]`)?.scrollIntoView({block:"nearest"});
    }
    page.querySelector("[data-prev]").addEventListener("click",()=>move(-1));
    page.querySelector("[data-next]").addEventListener("click",()=>move(1));
    page.querySelector("[data-stop]").addEventListener("click",stopAudio);
    play.addEventListener("click",togglePlay);
    search.addEventListener("input",()=>{
      const q=search.value.trim().toLowerCase();
      filtered=!q?tracks:tracks.filter(t=>[t.id,t.title,t.useCase,t.moment].join(" ").toLowerCase().includes(q));
      currentIndex=0;render();
    });
    audio.addEventListener("timeupdate",()=>{
      const max=current()?.sampleSeconds||20;
      progress.value=Math.min(audio.currentTime,max);
      time.textContent=`${timeText(audio.currentTime)} / 00:${pad(max)}`;
      if(audio.currentTime>=max){stopAudio();setStatus("20秒、おしまいです。次の番号もどうぞ。")}
    });
    audio.addEventListener("ended",()=>{disc.classList.remove("playing");play.textContent="▶";setStatus("試聴が終わりました。")});
    audio.addEventListener("error",()=>{disc.classList.remove("playing");play.textContent="▶";setStatus("この曲の試聴サンプルはまだ準備中です。")});
    document.addEventListener("keydown",e=>{
      if(document.activeElement?.matches("input,textarea,select"))return;
      if(e.key==="ArrowLeft"){e.preventDefault();move(-1)}
      if(e.key==="ArrowRight"){e.preventDefault();move(1)}
      if(e.code==="Space"){e.preventDefault();togglePlay()}
    });
    readJson("data/tracks.json").then(d=>{tracks=d.tracks||[];filtered=[...tracks];render()}).catch(()=>setStatus("曲一覧を読み込めませんでした。"));
  }

  function initMarketSnapshot(){
    const box=document.querySelector("[data-smartbuy-market]");if(!box)return;
    const meta=box.querySelector("[data-market-meta]");
    const used=box.querySelector("[data-market-used]");
    const fresh=box.querySelector("[data-market-new]");
    const statusLabel={strong_buy:"買い価格",consider:"検討価格",acceptable:"条件内"};
    const yen=v=>Number.isFinite(Number(v))?"¥"+Number(v).toLocaleString("ja-JP"):"—";
    const fmt=v=>{
      if(!v)return "未同期";
      try{return new Intl.DateTimeFormat("ja-JP",{timeZone:"Asia/Tokyo",year:"numeric",month:"2-digit",day:"2-digit",hour:"2-digit",minute:"2-digit"}).format(new Date(v))}
      catch{return String(v)}
    };
    const render=(target,items)=>{
      if(!items.length){
        target.innerHTML='<div class="market-empty">現在、厳格条件を通過した販売中候補は0件です。無理に1位を作りません。</div>';
        return;
      }
      target.innerHTML=items.map(x=>`<div class="market-item"><div><a href="#" target="_blank" rel="noopener noreferrer nofollow"></a><small></small></div><div class="market-price"><strong>${yen(x.price)}</strong><span>${statusLabel[x.priceStatus]||"確認済み"}</span></div></div>`).join("");
      [...target.querySelectorAll(".market-item")].forEach((el,i)=>{
        const item=items[i]||{};
        const link=el.querySelector("a");
        try{
          const u=new URL(item.url);
          link.href=u.protocol==="https:"?u.href:"#";
        }catch{link.href="#"}
        link.textContent=item.title||"商品ページ";
        el.querySelector("small").textContent=(item.marketLabel||item.market||"")+" · "+(item.model||"");
      });
    };
    readJson("data/smartbuy-projectors.json").then(d=>{
      const u=d.used||[],n=d.newItems||[];
      meta.textContent=`最終更新 ${fmt(d.generatedAt)} / 中古 ${u.length}件・新品 ${n.length}件`;
      render(used,u);render(fresh,n);
    }).catch(()=>{
      meta.textContent="市場データを一時取得できません。記事本文はそのまま読めます。";
      render(used,[]);render(fresh,[]);
    });
  }

  function initJournal(){
    const target=document.querySelector("[data-journal-list]");if(!target)return;
    readJson("data/articles.json").then(d=>{
      const items=d.articles||[];if(!items.length)return;
      target.innerHTML=items.map(a=>`<article class="journal-card"><p class="pixel-kicker">${a.date||""}</p><h2></h2><p class="excerpt"></p><a class="pixel-button" href="${a.url}">読む →</a></article>`).join("");
      [...target.querySelectorAll(".journal-card")].forEach((el,i)=>{el.querySelector("h2").textContent=items[i].title;el.querySelector(".excerpt").textContent=items[i].excerpt||""});
    }).catch(()=>{});
  }

  initJukebox();
  initJournal();
  initMarketSnapshot();
})();