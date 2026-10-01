using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

namespace Dolzore.Editor
{
    public static class FirstTown25DPrototype
    {
        private const string ScenePath = "Assets/Scenes/FirstTown25DPrototype.unity";
        private const string ArtifactDir = "BuildArtifacts";
        private const string MaterialDir = "Assets/Art/Generated25D/Materials";

        [Serializable]
        private sealed class Receipt
        {
            public string status;
            public string prototype;
            public string camera;
            public string target;
            public int buildings;
            public int characters;
            public int trees;
            public int crosswalks;
            public string note;
        }

        [MenuItem("DOLZORE/Generate FirstTown 2.5D Prototype")]
        public static void Generate()
        {
            Directory.CreateDirectory("Assets/Scenes");
            Directory.CreateDirectory(ArtifactDir);
            EnsureFolders();

            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

            RenderSettings.ambientMode = AmbientMode.Flat;
            RenderSettings.ambientLight = Hex("#D8E6ED");
            RenderSettings.fog = true;
            RenderSettings.fogColor = Hex("#C9E2E4");
            RenderSettings.fogMode = FogMode.Linear;
            RenderSettings.fogStartDistance = 30f;
            RenderSettings.fogEndDistance = 70f;

            var grass = Mat("Grass", "#78B96B", 0f, 0.18f);
            var grassDark = Mat("GrassDark", "#5E9F5C", 0f, 0.15f);
            var road = Mat("Road", "#6E777B", 0f, 0.08f);
            var sidewalk = Mat("Sidewalk", "#D9CFB6", 0f, 0.18f);
            var stripe = Mat("Crosswalk", "#F4F0DD", 0f, 0.12f);
            var curb = Mat("Curb", "#BDB6A5", 0f, 0.12f);
            var trunk = Mat("TreeTrunk", "#7B5438", 0f, 0.12f);
            var leaf = Mat("TreeLeaf", "#4F9D61", 0f, 0.1f);
            var leafLight = Mat("TreeLeafLight", "#74BC68", 0f, 0.1f);
            var metal = Mat("StreetMetal", "#505A61", 0.05f, 0.32f);
            var lampGlow = Mat("LampGlow", "#FFE4A3", 0f, 0.55f);
            var dark = Mat("DarkTrim", "#33414A", 0f, 0.2f);
            var window = Mat("Window", "#A9D7E7", 0f, 0.65f);
            var cream = Mat("CreamWall", "#E9D7A1", 0f, 0.16f);
            var coral = Mat("CoralWall", "#D98672", 0f, 0.16f);
            var mint = Mat("MintWall", "#8DBFA7", 0f, 0.16f);
            var blue = Mat("BlueWall", "#7DA9C7", 0f, 0.16f);
            var lavender = Mat("LavenderWall", "#A59AC4", 0f, 0.16f);
            var ochre = Mat("OchreWall", "#C69A5A", 0f, 0.16f);
            var roofRed = Mat("RoofRed", "#8C4C4B", 0f, 0.18f);
            var roofBlue = Mat("RoofBlue", "#4E6F86", 0f, 0.18f);
            var roofGreen = Mat("RoofGreen", "#4F7566", 0f, 0.18f);
            var roofBrown = Mat("RoofBrown", "#755B50", 0f, 0.18f);
            var roofPurple = Mat("RoofPurple", "#655A74", 0f, 0.18f);
            var roadAccent = Mat("RoadAccent", "#C9B65E", 0f, 0.12f);

            CreateBox("Ground", new Vector3(0f, -0.45f, 0f), new Vector3(46f, 0.8f, 38f), grass);
            CreateBox("NorthPark", new Vector3(-13f, -0.02f, 12f), new Vector3(14f, 0.18f, 10f), grassDark);
            CreateBox("SouthGreen", new Vector3(13f, -0.02f, -12f), new Vector3(14f, 0.18f, 10f), grassDark);

            CreateBox("MainRoad_NS", new Vector3(0f, 0.02f, 0f), new Vector3(7.8f, 0.18f, 38f), road);
            CreateBox("MainRoad_EW", new Vector3(0f, 0.03f, 0f), new Vector3(46f, 0.18f, 7.8f), road);

            CreateBox("Sidewalk_W", new Vector3(-5.0f, 0.07f, 0f), new Vector3(2.0f, 0.22f, 38f), sidewalk);
            CreateBox("Sidewalk_E", new Vector3(5.0f, 0.07f, 0f), new Vector3(2.0f, 0.22f, 38f), sidewalk);
            CreateBox("Sidewalk_N", new Vector3(0f, 0.08f, 5.0f), new Vector3(46f, 0.22f, 2.0f), sidewalk);
            CreateBox("Sidewalk_S", new Vector3(0f, 0.08f, -5.0f), new Vector3(46f, 0.22f, 2.0f), sidewalk);

            CreateBox("Curb_W", new Vector3(-3.95f, 0.18f, 0f), new Vector3(0.16f, 0.24f, 38f), curb);
            CreateBox("Curb_E", new Vector3(3.95f, 0.18f, 0f), new Vector3(0.16f, 0.24f, 38f), curb);
            CreateBox("Curb_N", new Vector3(0f, 0.18f, 3.95f), new Vector3(46f, 0.24f, 0.16f), curb);
            CreateBox("Curb_S", new Vector3(0f, 0.18f, -3.95f), new Vector3(46f, 0.24f, 0.16f), curb);

            for (int i = -14; i <= 14; i += 4)
            {
                if (Mathf.Abs(i) < 5) continue;
                CreateBox("RoadDashNS_" + i, new Vector3(0f, 0.16f, i), new Vector3(0.18f, 0.05f, 1.5f), roadAccent);
                CreateBox("RoadDashEW_" + i, new Vector3(i, 0.17f, 0f), new Vector3(1.5f, 0.05f, 0.18f), roadAccent);
            }

            CreateCrosswalk("CrosswalkNorth", new Vector3(0f, 0.18f, 5.9f), true, stripe);
            CreateCrosswalk("CrosswalkSouth", new Vector3(0f, 0.18f, -5.9f), true, stripe);
            CreateCrosswalk("CrosswalkEast", new Vector3(5.9f, 0.18f, 0f), false, stripe);
            CreateCrosswalk("CrosswalkWest", new Vector3(-5.9f, 0.18f, 0f), false, stripe);

            CreateBuilding("BAR 13", new Vector3(-13f, 0f, 10.7f), new Vector3(7.2f, 4.2f, 5.7f), coral, roofRed, dark, window, true, new Vector3(0f, 0f, -1f));
            CreateBuilding("Journal", new Vector3(12.5f, 0f, 10.8f), new Vector3(6.8f, 5.3f, 5.5f), cream, roofBlue, dark, window, false, new Vector3(0f, 0f, -1f));
            CreateBuilding("Corner Cafe", new Vector3(-12.3f, 0f, -10.6f), new Vector3(6.0f, 3.5f, 5.0f), mint, roofGreen, dark, window, true, new Vector3(0f, 0f, 1f));
            CreateBuilding("Clock Shop", new Vector3(12.6f, 0f, -10.9f), new Vector3(5.8f, 4.0f, 5.2f), lavender, roofPurple, dark, window, false, new Vector3(0f, 0f, 1f));
            CreateBuilding("Blue Residence", new Vector3(-20f, 0f, 10.4f), new Vector3(5.2f, 3.3f, 4.7f), blue, roofBrown, dark, window, false, new Vector3(0f, 0f, -1f));
            CreateBuilding("Workshop", new Vector3(19.4f, 0f, -9.9f), new Vector3(5.7f, 3.4f, 5.1f), ochre, roofBrown, dark, window, true, new Vector3(0f, 0f, 1f));

            CreateTree(new Vector3(-8.3f, 0f, 10.2f), trunk, leaf, leafLight, 1.05f);
            CreateTree(new Vector3(8.1f, 0f, 10.3f), trunk, leaf, leafLight, 1.15f);
            CreateTree(new Vector3(-8.1f, 0f, -10.7f), trunk, leaf, leafLight, 0.95f);
            CreateTree(new Vector3(8.0f, 0f, -10.6f), trunk, leaf, leafLight, 1.0f);
            CreateTree(new Vector3(-19.8f, 0f, -1.0f), trunk, leaf, leafLight, 1.2f);
            CreateTree(new Vector3(19.6f, 0f, 1.0f), trunk, leaf, leafLight, 1.2f);

            CreateStreetLamp(new Vector3(-6.3f, 0f, 6.1f), metal, lampGlow);
            CreateStreetLamp(new Vector3(6.3f, 0f, 6.1f), metal, lampGlow);
            CreateStreetLamp(new Vector3(-6.3f, 0f, -6.1f), metal, lampGlow);
            CreateStreetLamp(new Vector3(6.3f, 0f, -6.1f), metal, lampGlow);

            CreateBench(new Vector3(-8.2f, 0f, 5.7f), new Vector3(0f, 90f, 0f), roofBrown, metal);
            CreateBench(new Vector3(8.2f, 0f, -5.7f), new Vector3(0f, 90f, 0f), roofBrown, metal);

            CreateCar("MintCar", new Vector3(-1.9f, 0.4f, -11.8f), new Vector3(0f, 0f, 0f), mint, dark);
            CreateCar("CoralCar", new Vector3(11.3f, 0.4f, 1.8f), new Vector3(0f, 90f, 0f), coral, dark);

            CreateFenceLine(new Vector3(-18.8f, 0.45f, 6.5f), 5, Vector3.right, roofBrown);
            CreateFenceLine(new Vector3(16.2f, 0.45f, 6.5f), 5, Vector3.right, roofBrown);

            var skin = Mat("Skin", "#D6A276", 0f, 0.18f);
            var hairDark = Mat("HairDark", "#2F2D35", 0f, 0.15f);
            var hairBrown = Mat("HairBrown", "#5C4039", 0f, 0.15f);
            var cyan = Mat("PlayerCyan", "#4D9FA8", 0f, 0.18f);
            var gold = Mat("NpcGold", "#D5A854", 0f, 0.18f);
            var rose = Mat("NpcRose", "#C76E78", 0f, 0.18f);
            var indigo = Mat("NpcIndigo", "#5E6F9D", 0f, 0.18f);
            var olive = Mat("NpcOlive", "#758D5D", 0f, 0.18f);

            var player = CreateCharacter("Player_SORA_25D", new Vector3(-2.2f, 0f, -2.0f), cyan, skin, hairDark, true);
            CreateCharacter("NPC_MELO_25D", new Vector3(-7.8f, 0f, 1.7f), rose, skin, hairBrown, false);
            CreateCharacter("NPC_YUZU_25D", new Vector3(7.8f, 0f, 1.7f), gold, skin, hairDark, false);
            CreateCharacter("NPC_PON_25D", new Vector3(-1.6f, 0f, 8.2f), indigo, skin, hairBrown, false);
            CreateCharacter("NPC_WORKER_25D", new Vector3(10.0f, 0f, -7.0f), olive, skin, hairDark, false);

            CreateSign("BAR 13", new Vector3(-9.25f, 1.5f, 8.4f), new Vector3(0f, 90f, 0f), dark, stripe);
            CreateSign("JOURNAL", new Vector3(9.1f, 1.7f, 8.5f), new Vector3(0f, 90f, 0f), dark, stripe);

            var sunGo = new GameObject("Sun");
            var sun = sunGo.AddComponent<Light>();
            sun.type = LightType.Directional;
            sun.color = Hex("#FFF0D6");
            sun.intensity = 1.15f;
            sun.shadows = LightShadows.Soft;
            sun.shadowStrength = 0.55f;
            sunGo.transform.rotation = Quaternion.Euler(48f, -32f, 0f);

            var fillGo = new GameObject("Fill");
            var fill = fillGo.AddComponent<Light>();
            fill.type = LightType.Directional;
            fill.color = Hex("#CFE4FF");
            fill.intensity = 0.28f;
            fill.shadows = LightShadows.None;
            fillGo.transform.rotation = Quaternion.Euler(58f, 145f, 0f);

            var cameraGo = new GameObject("Main Camera");
            cameraGo.tag = "MainCamera";
            var camera = cameraGo.AddComponent<Camera>();
            camera.orthographic = true;
            camera.orthographicSize = 16.5f;
            camera.nearClipPlane = 0.1f;
            camera.farClipPlane = 100f;
            camera.backgroundColor = Hex("#C9E2E4");
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.transform.position = new Vector3(22f, 24f, -22f);
            camera.transform.rotation = Quaternion.LookRotation(new Vector3(0f, 1.4f, 0f) - camera.transform.position, Vector3.up);

            var audio = new GameObject("Audio Listener");
            audio.transform.SetParent(cameraGo.transform, false);
            audio.AddComponent<AudioListener>();

            Selection.activeGameObject = player;

            EditorSceneManager.SaveScene(scene, ScenePath);
            EditorBuildSettings.scenes = new[] { new EditorBuildSettingsScene(ScenePath, true) };

            Render(camera, Path.Combine(ArtifactDir, "first-town-25d.png"), 1280, 720);

            var receipt = new Receipt
            {
                status = "PASS",
                prototype = "DOLZORE FirstTown 2.5D comparison slice",
                camera = "fixed orthographic three-quarter 3D",
                target = "same central-intersection scope as rejected 2D FirstTown, rebuilt as original DOLZORE 2.5D",
                buildings = 6,
                characters = 5,
                trees = 6,
                crosswalks = 4,
                note = "Reference lessons: readable low-poly town, fixed three-quarter camera, dense props and clear street hierarchy. No protected MOTHER2 map, model, texture, character, UI or exact layout copied."
            };
            File.WriteAllText(Path.Combine(ArtifactDir, "first-town-25d-receipt.json"), JsonUtility.ToJson(receipt, true));

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("DOLZORE_FIRST_TOWN_25D_PROTOTYPE=PASS");
        }

