'use strict';
// Requires separate HQ-approved disposable environment. No fetch, install, push or deployment. Lock only covers cooperating local runs; no process-tree sandbox.
const pins={
  "base": "1db115626e657eab5d171a34fd09bf4bbdfc6034",
  "stage": "d6496726568ed710a2621a0faab3450929b8e83e",
  "market": {
    "workflow": ".github/workflows/update-market-snapshot.yml",
    "workflowBlob": "500b3e5477516792405bfb7044842402769994ed",
    "script": "scripts/update_market_snapshot.py",
    "scriptBlob": "27365acd158d598bbbdb6f1fed64051486a8ebcd",
    "data": "docs/data/smartbuy-projectors.json",
    "dataBlob": "563b0e3cf7bf9b284683d42e2e207cfef85ee510",
    "test": "tests/test_market_snapshot_contract.py",
    "testBlob": "859bdbbc7194d8d3a657046e4ca1697d1b49a0b7"
  },
  "media": {
    "workflow": ".github/workflows/update-product-media.yml",
    "workflowBlob": "bf4b616b9038f88a42eb1998a31382aa2f2d4311",
    "script": "scripts/update_product_media.py",
    "scriptBlob": "75efdcd3e2e827f9a4524679f80cef58fbfda94e",
    "data": "docs/data/product-media.json",
    "dataBlob": "d57f8a32e1d386c53222f52c4fbc173e41e54f7b"
  }
};
function requireOk(ok,msg){if(!ok)throw Error(msg)}
function plan(profile,event){
 const p=pins[profile];requireOk(p&&typeof p==="object"&&p.script,"unknown profile");
 requireOk(["pull_request","push","schedule","workflow_dispatch"].includes(event),"unknown event");
 requireOk(profile!=="media"||event!=="pull_request","media PR trigger absent");
 const marketTest=["-m","unittest","discover","-s","tests","-p","test_market_snapshot_contract.py","-v"];
 const commands=profile==="market"?[
  {name:"compile",phase:"contract",args:["-m","py_compile",p.script],seconds:300},
  {name:"contract_before",phase:"contract",args:marketTest,seconds:300},
  ...(event==="pull_request"?[]:[{name:"collect",phase:"update",args:[p.script,"--output",p.data],seconds:900},{name:"contract_after",phase:"update",args:marketTest,seconds:900}])
 ]:[{name:"collect",phase:"update",args:[p.script,"--file",p.data],seconds:600},{name:"contract_after",phase:"update",args:["-m","unittest","discover","-s","tests","-p","test_*.py","-v"],seconds:600}];
 return {profile,event,commands,collect:event!=="pull_request",target:p.data,phaseSeconds:{contract:300,update:profile==="media"?600:900},deadlineSeconds:profile==="media"?600:event==="pull_request"?300:1200};
}
// Strict final unittest summary; do not accept a stray OK from earlier output.
function allTestsPass(log){
 if(typeof log!=="string")return false;
 const lines=log.split(/\r?\n/).map(x=>x.trim()).filter(Boolean);
 const runs=lines.filter(x=>/^Ran\s+\d+\s+tests?\b/.test(x));
 const oks=lines.filter(x=>/^OK(?:\s|$)/.test(x));
 if(runs.length!==1||oks.length!==1||lines.at(-1)!=="OK")return false;
 const match=/^Ran ([1-9][0-9]*) tests?(?: in [0-9]+(?:\.[0-9]+)?s)?$/.exec(runs[0]);
 if(!match||!Number.isSafeInteger(Number(match[1])))return false;
 if(lines.indexOf(runs[0])>=lines.length-1)return false;
 if(lines.slice(lines.indexOf(runs[0])+1,-1).some(x=>!/^[-=]+$/.test(x)))return false;
 return !lines.some(x=>/^(?:FAILED\b|Traceback \(most recent call last\):|ERROR:|FAIL:)/.test(x)||/\b(?:skipped|expected failures|unexpected successes)=/.test(x));
}
function execute(profile,event,ops){
 const p=plan(profile,event);
 return ops.lock(()=>{
  ops.preflight(p);
  ops.begin(p);
  const original=ops.snapshot(p.target);
  let phase=null,collected=null;
  for(const cmd of p.commands){
   if(phase!==cmd.phase){phase=cmd.phase;ops.phase(phase,p.phaseSeconds[phase]);}
   const result=ops.run(cmd);
   requireOk(result.status===0&&!result.error,"STEP_FAILED:"+cmd.name);
   if(cmd.name.startsWith("contract"))requireOk(allTestsPass(result.log),"CONTRACT_NOT_ALL_PASS:"+cmd.name);
   if(cmd.name==="collect"){ops.checkDiff(p.target);collected=ops.readTarget(p.target);ops.validate(p,original,collected);}
  }
  const current=ops.readTarget(p.target);
  ops.checkDiff(p.collect?p.target:null);
  if(!p.collect){requireOk(original===current,"PR changed data");return ops.finish({profile,event,state:"CI_ONLY_PASS",candidate:false,publication_authorized:false})}
  requireOk(collected===current,"candidate changed after validation");
  const staged=ops.stage(p,original,current);
  requireOk(staged.stage_status==="STAGED"&&staged.profile===profile&&staged.publication_authorized===false&&staged.production_writer_authorized===false,"stage receipt boundary");
  return ops.finish({profile,event,state:"LOCAL_STAGE_PREPARED",candidate:true,stage:staged,publication_authorized:false});
 });
}
function selftest(){
 let n=0;const ok=x=>{requireOk(x,"test failed");n++};const reject=fn=>{let bad=false;try{fn()}catch(e){bad=true}ok(bad)};
 function fixture(opts={}){
  const trace=[];let locked=false;
  const ops={lock(fn){if(opts.locked)throw Error("LOCK");locked=true;trace.push("lock");try{return fn()}finally{locked=false;trace.push("unlock")}},preflight(){requireOk(locked,"not locked");trace.push("preflight");if(opts.drift)throw Error("drift")},begin(){trace.push("begin")},phase(p,n){trace.push("phase:"+p+":"+n)},snapshot(){return "old"},run(c){requireOk(locked,"not locked");trace.push(c.name);return {status:opts.fail===c.name?1:0,log:opts.empty?"Ran 0 tests\nOK":opts.skip?"Ran 2 tests\nOK (skipped=1)":"Ran 2 tests\nOK\n"}},readTarget(){return opts.tamper&&trace.includes("contract_after")?"tampered":opts.changed?"new":"old"},validate(){trace.push("validate");if(opts.invalid)throw Error("invalid candidate")},checkDiff(p){trace.push("diff");if(opts.extra)throw Error("extra")},stage(p){trace.push("stage");return {stage_status:"STAGED",profile:p.profile,publication_authorized:!!opts.authority,production_writer_authorized:false}},finish(r){trace.push("finish");return r}};
  return {ops,trace};
 }
 let f=fixture();ok(execute("market","pull_request",f.ops).state==="CI_ONLY_PASS");ok(!f.trace.includes("collect")&&!f.trace.includes("stage"));
 f=fixture({changed:true});ok(execute("market","schedule",f.ops).state==="LOCAL_STAGE_PREPARED");ok(f.trace.join(",")==="lock,preflight,begin,phase:contract:300,compile,contract_before,phase:update:900,collect,diff,validate,contract_after,diff,stage,finish,unlock");
 f=fixture();ok(execute("media","workflow_dispatch",f.ops).candidate);ok(f.trace.join(",")==="lock,preflight,begin,phase:update:600,collect,diff,validate,contract_after,diff,stage,finish,unlock");
 reject(()=>plan("media","pull_request"));reject(()=>plan("market","bad"));reject(()=>plan("bad","push"));
 for(const opts of [{locked:true},{drift:true},{fail:"compile"},{fail:"contract_before"},{fail:"collect"},{fail:"contract_after"},{empty:true},{skip:true},{extra:true},{authority:true},{invalid:true},{tamper:true}]){f=fixture(opts);reject(()=>execute("market","schedule",f.ops));ok(!f.trace.includes("finish"))}
 f=fixture({changed:true});reject(()=>execute("market","pull_request",f.ops));ok(!f.trace.includes("stage"));
 ok(plan("market","push").deadlineSeconds===1200&&plan("media","push").deadlineSeconds===600);
 f=fixture({invalid:true});reject(()=>execute("market","push",f.ops));ok(!f.trace.includes("contract_after")&&!f.trace.includes("stage"));
 f=fixture({tamper:true});reject(()=>execute("media","push",f.ops));ok(!f.trace.includes("stage"));
 for(const log of ["Ran 2 tests\nOK\nFAILED (failures=1)\n","Ran 2 tests\nOK\nRan 0 tests\n","Ran 2 tests\nOK\nOK\n","Ran 2 tests\nOK\nTraceback (most recent call last):\n","Ran 999999999999999999999 tests\nOK\n"]){ok(!allTestsPass(log));f=fixture();f.ops.run=()=>({status:0,log});reject(()=>execute("market","pull_request",f.ops));}
 ok(allTestsPass("test_x ... ok\n---\nRan 1 test in 0.001s\n\nOK\n"));
 ok(allTestsPass("Ran 2 tests in 1s\r\n\r\nOK\r\n"));
 return n;
}
module.exports={pins,plan,execute,selftest,allTestsPass};

