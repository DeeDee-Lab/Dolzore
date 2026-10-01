using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

namespace Dolzore.Editor
{
    public static class FirstTown25DQualityBenchmark
    {
        private const string ScenePath = "Assets/Scenes/FirstTown25DQualityBenchmark.unity";
        private const string ArtifactDir = "BuildArtifacts";
        private const string RootArt = "Assets/Art/Generated25D/Quality";
        private const string TextureDir = RootArt + "/Textures";
        private const string MaterialDir = RootArt + "/Materials";

        private static int propCount;
        private static int qaChecks;

        [Serializable]
        private sealed class Receipt
        {
            public string status;
            public string scene;
            public string camera;
            public string qualityStage;
            public int buildings;
            public int characters;
            public int environmentalProps;
            public int staticRoadQaChecks;
            public bool proceduralSurfaceTextures;
            public bool layeredFacades;
            public bool roadIntrusionQa;
            public string note;
        }

        [MenuItem("DOLZORE/Generate FirstTown 2.5D Quality Benchmark")]
        public static void Generate()
        {
            Directory.CreateDirectory("Assets/Scenes");
            Directory.CreateDirectory(ArtifactDir);
            EnsureFolders();
            GenerateSurfaceTextures();

            propCount = 0;
            qaChecks = 0;

            Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

            QualitySettings.shadowDistance = 80f;
            QualitySettings.antiAliasing = 4;
            RenderSettings.ambientMode = AmbientMode.Trilight;
            RenderSettings.ambientSkyColor = Hex("#CFE4E5");
            RenderSettings.ambientEquatorColor = Hex("#D8D1BD");
            RenderSettings.ambientGroundColor = Hex("#6E7669");
            RenderSettings.ambientIntensity = 0.78f;
            RenderSettings.fog = true;
            RenderSettings.fogColor = Hex("#C7D9D8");
            RenderSettings.fogMode = FogMode.Linear;
            RenderSettings.fogStartDistance = 38f;
            RenderSettings.fogEndDistance = 74f;

            Material grass = TexturedMat("Q_Grass", "grass.png", 0f, 0.14f, new Vector2(8f, 8f));
            Material grassDeep = TexturedMat("Q_GrassDeep", "grass_deep.png", 0f, 0.12f, new Vector2(6f, 6f));
            Material road = TexturedMat("Q_Road", "road.png", 0f, 0.08f, new Vector2(7f, 4f));
            Material sidewalk = TexturedMat("Q_Sidewalk", "sidewalk.png", 0f, 0.10f, new Vector2(8f, 3f));
            Material stuccoCream = TexturedMat("Q_StuccoCream", "stucco_cream.png", 0f, 0.18f, new Vector2(3f, 3f));
            Material stuccoCoral = TexturedMat("Q_StuccoCoral", "stucco_coral.png", 0f, 0.18f, new Vector2(3f, 3f));
            Material brick = TexturedMat("Q_Brick", "brick.png", 0f, 0.20f, new Vector2(3f, 3f));
            Material roofRed = TexturedMat("Q_RoofRed", "roof_red.png", 0f, 0.20f, new Vector2(4f, 4f));
            Material roofBlue = TexturedMat("Q_RoofBlue", "roof_blue.png", 0f, 0.20f, new Vector2(4f, 4f));
            Material roofGreen = TexturedMat("Q_RoofGreen", "roof_green.png", 0f, 0.20f, new Vector2(4f, 4f));
            Material wood = TexturedMat("Q_Wood", "wood.png", 0f, 0.22f, new Vector2(3f, 3f));

            Material curb = FlatMat("Q_Curb", "#C8BFA9", 0f, 0.12f);
            Material crosswalk = FlatMat("Q_Crosswalk", "#F1EBD7", 0f, 0.10f);
            Material lane = FlatMat("Q_Lane", "#D3BC61", 0f, 0.08f);
            Material trimDark = FlatMat("Q_TrimDark", "#38454B", 0f, 0.22f);
            Material trimWarm = FlatMat("Q_TrimWarm", "#F0D9B1", 0f, 0.18f);
            Material glass = FlatMat("Q_Glass", "#8FC6D5", 0.05f, 0.70f);
            Material glassDark = FlatMat("Q_GlassDark", "#527B88", 0.05f, 0.65f);
            Material metal = FlatMat("Q_Metal", "#465057", 0.18f, 0.32f);
            Material copper = FlatMat("Q_Copper", "#9F674B", 0.15f, 0.28f);
            Material soil = FlatMat("Q_Soil", "#72543F", 0f, 0.06f);
            Material stone = FlatMat("Q_Stone", "#8C8E86", 0f, 0.12f);
            Material flowerYellow = FlatMat("Q_FlowerYellow", "#E6C25A", 0f, 0.12f);
            Material flowerPink = FlatMat("Q_FlowerPink", "#D98291", 0f, 0.12f);
            Material leafA = FlatMat("Q_LeafA", "#477A51", 0f, 0.10f);
            Material leafB = FlatMat("Q_LeafB", "#6B9E5D", 0f, 0.10f);
            Material trunk = FlatMat("Q_Trunk", "#6B4A39", 0f, 0.12f);
            Material skin = FlatMat("Q_Skin", "#D3A078", 0f, 0.18f);
            Material hair = FlatMat("Q_Hair", "#2E3037", 0f, 0.16f);
            Material teal = FlatMat("Q_Teal", "#4A8E92", 0f, 0.18f);
            Material burgundy = FlatMat("Q_Burgundy", "#8B5260", 0f, 0.18f);
            Material mustard = FlatMat("Q_Mustard", "#C29A4B", 0f, 0.18f);
            Material denim = FlatMat("Q_Denim", "#566E87", 0f, 0.18f);
            Material shoe = FlatMat("Q_Shoe", "#3F3A39", 0f, 0.15f);
            Material carPaint = FlatMat("Q_CarPaint", "#78A99F", 0.1f, 0.38f);

            CreateBox("Ground", new Vector3(0f, -0.55f, 2f), new Vector3(46f, 0.9f, 36f), grass);
            CreateBox("RearGarden", new Vector3(-2.5f, -0.04f, 10f), new Vector3(30f, 0.18f, 10f), grassDeep);

            // Main road and T-junction.
            CreateBox("Road_Main", new Vector3(0f, 0.01f, -5f), new Vector3(46f, 0.18f, 6.4f), road);
            CreateBox("Road_Side", new Vector3(8f, 0.02f, 2.8f), new Vector3(5.6f, 0.18f, 15.6f), road);
            CreateBox("Sidewalk_North", new Vector3(-3.5f, 0.10f, -0.65f), new Vector3(37f, 0.24f, 2.2f), sidewalk);
            CreateBox("Sidewalk_East", new Vector3(11.9f, 0.10f, 3.0f), new Vector3(2.2f, 0.24f, 15.6f), sidewalk);
            CreateBox("Curb_North", new Vector3(-3.5f, 0.22f, -1.82f), new Vector3(37f, 0.22f, 0.18f), curb);
            CreateBox("Curb_East", new Vector3(10.72f, 0.22f, 3f), new Vector3(0.18f, 0.22f, 15.6f), curb);

            for (float x = -18f; x <= 18f; x += 4.5f)
            {
                if (x > 4f && x < 12f) continue;
                CreateBox("LaneMark", new Vector3(x, 0.16f, -5f), new Vector3(1.6f, 0.05f, 0.16f), lane);
            }
            for (float z = -1f; z <= 9f; z += 3.2f)
                CreateBox("SideLaneMark", new Vector3(8f, 0.17f, z), new Vector3(0.16f, 0.05f, 1.25f), lane);

            CreateCrosswalk(new Vector3(7.9f, 0.18f, -1.9f), true, crosswalk);
            CreateCrosswalk(new Vector3(10.85f, 0.18f, -5f), false, crosswalk);

            // Authored block: three distinct buildings, each with layered facade/depth.
            CreateShop("BAR 13", new Vector3(-10.2f, 0f, 4.0f), 7.6f, 6.2f, 4.7f,
                stuccoCoral, roofRed, trimDark, trimWarm, glass, wood, true);
            CreateCafe("MOON CAFE", new Vector3(-1.7f, 0f, 4.5f), 6.2f, 5.5f, 4.2f,
                stuccoCream, roofGreen, trimDark, glass, wood, brick);
            CreateResidence("CLOCK HOUSE", new Vector3(14.8f, 0f, 6.0f), 6.8f, 6.0f, 5.7f,
                brick, roofBlue, trimWarm, trimDark, glassDark, wood);

            // Planted/inhabited exterior instead of empty grass.
            CreateTree(new Vector3(-15.6f, 0f, 9.4f), 1.15f, trunk, leafA, leafB);
            CreateTree(new Vector3(3.4f, 0f, 10.8f), 1.0f, trunk, leafA, leafB);
            CreateTree(new Vector3(18.5f, 0f, 11.0f), 0.95f, trunk, leafA, leafB);
            CreateHedgeRow(new Vector3(-15.2f, 0.65f, 1.25f), 5, Vector3.right, leafB);
            CreateHedgeRow(new Vector3(-0.2f, 0.65f, 1.15f), 4, Vector3.right, leafA);
            CreatePlanter(new Vector3(-6.7f, 0f, -0.15f), soil, stone, leafB, flowerPink);
            CreatePlanter(new Vector3(2.8f, 0f, -0.15f), soil, stone, leafA, flowerYellow);

            CreateStreetLamp(new Vector3(-13.6f, 0f, -0.1f), metal, trimWarm);
            CreateStreetLamp(new Vector3(-4.7f, 0f, -0.1f), metal, trimWarm);
            CreateStreetLamp(new Vector3(3.5f, 0f, -0.1f), metal, trimWarm);
            CreateBench(new Vector3(-5.2f, 0f, 0.1f), wood, metal);
            CreateBench(new Vector3(3.4f, 0f, 0.1f), wood, metal);
            CreateHydrant(new Vector3(-16.7f, 0f, -0.2f), burgundy, metal);
            CreateTrashCan(new Vector3(1.1f, 0f, 0.1f), metal);
            CreateBikeRack(new Vector3(3.1f, 0f, 1.1f), metal);
            CreateStormDrain(new Vector3(-14f, 0.15f, -2.05f), metal);
            CreateStormDrain(new Vector3(1.5f, 0.15f, -2.05f), metal);
            CreateManhole(new Vector3(-3.0f, 0.16f, -5.1f), metal);
            CreateCar(new Vector3(15.8f, 0.38f, -5.15f), carPaint, glassDark, shoe);

            // Character silhouettes get dedicated authored pieces instead of capsule-only figures.
            GameObject player = CreateChibi("SORA", new Vector3(-7.0f, 0f, -0.3f), teal, denim, skin, hair, shoe, true, 0);
            CreateChibi("MELO", new Vector3(-0.3f, 0f, -0.25f), burgundy, shoe, skin, hair, shoe, false, 1);
            CreateChibi("YUZU", new Vector3(4.2f, 0f, 1.25f), mustard, denim, skin, hair, shoe, false, 2);

            CreateDirectionalLight("Sun", Hex("#FFF0D8"), 1.18f, new Vector3(48f, -38f, 0f), true);
            CreateDirectionalLight("Sky Fill", Hex("#BFD9ED"), 0.24f, new Vector3(58f, 145f, 0f), false);

            GameObject camGo = new GameObject("Main Camera");
            camGo.tag = "MainCamera";
            Camera cam = camGo.AddComponent<Camera>();
            cam.orthographic = false;
            cam.fieldOfView = 27.5f;
            cam.nearClipPlane = 0.1f;
            cam.farClipPlane = 140f;
            cam.backgroundColor = Hex("#C7D9D8");
            cam.clearFlags = CameraClearFlags.SolidColor;
            cam.transform.position = new Vector3(29f, 24f, -31f);
            cam.transform.rotation = Quaternion.LookRotation(new Vector3(-1.5f, 2.4f, 3.2f) - cam.transform.position, Vector3.up);
            camGo.AddComponent<AudioListener>();

            Selection.activeGameObject = player;
            EditorSceneManager.SaveScene(scene, ScenePath);
            EditorBuildSettings.scenes = new[] { new EditorBuildSettingsScene(ScenePath, true) };

            Render(cam, Path.Combine(ArtifactDir, "first-town-25d-quality.png"), 1440, 900);

            Receipt receipt = new Receipt
            {
                status = "PASS",
                scene = ScenePath,
                camera = "fixed perspective three-quarter 2.5D",
                qualityStage = "authored one-block benchmark",
                buildings = 3,
                characters = 3,
                environmentalProps = propCount,
                staticRoadQaChecks = qaChecks,
                proceduralSurfaceTextures = true,
                layeredFacades = true,
                roadIntrusionQa = true,
                note = "Original DOLZORE quality benchmark. Uses high-level town readability/density lessons only; no protected MOTHER2 map, layout, model, texture, character or UI copied."
            };
            File.WriteAllText(Path.Combine(ArtifactDir, "first-town-25d-quality-receipt.json"), JsonUtility.ToJson(receipt, true));

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("DOLZORE_FIRST_TOWN_25D_QUALITY_BENCHMARK=PASS");
        }