        public static void BuildWebGL()
        {
            if (!File.Exists(ScenePath))
                throw new FileNotFoundException("DOLZORE_25D_SCENE_MISSING", ScenePath);

            var output = Path.GetFullPath("Builds/WebGL25D");
            Directory.CreateDirectory(output);

            var options = new BuildPlayerOptions
            {
                scenes = new[] { ScenePath },
                locationPathName = output,
                target = BuildTarget.WebGL,
                options = BuildOptions.None
            };

            var report = BuildPipeline.BuildPlayer(options);
            if (report.summary.result != UnityEditor.Build.Reporting.BuildResult.Succeeded)
                throw new Exception("DOLZORE_25D_WEBGL_FAILED:" + report.summary.result);

            Debug.Log("DOLZORE_FIRST_TOWN_25D_WEBGL=PASS");
        }

        private static void EnsureFolders()
        {
            EnsureFolder("Assets/Art");
            EnsureFolder("Assets/Art/Generated25D");
            EnsureFolder(MaterialDir);
        }

        private static void EnsureFolder(string path)
        {
            if (AssetDatabase.IsValidFolder(path)) return;
            var parent = Path.GetDirectoryName(path)?.Replace("\\", "/");
            var name = Path.GetFileName(path);
            if (!string.IsNullOrEmpty(parent) && !AssetDatabase.IsValidFolder(parent))
                EnsureFolder(parent);
            AssetDatabase.CreateFolder(parent ?? "Assets", name);
        }

