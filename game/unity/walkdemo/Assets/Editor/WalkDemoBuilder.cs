using System;
using System.IO;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Dolzore.Editor
{
    public static class WalkDemoBuilder
    {
        private const string ScenePath = "Assets/Scenes/WalkDemo.unity";
        private const string MaterialDir = "Assets/Generated/Materials";
        private const string MeshDir = "Assets/Generated/Meshes";
        private const string BuildDir = "Builds/WalkDemo";
        private const string ArtifactDir = "BuildArtifacts";

        private static Material skin;
        private static Material hair;
        private static Material white;
        private static Material blue;
        private static Material navy;
        private static Material gold;
        private static Material boot;
        private static Material eye;
        private static Material blush;
        private static Material crystal;
        private static Material stone;
        private static Material stoneLight;
        private static Material grass;
        private static Material bark;
        private static Material leaf;
        private static Material auraLine;

        public static void BuildAndExport()
        {
            Debug.Log("DOLZORE_WALK_DEMO_BEGIN");
            ConfigurePlayer();
            EnsureDirectories();
            CreateMaterials();
            Scene scene = BuildScene();
            EditorSceneManager.SaveScene(scene, ScenePath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            VerifyScene();
            RenderPreview();
            BuildWebGL();
            WriteReceipt();
            Debug.Log("DOLZORE_WALK_DEMO_SUCCESS");
        }

        private static void ConfigurePlayer()
        {
            PlayerSettings.companyName = "DOLZORE";
            PlayerSettings.productName = "DOLZORE Walk Demo";
            PlayerSettings.bundleVersion = "0.1.0-walk";
            PlayerSettings.defaultScreenWidth = 1280;
            PlayerSettings.defaultScreenHeight = 720;
            PlayerSettings.runInBackground = true;
            PlayerSettings.colorSpace = ColorSpace.Linear;
            PlayerSettings.resizableWindow = true;
            PlayerSettings.WebGL.compressionFormat = WebGLCompressionFormat.Disabled;
            QualitySettings.vSyncCount = 1;
            QualitySettings.shadowDistance = 35f;
            QualitySettings.antiAliasing = 2;
        }

        private static void EnsureDirectories()
        {
            Directory.CreateDirectory(Abs("Assets/Scenes"));
            Directory.CreateDirectory(Abs(MaterialDir));
            Directory.CreateDirectory(Abs(MeshDir));
            Directory.CreateDirectory(Abs(BuildDir));
            Directory.CreateDirectory(Abs(ArtifactDir));
            AssetDatabase.Refresh();
        }

        private static void CreateMaterials()
        {
            skin = Mat("Skin", Hex("#F3D1B2"), 0f, 0.48f);
            hair = Mat("Hair", Hex("#F2C462"), 0.03f, 0.58f);
            white = Mat("Ivory", Hex("#F7F4EA"), 0f, 0.42f);
            blue = Mat("RoyalBlue", Hex("#3565CF"), 0.12f, 0.6f);
            navy = Mat("Navy", Hex("#1C2A68"), 0.1f, 0.5f);
            gold = Mat("Gold", Hex("#D8A53A"), 0.62f, 0.72f);
            boot = Mat("Boot", Hex("#E6E0D2"), 0.05f, 0.5f);
            eye = Mat("Eye", Hex("#4858CC"), 0.05f, 0.76f);
            blush = Mat("Blush", Hex("#E9A2A5"), 0f, 0.36f);
            crystal = Mat("Crystal", Hex("#61D9FF"), 0.15f, 0.82f, Hex("#218BD8") * 1.8f);
            stone = Mat("Stone", Hex("#596579"), 0f, 0.18f);
            stoneLight = Mat("StoneLight", Hex("#8593A5"), 0f, 0.2f);
            grass = Mat("Grass", Hex("#365C49"), 0f, 0.12f);
            bark = Mat("Bark", Hex("#73523D"), 0f, 0.2f);
            leaf = Mat("Leaf", Hex("#4A8C64"), 0f, 0.12f);
            auraLine = MatWithShader("AuraLine", Hex("#A4EEFF"), "Unlit/Color");
        }

        private static Scene BuildScene()
        {
            Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

            RenderSettings.ambientMode = AmbientMode.Flat;
            RenderSettings.ambientLight = Hex("#7684A0") * 0.68f;
            RenderSettings.fog = true;
            RenderSettings.fogMode = FogMode.Linear;
            RenderSettings.fogColor = Hex("#1B263C");
            RenderSettings.fogStartDistance = 18f;
            RenderSettings.fogEndDistance = 42f;

            Camera cam = CreateCamera();
            AddLighting();
            BuildEnvironment();
            GameObject player = BuildCharacter();

            WalkDemoCamera follow = cam.gameObject.AddComponent<WalkDemoCamera>();
            follow.target = player.transform;
            follow.offset = new Vector3(0f, 5.2f, -8.4f);
            follow.lookHeight = 1.65f;
            follow.smooth = 9f;

            BuildUI();
            return scene;
        }

        private static Camera CreateCamera()
        {
            GameObject go = new GameObject("Main Camera");
            go.tag = "MainCamera";
            Camera cam = go.AddComponent<Camera>();
            cam.clearFlags = CameraClearFlags.SolidColor;
            cam.backgroundColor = Hex("#17213A");
            cam.fieldOfView = 45f;
            cam.nearClipPlane = 0.05f;
            cam.farClipPlane = 80f;
            cam.transform.position = new Vector3(0f, 5.2f, -8.4f);
            cam.transform.LookAt(new Vector3(0f, 1.7f, 0f));
            go.AddComponent<AudioListener>();
            return cam;
        }

        private static void AddLighting()
        {
            GameObject sunGo = new GameObject("Key Light");
            Light sun = sunGo.AddComponent<Light>();
            sun.type = LightType.Directional;
            sun.color = Hex("#FFF2DB");
            sun.intensity = 1.45f;
            sun.shadows = LightShadows.Soft;
            sun.shadowStrength = 0.65f;
            sunGo.transform.rotation = Quaternion.Euler(46f, -34f, 0f);

            GameObject fillGo = new GameObject("Blue Fill");
            Light fill = fillGo.AddComponent<Light>();
            fill.type = LightType.Directional;
            fill.color = Hex("#8FB8FF");
            fill.intensity = 0.35f;
            fillGo.transform.rotation = Quaternion.Euler(28f, 148f, 0f);
        }

        private static void BuildEnvironment()
        {
            GameObject ground = GameObject.CreatePrimitive(PrimitiveType.Plane);
            ground.name = "Walk Ground";
            ground.transform.localScale = new Vector3(3.2f, 1f, 3.2f);
            ground.GetComponent<Renderer>().sharedMaterial = grass;

            GameObject platform = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            platform.name = "Start Platform";
            platform.transform.position = new Vector3(0f, 0.08f, 0f);
            platform.transform.localScale = new Vector3(2.5f, 0.08f, 2.5f);
            platform.GetComponent<Renderer>().sharedMaterial = stone;
            UnityEngine.Object.DestroyImmediate(platform.GetComponent<Collider>());

            for (int i = -4; i <= 4; i++)
            {
                AddStoneTile(new Vector3(i * 1.1f, 0.025f, 0f), new Vector3(0.98f, 0.05f, 1.0f), (i & 1) == 0 ? stoneLight : stone);
                AddStoneTile(new Vector3(0f, 0.025f, i * 1.1f), new Vector3(1.0f, 0.05f, 0.98f), (i & 1) == 0 ? stone : stoneLight);
            }

            AddTree(new Vector3(-5.2f, 0f, 4.2f), 1.05f);
            AddTree(new Vector3(5.3f, 0f, 4.4f), 1.15f);
            AddTree(new Vector3(-5.5f, 0f, -4.7f), 0.9f);
            AddTree(new Vector3(5.5f, 0f, -4.6f), 1.0f);

            AddCrystalPillar(new Vector3(-3.7f, 0f, 3.4f));
            AddCrystalPillar(new Vector3(3.7f, 0f, 3.4f));
            AddCrystalPillar(new Vector3(-3.7f, 0f, -3.4f));
            AddCrystalPillar(new Vector3(3.7f, 0f, -3.4f));

            AddBoundary("North Wall", new Vector3(0f, 0.75f, 8f), new Vector3(16f, 1.5f, 0.35f));
            AddBoundary("South Wall", new Vector3(0f, 0.75f, -8f), new Vector3(16f, 1.5f, 0.35f));
            AddBoundary("East Wall", new Vector3(8f, 0.75f, 0f), new Vector3(0.35f, 1.5f, 16f));
            AddBoundary("West Wall", new Vector3(-8f, 0.75f, 0f), new Vector3(0.35f, 1.5f, 16f));
        }

        private static GameObject BuildCharacter()
        {
            GameObject root = new GameObject("Lunaria Walkable Character");
            root.transform.position = new Vector3(0f, 0.08f, 0f);
            root.transform.rotation = Quaternion.Euler(0f, 180f, 0f);

            CharacterController cc = root.AddComponent<CharacterController>();
            cc.height = 3.45f;
            cc.radius = 0.62f;
            cc.center = new Vector3(0f, 1.72f, 0f);
            cc.stepOffset = 0.28f;
            cc.slopeLimit = 45f;

            GameObject visualGo = new GameObject("Visual");
            visualGo.transform.SetParent(root.transform, false);
            Transform visual = visualGo.transform;

            // Dress/body core.
            Transform body = NewPivot("Body", visual, new Vector3(0f, 1.46f, 0f));
            GameObject dress = CreateMeshObject("Bell Dress", body, DressMesh(), Vector3.zero, new Vector3(1f, 1f, 1f), blue);
            dress.transform.localRotation = Quaternion.identity;

            Part(PrimitiveType.Capsule, "Torso", body, new Vector3(0f, 0.44f, 0f), new Vector3(0.72f, 0.5f, 0.52f), white);
            Part(PrimitiveType.Cube, "Blue Bodice", body, new Vector3(0f, 0.44f, 0.33f), new Vector3(0.78f, 0.56f, 0.16f), navy);
            Part(PrimitiveType.Cube, "Gold Belt", body, new Vector3(0f, 0.08f, 0.18f), new Vector3(1.05f, 0.12f, 0.52f), gold);
            Part(PrimitiveType.Cube, "Cape", body, new Vector3(0f, 0.38f, -0.43f), new Vector3(1.08f, 1.12f, 0.08f), navy);

            // Legs.
            Transform leftLeg = NewPivot("LeftLeg", visual, new Vector3(-0.27f, 0.88f, 0f));
            Transform rightLeg = NewPivot("RightLeg", visual, new Vector3(0.27f, 0.88f, 0f));
            Part(PrimitiveType.Capsule, "Leg", leftLeg, new Vector3(0f, -0.38f, 0f), new Vector3(0.23f, 0.40f, 0.23f), skin);
            Part(PrimitiveType.Capsule, "Leg", rightLeg, new Vector3(0f, -0.38f, 0f), new Vector3(0.23f, 0.40f, 0.23f), skin);
            Part(PrimitiveType.Cube, "Boot", leftLeg, new Vector3(0f, -0.82f, 0.08f), new Vector3(0.42f, 0.28f, 0.62f), boot);
            Part(PrimitiveType.Cube, "Boot", rightLeg, new Vector3(0f, -0.82f, 0.08f), new Vector3(0.42f, 0.28f, 0.62f), boot);
            Part(PrimitiveType.Cube, "Boot Gold", leftLeg, new Vector3(0f, -0.69f, 0.26f), new Vector3(0.46f, 0.10f, 0.18f), gold);
            Part(PrimitiveType.Cube, "Boot Gold", rightLeg, new Vector3(0f, -0.69f, 0.26f), new Vector3(0.46f, 0.10f, 0.18f), gold);

            // Arms.
            Transform leftArm = NewPivot("LeftArm", visual, new Vector3(-0.72f, 1.92f, 0f));
            Transform rightArm = NewPivot("RightArm", visual, new Vector3(0.72f, 1.92f, 0f));
            Part(PrimitiveType.Capsule, "Sleeve", leftArm, new Vector3(0f, -0.36f, 0f), new Vector3(0.27f, 0.42f, 0.27f), white);
            Part(PrimitiveType.Capsule, "Sleeve", rightArm, new Vector3(0f, -0.36f, 0f), new Vector3(0.27f, 0.42f, 0.27f), white);
            Part(PrimitiveType.Sphere, "Hand", leftArm, new Vector3(0f, -0.82f, 0f), new Vector3(0.28f, 0.28f, 0.28f), skin);
            Part(PrimitiveType.Sphere, "Hand", rightArm, new Vector3(0f, -0.82f, 0f), new Vector3(0.28f, 0.28f, 0.28f), skin);
            Part(PrimitiveType.Cube, "Cuff", leftArm, new Vector3(0f, -0.62f, 0f), new Vector3(0.36f, 0.13f, 0.36f), gold);
            Part(PrimitiveType.Cube, "Cuff", rightArm, new Vector3(0f, -0.62f, 0f), new Vector3(0.36f, 0.13f, 0.36f), gold);

            // Staff held by right hand.
            Transform staff = NewPivot("Staff", rightArm, new Vector3(0.30f, -0.82f, 0.02f));
            Part(PrimitiveType.Cylinder, "Staff Shaft", staff, new Vector3(0f, 0.62f, 0f), new Vector3(0.075f, 0.78f, 0.075f), gold);
            Part(PrimitiveType.Sphere, "Staff Crown", staff, new Vector3(0f, 1.48f, 0f), new Vector3(0.34f, 0.34f, 0.34f), gold);
            Part(PrimitiveType.Sphere, "Crystal Orb", staff, new Vector3(0f, 1.48f, 0.12f), new Vector3(0.24f, 0.24f, 0.24f), crystal);
            Part(PrimitiveType.Capsule, "Staff Wing L", staff, new Vector3(-0.30f, 1.45f, 0f), new Vector3(0.09f, 0.30f, 0.09f), gold).transform.localRotation = Quaternion.Euler(0f, 0f, 62f);
            Part(PrimitiveType.Capsule, "Staff Wing R", staff, new Vector3(0.30f, 1.45f, 0f), new Vector3(0.09f, 0.30f, 0.09f), gold).transform.localRotation = Quaternion.Euler(0f, 0f, -62f);

            Light orbLight = new GameObject("Crystal Light").AddComponent<Light>();
            orbLight.transform.SetParent(staff, false);
            orbLight.transform.localPosition = new Vector3(0f, 1.48f, 0.1f);
            orbLight.type = LightType.Point;
            orbLight.color = Hex("#61D9FF");
            orbLight.intensity = 1.4f;
            orbLight.range = 3.2f;

            // Head rig.
            Transform headRig = NewPivot("HeadRig", visual, new Vector3(0f, 2.72f, 0f));
            Part(PrimitiveType.Sphere, "Head", headRig, Vector3.zero, new Vector3(1.24f, 1.12f, 1.02f), skin);

            // Back hair mass and side locks.
            Part(PrimitiveType.Sphere, "Hair Back", headRig, new Vector3(0f, 0.12f, -0.28f), new Vector3(1.34f, 1.22f, 0.78f), hair);
            Part(PrimitiveType.Sphere, "Hair Crown", headRig, new Vector3(0f, 0.62f, -0.02f), new Vector3(1.03f, 0.54f, 0.86f), hair);
            Part(PrimitiveType.Capsule, "Hair Lock L", headRig, new Vector3(-0.72f, -0.24f, 0.03f), new Vector3(0.22f, 0.55f, 0.20f), hair).transform.localRotation = Quaternion.Euler(0f, 0f, -12f);
            Part(PrimitiveType.Capsule, "Hair Lock R", headRig, new Vector3(0.72f, -0.24f, 0.03f), new Vector3(0.22f, 0.55f, 0.20f), hair).transform.localRotation = Quaternion.Euler(0f, 0f, 12f);

            // Front fringe pieces.
            Part(PrimitiveType.Sphere, "Bang Center", headRig, new Vector3(0f, 0.43f, 0.77f), new Vector3(0.34f, 0.43f, 0.20f), hair);
            Part(PrimitiveType.Sphere, "Bang L", headRig, new Vector3(-0.38f, 0.39f, 0.70f), new Vector3(0.32f, 0.44f, 0.20f), hair);
            Part(PrimitiveType.Sphere, "Bang R", headRig, new Vector3(0.38f, 0.39f, 0.70f), new Vector3(0.32f, 0.44f, 0.20f), hair);

            // Hair tail pivot for secondary movement.
            Transform hairTail = NewPivot("HairTail", headRig, new Vector3(0.58f, 0.16f, -0.68f));
            Part(PrimitiveType.Capsule, "Ponytail", hairTail, new Vector3(0.15f, -0.55f, -0.05f), new Vector3(0.30f, 0.74f, 0.28f), hair).transform.localRotation = Quaternion.Euler(0f, 0f, -18f);
            Part(PrimitiveType.Sphere, "Hair Ribbon", hairTail, new Vector3(-0.02f, 0.06f, 0f), new Vector3(0.30f, 0.18f, 0.16f), blue);

            // Face details.
            Part(PrimitiveType.Sphere, "Eye L", headRig, new Vector3(-0.34f, 0.03f, 0.98f), new Vector3(0.14f, 0.18f, 0.07f), eye);
            Part(PrimitiveType.Sphere, "Eye R", headRig, new Vector3(0.34f, 0.03f, 0.98f), new Vector3(0.14f, 0.18f, 0.07f), eye);
            Part(PrimitiveType.Sphere, "Eye Glint L", headRig, new Vector3(-0.30f, 0.09f, 1.045f), new Vector3(0.035f, 0.045f, 0.02f), white);
            Part(PrimitiveType.Sphere, "Eye Glint R", headRig, new Vector3(0.38f, 0.09f, 1.045f), new Vector3(0.035f, 0.045f, 0.02f), white);
            Part(PrimitiveType.Cube, "Mouth", headRig, new Vector3(0f, -0.30f, 1.03f), new Vector3(0.22f, 0.055f, 0.04f), blush);
            Part(PrimitiveType.Sphere, "Cheek L", headRig, new Vector3(-0.59f, -0.20f, 0.88f), new Vector3(0.14f, 0.07f, 0.03f), blush);
            Part(PrimitiveType.Sphere, "Cheek R", headRig, new Vector3(0.59f, -0.20f, 0.88f), new Vector3(0.14f, 0.07f, 0.03f), blush);

            // Hair ornament.
            Part(PrimitiveType.Sphere, "Blue Gem", headRig, new Vector3(-0.73f, 0.48f, 0.49f), new Vector3(0.19f, 0.19f, 0.10f), crystal);
            Part(PrimitiveType.Capsule, "Gold Feather", headRig, new Vector3(-0.91f, 0.62f, 0.34f), new Vector3(0.08f, 0.27f, 0.07f), gold).transform.localRotation = Quaternion.Euler(0f, 0f, 55f);

            Transform ringA = CreateAuraRing("AuraRingA", visual, 1.05f, 0.030f, new Color(0.38f, 0.88f, 1f, 0.88f));
            Transform ringB = CreateAuraRing("AuraRingB", visual, 1.32f, 0.018f, new Color(0.90f, 0.72f, 0.24f, 0.66f));

            HeroStyleWalkCharacter movement = root.AddComponent<HeroStyleWalkCharacter>();
            movement.moveSpeed = 3.8f;
            movement.turnSpeed = 12f;
            movement.gravity = -22f;
            movement.visualRoot = visual;
            movement.body = body;
            movement.head = headRig;
            movement.leftArm = leftArm;
            movement.rightArm = rightArm;
            movement.leftLeg = leftLeg;
            movement.rightLeg = rightLeg;
            movement.hairTail = hairTail;
            movement.auraRingA = ringA;
            movement.auraRingB = ringB;

            return root;
        }

        private static Transform CreateAuraRing(string name, Transform parent, float radius, float width, Color color)
        {
            GameObject go = new GameObject(name);
            go.transform.SetParent(parent, false);
            go.transform.localPosition = new Vector3(0f, 0.02f, 0f);

            LineRenderer line = go.AddComponent<LineRenderer>();
            line.sharedMaterial = auraLine;
            line.useWorldSpace = false;
            line.loop = true;
            line.positionCount = 64;
            line.startWidth = width;
            line.endWidth = width;
            line.startColor = color;
            line.endColor = color;
            line.shadowCastingMode = ShadowCastingMode.Off;
            line.receiveShadows = false;

            for (int i = 0; i < line.positionCount; i++)
            {
                float angle = (Mathf.PI * 2f * i) / line.positionCount;
                line.SetPosition(i, new Vector3(Mathf.Cos(angle) * radius, 0f, Mathf.Sin(angle) * radius));
            }

            return go.transform;
        }

        private static void BuildUI()
        {
            GameObject canvasGo = new GameObject("Demo UI", typeof(RectTransform));
            Canvas canvas = canvasGo.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            CanvasScaler scaler = canvasGo.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1280f, 720f);
            canvasGo.AddComponent<GraphicRaycaster>();

            GameObject top = Panel(canvasGo.transform, "Top Panel", new Color(0.035f, 0.055f, 0.11f, 0.86f));
            RectTransform tr = top.GetComponent<RectTransform>();
            tr.anchorMin = new Vector2(0f, 1f);
            tr.anchorMax = new Vector2(1f, 1f);
            tr.pivot = new Vector2(0.5f, 1f);
            tr.sizeDelta = new Vector2(0f, 74f);
            tr.anchoredPosition = Vector2.zero;

            Text title = TextUI(top.transform, "Title", "DOLZORE  •  UNITY WALKABLE CHARACTER TEST", 22, Color.white, TextAnchor.MiddleLeft, FontStyle.Bold);
            Stretch(title.rectTransform);
            title.rectTransform.offsetMin = new Vector2(28f, 0f);
            title.rectTransform.offsetMax = new Vector2(-28f, 0f);

            GameObject help = Panel(canvasGo.transform, "Control Panel", new Color(0.035f, 0.055f, 0.11f, 0.88f));
            RectTransform hr = help.GetComponent<RectTransform>();
            hr.anchorMin = new Vector2(0f, 0f);
            hr.anchorMax = new Vector2(0f, 0f);
            hr.pivot = new Vector2(0f, 0f);
            hr.sizeDelta = new Vector2(520f, 112f);
            hr.anchoredPosition = new Vector2(22f, 22f);

            Text controls = TextUI(help.transform, "Controls", "CLICK GAME FIRST\nWASD / ARROW KEYS  •  WALK", 20, Hex("#E8F7FF"), TextAnchor.MiddleLeft, FontStyle.Bold);
            Stretch(controls.rectTransform);
            controls.rectTransform.offsetMin = new Vector2(24f, 12f);
            controls.rectTransform.offsetMax = new Vector2(-20f, -12f);

            GameObject badge = Panel(canvasGo.transform, "Unity Badge", new Color(0.10f, 0.20f, 0.38f, 0.84f));
            RectTransform br = badge.GetComponent<RectTransform>();
            br.anchorMin = new Vector2(1f, 0f);
            br.anchorMax = new Vector2(1f, 0f);
            br.pivot = new Vector2(1f, 0f);
            br.sizeDelta = new Vector2(380f, 74f);
            br.anchoredPosition = new Vector2(-22f, 22f);

            Text badgeText = TextUI(badge.transform, "Badge Text", "ACTUAL UNITY WEBGL\nPROCEDURAL 3D • NO CHARACTER IMAGE", 14, Hex("#BDEFFF"), TextAnchor.MiddleCenter, FontStyle.Bold);
            Stretch(badgeText.rectTransform);
        }

        private static void VerifyScene()
        {
            Scene scene = EditorSceneManager.OpenScene(ScenePath, OpenSceneMode.Single);
            GameObject player = GameObject.Find("Lunaria Walkable Character");
            if (player == null) throw new InvalidOperationException("WALK_DEMO_PLAYER_MISSING");
            if (player.GetComponent<CharacterController>() == null) throw new InvalidOperationException("WALK_DEMO_CHARACTER_CONTROLLER_MISSING");
            if (player.GetComponent<HeroStyleWalkCharacter>() == null) throw new InvalidOperationException("WALK_DEMO_MOVEMENT_SCRIPT_MISSING");
            if (Camera.main == null) throw new InvalidOperationException("WALK_DEMO_CAMERA_MISSING");
            if (Camera.main.GetComponent<WalkDemoCamera>() == null) throw new InvalidOperationException("WALK_DEMO_FOLLOW_CAMERA_MISSING");

            int meshRenderers = UnityEngine.Object.FindObjectsByType<MeshRenderer>(FindObjectsSortMode.None).Length;
            if (meshRenderers < 30) throw new InvalidOperationException("WALK_DEMO_VISUAL_COMPLEXITY_TOO_LOW:" + meshRenderers);

            Debug.Log("DOLZORE_WALK_DEMO_VERIFY=PASS");
            Debug.Log("DOLZORE_WALK_DEMO_MESH_RENDERERS=" + meshRenderers);
        }

        private static void RenderPreview()
        {
            EditorSceneManager.OpenScene(ScenePath, OpenSceneMode.Single);
            Camera cam = Camera.main;
            if (cam == null) throw new InvalidOperationException("WALK_DEMO_PREVIEW_CAMERA_MISSING");

            const int width = 1280;
            const int height = 720;
            RenderTexture rt = new RenderTexture(width, height, 24, RenderTextureFormat.ARGB32);
            Texture2D tex = new Texture2D(width, height, TextureFormat.RGB24, false);

            cam.targetTexture = rt;
            RenderTexture.active = rt;
            cam.Render();
            tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
            tex.Apply();

            File.WriteAllBytes(Abs(ArtifactDir + "/walk-demo.png"), tex.EncodeToPNG());

            cam.targetTexture = null;
            RenderTexture.active = null;
            UnityEngine.Object.DestroyImmediate(rt);
            UnityEngine.Object.DestroyImmediate(tex);
        }

        private static void BuildWebGL()
        {
            string output = Abs(BuildDir);
            if (Directory.Exists(output)) Directory.Delete(output, true);
            Directory.CreateDirectory(output);

            EditorBuildSettings.scenes = new[]
            {
                new EditorBuildSettingsScene(ScenePath, true)
            };

            BuildPlayerOptions options = new BuildPlayerOptions
            {
                scenes = new[] { ScenePath },
                locationPathName = output,
                target = BuildTarget.WebGL,
                options = BuildOptions.None
            };

            BuildReport report = BuildPipeline.BuildPlayer(options);
            if (report.summary.result != BuildResult.Succeeded)
                throw new InvalidOperationException("WALK_DEMO_WEBGL_BUILD_FAILED:" + report.summary.result);

            Debug.Log("DOLZORE_WALK_DEMO_WEBGL_BYTES=" + report.summary.totalSize);
        }

        private static void WriteReceipt()
        {
            string json =
                "{\n" +
                "  \"engine\": \"Unity\",\n" +
                "  \"scene\": \"Assets/Scenes/WalkDemo.unity\",\n" +
                "  \"character\": \"Lunaria Walkable Character\",\n" +
                "  \"character_source\": \"procedural Unity meshes/materials\",\n" +
                "  \"character_image_texture_used\": false,\n" +
                "  \"movement\": [\"W\",\"A\",\"S\",\"D\",\"ArrowKeys\"],\n" +
                "  \"walk_animation\": \"runtime transform gait animation\",\n" +
                "  \"webgl\": true,\n" +
                "  \"acceptance\": \"PASS\"\n" +
                "}\n";
            File.WriteAllText(Abs(ArtifactDir + "/walk-demo-receipt.json"), json);
        }

        private static Mesh DressMesh()
        {
            const string path = MeshDir + "/BellDress.asset";
            Mesh existing = AssetDatabase.LoadAssetAtPath<Mesh>(path);
            if (existing != null) return existing;

            const int segments = 20;
            const float topRadius = 0.46f;
            const float bottomRadius = 0.74f;
            const float height = 1.08f;
            Vector3[] vertices = new Vector3[segments * 2 + 2];
            int[] triangles = new int[segments * 12];

            for (int i = 0; i < segments; i++)
            {
                float a = Mathf.PI * 2f * i / segments;
                float c = Mathf.Cos(a);
                float s = Mathf.Sin(a);
                vertices[i] = new Vector3(c * bottomRadius, -height * 0.5f, s * bottomRadius);
                vertices[i + segments] = new Vector3(c * topRadius, height * 0.5f, s * topRadius);
            }

            int bottomCenter = segments * 2;
            int topCenter = bottomCenter + 1;
            vertices[bottomCenter] = new Vector3(0f, -height * 0.5f, 0f);
            vertices[topCenter] = new Vector3(0f, height * 0.5f, 0f);

            int t = 0;
            for (int i = 0; i < segments; i++)
            {
                int n = (i + 1) % segments;
                triangles[t++] = i;
                triangles[t++] = i + segments;
                triangles[t++] = n + segments;
                triangles[t++] = i;
                triangles[t++] = n + segments;
                triangles[t++] = n;

                triangles[t++] = bottomCenter;
                triangles[t++] = n;
                triangles[t++] = i;

                triangles[t++] = topCenter;
                triangles[t++] = i + segments;
                triangles[t++] = n + segments;
            }

            Mesh mesh = new Mesh();
            mesh.name = "DOLZORE Bell Dress";
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            AssetDatabase.CreateAsset(mesh, path);
            return mesh;
        }

        private static Transform NewPivot(string name, Transform parent, Vector3 localPosition)
        {
            GameObject go = new GameObject(name);
            go.transform.SetParent(parent, false);
            go.transform.localPosition = localPosition;
            return go.transform;
        }

        private static GameObject Part(PrimitiveType type, string name, Transform parent, Vector3 localPosition, Vector3 localScale, Material material)
        {
            GameObject go = GameObject.CreatePrimitive(type);
            go.name = name;
            go.transform.SetParent(parent, false);
            go.transform.localPosition = localPosition;
            go.transform.localScale = localScale;
            Renderer renderer = go.GetComponent<Renderer>();
            if (renderer != null) renderer.sharedMaterial = material;
            Collider col = go.GetComponent<Collider>();
            if (col != null) UnityEngine.Object.DestroyImmediate(col);
            return go;
        }

        private static GameObject CreateMeshObject(string name, Transform parent, Mesh mesh, Vector3 localPosition, Vector3 localScale, Material material)
        {
            GameObject go = new GameObject(name);
            go.transform.SetParent(parent, false);
            go.transform.localPosition = localPosition;
            go.transform.localScale = localScale;
            MeshFilter filter = go.AddComponent<MeshFilter>();
            filter.sharedMesh = mesh;
            MeshRenderer renderer = go.AddComponent<MeshRenderer>();
            renderer.sharedMaterial = material;
            return go;
        }

        private static void AddStoneTile(Vector3 position, Vector3 scale, Material material)
        {
            GameObject tile = GameObject.CreatePrimitive(PrimitiveType.Cube);
            tile.name = "Stone Tile";
            tile.transform.position = position;
            tile.transform.localScale = scale;
            tile.GetComponent<Renderer>().sharedMaterial = material;
            UnityEngine.Object.DestroyImmediate(tile.GetComponent<Collider>());
        }

        private static void AddTree(Vector3 position, float scale)
        {
            GameObject trunk = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            trunk.name = "Tree";
            trunk.transform.position = position + Vector3.up * (1.0f * scale);
            trunk.transform.localScale = new Vector3(0.34f * scale, 1.0f * scale, 0.34f * scale);
            trunk.GetComponent<Renderer>().sharedMaterial = bark;
            UnityEngine.Object.DestroyImmediate(trunk.GetComponent<Collider>());

            GameObject crown = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            crown.name = "Tree Crown";
            crown.transform.position = position + Vector3.up * (2.55f * scale);
            crown.transform.localScale = new Vector3(1.35f * scale, 1.15f * scale, 1.25f * scale);
            crown.GetComponent<Renderer>().sharedMaterial = leaf;
            UnityEngine.Object.DestroyImmediate(crown.GetComponent<Collider>());
        }

        private static void AddCrystalPillar(Vector3 position)
        {
            GameObject baseGo = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            baseGo.name = "Crystal Pedestal";
            baseGo.transform.position = position + Vector3.up * 0.25f;
            baseGo.transform.localScale = new Vector3(0.55f, 0.25f, 0.55f);
            baseGo.GetComponent<Renderer>().sharedMaterial = stone;
            UnityEngine.Object.DestroyImmediate(baseGo.GetComponent<Collider>());

            GameObject gem = GameObject.CreatePrimitive(PrimitiveType.Cube);
            gem.name = "World Crystal";
            gem.transform.position = position + Vector3.up * 1.05f;
            gem.transform.rotation = Quaternion.Euler(35f, 45f, 35f);
            gem.transform.localScale = new Vector3(0.42f, 0.82f, 0.42f);
            gem.GetComponent<Renderer>().sharedMaterial = crystal;
            UnityEngine.Object.DestroyImmediate(gem.GetComponent<Collider>());

            Light light = new GameObject("Crystal Glow").AddComponent<Light>();
            light.transform.position = position + Vector3.up * 1.05f;
            light.type = LightType.Point;
            light.color = Hex("#61D9FF");
            light.intensity = 0.75f;
            light.range = 3.2f;
        }

        private static void AddBoundary(string name, Vector3 position, Vector3 scale)
        {
            GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
            wall.name = name;
            wall.transform.position = position;
            wall.transform.localScale = scale;
            wall.GetComponent<Renderer>().sharedMaterial = stone;
        }

        private static Material Mat(string name, Color color, float metallic, float smoothness)
        {
            return Mat(name, color, metallic, smoothness, Color.black);
        }

        private static Material Mat(string name, Color color, float metallic, float smoothness, Color emission)
        {
            string path = MaterialDir + "/" + name + ".mat";
            Material existing = AssetDatabase.LoadAssetAtPath<Material>(path);
            if (existing != null) return existing;

            Shader shader = Shader.Find("Standard");
            if (shader == null) shader = Shader.Find("Universal Render Pipeline/Lit");
            if (shader == null) throw new InvalidOperationException("WALK_DEMO_SHADER_NOT_FOUND");

            Material m = new Material(shader);
            m.name = name;
            m.color = color;
            if (m.HasProperty("_Metallic")) m.SetFloat("_Metallic", metallic);
            if (m.HasProperty("_Glossiness")) m.SetFloat("_Glossiness", smoothness);
            if (emission.maxColorComponent > 0.001f && m.HasProperty("_EmissionColor"))
            {
                m.EnableKeyword("_EMISSION");
                m.SetColor("_EmissionColor", emission);
            }
            AssetDatabase.CreateAsset(m, path);
            return m;
        }

        private static Material MatWithShader(string name, Color color, string shaderName)
        {
            string path = MaterialDir + "/" + name + ".mat";
            Material existing = AssetDatabase.LoadAssetAtPath<Material>(path);
            if (existing != null) return existing;

            Shader shader = Shader.Find(shaderName);
            if (shader == null) shader = Shader.Find("Sprites/Default");
            if (shader == null) throw new InvalidOperationException("WALK_DEMO_UNLIT_SHADER_NOT_FOUND");

            Material m = new Material(shader);
            m.name = name;
            m.color = color;
            AssetDatabase.CreateAsset(m, path);
            return m;
        }

        private static GameObject Panel(Transform parent, string name, Color color)
        {
            GameObject go = new GameObject(name, typeof(RectTransform));
            go.transform.SetParent(parent, false);
            Image image = go.AddComponent<Image>();
            image.color = color;
            return go;
        }

        private static Text TextUI(Transform parent, string name, string value, int size, Color color, TextAnchor anchor, FontStyle style)
        {
            GameObject go = new GameObject(name, typeof(RectTransform));
            go.transform.SetParent(parent, false);
            Text text = go.AddComponent<Text>();
            text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            text.text = value;
            text.fontSize = size;
            text.fontStyle = style;
            text.color = color;
            text.alignment = anchor;
            text.horizontalOverflow = HorizontalWrapMode.Overflow;
            text.verticalOverflow = VerticalWrapMode.Overflow;
            text.raycastTarget = false;
            return text;
        }

        private static void Stretch(RectTransform rt)
        {
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.offsetMin = Vector2.zero;
            rt.offsetMax = Vector2.zero;
        }

        private static Color Hex(string hex)
        {
            Color c;
            if (!ColorUtility.TryParseHtmlString(hex, out c))
                throw new ArgumentException("Invalid color: " + hex);
            return c;
        }

        private static string Abs(string relative)
        {
            string root = Directory.GetParent(Application.dataPath).FullName;
            return Path.GetFullPath(Path.Combine(root, relative.Replace('/', Path.DirectorySeparatorChar)));
        }
    }
}
