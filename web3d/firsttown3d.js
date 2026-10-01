import * as THREE from 'three';
import { GLTFLoader } from 'three/examples/jsm/loaders/GLTFLoader.js';

import bA from './models/building-a.glb';
import bB from './models/building-b.glb';
import bC from './models/building-c.glb';
import bD from './models/building-d.glb';
import bE from './models/building-e.glb';
import bF from './models/building-f.glb';
import chimneyLarge from './models/chimney-large.glb';
import chimneyMedium from './models/chimney-medium.glb';
import tank from './models/detail-tank.glb';
import pipeLong from './models/pipe-large-long.glb';
import pipeBend from './models/pipe-large-bend.glb';
import catwalkStairs from './models/catwalk-stairs.glb';
import machine from './models/machine.glb';

const qa = new URLSearchParams(location.search).get('qa') === '1';

const scene = new THREE.Scene();
scene.background = new THREE.Color(0x8d9aa1);
scene.fog = new THREE.FogExp2(0x8d9aa1, 0.0062);

const camera = new THREE.PerspectiveCamera(60, innerWidth / innerHeight, 0.1, 500);
const renderer = new THREE.WebGLRenderer({ antialias: true });
renderer.setPixelRatio(Math.min(devicePixelRatio, 2));
renderer.setSize(innerWidth, innerHeight);
renderer.shadowMap.enabled = true;
renderer.shadowMap.type = THREE.PCFSoftShadowMap;
renderer.outputColorSpace = THREE.SRGBColorSpace;
document.getElementById('app').appendChild(renderer.domElement);

const hemi = new THREE.HemisphereLight(0xd4dde2, 0x4b4236, 2.2);
scene.add(hemi);
const sun = new THREE.DirectionalLight(0xffddad, 3.2);
sun.position.set(-35, 55, -25);
sun.castShadow = true;
sun.shadow.mapSize.set(2048, 2048);
sun.shadow.camera.left = -90; sun.shadow.camera.right = 90; sun.shadow.camera.top = 90; sun.shadow.camera.bottom = -90;
scene.add(sun);

function canvasTex(base, line, mode='stone') {
  const c = document.createElement('canvas'); c.width = c.height = 256;
  const x = c.getContext('2d'); x.fillStyle = base; x.fillRect(0,0,256,256);
  x.strokeStyle = line; x.lineWidth = mode === 'road' ? 3 : 4;
  if (mode === 'road') {
    for (let y=0;y<256;y+=32) { x.beginPath(); x.moveTo(0,y); x.lineTo(256,y); x.stroke(); }
    for (let y=0;y<256;y+=32) for (let xx=(y/32)%2?16:0;xx<256;xx+=32) { x.beginPath(); x.moveTo(xx,y); x.lineTo(xx,y+32); x.stroke(); }
  } else {
    for (let y=0;y<256;y+=42) { x.beginPath(); x.moveTo(0,y); x.lineTo(256,y); x.stroke(); }
    for (let y=0;y<256;y+=42) for (let xx=(y/42)%2?42:0;xx<256;xx+=84) { x.beginPath(); x.moveTo(xx,y); x.lineTo(xx,y+42); x.stroke(); }
  }
  const t = new THREE.CanvasTexture(c); t.wrapS=t.wrapT=THREE.RepeatWrapping; t.colorSpace=THREE.SRGBColorSpace; return t;
}
const stoneTex = canvasTex('#6b5e4e','#4b4035');
const paleTex = canvasTex('#998b72','#766b59');
const roadTex = canvasTex('#59544d','#45413c','road');
const stoneMat = new THREE.MeshStandardMaterial({ map: stoneTex, roughness: 0.9 });
const paleMat = new THREE.MeshStandardMaterial({ map: paleTex, roughness: 0.9 });
const darkStoneMat = new THREE.MeshStandardMaterial({ color: 0x393735, roughness: 0.95 });
const metalMat = new THREE.MeshStandardMaterial({ color:0x40484c, metalness:0.72, roughness:0.38 });
const copperMat = new THREE.MeshStandardMaterial({ color:0x8b4b25, metalness:0.62, roughness:0.38 });
const roadMat = new THREE.MeshStandardMaterial({ map: roadTex, roughness:0.96 });
roadTex.repeat.set(10,30);
const groundMat = new THREE.MeshStandardMaterial({ color:0x4f5e3f, roughness:1 });
const waterMat = new THREE.MeshPhysicalMaterial({ color:0x174c60, roughness:0.16, metalness:0.05, transparent:true, opacity:0.84 });
const glowMat = new THREE.MeshStandardMaterial({color:0xffb14a, emissive:0xff7d19, emissiveIntensity:2.5});

