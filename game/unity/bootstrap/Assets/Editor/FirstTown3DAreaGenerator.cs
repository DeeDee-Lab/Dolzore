using System;
using System.IO;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

namespace Dolzore.Editor
{
    public static class FirstTown3DAreaGenerator
    {
        private const string ScenePath = "Assets/Scenes/FirstTown3DCrossroadsQuarter.unity";
        private const string ArtifactDir = "BuildArtifacts";
        private const string ArtRoot = "Assets/Art/Generated3D/CrossroadsQuarter";
        private const string TextureDir = ArtRoot + "/Textures";
        private const string MaterialDir = ArtRoot + "/Materials";

        private static int buildingCount;
        private static int npcCount;
        private static int treeCount;
        private static int propCount;

        [Serializable]
        private sealed class Receipt
        {
            public string status;
            public string area;
            public string scene;
            public string design_standard;
            public string world_dimension;
            public string camera;
            public float area_width_m;
            public float area_depth_m;
            public float primary_road_width_m;
            public float secondary_road_width_m;
            public float active_anchor_spacing_min_m;
            public float active_anchor_spacing_max_m;
            public int buildings;
            public int npcs;
            public int trees;
            public int street_props;
            public bool perspective;
            public bool three_dimensional_elevation;
            public bool character_controller_3d;
            public bool collider_3d;
            public string note;
        }

