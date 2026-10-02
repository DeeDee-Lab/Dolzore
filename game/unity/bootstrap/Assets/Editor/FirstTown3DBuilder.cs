using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Animations;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering;

namespace Dolzore.Editor
{
    public static class FirstTown3DBuilder
    {
        public const string ScenePath = "Assets/Scenes/FirstTown3D.unity";
        private const string TownRoot = "Assets/External/FantasyTown";
        private const string CastleRoot = "Assets/External/Castle";
        private const string CharacterRoot = "Assets/External/Characters";
        private const string KayRoot = "Assets/External/KayTown";
        private const string AdventurerRoot = "Assets/External/Adventurers";
        private const string GeneratedRoot = "Assets/Art/Generated3D/FirstTownFF11";

        private static readonly Dictionary<string, Material> Materials = new Dictionary<string, Material>();

        private static Color Hex(string html)
        {
            ColorUtility.TryParseHtmlString(html, out var c);
            return c;
        }

        private static Material Flat(string key, string html, float smooth=0.15f)
        {
            if (Materials.TryGetValue(key, out var cached)) return cached;
            var shader=Shader.Find("Standard") ?? Shader.Find("Universal Render Pipeline/Lit") ?? Shader.Find("Unlit/Color");
            var mat=new Material(shader){name=key,color=Hex(html)};
            if(mat.HasProperty("_Metallic")) mat.SetFloat("_Metallic",0f);
            if(mat.HasProperty("_Smoothness")) mat.SetFloat("_Smoothness",smooth);
            if(mat.HasProperty("_Glossiness")) mat.SetFloat("_Glossiness",smooth);
            Materials[key]=mat;
            return mat;
        }

        private static Material Textured(string key, string file, string baseHtml, Vector2 tiling, float smooth=0.12f)
        {
            if (Materials.TryGetValue(key, out var cached)) return cached;
            EnsureGeneratedTextures();
            var shader=Shader.Find("Standard") ?? Shader.Find("Universal Render Pipeline/Lit") ?? Shader.Find("Unlit/Texture");
            var mat=new Material(shader){name=key,color=Hex(baseHtml)};
            var tex=AssetDatabase.LoadAssetAtPath<Texture2D>(GeneratedRoot+"/"+file);
            mat.mainTexture=tex;
            mat.mainTextureScale=tiling;
            if(mat.HasProperty("_BaseMap")) mat.SetTexture("_BaseMap",tex);
            if(mat.HasProperty("_BaseColor")) mat.SetColor("_BaseColor",Hex(baseHtml));
            if(mat.HasProperty("_Smoothness")) mat.SetFloat("_Smoothness",smooth);
            if(mat.HasProperty("_Glossiness")) mat.SetFloat("_Glossiness",smooth);
            Materials[key]=mat;
            return mat;
        }

        private static Material Atlas(string key,string texturePath,float smooth=0.10f)
        {
            if(Materials.TryGetValue(key,out var cached)) return cached;
            var tex=AssetDatabase.LoadAssetAtPath<Texture2D>(texturePath);
            if(tex==null) throw new Exception("DOLZORE_MISSING_ATLAS:"+texturePath);
            var shader=Shader.Find("Standard") ?? Shader.Find("Universal Render Pipeline/Lit") ?? Shader.Find("Unlit/Texture");
            var mat=new Material(shader){name=key,color=Color.white,mainTexture=tex};
            if(mat.HasProperty("_BaseMap")) mat.SetTexture("_BaseMap",tex);
            if(mat.HasProperty("_BaseColor")) mat.SetColor("_BaseColor",Color.white);
            if(mat.HasProperty("_Smoothness")) mat.SetFloat("_Smoothness",smooth);
            if(mat.HasProperty("_Glossiness")) mat.SetFloat("_Glossiness",smooth);
            Materials[key]=mat;
            return mat;
        }

        private static void ApplyExternalAtlas(GameObject go,string rootPath)
        {
            if(rootPath==TownRoot) SetLayerMaterial(go,Atlas("FantasyTownAtlas",TownRoot+"/Textures/variation-a.png",0.08f));
            else if(rootPath.StartsWith(KayRoot,StringComparison.Ordinal)) SetLayerMaterial(go,Atlas("KayMedievalAtlas",KayRoot+"/hexagons_medieval.png",0.10f));
            else if(rootPath==CastleRoot) SetLayerMaterial(go,Atlas("CastleAtlas",CastleRoot+"/Textures/variation-a.png",0.08f));
        }

        private static GameObject Cube(string name, Vector3 pos, Vector3 scale, Material mat, Transform parent=null)
        {
            var go=GameObject.CreatePrimitive(PrimitiveType.Cube);
            go.name=name; go.transform.position=pos; go.transform.localScale=scale;
            if(parent!=null) go.transform.SetParent(parent,true);
            var r=go.GetComponent<Renderer>(); r.sharedMaterial=mat; r.shadowCastingMode=ShadowCastingMode.On; r.receiveShadows=true;
            return go;
        }