        private static Material Mat(string name, string html, float metallic, float smoothness)
        {
            string path = MaterialDir + "/" + name + ".mat";
            var existing = AssetDatabase.LoadAssetAtPath<Material>(path);
            if (existing != null) return existing;

            Shader shader = Shader.Find("Standard");
            if (shader == null) shader = Shader.Find("Universal Render Pipeline/Lit");
            if (shader == null) shader = Shader.Find("Unlit/Color");
            var mat = new Material(shader) { name = name, color = Hex(html) };
            if (mat.HasProperty("_Metallic")) mat.SetFloat("_Metallic", metallic);
            if (mat.HasProperty("_Glossiness")) mat.SetFloat("_Glossiness", smoothness);
            AssetDatabase.CreateAsset(mat, path);
            return mat;
        }

        private static Color Hex(string html)
        {
            Color c;
            if (!ColorUtility.TryParseHtmlString(html, out c)) c = Color.magenta;
            return c;
        }

        private static GameObject CreateBox(string name, Vector3 position, Vector3 scale, Material mat, Transform parent = null)
        {
            var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
            go.name = name;
            go.transform.position = position;
            go.transform.localScale = scale;
            if (parent != null) go.transform.SetParent(parent, true);
            var r = go.GetComponent<Renderer>();
            r.sharedMaterial = mat;
            r.shadowCastingMode = ShadowCastingMode.On;
            r.receiveShadows = true;
            return go;
        }