const colliders = [];
function box(name, pos, size, mat, solid=true) {
  const m = new THREE.Mesh(new THREE.BoxGeometry(...size), mat);
  m.name=name; m.position.set(...pos); m.castShadow=true; m.receiveShadow=true; scene.add(m);
  if (solid) colliders.push(new THREE.Box3().setFromObject(m));
  return m;
}
function cyl(name,pos,r,h,mat,solid=true) {
  const m=new THREE.Mesh(new THREE.CylinderGeometry(r,r*1.04,h,18),mat);
  m.name=name; m.position.set(...pos); m.castShadow=true; m.receiveShadow=true; scene.add(m);
  if(solid) colliders.push(new THREE.Box3().setFromObject(m)); return m;
}

box('ground',[0,-.6,0],[150,1,120],groundMat,false);
box('main avenue',[0,.02,2],[17,.25,104],roadMat,false);
box('market road',[-25,.02,4],[40,.25,14],roadMat,false);
box('civic road',[27,.02,-25],[44,.25,14],roadMat,false);
box('canal',[31,-.18,18],[16,.16,84],waterMat,false);
for (const z of [-16,15,46]) {
  box('bridge',[31,.5,z],[18,.8,7],paleMat,true);
  box('bridge rail',[23.2,1.45,z],[.5,1.9,7],metalMat,true);
  box('bridge rail',[38.8,1.45,z],[.5,1.9,7],metalMat,true);
}
box('west terrace',[-53,2.4,0],[34,4.8,112],darkStoneMat,true);
box('north ridge',[0,2.8,55],[120,5.6,12],darkStoneMat,true);

const manager = new THREE.LoadingManager();
const pixel = 'data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAQAAAC1HAwCAAAAC0lEQVR42mNk+A8AAQUBAScY42YAAAAASUVORK5CYII=';
manager.setURLModifier(url => /\.(png|jpg|jpeg)$/i.test(url) ? pixel : url);
const loader = new GLTFLoader(manager);

const modelUrls = {bA,bB,bC,bD,bE,bF,chimneyLarge,chimneyMedium,tank,pipeLong,pipeBend,catwalkStairs,machine};
const models = {};
async function loadAll() {
  for (const [k,url] of Object.entries(modelUrls)) {
    const g = await loader.loadAsync(url);
    const root=g.scene;
    root.traverse(o => {
      if(o.isMesh) {
        o.castShadow=o.receiveShadow=true;
        const lower=(o.name||'').toLowerCase();
        o.material = lower.includes('glass') ? new THREE.MeshStandardMaterial({color:0x688a94,roughness:.25,metalness:.15}) : stoneMat.clone();
      }
    });
    models[k]=root;
  }
}
function place(key, x,z, scale=8, rot=0, matKind='stone') {
  const o=models[key].clone(true);
  o.position.set(x,0,z); o.rotation.y=rot; o.scale.setScalar(scale);
  o.traverse(m=>{if(m.isMesh) m.material = matKind==='metal'?metalMat.clone():matKind==='copper'?copperMat.clone():matKind==='dark'?darkStoneMat.clone():stoneMat.clone();});
  scene.add(o);
  const b=new THREE.Box3().setFromObject(o); colliders.push(b);
  return o;
}
function decorative(key,x,y,z,scale=8,rot=0,matKind='metal') {
  const o=models[key].clone(true); o.position.set(x,y,z); o.rotation.y=rot; o.scale.setScalar(scale);
  o.traverse(m=>{if(m.isMesh)m.material=matKind==='copper'?copperMat.clone():metalMat.clone();});
  scene.add(o); return o;
}