        public static void BuildWebGL()
        {
            if (!File.Exists(ScenePath))
                throw new FileNotFoundException("DOLZORE_25D_QUALITY_SCENE_MISSING", ScenePath);

            string output = Path.GetFullPath("Builds/WebGL25DQuality");
            Directory.CreateDirectory(output);

            BuildPlayerOptions options = new BuildPlayerOptions
            {
                scenes = new[] { ScenePath },
                locationPathName = output,
                target = BuildTarget.WebGL,
                options = BuildOptions.None
            };

            BuildReport report = BuildPipeline.BuildPlayer(options);
            if (report.summary.result != BuildResult.Succeeded)
                throw new InvalidOperationException("DOLZORE_25D_QUALITY_WEBGL_FAILED:" + report.summary.result);

            Debug.Log("DOLZORE_FIRST_TOWN_25D_QUALITY_WEBGL=PASS");
        }

        private static void EnsureFolders()
        {
            EnsureFolder("Assets/Art");
            EnsureFolder("Assets/Art/Generated25D");
            EnsureFolder(RootArt);
            EnsureFolder(TextureDir);
            EnsureFolder(MaterialDir);
        }

        private static void EnsureFolder(string path)
        {
            if (AssetDatabase.IsValidFolder(path)) return;
            string parent = Path.GetDirectoryName(path)?.Replace("\\", "/");
            string name = Path.GetFileName(path);
            if (!string.IsNullOrEmpty(parent) && !AssetDatabase.IsValidFolder(parent))
                EnsureFolder(parent);
            AssetDatabase.CreateFolder(parent ?? "Assets", name);
        }