        private static Bounds BoundsOf(GameObject root)
        {
            var rs=root.GetComponentsInChildren<Renderer>(true);
            if(rs.Length==0) return new Bounds(root.transform.position,Vector3.one);
            var b=rs[0].bounds;
            for(int i=1;i<rs.Length;i++) b.Encapsulate(rs[i].bounds);
            return b;
        }

        private static GameObject Spawn(string rootPath,string file,Vector3 bottomPos,float yaw,float targetHeight,Transform parent,bool collider=true)
        {
            var path=rootPath+"/"+file+".fbx";
            var prefab=AssetDatabase.LoadAssetAtPath<GameObject>(path);
            if(prefab==null) throw new Exception("DOLZORE_MISSING_EXTERNAL_ASSET:"+path);
            var go=(GameObject)PrefabUtility.InstantiatePrefab(prefab);
            go.name=file;
            if(parent!=null) go.transform.SetParent(parent,true);
            go.transform.position=Vector3.zero;
            go.transform.rotation=Quaternion.identity;
            var b=BoundsOf(go);
            float h=Mathf.Max(0.001f,b.size.y);
            float s=targetHeight>0f?targetHeight/h:1f;
            go.transform.localScale=Vector3.one*s;
            go.transform.rotation=Quaternion.Euler(0f,yaw,0f);
            ApplyExternalAtlas(go,rootPath);
            b=BoundsOf(go);
            go.transform.position=bottomPos+Vector3.up*(-b.min.y);
            if(collider)
            {
                b=BoundsOf(go);
                var bc=go.AddComponent<BoxCollider>();
                bc.center=go.transform.InverseTransformPoint(b.center);
                var localMin=go.transform.InverseTransformPoint(b.min);
                var localMax=go.transform.InverseTransformPoint(b.max);
                bc.size=new Vector3(Mathf.Abs(localMax.x-localMin.x),Mathf.Abs(localMax.y-localMin.y),Mathf.Abs(localMax.z-localMin.z));
            }
            return go;
        }

        private static GameObject SpawnWidth(string rootPath,string file,Vector3 bottomPos,float yaw,float targetWidth,Transform parent,bool collider=true)
        {
            var path=rootPath+"/"+file+".fbx";
            var prefab=AssetDatabase.LoadAssetAtPath<GameObject>(path);
            if(prefab==null) throw new Exception("DOLZORE_MISSING_EXTERNAL_ASSET:"+path);
            var go=(GameObject)PrefabUtility.InstantiatePrefab(prefab);
            go.name=file;
            if(parent!=null) go.transform.SetParent(parent,true);
            go.transform.position=Vector3.zero; go.transform.rotation=Quaternion.identity;
            var b=BoundsOf(go);
            // Kenney wall modules are not guaranteed to use local X as their long axis.
            // Scale against the dominant horizontal extent so a thin depth axis can never
            // explode the whole prefab into a camera-blocking slab.
            float w=Mathf.Max(0.001f,Mathf.Max(b.size.x,b.size.z));
            go.transform.localScale=Vector3.one*(targetWidth/w);
            go.transform.rotation=Quaternion.Euler(0f,yaw,0f);
            ApplyExternalAtlas(go,rootPath);
            b=BoundsOf(go);
            go.transform.position=bottomPos+Vector3.up*(-b.min.y);
            if(collider)
            {
                b=BoundsOf(go);
                var bc=go.AddComponent<BoxCollider>();
                bc.center=go.transform.InverseTransformPoint(b.center);
                var localMin=go.transform.InverseTransformPoint(b.min);
                var localMax=go.transform.InverseTransformPoint(b.max);
                bc.size=new Vector3(Mathf.Abs(localMax.x-localMin.x),Mathf.Abs(localMax.y-localMin.y),Mathf.Abs(localMax.z-localMin.z));
            }
            return go;
        }

        private static void SetLayerMaterial(GameObject root, Material mat)
        {
            foreach(var r in root.GetComponentsInChildren<Renderer>(true)) r.sharedMaterial=mat;
        }

