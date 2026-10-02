using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
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
            b=BoundsOf(go);
            go.transform.position=bottomPos+Vector3.up*(bottomPos.y-b.min.y);
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
            b=BoundsOf(go);
            go.transform.position=bottomPos+Vector3.up*(bottomPos.y-b.min.y);
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
            var stone=Flat("GateStone","#9A8D78",0.18f);
            Cube("GateFoundation",center+new Vector3(0,1.0f,0),new Vector3(22,2,8),stone,parent);
            Spawn(CastleRoot,"tower-square",center+new Vector3(-10,2.0f,0),0f,18f,parent);
            Spawn(CastleRoot,"tower-square",center+new Vector3(10,2.0f,0),0f,18f,parent);
            Spawn(CastleRoot,"gate",center+new Vector3(0,2.0f,-0.2f),0f,9.0f,parent);
            Spawn(CastleRoot,"flag-banner-long",center+new Vector3(-10,18.5f,-1.0f),0f,5.0f,parent,false);
            Spawn(CastleRoot,"flag-banner-long",center+new Vector3(10,18.5f,-1.0f),0f,5.0f,parent,false);
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
                Vector3 p=origin+rot*(new Vector3((i-1.5f)*4.1f,0f,0f));
                Spawn(TownRoot,"stall-bench",p,yaw,1.6f,parent,false);
                Spawn(TownRoot,i%2==0?"banner-red":"banner-green",p+Vector3.up*1.6f+rot*Vector3.back*0.2f,yaw,1.8f,parent,false);
            }
            Spawn(TownRoot,"cart",origin+rot*new Vector3(-10f,0f,2.6f),yaw+12f,2.0f,parent,false);
            Spawn(TownRoot,"cart-high",origin+rot*new Vector3(10f,0f,2.0f),yaw-9f,2.2f,parent,false);
        }

        private static Material CharacterMaterial(string skinFile,string key)
        {
            if(Materials.TryGetValue(key,out var cached)) return cached;
            var tex=AssetDatabase.LoadAssetAtPath<Texture2D>(CharacterRoot+"/Skins/"+skinFile);
            var shader=Shader.Find("Standard") ?? Shader.Find("Universal Render Pipeline/Lit") ?? Shader.Find("Unlit/Texture");
            var mat=new Material(shader){name=key,color=Color.white,mainTexture=tex};
            if(mat.HasProperty("_BaseMap")) mat.SetTexture("_BaseMap",tex);
            if(mat.HasProperty("_Smoothness")) mat.SetFloat("_Smoothness",0.08f);
            Materials[key]=mat; return mat;
        }

        private static GameObject Character(string name,Vector3 p,float yaw,string skin,bool player,Transform parent)
        {
            var visual=Spawn(CharacterRoot+"/Model","characterMedium",p,yaw,1.72f,parent,false);
            visual.name=name+" Visual";
            SetLayerMaterial(visual,CharacterMaterial(skin,name+" Mat"));
            if(!player) return visual;

            var root=new GameObject(name);
            root.transform.position=p; root.transform.rotation=Quaternion.Euler(0f,yaw,0f); root.transform.SetParent(parent,true);
            visual.transform.SetParent(root.transform,true);
            var cc=root.AddComponent<CharacterController>(); cc.height=1.78f; cc.radius=0.33f; cc.center=new Vector3(0,0.89f,0);
            root.AddComponent<Dolzore.ThirdPersonWalker3D>();
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
                CharacterRoot+"/Model/characterMedium.fbx"
            };
            foreach(var p in mustExist) if(AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(p)==null) throw new Exception("DOLZORE_EXTERNAL_ASSET_IMPORT_FAILED:"+p);

            var scene=EditorSceneManager.NewScene(NewSceneSetup.EmptyScene,NewSceneMode.Single);
            RenderSettings.ambientMode=AmbientMode.Trilight;
            RenderSettings.ambientSkyColor=Hex("#AAB9C7");
            RenderSettings.ambientEquatorColor=Hex("#8A7D69");
            RenderSettings.ambientGroundColor=Hex("#3D433A");
            RenderSettings.ambientIntensity=0.62f;
            RenderSettings.fog=true;RenderSettings.fogMode=FogMode.ExponentialSquared;RenderSettings.fogDensity=0.0045f;RenderSettings.fogColor=Hex("#A7B6BE");

            QualitySettings.shadowDistance=120f; QualitySettings.antiAliasing=4;

            var root=new GameObject("FIRST TOWN 3D - ASTER GATE").transform;
            var grass=Textured("Grass","grass.png","#617B4A",new Vector2(12,12));
            var cobble=Textured("Cobble","cobble.png","#9B907E",new Vector2(9,9),0.10f);
            var plaster=Textured("Plaster","plaster.png","#C8B697",new Vector2(3,3),0.09f);
            var stone=Flat("Stone","#8D8373",0.12f);
            var dark=Flat("DarkWood","#4A3A31",0.10f);

            Cube("Ground",new Vector3(0,-0.55f,7),new Vector3(110,1,105),grass,root);

            // Dense street ribbons: narrow enough to feel enclosed, never the old empty-lawn blockout.
            Cube("SouthStreet",new Vector3(-2,0,-22),new Vector3(8.2f,0.20f,34),cobble,root);
            var bend=Cube("CentralStreet",new Vector3(2,0,-2),new Vector3(8.2f,0.20f,25),cobble,root); bend.transform.eulerAngles=new Vector3(0,-10,0);
            var north=Cube("NorthStreet",new Vector3(7,0,22),new Vector3(8.2f,0.20f,29),cobble,root); north.transform.eulerAngles=new Vector3(0,-14,0);
            Cube("MarketLane",new Vector3(-15,0,-1),new Vector3(27,0.18f,5.8f),cobble,root).transform.eulerAngles=new Vector3(0,7,0);
            Cube("CivicLane",new Vector3(20,0,8),new Vector3(28,0.18f,5.5f),cobble,root).transform.eulerAngles=new Vector3(0,-12,0);
            Cube("Plaza",new Vector3(4,0.06f,6),new Vector3(18,0.20f,17),cobble,root);

            // Raised northern ward and visible ascent.
            Cube("UpperWard",new Vector3(11,2.1f,38),new Vector3(52,4.2f,22),stone,root);
            for(int i=0;i<10;i++) Cube("UpperStep "+i,new Vector3(4.5f,0.18f+i*0.21f,28+i*0.8f),new Vector3(8.0f,0.35f,1.0f),stone,root);

            // Street enclosure: asymmetrical multi-storey blocks.
            BuildHouse("Lantern Inn",new Vector3(-10,0,-23),18f,3,2,true,true,true,root,plaster);
            BuildHouse("South Residence",new Vector3(9,0,-23),-12f,3,2,false,false,false,root,plaster);
            BuildHouse("Market House A",new Vector3(-16,0,-8),83f,3,2,true,true,false,root,plaster);
            BuildHouse("Market House B",new Vector3(-18,0,8),96f,2,3,false,true,true,root,plaster);
            BuildHouse("Guild Annex",new Vector3(16,0,-8),-78f,3,3,false,true,true,root,plaster);
            BuildHouse("Workshop Row",new Vector3(19,0,17),-78f,3,2,true,false,false,root,plaster);
            BuildHouse("Canal House",new Vector3(-11,0,18),30f,2,2,true,true,false,root,plaster);
            BuildHouse("North Hostel",new Vector3(-5,0,32),10f,3,3,false,true,true,root,plaster);
            BuildHouse("Upper Archive",new Vector3(23,4.25f,38),-18f,4,3,false,true,true,root,plaster);
            BuildHouse("Upper Residence",new Vector3(-13,4.25f,39),12f,3,2,true,false,false,root,plaster);

            // Landmark square and market life.
            BuildFountain(new Vector3(4,0.18f,7),root);
            BuildMarket(new Vector3(-17,0.05f,2),92f,root);
            BuildMarket(new Vector3(15,0.05f,6),-83f,root);

            // Arched thresholds and skyline.
            Spawn(TownRoot,"wall-arch",new Vector3(4,0.1f,24.5f),-12f,6.5f,root);
            Spawn(TownRoot,"wall-arch-top-detail",new Vector3(4,5.9f,24.5f),-12f,2.2f,root,false);
            BuildGate(new Vector3(9,4.25f,50),root);

            // Vertical detail / trees / lamps / carts / hedges.
            Vector3[] trees={new Vector3(-25,0,15),new Vector3(28,0,23),new Vector3(-25,0,-18),new Vector3(31,4.25f,34),new Vector3(-22,4.25f,45)};
            for(int i=0;i<trees.Length;i++) Spawn(TownRoot,i%2==0?"tree-high":"tree-crooked",trees[i],i*33f,5.5f+(i%3)*0.7f,root,false);
            Vector3[] lamps={new Vector3(-5,0,-29),new Vector3(4,0,-18),new Vector3(-2,0,-6),new Vector3(10,0,1),new Vector3(-5,0,12),new Vector3(13,0,18),new Vector3(2,0,24)};
            foreach(var p in lamps) Spawn(TownRoot,"lantern",p,0f,2.8f,root,false);
            Spawn(TownRoot,"cart",new Vector3(-21,0,-2),98f,2.0f,root,false);
            Spawn(TownRoot,"cart-high",new Vector3(22,0,13),-77f,2.1f,root,false);
            for(int i=0;i<5;i++) Spawn(TownRoot,"hedge",new Vector3(-27+i*4,0,20),0f,1.1f,root,false);
            Spawn(TownRoot,"fence-gate",new Vector3(-11,0,26),10f,1.4f,root,false);

            // Hero character and visible town population.
            var player=Character("SORA",new Vector3(-1,0,-31),4f,"skaterMaleA.png",true,root);
            string[] skins={"skaterFemaleA.png","skaterMaleA.png","criminalMaleA.png","cyborgFemaleA.png"};
            Vector3[] npcPos={
                new Vector3(-14,0,-3),new Vector3(-18,0,3),new Vector3(12,0,5),new Vector3(16,0,9),
                new Vector3(1,0,9),new Vector3(8,0,15),new Vector3(-7,0,18),new Vector3(5,0,22),
                new Vector3(-1,4.25f,39),new Vector3(18,4.25f,42)
            };
            for(int i=0;i<npcPos.Length;i++) Character("Resident "+(i+1),npcPos[i],(i*47)%360,skins[i%skins.Length],false,root);

            AddLight("Sun",Hex("#FFE0B8"),1.28f,new Vector3(47,-32,0),true);
            AddLight("SkyFill",Hex("#B9D0E2"),0.24f,new Vector3(62,148,0),false);

            var camGo=new GameObject("Main Camera");camGo.tag="MainCamera";
            var cam=camGo.AddComponent<Camera>();cam.clearFlags=CameraClearFlags.SolidColor;cam.backgroundColor=Hex("#9DB8C8");
            cam.nearClipPlane=0.08f;cam.farClipPlane=220f;cam.fieldOfView=54f;
            camGo.AddComponent<AudioListener>();
            var rig=camGo.AddComponent<Dolzore.ThirdPersonCamera3D>();rig.target=player.transform;rig.distance=8.5f;rig.height=3.7f;
            camGo.transform.position=new Vector3(0f,4.4f,-40f);
            camGo.transform.rotation=Quaternion.LookRotation(new Vector3(4f,3.0f,16f)-camGo.transform.position,Vector3.up);

            // Dedicated deterministic preview camera. The runtime follow camera can move during
            // component initialization, so CI screenshots must not depend on its transient pose.
            var previewGo=new GameObject("Preview Camera");
            var preview=previewGo.AddComponent<Camera>();
            preview.enabled=false;
            preview.clearFlags=CameraClearFlags.SolidColor;
            preview.backgroundColor=Hex("#9DB8C8");
            preview.nearClipPlane=0.08f;
            preview.farClipPlane=240f;
            preview.fieldOfView=58f;
            previewGo.transform.position=new Vector3(-1.5f,4.8f,-40.5f);
            previewGo.transform.rotation=Quaternion.LookRotation(new Vector3(4f,3.1f,18f)-previewGo.transform.position,Vector3.up);

            EditorSceneManager.SaveScene(scene,ScenePath);
            Debug.Log("DOLZORE_FIRST_TOWN_3D_FF11_STYLE_BUILD=PASS");
        }
    }
}