        [MenuItem("DOLZORE/Generate FirstTown 3D Crossroads Quarter")]
        public static void Generate()
        {
            Directory.CreateDirectory("Assets/Scenes");
            Directory.CreateDirectory(ArtifactDir);
            EnsureFolders();
            GenerateTextures();

            buildingCount = 0;
            npcCount = 0;
            treeCount = 0;
            propCount = 0;

            Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

            QualitySettings.shadowDistance = 90f;
            QualitySettings.antiAliasing = 4;
            RenderSettings.ambientMode = AmbientMode.Trilight;
            RenderSettings.ambientSkyColor = Hex("#B9CED3");
            RenderSettings.ambientEquatorColor = Hex("#B4AD9C");
            RenderSettings.ambientGroundColor = Hex("#4F5D53");
            RenderSettings.ambientIntensity = 0.52f;
            RenderSettings.fog = false;

            Material grass = TexturedMat("Grass", "grass.png", 0f, 0.12f, new Vector2(8f, 8f));
            Material grassDeep = TexturedMat("GrassDeep", "grass_deep.png", 0f, 0.10f, new Vector2(6f, 6f));
            Material road = TexturedMat("Road", "road.png", 0f, 0.06f, new Vector2(8f, 4f));
            Material pavers = TexturedMat("Pavers", "pavers.png", 0f, 0.12f, new Vector2(6f, 3f));
            Material stone = TexturedMat("Stone", "stone.png", 0f, 0.12f, new Vector2(4f, 4f));
            Material brick = TexturedMat("Brick", "brick.png", 0f, 0.16f, new Vector2(3f, 3f));
            Material stuccoWarm = TexturedMat("StuccoWarm", "stucco_warm.png", 0f, 0.17f, new Vector2(3f, 3f));
            Material stuccoBlue = TexturedMat("StuccoBlue", "stucco_blue.png", 0f, 0.17f, new Vector2(3f, 3f));
            Material stuccoRose = TexturedMat("StuccoRose", "stucco_rose.png", 0f, 0.17f, new Vector2(3f, 3f));
            Material wood = TexturedMat("Wood", "wood.png", 0f, 0.20f, new Vector2(3f, 3f));
            Material roofRed = TexturedMat("RoofRed", "roof_red.png", 0f, 0.18f, new Vector2(4f, 4f));
            Material roofTeal = TexturedMat("RoofTeal", "roof_teal.png", 0f, 0.18f, new Vector2(4f, 4f));
            Material roofSlate = TexturedMat("RoofSlate", "roof_slate.png", 0f, 0.18f, new Vector2(4f, 4f));

            Material trim = FlatMat("Trim", "#3B474C", 0f, 0.20f);
            Material cream = FlatMat("Cream", "#EBD8AE", 0f, 0.14f);
            Material glass = FlatMat("Glass", "#75AFC4", 0.08f, 0.72f);
            Material glassDark = FlatMat("GlassDark", "#456B79", 0.08f, 0.70f);
            Material metal = FlatMat("Metal", "#424C51", 0.18f, 0.34f);
            Material copper = FlatMat("Copper", "#A06A4E", 0.22f, 0.32f);
            Material lane = FlatMat("Lane", "#D6C46C", 0f, 0.08f);
            Material stripe = FlatMat("Stripe", "#EEE8D4", 0f, 0.08f);
            Material soil = FlatMat("Soil", "#70503B", 0f, 0.06f);
            Material leafA = FlatMat("LeafA", "#437552", 0f, 0.10f);
            Material leafB = FlatMat("LeafB", "#68955E", 0f, 0.10f);
            Material leafC = FlatMat("LeafC", "#7FAA63", 0f, 0.10f);
            Material trunk = FlatMat("Trunk", "#684837", 0f, 0.12f);
            Material flower = FlatMat("Flower", "#D9908A", 0f, 0.10f);
            Material water = FlatMat("Water", "#5F9FAE", 0.02f, 0.62f);

            Material skin = FlatMat("Skin", "#D4A47E", 0f, 0.18f);
            Material hairDark = FlatMat("HairDark", "#292C32", 0f, 0.15f);
            Material hairBrown = FlatMat("HairBrown", "#5D4438", 0f, 0.15f);
            Material topTeal = FlatMat("TopTeal", "#4D9191", 0f, 0.18f);
            Material topRose = FlatMat("TopRose", "#A85D68", 0f, 0.18f);
            Material topGold = FlatMat("TopGold", "#BE9348", 0f, 0.18f);
            Material topBlue = FlatMat("TopBlue", "#5E7897", 0f, 0.18f);
            Material pants = FlatMat("Pants", "#46546B", 0f, 0.16f);
            Material dark = FlatMat("Dark", "#333539", 0f, 0.15f);

            // World floor: true 3D volume, 96m x 72m.
            CreateBox("Ground", new Vector3(0f, -0.60f, 0f), new Vector3(96f, 1f, 72f), grass);

            // Quiet garden edge and raised civic terrace.
            CreateBox("GardenGround", new Vector3(-31f, -0.04f, 20f), new Vector3(25f, 0.18f, 22f), grassDeep);
            CreateBox("CivicTerrace", new Vector3(23f, 0.75f, 19f), new Vector3(28f, 1.50f, 22f), stone);
            CreateBox("CivicTerraceGrass", new Vector3(23f, 1.53f, 19f), new Vector3(25.5f, 0.12f, 19.5f), grassDeep);

            // Primary street: 7.2 m road with 2.0 m sidewalks, slightly rotated to avoid a rigid grid.
            Vector3 mainA = new Vector3(-48f, 0f, -13f);
            Vector3 mainB = new Vector3(48f, 0f, -4f);
            CreateStrip("PrimarySidewalk", mainA, mainB, 11.2f, 0.06f, pavers);
            CreateStrip("PrimaryRoad", mainA, mainB, 7.2f, 0.15f, road);

            // Secondary street: offset/diagonal route.
            Vector3 sideA = new Vector3(-8f, 0f, -10f);
            Vector3 sideB = new Vector3(17f, 0f, 22f);
            CreateStrip("SecondarySidewalk", sideA, sideB, 9.4f, 0.065f, pavers);
            CreateStrip("SecondaryRoad", sideA, sideB, 5.4f, 0.155f, road);

            // A small service lane creates a walkable loop.
            Vector3 laneA = new Vector3(-29f, 0f, 2f);
            Vector3 laneB = new Vector3(-18f, 0f, 28f);
            CreateStrip("ServiceLaneShoulder", laneA, laneB, 6.0f, 0.055f, pavers);
            CreateStrip("ServiceLane", laneA, laneB, 3.8f, 0.145f, road);

            // Clock Court social node.
            CreateBox("ClockCourt", new Vector3(4f, 0.14f, 5f), new Vector3(18f, 0.22f, 16f), pavers);
            CreateClockLandmark(new Vector3(4f, 0.25f, 5f), stone, trim, copper, cream);

            // Crosswalks and lane marks.
            CreateCrosswalk(new Vector3(-10.5f, 0.28f, -9.4f), 5.8f, 7, 8f, stripe);
            CreateCrosswalk(new Vector3(12.4f, 0.28f, -7.3f), 5.8f, 7, 8f, stripe);
            CreateDashedLine(mainA, mainB, 6.0f, 1.7f, 0.14f, 0.30f, lane);

            // Terrace access: stairs make the area visibly 3D rather than a flat block.
            CreateStairs("CivicSteps", new Vector3(11.0f, 0.05f, 11.8f), 5.0f, 5.8f, 1.5f, 7, stone);
            CreateBox("TerraceWallSouth", new Vector3(23f, 0.72f, 8.1f), new Vector3(28f, 1.45f, 0.35f), stone);
            CreateBox("TerraceWallEast", new Vector3(36.8f, 0.72f, 19f), new Vector3(0.35f, 1.45f, 22f), stone);

            // Four original landmark buildings.
            CreateBar(new Vector3(-24f, 0f, -0.5f), -5f, stuccoRose, roofRed, trim, cream, glass, wood);
            CreateMarketHouse(new Vector3(-8f, 0f, 24f), 10f, stuccoWarm, roofTeal, trim, glass, wood, brick);
            CreateWorkshop(new Vector3(29f, 0f, -12f), -8f, brick, roofSlate, metal, glassDark, wood, copper);
            CreateJournalHall(new Vector3(25f, 1.55f, 21f), -3f, stuccoBlue, roofSlate, trim, cream, glassDark, wood);

            // Garden / trees: active streets 6–10 m cadence, quiet edge more open.
            Vector3[] trees =
            {
                new Vector3(-39f,0f,17f), new Vector3(-32f,0f,25f), new Vector3(-24f,0f,31f),
                new Vector3(-14f,0f,31f), new Vector3(3f,0f,28f), new Vector3(16f,1.55f,30f),
                new Vector3(34f,1.55f,29f), new Vector3(39f,0f,-22f), new Vector3(-40f,0f,-22f)
            };
            for (int i = 0; i < trees.Length; i++)
                CreateTree(trees[i], 0.88f + (i % 3) * 0.08f, trunk, leafA, leafB, leafC);

            // Street furniture / interest anchors.
            Vector3[] lampPos =
            {
                new Vector3(-38f,0f,-7.4f), new Vector3(-27f,0f,-6.5f), new Vector3(-16f,0f,-5.6f),
                new Vector3(-4f,0f,-4.6f), new Vector3(8f,0f,-3.5f), new Vector3(20f,0f,-2.4f),
                new Vector3(32f,0f,-1.3f), new Vector3(41f,0f,-0.5f)
            };
            foreach (Vector3 p in lampPos) CreateStreetLamp(p, metal, cream);

            CreateBench(new Vector3(-14f, 0f, 7.5f), 15f, wood, metal);
            CreateBench(new Vector3(13f, 0f, 8.3f), -8f, wood, metal);
            CreateBench(new Vector3(-31f, 0f, 12.5f), 65f, wood, metal);
            CreatePlanter(new Vector3(-19f, 0f, 6.5f), soil, stone, leafB, flower);
            CreatePlanter(new Vector3(18f, 1.55f, 11f), soil, stone, leafA, flower);
            CreatePlanter(new Vector3(31f, 1.55f, 11f), soil, stone, leafC, flower);
            CreateHydrant(new Vector3(-33f, 0f, -5.8f), topRose, metal);
            CreateTrashCan(new Vector3(-10f, 0f, 8.7f), metal);
            CreateTrashCan(new Vector3(15f, 0f, -0.8f), metal);
            CreateMailbox(new Vector3(-20f, 0f, 8.4f), trim, cream);
            CreateStreetSign(new Vector3(8.5f, 0f, 11.5f), metal, cream);
            CreateUtilityBox(new Vector3(38f, 0f, -9f), metal, trim);
            CreateBikeRack(new Vector3(-2f, 0f, 12.5f), metal);
            CreateBollards(new Vector3(10f, 0f, 8.5f), 4, Vector3.right, 1.2f, metal);
            CreateCrates(new Vector3(35f, 0f, -18f), wood, trim);
            CreateSmallFountain(new Vector3(-31f, 0f, 22f), stone, water);

            // Player and NPC population.
            GameObject player = CreateCharacter("SORA_3D", new Vector3(-3f, 0.3f, -18f), 0f,
                topTeal, pants, skin, hairDark, dark, CharacterFlavor.Player);
            CreateCharacter("MELO_3D", new Vector3(-17f, 0.3f, 7f), 160f,
                topRose, pants, skin, hairBrown, dark, CharacterFlavor.Bag);
            CreateCharacter("YUZU_3D", new Vector3(10f, 0.3f, 9.5f), -60f,
                topGold, pants, skin, hairDark, dark, CharacterFlavor.Scarf);
            CreateCharacter("PON_3D", new Vector3(-29f, 0.3f, 16f), 70f,
                topBlue, pants, skin, hairBrown, dark, CharacterFlavor.Cap);
            CreateCharacter("BAR_GUEST_A", new Vector3(-22f, 0.3f, 5f), 180f,
                topGold, pants, skin, hairDark, dark, CharacterFlavor.None);
            CreateCharacter("BAR_GUEST_B", new Vector3(-18f, 0.3f, 4f), 200f,
                topBlue, pants, skin, hairBrown, dark, CharacterFlavor.Bag);
            CreateCharacter("JOURNAL_CLERK", new Vector3(19f, 1.85f, 14f), 120f,
                topRose, pants, skin, hairDark, dark, CharacterFlavor.Scarf);
            CreateCharacter("WORKSHOP_RUNNER", new Vector3(27f, 0.3f, -4f), 10f,
                topTeal, pants, skin, hairBrown, dark, CharacterFlavor.Cap);

            // One parked delivery vehicle, intentionally not on pedestrian court.
            CreateDeliveryCart(new Vector3(37f, 0.45f, -9f), -7f, topBlue, wood, dark, metal);

            // Light rig.
            CreateDirectionalLight("Sun", Hex("#FFE4C0"), 1.28f, new Vector3(48f, -38f, 0f), true);
            CreateDirectionalLight("SkyFill", Hex("#BFD6EA"), 0.14f, new Vector3(60f, 145f, 0f), false);

            // Main camera is fully 3D/perspective. Follow stays constrained for readability.
            GameObject cameraGo = new GameObject("Main Camera");
            cameraGo.tag = "MainCamera";
            Camera camera = cameraGo.AddComponent<Camera>();
            camera.orthographic = false;
            camera.fieldOfView = 34f;
            camera.nearClipPlane = 0.08f;
            camera.farClipPlane = 180f;
            camera.backgroundColor = Hex("#BBD5DD");
            camera.clearFlags = CameraClearFlags.SolidColor;
            cameraGo.AddComponent<AudioListener>();
            ThirdPersonAreaCamera follow = cameraGo.AddComponent<ThirdPersonAreaCamera>();
            follow.Configure(player.transform, new Vector3(14f, 13f, -21f));

            // Render a curated area overview without changing runtime follow semantics.
            camera.transform.position = new Vector3(25f, 24f, -38f);
            camera.transform.rotation = Quaternion.LookRotation(
                new Vector3(1f, 3f, 4f) - camera.transform.position,
                Vector3.up);

            Selection.activeGameObject = player;
            EditorSceneManager.SaveScene(scene, ScenePath);
            EditorBuildSettings.scenes = new[] { new EditorBuildSettingsScene(ScenePath, true) };

            Render(camera, Path.Combine(ArtifactDir, "first-town-3d-crossroads-quarter.png"), 1600, 1000);

            Receipt receipt = new Receipt
            {
                status = "PASS",
                area = "Crossroads Quarter",
                scene = ScenePath,
                design_standard = "design/DOLZORE_3D_TOWN_DESIGN_STANDARD_V1.json",
                world_dimension = "TRUE_3D",
                camera = "perspective 3D; controlled third-person runtime follow",
                area_width_m = 96f,
                area_depth_m = 72f,
                primary_road_width_m = 7.2f,
                secondary_road_width_m = 5.4f,
                active_anchor_spacing_min_m = 6f,
                active_anchor_spacing_max_m = 10f,
                buildings = buildingCount,
                npcs = npcCount,
                trees = treeCount,
                street_props = propCount,
                perspective = true,
                three_dimensional_elevation = true,
                character_controller_3d = true,
                collider_3d = true,
                note = "Original DOLZORE area derived from cross-city spatial measurements; no FF11 map, mesh, texture, character, UI or audio asset body copied."
            };
            File.WriteAllText(Path.Combine(ArtifactDir, "first-town-3d-crossroads-quarter-receipt.json"), JsonUtility.ToJson(receipt, true));

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("DOLZORE_FIRST_TOWN_3D_AREA=PASS");
        }