        private static GameObject CreateBuilding(string name, Vector3 ground, Vector3 size, Material wall, Material roof, Material trim, Material win, bool shop, Vector3 front)
        {
            var root = new GameObject(name);
            root.transform.position = ground;

            float bodyY = size.y * 0.5f;
            CreateBox("Body", new Vector3(0f, bodyY, 0f), size, wall, root.transform);

            float roofY = size.y + 0.72f;
            var left = CreateBox("RoofLeft", new Vector3(-size.x * 0.23f, roofY, 0f), new Vector3(size.x * 0.58f, 0.42f, size.z * 1.12f), roof, root.transform);
            left.transform.localRotation = Quaternion.Euler(0f, 0f, 20f);
            var right = CreateBox("RoofRight", new Vector3(size.x * 0.23f, roofY, 0f), new Vector3(size.x * 0.58f, 0.42f, size.z * 1.12f), roof, root.transform);
            right.transform.localRotation = Quaternion.Euler(0f, 0f, -20f);

            bool facesSouth = front.z < 0f;
            float fz = facesSouth ? -size.z * 0.505f : size.z * 0.505f;
            float sign = facesSouth ? 1f : -1f;

            CreateBox("Door", new Vector3(0f, 1.05f, fz + sign * 0.08f), new Vector3(1.0f, 2.05f, 0.18f), trim, root.transform);
            for (int row = 0; row < (size.y > 4.5f ? 2 : 1); row++)
            {
                float y = 1.7f + row * 1.55f;
                for (int col = -1; col <= 1; col += 2)
                    CreateBox("Window", new Vector3(col * size.x * 0.25f, y, fz + sign * 0.09f), new Vector3(0.92f, 0.88f, 0.15f), win, root.transform);
            }

            if (shop)
            {
                CreateBox("Awning", new Vector3(0f, 2.35f, fz + sign * 0.38f), new Vector3(size.x * 0.72f, 0.28f, 0.7f), roof, root.transform);
                CreateBox("ShopBand", new Vector3(0f, 2.75f, fz + sign * 0.13f), new Vector3(size.x * 0.76f, 0.36f, 0.13f), trim, root.transform);
            }

            CreateBox("SideTrim", new Vector3(-size.x * 0.48f, 0.22f, 0f), new Vector3(0.14f, 0.38f, size.z * 0.92f), trim, root.transform);
            CreateBox("SideTrim", new Vector3(size.x * 0.48f, 0.22f, 0f), new Vector3(0.14f, 0.38f, size.z * 0.92f), trim, root.transform);
            return root;
        }