        private static void GenerateSurfaceTextures()
        {
            WriteNoiseTexture("grass.png", Hex("#759F63"), Hex("#95B879"), 101, TexturePattern.Organic);
            WriteNoiseTexture("grass_deep.png", Hex("#527C50"), Hex("#729663"), 203, TexturePattern.Organic);
            WriteNoiseTexture("road.png", Hex("#656B6C"), Hex("#818586"), 307, TexturePattern.Asphalt);
            WritePaverTexture("sidewalk.png", Hex("#D0C6AD"), Hex("#B6AD98"));
            WriteStuccoTexture("stucco_cream.png", Hex("#DCCCA4"), Hex("#F0E1BC"), 401);
            WriteStuccoTexture("stucco_coral.png", Hex("#C87767"), Hex("#E2937E"), 409);
            WriteBrickTexture("brick.png", Hex("#A86555"), Hex("#C17A64"), Hex("#D4B7A0"));
            WriteRoofTexture("roof_red.png", Hex("#7F4C4C"), Hex("#A3615B"));
            WriteRoofTexture("roof_blue.png", Hex("#4E6777"), Hex("#6C8997"));
            WriteRoofTexture("roof_green.png", Hex("#4D6F61"), Hex("#698D78"));
            WriteWoodTexture("wood.png", Hex("#6D503C"), Hex("#956E50"));
            AssetDatabase.Refresh();
        }

        private enum TexturePattern { Organic, Asphalt }

        private static void WriteNoiseTexture(string file, Color a, Color b, int seed, TexturePattern pattern)
        {
            const int w = 96, h = 96;
            Texture2D tex = new Texture2D(w, h, TextureFormat.RGBA32, false);
            System.Random rng = new System.Random(seed);
            Color32[] px = new Color32[w * h];
            for (int y = 0; y < h; y++)
            for (int x = 0; x < w; x++)
            {
                float n = (float)rng.NextDouble();
                float low = Mathf.PerlinNoise((x + seed) * 0.065f, (y - seed) * 0.065f);
                float t = pattern == TexturePattern.Asphalt ? 0.22f * n + 0.20f * low : 0.30f * n + 0.32f * low;
                Color c = Color.Lerp(a, b, t);
                if (pattern == TexturePattern.Asphalt && rng.NextDouble() < 0.035)
                    c *= 0.82f;
                px[y * w + x] = c;
            }
            tex.SetPixels32(px);
            tex.Apply();
            SaveTexture(file, tex);
            UnityEngine.Object.DestroyImmediate(tex);
        }

        private static void WritePaverTexture(string file, Color baseC, Color lineC)
        {
            const int w = 96, h = 96;
            Texture2D tex = new Texture2D(w, h, TextureFormat.RGBA32, false);
            Color32[] px = new Color32[w * h];
            for (int y = 0; y < h; y++)
            for (int x = 0; x < w; x++)
            {
                int row = y / 16;
                int shiftedX = x + ((row & 1) == 0 ? 0 : 12);
                bool seam = (y % 16 == 0) || (shiftedX % 24 == 0);
                float n = Mathf.PerlinNoise(x * 0.11f, y * 0.11f) * 0.08f;
                Color c = seam ? lineC : Color.Lerp(baseC, Color.white, n);
                px[y * w + x] = c;
            }
            tex.SetPixels32(px);
            tex.Apply();
            SaveTexture(file, tex);
            UnityEngine.Object.DestroyImmediate(tex);
        }

        private static void WriteStuccoTexture(string file, Color a, Color b, int seed)
        {
            const int w = 96, h = 96;
            Texture2D tex = new Texture2D(w, h, TextureFormat.RGBA32, false);
            System.Random rng = new System.Random(seed);
            Color32[] px = new Color32[w * h];
            for (int i = 0; i < px.Length; i++)
            {
                float t = 0.20f + (float)rng.NextDouble() * 0.18f;
                Color c = Color.Lerp(a, b, t);
                if (rng.NextDouble() < 0.018) c *= 0.88f;
                px[i] = c;
            }
            tex.SetPixels32(px);
            tex.Apply();
            SaveTexture(file, tex);
            UnityEngine.Object.DestroyImmediate(tex);
        }

        private static void WriteBrickTexture(string file, Color mortar, Color a, Color b)
        {
            const int w = 96, h = 96;
            Texture2D tex = new Texture2D(w, h, TextureFormat.RGBA32, false);
            Color32[] px = new Color32[w * h];
            for (int y = 0; y < h; y++)
            for (int x = 0; x < w; x++)
            {
                int row = y / 12;
                int sx = x + ((row & 1) == 0 ? 0 : 12);
                bool seam = (y % 12 <= 1) || (sx % 24 <= 1);
                float t = Mathf.PerlinNoise(x * 0.08f, y * 0.08f) * 0.45f;
                px[y * w + x] = seam ? mortar : Color.Lerp(a, b, t);
            }
            tex.SetPixels32(px);
            tex.Apply();
            SaveTexture(file, tex);
            UnityEngine.Object.DestroyImmediate(tex);
        }