function lamp(x,z) {
  cyl('lamp post',[x,1.7,z],.10,3.4,metalMat,true);
  const b=new THREE.Mesh(new THREE.SphereGeometry(.28,12,8),glowMat); b.position.set(x,3.55,z); scene.add(b);
  const l=new THREE.PointLight(0xffa13f,12,9,2); l.position.set(x,3.5,z); scene.add(l);
}
function sign(text,x,y,z,rot=0) {
  const c=document.createElement('canvas');c.width=640;c.height=160;const cx=c.getContext('2d');
  cx.fillStyle='#1e1914';cx.fillRect(0,0,640,160);cx.strokeStyle='#bd8a50';cx.lineWidth=10;cx.strokeRect(8,8,624,144);
  cx.fillStyle='#f4e2be';cx.font='700 48px sans-serif';cx.textAlign='center';cx.textBaseline='middle';cx.fillText(text,320,80);
  const tex=new THREE.CanvasTexture(c);tex.colorSpace=THREE.SRGBColorSpace;
  const m=new THREE.Mesh(new THREE.PlaneGeometry(7,1.75),new THREE.MeshBasicMaterial({map:tex,side:THREE.DoubleSide}));
  m.position.set(x,y,z);m.rotation.y=rot;scene.add(m);
}
function marketStall(x,z,color) {
  box('stall counter',[x,1.0,z],[3.3,1.2,1.2],copperMat,true);
  const canopy=new THREE.Mesh(new THREE.BoxGeometry(4,.25,3.2),new THREE.MeshStandardMaterial({color,roughness:.8}));
  canopy.position.set(x,3,z);canopy.castShadow=true;scene.add(canopy);
  for(const dx of [-1.6,1.6]) cyl('stall post',[x+dx,1.65,z],.07,3.3,metalMat,true);
}

const player = new THREE.Group();
const body=new THREE.Mesh(new THREE.CapsuleGeometry(.42,1.0,6,10),new THREE.MeshStandardMaterial({color:0x345974,roughness:.75}));
body.position.y=1.1; body.castShadow=true; player.add(body);
const head=new THREE.Mesh(new THREE.SphereGeometry(.36,16,12),new THREE.MeshStandardMaterial({color:0xc89470,roughness:.9}));
head.position.y=2.05;head.castShadow=true;player.add(head);
const pack=new THREE.Mesh(new THREE.BoxGeometry(.58,.75,.28),new THREE.MeshStandardMaterial({color:0x76552f,roughness:.9}));
pack.position.set(0,1.25,.38);player.add(pack);
player.position.set(0,0,-48);scene.add(player);

const state = {yaw:0, pitch:.16, y:0, vy:0, onGround:true, keys:{}};
window.__playerPos = () => ({x:player.position.x,y:player.position.y,z:player.position.z});

function blocked(nx,nz) {
  const p=new THREE.Box3(new THREE.Vector3(nx-.42,.1,nz-.42),new THREE.Vector3(nx+.42,2.3,nz+.42));
  return colliders.some(b=>p.intersectsBox(b));
}
function district(x,z) {
  if(z>48)return'DEEP WORKS';
  if(x<-35)return'UPPER INDUSTRIAL';
  if(x>38)return'CANAL WORKSHOPS';
  if(z<-28)return'REPUBLIC DISTRICT';
  if(x<-10&&z>8)return'FOUNDRY DISTRICT';
  if(x<-10)return'MARKET DISTRICT';
  if(x>20)return'CANAL QUARTER';
  return'CENTRAL AVENUE';
}

window.addEventListener('keydown',e=>{state.keys[e.code]=true;if(e.code==='Space'&&state.onGround){state.vy=7;state.onGround=false;}});
window.addEventListener('keyup',e=>state.keys[e.code]=false);
renderer.domElement.addEventListener('click',()=>{if(!qa)renderer.domElement.requestPointerLock();});
window.addEventListener('mousemove',e=>{if(document.pointerLockElement===renderer.domElement){state.yaw-=e.movementX*.0023;state.pitch-=e.movementY*.0017;state.pitch=THREE.MathUtils.clamp(state.pitch,-.1,.65);}});

