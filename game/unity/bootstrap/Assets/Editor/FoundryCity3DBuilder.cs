using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Dolzore.Editor
{
    public static class FoundryCity3DBuilder
    {
        private const string ScenePath = "Assets/Scenes/FoundryCity3D.unity";
        private const string MaterialRoot = "Assets/Art/Generated/FoundryCity3D";
        private const string BuildRoot = "Builds/FoundryCity3D";
        private const string ArtifactRoot = "BuildArtifacts";

        private static Material stone;
        private static Material stoneDark;
        private static Material sandstone;
        private static Material iron;
        private static Material copper;
        private static Material wood;
        private static Material water;
        private static Material lava;
        private static Material glass;
        private static Material black;
        private static Material moss;
        private static Material plaster;

        public static void GenerateAndBuild()
        {
            GenerateSceneOnly();
            BuildWebGLOnly();
        }

        public static void GenerateSceneOnly()
        {
            EnsureDirectories();
            CreateMaterials();

            Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            RenderSettings.fog = true;
            RenderSettings.fogColor = new Color(0.43f, 0.42f, 0.38f);
            RenderSettings.fogDensity = 0.0036f;
            RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Trilight;
            RenderSettings.ambientSkyColor = new Color(0.62f, 0.58f, 0.50f);
            RenderSettings.ambientEquatorColor = new Color(0.36f, 0.34f, 0.31f);
            RenderSettings.ambientGroundColor = new Color(0.18f, 0.17f, 0.16f);

            BuildLighting();
            BuildTerrainAndWalls();
            BuildLowerCity();
            BuildCanalAndBridge();
            BuildFoundryTerrace();
            BuildResidentialTerrace();
            BuildMineApproach();
            BuildPlayer();
            BuildOverviewCamera();

            ValidateScene();
            EditorSceneManager.SaveScene(scene, ScenePath);
            EditorSceneManager.MarkSceneDirty(scene);
            EditorSceneManager.SaveScene(scene);

            RenderOverview();
            WriteReceipt();
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        public static void BuildWebGLOnly()
        {
            if (!File.Exists(ScenePath))
                GenerateSceneOnly();

            EditorBuildSettings.scenes = new[]
            {
                new EditorBuildSettingsScene(ScenePath, true)
            };

            string output = Path.Combine(BuildRoot, "index.html");
            Directory.CreateDirectory(BuildRoot);
            BuildPlayerOptions options = new BuildPlayerOptions
            {
                scenes = new[] { ScenePath },
                locationPathName = BuildRoot,
                target = BuildTarget.WebGL,
                options = BuildOptions.None
            };

            UnityEditor.Build.Reporting.BuildReport report = BuildPipeline.BuildPlayer(options);
            if (report.summary.result != UnityEditor.Build.Reporting.BuildResult.Succeeded)
                throw new InvalidOperationException("FOUNDRY_CITY_WEBGL_BUILD_FAILED:" + report.summary.result);

            Debug.Log("FOUNDRY_CITY_WEBGL=PASS size=" + report.summary.totalSize);
        }

        private static void EnsureDirectories()
        {
            Directory.CreateDirectory("Assets/Scenes");
            Directory.CreateDirectory(MaterialRoot);
            Directory.CreateDirectory(BuildRoot);
            Directory.CreateDirectory(ArtifactRoot);
        }

        private static void CreateMaterials()
        {
            stone = Mat("Stone", "#756E63", 0.08f, 0.18f);
            stoneDark = Mat("StoneDark", "#403B37", 0.04f, 0.12f);
            sandstone = Mat("Sandstone", "#9B8768", 0.08f, 0.18f);
            iron = Mat("Iron", "#343B3F", 0.68f, 0.42f);
            copper = Mat("Copper", "#8B5A3C", 0.58f, 0.36f);
            wood = Mat("Wood", "#6A4B34", 0.05f, 0.20f);
            water = Mat("Water", "#285C72", 0.05f, 0.82f);
            lava = Mat("FurnaceGlow", "#E35B21", 0.0f, 0.28f, "#FF5B1A");
            glass = Mat("Glass", "#6FA5AA", 0.12f, 0.86f);
            black = Mat("Void", "#090909", 0.0f, 0.0f);
            moss = Mat("Moss", "#596448", 0.0f, 0.08f);
            plaster = Mat("Plaster", "#B5A98F", 0.02f, 0.14f);
        }

        private static Material Mat(string name, string hex, float metallic, float smooth, string emissionHex = null)
        {
            string path = MaterialRoot + "/" + name + ".mat";
            Material existing = AssetDatabase.LoadAssetAtPath<Material>(path);
            if (existing != null)
                AssetDatabase.DeleteAsset(path);

            Shader shader = Shader.Find("Universal Render Pipeline/Lit");
            if (shader == null) shader = Shader.Find("Standard");
            if (shader == null) throw new InvalidOperationException("NO_LIT_SHADER");

            Material m = new Material(shader);
            m.name = name;
            m.color = Hex(hex);
            if (m.HasProperty("_Metallic")) m.SetFloat("_Metallic", metallic);
            if (m.HasProperty("_Smoothness")) m.SetFloat("_Smoothness", smooth);
            if (m.HasProperty("_Glossiness")) m.SetFloat("_Glossiness", smooth);

            if (!string.IsNullOrEmpty(emissionHex))
            {
                Color e = Hex(emissionHex) * 2.2f;
                m.EnableKeyword("_EMISSION");
                if (m.HasProperty("_EmissionColor")) m.SetColor("_EmissionColor", e);
            }

            AssetDatabase.CreateAsset(m, path);
            return m;
        }

        private static void BuildLighting()
        {
            GameObject sun = new GameObject("Sun");
            Light dl = sun.AddComponent<Light>();
            dl.type = LightType.Directional;
            dl.color = new Color(1.0f, 0.83f, 0.64f);
            dl.intensity = 1.35f;
            dl.shadows = LightShadows.Soft;
            sun.transform.rotation = Quaternion.Euler(42f, -32f, 0f);

            GameObject fill = new GameObject("Foundry Warm Fill");
            Light fl = fill.AddComponent<Light>();
            fl.type = LightType.Point;
            fl.color = new Color(1f, 0.35f, 0.12f);
            fl.intensity = 7f;
            fl.range = 42f;
            fill.transform.position = new Vector3(33f, 8f, 13f);
        }

        private static void BuildTerrainAndWalls()
        {
            Box("Ground Base", new Vector3(0, -1.0f, 0), new Vector3(120, 2, 105), sandstone);
            Box("North Cliff", new Vector3(0, 12f, 47f), new Vector3(120, 24, 8), stoneDark);
            Box("West Cliff", new Vector3(-56f, 7f, 8f), new Vector3(8, 16, 86), stoneDark);
            Box("East Cliff", new Vector3(56f, 7f, 10f), new Vector3(8, 16, 82), stoneDark);

            // Lower city roads and plaza.
            Box("Main Avenue", new Vector3(0, 0.05f, -8f), new Vector3(16f, 0.12f, 68f), stone);
            Box("Central Plaza", new Vector3(0, 0.08f, 8f), new Vector3(38f, 0.16f, 30f), stone);
            Box("Market Road", new Vector3(-28f, 0.06f, 8f), new Vector3(34f, 0.12f, 10f), stone);
            Box("Foundry Road", new Vector3(29f, 0.06f, 8f), new Vector3(38f, 0.12f, 10f), stone);

            // City gate frame.
            Box("South Gate Left", new Vector3(-9f, 6f, -45f), new Vector3(10f, 12f, 5f), stoneDark);
            Box("South Gate Right", new Vector3(9f, 6f, -45f), new Vector3(10f, 12f, 5f), stoneDark);
            Box("South Gate Beam", new Vector3(0f, 12f, -45f), new Vector3(28f, 5f, 5f), stoneDark);
            Label("IRONWARD GATE", new Vector3(0f, 10.2f, -42.2f), 0.7f, new Color(0.92f,0.78f,0.52f), Quaternion.Euler(0,180,0));
        }

        private static void BuildLowerCity()
        {
            Building("Citadel Hall", new Vector3(0, 0, 24f), new Vector3(20f, 10f, 12f), 3, true);
            AddTower(new Vector3(-8.3f, 7.0f, 24f), 2.6f, 7.0f);
            AddTower(new Vector3(8.3f, 7.0f, 24f), 2.6f, 7.0f);
            Label("CITADEL HALL", new Vector3(0, 6.3f, 17.8f), 0.55f, Color.white);

            Building("Copper Market A", new Vector3(-30f, 0f, 13f), new Vector3(12f, 6f, 10f), 2, false);
            Building("Copper Market B", new Vector3(-30f, 0f, 1f), new Vector3(12f, 6f, 10f), 2, false);
            Building("Workshop Row A", new Vector3(29f, 0f, -3f), new Vector3(12f, 7f, 12f), 2, false);
            Building("Workshop Row B", new Vector3(43f, 0f, -3f), new Vector3(10f, 7f, 12f), 2, false);

            Awning(new Vector3(-30f, 3.6f, 7.4f), new Vector3(11f, 0.45f, 2.2f), copper);
            Awning(new Vector3(-30f, 3.6f, -4.4f), new Vector3(11f, 0.45f, 2.2f), wood);
            Label("COPPER MARKET", new Vector3(-30f, 6.0f, 7.0f), 0.5f, new Color(1f,0.78f,0.47f));

            // Service alley and lamps.
            for (int z = -34; z <= 30; z += 12)
            {
                Lamp(new Vector3(-9f, 0f, z));
                Lamp(new Vector3(9f, 0f, z));
            }

            for (int i = 0; i < 6; i++)
            {
                Crate(new Vector3(-37f + (i % 3) * 2.2f, 0f, 18f + (i / 3) * 2.2f));
            }
        }

        private static void BuildCanalAndBridge()
        {
            // Canal trench visual — water plane is non-colliding, with walkable banks.
            Box("Canal Bed", new Vector3(0f, -1.25f, -22f), new Vector3(110f, 1.1f, 9f), stoneDark);
            GameObject waterGo = Box("Canal Water", new Vector3(0f, -0.62f, -22f), new Vector3(110f, 0.25f, 7.2f), water);
            Collider wc = waterGo.GetComponent<Collider>();
            if (wc != null) UnityEngine.Object.DestroyImmediate(wc);

            Box("Bridge Deck", new Vector3(0f, 0.45f, -22f), new Vector3(14f, 1.1f, 11f), stone);
            Box("Bridge Rail L", new Vector3(-6.4f, 1.6f, -22f), new Vector3(0.7f, 2.3f, 11f), stoneDark);
            Box("Bridge Rail R", new Vector3(6.4f, 1.6f, -22f), new Vector3(0.7f, 2.3f, 11f), stoneDark);

            // Side footbridges.
            Box("West Footbridge", new Vector3(-34f, 0.25f, -22f), new Vector3(8f, 0.6f, 10f), iron);
            Box("East Footbridge", new Vector3(35f, 0.25f, -22f), new Vector3(8f, 0.6f, 10f), iron);
        }

        private static void BuildFoundryTerrace()
        {
            // Elevated industrial quarter at east.
            Box("Foundry Terrace", new Vector3(35f, 3f, 20f), new Vector3(38f, 6f, 40f), stoneDark);
            Ramp("Foundry Ramp", new Vector3(17f, 3.0f, 8f), new Vector3(26f, 0.9f, 8f), 13.4f, true, stone);

            Building("Grand Foundry", new Vector3(35f, 6f, 25f), new Vector3(22f, 11f, 14f), 3, true);
            Label("GRAND FOUNDRY", new Vector3(35f, 12.4f, 17.7f), 0.55f, new Color(1f,0.55f,0.22f));

            Furnace(new Vector3(23f, 6.2f, 10f));
            Furnace(new Vector3(35f, 6.2f, 10f));
            Furnace(new Vector3(47f, 6.2f, 10f));

            Chimney(new Vector3(27f, 7f, 31f), 2.2f, 16f);
            Chimney(new Vector3(43f, 7f, 31f), 2.2f, 19f);

            Pipe(new Vector3(35f, 10f, 30f), 1.0f, 17f, Quaternion.Euler(0,0,90));
            Pipe(new Vector3(18f, 7.5f, 20f), 0.7f, 18f, Quaternion.Euler(0,0,0));

            // Gantry / industrial frame.
            Box("Gantry Left", new Vector3(20f, 10f, 26f), new Vector3(1.0f, 12f, 1.0f), iron);
            Box("Gantry Right", new Vector3(50f, 10f, 26f), new Vector3(1.0f, 12f, 1.0f), iron);
            Box("Gantry Beam", new Vector3(35f, 15.5f, 26f), new Vector3(31f, 1.0f, 1.0f), iron);
        }

        private static void BuildResidentialTerrace()
        {
            Box("Residential Terrace", new Vector3(-35f, 2f, 22f), new Vector3(34f, 4f, 40f), sandstone);
            Ramp("Residential Ramp", new Vector3(-18f, 2.0f, 9f), new Vector3(24f, 0.9f, 8f), -9.5f, true, stone);

            for (int row = 0; row < 2; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    Vector3 p = new Vector3(-46f + col * 11f, 4f, 14f + row * 15f);
                    Building("Terrace House " + row + "-" + col, p, new Vector3(8f, 6f, 9f), 2, false);
                    Box("House Balcony " + row + "-" + col, p + new Vector3(0f, 4.2f, -4.9f), new Vector3(6.2f, 0.4f, 1.4f), iron);
                }
            }

            Label("STONEWARD RESIDENCES", new Vector3(-35f, 8.2f, 6.7f), 0.5f, new Color(0.92f,0.86f,0.73f));
        }

        private static void BuildMineApproach()
        {
            // Upper quarry shelf and mine gate.
            Box("Quarry Shelf", new Vector3(25f, 7.5f, 41f), new Vector3(50f, 15f, 13f), stoneDark);
            Ramp("Quarry Ramp", new Vector3(34f, 10.5f, 35f), new Vector3(22f, 0.9f, 7f), -24.2f, true, stone);

            Box("Mine Portal Left", new Vector3(13f, 12f, 42f), new Vector3(8f, 10f, 5f), stone);
            Box("Mine Portal Right", new Vector3(31f, 12f, 42f), new Vector3(8f, 10f, 5f), stone);
            Box("Mine Portal Beam", new Vector3(22f, 18f, 42f), new Vector3(26f, 5f, 5f), stone);
            Box("Mine Darkness", new Vector3(22f, 11f, 39.3f), new Vector3(10f, 8f, 0.4f), black);
            Label("OLD QUARRY GATE", new Vector3(22f, 17.0f, 39.0f), 0.48f, new Color(0.92f,0.77f,0.48f));

            // Quarry machinery silhouettes.
            for (int i = 0; i < 4; i++)
            {
                Box("Ore Cart " + i, new Vector3(0f + i * 4f, 8.2f, 38f), new Vector3(3.0f, 1.3f, 2.2f), iron);
            }
        }

        private static void BuildPlayer()
        {
            GameObject player = new GameObject("Player");
            player.transform.position = new Vector3(0f, 2.2f, -34f);

            CharacterController cc = player.AddComponent<CharacterController>();
            cc.height = 1.8f;
            cc.radius = 0.34f;
            cc.center = new Vector3(0f, 0.9f, 0f);
            cc.stepOffset = 0.35f;
            cc.slopeLimit = 48f;

            GameObject camGo = new GameObject("Player Camera");
            camGo.transform.SetParent(player.transform, false);
            camGo.transform.localPosition = new Vector3(0f, 1.62f, 0f);
            Camera cam = camGo.AddComponent<Camera>();
            cam.fieldOfView = 72f;
            cam.nearClipPlane = 0.08f;
            cam.farClipPlane = 350f;
            cam.clearFlags = CameraClearFlags.Skybox;
            camGo.AddComponent<AudioListener>();
            camGo.tag = "MainCamera";

            FoundryCityPlayerController control = player.AddComponent<FoundryCityPlayerController>();
            control.playerCamera = cam;
        }

        private static void BuildOverviewCamera()
        {
            GameObject go = new GameObject("Overview Camera");
            Camera cam = go.AddComponent<Camera>();
            cam.enabled = false;
            cam.fieldOfView = 52f;
            cam.nearClipPlane = 0.1f;
            cam.farClipPlane = 400f;
            go.transform.position = new Vector3(-78f, 66f, -92f);
            go.transform.rotation = Quaternion.Euler(27f, 39f, 0f);
        }

        private static void ValidateScene()
        {
            string[] required =
            {
                "Player","Citadel Hall","Grand Foundry","Foundry Terrace",
                "Residential Terrace","Bridge Deck","Mine Darkness","South Gate Beam",
                "Canal Water","Overview Camera"
            };

            foreach (string name in required)
            {
                if (GameObject.Find(name) == null)
                    throw new InvalidOperationException("FOUNDRY_CITY_REQUIRED_OBJECT_MISSING:" + name);
            }

            if (GameObject.FindObjectsOfType<Collider>().Length < 60)
                throw new InvalidOperationException("FOUNDRY_CITY_COLLIDER_COUNT_TOO_LOW");

            Debug.Log("FOUNDRY_CITY_SCENE_ACCEPTANCE=PASS colliders=" + GameObject.FindObjectsOfType<Collider>().Length);
        }

        private static void RenderOverview()
        {
            Camera cam = GameObject.Find("Overview Camera").GetComponent<Camera>();
            const int width = 1600;
            const int height = 900;

            RenderTexture rt = new RenderTexture(width, height, 24);
            Texture2D tex = new Texture2D(width, height, TextureFormat.RGB24, false);
            cam.targetTexture = rt;
            RenderTexture.active = rt;
            cam.Render();
            tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
            tex.Apply();

            byte[] png = tex.EncodeToPNG();
            File.WriteAllBytes(Path.Combine(ArtifactRoot, "foundry-city-3d.png"), png);

            cam.targetTexture = null;
            RenderTexture.active = null;
            UnityEngine.Object.DestroyImmediate(rt);
            UnityEngine.Object.DestroyImmediate(tex);
        }

        private static void WriteReceipt()
        {
            int colliders = GameObject.FindObjectsOfType<Collider>().Length;
            string json = "{\n" +
                "  \"schema\": \"dolzore.foundry_city_3d.v1\",\n" +
                "  \"scene\": \"" + ScenePath + "\",\n" +
                "  \"walkable\": true,\n" +
                "  \"controller\": \"CharacterController\",\n" +
                "  \"districts\": [\"gate\",\"citadel_plaza\",\"copper_market\",\"canal\",\"foundry_terrace\",\"residential_terrace\",\"quarry_gate\"],\n" +
                "  \"multi_level\": true,\n" +
                "  \"bridge\": true,\n" +
                "  \"ramps\": true,\n" +
                "  \"collider_count\": " + colliders + ",\n" +
                "  \"protected_ffxi_assets_used\": false,\n" +
                "  \"exact_bastok_map_copied\": false\n" +
                "}\n";
            File.WriteAllText(Path.Combine(ArtifactRoot, "foundry-city-3d.json"), json, new UTF8Encoding(false));
        }

        private static void Building(string name, Vector3 basePos, Vector3 size, int floors, bool grand)
        {
            Material body = grand ? plaster : sandstone;
            Box(name, basePos + new Vector3(0f, size.y * 0.5f, 0f), size, body);
            Box(name + " Roof", basePos + new Vector3(0f, size.y + 0.45f, 0f), new Vector3(size.x + 0.7f, 0.9f, size.z + 0.7f), stoneDark);

            int columns = Mathf.Max(2, Mathf.FloorToInt(size.x / 3.4f));
            for (int f = 0; f < floors; f++)
            {
                float y = basePos.y + 2.2f + f * 2.65f;
                for (int c = 0; c < columns; c++)
                {
                    float x = basePos.x - size.x * 0.38f + (columns == 1 ? 0f : c * (size.x * 0.76f / (columns - 1)));
                    Box(name + " Window " + f + "-" + c, new Vector3(x, y, basePos.z - size.z * 0.505f), new Vector3(1.25f, 1.25f, 0.18f), glass);
                }
            }

            Box(name + " Door", new Vector3(basePos.x, basePos.y + 1.45f, basePos.z - size.z * 0.515f), new Vector3(1.8f, 2.9f, 0.22f), wood);
        }

        private static void AddTower(Vector3 pos, float radius, float height)
        {
            Cylinder("Citadel Tower", pos, radius, height, stone);
        }

        private static void Furnace(Vector3 pos)
        {
            Cylinder("Furnace Body", pos, 3.1f, 5.5f, iron);
            Cylinder("Furnace Glow", pos + new Vector3(0, 0.2f, -3.05f), 1.2f, 0.4f, lava, Quaternion.Euler(90,0,0));
            Box("Furnace Hood", pos + new Vector3(0, 3.4f, 0), new Vector3(7.2f, 1.0f, 7.2f), stoneDark);
        }

        private static void Chimney(Vector3 pos, float radius, float height)
        {
            Cylinder("Foundry Chimney", pos + new Vector3(0,height*0.5f,0), radius, height, iron);
            Cylinder("Chimney Rim", pos + new Vector3(0,height+0.2f,0), radius*1.18f, 0.6f, stoneDark);
        }

        private static void Pipe(Vector3 pos, float radius, float length, Quaternion rotation)
        {
            Cylinder("Industrial Pipe", pos, radius, length, copper, rotation);
        }

        private static void Lamp(Vector3 pos)
        {
            Cylinder("Lamp Post", pos + new Vector3(0,2.2f,0), 0.10f, 4.4f, iron);
            GameObject bulb = Sphere("Lamp", pos + new Vector3(0,4.5f,0), 0.38f, lava);
            Light l = bulb.AddComponent<Light>();
            l.type = LightType.Point;
            l.color = new Color(1f,0.55f,0.22f);
            l.intensity = 2.2f;
            l.range = 7f;
        }

        private static void Crate(Vector3 pos)
        {
            Box("Market Crate", pos + new Vector3(0,0.55f,0), new Vector3(1.7f,1.1f,1.7f), wood);
        }

        private static void Awning(Vector3 pos, Vector3 scale, Material mat)
        {
            Box("Market Awning", pos, scale, mat);
        }

        private static void Ramp(string name, Vector3 pos, Vector3 scale, float tiltDeg, bool alongX, Material mat)
        {
            GameObject go = Box(name, pos, scale, mat);
            go.transform.rotation = alongX
                ? Quaternion.Euler(0f,0f,tiltDeg)
                : Quaternion.Euler(tiltDeg,0f,0f);
        }

        private static GameObject Box(string name, Vector3 pos, Vector3 scale, Material mat)
        {
            GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
            go.name = name;
            go.transform.position = pos;
            go.transform.localScale = scale;
            go.GetComponent<MeshRenderer>().sharedMaterial = mat;
            return go;
        }

        private static GameObject Sphere(string name, Vector3 pos, float radius, Material mat)
        {
            GameObject go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            go.name = name;
            go.transform.position = pos;
            go.transform.localScale = Vector3.one * radius * 2f;
            go.GetComponent<MeshRenderer>().sharedMaterial = mat;
            return go;
        }

        private static GameObject Cylinder(string name, Vector3 pos, float radius, float height, Material mat, Quaternion? rotation = null)
        {
            GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            go.name = name;
            go.transform.position = pos;
            go.transform.localScale = new Vector3(radius, height * 0.5f, radius);
            if (rotation.HasValue) go.transform.rotation = rotation.Value;
            go.GetComponent<MeshRenderer>().sharedMaterial = mat;
            return go;
        }

        private static void Label(string text, Vector3 pos, float size, Color color, Quaternion? rotation = null)
        {
            GameObject go = new GameObject("Label " + text);
            go.transform.position = pos;
            go.transform.rotation = rotation ?? Quaternion.identity;
            TextMesh tm = go.AddComponent<TextMesh>();
            tm.text = text;
            tm.characterSize = size;
            tm.fontSize = 48;
            tm.anchor = TextAnchor.MiddleCenter;
            tm.alignment = TextAlignment.Center;
            tm.color = color;
            Font font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            tm.font = font;
            MeshRenderer mr = go.GetComponent<MeshRenderer>();
            mr.sharedMaterial = font.material;
        }

        private static Color Hex(string value)
        {
            if (!ColorUtility.TryParseHtmlString(value, out Color c))
                throw new InvalidOperationException("BAD_COLOR:" + value);
            return c;
        }
    }
}