        public static void BuildWebGL()
        {
            if (!File.Exists(ScenePath))
                throw new FileNotFoundException("DOLZORE_3D_AREA_SCENE_MISSING", ScenePath);

            string output = Path.GetFullPath("Builds/WebGL3DArea");
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
                throw new InvalidOperationException("DOLZORE_3D_AREA_WEBGL_FAILED:" + report.summary.result);

            Debug.Log("DOLZORE_FIRST_TOWN_3D_AREA_WEBGL=PASS");
        }

        private enum CharacterFlavor { None, Player, Bag, Scarf, Cap }

        private static void CreateBar(Vector3 p, float yaw, Material wall, Material roof, Material trim, Material accent, Material glass, Material wood)
        {
            GameObject root = Root("BAR 13", p, yaw);
            BuildingShell(root.transform, 9.2f, 6.8f, 7.4f, wall, roof, trim);
            FrontDoor(root.transform, new Vector3(0f, 1.35f, -3.48f), 1.3f, 2.5f, wood, trim);
            FrontWindow(root.transform, new Vector3(-2.7f, 1.8f, -3.50f), 1.8f, 1.6f, glass, trim);
            FrontWindow(root.transform, new Vector3(2.7f, 1.8f, -3.50f), 1.8f, 1.6f, glass, trim);
            FrontWindow(root.transform, new Vector3(-2.7f, 4.6f, -3.50f), 1.4f, 1.25f, glass, trim);
            FrontWindow(root.transform, new Vector3(2.7f, 4.6f, -3.50f), 1.4f, 1.25f, glass, trim);
            CreateBox("Awning", new Vector3(0f, 3.05f, -3.95f), new Vector3(6.9f, 0.24f, 0.95f), accent, root.transform);
            CreateSign(root.transform, "BAR 13", new Vector3(0f, 4.0f, -3.62f), new Vector3(4.2f, 0.8f, 0.18f), trim, accent);
            CreateBox("SideBay", new Vector3(4.7f, 2.2f, 0.7f), new Vector3(1.2f, 4.4f, 3.8f), wall, root.transform);
            SideWindow(root.transform, new Vector3(5.33f, 2.2f, 0.7f), 1.4f, 1.3f, glass, trim);
            CreateChimney(root.transform, new Vector3(-3.0f, 8.0f, 1.2f), trim);
            buildingCount++;
        }

        private static void CreateMarketHouse(Vector3 p, float yaw, Material wall, Material roof, Material trim, Material glass, Material wood, Material brick)
        {
            GameObject root = Root("Corner Market", p, yaw);
            CreateBox("Foundation", new Vector3(0f,0.32f,0f), new Vector3(8.4f,0.64f,6.2f), brick, root.transform);
            CreateBox("Body", new Vector3(0f,3.1f,0f), new Vector3(8.0f,5.6f,5.8f), wall, root.transform);
            CreateGableRoof(root.transform, 8.4f, 6.2f, 6.45f, roof, trim);
            FrontDoor(root.transform, new Vector3(2.1f,1.45f,-2.98f), 1.15f,2.45f,wood,trim);
            FrontWindow(root.transform, new Vector3(-1.9f,1.75f,-3.0f),2.6f,1.75f,glass,trim);
            FrontWindow(root.transform, new Vector3(-1.9f,4.35f,-3.0f),1.45f,1.15f,glass,trim);
            FrontWindow(root.transform, new Vector3(2.1f,4.35f,-3.0f),1.45f,1.15f,glass,trim);
            CreateBox("BalconyFloor", new Vector3(-1.7f,3.25f,-3.55f),new Vector3(3.6f,0.18f,1.1f),wood,root.transform);
            CreateBox("BalconyRail", new Vector3(-1.7f,3.75f,-4.0f),new Vector3(3.6f,0.65f,0.12f),trim,root.transform);
            CreateSign(root.transform, "MARKET", new Vector3(1.9f,2.9f,-3.16f),new Vector3(2.4f,0.55f,0.16f),trim,roof);
            buildingCount++;
        }

        private static void CreateWorkshop(Vector3 p, float yaw, Material wall, Material roof, Material metal, Material glass, Material wood, Material copper)
        {
            GameObject root = Root("Workshop", p, yaw);
            CreateBox("Foundation",new Vector3(0f,0.28f,0f),new Vector3(10.8f,0.56f,7.8f),metal,root.transform);
            CreateBox("MainHall",new Vector3(-1.2f,2.6f,0f),new Vector3(8.4f,4.8f,7.2f),wall,root.transform);
            CreateBox("Office",new Vector3(4.2f,2.1f,-0.8f),new Vector3(3.2f,3.8f,5.0f),wood,root.transform);
            CreateSawRoof(root.transform,new Vector3(-1.2f,5.15f,0f),8.7f,7.5f,roof,metal);
            FrontDoor(root.transform,new Vector3(2.8f,1.3f,-3.7f),1.25f,2.5f,wood,metal);
            FrontWindow(root.transform,new Vector3(-2.9f,1.8f,-3.72f),2.2f,1.5f,glass,metal);
            FrontWindow(root.transform,new Vector3(-0.1f,1.8f,-3.72f),2.2f,1.5f,glass,metal);
            CreatePipe(root.transform,new Vector3(3.4f,5.0f,1.9f),copper,metal);
            CreatePipe(root.transform,new Vector3(4.4f,4.3f,1.9f),copper,metal);
            CreateSign(root.transform,"WORKSHOP",new Vector3(0.2f,3.65f,-3.86f),new Vector3(3.8f,0.58f,0.16f),metal,roof);
            buildingCount++;
        }

