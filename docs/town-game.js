(()=>{
  const root=document.querySelector("[data-town-world]");
  if(!root)return;
  const canvas=root.querySelector("[data-town-canvas]");
  const ctx=canvas.getContext("2d",{alpha:false});
  const dialogueName=root.querySelector("[data-dialogue-name]");
  const dialogueText=root.querySelector("[data-dialogue-text]");
  const actionButton=root.querySelector("[data-action]");
  const W=480,H=270,SW=24,SH=32;

  ctx.imageSmoothingEnabled=false;

  const bg=new Image();
  const sprites=new Image();
  bg.src="assets/dolzore-town-game.png";
  sprites.src="assets/dolzore-characters.png";

  const frames={down:[0,1],left:[2,3],right:[4,5],up:[6,7]};
  const player={name:"SORA",row:0,x:252,y:184,dir:"down",step:0,moving:false};
  const residents=[
    {name:"MELO",row:1,x:414,y:137,dir:"down",step:0,text:"20秒だけ聴けば、だいたいの空気はつかめるよ。"},
    {name:"YUZU",row:2,x:216,y:139,dir:"down",step:0,text:"掲示板には、調べたことをまとめて貼ってるよ。"},
    {name:"PON",row:3,x:128,y:181,dir:"right",step:0,text:"いま散歩中。行き先は、あとで決める。",wander:true,vx:.22,minX:118,maxX:168},
  ];
  const buildings=[
    {name:"MUSIC",x:394,y:126,r:25,url:"music/",text:"MUSICへ入ります。"},
    {name:"JOURNAL",x:236,y:126,r:25,url:"journal/",text:"JOURNALへ入ります。"},
    {name:"CAFE",x:90,y:123,r:26,url:null,text:"CAFEは、ただいま準備中です。"},
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
    return blockers.some(([x1,y1,x2,y2])=>x>x1-6&&x<x2+6&&y>y1-4&&y<y2+5);
  }
  function sprite(row,x,y,dir,step){
    const col=frames[dir][step%2];
    ctx.save();
    ctx.globalAlpha=.28;
    ctx.fillStyle="#17131b";
    ctx.fillRect(Math.round(x-8),Math.round(y-3),16,4);
    ctx.restore();
    ctx.drawImage(sprites,col*SW,row*SH,SW,SH,Math.round(x-SW/2),Math.round(y-SH+2),SW,SH);
  }
  function draw(){
    if(!ready)return;
    ctx.imageSmoothingEnabled=false;
    ctx.drawImage(bg,0,0,W,H);
    for(const n of residents)sprite(n.row,n.x,n.y,n.dir,n.step);
    sprite(player.row,player.x,player.y,player.dir,player.step);
  }
  function move(dx,dy,dir,dt){
    player.dir=dir;
    const speed=.09*dt;
    const nx=player.x+dx*speed,ny=player.y+dy*speed;
    if(!blocked(nx,player.y))player.x=nx;
    if(!blocked(player.x,ny))player.y=ny;
    player.moving=true;
  }
  function nearestInteraction(){
    const candidates=[
      ...residents.map(x=>({type:"npc",...x})),
      ...buildings.map(x=>({type:"building",...x}))
    ].filter(x=>dist(player,x)<x.r??32);
    candidates.sort((a,b)=>dist(player,a)-dist(player,b));
    return candidates[0]||null;
  }
  function interact(){
    const all=[
      ...residents.map(x=>({type:"npc",r:32,...x})),
      ...buildings.map(x=>({type:"building",...x}))
    ].filter(x=>dist(player,x)<(x.r||32)).sort((a,b)=>dist(player,a)-dist(player,b));
    const t=all[0];
    if(!t){show("SORA","ここには、特に何もない。");return;}
    if(t.type==="npc"){show(t.name,t.text);return;}
    show(t.name,t.text);
    if(t.url)setTimeout(()=>{location.href=t.url},180);
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

    for(const n of residents){
      if(!n.wander)continue;
      n.x+=n.vx*dt/16;
      if(n.x<n.minX){n.x=n.minX;n.vx=Math.abs(n.vx);n.dir="right"}
      if(n.x>n.maxX){n.x=n.maxX;n.vx=-Math.abs(n.vx);n.dir="left"}
      n.step=Math.floor(performance.now()/320)%2;
    }
  }
  function loop(now){
    const dt=Math.min(32,now-last);last=now;
    update(dt);draw();
    requestAnimationFrame(loop);
  }

  const controlKeys=new Set(["ArrowLeft","ArrowRight","ArrowUp","ArrowDown","a","d","w","s","Enter"," "]);
  addEventListener("keydown",e=>{
    const k=e.key.length===1?e.key.toLowerCase():e.key;
    if(!controlKeys.has(k)||document.activeElement?.matches("input,textarea,select"))return;
    e.preventDefault();
    if(k==="Enter"||k===" "){interact();return;}
    keys.add(k);
  });
  addEventListener("keyup",e=>keys.delete(e.key.length===1?e.key.toLowerCase():e.key));

  root.querySelectorAll("[data-move]").forEach(btn=>{
    const map={left:"ArrowLeft",right:"ArrowRight",up:"ArrowUp",down:"ArrowDown"};
    const key=map[btn.dataset.move];
    const on=()=>keys.add(key),off=()=>keys.delete(key);
    btn.addEventListener("pointerdown",e=>{e.preventDefault();on()});
    for(const type of ["pointerup","pointercancel","pointerleave"])btn.addEventListener(type,off);
  });
  actionButton.addEventListener("click",interact);

  root.querySelector('[data-place-label="cafe"]').addEventListener("click",e=>{
    e.preventDefault();show("CAFE","CAFEは、ただいま準備中です。");
  });

  Promise.all([
    new Promise((ok,ng)=>{bg.onload=ok;bg.onerror=ng}),
    new Promise((ok,ng)=>{sprites.onload=ok;sprites.onerror=ng})
  ]).then(()=>{
    ready=true;draw();show("DOLZORE","SORAを動かして、町を歩けます。");
    window.__DOLZORE_WORLD__={
      ready:true,
      getState:()=>({player:{x:player.x,y:player.y,dir:player.dir},residents:residents.map(x=>({name:x.name,x:x.x,y:x.y}))})
    };
    requestAnimationFrame(loop);
  }).catch(()=>show("DOLZORE","町の読み込みに失敗しました。下のリンクから移動できます。"));
})();