        private static void BuildHouse(string name,Vector3 center,float yaw,int cells,int floors,bool timber,bool balcony,bool banners,Transform parent,Material inner)
        {
            var holder=new GameObject(name); holder.transform.SetParent(parent,true); holder.transform.position=center; holder.transform.rotation=Quaternion.Euler(0f,yaw,0f);
            float cell=3.0f, floorH=3.0f, depth=8.2f;
            float width=cells*cell;
            Cube(name+" Inner",center+Vector3.up*(floors*floorH*0.5f),new Vector3(width-0.35f,floors*floorH-0.25f,depth-0.35f),inner,parent);

            Vector3 right=holder.transform.right;
            Vector3 forward=holder.transform.forward;
            string prefix=timber?"wall-wood-":"wall-";
            for(int f=0;f<floors;f++)
            {
                float y=center.y+f*floorH;
                for(int i=0;i<cells;i++)
                {
                    float x=(i-(cells-1)*0.5f)*cell;
                    bool door=f==0 && i==cells/2;
                    string front=door?prefix+"door":(i%2==0?prefix+"window-shutters":prefix+"window-stone");
                    SpawnWidth(TownRoot,front,center+right*x-forward*(depth*0.5f)+Vector3.up*(f*floorH),yaw,cell,parent);
                    SpawnWidth(TownRoot,prefix+"window-small",center+right*x+forward*(depth*0.5f)+Vector3.up*(f*floorH),yaw+180f,cell,parent);
                }
                int sideCells=2;
                for(int j=0;j<sideCells;j++)
                {
                    float z=(j-(sideCells-1)*0.5f)*(depth/sideCells);
                    SpawnWidth(TownRoot,(j==0?prefix+"window-small":(timber?"wall-wood":"wall")),center-right*(width*0.5f)+forward*z+Vector3.up*(f*floorH),yaw-90f,depth/sideCells,parent);
                    SpawnWidth(TownRoot,(j==1?prefix+"window-small":(timber?"wall-wood":"wall")),center+right*(width*0.5f)+forward*z+Vector3.up*(f*floorH),yaw+90f,depth/sideCells,parent);
                }
            }

            float roofY=center.y+floors*floorH;
            for(int i=0;i<cells;i++)
            {
                float x=(i-(cells-1)*0.5f)*cell;
                SpawnWidth(TownRoot,timber?"roof-high":"roof-gable",center+right*x+Vector3.up*roofY,yaw,cell*1.15f,parent,false);
            }

            if(balcony && floors>=2)
            {
                SpawnWidth(TownRoot,"balcony-wall-fence",center-forward*(depth*0.5f+0.55f)+Vector3.up*(floorH+0.25f),yaw,width*0.85f,parent,false);
                SpawnWidth(TownRoot,"overhang",center-forward*(depth*0.5f+0.28f)+Vector3.up*(floorH-0.3f),yaw,width*0.55f,parent,false);
            }
            if(banners)
            {
                Spawn(TownRoot,"banner-red",center-right*(width*0.38f)-forward*(depth*0.5f+0.18f)+Vector3.up*(floorH*0.85f),yaw,2.5f,parent,false);
                Spawn(TownRoot,"banner-green",center+right*(width*0.38f)-forward*(depth*0.5f+0.18f)+Vector3.up*(floorH*0.85f),yaw,2.5f,parent,false);
            }
            if((cells+floors)%2==0) Spawn(TownRoot,"chimney",center+right*(width*0.28f)+Vector3.up*(roofY+0.4f),yaw,2.2f,parent,false);
        }

        private static void BuildGate(Vector3 center,Transform parent)
        {
            // Use the detailed town kit for the gate silhouette; avoid giant blank castle slabs.
            var arch=Spawn(TownRoot,"wall-arch",center,0f,7.4f,parent);
            var top=Spawn(TownRoot,"wall-arch-top-detail",center+Vector3.up*6.55f,0f,2.0f,parent,false);
            var left=Spawn(TownRoot,"pillar-stone",center+new Vector3(-5.0f,0f,0),0f,8.1f,parent);
            var right=Spawn(TownRoot,"pillar-stone",center+new Vector3(5.0f,0f,0),0f,8.1f,parent);
            Spawn(TownRoot,"banner-red",center+new Vector3(-5.0f,4.8f,-0.25f),0f,3.0f,parent,false);
            Spawn(TownRoot,"banner-green",center+new Vector3(5.0f,4.8f,-0.25f),0f,3.0f,parent,false);
        }

        private static void BuildFountain(Vector3 p,Transform parent)
        {
            Spawn(TownRoot,"fountain-round",p,0f,1.0f,parent,false);
            Spawn(TownRoot,"fountain-round-detail",p+Vector3.up*0.1f,0f,1.0f,parent,false);
            Spawn(TownRoot,"fountain-center",p+Vector3.up*0.15f,0f,3.8f,parent,false);
        }

        private static void BuildMarket(Vector3 origin,float yaw,Transform parent)
        {
            var rot=Quaternion.Euler(0f,yaw,0f);
            for(int i=0;i<4;i++)
            {
                Vector3 p=origin+rot*(new Vector3((i-1.5f)*3.65f,0f,0f));
                Spawn(TownRoot,i%2==0?"stall-red":"stall-green",p,yaw,2.6f,parent,false);
                Spawn(TownRoot,"stall-bench",p+rot*new Vector3(0f,0f,0.8f),yaw,1.2f,parent,false);
                if(i==1||i==2) Spawn(TownRoot,"stall-stool",p+rot*new Vector3(1.1f,0f,1.15f),yaw,0.9f,parent,false);
            }
            Spawn(TownRoot,"cart",origin+rot*new Vector3(-8.8f,0f,2.6f),yaw+12f,1.8f,parent,false);
            Spawn(TownRoot,"cart-high",origin+rot*new Vector3(8.8f,0f,2.2f),yaw-9f,2.0f,parent,false);
        }

