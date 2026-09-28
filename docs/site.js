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
      setStatus(`${track.id} を選びました。再生で20秒試聴できます。`);
      updateButtons();
    }
    function render(){
      selector.innerHTML="";
      filtered.forEach(track=>{
        const b=document.createElement("button");
        b.type="button";b.className="track-choice";b.dataset.id=track.id;
        b.innerHTML=`<span class="n">${track.number}</span><strong></strong><small></small>`;
        b.querySelector("strong").textContent=track.title;
        b.querySelector("small").textContent=track.useCase;
        b.addEventListener("click",()=>selectByTrack(track));
        selector.appendChild(b);
      });
      currentIndex=Math.min(currentIndex,Math.max(0,filtered.length-1));
      if(filtered.length) selectByTrack(filtered[currentIndex]);
      else setStatus("一致する曲がありません。検索語を変えてください。");
    }
    function ensureAudio(){
      const track=current();if(!track)return false;
      if(loadedId!==track.id){
        audio.src=new URL(`audio/samples/${track.sampleFile}`,root).href;
        loadedId=track.id;
      }
      return true;
    }
    async function togglePlay(){
      if(!ensureAudio()) return;
      if(audio.paused){
        try{await audio.play();disc.classList.add("playing");play.textContent="Ⅱ";setStatus(`${current().title} を試聴中です。`)}
        catch(e){disc.classList.remove("playing");play.textContent="▶";setStatus("この曲の試聴サンプルはまだ準備中です。")}
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
      if(audio.currentTime>=max){stopAudio();setStatus("20秒の試聴が終わりました。次の曲もどうぞ。")}
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
})();