        private static void WriteRoofTexture(string file, Color a, Color b)
        {
            const int w = 96, h = 96;
            Texture2D tex = new Texture2D(w, h, TextureFormat.RGBA32, false);
            Color32[] px = new Color32[w * h];
            for (int y = 0; y < h; y++)
            for (int x = 0; x < w; x++)
            {
                bool seam = (y % 12 <= 1) || ((x + ((y / 12) & 1) * 8) % 16 <= 1);
                float shade = (y % 12) / 12f * 0.15f;
                Color c = Color.Lerp(a, b, 0.28f + shade);
                if (seam) c *= 0.72f;
                px[y * w + x] = c;
            }
            tex.SetPixels32(px);
            tex.Apply();
            SaveTexture(file, tex);
            UnityEngine.Object.DestroyImmediate(tex);
        }

        private static void WriteWoodTexture(string file, Color a, Color b)
        {
            const int w = 96, h = 96;
            Texture2D tex = new Texture2D(w, h, TextureFormat.RGBA32, false);
            Color32[] px = new Color32[w * h];
            for (int y = 0; y < h; y++)
            for (int x = 0; x < w; x++)
            {
                float grain = Mathf.PerlinNoise(x * 0.18f, y * 0.035f);
                bool seam = x % 24 <= 1;
                Color c = Color.Lerp(a, b, 0.22f + grain * 0.34f);
                if (seam) c *= 0.72f;
                px[y * w + x] = c;
            }
            tex.SetPixels32(px);
            tex.Apply();
            SaveTexture(file, tex);
            UnityEngine.Object.DestroyImmediate(tex);
        }

        private static void SaveTexture(string file, Texture2D tex)
        {
            string assetPath = TextureDir + "/" + file;
            string projectRoot = Directory.GetParent(Application.dataPath).FullName;
            string abs = Path.Combine(projectRoot, assetPath.Replace('/', Path.DirectorySeparatorChar));
            Directory.CreateDirectory(Path.GetDirectoryName(abs));
            File.WriteAllBytes(abs, tex.EncodeToPNG());
            AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceUpdate);
            TextureImporter importer = AssetImporter.GetAtPath(assetPath) as TextureImporter;
            if (importer != null)
            {
                importer.wrapMode = TextureWrapMode.Repeat;
                importer.filterMode = FilterMode.Bilinear;
                importer.textureCompression = TextureImporterCompression.Uncompressed;
                importer.mipmapEnabled = true;
                importer.SaveAndReimport();
            }
        }

        private static Material TexturedMat(string name, string textureFile, float metallic, float smoothness, Vector2 tiling)
        {
            string path = MaterialDir + "/" + name + ".mat";
            Material mat = AssetDatabase.LoadAssetAtPath<Material>(path);
            Shader shader = Shader.Find("Universal Render Pipeline/Lit") ?? Shader.Find("Standard");
            if (mat == null)
            {
                mat = new Material(shader) { name = name };
                AssetDatabase.CreateAsset(mat, path);
            }
            Texture2D tex = AssetDatabase.LoadAssetAtPath<Texture2D>(TextureDir + "/" + textureFile);
            mat.color = Color.white;
            mat.mainTexture = tex;
            mat.mainTextureScale = tiling;
            if (mat.HasProperty("_BaseMap")) mat.SetTexture("_BaseMap", tex);
            if (mat.HasProperty("_Metallic")) mat.SetFloat("_Metallic", metallic);
            if (mat.HasProperty("_Smoothness")) mat.SetFloat("_Smoothness", smoothness);
            if (mat.HasProperty("_Glossiness")) mat.SetFloat("_Glossiness", smoothness);
            EditorUtility.SetDirty(mat);
            return mat;
        }

        private static Material FlatMat(string name, string html, float metallic, float smoothness)
        {
            string path = MaterialDir + "/" + name + ".mat";
            Material mat = AssetDatabase.LoadAssetAtPath<Material>(path);
            Shader shader = Shader.Find("Universal Render Pipeline/Lit") ?? Shader.Find("Standard");
            if (mat == null)
            {
                mat = new Material(shader) { name = name };
                AssetDatabase.CreateAsset(mat, path);
            }
            mat.color = Hex(html);
            if (mat.HasProperty("_BaseColor")) mat.SetColor("_BaseColor", Hex(html));
            if (mat.HasProperty("_Metallic")) mat.SetFloat("_Metallic", metallic);
            if (mat.HasProperty("_Smoothness")) mat.SetFloat("_Smoothness", smoothness);
            if (mat.HasProperty("_Glossiness")) mat.SetFloat("_Glossiness", smoothness);
            EditorUtility.SetDirty(mat);
            return mat;
        }

        private static GameObject CreateShop(string label, Vector3 p, float w, float d, float h,
            Material wall, Material roof, Material trim, Material accent, Material glass, Material wood, bool awning)
        {
            AssertOffRoad("BUILDING:" + label, p, w * 0.52f, d * 0.52f);
            GameObject root = new GameObject(label);
            root.transform.position = p;

            CreateBox("Foundation", new Vector3(0f, 0.28f, 0f), new Vector3(w + 0.35f, 0.55f, d + 0.35f), trim, root.transform);
            CreateBox("GroundFloor", new Vector3(0f, h * 0.32f, 0f), new Vector3(w, h * 0.58f, d), wall, root.transform);
            CreateBox("UpperFloor", new Vector3(0f, h * 0.72f, 0.18f), new Vector3(w * 0.96f, h * 0.34f, d * 0.94f), wall, root.transform);
            AddCornerPosts(root.transform, w, d, h, trim);

            float frontZ = -d * 0.505f;
            CreateDoor(root.transform, new Vector3(0f, 1.18f, frontZ - 0.05f), wood, trim, 1.1f, 2.35f);
            CreateWindow(root.transform, new Vector3(-w * 0.28f, 1.55f, frontZ - 0.06f), glass, trim, 1.5f, 1.45f);
            CreateWindow(root.transform, new Vector3(w * 0.28f, 1.55f, frontZ - 0.06f), glass, trim, 1.5f, 1.45f);
            CreateWindow(root.transform, new Vector3(-w * 0.25f, 3.55f, frontZ - 0.04f), glass, trim, 1.15f, 1.05f);
            CreateWindow(root.transform, new Vector3(w * 0.25f, 3.55f, frontZ - 0.04f), glass, trim, 1.15f, 1.05f);

            if (awning)
            {
                CreateBox("Awning", new Vector3(0f, 2.7f, frontZ - 0.48f), new Vector3(w * 0.72f, 0.22f, 0.9f), accent, root.transform);
                for (int i = -3; i <= 3; i += 2)
                    CreateBox("AwningStripe", new Vector3(i * w * 0.085f, 2.69f, frontZ - 0.50f), new Vector3(w * 0.075f, 0.24f, 0.94f), trim, root.transform);
            }

            CreateSignPanel(root.transform, label, new Vector3(0f, h * 0.62f, frontZ - 0.18f), new Vector3(w * 0.55f, 0.78f, 0.16f), trim, accent);
            CreateGableRoof(root.transform, w, d, h + 0.45f, roof, trim);
            CreateChimney(root.transform, new Vector3(w * 0.28f, h + 1.25f, 0.65f), trim);
            CreateSteps(root.transform, new Vector3(0f, 0.18f, frontZ - 0.8f), 1.7f, 1.15f, trim);
            return root;
        }