        private static Material AdventurerMaterial(string textureFile,string key)
        {
            if(Materials.TryGetValue(key,out var cached)) return cached;
            var tex=AssetDatabase.LoadAssetAtPath<Texture2D>(AdventurerRoot+"/Characters/"+textureFile);
            var shader=Shader.Find("Standard") ?? Shader.Find("Universal Render Pipeline/Lit") ?? Shader.Find("Unlit/Texture");
            var mat=new Material(shader){name=key,color=Color.white,mainTexture=tex};
            if(mat.HasProperty("_BaseMap")) mat.SetTexture("_BaseMap",tex);
            if(mat.HasProperty("_Smoothness")) mat.SetFloat("_Smoothness",0.12f);
            if(mat.HasProperty("_Glossiness")) mat.SetFloat("_Glossiness",0.12f);
            Materials[key]=mat; return mat;
        }

        private static AnimationClip FindClip(string path, params string[] words)
        {
            AnimationClip fallback=null;
            foreach(var asset in AssetDatabase.LoadAllAssetsAtPath(path))
            {
                var clip=asset as AnimationClip;
                if(clip==null || clip.name.StartsWith("__preview__",StringComparison.OrdinalIgnoreCase)) continue;
                if(fallback==null) fallback=clip;
                foreach(var word in words)
                    if(clip.name.IndexOf(word,StringComparison.OrdinalIgnoreCase)>=0) return clip;
            }
            return fallback;
        }

        private static RuntimeAnimatorController EnsureAdventurerController(out AnimationClip idleClip)
        {
            string general=AdventurerRoot+"/Animations/Rig_Medium_General.fbx";
            string movement=AdventurerRoot+"/Animations/Rig_Medium_MovementBasic.fbx";
            idleClip=FindClip(general,"idle");
            var runClip=FindClip(movement,"run","sprint");
            var walkClip=FindClip(movement,"walk");

            string path=GeneratedRoot+"/FirstTownAdventurer.controller";
            if(AssetDatabase.LoadAssetAtPath<AnimatorController>(path)!=null) AssetDatabase.DeleteAsset(path);
            var controller=AnimatorController.CreateAnimatorControllerAtPath(path);
            controller.AddParameter("Speed",AnimatorControllerParameterType.Float);
            var sm=controller.layers[0].stateMachine;
            foreach(var child in sm.states) sm.RemoveState(child.state);

            var idle=sm.AddState("Idle"); idle.motion=idleClip; sm.defaultState=idle;
            var move=sm.AddState("Move"); move.motion=runClip??walkClip??idleClip;
            var toMove=idle.AddTransition(move); toMove.hasExitTime=false; toMove.duration=0.12f; toMove.AddCondition(AnimatorConditionMode.Greater,0.10f,"Speed");
            var toIdle=move.AddTransition(idle); toIdle.hasExitTime=false; toIdle.duration=0.16f; toIdle.AddCondition(AnimatorConditionMode.Less,0.10f,"Speed");
            EditorUtility.SetDirty(controller);
            AssetDatabase.SaveAssets();
            return controller;
        }

        private static GameObject FantasyCharacter(string name,Vector3 p,float yaw,string archetype,string textureFile,bool player,Transform parent,RuntimeAnimatorController controller,AnimationClip idleClip)
        {
            var visual=Spawn(AdventurerRoot+"/Characters",archetype,p,yaw,1.76f,parent,false);
            visual.name=name+" Visual";
            SetLayerMaterial(visual,AdventurerMaterial(textureFile,name+" Mat"));
            var animator=visual.GetComponent<Animator>();
            if(animator==null) animator=visual.AddComponent<Animator>();
            animator.runtimeAnimatorController=controller;
            if(idleClip!=null)
            {
                float sample=Mathf.Min(idleClip.length*0.23f,0.33f);
                idleClip.SampleAnimation(visual,sample);
            }
            if(!player) return visual;

            var root=new GameObject(name);
            root.transform.position=p; root.transform.rotation=Quaternion.Euler(0f,yaw,0f); root.transform.SetParent(parent,true);
            visual.transform.SetParent(root.transform,true);
            var cc=root.AddComponent<CharacterController>(); cc.height=1.78f; cc.radius=0.33f; cc.center=new Vector3(0,0.89f,0);
            var walker=root.AddComponent<Dolzore.ThirdPersonWalker3D>();
            walker.animator=animator;
            return root;
        }

        private static void AddLight(string name,Color color,float intensity,Vector3 euler,bool shadows)
        {
            var go=new GameObject(name);
            var l=go.AddComponent<Light>(); l.type=LightType.Directional;l.color=color;l.intensity=intensity;
            l.shadows=shadows?LightShadows.Soft:LightShadows.None;l.shadowStrength=shadows?0.68f:0f;
            go.transform.eulerAngles=euler;
        }