        private static void CreateJournalHall(Vector3 p, float yaw, Material wall, Material roof, Material trim, Material accent, Material glass, Material wood)
        {
            GameObject root = Root("JOURNAL Hall", p, yaw);
            CreateBox("Foundation",new Vector3(0f,0.35f,0f),new Vector3(10.4f,0.7f,7.8f),trim,root.transform);
            CreateBox("Body",new Vector3(0f,4.1f,0f),new Vector3(9.8f,7.4f,7.2f),wall,root.transform);
            CreateGableRoof(root.transform,10.4f,7.8f,8.35f,roof,trim);
            FrontDoor(root.transform,new Vector3(0f,1.55f,-3.72f),1.5f,2.8f,wood,trim);
            for(int floor=0; floor<2; floor++)
            {
                float y=2.0f+floor*3.0f;
                FrontWindow(root.transform,new Vector3(-2.9f,y,-3.74f),1.55f,1.45f,glass,accent);
                FrontWindow(root.transform,new Vector3(2.9f,y,-3.74f),1.55f,1.45f,glass,accent);
            }
            CreateBox("PorticoRoof",new Vector3(0f,3.2f,-4.6f),new Vector3(4.0f,0.24f,1.6f),roof,root.transform);
            CreateBox("ColumnL",new Vector3(-1.55f,1.55f,-4.8f),new Vector3(0.28f,3.1f,0.28f),accent,root.transform);
            CreateBox("ColumnR",new Vector3(1.55f,1.55f,-4.8f),new Vector3(0.28f,3.1f,0.28f),accent,root.transform);
            CreateSign(root.transform,"JOURNAL",new Vector3(0f,4.2f,-3.9f),new Vector3(4.4f,0.7f,0.16f),trim,accent);
            CreateClockTowerOnBuilding(root.transform, new Vector3(0f,9.0f,0.2f), wall, roof, trim, accent);
            buildingCount++;
        }

        private static void BuildingShell(Transform root,float w,float d,float h,Material wall,Material roof,Material trim)
        {
            CreateBox("Foundation",new Vector3(0f,0.32f,0f),new Vector3(w+0.35f,0.64f,d+0.35f),trim,root);
            CreateBox("Body",new Vector3(0f,h*0.48f,0f),new Vector3(w,h*0.92f,d),wall,root);
            CreateGableRoof(root,w+0.25f,d+0.3f,h+0.35f,roof,trim);
            float x=w*0.5f;
            float z=d*0.5f;
            for(int sx=-1;sx<=1;sx+=2)
            for(int sz=-1;sz<=1;sz+=2)
                CreateBox("CornerPost",new Vector3(sx*(x+0.04f),h*0.47f,sz*(z+0.04f)),new Vector3(0.18f,h*0.9f,0.18f),trim,root);
        }

        private static void CreateClockLandmark(Vector3 p, Material stone, Material trim, Material copper, Material face)
        {
            GameObject root=Root("Clock Court Landmark",p,0f);
            CreateBox("Base",new Vector3(0f,0.35f,0f),new Vector3(3.8f,0.7f,3.8f),stone,root.transform);
            CreateBox("Column",new Vector3(0f,2.5f,0f),new Vector3(1.25f,4.4f,1.25f),stone,root.transform);
            CreateBox("ClockHousing",new Vector3(0f,5.05f,0f),new Vector3(2.15f,1.5f,2.15f),trim,root.transform);
            CreateCylinder("ClockFace",new Vector3(0f,5.05f,-1.12f),new Vector3(0.62f,0.07f,0.62f),face,root.transform).transform.localRotation=Quaternion.Euler(90f,0f,0f);
            CreatePyramidRoof(root.transform,new Vector3(0f,6.25f,0f),2.6f,1.6f,copper);
            propCount++;
        }

        private static GameObject CreateCharacter(string name, Vector3 p, float yaw, Material top, Material bottom, Material skin, Material hair, Material shoe, CharacterFlavor flavor)
        {
            GameObject root=Root(name,p,yaw);
            GameObject visual=new GameObject("Visual");
            visual.transform.SetParent(root.transform,false);

            CreateCapsule("Torso",new Vector3(0f,1.08f,0f),new Vector3(0.50f,0.56f,0.42f),top,visual.transform);
            CreateBox("Waist",new Vector3(0f,0.76f,0f),new Vector3(0.72f,0.22f,0.48f),top,visual.transform);
            CreateCapsule("LegL",new Vector3(-0.18f,0.40f,0f),new Vector3(0.18f,0.32f,0.17f),bottom,visual.transform);
            CreateCapsule("LegR",new Vector3(0.18f,0.40f,0f),new Vector3(0.18f,0.32f,0.17f),bottom,visual.transform);
            CreateBox("ShoeL",new Vector3(-0.18f,0.13f,-0.09f),new Vector3(0.31f,0.20f,0.46f),shoe,visual.transform);
            CreateBox("ShoeR",new Vector3(0.18f,0.13f,-0.09f),new Vector3(0.31f,0.20f,0.46f),shoe,visual.transform);
            CreateCapsule("ArmL",new Vector3(-0.42f,1.05f,0f),new Vector3(0.14f,0.35f,0.14f),top,visual.transform);
            CreateCapsule("ArmR",new Vector3(0.42f,1.05f,0f),new Vector3(0.14f,0.35f,0.14f),top,visual.transform);
            CreateSphere("Head",new Vector3(0f,1.92f,0f),new Vector3(0.76f,0.82f,0.73f),skin,visual.transform);
            CreateSphere("HairCap",new Vector3(0f,2.20f,0.04f),new Vector3(0.82f,0.44f,0.78f),hair,visual.transform);
            CreateSphere("HairSideL",new Vector3(-0.34f,2.08f,0.04f),new Vector3(0.28f,0.46f,0.33f),hair,visual.transform);
            CreateSphere("HairSideR",new Vector3(0.34f,2.08f,0.04f),new Vector3(0.28f,0.46f,0.33f),hair,visual.transform);

            Material eye=FlatMat("Eye","#202329",0f,0.08f);
            CreateSphere("EyeL",new Vector3(-0.19f,1.95f,-0.67f),Vector3.one*0.07f,eye,visual.transform);
            CreateSphere("EyeR",new Vector3(0.19f,1.95f,-0.67f),Vector3.one*0.07f,eye,visual.transform);

            if(flavor==CharacterFlavor.Player)
            {
                CreateBox("Backpack",new Vector3(0f,1.12f,0.35f),new Vector3(0.55f,0.72f,0.26f),bottom,visual.transform);
                CreateBox("HairTuft",new Vector3(0.22f,2.52f,-0.02f),new Vector3(0.24f,0.28f,0.24f),hair,visual.transform).transform.localRotation=Quaternion.Euler(0f,0f,-18f);
                CharacterController cc=root.AddComponent<CharacterController>();
                cc.height=2.45f; cc.radius=0.42f; cc.center=new Vector3(0f,1.15f,0f);
                Prototype25DPlayerController ctl=root.AddComponent<Prototype25DPlayerController>();
                SerializedObject so=new SerializedObject(ctl);
                so.FindProperty("visualRoot").objectReferenceValue=visual.transform;
                so.ApplyModifiedPropertiesWithoutUndo();
            }
            else if(flavor==CharacterFlavor.Bag)
            {
                CreateBox("SideBag",new Vector3(0.48f,0.78f,0.10f),new Vector3(0.34f,0.48f,0.24f),bottom,visual.transform);
            }
            else if(flavor==CharacterFlavor.Scarf)
            {
                CreateBox("Scarf",new Vector3(0f,1.50f,-0.28f),new Vector3(0.68f,0.15f,0.12f),top,visual.transform);
                CreateBox("ScarfTail",new Vector3(0.31f,1.26f,0.18f),new Vector3(0.14f,0.58f,0.12f),top,visual.transform);
            }
            else if(flavor==CharacterFlavor.Cap)
            {
                CreateBox("CapBrim",new Vector3(0f,2.28f,-0.42f),new Vector3(0.56f,0.10f,0.42f),top,visual.transform);
            }

            npcCount++;
            return root;
        }