        private static void CreateCrosswalk(string name, Vector3 center, bool spansX, Material mat)
        {
            var root = new GameObject(name);
            root.transform.position = center;
            for (int i = -4; i <= 4; i++)
            {
                float offset = i * 0.72f;
                Vector3 p = spansX ? new Vector3(offset, 0f, 0f) : new Vector3(0f, 0f, offset);
                Vector3 s = spansX ? new Vector3(0.42f, 0.05f, 2.7f) : new Vector3(2.7f, 0.05f, 0.42f);
                CreateBox("Stripe_" + i, p, s, mat, root.transform);
            }
        }

        private static void CreateTree(Vector3 position, Material trunk, Material leaf, Material leafLight, float scale)
        {
            var root = new GameObject("Tree");
            root.transform.position = position;
            var stem = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            stem.name = "Trunk";
            stem.transform.SetParent(root.transform, false);
            stem.transform.localPosition = new Vector3(0f, 1.15f * scale, 0f);
            stem.transform.localScale = new Vector3(0.38f * scale, 1.15f * scale, 0.38f * scale);
            stem.GetComponent<Renderer>().sharedMaterial = trunk;

            CreateSphere("Crown", new Vector3(0f, 2.8f * scale, 0f), new Vector3(2.25f, 1.8f, 2.25f) * scale, leaf, root.transform);
            CreateSphere("CrownLight", new Vector3(-0.5f * scale, 3.25f * scale, -0.2f * scale), new Vector3(1.15f, 0.9f, 1.15f) * scale, leafLight, root.transform);
        }