        private static void EnsureGeneratedTextures()
        {
            if(!AssetDatabase.IsValidFolder("Assets/Art")) AssetDatabase.CreateFolder("Assets","Art");
            if(!AssetDatabase.IsValidFolder("Assets/Art/Generated3D")) AssetDatabase.CreateFolder("Assets/Art","Generated3D");
            if(!AssetDatabase.IsValidFolder(GeneratedRoot)) AssetDatabase.CreateFolder("Assets/Art/Generated3D","FirstTownFF11");
            MakeCobble("cobble.png",Hex("#81786A"),Hex("#B2A58F"),Hex("#58534D"));
            MakeNoise("plaster.png",Hex("#BBA98A"),Hex("#D5C5A5"),71);
            MakeNoise("grass.png",Hex("#526A3D"),Hex("#738A52"),91);
            AssetDatabase.Refresh();
        }

        private static void MakeCobble(string file,Color baseC,Color hi,Color seam)
        {
            string path=GeneratedRoot+"/"+file; if(File.Exists(Path.GetFullPath(path))) return;
            const int w=128,h=128; var tex=new Texture2D(w,h,TextureFormat.RGBA32,false);
            var px=new Color32[w*h];
            for(int y=0;y<h;y++) for(int x=0;x<w;x++)
            {
                int row=y/16; int sx=x+((row&1)==0?0:13);
                bool s=(y%16<=1)||(sx%26<=1);
                float n=Mathf.PerlinNoise(x*0.07f,y*0.07f)*0.22f;
                px[y*w+x]=s?seam:Color.Lerp(baseC,hi,n);
            }
            tex.SetPixels32(px);tex.Apply();File.WriteAllBytes(Path.GetFullPath(path),tex.EncodeToPNG());UnityEngine.Object.DestroyImmediate(tex);
        }

        private static void MakeNoise(string file,Color a,Color b,int seed)
        {
            string path=GeneratedRoot+"/"+file; if(File.Exists(Path.GetFullPath(path))) return;
            const int w=128,h=128; var tex=new Texture2D(w,h,TextureFormat.RGBA32,false);
            var rng=new System.Random(seed);var px=new Color32[w*h];
            for(int y=0;y<h;y++) for(int x=0;x<w;x++)
            {
                float n=(float)rng.NextDouble()*0.22f+Mathf.PerlinNoise((x+seed)*0.05f,(y-seed)*0.05f)*0.30f;
                px[y*w+x]=Color.Lerp(a,b,n);
            }
            tex.SetPixels32(px);tex.Apply();File.WriteAllBytes(Path.GetFullPath(path),tex.EncodeToPNG());UnityEngine.Object.DestroyImmediate(tex);
        }