function main(argv){
 const fs=require("node:fs"),path=require("node:path"),cp=require("node:child_process"),crypto=require("node:crypto");
 if(argv.length===1&&argv[0]==="--selftest"){console.log("PASS "+selftest());return}
 const [flag,profile,event,checkoutArg,pythonArg,evidenceArg,stageArg,runId,depArg,depSha]=argv;
 requireOk(flag==="--approved-isolated-run"&&argv.length===10&&/^[A-Za-z0-9_-]{1,80}$/.test(runId)&&/^[0-9a-f]{64}$/.test(depSha),"usage: --approved-isolated-run profile event disposable-checkout external-python evidence-root shared-stage-root run-id approved-dependencies.json sha256");
 const root=fs.realpathSync(checkoutArg),python=fs.realpathSync(pythonArg),evidence=fs.realpathSync(evidenceArg),stageRoot=fs.realpathSync(stageArg),depPath=fs.realpathSync(depArg),stageScript=path.join(__dirname,"dolzore_shared_writer_stage.py");
 const inside=(a,b)=>{const r=path.relative(a,b);return r===""||(!r.startsWith(".."+path.sep)&&r!==".."&&!path.isAbsolute(r))};
 for(const [a,b] of [[root,evidence],[root,stageRoot],[evidence,stageRoot]])requireOk(!inside(a,b)&&!inside(b,a),"separate directories required");
 requireOk(!inside(root,python)&&!inside(root,depPath)&&!inside(root,stageScript),"external interpreter, dependency lock and carrier required");
 const sha=b=>crypto.createHash("sha256").update(b).digest("hex"),blob=b=>crypto.createHash("sha1").update("blob "+b.length+"\0").update(b).digest("hex");
 const read=p=>{const s=fs.lstatSync(p);requireOk(s.isFile()&&!s.isSymbolicLink()&&s.size<=16*1024*1024,"bounded regular input required");return fs.readFileSync(p)};
 const write=(p,b)=>{const fd=fs.openSync(p,"wx");try{fs.writeFileSync(fd,b);fs.fsyncSync(fd)}finally{fs.closeSync(fd)}requireOk(read(p).equals(Buffer.from(b)),"readback mismatch")};
 const env=Object.fromEntries(Object.entries(process.env).filter(([k])=>!/^GIT_|^PYTHON|^PIP_/i.test(k)));
 Object.assign(env,{GIT_NO_REPLACE_OBJECTS:"1",GIT_NO_LAZY_FETCH:"1",GIT_TERMINAL_PROMPT:"0",GIT_OPTIONAL_LOCKS:"0",PYTHONUTF8:"1",PYTHONDONTWRITEBYTECODE:"1",PYTHONNOUSERSITE:"1"});
 const git=(...args)=>cp.execFileSync("git",["-c","protocol.allow=never","-C",root,...args],{env,timeout:30000,maxBuffer:16*1024*1024});
 const status=()=>git("status","--porcelain=v1","-z","--untracked-files=all").toString();
 let runDir,started,deadline,uncertain=false;
 const runRaw=(args,seconds)=>{requireOk(Date.now()<deadline,"phase timeout");const r=cp.spawnSync(python,args,{cwd:root,env,timeout:Math.max(1,Math.min(seconds*1000,deadline-Date.now())),killSignal:"SIGKILL",maxBuffer:4*1024*1024,windowsHide:true,encoding:"utf8"});if(r.error||r.signal||r.status===null)uncertain=true;return r};
 const ops={
  lock(fn){const lock=path.join(stageRoot,".dolzore-data-writers-collection.lock");fs.mkdirSync(lock);try{return fn()}finally{if(!uncertain)fs.rmdirSync(lock)}},
  preflight(p){
   requireOk(fs.realpathSync(git("rev-parse","--show-toplevel").toString().trim())===root&&git("rev-parse","HEAD").toString().trim()===pins.base&&!status(),"pinned clean original checkout required");
   requireOk(fs.lstatSync(path.join(root,".git")).isDirectory()&&!fs.existsSync(path.join(root,".git","objects","info","alternates")),"standalone checkout required");
   requireOk(!fs.existsSync(path.join(evidence,runId))&&!fs.existsSync(path.join(stageRoot,runId))&&!fs.existsSync(path.join(stageRoot,".dolzore-data-writers.lock")),"existing run or stage lock");
   const x=pins[p.profile];for(const [file,hash] of [[x.workflow,x.workflowBlob],[x.script,x.scriptBlob],[x.data,x.dataBlob],...(x.test?[[x.test,x.testBlob]]:[])]){const full=path.join(root,file);requireOk(fs.realpathSync(full)===full&&blob(read(full))===hash&&blob(git("show",pins.base+":"+file))===hash,"pin mismatch "+file)}
   requireOk(blob(read(stageScript))===pins.stage,"stage carrier drift");
   const b=read(depPath);requireOk(sha(b)===depSha,"dependency inventory hash drift");const expected=JSON.parse(b);requireOk(expected&&typeof expected.requests==="string"&&typeof expected.beautifulsoup4==="string","approved dependency versions required");
   deadline=Date.now()+30000;
   const probe=runRaw(["-I","-c","import json,sys,importlib.metadata as m; print(json.dumps({'python':list(sys.version_info[:2]),'requests':m.version('requests'),'beautifulsoup4':m.version('beautifulsoup4')}))"],30);
   requireOk(probe.status===0&&!probe.error,"dependency probe failed");const actual=JSON.parse(probe.stdout);requireOk(JSON.stringify(actual.python)==="[3,11]"&&actual.requests===expected.requests&&actual.beautifulsoup4===expected.beautifulsoup4,"dependency version mismatch");
  },
  begin(p){const dir=path.join(evidence,runId);fs.mkdirSync(dir);runDir=dir;started=new Date().toISOString();write(path.join(runDir,"intent.json"),JSON.stringify({profile,event,runId,base:pins.base,depSha,started})+"\n")},
  phase(name,seconds){deadline=Date.now()+seconds*1000;write(path.join(runDir,"phase-"+name+".json"),JSON.stringify({name,seconds,started:new Date().toISOString()})+"\n")},
  snapshot(target){const b=read(path.join(root,target));write(path.join(runDir,"original.json"),b);return sha(b)},
  run(c){requireOk(Date.now()<deadline,"whole run timeout");const result=runRaw(c.args,c.seconds),log=(result.stdout||"")+"\n"+(result.stderr||"");write(path.join(runDir,c.name+".log"),log);return {status:result.status,error:result.error,log}},
  readTarget(target){return sha(read(path.join(root,target)))},
  validate(p,original,current){const output=path.join(root,p.target),old=path.join(runDir,"original.json");requireOk(sha(read(old))===original&&sha(read(output))===current,"validation input drift");const command="import runpy,sys,pathlib,datetime; m=runpy.run_path(sys.argv[1]); m['validate'](sys.argv[2],pathlib.Path(sys.argv[3]).read_bytes(),pathlib.Path(sys.argv[4]).read_bytes(),datetime.datetime.now(datetime.timezone.utc))";const r=runRaw(["-I","-c",command,stageScript,profile,old,output],60);write(path.join(runDir,"validate.log"),(r.stdout||"")+"\n"+(r.stderr||""));requireOk(r.status===0&&!r.error,"candidate validation failed")},
  checkDiff(target){
   requireOk(git("rev-parse","HEAD").toString().trim()===pins.base,"HEAD drift");
   const tracked=git("diff","--name-only","-z","HEAD").toString().split("\0").filter(Boolean),staged=git("diff","--cached","--name-only","-z").toString(),untracked=git("ls-files","--others","--exclude-standard","-z").toString();
   requireOk(!staged&&!untracked&&tracked.every(p=>p===target),"unexpected checkout changes");
  },
  stage(p,original,current){
   requireOk(Date.now()<deadline,"whole run timeout");
   const output=path.join(root,p.target);
   const result=runRaw([stageScript,"--profile",profile,"--root",stageRoot,"--original",path.join(runDir,"original.json"),"--candidate",output,"--run-id",runId,"--expected-original",original,"--expected-candidate",current],60);
   write(path.join(runDir,"stage.log"),(result.stdout||"")+"\n"+(result.stderr||""));requireOk(result.status===0&&!result.error,"stage failed");
   const receipt=JSON.parse(result.stdout),saved=JSON.parse(read(path.join(stageRoot,runId,"receipt.json")));requireOk(JSON.stringify(receipt)===JSON.stringify(saved)&&sha(read(path.join(stageRoot,runId,"candidate.json")))===current,"stage readback mismatch");return receipt;
  },
  finish(result){const receipt={...result,schema:"dolzore-local-writer-run/v1",runId,source_commit:pins.base,started,finished:new Date().toISOString(),dependency_inventory_sha256:depSha,production_writer_authorized:false,runtime_acceptance:"NOT_ESTABLISHED",remote_write:false};write(path.join(runDir,"result.json"),JSON.stringify(receipt)+"\n");return receipt}
 };
 try{console.log(JSON.stringify(execute(profile,event,ops)))}catch(err){if(runDir&&fs.existsSync(runDir)){try{write(path.join(runDir,"failure.json"),JSON.stringify({state:"FAILED",reason:String(err.message).slice(0,300),lock_retained_for_hq:uncertain,publication_authorized:false})+"\n")}catch{}}throw err}
}
if(require.main===module){try{main(process.argv.slice(2))}catch(e){console.error("DOLZORE_LOCAL_WRITER_REJECTED: "+e.message);process.exitCode=2}}