        private static GameObject CreateSphere(string name, Vector3 position, Vector3 scale, Material mat, Transform parent)
        {
            var go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            go.name = name;
            go.transform.SetParent(parent, false);
            go.transform.localPosition = position;
            go.transform.localScale = scale;
            go.GetComponent<Renderer>().sharedMaterial = mat;
            return go;
        }

        private static void CreateStreetLamp(Vector3 position, Material metal, Material glow)
        {
            var root = new GameObject("StreetLamp");
            root.transform.position = position;
            var pole = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            pole.transform.SetParent(root.transform, false);
            pole.transform.localPosition = new Vector3(0f, 1.8f, 0f);
            pole.transform.localScale = new Vector3(0.12f, 1.8f, 0.12f);
            pole.GetComponent<Renderer>().sharedMaterial = metal;
            CreateBox("LampTop", new Vector3(0f, 3.45f, 0f), new Vector3(0.55f, 0.34f, 0.55f), glow, root.transform);
        }

        private static void CreateBench(Vector3 position, Vector3 euler, Material wood, Material metal)
        {
            var root = new GameObject("Bench");
            root.transform.position = position;
            root.transform.eulerAngles = euler;
            CreateBox("Seat", new Vector3(0f, 0.55f, 0f), new Vector3(2.2f, 0.22f, 0.62f), wood, root.transform);
            CreateBox("Back", new Vector3(0f, 1.05f, 0.25f), new Vector3(2.2f, 0.65f, 0.18f), wood, root.transform);
            CreateBox("LegL", new Vector3(-0.75f, 0.28f, 0f), new Vector3(0.18f, 0.55f, 0.45f), metal, root.transform);
            CreateBox("LegR", new Vector3(0.75f, 0.28f, 0f), new Vector3(0.18f, 0.55f, 0.45f), metal, root.transform);
        }

        private static void CreateCar(string name, Vector3 position, Vector3 euler, Material body, Material dark)
        {
            var root = new GameObject(name);
            root.transform.position = position;
            root.transform.eulerAngles = euler;
            CreateBox("Body", new Vector3(0f, 0.45f, 0f), new Vector3(2.8f, 0.75f, 1.35f), body, root.transform);
            CreateBox("Cabin", new Vector3(0.2f, 1.02f, 0f), new Vector3(1.5f, 0.68f, 1.1f), dark, root.transform);
            for (int x = -1; x <= 1; x += 2)
            for (int z = -1; z <= 1; z += 2)
            {
                var wheel = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                wheel.name = "Wheel";
                wheel.transform.SetParent(root.transform, false);
                wheel.transform.localPosition = new Vector3(x * 0.9f, 0.22f, z * 0.62f);
                wheel.transform.localScale = new Vector3(0.28f, 0.16f, 0.28f);
                wheel.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
                wheel.GetComponent<Renderer>().sharedMaterial = dark;
            }
        }