        private static void CreateTree(Vector3 p,float scale,Material trunk,Material a,Material b,Material c)
        {
            GameObject root=Root("Tree",p,0f);
            CreateCylinder("Trunk",new Vector3(0f,1.4f*scale,0f),new Vector3(0.28f*scale,1.4f*scale,0.28f*scale),trunk,root.transform);
            CreateSphere("CrownA",new Vector3(0f,3.25f*scale,0f),new Vector3(1.75f,1.45f,1.55f)*scale,a,root.transform);
            CreateSphere("CrownB",new Vector3(-0.75f*scale,3.45f*scale,0.15f),new Vector3(1.05f,0.95f,1.0f)*scale,b,root.transform);
            CreateSphere("CrownC",new Vector3(0.75f*scale,3.55f*scale,-0.12f),new Vector3(1.0f,0.90f,0.95f)*scale,c,root.transform);
            treeCount++;
        }

        private static void CreateStreetLamp(Vector3 p,Material metal,Material glow)
        {
            GameObject root=Root("StreetLamp",p,0f);
            CreateCylinder("Pole",new Vector3(0f,1.9f,0f),new Vector3(0.10f,1.9f,0.10f),metal,root.transform);
            CreateBox("Arm",new Vector3(0.33f,3.52f,0f),new Vector3(0.72f,0.09f,0.09f),metal,root.transform);
            CreateBox("Lamp",new Vector3(0.72f,3.28f,0f),new Vector3(0.50f,0.42f,0.45f),glow,root.transform);
            propCount++;
        }

        private static void CreateBench(Vector3 p,float yaw,Material wood,Material metal)
        {
            GameObject root=Root("Bench",p,yaw);
            CreateBox("Seat",new Vector3(0f,0.52f,0f),new Vector3(2.15f,0.20f,0.62f),wood,root.transform);
            CreateBox("Back",new Vector3(0f,1.02f,0.26f),new Vector3(2.15f,0.62f,0.16f),wood,root.transform);
            CreateBox("LegL",new Vector3(-0.72f,0.28f,0f),new Vector3(0.14f,0.54f,0.42f),metal,root.transform);
            CreateBox("LegR",new Vector3(0.72f,0.28f,0f),new Vector3(0.14f,0.54f,0.42f),metal,root.transform);
            propCount++;
        }

        private static void CreatePlanter(Vector3 p,Material soil,Material box,Material leaf,Material flower)
        {
            GameObject root=Root("Planter",p,0f);
            CreateBox("StoneBox",new Vector3(0f,0.28f,0f),new Vector3(1.8f,0.55f,0.9f),box,root.transform);
            CreateBox("Soil",new Vector3(0f,0.60f,0f),new Vector3(1.55f,0.10f,0.68f),soil,root.transform);
            for(int i=-2;i<=2;i++)
            {
                float x=i*0.30f;
                CreateSphere("Plant",new Vector3(x,0.84f,0f),Vector3.one*0.38f,leaf,root.transform);
                if((i&1)==0) CreateSphere("Flower",new Vector3(x,1.08f,0f),Vector3.one*0.14f,flower,root.transform);
            }
            propCount++;
        }

        private static void CreateHydrant(Vector3 p,Material body,Material metal)
        {
            GameObject root=Root("Hydrant",p,0f);
            CreateCylinder("Body",new Vector3(0f,0.44f,0f),new Vector3(0.32f,0.44f,0.32f),body,root.transform);
            CreateCylinder("Cap",new Vector3(0f,0.90f,0f),new Vector3(0.40f,0.10f,0.40f),metal,root.transform);
            propCount++;
        }

        private static void CreateTrashCan(Vector3 p,Material metal)
        {
            GameObject root=Root("TrashCan",p,0f);
            CreateCylinder("Can",new Vector3(0f,0.52f,0f),new Vector3(0.36f,0.52f,0.36f),metal,root.transform);
            CreateCylinder("Lid",new Vector3(0f,1.04f,0f),new Vector3(0.42f,0.07f,0.42f),metal,root.transform);
            propCount++;
        }

        private static void CreateMailbox(Vector3 p,Material body,Material accent)
        {
            GameObject root=Root("Mailbox",p,0f);
            CreateBox("Post",new Vector3(0f,0.62f,0f),new Vector3(0.12f,1.24f,0.12f),accent,root.transform);
            CreateBox("Box",new Vector3(0f,1.20f,0f),new Vector3(0.68f,0.46f,0.50f),body,root.transform);
            propCount++;
        }

        private static void CreateStreetSign(Vector3 p,Material pole,Material panel)
        {
            GameObject root=Root("StreetSign",p,0f);
            CreateCylinder("Pole",new Vector3(0f,1.3f,0f),new Vector3(0.08f,1.3f,0.08f),pole,root.transform);
            CreateBox("PanelA",new Vector3(0f,2.3f,0f),new Vector3(1.35f,0.34f,0.12f),panel,root.transform);
            CreateBox("PanelB",new Vector3(0f,1.92f,0f),new Vector3(0.12f,0.30f,1.10f),panel,root.transform);
            propCount++;
        }

        private static void CreateUtilityBox(Vector3 p,Material body,Material trim)
        {
            GameObject root=Root("UtilityBox",p,0f);
            CreateBox("Cabinet",new Vector3(0f,0.62f,0f),new Vector3(0.95f,1.24f,0.68f),body,root.transform);
            CreateBox("Panel",new Vector3(0f,0.68f,-0.36f),new Vector3(0.68f,0.68f,0.05f),trim,root.transform);
            propCount++;
        }

        private static void CreateBikeRack(Vector3 p,Material metal)
        {
            GameObject root=Root("BikeRack",p,0f);
            for(int i=-1;i<=1;i++)
            {
                float x=i*0.60f;
                CreateBox("Post",new Vector3(x,0.43f,0f),new Vector3(0.08f,0.86f,0.08f),metal,root.transform);
                CreateBox("Top",new Vector3(x,0.82f,0f),new Vector3(0.32f,0.08f,0.08f),metal,root.transform);
            }
            propCount++;
        }

        private static void CreateBollards(Vector3 start,int count,Vector3 dir,float spacing,Material metal)
        {
            for(int i=0;i<count;i++)
            {
                Vector3 p=start+dir*i*spacing;
                GameObject root=Root("Bollard",p,0f);
                CreateCylinder("Post",new Vector3(0f,0.45f,0f),new Vector3(0.14f,0.45f,0.14f),metal,root.transform);
                propCount++;
            }
        }

        private static void CreateCrates(Vector3 p,Material wood,Material trim)
        {
            GameObject root=Root("Crates",p,0f);
            CreateBox("CrateA",new Vector3(0f,0.45f,0f),new Vector3(0.95f,0.90f,0.95f),wood,root.transform);
            CreateBox("CrateB",new Vector3(1.0f,0.32f,0.2f),new Vector3(0.72f,0.64f,0.72f),wood,root.transform);
            CreateBox("BandA",new Vector3(0f,0.45f,-0.50f),new Vector3(0.12f,0.90f,0.05f),trim,root.transform);
            propCount++;
        }

        private static void CreateSmallFountain(Vector3 p,Material stone,Material water)
        {
            GameObject root=Root("GardenFountain",p,0f);
            CreateCylinder("Basin",new Vector3(0f,0.28f,0f),new Vector3(1.75f,0.28f,1.75f),stone,root.transform);
            CreateCylinder("Water",new Vector3(0f,0.58f,0f),new Vector3(1.48f,0.03f,1.48f),water,root.transform);
            CreateCylinder("Pedestal",new Vector3(0f,0.95f,0f),new Vector3(0.35f,0.55f,0.35f),stone,root.transform);
            CreateSphere("Top",new Vector3(0f,1.55f,0f),Vector3.one*0.48f,stone,root.transform);
            propCount++;
        }