        private static GameObject CreateCafe(string label, Vector3 p, float w, float d, float h,
            Material wall, Material roof, Material trim, Material glass, Material wood, Material brick)
        {
            AssertOffRoad("BUILDING:" + label, p, w * 0.52f, d * 0.52f);
            GameObject root = new GameObject(label);
            root.transform.position = p;
            CreateBox("Foundation", new Vector3(0f, 0.24f, 0f), new Vector3(w + 0.25f, 0.48f, d + 0.25f), trim, root.transform);
            CreateBox("Body", new Vector3(0f, h * 0.48f, 0f), new Vector3(w, h * 0.92f, d), wall, root.transform);

            float frontZ = -d * 0.505f;
            CreateDoor(root.transform, new Vector3(w * 0.26f, 1.1f, frontZ - 0.05f), wood, trim, 1.0f, 2.2f);
            CreateWindow(root.transform, new Vector3(-w * 0.24f, 1.55f, frontZ - 0.06f), glass, trim, 2.15f, 1.55f);
            CreateBox("BrickSkirt", new Vector3(0f, 0.65f, frontZ - 0.025f), new Vector3(w * 0.95f, 1.05f, 0.12f), brick, root.transform);
            CreateWindow(root.transform, new Vector3(-w * 0.24f, 3.1f, frontZ - 0.07f), glass, trim, 1.25f, 0.95f);
            CreateWindow(root.transform, new Vector3(w * 0.24f, 3.1f, frontZ - 0.07f), glass, trim, 1.25f, 0.95f);
            AddCornerPosts(root.transform, w, d, h, trim);
            CreateGableRoof(root.transform, w, d, h + 0.35f, roof, trim);
            CreateSignPanel(root.transform, label, new Vector3(-w * 0.18f, 2.55f, frontZ - 0.15f), new Vector3(w * 0.48f, 0.58f, 0.14f), trim, wall);
            CreatePlanter(p + new Vector3(-w * 0.42f, 0f, -d * 0.58f), FlatMat("Q_SoilCafe", "#72543F", 0f, 0.06f), trim,
                FlatMat("Q_CafeLeaf", "#5D8E59", 0f, 0.1f), FlatMat("Q_CafeFlower", "#DCA46C", 0f, 0.1f), false);
            return root;
        }

        private static GameObject CreateResidence(string label, Vector3 p, float w, float d, float h,
            Material wall, Material roof, Material trim, Material dark, Material glass, Material wood)
        {
            AssertOffRoad("BUILDING:" + label, p, w * 0.52f, d * 0.52f);
            GameObject root = new GameObject(label);
            root.transform.position = p;

            CreateBox("Foundation", new Vector3(0f, 0.26f, 0f), new Vector3(w + 0.3f, 0.52f, d + 0.3f), dark, root.transform);
            CreateBox("Body", new Vector3(0f, h * 0.49f, 0f), new Vector3(w, h * 0.94f, d), wall, root.transform);
            AddCornerPosts(root.transform, w, d, h, trim);
            float frontZ = -d * 0.505f;
            CreateDoor(root.transform, new Vector3(w * 0.22f, 1.2f, frontZ - 0.05f), wood, dark, 1.05f, 2.3f);
            CreateWindow(root.transform, new Vector3(-w * 0.24f, 1.7f, frontZ - 0.06f), glass, trim, 1.45f, 1.35f);
            CreateWindow(root.transform, new Vector3(-w * 0.24f, 3.75f, frontZ - 0.06f), glass, trim, 1.25f, 1.1f);
            CreateWindow(root.transform, new Vector3(w * 0.24f, 3.75f, frontZ - 0.06f), glass, trim, 1.25f, 1.1f);
            CreateBox("PorchRoof", new Vector3(w * 0.22f, 2.75f, frontZ - 0.75f), new Vector3(2.6f, 0.22f, 1.45f), roof, root.transform);
            CreateBox("PorchPostL", new Vector3(w * 0.22f - 1.0f, 1.35f, frontZ - 1.1f), new Vector3(0.16f, 2.7f, 0.16f), trim, root.transform);
            CreateBox("PorchPostR", new Vector3(w * 0.22f + 1.0f, 1.35f, frontZ - 1.1f), new Vector3(0.16f, 2.7f, 0.16f), trim, root.transform);
            CreateSteps(root.transform, new Vector3(w * 0.22f, 0.18f, frontZ - 1.25f), 2.4f, 1.4f, dark);
            CreateGableRoof(root.transform, w, d, h + 0.48f, roof, dark);
            CreateChimney(root.transform, new Vector3(-w * 0.25f, h + 1.25f, 0.9f), dark);
            return root;
        }

        private static void AddCornerPosts(Transform parent, float w, float d, float h, Material mat)
        {
            float x = w * 0.5f + 0.03f;
            float z = d * 0.5f + 0.03f;
            for (int sx = -1; sx <= 1; sx += 2)
            for (int sz = -1; sz <= 1; sz += 2)
                CreateBox("CornerPost", new Vector3(sx * x, h * 0.48f, sz * z), new Vector3(0.18f, h * 0.96f, 0.18f), mat, parent);
            CreateBox("FasciaFront", new Vector3(0f, h * 0.98f, -z), new Vector3(w + 0.18f, 0.22f, 0.18f), mat, parent);
        }

        private static void CreateDoor(Transform parent, Vector3 p, Material door, Material frame, float w, float h)
        {
            CreateBox("DoorFrame", p, new Vector3(w + 0.22f, h + 0.22f, 0.18f), frame, parent);
            CreateBox("Door", p + new Vector3(0f, -0.03f, -0.08f), new Vector3(w, h, 0.16f), door, parent);
            CreateSphere("Knob", p + new Vector3(w * 0.30f, 0f, -0.18f), Vector3.one * 0.10f, frame, parent);
        }

