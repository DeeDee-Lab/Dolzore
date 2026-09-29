(()=>{
  const root=document.querySelector("[data-town-world]");
  if(!root)return;

  const canvas=root.querySelector("[data-town-canvas]");
  const ctx=canvas.getContext("2d",{alpha:false});
  const dialogueName=root.querySelector("[data-dialogue-name]");
  const dialogueText=root.querySelector("[data-dialogue-text]");
  const actionButton=root.querySelector("[data-action]");
  const jumpButton=root.querySelector("[data-jump]");
  const bgmButton=root.querySelector("[data-bgm-toggle]");

  const W=480,H=270,SW=32,SH=40;
  ctx.imageSmoothingEnabled=false;

  const bg=new Image();
  const sprites=new Image();
  bg.src="assets/dolzore-town-game.png";
  sprites.src="assets/dolzore-characters.png";

  const frames={down:[0,1],left:[2,3],right:[4,5],up:[6,7]};
  const player={
    name:"SORA",row:0,x:252,y:184,dir:"down",step:0,moving:false,
    jumpActive:false,jumpT:0,jumpHeight:0
  };

  const residents=[
    {name:"MELO",row:1,x:414,y:137,dir:"down",step:0,text:"今の音、前より少し軽い。たぶん気のせいじゃない。"},
    {name:"YUZU",row:2,x:216,y:139,dir:"down",step:0,text:"今日の掲示は二つ。確認できた話だけ貼ってあるよ。"},
    {name:"PON",row:3,x:128,y:181,dir:"right",step:0,text:"駅に行く途中。……だった気がする。",wander:true,vx:.22,minX:118,maxX:168},
  ];

  const buildings=[
    {name:"MUSIC",x:394,y:126,r:25,url:"music/",text:"MUSICへ入ります。"},
    {name:"JOURNAL",x:236,y:126,r:25,url:"journal/",text:"JOURNALへ入ります。"},
    {name:"CAFE",x:90,y:123,r:26,url:null,text:"CAFEは、まだ準備中。中からラジオの音だけ聞こえる。"},
  ];

  const blockers=[
    [30,40,153,122],[170,34,306,126],[332,31,458,130],
    [75,136,126,169],[224,96,276,122],[282,82,302,116],[387,91,411,121],
  ];

  const keys=new Set();
  let ready=false,last=performance.now(),walkTick=0;

  function show(name,text){
    dialogueName.textContent=name;
    dialogueText.textContent=text;
  }

  function dist(a,b){return Math.hypot(a.x-b.x,a.y-b.y)}

  function blocked(x,y){
    if(x<12||x>468||y<105||y>255)return true;
    return blockers.some(([x1,y1,x2,y2])=>x>x1-7&&x<x2+7&&y>y1-5&&y<y2+6);
  }

  function sprite(row,x,y,dir,step,jumpHeight=0){
    const col=frames[dir][step%2];
    const lift=Math.round(jumpHeight);
    const shadowScale=Math.max(.58,1-jumpHeight/30);
    const shadowW=Math.round(20*shadowScale);

    ctx.save();
    ctx.globalAlpha=.25;
    ctx.fillStyle="#17131b";
    ctx.fillRect(Math.round(x-shadowW/2),Math.round(y-3),shadowW,4);
    ctx.restore();

    ctx.drawImage(
      sprites,
      col*SW,row*SH,SW,SH,
      Math.round(x-SW/2),Math.round(y-SH+4-lift),SW,SH
    );
  }

  function draw(){
    if(!ready)return;
    ctx.imageSmoothingEnabled=false;
    ctx.drawImage(bg,0,0,W,H);

    // depth ordering by feet position
    const actors=residents.map(n=>({...n,jumpHeight:0,isPlayer:false}));
    actors.push({...player,isPlayer:true});
    actors.sort((a,b)=>a.y-b.y);
    for(const a of actors){
      sprite(a.row,a.x,a.y,a.dir,a.step,a.isPlayer?player.jumpHeight:0);
    }
  }

  function move(dx,dy,dir,dt){
    player.dir=dir;
    const speed=.09*dt;
    const nx=player.x+dx*speed,ny=player.y+dy*speed;
    if(!blocked(nx,player.y))player.x=nx;
    if(!blocked(player.x,ny))player.y=ny;
    player.moving=true;
  }

  function interact(){
    audioFromGesture();
    const all=[
      ...residents.map(x=>({type:"npc",r:35,...x})),
      ...buildings.map(x=>({type:"building",...x}))
    ].filter(x=>dist(player,x)<(x.r||32))
     .sort((a,b)=>dist(player,a)-dist(player,b));

    const t=all[0];
    if(!t){
      show("SORA","ここには、特に何もない。");
      return;
    }

    if(t.type==="npc"){
      show(t.name,t.text);
      return;
    }

    show(t.name,t.text);
    if(t.url)setTimeout(()=>{location.href=t.url},180);
  }

  function jump(){
    audioFromGesture();
    if(player.jumpActive)return;
    player.jumpActive=true;
    player.jumpT=0;
    player.jumpHeight=.01;
    show("SORA","よいしょ。");
  }

  function updateJump(dt){
    if(!player.jumpActive){player.jumpHeight=0;return;}
    player.jumpT+=dt;
    const duration=520;
    const p=Math.min(1,player.jumpT/duration);
    player.jumpHeight=Math.sin(Math.PI*p)*14;
    if(p>=1){
      player.jumpActive=false;
      player.jumpT=0;
      player.jumpHeight=0;
    }
  }

  // -------------------------------------------------------
  // ORIGINAL TOWN BGM — procedural Web Audio, no samples.
  // -------------------------------------------------------
  const BGM_KEY="dolzore_bgm_enabled_v1";
  let bgmEnabled=localStorage.getItem(BGM_KEY)!=="off";
  let audioCtx=null,master=null,scheduler=null,nextNoteTime=0,musicStep=0;

  const melody=[
    523.25,659.25,783.99,659.25,
    587.33,698.46,830.61,698.46,
    523.25,659.25,739.99,659.25,
    493.88,587.33,659.25,null
  ];
  const bass=[130.81,164.81,146.83,196.00];

  function tone(freq,type,when,duration,level){
    if(!audioCtx||!master||!freq)return;
    const o=audioCtx.createOscillator();
    const g=audioCtx.createGain();
    o.type=type;
    o.frequency.setValueAtTime(freq,when);
    g.gain.setValueAtTime(0.0001,when);
    g.gain.exponentialRampToValueAtTime(level,when+.01);
    g.gain.exponentialRampToValueAtTime(0.0001,when+duration);
    o.connect(g);g.connect(master);
    o.start(when);o.stop(when+duration+.02);
  }

  function scheduleMusic(){
    if(!audioCtx||!bgmEnabled)return;
    const stepDur=60/100/2; // 100 BPM, eighth-note grid
    while(nextNoteTime<audioCtx.currentTime+.35){
      const m=melody[musicStep%melody.length];
      tone(m,"square",nextNoteTime,stepDur*.72,.055);
      if(musicStep%2===0){
        const b=bass[Math.floor(musicStep/4)%bass.length];
        tone(b,"triangle",nextNoteTime,stepDur*1.65,.075);
      }
      if(musicStep%4===2){
        tone(1046.5,"square",nextNoteTime,stepDur*.08,.018);
      }
      musicStep=(musicStep+1)%melody.length;
      nextNoteTime+=stepDur;
    }
  }

  function startBgm(){
    if(!bgmEnabled)return;
    if(!audioCtx){
      const AC=window.AudioContext||window.webkitAudioContext;
      if(!AC)return;
      audioCtx=new AC();
      master=audioCtx.createGain();
      master.gain.value=.14;
      master.connect(audioCtx.destination);
      nextNoteTime=audioCtx.currentTime+.05;
      scheduler=setInterval(scheduleMusic,80);
    }
    if(audioCtx.state==="suspended")audioCtx.resume();
    updateBgmButton();
  }

  function stopBgm(){
    if(audioCtx&&audioCtx.state==="running")audioCtx.suspend();
    updateBgmButton();
  }

  function audioFromGesture(){
    if(bgmEnabled)startBgm();
  }

  function updateBgmButton(){
    if(!bgmButton)return;
    const playing=!!(bgmEnabled&&audioCtx&&audioCtx.state==="running");
    bgmButton.textContent=bgmEnabled?(playing?"♪ BGM ON":"♪ BGM ON"):"♪ BGM OFF";
    bgmButton.setAttribute("aria-pressed",bgmEnabled?"true":"false");
  }

  function toggleBgm(){
    bgmEnabled=!bgmEnabled;
    localStorage.setItem(BGM_KEY,bgmEnabled?"on":"off");
    if(bgmEnabled)startBgm();else stopBgm();
    updateBgmButton();
  }

  function update(dt){
    player.moving=false;
    if(keys.has("ArrowLeft")||keys.has("a"))move(-1,0,"left",dt);
    if(keys.has("ArrowRight")||keys.has("d"))move(1,0,"right",dt);
    if(keys.has("ArrowUp")||keys.has("w"))move(0,-1,"up",dt);
    if(keys.has("ArrowDown")||keys.has("s"))move(0,1,"down",dt);

    if(player.moving){
      walkTick+=dt;
      player.step=Math.floor(walkTick/150)%2;
    }else player.step=0;

    updateJump(dt);

    for(const n of residents){
      if(!n.wander)continue;
      n.x+=n.vx*dt/16;
      if(n.x<n.minX){n.x=n.minX;n.vx=Math.abs(n.vx);n.dir="right"}
      if(n.x>n.maxX){n.x=n.maxX;n.vx=-Math.abs(n.vx);n.dir="left"}
      n.step=Math.floor(performance.now()/320)%2;
    }
  }

  function loop(now){
    const dt=Math.min(32,now-last);
    last=now;
    update(dt);
    draw();
    requestAnimationFrame(loop);
  }

  const controlKeys=new Set([
    "ArrowLeft","ArrowRight","ArrowUp","ArrowDown",
    "a","d","w","s","Enter"," "
  ]);

  addEventListener("keydown",e=>{
    const k=e.key.length===1?e.key.toLowerCase():e.key;
    if(!controlKeys.has(k)||document.activeElement?.matches("input,textarea,select"))return;
    e.preventDefault();
    audioFromGesture();

    if(k==="Enter"){
      if(!e.repeat)interact();
      return;
    }
    if(k===" "){
      if(!e.repeat)jump();
      return;
    }
    keys.add(k);
  });

  addEventListener("keyup",e=>{
    const k=e.key.length===1?e.key.toLowerCase():e.key;
    keys.delete(k);
  });

  root.querySelectorAll("[data-move]").forEach(btn=>{
    const map={left:"ArrowLeft",right:"ArrowRight",up:"ArrowUp",down:"ArrowDown"};
    const key=map[btn.dataset.move];
    const on=()=>{audioFromGesture();keys.add(key)};
    const off=()=>keys.delete(key);
    btn.addEventListener("pointerdown",e=>{e.preventDefault();on()});
    for(const type of ["pointerup","pointercancel","pointerleave"])btn.addEventListener(type,off);
  });

  actionButton.addEventListener("click",interact);
  if(jumpButton)jumpButton.addEventListener("click",jump);
  if(bgmButton)bgmButton.addEventListener("click",toggleBgm);

  root.querySelector('[data-place-label="cafe"]').addEventListener("click",e=>{
    e.preventDefault();
    audioFromGesture();
    show("CAFE","CAFEは、まだ準備中。中からラジオの音だけ聞こえる。");
  });

  updateBgmButton();

  Promise.all([
    new Promise((ok,ng)=>{bg.onload=ok;bg.onerror=ng}),
    new Promise((ok,ng)=>{sprites.onload=ok;sprites.onerror=ng})
  ]).then(()=>{
    ready=true;
    draw();
    show("DOLZORE","SORAを動かして、町を歩けます。Spaceでジャンプ。");
    window.__DOLZORE_WORLD__={
      ready:true,
      getState:()=>({
        player:{
          x:player.x,y:player.y,dir:player.dir,
          jumpActive:player.jumpActive,jumpHeight:player.jumpHeight
        },
        residents:residents.map(x=>({name:x.name,x:x.x,y:x.y})),
        bgm:{
          enabled:bgmEnabled,
          playing:!!(audioCtx&&audioCtx.state==="running")
        }
      }),
      jump,
      interact,
      toggleBgm
    };
    requestAnimationFrame(loop);
  }).catch(()=>{
    show("DOLZORE","町の読み込みに失敗しました。下のリンクから移動できます。");
  });
})();