function makeScene() {
  // Industrial stone town blocks
  place('bA',-23,-26,8,0,'stone');
  place('bC', 22,-27,8,Math.PI,'stone');
  place('bF',-24, -4,8,0,'dark');
  place('bB', 18, -4,8,Math.PI,'stone');
  place('bE',-22, 22,8,0,'dark');
  place('bD', 18, 24,8,Math.PI,'stone');
  place('bA', 48,-18,7.4,Math.PI/2,'dark');
  place('bC', 48,  5,7.4,Math.PI/2,'stone');
  place('bF', 49, 30,7.4,Math.PI/2,'dark');
  place('bB',-51,-31,7.6,Math.PI/2,'stone');
  place('bD',-51, 15,7.6,Math.PI/2,'stone');

  // civic plaza
  box('civic plaza',[5,.08,-19],[25,.16,19],paleMat,false);
  cyl('monument base',[5,.8,-19],2.1,1.6,darkStoneMat,true);
  cyl('monument',[5,3.5,-19],.75,5.5,copperMat,true);

  // Foundry skyline and machinery
  decorative('chimneyLarge',-47,5,-5,8,0,'metal');
  decorative('chimneyLarge',-47,5,18,8,0,'metal');
  decorative('chimneyMedium',-39,5,35,8,0,'metal');
  decorative('tank',-34,0,25,8,0,'copper');
  decorative('machine',-31,0,17,7,0,'metal');
  decorative('pipeLong',-29,6.5,12,8,Math.PI/2,'metal');
  decorative('pipeBend',-18,6.5,12,8,0,'metal');
  decorative('catwalkStairs',-38,0,-18,8,Math.PI/2,'metal');

  // mine gate
  box('mine cliff',[0,12,61],[54,20,12],darkStoneMat,true);
  box('gate left',[-8,7,54.4],[10,14,3],stoneMat,true);
  box('gate right',[8,7,54.4],[10,14,3],stoneMat,true);
  box('gate lintel',[0,14,54.4],[26,4,3],stoneMat,true);
  box('mine void',[0,6.2,52.8],[7.5,10,.5],new THREE.MeshStandardMaterial({color:0x050505}),true);

  // market and street furniture
  [-33,-28.5,-24,-19.5,-15].forEach((x,i)=>marketStall(x,4,i%2?0x7d3c40:0xa16b32));
  for(let z=-41;z<=43;z+=12){lamp(-7,z);lamp(7,z);}
  for(let z=-15;z<=47;z+=6){
    box('canal rail',[22.3,1.0,z],[.22,1.6,4.4],metalMat,true);
    box('canal rail',[39.7,1.0,z],[.22,1.6,4.4],metalMat,true);
  }
  // large overhead foundry pipe crossing avenue
  box('pipe cross',[0,7.6,13],[19,1.1,1.1],metalMat,true);
  box('pipe support',[-8.2,3.8,13],[1,7.6,1],metalMat,true);
  box('pipe support',[8.2,3.8,13],[1,7.6,1],metalMat,true);

  sign('IRON BASIN',0,7,-48,0);
  sign('DEEP WORKS',0,17,52.7,Math.PI);
  sign('FOUNDRY',-23,7,13,Math.PI);
  sign('REPUBLIC HALL',20,7,-34,0);
}

const start=document.getElementById('start');
document.getElementById('enter').onclick=()=>{start.style.display='none';renderer.domElement.requestPointerLock();};
if(qa) start.style.display='none';

const clock=new THREE.Clock();
function update() {
  const dt=Math.min(.033,clock.getDelta());
  if(!qa){
    let f=(state.keys.KeyW?1:0)-(state.keys.KeyS?1:0), s=(state.keys.KeyD?1:0)-(state.keys.KeyA?1:0);
    const l=Math.hypot(f,s)||1;f/=l;s/=l;
    const speed=(state.keys.ShiftLeft||state.keys.ShiftRight)?9.2:5.8;
    const sy=Math.sin(state.yaw), cy=Math.cos(state.yaw);
    const dx=(s*cy+f*sy)*speed*dt, dz=(f*cy-s*sy)*speed*dt;
    if(!blocked(player.position.x+dx,player.position.z+dz)){player.position.x+=dx;player.position.z+=dz;}
    state.vy-=18*dt; state.y+=state.vy*dt; if(state.y<0){state.y=0;state.vy=0;state.onGround=true;} else state.onGround=false;
    player.position.y=state.y;
  }
  document.getElementById('district').textContent=district(player.position.x,player.position.z);
  document.getElementById('coords').textContent=`X ${player.position.x.toFixed(1)} / Z ${player.position.z.toFixed(1)}`;

  if(qa){
    camera.position.set(0,26,-66);
    camera.lookAt(0,4,10);
  } else {
    const target=new THREE.Vector3(player.position.x,player.position.y+1.4,player.position.z);
    const dir=new THREE.Vector3(Math.sin(state.yaw)*Math.cos(state.pitch),Math.sin(state.pitch),Math.cos(state.yaw)*Math.cos(state.pitch));
    camera.position.copy(target).addScaledVector(dir,-8.4).add(new THREE.Vector3(0,3.8,0));
    camera.lookAt(target);
  }
  renderer.render(scene,camera);
}

(async()=>{
  await loadAll();
  makeScene();
  window.__DOLZORE_READY__=true;
  renderer.setAnimationLoop(update);
})();

addEventListener('resize',()=>{camera.aspect=innerWidth/innerHeight;camera.updateProjectionMatrix();renderer.setSize(innerWidth,innerHeight);});