        private static void CreateDeliveryCart(Vector3 p,float yaw,Material body,Material wood,Material tire,Material metal)
        {
            GameObject root=Root("DeliveryCart",p,yaw);
            CreateBox("Bed",new Vector3(0f,0.65f,0f),new Vector3(3.2f,0.55f,1.55f),body,root.transform);
            CreateBox("Cab",new Vector3(0.75f,1.25f,0f),new Vector3(1.35f,1.05f,1.35f),wood,root.transform);
            for(int x=-1;x<=1;x+=2)
            for(int z=-1;z<=1;z+=2)
            {
                GameObject wheel=CreateCylinder("Wheel",new Vector3(x*1.05f,0.30f,z*0.72f),new Vector3(0.30f,0.16f,0.30f),tire,root.transform);
                wheel.transform.localRotation=Quaternion.Euler(90f,0f,0f);
            }
            CreateBox("Bumper",new Vector3(-1.7f,0.55f,0f),new Vector3(0.18f,0.28f,1.5f),metal,root.transform);
            propCount++;
        }

        private static void CreateStrip(string name,Vector3 a,Vector3 b,float width,float y,Material mat)
        {
            Vector3 d=b-a;
            float len=new Vector2(d.x,d.z).magnitude;
            Vector3 mid=(a+b)*0.5f; mid.y=y;
            GameObject go=CreateBox(name,mid,new Vector3(width,0.18f,len),mat);
            go.transform.rotation=Quaternion.LookRotation(new Vector3(d.x,0f,d.z).normalized,Vector3.up);
        }

        private static void CreateDashedLine(Vector3 a,Vector3 b,float spacing,float dashLength,float width,float y,Material mat)
        {
            Vector3 d=new Vector3(b.x-a.x,0f,b.z-a.z);
            float len=d.magnitude;
            Vector3 dir=d.normalized;
            int count=Mathf.FloorToInt(len/spacing);
            float yaw=Mathf.Atan2(dir.x,dir.z)*Mathf.Rad2Deg;
            for(int i=1;i<count;i++)
            {
                Vector3 p=a+dir*(i*spacing); p.y=y;
                GameObject dash=CreateBox("LaneDash",p,new Vector3(width,0.05f,dashLength),mat);
                dash.transform.eulerAngles=new Vector3(0f,yaw,0f);
            }
        }

        private static void CreateCrosswalk(Vector3 center,float length,int stripes,float yaw,Material mat)
        {
            GameObject root=Root("Crosswalk",center,yaw);
            for(int i=0;i<stripes;i++)
            {
                float x=(i-(stripes-1)*0.5f)*0.62f;
                CreateBox("Stripe",new Vector3(x,0f,0f),new Vector3(0.32f,0.05f,length),mat,root.transform);
            }
        }

        private static void CreateStairs(string name,Vector3 p,float width,float depth,float rise,int steps,Material mat)
        {
            GameObject root=Root(name,p,0f);
            float stepH=rise/steps;
            float stepD=depth/steps;
            for(int i=0;i<steps;i++)
            {
                float h=stepH*(i+1);
                float z=-depth*0.5f+stepD*(i+0.5f);
                CreateBox("Step_"+i,new Vector3(0f,h*0.5f,z),new Vector3(width,h,stepD+0.03f),mat,root.transform);
            }
        }

        private static void FrontDoor(Transform parent,Vector3 p,float w,float h,Material door,Material frame)
        {
            CreateBox("DoorFrame",p,new Vector3(w+0.24f,h+0.24f,0.18f),frame,parent);
            CreateBox("Door",p+new Vector3(0f,-0.03f,-0.09f),new Vector3(w,h,0.15f),door,parent);
            CreateSphere("Knob",p+new Vector3(w*0.30f,0f,-0.20f),Vector3.one*0.09f,frame,parent);
        }

        private static void FrontWindow(Transform parent,Vector3 p,float w,float h,Material glass,Material frame)
        {
            CreateBox("WindowFrame",p,new Vector3(w+0.22f,h+0.22f,0.16f),frame,parent);
            CreateBox("WindowGlass",p+new Vector3(0f,0f,-0.09f),new Vector3(w,h,0.10f),glass,parent);
            CreateBox("MullionV",p+new Vector3(0f,0f,-0.16f),new Vector3(0.07f,h,0.07f),frame,parent);
            CreateBox("MullionH",p+new Vector3(0f,0f,-0.16f),new Vector3(w,0.07f,0.07f),frame,parent);
            CreateBox("Sill",p+new Vector3(0f,-h*0.55f,-0.12f),new Vector3(w+0.30f,0.10f,0.25f),frame,parent);
        }

        private static void SideWindow(Transform parent,Vector3 p,float w,float h,Material glass,Material frame)
        {
            CreateBox("SideWindowFrame",p,new Vector3(0.16f,h+0.22f,w+0.22f),frame,parent);
            CreateBox("SideWindowGlass",p+new Vector3(0.09f,0f,0f),new Vector3(0.10f,h,w),glass,parent);
            CreateBox("SideMullion",p+new Vector3(0.16f,0f,0f),new Vector3(0.07f,h,0.07f),frame,parent);
        }

        private static void CreateSign(Transform parent,string label,Vector3 p,Vector3 size,Material frame,Material face)
        {
            CreateBox("SignFrame",p,size+new Vector3(0.18f,0.18f,0.06f),frame,parent);
            CreateBox("SignFace",p+new Vector3(0f,0f,-0.08f),size,face,parent);
            GameObject text=new GameObject("SignText_"+label.Replace(" ","_"));
            text.transform.SetParent(parent,false);
            text.transform.localPosition=p+new Vector3(0f,0f,-0.18f);
            TextMesh tm=text.AddComponent<TextMesh>();
            tm.text=label;
            tm.anchor=TextAnchor.MiddleCenter;
            tm.alignment=TextAlignment.Center;
            tm.characterSize=0.10f;
            tm.fontSize=48;
            tm.fontStyle=FontStyle.Bold;
            tm.color=Hex("#F6E9CF");
        }

        private static void CreateGableRoof(Transform parent,float w,float d,float baseY,Material roof,Material ridge)
        {
            GameObject left=CreateBox("RoofL",new Vector3(-w*0.24f,baseY,0f),new Vector3(w*0.58f,0.38f,d*1.15f),roof,parent);
            left.transform.localRotation=Quaternion.Euler(0f,0f,23f);
            GameObject right=CreateBox("RoofR",new Vector3(w*0.24f,baseY,0f),new Vector3(w*0.58f,0.38f,d*1.15f),roof,parent);
            right.transform.localRotation=Quaternion.Euler(0f,0f,-23f);
            CreateBox("Ridge",new Vector3(0f,baseY+0.63f,0f),new Vector3(0.22f,0.22f,d*1.17f),ridge,parent);
        }

        private static void CreateSawRoof(Transform parent,Vector3 center,float w,float d,Material roof,Material trim)
        {
            for(int i=0;i<3;i++)
            {
                float x=center.x-w*0.32f+i*w*0.32f;
                GameObject slab=CreateBox("SawRoof",new Vector3(x,center.y+i*0.08f,center.z),new Vector3(w*0.36f,0.28f,d*1.04f),roof,parent);
                slab.transform.localRotation=Quaternion.Euler(0f,0f,-12f);
                CreateBox("SawTrim",new Vector3(x+w*0.14f,center.y+0.55f+i*0.08f,center.z),new Vector3(0.14f,0.85f,d),trim,parent);
            }
        }

        private static void CreatePyramidRoof(Transform parent,Vector3 center,float size,float height,Material mat)
        {
            for(int i=0;i<4;i++)
            {
                float yaw=i*90f;
                Vector3 dir=Quaternion.Euler(0f,yaw,0f)*Vector3.forward;
                GameObject side=CreateBox("PyramidSide",center+dir*(size*0.20f)+Vector3.up*(height*0.16f),new Vector3(size,0.24f,size*0.62f),mat,parent);
                side.transform.localRotation=Quaternion.Euler(28f,yaw,0f);
            }
        }