        private static void CreateWindow(Transform parent, Vector3 p, Material glass, Material frame, float w, float h)
        {
            CreateBox("WindowFrame", p, new Vector3(w + 0.24f, h + 0.24f, 0.16f), frame, parent);
            CreateBox("Glass", p + new Vector3(0f, 0f, -0.09f), new Vector3(w, h, 0.12f), glass, parent);
            CreateBox("MullionV", p + new Vector3(0f, 0f, -0.17f), new Vector3(0.08f, h, 0.08f), frame, parent);
            CreateBox("MullionH", p + new Vector3(0f, 0f, -0.17f), new Vector3(w, 0.08f, 0.08f), frame, parent);
            CreateBox("Sill", p + new Vector3(0f, -h * 0.55f, -0.12f), new Vector3(w + 0.35f, 0.12f, 0.28f), frame, parent);
        }

        private static void CreateGableRoof(Transform parent, float w, float d, float baseY, Material roof, Material ridge)
        {
            GameObject left = CreateBox("RoofLeft", new Vector3(-w * 0.24f, baseY, 0f), new Vector3(w * 0.58f, 0.36f, d * 1.18f), roof, parent);
            left.transform.localRotation = Quaternion.Euler(0f, 0f, 23f);
            GameObject right = CreateBox("RoofRight", new Vector3(w * 0.24f, baseY, 0f), new Vector3(w * 0.58f, 0.36f, d * 1.18f), roof, parent);
            right.transform.localRotation = Quaternion.Euler(0f, 0f, -23f);
            CreateBox("Ridge", new Vector3(0f, baseY + 0.62f, 0f), new Vector3(0.24f, 0.24f, d * 1.20f), ridge, parent);
        }

        private static void CreateChimney(Transform parent, Vector3 p, Material mat)
        {
            CreateBox("Chimney", p, new Vector3(0.72f, 1.45f, 0.72f), mat, parent);
            CreateBox("ChimneyCap", p + new Vector3(0f, 0.78f, 0f), new Vector3(0.94f, 0.16f, 0.94f), mat, parent);
        }

        private static void CreateSteps(Transform parent, Vector3 p, float w, float d, Material mat)
        {
            CreateBox("Step1", p, new Vector3(w, 0.22f, d), mat, parent);
            CreateBox("Step2", p + new Vector3(0f, 0.18f, 0.34f), new Vector3(w * 0.82f, 0.22f, d * 0.62f), mat, parent);
        }

        private static void CreateSignPanel(Transform parent, string label, Vector3 p, Vector3 size, Material frame, Material face)
        {
            CreateBox("SignFrame", p, size + new Vector3(0.20f, 0.20f, 0.06f), frame, parent);
            CreateBox("SignFace", p + new Vector3(0f, 0f, -0.08f), size, face, parent);
            GameObject textGo = new GameObject("SignText_" + label.Replace(" ", "_"));
            textGo.transform.SetParent(parent, false);
            textGo.transform.localPosition = p + new Vector3(0f, 0f, -0.18f);
            textGo.transform.localRotation = Quaternion.Euler(0f, 180f, 0f);
            TextMesh tm = textGo.AddComponent<TextMesh>();
            tm.text = label;
            tm.anchor = TextAnchor.MiddleCenter;
            tm.alignment = TextAlignment.Center;
            tm.characterSize = 0.115f;
            tm.fontSize = 44;
            tm.fontStyle = FontStyle.Bold;
            tm.color = Hex("#F4E8CD");
        }

        private static void CreateCrosswalk(Vector3 center, bool acrossX, Material mat)
        {
            for (int i = -4; i <= 4; i++)
            {
                float offset = i * 0.56f;
                Vector3 p = acrossX ? center + new Vector3(offset, 0f, 0f) : center + new Vector3(0f, 0f, offset);
                Vector3 s = acrossX ? new Vector3(0.30f, 0.05f, 2.55f) : new Vector3(2.55f, 0.05f, 0.30f);
                CreateBox("Crosswalk", p, s, mat);
            }
        }

        private static void CreateTree(Vector3 p, float scale, Material trunk, Material a, Material b)
        {
            AssertOffRoad("TREE", p, 1.15f * scale, 1.15f * scale);
            qaChecks++;
            GameObject root = new GameObject("Tree");
            root.transform.position = p;
            GameObject stem = CreateCylinder("Trunk", new Vector3(0f, 1.15f * scale, 0f), new Vector3(0.38f, 1.15f, 0.38f) * scale, trunk, root.transform);
            stem.transform.localRotation = Quaternion.Euler(0f, 14f, 0f);
            CreateSphere("CrownA", new Vector3(0f, 2.8f * scale, 0f), new Vector3(1.8f, 1.45f, 1.65f) * scale, a, root.transform);
            CreateSphere("CrownB", new Vector3(-0.72f * scale, 3.0f * scale, 0.2f), new Vector3(1.15f, 1.05f, 1.1f) * scale, b, root.transform);
            CreateSphere("CrownC", new Vector3(0.62f * scale, 3.15f * scale, -0.15f), new Vector3(1.05f, 0.95f, 1.0f) * scale, b, root.transform);
            propCount++;
        }

        private static void CreateHedgeRow(Vector3 start, int count, Vector3 step, Material mat)
        {
            for (int i = 0; i < count; i++)
            {
                Vector3 p = start + step * i * 1.15f;
                AssertOffRoad("HEDGE", p, 0.62f, 0.48f);
                qaChecks++;
                CreateSphere("Hedge", p, new Vector3(1.15f, 0.85f, 0.82f), mat);
                propCount++;
            }
        }

        private static void CreatePlanter(Vector3 p, Material soil, Material stone, Material leaf, Material flower, bool enforceQa = true)
        {
            if (enforceQa)
            {
                AssertOffRoad("PLANTER", p, 0.95f, 0.55f);
                qaChecks++;
            }
            GameObject root = new GameObject("Planter");
            root.transform.position = p;
            CreateBox("Box", new Vector3(0f, 0.28f, 0f), new Vector3(1.8f, 0.55f, 0.95f), stone, root.transform);
            CreateBox("Soil", new Vector3(0f, 0.59f, 0f), new Vector3(1.55f, 0.12f, 0.72f), soil, root.transform);
            for (int i = -2; i <= 2; i++)
            {
                float x = i * 0.30f;
                CreateSphere("Leaf", new Vector3(x, 0.86f, 0f), Vector3.one * 0.42f, leaf, root.transform);
                if ((i & 1) == 0)
                    CreateSphere("Flower", new Vector3(x, 1.11f, 0f), Vector3.one * 0.17f, flower, root.transform);
            }
            propCount++;
        }

