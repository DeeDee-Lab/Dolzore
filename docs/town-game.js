(()=>{
  const root=document.querySelector("[data-town-world]");
  if(!root)return;

  const canvas=root.querySelector("[data-town-canvas]");
  const ctx=canvas.getContext("2d",{alpha:false});
  const dialogueName=root.querySelector("[data-dialogue-name]");
  const dialogueText=root.querySelector("[data-dialogue-text]");
  const districtName=root.querySelector("[data-district-name]");
  const labelLayer=root.querySelector("[data-world-label-layer]");
  const actionButton=root.querySelector("[data-action]");
  const jumpButton=root.querySelector("[data-jump]");
  const bgmButton=root.querySelector("[data-bgm-toggle]");

  const VIEW_W=480,VIEW_H=270,WORLD_W=1536,WORLD_H=1152,SW=32,SH=40;
  ctx.imageSmoothingEnabled=false;

  const bg=new Image();
  const sprites=new Image();
  bg.src="assets/dolzore-first-town.png";
  sprites.src="assets/dolzore-characters.png";

  const frames={down:[0,1],left:[2,3],right:[4,5],up:[6,7]};
  const player={
    name:"SORA",row:0,x:360,y:350,dir:"down",step:0,moving:false,
    jumpActive:false,jumpT:0,jumpHeight:0
  };
  const camera={x:0,y:0};

  const residents=[
    {name:"MELO",row:1,x:735,y:472,dir:"down",step:0,text:"BARの中、まだ片づけ中。外まで音が漏れてる。"},
    {name:"YUZU",row:2,x:455,y:746,dir:"down",step:0,text:"この辺りは古い記録が多い。住所だけ、少し合わない。"},
    {name:"PON",row:3,x:980,y:530,dir:"right",step:0,text:"駅へ行くなら東。寄り道するなら、こっち。",wander:true,vx:.18,minX:900,maxX:1060},
  ];

  const places=[
    {id:"home",name:"HOME",x:190,y:215,r:40,label:"HOME",text:"ここが今の拠点。坂を下りると中央通り。"},
    {id:"bar",name:"BAR",x:700,y:447,r:46,label:"BAR",text:"最初のBAR。中はまだ準備中だけど、ここから音楽の街が始まる。"},
    {id:"cafe",name:"CAFE",x:892,y:450,r:38,label:"CAFE",text:"昼は人が多いらしい。今日は静かだ。"},
    {id:"journal",name:"JOURNAL",x:415,y:722,r:46,label:"JOURNAL",text:"町の記録を集めている場所。"},
    {id:"market",name:"MARKET",x:1110,y:672,r:54,label:"MARKET",text:"工房と露店が集まる路地。"},
    {id:"river",name:"RIVERSIDE",x:565,y:860,r:58,label:"RIVERSIDE",text:"川沿い。ここは最初の釣り場になる予定。"},
    {id:"station",name:"STATION",x:1330,y:835,r:56,label:"STATION",text:"東へ続く駅と街道。次の地域はこの先だ。"},
    {id:"alley",name:"ALLEY",x:845,y:300,r:42,label:"",text:"行き止まりみたいだけど、妙に風が通る。"},
  ];

  const blockers=[
    [105,95,280,220],[305,80,460,205],[85,235,230,350],[275,220,450,340],[525,85,610,220],
    [600,325,805,455],[810,340,985,460],[1070,325,1260,450],[1250,345,1425,460],
    [1005,190,1195,310],[1200,195,1375,305],[1350,240,1505,340],
    [310,595,530,735],[530,620,700,735],[705,642,780,728],
    [118,768,295,885],[1008,778,1195,895],[1208,682,1455,842],[1308,612,1372,695],
    [805,232,835,312],
  ];

  const keys=new Set();
  let ready=false,last=performance.now(),walkTick=0;

  function clamp(v,min,max){return Math.max(min,Math.min(max,v))}
  function dist(a,b){return Math.hypot(a.x-b.x,a.y-b.y)}
  function show(name,text){dialogueName.textContent=name;dialogueText.textContent=text}

  function districtFor(x,y){
    if(y<390&&x<650)return "RESIDENTIAL HILL";
    if(y<380&&x>=900)return "MARKET / WORKSHOP";
    if(y>=580&&y<820&&x<760)return "CIVIC / JOURNAL";
    if(y>=820&&x<1180)return "RIVERSIDE";
    if(x>=1170&&y>=620)return "STATION / EAST GATE";
    if(x>=760&&x<930&&y<360)return "BACK ALLEY";
    return "CENTRAL MAIN STREET";
  }

  function blocked(x,y){
    if(x<16||x>WORLD_W-16||y<60||y>WORLD_H-18)return true;
    if(blockers.some(([x1,y1,x2,y2])=>x>x1-9&&x<x2+9&&y>y1-6&&y<y2+8))return true;
    // River can only be crossed on the bridge.
    if(y>905&&y<1030&&!(x>790&&x<860))return true;
    return false;
  }

  function updateCamera(){
    camera.x=clamp(Math.round(player.x-VIEW_W/2),0,WORLD_W-VIEW_W);
    camera.y=clamp(Math.round(player.y-VIEW_H/2),0,WORLD_H-VIEW_H);
    const district=districtFor(player.x,player.y);
    if(districtName&&districtName.textContent!==district)districtName.textContent=district;
  }

  function sprite(row,x,y,dir,step,jumpHeight=0){
    const sx=x-camera.x,sy=y-camera.y;
    if(sx<-40||sx>VIEW_W+40||sy<-50||sy>VIEW_H+50)return;
    const col=frames[dir][step%2];
    const lift=Math.round(jumpHeight);
    const shadowScale=Math.max(.58,1-jumpHeight/30);
    const shadowW=Math.round(20*shadowScale);
    ctx.save();
    ctx.globalAlpha=.25;
    ctx.fillStyle="#17131b";
    ctx.fillRect(Math.round(sx-shadowW/2),Math.round(sy-3),shadowW,4);
    ctx.restore();
    ctx.drawImage(sprites,col*SW,row*SH,SW,SH,
      Math.round(sx-SW/2),Math.round(sy-SH+4-lift),SW,SH);
  }

  const labelEls=new Map();
  function createLabels(){
    if(!labelLayer)return;
    labelLayer.innerHTML="";
    for(const place of places.filter(p=>p.label)){
      const el=document.createElement("button");
      el.type="button";
      el.className="world-place-label";
      el.textContent=place.label;
      el.dataset.place=place.id;
      el.addEventListener("click",()=>{show(place.name,place.text);audioFromGesture()});
      labelLayer.appendChild(el);
      labelEls.set(place.id,el);
    }
  }

  function updateLabels(){
    for(const place of places.filter(p=>p.label)){
      const el=labelEls.get(place.id);if(!el)continue;
      const sx=place.x-camera.x,sy=place.y-camera.y-48;
      const visible=sx>-60&&sx<VIEW_W+60&&sy>-40&&sy<VIEW_H+30;
      el.hidden=!visible;
      if(visible)el.style.transform=`translate(${Math.round(sx)}px,${Math.round(sy)}px) translate(-50%,-50%)`;
    }
  }

  function draw(){
    if(!ready)return;
    ctx.imageSmoothingEnabled=false;
    ctx.drawImage(bg,camera.x,camera.y,VIEW_W,VIEW_H,0,0,VIEW_W,VIEW_H);
    const actors=residents.map(n=>({...n,isPlayer:false}));
    actors.push({...player,isPlayer:true});
    actors.sort((a,b)=>a.y-b.y);
    for(const a of actors)sprite(a.row,a.x,a.y,a.dir,a.step,a.isPlayer?player.jumpHeight:0);
    updateLabels();
  }

  function move(dx,dy,dir,dt){
    player.dir=dir;
    const speed=.105*dt;
    const nx=player.x+dx*speed,ny=player.y+dy*speed;
    if(!blocked(nx,player.y))player.x=nx;
    if(!blocked(player.x,ny))player.y=ny;
    player.moving=true;
  }

  function interact(){
    audioFromGesture();
    const all=[
      ...residents.map(x=>({type:"npc",r:38,...x})),
      ...places.map(x=>({type:"place",...x}))
    ].filter(x=>dist(player,x)<(x.r||40)).sort((a,b)=>dist(player,a)-dist(player,b));
    const t=all[0];
    if(!t){show("SORA","ここには、特に何もない。");return}
    show(t.name,t.text);
  }

  function jump(){
    audioFromGesture();
    if(player.jumpActive)return;
    player.jumpActive=true;player.jumpT=0;player.jumpHeight=.01;
  }
  function updateJump(dt){
    if(!player.jumpActive){player.jumpHeight=0;return}
    player.jumpT+=dt;
    const p=Math.min(1,player.jumpT/520);
    player.jumpHeight=Math.sin(Math.PI*p)*14;
    if(p>=1){player.jumpActive=false;player.jumpT=0;player.jumpHeight=0}
  }

  const BGM_KEY="dolzore_bgm_enabled_v1";
  let bgmEnabled=localStorage.getItem(BGM_KEY)!=="off";
  let audioCtx=null,master=null,scheduler=null,nextNoteTime=0,musicStep=0;
  const melody=[523.25,659.25,783.99,659.25,587.33,698.46,830.61,698.46,523.25,659.25,739.99,659.25,493.88,587.33,659.25,null];
  const bass=[130.81,164.81,146.83,196.00];

  function tone(freq,type,when,duration,level){
    if(!audioCtx||!master||!freq)return;
    const o=audioCtx.createOscillator(),g=audioCtx.createGain();
    o.type=type;o.frequency.setValueAtTime(freq,when);
    g.gain.setValueAtTime(.0001,when);g.gain.exponentialRampToValueAtTime(level,when+.01);g.gain.exponentialRampToValueAtTime(.0001,when+duration);
    o.connect(g);g.connect(master);o.start(when);o.stop(when+duration+.02);
  }
  function scheduleMusic(){
    if(!audioCtx||!bgmEnabled)return;
    const stepDur=60/100/2;
    while(nextNoteTime<audioCtx.currentTime+.35){
      const m=melody[musicStep%melody.length];
      tone(m,"square",nextNoteTime,stepDur*.72,.055);
      if(musicStep%2===0)tone(bass[Math.floor(musicStep/4)%bass.length],"triangle",nextNoteTime,stepDur*1.65,.075);
      if(musicStep%4===2)tone(1046.5,"square",nextNoteTime,stepDur*.08,.018);
      musicStep=(musicStep+1)%melody.length;nextNoteTime+=stepDur;
    }
  }
  function startBgm(){
    if(!bgmEnabled)return;
    if(!audioCtx){
      const AC=window.AudioContext||window.webkitAudioContext;if(!AC)return;
      audioCtx=new AC();master=audioCtx.createGain();master.gain.value=.14;master.connect(audioCtx.destination);
      nextNoteTime=audioCtx.currentTime+.05;scheduler=setInterval(scheduleMusic,80);
    }
    if(audioCtx.state==="suspended")audioCtx.resume();
    updateBgmButton();
  }
  function stopBgm(){if(audioCtx&&audioCtx.state==="running")audioCtx.suspend();updateBgmButton()}
  function audioFromGesture(){if(bgmEnabled)startBgm()}
  function updateBgmButton(){
    if(!bgmButton)return;
    bgmButton.textContent=bgmEnabled?"♪ BGM ON":"♪ BGM OFF";
    bgmButton.setAttribute("aria-pressed",bgmEnabled?"true":"false");
  }
  function toggleBgm(){
    bgmEnabled=!bgmEnabled;localStorage.setItem(BGM_KEY,bgmEnabled?"on":"off");
    if(bgmEnabled)startBgm();else stopBgm();updateBgmButton();
  }

  function update(dt){
    player.moving=false;
    if(keys.has("ArrowLeft")||keys.has("a"))move(-1,0,"left",dt);
    if(keys.has("ArrowRight")||keys.has("d"))move(1,0,"right",dt);
    if(keys.has("ArrowUp")||keys.has("w"))move(0,-1,"up",dt);
    if(keys.has("ArrowDown")||keys.has("s"))move(0,1,"down",dt);
    if(player.moving){walkTick+=dt;player.step=Math.floor(walkTick/150)%2}else player.step=0;
    updateJump(dt);
    for(const n of residents){
      if(!n.wander)continue;
      n.x+=n.vx*dt/16;
      if(n.x<n.minX){n.x=n.minX;n.vx=Math.abs(n.vx);n.dir="right"}
      if(n.x>n.maxX){n.x=n.maxX;n.vx=-Math.abs(n.vx);n.dir="left"}
      n.step=Math.floor(performance.now()/320)%2;
    }
    updateCamera();
  }

  function loop(now){
    const dt=Math.min(32,now-last);last=now;update(dt);draw();requestAnimationFrame(loop);
  }

  const controlKeys=new Set(["ArrowLeft","ArrowRight","ArrowUp","ArrowDown","a","d","w","s","Enter"," "]);
  addEventListener("keydown",e=>{
    const k=e.key.length===1?e.key.toLowerCase():e.key;
    if(!controlKeys.has(k)||document.activeElement?.matches("input,textarea,select"))return;
    e.preventDefault();audioFromGesture();
    if(k==="Enter"){if(!e.repeat)interact();return}
    if(k===" "){if(!e.repeat)jump();return}
    keys.add(k);
  });
  addEventListener("keyup",e=>keys.delete(e.key.length===1?e.key.toLowerCase():e.key));

  root.querySelectorAll("[data-move]").forEach(btn=>{
    const map={left:"ArrowLeft",right:"ArrowRight",up:"ArrowUp",down:"ArrowDown"},key=map[btn.dataset.move];
    const on=()=>{audioFromGesture();keys.add(key)},off=()=>keys.delete(key);
    btn.addEventListener("pointerdown",e=>{e.preventDefault();on()});
    for(const type of ["pointerup","pointercancel","pointerleave"])btn.addEventListener(type,off);
  });
  actionButton.addEventListener("click",interact);
  if(jumpButton)jumpButton.addEventListener("click",jump);
  if(bgmButton)bgmButton.addEventListener("click",toggleBgm);
  updateBgmButton();createLabels();

  Promise.all([
    new Promise((ok,ng)=>{bg.onload=ok;bg.onerror=ng}),
    new Promise((ok,ng)=>{sprites.onload=ok;sprites.onerror=ng})
  ]).then(()=>{
    ready=true;updateCamera();draw();
    show("DOLZORE","最初の街。坂を下りると中央通り、南は川、東は駅。");
    window.__DOLZORE_WORLD__={
      ready:true,
      getState:()=>({
        player:{x:player.x,y:player.y,dir:player.dir,jumpActive:player.jumpActive,jumpHeight:player.jumpHeight},
        camera:{...camera},district:districtFor(player.x,player.y),
        residents:residents.map(x=>({name:x.name,x:x.x,y:x.y})),
        bgm:{enabled:bgmEnabled,playing:!!(audioCtx&&audioCtx.state==="running")}
      }),
      teleport:(x,y)=>{player.x=x;player.y=y;updateCamera();draw()},
      jump,interact,toggleBgm
    };
    requestAnimationFrame(loop);
  }).catch(()=>show("DOLZORE","街の読み込みに失敗しました。"));
})();