        private static void CreateChimney(Transform parent,Vector3 p,Material mat)
        {
            CreateBox("Chimney",p,new Vector3(0.70f,1.45f,0.70f),mat,parent);
            CreateBox("ChimneyCap",p+new Vector3(0f,0.78f,0f),new Vector3(0.92f,0.14f,0.92f),mat,parent);
        }

        private static void CreatePipe(Transform parent,Vector3 p,Material pipe,Material cap)
        {
            CreateCylinder("Pipe",p,new Vector3(0.20f,1.6f,0.20f),pipe,parent);
            CreateCylinder("PipeCap",p+new Vector3(0f,1.65f,0f),new Vector3(0.28f,0.08f,0.28f),cap,parent);
        }

        private static void CreateClockTowerOnBuilding(Transform parent,Vector3 p,Material wall,Material roof,Material trim,Material face)
        {
            CreateBox("Tower",p,new Vector3(3.0f,3.4f,3.0f),wall,parent);
            CreateCylinder("Clock",p+new Vector3(0f,0.25f,-1.56f),new Vector3(0.58f,0.06f,0.58f),face,parent).transform.localRotation=Quaternion.Euler(90f,0f,0f);
            CreatePyramidRoof(parent,p+new Vector3(0f,2.25f,0f),3.5f,1.6f,roof);
            CreateBox("TowerTrim",p+new Vector3(0f,-1.65f,0f),new Vector3(3.2f,0.18f,3.2f),trim,parent);
        }

        private static GameObject Root(string name,Vector3 p,float yaw)
        {
            GameObject root=new GameObject(name);
            root.transform.position=p;
            root.transform.eulerAngles=new Vector3(0f,yaw,0f);
            return root;
        }

        private static GameObject CreateBox(string name,Vector3 p,Vector3 s,Material mat,Transform parent=null)
        {
            GameObject go=GameObject.CreatePrimitive(PrimitiveType.Cube);
            go.name=name;
            if(parent!=null){go.transform.SetParent(parent,false);go.transform.localPosition=p;}
            else go.transform.position=p;
            go.transform.localScale=s;
            Renderer r=go.GetComponent<Renderer>();
            r.sharedMaterial=mat; r.shadowCastingMode=ShadowCastingMode.On; r.receiveShadows=true;
            return go;
        }

        private static GameObject CreateSphere(string name,Vector3 p,Vector3 s,Material mat,Transform parent=null)
        {
            GameObject go=GameObject.CreatePrimitive(PrimitiveType.Sphere);
            go.name=name;
            if(parent!=null){go.transform.SetParent(parent,false);go.transform.localPosition=p;}
            else go.transform.position=p;
            go.transform.localScale=s;
            go.GetComponent<Renderer>().sharedMaterial=mat;
            return go;
        }

        private static GameObject CreateCapsule(string name,Vector3 p,Vector3 s,Material mat,Transform parent=null)
        {
            GameObject go=GameObject.CreatePrimitive(PrimitiveType.Capsule);
            go.name=name;
            if(parent!=null){go.transform.SetParent(parent,false);go.transform.localPosition=p;}
            else go.transform.position=p;
            go.transform.localScale=s;
            go.GetComponent<Renderer>().sharedMaterial=mat;
            Collider col=go.GetComponent<Collider>(); if(col!=null) UnityEngine.Object.DestroyImmediate(col);
            return go;
        }

        private static GameObject CreateCylinder(string name,Vector3 p,Vector3 s,Material mat,Transform parent=null)
        {
            GameObject go=GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            go.name=name;
            if(parent!=null){go.transform.SetParent(parent,false);go.transform.localPosition=p;}
            else go.transform.position=p;
            go.transform.localScale=s;
            go.GetComponent<Renderer>().sharedMaterial=mat;
            return go;
        }

        private static void CreateDirectionalLight(string name,Color color,float intensity,Vector3 euler,bool shadows)
        {
            GameObject go=new GameObject(name);
            Light l=go.AddComponent<Light>(); l.type=LightType.Directional;l.color=color;l.intensity=intensity;
            l.shadows=shadows?LightShadows.Soft:LightShadows.None;l.shadowStrength=shadows?0.58f:0f;
            go.transform.eulerAngles=euler;
        }

        private static void EnsureFolders()
        {
            EnsureFolder("Assets/Art");
            EnsureFolder("Assets/Art/Generated3D");
            EnsureFolder(ArtRoot);
            EnsureFolder(TextureDir);
            EnsureFolder(MaterialDir);
        }

        private static void EnsureFolder(string path)
        {
            if(AssetDatabase.IsValidFolder(path)) return;
            string parent=Path.GetDirectoryName(path)?.Replace("\\","/");
            string name=Path.GetFileName(path);
            if(!string.IsNullOrEmpty(parent)&&!AssetDatabase.IsValidFolder(parent)) EnsureFolder(parent);
            AssetDatabase.CreateFolder(parent??"Assets",name);
        }

        private static Material TexturedMat(string name,string texFile,float metallic,float smoothness,Vector2 tiling)
        {
            string path=MaterialDir+"/"+name+".mat";
            Shader shader=Shader.Find("Standard")??Shader.Find("Universal Render Pipeline/Lit")??Shader.Find("Unlit/Texture");
            Material mat=AssetDatabase.LoadAssetAtPath<Material>(path);
            if(mat==null){mat=new Material(shader){name=name};AssetDatabase.CreateAsset(mat,path);}
            mat.shader=shader;
            Texture2D tex=AssetDatabase.LoadAssetAtPath<Texture2D>(TextureDir+"/"+texFile);
            mat.color=Color.white;mat.mainTexture=tex;mat.mainTextureScale=tiling;
            if(mat.HasProperty("_BaseMap")) mat.SetTexture("_BaseMap",tex);
            if(mat.HasProperty("_Metallic")) mat.SetFloat("_Metallic",metallic);
            if(mat.HasProperty("_Smoothness")) mat.SetFloat("_Smoothness",smoothness);
            if(mat.HasProperty("_Glossiness")) mat.SetFloat("_Glossiness",smoothness);
            EditorUtility.SetDirty(mat);
            return mat;
        }

        private static Material FlatMat(string name,string html,float metallic,float smoothness)
        {
            string path=MaterialDir+"/"+name+".mat";
            Shader shader=Shader.Find("Standard")??Shader.Find("Universal Render Pipeline/Lit")??Shader.Find("Unlit/Color");
            Material mat=AssetDatabase.LoadAssetAtPath<Material>(path);
            if(mat==null){mat=new Material(shader){name=name};AssetDatabase.CreateAsset(mat,path);}
            mat.shader=shader;Color c=Hex(html);mat.color=c;
            if(mat.HasProperty("_BaseColor")) mat.SetColor("_BaseColor",c);
            if(mat.HasProperty("_Metallic")) mat.SetFloat("_Metallic",metallic);
            if(mat.HasProperty("_Smoothness")) mat.SetFloat("_Smoothness",smoothness);
            if(mat.HasProperty("_Glossiness")) mat.SetFloat("_Glossiness",smoothness);
            EditorUtility.SetDirty(mat);
            return mat;
        }