        private static void CreateStreetLamp(Vector3 p, Material metal, Material glow)
        {
            AssertOffRoad("LAMP", p, 0.35f, 0.35f);
            qaChecks++;
            GameObject root = new GameObject("StreetLamp");
            root.transform.position = p;
            CreateCylinder("Pole", new Vector3(0f, 1.75f, 0f), new Vector3(0.12f, 1.75f, 0.12f), metal, root.transform);
            CreateBox("Arm", new Vector3(0.34f, 3.22f, 0f), new Vector3(0.75f, 0.10f, 0.10f), metal, root.transform);
            CreateBox("Lamp", new Vector3(0.72f, 3.02f, 0f), new Vector3(0.55f, 0.40f, 0.42f), glow, root.transform);
            propCount++;
        }

        private static void CreateBench(Vector3 p, Material wood, Material metal)
        {
            AssertOffRoad("BENCH", p, 1.2f, 0.55f);
            qaChecks++;
            GameObject root = new GameObject("Bench");
            root.transform.position = p;
            CreateBox("Seat", new Vector3(0f, 0.58f, 0f), new Vector3(2.25f, 0.20f, 0.62f), wood, root.transform);
            CreateBox("Back", new Vector3(0f, 1.08f, 0.25f), new Vector3(2.25f, 0.65f, 0.18f), wood, root.transform);
            CreateBox("LegL", new Vector3(-0.78f, 0.30f, 0f), new Vector3(0.15f, 0.58f, 0.45f), metal, root.transform);
            CreateBox("LegR", new Vector3(0.78f, 0.30f, 0f), new Vector3(0.15f, 0.58f, 0.45f), metal, root.transform);
            propCount++;
        }

        private static void CreateHydrant(Vector3 p, Material body, Material metal)
        {
            AssertOffRoad("HYDRANT", p, 0.4f, 0.4f);
            qaChecks++;
            GameObject root = new GameObject("Hydrant");
            root.transform.position = p;
            CreateCylinder("Body", new Vector3(0f, 0.45f, 0f), new Vector3(0.34f, 0.45f, 0.34f), body, root.transform);
            CreateCylinder("Cap", new Vector3(0f, 0.92f, 0f), new Vector3(0.43f, 0.12f, 0.43f), metal, root.transform);
            CreateCylinder("Side", new Vector3(0.38f, 0.54f, 0f), new Vector3(0.15f, 0.20f, 0.15f), metal, root.transform).transform.localRotation = Quaternion.Euler(0f, 0f, 90f);
            propCount++;
        }

        private static void CreateTrashCan(Vector3 p, Material metal)
        {
            AssertOffRoad("TRASH_CAN", p, 0.42f, 0.42f);
            qaChecks++;
            GameObject root = new GameObject("TrashCan");
            root.transform.position = p;
            CreateCylinder("Can", new Vector3(0f, 0.55f, 0f), new Vector3(0.38f, 0.55f, 0.38f), metal, root.transform);
            CreateCylinder("Lid", new Vector3(0f, 1.10f, 0f), new Vector3(0.44f, 0.08f, 0.44f), metal, root.transform);
            propCount++;
        }

        private static void CreateBikeRack(Vector3 p, Material metal)
        {
            AssertOffRoad("BIKE_RACK", p, 0.9f, 0.45f);
            qaChecks++;
            GameObject root = new GameObject("BikeRack");
            root.transform.position = p;
            for (int i = -1; i <= 1; i++)
            {
                float x = i * 0.62f;
                CreateBox("RackPost", new Vector3(x, 0.45f, 0f), new Vector3(0.09f, 0.90f, 0.09f), metal, root.transform);
                CreateBox("RackTop", new Vector3(x, 0.85f, 0f), new Vector3(0.34f, 0.09f, 0.09f), metal, root.transform);
            }
            propCount++;
        }

        private static void CreateStormDrain(Vector3 p, Material metal)
        {
            GameObject root = new GameObject("StormDrain");
            root.transform.position = p;
            for (int i = -3; i <= 3; i++)
                CreateBox("Slot", new Vector3(i * 0.12f, 0f, 0f), new Vector3(0.06f, 0.03f, 0.62f), metal, root.transform);
            propCount++;
        }

        private static void CreateManhole(Vector3 p, Material metal)
        {
            GameObject root = new GameObject("Manhole");
            root.transform.position = p;
            GameObject disc = CreateCylinder("Cover", Vector3.zero, new Vector3(0.72f, 0.035f, 0.72f), metal, root.transform);
            disc.transform.localRotation = Quaternion.identity;
            propCount++;
        }

        private static void CreateCar(Vector3 p, Material paint, Material glass, Material tire)
        {
            GameObject root = new GameObject("ParkedCar");
            root.transform.position = p;
            CreateBox("Body", new Vector3(0f, 0.45f, 0f), new Vector3(3.0f, 0.78f, 1.45f), paint, root.transform);
            CreateBox("Cabin", new Vector3(0.18f, 1.05f, 0f), new Vector3(1.55f, 0.72f, 1.16f), glass, root.transform);
            for (int x = -1; x <= 1; x += 2)
            for (int z = -1; z <= 1; z += 2)
            {
                GameObject wheel = CreateCylinder("Wheel", new Vector3(x * 0.95f, 0.24f, z * 0.68f), new Vector3(0.28f, 0.16f, 0.28f), tire, root.transform);
                wheel.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
            }
            propCount++;
        }