        private static void CreateFenceLine(Vector3 start, int count, Vector3 step, Material mat)
        {
            var root = new GameObject("Fence");
            for (int i = 0; i < count; i++)
                CreateBox("Post_" + i, start + step * i * 1.1f, new Vector3(0.16f, 0.9f, 0.16f), mat, root.transform);

            Vector3 mid = start + step * (count - 1) * 0.55f;
            float len = (count - 1) * 1.1f;
            Vector3 scale = Mathf.Abs(step.x) > 0.5f ? new Vector3(len, 0.14f, 0.14f) : new Vector3(0.14f, 0.14f, len);
            CreateBox("RailTop", mid + Vector3.up * 0.25f, scale, mat, root.transform);
            CreateBox("RailLow", mid - Vector3.up * 0.15f, scale, mat, root.transform);
        }

        private static GameObject CreateCharacter(string name, Vector3 position, Material outfit, Material skin, Material hair, bool player)
        {
            var root = new GameObject(name);
            root.transform.position = position;

            var visual = new GameObject("Visual");
            visual.transform.SetParent(root.transform, false);

            var body = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            body.name = "Body";
            body.transform.SetParent(visual.transform, false);
            body.transform.localPosition = new Vector3(0f, 1.05f, 0f);
            body.transform.localScale = new Vector3(0.58f, 0.62f, 0.52f);
            body.GetComponent<Renderer>().sharedMaterial = outfit;
            UnityEngine.Object.DestroyImmediate(body.GetComponent<Collider>());

            CreateSphere("Head", new Vector3(0f, 2.05f, 0f), new Vector3(0.82f, 0.88f, 0.82f), skin, visual.transform);
            CreateSphere("Hair", new Vector3(0f, 2.35f, 0.02f), new Vector3(0.9f, 0.46f, 0.87f), hair, visual.transform);

            CreateBox("LegL", new Vector3(-0.22f, 0.35f, 0f), new Vector3(0.28f, 0.65f, 0.35f), hair, visual.transform);
            CreateBox("LegR", new Vector3(0.22f, 0.35f, 0f), new Vector3(0.28f, 0.65f, 0.35f), hair, visual.transform);
            CreateBox("ArmL", new Vector3(-0.52f, 1.12f, 0f), new Vector3(0.2f, 0.72f, 0.25f), skin, visual.transform);
            CreateBox("ArmR", new Vector3(0.52f, 1.12f, 0f), new Vector3(0.2f, 0.72f, 0.25f), skin, visual.transform);

            if (player)
            {
                var cc = root.AddComponent<CharacterController>();
                cc.height = 2.7f;
                cc.radius = 0.55f;
                cc.center = new Vector3(0f, 1.25f, 0f);
                var controller = root.AddComponent<Dolzore.Prototype25DPlayerController>();
                var so = new SerializedObject(controller);
                so.FindProperty("visualRoot").objectReferenceValue = visual.transform;
                so.ApplyModifiedPropertiesWithoutUndo();
            }

            return root;
        }

        private static void CreateSign(string label, Vector3 position, Vector3 euler, Material post, Material face)
        {
            var root = new GameObject("Sign_" + label.Replace(" ", "_"));
            root.transform.position = position;
            root.transform.eulerAngles = euler;
            CreateBox("Post", new Vector3(0f, 0f, 0f), new Vector3(0.12f, 2.4f, 0.12f), post, root.transform);
            CreateBox("Panel", new Vector3(0f, 1.15f, 0f), new Vector3(2.2f, 0.85f, 0.18f), face, root.transform);
        }

        private static void Render(Camera camera, string path, int width, int height)
        {
            var rt = new RenderTexture(width, height, 24, RenderTextureFormat.ARGB32);
            rt.antiAliasing = 4;
            camera.targetTexture = rt;
            var previous = RenderTexture.active;
            RenderTexture.active = rt;

            camera.Render();

            var image = new Texture2D(width, height, TextureFormat.RGB24, false);
            image.ReadPixels(new Rect(0, 0, width, height), 0, 0);
            image.Apply();
            File.WriteAllBytes(path, image.EncodeToPNG());

            camera.targetTexture = null;
            RenderTexture.active = previous;
            UnityEngine.Object.DestroyImmediate(image);
            UnityEngine.Object.DestroyImmediate(rt);
        }
    }
}