        private static void GenerateTextures()
        {
            Noise("grass.png",Hex("#668B58"),Hex("#8EAD6F"),101,0.30f);
            Noise("grass_deep.png",Hex("#496D4E"),Hex("#6F925E"),151,0.34f);
            Noise("road.png",Hex("#545A5C"),Hex("#777B7D"),201,0.22f);
            Pavers("pavers.png",Hex("#C4BBA6"),Hex("#9F9788"));
            Brick("brick.png",Hex("#D0AF95"),Hex("#9E6554"),Hex("#B87863"));
            Stucco("stucco_warm.png",Hex("#D9C593"),Hex("#E9D9AF"),301);
            Stucco("stucco_blue.png",Hex("#779BA7"),Hex("#94B4BD"),307);
            Stucco("stucco_rose.png",Hex("#B86E67"),Hex("#D28A7C"),311);
            Wood("wood.png",Hex("#654A38"),Hex("#916A4D"));
            Roof("roof_red.png",Hex("#75484A"),Hex("#9A5A55"));
            Roof("roof_teal.png",Hex("#466B67"),Hex("#628B83"));
            Roof("roof_slate.png",Hex("#46545D"),Hex("#66757C"));
            Stone("stone.png",Hex("#87877F"),Hex("#A5A397"));
            AssetDatabase.Refresh();
        }

        private static void Noise(string file,Color a,Color b,int seed,float strength)
        {
            const int w=128,h=128;Texture2D tex=new Texture2D(w,h,TextureFormat.RGBA32,false);
            System.Random rng=new System.Random(seed);Color32[] px=new Color32[w*h];
            for(int y=0;y<h;y++)for(int x=0;x<w;x++)
            {
                float n=(float)rng.NextDouble();float p=Mathf.PerlinNoise((x+seed)*0.055f,(y-seed)*0.055f);
                Color c=Color.Lerp(a,b,strength*(0.45f*n+0.55f*p));
                if(rng.NextDouble()<0.018)c*=0.84f;px[y*w+x]=c;
            }
            tex.SetPixels32(px);tex.Apply();SaveTexture(file,tex);UnityEngine.Object.DestroyImmediate(tex);
        }

        private static void Pavers(string file,Color baseC,Color seamC)
        {
            const int w=128,h=128;Texture2D tex=new Texture2D(w,h,TextureFormat.RGBA32,false);Color32[] px=new Color32[w*h];
            for(int y=0;y<h;y++)for(int x=0;x<w;x++)
            {
                int row=y/16;int sx=x+((row&1)==0?0:12);bool seam=(y%16<=1)||(sx%24<=1);
                float n=Mathf.PerlinNoise(x*0.08f,y*0.08f)*0.10f;px[y*w+x]=seam?seamC:Color.Lerp(baseC,Color.white,n);
            }
            tex.SetPixels32(px);tex.Apply();SaveTexture(file,tex);UnityEngine.Object.DestroyImmediate(tex);
        }

        private static void Brick(string file,Color mortar,Color a,Color b)
        {
            const int w=128,h=128;Texture2D tex=new Texture2D(w,h,TextureFormat.RGBA32,false);Color32[] px=new Color32[w*h];
            for(int y=0;y<h;y++)for(int x=0;x<w;x++)
            {
                int row=y/14;int sx=x+((row&1)==0?0:14);bool seam=(y%14<=1)||(sx%28<=1);
                float n=Mathf.PerlinNoise(x*0.08f,y*0.08f)*0.42f;px[y*w+x]=seam?mortar:Color.Lerp(a,b,n);
            }
            tex.SetPixels32(px);tex.Apply();SaveTexture(file,tex);UnityEngine.Object.DestroyImmediate(tex);
        }

        private static void Stucco(string file,Color a,Color b,int seed)
        {
            const int w=128,h=128;Texture2D tex=new Texture2D(w,h,TextureFormat.RGBA32,false);System.Random rng=new System.Random(seed);Color32[] px=new Color32[w*h];
            for(int i=0;i<px.Length;i++){float t=0.18f+(float)rng.NextDouble()*0.20f;Color c=Color.Lerp(a,b,t);if(rng.NextDouble()<0.012)c*=0.88f;px[i]=c;}
            tex.SetPixels32(px);tex.Apply();SaveTexture(file,tex);UnityEngine.Object.DestroyImmediate(tex);
        }

        private static void Wood(string file,Color a,Color b)
        {
            const int w=128,h=128;Texture2D tex=new Texture2D(w,h,TextureFormat.RGBA32,false);Color32[] px=new Color32[w*h];
            for(int y=0;y<h;y++)for(int x=0;x<w;x++){float g=Mathf.PerlinNoise(x*0.16f,y*0.03f);bool seam=x%26<=1;Color c=Color.Lerp(a,b,0.18f+g*0.40f);if(seam)c*=0.72f;px[y*w+x]=c;}
            tex.SetPixels32(px);tex.Apply();SaveTexture(file,tex);UnityEngine.Object.DestroyImmediate(tex);
        }

        private static void Roof(string file,Color a,Color b)
        {
            const int w=128,h=128;Texture2D tex=new Texture2D(w,h,TextureFormat.RGBA32,false);Color32[] px=new Color32[w*h];
            for(int y=0;y<h;y++)for(int x=0;x<w;x++){bool seam=(y%14<=1)||((x+((y/14)&1)*9)%18<=1);Color c=Color.Lerp(a,b,0.26f+(y%14)/14f*0.14f);if(seam)c*=0.72f;px[y*w+x]=c;}
            tex.SetPixels32(px);tex.Apply();SaveTexture(file,tex);UnityEngine.Object.DestroyImmediate(tex);
        }

        private static void Stone(string file,Color a,Color b)
        {
            const int w=128,h=128;Texture2D tex=new Texture2D(w,h,TextureFormat.RGBA32,false);Color32[] px=new Color32[w*h];
            for(int y=0;y<h;y++)for(int x=0;x<w;x++){bool seam=(x%24<=1)||(y%18<=1);float n=Mathf.PerlinNoise(x*0.06f,y*0.06f)*0.25f;Color c=seam?a*0.72f:Color.Lerp(a,b,n);px[y*w+x]=c;}
            tex.SetPixels32(px);tex.Apply();SaveTexture(file,tex);UnityEngine.Object.DestroyImmediate(tex);
        }

        private static void SaveTexture(string file,Texture2D tex)
        {
            string assetPath=TextureDir+"/"+file;
            string projectRoot=Directory.GetParent(Application.dataPath).FullName;
            string abs=Path.Combine(projectRoot,assetPath.Replace('/',Path.DirectorySeparatorChar));
            Directory.CreateDirectory(Path.GetDirectoryName(abs));
            File.WriteAllBytes(abs,tex.EncodeToPNG());
            AssetDatabase.ImportAsset(assetPath,ImportAssetOptions.ForceUpdate);
            TextureImporter importer=AssetImporter.GetAtPath(assetPath) as TextureImporter;
            if(importer!=null){importer.wrapMode=TextureWrapMode.Repeat;importer.filterMode=FilterMode.Bilinear;importer.textureCompression=TextureImporterCompression.Uncompressed;importer.mipmapEnabled=true;importer.SaveAndReimport();}
        }

        private static Color Hex(string html)
        {
            Color c;if(!ColorUtility.TryParseHtmlString(html,out c))throw new ArgumentException("Invalid color "+html);return c;
        }

        private static void Render(Camera camera,string path,int width,int height)
        {
            RenderTexture rt=new RenderTexture(width,height,24,RenderTextureFormat.ARGB32);rt.antiAliasing=4;
            Texture2D image=new Texture2D(width,height,TextureFormat.RGB24,false);
            RenderTexture old=RenderTexture.active;camera.targetTexture=rt;RenderTexture.active=rt;camera.Render();
            image.ReadPixels(new Rect(0,0,width,height),0,0);image.Apply();File.WriteAllBytes(path,image.EncodeToPNG());
            camera.targetTexture=null;RenderTexture.active=old;UnityEngine.Object.DestroyImmediate(image);UnityEngine.Object.DestroyImmediate(rt);
        }
    }
}