        public static void Build()
        {
            Materials.Clear();
            EnsureGeneratedTextures();
            AssetDatabase.Refresh();

            string[] mustExist={
                TownRoot+"/wall-wood-window-shutters.fbx",
                TownRoot+"/roof-high.fbx",
                TownRoot+"/fountain-round.fbx",
                CastleRoot+"/tower-square.fbx",
                KayRoot+"/building_tavern_blue.fbx",
                KayRoot+"/building_townhall_blue.fbx",
                KayRoot+"/Props/barrel.fbx",
                KayRoot+"/Nature/mountain_A_grass_trees.fbx",
                TownRoot+"/Textures/variation-a.png",
                KayRoot+"/hexagons_medieval.png",
                AdventurerRoot+"/Characters/Knight.fbx",
                AdventurerRoot+"/Characters/Mage.fbx",
                AdventurerRoot+"/Animations/Rig_Medium_General.fbx",
                AdventurerRoot+"/Animations/Rig_Medium_MovementBasic.fbx"
            };
            foreach(var p in mustExist) if(AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(p)==null) throw new Exception("DOLZORE_EXTERNAL_ASSET_IMPORT_FAILED:"+p);

            var scene=EditorSceneManager.NewScene(NewSceneSetup.EmptyScene,NewSceneMode.Single);
            RenderSettings.ambientMode=AmbientMode.Trilight;
            RenderSettings.ambientSkyColor=Hex("#C4D3DE");
            RenderSettings.ambientEquatorColor=Hex("#A28C70");
            RenderSettings.ambientGroundColor=Hex("#465040");
            RenderSettings.ambientIntensity=0.54f;
            RenderSettings.fog=true;RenderSettings.fogMode=FogMode.ExponentialSquared;RenderSettings.fogDensity=0.0027f;RenderSettings.fogColor=Hex("#B9C6CC");
            var skyShader=Shader.Find("Skybox/Procedural");
            if(skyShader!=null)
            {
                var sky=new Material(skyShader);
                sky.SetColor("_SkyTint",Hex("#A9C5D9"));
                sky.SetColor("_GroundColor",Hex("#6E766E"));
                sky.SetFloat("_AtmosphereThickness",0.78f);
                sky.SetFloat("_Exposure",1.05f);
                RenderSettings.skybox=sky;
            }

            QualitySettings.shadowDistance=120f; QualitySettings.antiAliasing=4;

            var root=new GameObject("FIRST TOWN 3D - ASTER GATE").transform;
            var grass=Textured("Grass","grass.png","#617B4A",new Vector2(12,12));
            var cobble=Textured("Cobble","cobble.png","#9B907E",new Vector2(9,9),0.10f);
            var plaster=Textured("Plaster","plaster.png","#C8B697",new Vector2(3,3),0.09f);
            var stone=Flat("Stone","#8D8373",0.12f);
            var walkway=Flat("Walkway","#958671",0.10f);
            var dark=Flat("DarkWood","#4A3A31",0.10f);

            Cube("Ground",new Vector3(0,-0.55f,7),new Vector3(110,1,105),grass,root);

            // FF11-like density: narrow carriageway, readable pedestrian edges and almost no dead lawn.
            Cube("SouthWalk",new Vector3(-2,-0.01f,-22),new Vector3(8.5f,0.16f,34),walkway,root);
            Cube("SouthStreet",new Vector3(-2,0.04f,-22),new Vector3(6.2f,0.20f,34),cobble,root);
            var bendWalk=Cube("CentralWalk",new Vector3(2,-0.01f,-2),new Vector3(8.5f,0.16f,25),walkway,root); bendWalk.transform.eulerAngles=new Vector3(0,-10,0);
            var bend=Cube("CentralStreet",new Vector3(2,0.04f,-2),new Vector3(6.2f,0.20f,25),cobble,root); bend.transform.eulerAngles=new Vector3(0,-10,0);
            var northWalk=Cube("NorthWalk",new Vector3(7,-0.01f,22),new Vector3(8.5f,0.16f,29),walkway,root); northWalk.transform.eulerAngles=new Vector3(0,-14,0);
            var north=Cube("NorthStreet",new Vector3(7,0.04f,22),new Vector3(6.2f,0.20f,29),cobble,root); north.transform.eulerAngles=new Vector3(0,-14,0);
            Cube("MarketLane",new Vector3(-15,0.02f,-1),new Vector3(27,0.18f,5.0f),cobble,root).transform.eulerAngles=new Vector3(0,7,0);
            Cube("CivicLane",new Vector3(20,0.02f,8),new Vector3(28,0.18f,4.8f),cobble,root).transform.eulerAngles=new Vector3(0,-12,0);
            Cube("Plaza",new Vector3(4,0.06f,6),new Vector3(15.5f,0.20f,14.5f),cobble,root);

            // Raised northern ward: lower retaining wall, stronger visible skyline.
            Cube("UpperWard",new Vector3(11,1.20f,38),new Vector3(52,2.4f,22),stone,root);
            for(int i=0;i<8;i++) Cube("UpperStep "+i,new Vector3(4.5f,0.17f+i*0.28f,28+i*0.82f),new Vector3(7.2f,0.34f,1.05f),stone,root);

            // Street enclosure now uses complete low-poly authored buildings.
            // This keeps the FF11-like light geometry budget while avoiding fragile wall-piece assembly.
            Spawn(KayRoot,"building_tavern_blue",new Vector3(-11,0,-23),18f,8.5f,root);
            Spawn(KayRoot,"building_home_A_blue",new Vector3(10,0,-23),-12f,7.4f,root);
            Spawn(KayRoot,"building_home_B_blue",new Vector3(-10.0f,0,-13.0f),12f,6.6f,root);
            Spawn(KayRoot,"building_home_A_blue",new Vector3(10.5f,0,-12.0f),-10f,6.4f,root);
            Spawn(KayRoot,"building_market_blue",new Vector3(-17,0,-8),83f,7.5f,root);
            Spawn(KayRoot,"building_townhall_blue",new Vector3(-18,0,8),96f,11.5f,root);
            Spawn(KayRoot,"building_blacksmith_blue",new Vector3(17,0,-8),-78f,7.8f,root);
            Spawn(KayRoot,"building_workshop_blue",new Vector3(20,0,17),-78f,7.8f,root);
            Spawn(KayRoot,"building_home_A_blue",new Vector3(-10.5f,0,8.0f),15f,6.2f,root);
            Spawn(KayRoot,"building_home_B_blue",new Vector3(12.5f,0,9.0f),-20f,6.5f,root);
            Spawn(KayRoot,"building_home_B_blue",new Vector3(-11,0,18),30f,7.6f,root);
            Spawn(KayRoot,"building_church_blue",new Vector3(-6,0,32),10f,12.5f,root);
            Spawn(KayRoot,"building_castle_blue",new Vector3(22,2.45f,39),-18f,15.0f,root);
            Spawn(KayRoot,"building_barracks_blue",new Vector3(-14,2.45f,39),12f,9.5f,root);
            Spawn(KayRoot,"building_watchtower_blue",new Vector3(31,2.45f,45),-15f,12.0f,root);

            // Landmark square and market life.
            BuildFountain(new Vector3(4,0.18f,7),root);
            Spawn(KayRoot,"building_well_blue",new Vector3(7.5f,0.18f,11.5f),18f,3.2f,root,false);
            BuildMarket(new Vector3(-17,0.05f,2),92f,root);
            BuildMarket(new Vector3(15,0.05f,6),-83f,root);

            // Everyday prop density: cheap repeated meshes, the same principle that keeps FF11 towns alive.
            string propRoot=KayRoot+"/Props";
            Vector3[] barrels={new Vector3(-8,0,-17),new Vector3(-7.2f,0,-16.4f),new Vector3(11.5f,0,-15),new Vector3(17,0,-1),new Vector3(-18,0,11)};
            foreach(var p in barrels) Spawn(propRoot,"barrel",p,0f,0.9f,root,false);
            Spawn(propRoot,"crate_A_big",new Vector3(-9,0,-15.5f),12f,1.0f,root,false);
            Spawn(propRoot,"crate_A_small",new Vector3(-8.1f,0,-14.8f),-7f,0.65f,root,false);
            Spawn(propRoot,"crate_B_small",new Vector3(15.8f,0,10.2f),19f,0.65f,root,false);
            Spawn(propRoot,"sack",new Vector3(-16.5f,0,5.2f),35f,0.65f,root,false);
            Spawn(propRoot,"wheelbarrow",new Vector3(14.8f,0,-4.5f),-30f,1.2f,root,false);
            Spawn(propRoot,"weaponrack",new Vector3(15.5f,0,-7.5f),-80f,1.8f,root,false);
            Spawn(propRoot,"haybale",new Vector3(-12.5f,0,23.5f),25f,0.9f,root,false);
            Spawn(propRoot,"flag_blue",new Vector3(-7.0f,0,10.5f),0f,3.2f,root,false);
            Spawn(propRoot,"flag_red",new Vector3(14.0f,0,13.0f),0f,3.2f,root,false);

            // Distant terrain gives the town a place in a wider world without expensive geometry.
            string natureRoot=KayRoot+"/Nature";
            Spawn(natureRoot,"mountain_A_grass_trees",new Vector3(-34,0,78),10f,24f,root,false);
            Spawn(natureRoot,"mountain_B_grass_trees",new Vector3(33,0,82),-12f,28f,root,false);
            Spawn(natureRoot,"hill_single_A",new Vector3(-48,0,60),15f,9f,root,false);
            Spawn(natureRoot,"hill_single_B",new Vector3(48,0,61),-18f,10f,root,false);
            Spawn(natureRoot,"trees_A_medium",new Vector3(-31,0,53),0f,6f,root,false);
            Spawn(natureRoot,"trees_B_medium",new Vector3(30,0,55),0f,6f,root,false);

            // Arched thresholds and skyline.
            Spawn(TownRoot,"wall-arch",new Vector3(4,0.1f,24.5f),-12f,6.5f,root);
            Spawn(TownRoot,"wall-arch-top-detail",new Vector3(4,5.9f,24.5f),-12f,2.2f,root,false);
            BuildGate(new Vector3(9,2.45f,50),root);

            // FF11-like set dressing: many inexpensive objects instead of a few giant meshes.
            Vector3[] trees={
                new Vector3(-25,0,15),new Vector3(28,0,23),new Vector3(-25,0,-18),new Vector3(25,0,-18),
                new Vector3(-28,0,2),new Vector3(29,0,7),new Vector3(-24,0,27),new Vector3(31,2.45f,34),
                new Vector3(-22,2.45f,45),new Vector3(3,2.45f,47)
            };
            for(int i=0;i<trees.Length;i++)
                Spawn(TownRoot,i%3==0?"tree-high-round":(i%2==0?"tree-high":"tree-crooked"),trees[i],i*29f,5.0f+(i%4)*0.55f,root,false);

            Vector3[] lamps={
                new Vector3(-5,0,-29),new Vector3(4,0,-24),new Vector3(4,0,-18),new Vector3(-3,0,-12),
                new Vector3(-2,0,-6),new Vector3(10,0,1),new Vector3(-5,0,12),new Vector3(13,0,18),
                new Vector3(2,0,24),new Vector3(-10,0,6),new Vector3(18,0,-2),new Vector3(7,2.45f,35)
            };
            foreach(var p in lamps) Spawn(TownRoot,"lantern",p,0f,2.55f,root,false);

            Spawn(TownRoot,"cart",new Vector3(-21,0,-2),98f,1.8f,root,false);
            Spawn(TownRoot,"cart-high",new Vector3(22,0,13),-77f,2.0f,root,false);
            Spawn(TownRoot,"fence-gate",new Vector3(-11,0,26),10f,1.35f,root,false);
            for(int i=0;i<7;i++) Spawn(TownRoot,"hedge",new Vector3(-28+i*3.4f,0,21),0f,1.0f,root,false);
            for(int i=0;i<4;i++) Spawn(TownRoot,"hedge-large",new Vector3(22+i*2.8f,0,25),90f,1.25f,root,false);
            for(int i=0;i<4;i++) Spawn(TownRoot,"fence",new Vector3(-27+i*2.6f,0,-13),90f,1.0f,root,false);

            Spawn(TownRoot,"pillar-stone",new Vector3(-7,0,4),0f,2.4f,root,false);
            Spawn(TownRoot,"pillar-stone",new Vector3(15,0,4),0f,2.4f,root,false);
            Spawn(TownRoot,"banner-red",new Vector3(-7,2.2f,4),0f,2.2f,root,false);
            Spawn(TownRoot,"banner-green",new Vector3(15,2.2f,4),0f,2.2f,root,false);
            Spawn(TownRoot,"rock-wide",new Vector3(-30,0,31),22f,1.2f,root,false);
            Spawn(TownRoot,"rock-small",new Vector3(33,0,27),-15f,0.8f,root,false);

            // Two extra thresholds break long sightlines and create the layered FF11-town feeling.
            Spawn(TownRoot,"wall-arch",new Vector3(-1,0.05f,-8),0f,4.6f,root,false);
            Spawn(TownRoot,"wall-arch-top-detail",new Vector3(-1,4.0f,-8),0f,1.5f,root,false);

            // Hero + residents use fantasy rigged characters; no modern skins / no T-pose acceptance.
            var controller=EnsureAdventurerController(out var idleClip);
            var player=FantasyCharacter("SORA",new Vector3(-1,0,-20),4f,"Knight","knight_texture.png",true,root,controller,idleClip);
            string[] archetypes={"Mage","Ranger","Rogue","Druid","Engineer","Knight"};
            string[] textures={"mage_texture.png","ranger_texture.png","rogue_texture.png","druid_texture.png","engineer_texture.png","knight_texture.png"};
            Vector3[] npcPos={
                new Vector3(-14,0,-5),new Vector3(-18,0,1),new Vector3(-16,0,6),new Vector3(12,0,3),
                new Vector3(16,0,7),new Vector3(13,0,11),new Vector3(0,0,8),new Vector3(7,0,14),
                new Vector3(-7,0,17),new Vector3(4,0,21),new Vector3(-2,0,-15),new Vector3(7,0,-18),
                new Vector3(-8,0,-22),new Vector3(18,0,-6),new Vector3(-3,2.45f,38),new Vector3(14,2.45f,40),
                new Vector3(24,2.45f,35),new Vector3(-16,2.45f,42)
            };
            for(int i=0;i<npcPos.Length;i++)
                FantasyCharacter("Resident "+(i+1),npcPos[i],(i*43)%360,archetypes[i%archetypes.Length],textures[i%textures.Length],false,root,controller,idleClip);

            AddLight("Sun",Hex("#FFE1BC"),1.16f,new Vector3(48,-34,-8),true);
            AddLight("SkyFill",Hex("#BDD4E5"),0.18f,new Vector3(62,148,0),false);

            var camGo=new GameObject("Main Camera");camGo.tag="MainCamera";
            var cam=camGo.AddComponent<Camera>();cam.clearFlags=RenderSettings.skybox!=null?CameraClearFlags.Skybox:CameraClearFlags.SolidColor;cam.backgroundColor=Hex("#AFC6D2");
            cam.nearClipPlane=0.08f;cam.farClipPlane=220f;cam.fieldOfView=54f;
            camGo.AddComponent<AudioListener>();
            var rig=camGo.AddComponent<Dolzore.ThirdPersonCamera3D>();rig.target=player.transform;rig.distance=7.2f;rig.height=3.2f;
            camGo.transform.position=new Vector3(0f,3.3f,-27.5f);
            camGo.transform.rotation=Quaternion.LookRotation(new Vector3(4f,2.7f,10f)-camGo.transform.position,Vector3.up);

            // Dedicated deterministic preview camera. The runtime follow camera can move during
            // component initialization, so CI screenshots must not depend on its transient pose.
            var previewGo=new GameObject("Preview Camera");
            var preview=previewGo.AddComponent<Camera>();
            preview.enabled=false;
            preview.clearFlags=RenderSettings.skybox!=null?CameraClearFlags.Skybox:CameraClearFlags.SolidColor;
            preview.backgroundColor=Hex("#AFC6D2");
            preview.nearClipPlane=0.08f;
            preview.farClipPlane=240f;
            preview.fieldOfView=52f;
            previewGo.transform.position=new Vector3(-0.6f,3.5f,-28.0f);
            previewGo.transform.rotation=Quaternion.LookRotation(new Vector3(4f,2.6f,9f)-previewGo.transform.position,Vector3.up);

            EditorSceneManager.SaveScene(scene,ScenePath);
            Debug.Log("DOLZORE_FIRST_TOWN_3D_FF11_STYLE_BUILD=PASS");
        }
    }
}