        private static GameObject CreateChibi(string name, Vector3 p, Material top, Material bottom, Material skin, Material hair, Material shoe, bool player, int hairStyle)
        {
            GameObject root = new GameObject("Character_" + name);
            root.transform.position = p;
            GameObject visual = new GameObject("Visual");
            visual.transform.SetParent(root.transform, false);

            CreateBox("Torso", new Vector3(0f, 1.16f, 0f), new Vector3(0.76f, 0.88f, 0.50f), top, visual.transform);
            CreateBox("JacketHem", new Vector3(0f, 0.76f, 0f), new Vector3(0.82f, 0.22f, 0.54f), top, visual.transform);
            CreateBox("LegL", new Vector3(-0.20f, 0.40f, 0f), new Vector3(0.28f, 0.62f, 0.34f), bottom, visual.transform);
            CreateBox("LegR", new Vector3(0.20f, 0.40f, 0f), new Vector3(0.28f, 0.62f, 0.34f), bottom, visual.transform);
            CreateBox("ShoeL", new Vector3(-0.20f, 0.11f, -0.08f), new Vector3(0.34f, 0.20f, 0.48f), shoe, visual.transform);
            CreateBox("ShoeR", new Vector3(0.20f, 0.11f, -0.08f), new Vector3(0.34f, 0.20f, 0.48f), shoe, visual.transform);
            CreateBox("ArmL", new Vector3(-0.49f, 1.12f, 0f), new Vector3(0.22f, 0.68f, 0.26f), top, visual.transform);
            CreateBox("ArmR", new Vector3(0.49f, 1.12f, 0f), new Vector3(0.22f, 0.68f, 0.26f), top, visual.transform);
            CreateSphere("HandL", new Vector3(-0.49f, 0.72f, 0f), Vector3.one * 0.22f, skin, visual.transform);
            CreateSphere("HandR", new Vector3(0.49f, 0.72f, 0f), Vector3.one * 0.22f, skin, visual.transform);

            CreateSphere("Head", new Vector3(0f, 2.05f, 0f), new Vector3(0.82f, 0.90f, 0.80f), skin, visual.transform);
            CreateSphere("HairCap", new Vector3(0f, 2.36f, 0.04f), new Vector3(0.88f, 0.48f, 0.82f), hair, visual.transform);
            if (hairStyle == 1)
            {
                CreateSphere("HairSideL", new Vector3(-0.42f, 2.22f, 0.03f), new Vector3(0.34f, 0.58f, 0.42f), hair, visual.transform);
                CreateSphere("HairSideR", new Vector3(0.42f, 2.18f, 0.03f), new Vector3(0.22f, 0.42f, 0.34f), hair, visual.transform);
            }
            else if (hairStyle == 2)
            {
                CreateSphere("HairBack", new Vector3(0f, 2.17f, 0.30f), new Vector3(0.74f, 0.70f, 0.35f), hair, visual.transform);
            }
            else
            {
                CreateBox("HairTuft", new Vector3(0.22f, 2.68f, -0.02f), new Vector3(0.30f, 0.28f, 0.30f), hair, visual.transform).transform.localRotation = Quaternion.Euler(0f, 0f, -18f);
            }

            Material eye = FlatMat("Q_Eye", "#25262B", 0f, 0.1f);
            CreateSphere("EyeL", new Vector3(-0.22f, 2.05f, -0.73f), Vector3.one * 0.085f, eye, visual.transform);
            CreateSphere("EyeR", new Vector3(0.22f, 2.05f, -0.73f), Vector3.one * 0.085f, eye, visual.transform);

            if (player)
            {
                CharacterController cc = root.AddComponent<CharacterController>();
                cc.height = 2.75f;
                cc.radius = 0.52f;
                cc.center = new Vector3(0f, 1.30f, 0f);
                Dolzore.Prototype25DPlayerController ctl = root.AddComponent<Dolzore.Prototype25DPlayerController>();
                SerializedObject so = new SerializedObject(ctl);
                so.FindProperty("visualRoot").objectReferenceValue = visual.transform;
                so.ApplyModifiedPropertiesWithoutUndo();
            }

            propCount++;
            return root;
        }

        private static void CreateDirectionalLight(string name, Color color, float intensity, Vector3 euler, bool shadow)
        {
            GameObject go = new GameObject(name);
            Light light = go.AddComponent<Light>();
            light.type = LightType.Directional;
            light.color = color;
            light.intensity = intensity;
            light.shadows = shadow ? LightShadows.Soft : LightShadows.None;
            light.shadowStrength = shadow ? 0.58f : 0f;
            go.transform.eulerAngles = euler;
        }

        private static void AssertOffRoad(string label, Vector3 center, float halfX, float halfZ)
        {
            // Main road: x [-23,23], z [-8.2,-1.8].
            bool main = Overlap(center.x - halfX, center.x + halfX, -23f, 23f) &&
                        Overlap(center.z - halfZ, center.z + halfZ, -8.2f, -1.8f);
            // Side road: x [5.2,10.8], z [-5.2,10.6].
            bool side = Overlap(center.x - halfX, center.x + halfX, 5.2f, 10.8f) &&
                        Overlap(center.z - halfZ, center.z + halfZ, -5.2f, 10.6f);

            if (main || side)
                throw new InvalidOperationException("DOLZORE_25D_QUALITY_STATIC_OBJECT_ON_ROAD:" + label +
                    ":center=(" + center.x.ToString("0.00") + "," + center.z.ToString("0.00") + ")" +
                    ":half=(" + halfX.ToString("0.00") + "," + halfZ.ToString("0.00") + ")");
        }

        private static bool Overlap(float a0, float a1, float b0, float b1)
        {
            return a0 < b1 && a1 > b0;
        }

        private static GameObject CreateBox(string name, Vector3 p, Vector3 s, Material mat, Transform parent = null)
        {
            GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
            go.name = name;
            if (parent != null)
            {
                go.transform.SetParent(parent, false);
                go.transform.localPosition = p;
            }
            else go.transform.position = p;
            go.transform.localScale = s;
            Renderer r = go.GetComponent<Renderer>();
            r.sharedMaterial = mat;
            r.shadowCastingMode = ShadowCastingMode.On;
            r.receiveShadows = true;
            return go;
        }

        private static GameObject CreateSphere(string name, Vector3 p, Vector3 s, Material mat, Transform parent = null)
        {
            GameObject go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            go.name = name;
            if (parent != null)
            {
                go.transform.SetParent(parent, false);
                go.transform.localPosition = p;
            }
            else go.transform.position = p;
            go.transform.localScale = s;
            go.GetComponent<Renderer>().sharedMaterial = mat;
            return go;
        }

        private static GameObject CreateCylinder(string name, Vector3 p, Vector3 s, Material mat, Transform parent = null)
        {
            GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            go.name = name;
            if (parent != null)
            {
                go.transform.SetParent(parent, false);
                go.transform.localPosition = p;
            }
            else go.transform.position = p;
            go.transform.localScale = s;
            go.GetComponent<Renderer>().sharedMaterial = mat;
            return go;
        }

        private static Color Hex(string html)
        {
            Color c;
            if (!ColorUtility.TryParseHtmlString(html, out c)) throw new ArgumentException("Invalid color: " + html);
            return c;
        }

        private static void Render(Camera camera, string path, int width, int height)
        {
            RenderTexture rt = new RenderTexture(width, height, 24, RenderTextureFormat.ARGB32);
            rt.antiAliasing = 4;
            Texture2D image = new Texture2D(width, height, TextureFormat.RGB24, false);
            RenderTexture old = RenderTexture.active;
            camera.targetTexture = rt;
            RenderTexture.active = rt;
            camera.Render();
            image.ReadPixels(new Rect(0, 0, width, height), 0, 0);
            image.Apply();
            File.WriteAllBytes(path, image.EncodeToPNG());
            camera.targetTexture = null;
            RenderTexture.active = old;
            UnityEngine.Object.DestroyImmediate(image);
            UnityEngine.Object.DestroyImmediate(rt);
        }
    }
}
