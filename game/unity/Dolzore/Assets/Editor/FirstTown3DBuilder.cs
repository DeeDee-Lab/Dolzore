using System;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Dolzore.Editor
{
    public static class FirstTown3DBuilder
    {
        public const string ScenePath = "Assets/Scenes/FirstTown3D.unity";

        private static Material Mat(string name, Color color, float metallic=0f, float smooth=0.15f)
        {
            var shader = Shader.Find("Standard") ?? Shader.Find("Universal Render Pipeline/Lit") ?? Shader.Find("Unlit/Color");
            var m = new Material(shader) { name = name, color = color };
            if (m.HasProperty("_Metallic")) m.SetFloat("_Metallic", metallic);
            if (m.HasProperty("_Smoothness")) m.SetFloat("_Smoothness", smooth);
            return m;
        }

        private static GameObject Cube(string name, Vector3 pos, Vector3 scale, Material mat, Transform parent=null)
        {
            var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
            go.name = name; go.transform.position = pos; go.transform.localScale = scale;
            if (parent != null) go.transform.SetParent(parent, true);
            go.GetComponent<Renderer>().sharedMaterial = mat;
            return go;
        }

        private static GameObject Cylinder(string name, Vector3 pos, Vector3 scale, Material mat, Transform parent=null)
        {
            var go = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            go.name = name; go.transform.position = pos; go.transform.localScale = scale;
            if (parent != null) go.transform.SetParent(parent, true);
            go.GetComponent<Renderer>().sharedMaterial = mat;
            return go;
        }

        private static void Building(string name, Vector3 p, Vector3 s, Material wall, Material roof, Transform parent)
        {
            Cube(name, p + Vector3.up * (s.y * 0.5f), s, wall, parent);
            Cube(name+" Roof", p + Vector3.up * (s.y + 0.35f), new Vector3(s.x+0.3f,0.6f,s.z+0.3f), roof, parent);
            Cube(name+" Door", p + new Vector3(0,1.15f,-s.z*0.5f-0.03f), new Vector3(1.2f,2.3f,0.12f), Mat(name+" DoorMat", new Color(0.16f,0.12f,0.09f)), parent);
            int cols=Mathf.Max(1,Mathf.FloorToInt(s.x/3f));
            for(int i=0;i<cols;i++){
                float x=(i-(cols-1)*0.5f)*2.4f;
                Cube(name+" Window "+i,p+new Vector3(x,Mathf.Min(s.y-1.3f,2.8f),-s.z*0.5f-0.05f),new Vector3(0.8f,0.9f,0.08f),Mat(name+" WindowMat"+i,new Color(0.26f,0.47f,0.58f)),parent);
            }
        }

        private static void Lamp(Vector3 p, Material metal, Material glow, Transform parent)
        {
            Cylinder("Lamp Post", p+Vector3.up*1.6f, new Vector3(0.08f,1.6f,0.08f), metal, parent);
            var l = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            l.name="Lamp"; l.transform.position=p+Vector3.up*3.25f; l.transform.localScale=Vector3.one*0.35f; l.transform.SetParent(parent,true);
            l.GetComponent<Renderer>().sharedMaterial=glow;
        }

        private static void Sign(string text, Vector3 pos, Quaternion rot, Transform parent)
        {
            var go=new GameObject("Sign "+text);
            go.transform.position=pos; go.transform.rotation=rot; go.transform.SetParent(parent,true);
            var tm=go.AddComponent<TextMesh>();
            tm.text=text; tm.anchor=TextAnchor.MiddleCenter; tm.alignment=TextAlignment.Center; tm.fontSize=48; tm.characterSize=0.10f;
            tm.color=new Color(0.95f,0.86f,0.61f);
            var r=go.GetComponent<MeshRenderer>();
            var f=Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf"); tm.font=f; r.sharedMaterial=f.material;
        }

        public static void Build()
        {
            var scene=EditorSceneManager.NewScene(NewSceneSetup.EmptyScene,NewSceneMode.Single);
            RenderSettings.ambientLight=new Color(0.38f,0.38f,0.36f);
            RenderSettings.fog=true; RenderSettings.fogColor=new Color(0.53f,0.58f,0.60f); RenderSettings.fogDensity=0.004f;

            var stone=Mat("Warm Ironstone",new Color(0.37f,0.31f,0.25f));
            var paleStone=Mat("Pale Cut Stone",new Color(0.56f,0.51f,0.43f));
            var darkStone=Mat("Dark Stone",new Color(0.20f,0.19f,0.18f));
            var metal=Mat("Industrial Metal",new Color(0.20f,0.23f,0.24f),0.75f,0.35f);
            var copper=Mat("Copper",new Color(0.48f,0.24f,0.11f),0.65f,0.3f);
            var road=Mat("Packed Road",new Color(0.30f,0.28f,0.25f));
            var water=Mat("Canal Water",new Color(0.08f,0.27f,0.34f),0.05f,0.65f);
            var grass=Mat("Dry Green",new Color(0.24f,0.31f,0.18f));
            var glow=Mat("Lamp Glow",new Color(0.95f,0.68f,0.28f));
            var black=Mat("Mine Void",new Color(0.025f,0.025f,0.025f));

            var root=new GameObject("FIRST TOWN 3D - IRON BASIN").transform;

            // land and roads
            Cube("Ground",new Vector3(0,-0.55f,0),new Vector3(150,1,120),grass,root);
            Cube("Main Avenue",new Vector3(0,0,5),new Vector3(18,0.35f,105),road,root);
            Cube("Market Road",new Vector3(-26,0,8),new Vector3(40,0.35f,14),road,root);
            Cube("Civic Road",new Vector3(28,0,-22),new Vector3(46,0.35f,14),road,root);

            // canal and bridges
            Cube("Canal",new Vector3(31,-0.22f,19),new Vector3(18,0.18f,82),water,root);
            for(int z=-12; z<=46; z+=29){
                Cube("Stone Bridge "+z,new Vector3(31,0.65f,z),new Vector3(20,1.1f,7),paleStone,root);
                for(int side=-1;side<=1;side+=2)
                    Cube("Bridge Rail "+z+" "+side,new Vector3(31+side*9.3f,1.8f,z),new Vector3(0.7f,2.2f,7),darkStone,root);
            }

            // terraced industrial wall / elevation
            Cube("West Terrace",new Vector3(-48,2.5f,0),new Vector3(38,5,110),darkStone,root);
            Cube("West Terrace Top",new Vector3(-48,5.15f,0),new Vector3(38,0.3f,110),stone,root);
            Cube("North Ridge",new Vector3(0,3.0f,55),new Vector3(120,6,14),darkStone,root);

            // civic / commercial buildings
            Building("Republic Hall",new Vector3(12,0,-35),new Vector3(16,10,12),paleStone,darkStone,root);
            Building("Market Exchange",new Vector3(-23,0,-4),new Vector3(18,8,14),stone,darkStone,root);
            Building("Foundry Guild",new Vector3(-23,0,20),new Vector3(18,9,15),darkStone,metal,root);
            Building("Inn Brass Lantern",new Vector3(9,0,31),new Vector3(14,8,11),stone,copper,root);
            Building("Workshop Quarter A",new Vector3(47,0,-19),new Vector3(14,7,12),darkStone,metal,root);
            Building("Workshop Quarter B",new Vector3(49,0,1),new Vector3(13,8,13),stone,metal,root);
            Building("Canal Warehouse",new Vector3(48,0,30),new Vector3(14,9,15),darkStone,copper,root);

            // industrial chimneys / pipes
            for(int i=0;i<5;i++){
                float z=-10+i*14;
                Cylinder("Foundry Chimney "+i,new Vector3(-47,10f,z),new Vector3(2.2f,10f,2.2f),metal,root);
                Cylinder("Copper Ring "+i,new Vector3(-47,8f,z),new Vector3(2.5f,0.45f,2.5f),copper,root);
            }
            Cube("Main Pipe",new Vector3(-37,7,18),new Vector3(2,2,45),metal,root);
            for(int z=-4;z<=40;z+=11) Cylinder("Pipe Brace "+z,new Vector3(-37,3.5f,z),new Vector3(0.35f,3.5f,0.35f),copper,root);

            // mine approach and entrance
            Cube("Mine Cliff",new Vector3(0,12,61),new Vector3(56,20,12),darkStone,root);
            Cube("Mine Gate Left",new Vector3(-8,7,54.6f),new Vector3(10,14,3),stone,root);
            Cube("Mine Gate Right",new Vector3(8,7,54.6f),new Vector3(10,14,3),stone,root);
            Cube("Mine Lintel",new Vector3(0,14,54.6f),new Vector3(26,4,3),stone,root);
            Cube("Mine Darkness",new Vector3(0,6.2f,53.0f),new Vector3(7.5f,10,0.5f),black,root);
            Sign("DEEP WORKS",new Vector3(0,16.8f,52.8f),Quaternion.identity,root);

            // plaza / monuments / details
            Cube("Civic Plaza",new Vector3(10,0.12f,-18),new Vector3(25,0.25f,20),paleStone,root);
            Cylinder("Founder Monument Base",new Vector3(10,1.0f,-18),new Vector3(2.4f,1,2.4f),darkStone,root);
            Cylinder("Founder Monument",new Vector3(10,4.0f,-18),new Vector3(0.9f,3,0.9f),copper,root);

            // stairs climbing west terrace
            for(int i=0;i<12;i++) Cube("Terrace Step "+i,new Vector3(-30-i*0.85f,0.22f+i*0.42f,-30),new Vector3(2.1f,0.42f,9),paleStone,root);
            Building("Upper Barracks",new Vector3(-49,5.3f,-31),new Vector3(18,8,15),stone,darkStone,root);
            Building("Upper Residence",new Vector3(-49,5.3f,15),new Vector3(16,7,14),stone,copper,root);

            // lamps, crates, ore carts
            for(int z=-42;z<=42;z+=14){ Lamp(new Vector3(-7,0,z),metal,glow,root); Lamp(new Vector3(7,0,z),metal,glow,root); }
            for(int i=0;i<12;i++){
                float x=-20+(i%4)*3.2f, z=37+(i/4)*3f;
                Cube("Ore Crate "+i,new Vector3(x,0.7f,z),new Vector3(2.2f,1.4f,2.2f),copper,root);
            }
            Cube("Ore Cart",new Vector3(-16,0.8f,45),new Vector3(4.5f,1.6f,2.5f),metal,root);
            for(int s=-1;s<=1;s+=2) Cylinder("Cart Wheel "+s,new Vector3(-16+s*1.5f,0.2f,43.9f),new Vector3(0.65f,0.25f,0.65f),darkStone,root);

            Sign("IRON BASIN",new Vector3(0,7.5f,-48),Quaternion.identity,root);
            Sign("MARKET",new Vector3(-23,6.2f,-11.2f),Quaternion.identity,root);
            Sign("FOUNDRY",new Vector3(-23,7.0f,12.2f),Quaternion.identity,root);
            Sign("REPUBLIC HALL",new Vector3(12,7.2f,-41.2f),Quaternion.identity,root);

            // player
            var player=GameObject.CreatePrimitive(PrimitiveType.Capsule);
            player.name="Player";
            player.transform.position=new Vector3(0,1.15f,-43);
            player.transform.localScale=new Vector3(0.8f,1.15f,0.8f);
            player.GetComponent<Renderer>().sharedMaterial=Mat("Player Coat",new Color(0.20f,0.36f,0.50f));
            UnityEngine.Object.DestroyImmediate(player.GetComponent<CapsuleCollider>());
            var cc=player.AddComponent<CharacterController>();
            cc.height=2.1f; cc.radius=0.45f; cc.center=new Vector3(0,1.05f,0);
            player.AddComponent<Dolzore.ThirdPersonWalker3D>();

            var camGo=new GameObject("Main Camera");
            camGo.tag="MainCamera";
            var cam=camGo.AddComponent<Camera>();
            cam.clearFlags=CameraClearFlags.SolidColor;
            cam.backgroundColor=new Color(0.40f,0.48f,0.53f);
            cam.nearClipPlane=0.1f; cam.farClipPlane=500f; cam.fieldOfView=62f;
            var rig=camGo.AddComponent<Dolzore.ThirdPersonCamera3D>();
            rig.target=player.transform;
            camGo.transform.position=player.transform.position+new Vector3(0,5,-9);

            var sunGo=new GameObject("Sun");
            var sun=sunGo.AddComponent<Light>(); sun.type=LightType.Directional; sun.intensity=1.25f; sun.color=new Color(1f,0.88f,0.72f);
            sunGo.transform.rotation=Quaternion.Euler(42,-35,0);
            var fillGo=new GameObject("Fill Light");
            var fill=fillGo.AddComponent<Light>(); fill.type=LightType.Directional; fill.intensity=0.45f; fill.color=new Color(0.55f,0.66f,0.78f);
            fillGo.transform.rotation=Quaternion.Euler(50,145,0);

            // preview camera target composition is same runtime camera
            EditorSceneManager.SaveScene(scene,ScenePath);
            Debug.Log("DOLZORE_FIRST_TOWN_3D_BUILD=PASS");
        }
    }
}
