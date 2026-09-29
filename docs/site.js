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

  function safeHttps(raw){
    try{
      const u=new URL(raw);
      return u.protocol==="https:"?u.href:"";
    }catch{return ""}
  }

  function initProductMedia(){
    const slots=[...document.querySelectorAll("[data-product-media]")];
    if(!slots.length)return;
    readJson("data/product-media.json").then(data=>{
      const map=new Map((data.products||[]).map(x=>[x.model,x]));
      slots.forEach(slot=>{
        const model=slot.dataset.productMedia||"";
        const row=map.get(model);
        const url=safeHttps(row&&row.imageUrl);
        if(!url)return;
        const img=document.createElement("img");
        img.className="official-product-photo";
        img.alt=model+" メーカー公式画像";
        img.loading=slot.closest(".electronics-hero")?"eager":"lazy";
        img.decoding="async";
        img.referrerPolicy="no-referrer";
        img.src=url;
        img.addEventListener("load",()=>{
          slot.classList.add("has-official-photo");
          const fallback=slot.querySelector(".pixel-projector");
          const placeholder=slot.querySelector(".official-photo-placeholder");
          if(fallback)fallback.setAttribute("hidden","");
          if(placeholder)placeholder.setAttribute("hidden","");
        },{once:true});
        img.addEventListener("error",()=>img.remove(),{once:true});
        slot.prepend(img);
      });
    }).catch(()=>{});
  }

  function initProjectorFilters(){
    const controls=document.querySelector("[data-projector-filters]");
    const cards=[...document.querySelectorAll("[data-product-card]")];
    if(!controls||!cards.length)return;
    controls.querySelectorAll("[data-filter]").forEach(btn=>{
      btn.setAttribute("aria-pressed",btn.classList.contains("is-active")?"true":"false");
      btn.addEventListener("click",()=>{
        const filter=btn.dataset.filter||"all";
        controls.querySelectorAll("[data-filter]").forEach(x=>{
          const active=x===btn;
          x.classList.toggle("is-active",active);
          x.setAttribute("aria-pressed",active?"true":"false");
        });
        cards.forEach(card=>{
          const visible=filter==="all"||(card.dataset.tags||"").split(/\s+/).includes(filter);
          card.hidden=!visible;
        });
      });
    });
  }

  function initThrowTool(){
    const tool=document.querySelector("[data-throw-tool]");if(!tool)return;
    const model=tool.querySelector("[data-throw-model]");
    const size=tool.querySelector("[data-screen-size]");
    const sizeLabel=tool.querySelector("[data-screen-label]");
    const result=tool.querySelector("[data-throw-result]");
    const update=()=>{
      const inches=Number(size.value||150);
      const ratios=String(model.value||"1,1").split(",").map(Number);
      const widthM=inches*(16/Math.sqrt(16*16+9*9))*0.0254;
      const min=widthM*ratios[0],max=widthM*ratios[1];
      sizeLabel.textContent=String(inches);
      result.textContent=Math.abs(max-min)<0.03 ? "約"+min.toFixed(2)+"m" : "約"+min.toFixed(2)+"〜"+max.toFixed(2)+"m";
    };
    model.addEventListener("change",update);
    size.addEventListener("input",update);
    update();
  }

  function initMarketSnapshot(){
    const box=document.querySelector("[data-smartbuy-market]");if(!box)return;
    const meta=box.querySelector("[data-market-meta]");
    const used=box.querySelector("[data-market-used]");
    const fresh=box.querySelector("[data-market-new]");
    const targetBoard=box.querySelector("[data-market-targets]");
    const statusLabel={strong_buy:"買い価格",consider:"検討価格",acceptable:"条件内"};
    const yen=v=>Number.isFinite(Number(v))?"¥"+Number(v).toLocaleString("ja-JP"):"—";
    const fmt=v=>{
      if(!v)return "未同期";
      try{return new Intl.DateTimeFormat("ja-JP",{timeZone:"Asia/Tokyo",year:"numeric",month:"2-digit",day:"2-digit",hour:"2-digit",minute:"2-digit"}).format(new Date(v))}
      catch{return String(v)}
    };
    const searchLinks=model=>{
      const q=encodeURIComponent(model);
      return [
        ["メルカリ","https://jp.mercari.com/search?keyword="+q+"&status=on_sale"],
        ["Yahoo!フリマ","https://paypayfleamarket.yahoo.co.jp/search/"+q],
        ["Yahoo!オークション","https://auctions.yahoo.co.jp/search/search?p="+q]
      ];
    };
    const renderTargets=(targets,items)=>{
      if(!targetBoard)return;
      if(!targets.length){
        targetBoard.innerHTML='<div class="market-empty">買い目安データを読み込めません。</div>';
        return;
      }
      targetBoard.innerHTML=targets.map(t=>{
        const candidates=items.filter(x=>x.model===t.model).sort((a,b)=>Number(a.price)-Number(b.price));
        const best=candidates[0];
        const links=searchLinks(t.model).map(pair=>'<a href="'+pair[1]+'" target="_blank" rel="noopener noreferrer nofollow">'+pair[0]+'で探す ↗</a>').join("");
        const current=best?'<div class="current"><small>現在候補</small><b>'+yen(best.price)+'</b></div>':"";
        return '<article class="market-target-card" data-market-model="'+t.model+'"><div class="market-target-head"><strong>'+t.model+'</strong><span>'+(best?"候補あり":"候補なし")+'</span></div><div class="market-target-prices"><div><small>強く注目</small><b>〜'+yen(t.strongBuy)+'</b></div><div><small>検討上限</small><b>〜'+yen(t.consider)+'</b></div>'+current+'</div><div class="market-search-links">'+links+'</div></article>';
      }).join("");
    };
    const render=(target,items)=>{
      if(!items.length){
        target.innerHTML='<div class="market-empty">現在、厳格条件を通過した販売中候補は0件です。上の買い目安は残してあるので、市場検索から確認できます。</div>';
        return;
      }
      target.innerHTML=items.map(item=>{
        const image=safeHttps(item.imageUrl);
        const url=safeHttps(item.url)||"#";
        const media=image?'<img src="'+image+'" alt="" loading="lazy" decoding="async">':'<span>NO PHOTO</span>';
        return '<article class="market-listing"><div class="market-listing-image">'+media+'</div><div class="market-listing-copy"><small>'+(item.marketLabel||item.market||"")+' · '+(item.model||"")+'</small><a href="'+url+'" target="_blank" rel="noopener noreferrer nofollow"></a><span>'+(statusLabel[item.priceStatus]||"確認済み")+'</span></div><div class="market-listing-cta"><strong>'+yen(item.price)+'</strong><a href="'+url+'" target="_blank" rel="noopener noreferrer nofollow">商品を見る →</a></div></article>';
      }).join("");
      [...target.querySelectorAll(".market-listing")].forEach((el,i)=>{
        const link=el.querySelector(".market-listing-copy>a");
        if(link)link.textContent=items[i]&&items[i].title?items[i].title:"商品ページ";
      });
    };
    readJson("data/smartbuy-projectors.json").then(d=>{
      const u=d.used||[],n=d.newItems||[],targets=d.targets||[];
      meta.textContent="最終確認 "+fmt(d.generatedAt)+" / 厳格候補 中古 "+u.length+"件・新品 "+n.length+"件";
      renderTargets(targets,[...u,...n]);
      render(used,u);render(fresh,n);
      document.querySelectorAll("[data-market-jump]").forEach(link=>{
        link.addEventListener("click",()=>{
          const model=link.dataset.marketJump||"";
          requestAnimationFrame(()=>{
            document.querySelectorAll(".market-target-card").forEach(card=>card.classList.toggle("is-highlight",card.dataset.marketModel===model));
          });
        });
      });
    }).catch(()=>{
      meta.textContent="市場データを一時取得できません。買い目安と記事本文はそのまま利用できます。";
      renderTargets([],[]);
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
  initProductMedia();
  initProjectorFilters();
  initThrowTool();
  initMarketSnapshot();
})();