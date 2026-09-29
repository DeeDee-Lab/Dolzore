using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Dolzore.Editor
{
    public static class ProjectBootstrap
    {
        private const string TitleScene = "Assets/Scenes/Title.unity";
        private const string TownScene = "Assets/Scenes/FirstTownShell.unity";
        private const string BackgroundPath = "Assets/Art/Generated/title_background.png";
        private const string LogoPath = "Assets/Art/Generated/dolzore_logo.png";
        private const string MinimapPath = "Assets/Art/Generated/first_town_minimap.png";

        private static readonly Color Panel = Hex("#10182BDD");
        private static readonly Color PanelStrong = Hex("#0A1020F2");
        private static readonly Color Cream = Hex("#F5E9CE");
        private static readonly Color Cyan = Hex("#76D7D2");
        private static readonly Color Orange = Hex("#F3A65A");
        private static readonly Color Muted = Hex("#93A0B8");

        public static void BuildInitialSlice()
        {
            GenerateInitialSlice();
            BuildWebGLOnly();
        }

        public static void GenerateInitialSlice()
        {
            Debug.Log("DOLZORE_GENERATE_BEGIN");
            ConfigureProject();
            EnsureDirectories();
            GenerateBackground();
            GenerateLogo();
            GenerateMinimap();
            BuildTitleScene();
            BuildTownShellScene();

            EditorBuildSettings.scenes = new[]
            {
                new EditorBuildSettingsScene(TitleScene, true),
                new EditorBuildSettingsScene(TownScene, true)
            };

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            RenderPreview(TitleScene, "BuildArtifacts/initial-screen.png");
            Debug.Log("DOLZORE_GENERATE_SUCCESS");
        }

        public static void BuildWebGLOnly()
        {
            Debug.Log("DOLZORE_WEBGL_BEGIN");
            BuildWebGL();
            Debug.Log("DOLZORE_WEBGL_SUCCESS");
        }

        private static void ConfigureProject()
        {
            PlayerSettings.companyName = "DOLZORE";
            PlayerSettings.productName = "DOLZORE";
            PlayerSettings.bundleVersion = "0.1.0";
            PlayerSettings.defaultScreenWidth = 1920;
            PlayerSettings.defaultScreenHeight = 1080;
            PlayerSettings.runInBackground = true;
            PlayerSettings.colorSpace = ColorSpace.Linear;
            PlayerSettings.resizableWindow = true;
            PlayerSettings.WebGL.compressionFormat = WebGLCompressionFormat.Brotli;
            QualitySettings.vSyncCount = 1;
        }

        private static void EnsureDirectories()
        {
            Directory.CreateDirectory(Abs("Assets/Scenes"));
            Directory.CreateDirectory(Abs("Assets/Art/Generated"));
            Directory.CreateDirectory(Abs("BuildArtifacts"));
            Directory.CreateDirectory(Abs("Builds/WebGL"));
            AssetDatabase.Refresh();

            if (File.Exists(Abs("Assets/Scenes/SampleScene.unity")))
                AssetDatabase.DeleteAsset("Assets/Scenes/SampleScene.unity");
        }

        private static void BuildTitleScene()
        {
            Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            Camera cam = MakeCamera("Title Camera", Color.black);
            Canvas canvas = MakeCanvas(cam);

            Image bg = MakeImage(canvas.transform, "Night Town", LoadSprite(BackgroundPath), Color.white);
            Stretch(bg.rectTransform);
            bg.rectTransform.offsetMin = new Vector2(-12f, -8f);
            bg.rectTransform.offsetMax = new Vector2(12f, 8f);
            bg.raycastTarget = false;

            Image veil = MakeImage(canvas.transform, "Atmospheric Veil", null, new Color(0.04f, 0.05f, 0.11f, 0.20f));
            Stretch(veil.rectTransform);
            veil.raycastTarget = false;
            CanvasGroup haze = veil.gameObject.AddComponent<CanvasGroup>();

            GameObject atmosphereGo = new GameObject("Atmosphere");
            atmosphereGo.transform.SetParent(canvas.transform, false);
            TitleAtmosphere atmosphere = atmosphereGo.AddComponent<TitleAtmosphere>();
            atmosphere.background = bg.rectTransform;
            atmosphere.haze = haze;

            Image logo = MakeImage(canvas.transform, "DOLZORE Logo", LoadSprite(LogoPath), Color.white);
            SetAnchored(logo.rectTransform, new Vector2(0.28f, 0.80f), new Vector2(780f, 140f), Vector2.zero, new Vector2(0.5f, 0.5f));
            logo.preserveAspect = true;
            logo.raycastTarget = false;

            Text tagline = MakeText(canvas.transform, "Tagline", "BEYOND THE SHIFTED HOUR", 23, Cream, TextAnchor.MiddleCenter, FontStyle.Normal);
            SetAnchored(tagline.rectTransform, new Vector2(0.28f, 0.705f), new Vector2(640f, 48f), Vector2.zero, new Vector2(0.5f, 0.5f));
            Outline tagOutline = tagline.gameObject.AddComponent<Outline>();
            tagOutline.effectColor = new Color(0f, 0f, 0f, 0.7f);

            GameObject menuPanel = MakePanel(canvas.transform, "Main Menu", new Color(Panel.r, Panel.g, Panel.b, 0.88f));
            SetAnchored(menuPanel.GetComponent<RectTransform>(), new Vector2(0f, 0f), new Vector2(500f, 390f), new Vector2(96f, 86f), new Vector2(0f, 0f));
            AddOutline(menuPanel, Cyan, 2f);

            Text menuEyebrow = MakeText(menuPanel.transform, "Menu Eyebrow", "WORLD ACCESS", 16, Cyan, TextAnchor.MiddleLeft, FontStyle.Bold);
            SetAnchored(menuEyebrow.rectTransform, new Vector2(0f, 1f), new Vector2(400f, 32f), new Vector2(34f, -40f), new Vector2(0f, 1f));

            Text menuHint = MakeText(menuPanel.transform, "Menu Hint", "ARROWS / D-PAD   ENTER / A", 14, Muted, TextAnchor.MiddleLeft, FontStyle.Normal);
            SetAnchored(menuHint.rectTransform, new Vector2(0f, 1f), new Vector2(400f, 26f), new Vector2(34f, -72f), new Vector2(0f, 1f));

            Button newGame = MakeButton(menuPanel.transform, "NEW GAME", "NEW GAME", 22, 34f, -126f);
            Button cont = MakeButton(menuPanel.transform, "CONTINUE", "CONTINUE", 22, 34f, -196f);
            Button settings = MakeButton(menuPanel.transform, "SETTINGS", "SETTINGS", 22, 34f, -266f);
            Button bgm = MakeButton(menuPanel.transform, "BGM", "BGM  ON", 20, 34f, -336f);
            Text bgmLabel = bgm.GetComponentInChildren<Text>();
            cont.interactable = false;

            GameObject statusPanel = MakePanel(canvas.transform, "World Status", new Color(PanelStrong.r, PanelStrong.g, PanelStrong.b, 0.82f));
            SetAnchored(statusPanel.GetComponent<RectTransform>(), new Vector2(1f, 1f), new Vector2(420f, 136f), new Vector2(-76f, -60f), new Vector2(1f, 1f));
            AddOutline(statusPanel, new Color(Cyan.r, Cyan.g, Cyan.b, 0.65f), 1f);

            Text statusTop = MakeText(statusPanel.transform, "Status Top", "PRESENT  //  FIRST TOWN", 18, Cyan, TextAnchor.MiddleLeft, FontStyle.Bold);
            SetAnchored(statusTop.rectTransform, new Vector2(0f, 1f), new Vector2(360f, 30f), new Vector2(28f, -28f), new Vector2(0f, 1f));
            Text status = MakeText(statusPanel.transform, "Status", "WORLD 01  /  LINK READY", 24, Cream, TextAnchor.MiddleLeft, FontStyle.Bold);
            SetAnchored(status.rectTransform, new Vector2(0f, 1f), new Vector2(360f, 38f), new Vector2(28f, -62f), new Vector2(0f, 1f));
            Text coord = MakeText(statusPanel.transform, "Coordinate", "ZURE SIGNAL  00.13", 14, Muted, TextAnchor.MiddleLeft, FontStyle.Normal);
            SetAnchored(coord.rectTransform, new Vector2(0f, 1f), new Vector2(360f, 26f), new Vector2(28f, -102f), new Vector2(0f, 1f));

            Text footer = MakeText(canvas.transform, "Footer", "UNITY FIRST-TOWN VERTICAL SLICE  //  BUILD 0001", 13, Muted, TextAnchor.MiddleRight, FontStyle.Normal);
            SetAnchored(footer.rectTransform, new Vector2(1f, 0f), new Vector2(620f, 30f), new Vector2(-58f, 38f), new Vector2(1f, 0f));

            GameObject settingsPanel = BuildSettingsPanel(canvas.transform);
            settingsPanel.SetActive(false);
            Button closeSettings = settingsPanel.transform.Find("CLOSE").GetComponent<Button>();

            GameObject ambientGo = new GameObject("Original Ambient BGM");
            ambientGo.transform.SetParent(canvas.transform, false);
            AudioSource ambientSource = ambientGo.AddComponent<AudioSource>();
            ambientGo.AddComponent<DolzoreAmbientSynth>();

            Image fade = MakeImage(canvas.transform, "Fade", null, Color.black);
            Stretch(fade.rectTransform);
            fade.color = new Color(0f, 0f, 0f, 0f);
            fade.raycastTarget = false;
            fade.transform.SetAsLastSibling();

            GameObject controllerGo = new GameObject("Title Screen Controller");
            controllerGo.transform.SetParent(canvas.transform, false);
            TitleScreenController controller = controllerGo.AddComponent<TitleScreenController>();
            controller.newGameButton = newGame;
            controller.continueButton = cont;
            controller.settingsButton = settings;
            controller.bgmButton = bgm;
            controller.settingsCloseButton = closeSettings;
            controller.settingsPanel = settingsPanel;
            controller.bgmLabel = bgmLabel;
            controller.statusLabel = status;
            controller.fadeImage = fade;
            controller.ambientSource = ambientSource;

            MakeEventSystem();
            EditorSceneManager.SaveScene(scene, TitleScene);
        }

        private static GameObject BuildSettingsPanel(Transform parent)
        {
            GameObject panel = MakePanel(parent, "SETTINGS PANEL", new Color(PanelStrong.r, PanelStrong.g, PanelStrong.b, 0.97f));
            SetAnchored(panel.GetComponent<RectTransform>(), new Vector2(0.5f, 0.5f), new Vector2(660f, 430f), Vector2.zero, new Vector2(0.5f, 0.5f));
            AddOutline(panel, Cyan, 2f);

            Text title = MakeText(panel.transform, "Settings Title", "SETTINGS", 34, Cream, TextAnchor.MiddleLeft, FontStyle.Bold);
            SetAnchored(title.rectTransform, new Vector2(0f, 1f), new Vector2(540f, 54f), new Vector2(44f, -42f), new Vector2(0f, 1f));

            string[] rows =
            {
                "DISPLAY   PIXEL / CRISP",
                "AUDIO     ORIGINAL SYNTH",
                "INPUT     KEYBOARD / GAMEPAD",
                "TOUCH     UI PATH READY"
            };

            for (int i = 0; i < rows.Length; i++)
            {
                Text row = MakeText(panel.transform, "Row " + i, rows[i], 18, i == 0 ? Cyan : Muted, TextAnchor.MiddleLeft, FontStyle.Normal);
                SetAnchored(row.rectTransform, new Vector2(0f, 1f), new Vector2(540f, 42f), new Vector2(44f, -118f - i * 54f), new Vector2(0f, 1f));
            }

            Button close = MakeButton(panel.transform, "CLOSE", "CLOSE", 20, 44f, -362f);
            close.GetComponent<RectTransform>().sizeDelta = new Vector2(240f, 56f);
            return panel;
        }

        private static void BuildTownShellScene()
        {
            Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            Camera cam = MakeCamera("First Town Camera", Hex("#0A1020"));
            Canvas canvas = MakeCanvas(cam);

            Image bg = MakeImage(canvas.transform, "Town Backdrop", LoadSprite(BackgroundPath), new Color(0.86f, 0.90f, 1f, 1f));
            Stretch(bg.rectTransform);
            bg.rectTransform.offsetMin = new Vector2(-20f, -12f);
            bg.rectTransform.offsetMax = new Vector2(20f, 12f);
            bg.raycastTarget = false;

            Image tint = MakeImage(canvas.transform, "Town Tint", null, new Color(0.02f, 0.04f, 0.08f, 0.25f));
            Stretch(tint.rectTransform);
            tint.raycastTarget = false;

            GameObject titlePanel = MakePanel(canvas.transform, "Zone Header", new Color(PanelStrong.r, PanelStrong.g, PanelStrong.b, 0.88f));
            SetAnchored(titlePanel.GetComponent<RectTransform>(), new Vector2(0f, 1f), new Vector2(560f, 104f), new Vector2(52f, -48f), new Vector2(0f, 1f));
            AddOutline(titlePanel, Cyan, 1f);
            Text zone = MakeText(titlePanel.transform, "Zone", "FIRST TOWN", 28, Cream, TextAnchor.MiddleLeft, FontStyle.Bold);
            SetAnchored(zone.rectTransform, new Vector2(0f, 1f), new Vector2(480f, 38f), new Vector2(26f, -22f), new Vector2(0f, 1f));
            Text district = MakeText(titlePanel.transform, "District", "RESIDENTIAL HILL  //  PRESENT", 15, Cyan, TextAnchor.MiddleLeft, FontStyle.Bold);
            SetAnchored(district.rectTransform, new Vector2(0f, 1f), new Vector2(480f, 26f), new Vector2(26f, -62f), new Vector2(0f, 1f));

            GameObject hud = MakePanel(canvas.transform, "Player HUD", new Color(PanelStrong.r, PanelStrong.g, PanelStrong.b, 0.91f));
            SetAnchored(hud.GetComponent<RectTransform>(), new Vector2(0f, 0f), new Vector2(560f, 190f), new Vector2(52f, 48f), new Vector2(0f, 0f));
            AddOutline(hud, new Color(Cyan.r, Cyan.g, Cyan.b, 0.75f), 1f);

            Text name = MakeText(hud.transform, "Player Name", "SORA", 26, Cream, TextAnchor.MiddleLeft, FontStyle.Bold);
            SetAnchored(name.rectTransform, new Vector2(0f, 1f), new Vector2(250f, 40f), new Vector2(26f, -22f), new Vector2(0f, 1f));
            Text meta = MakeText(hud.transform, "Meta", "LV 01   JOB  WANDERER", 14, Muted, TextAnchor.MiddleLeft, FontStyle.Bold);
            SetAnchored(meta.rectTransform, new Vector2(0f, 1f), new Vector2(360f, 26f), new Vector2(26f, -60f), new Vector2(0f, 1f));

            MakeBar(hud.transform, "HEART", 26f, -103f, 0.82f, Orange);
            MakeBar(hud.transform, "FOCUS", 26f, -145f, 0.64f, Cyan);

            GameObject mapPanel = MakePanel(canvas.transform, "Minimap Panel", new Color(PanelStrong.r, PanelStrong.g, PanelStrong.b, 0.91f));
            SetAnchored(mapPanel.GetComponent<RectTransform>(), new Vector2(1f, 1f), new Vector2(380f, 300f), new Vector2(-48f, -48f), new Vector2(1f, 1f));
            AddOutline(mapPanel, Cyan, 1f);
            Text mapTitle = MakeText(mapPanel.transform, "Map Title", "TOWN MAP", 16, Cyan, TextAnchor.MiddleLeft, FontStyle.Bold);
            SetAnchored(mapTitle.rectTransform, new Vector2(0f, 1f), new Vector2(300f, 26f), new Vector2(22f, -18f), new Vector2(0f, 1f));
            Image map = MakeImage(mapPanel.transform, "Map", LoadSprite(MinimapPath), Color.white);
            SetAnchored(map.rectTransform, new Vector2(0.5f, 0.5f), new Vector2(320f, 220f), new Vector2(0f, -22f), new Vector2(0.5f, 0.5f));
            map.preserveAspect = true;

            GameObject promptPanel = MakePanel(canvas.transform, "Interaction Prompt", new Color(Panel.r, Panel.g, Panel.b, 0.90f));
            SetAnchored(promptPanel.GetComponent<RectTransform>(), new Vector2(0.5f, 0f), new Vector2(580f, 68f), new Vector2(0f, 48f), new Vector2(0.5f, 0f));
            AddOutline(promptPanel, Orange, 1f);
            Text prompt = MakeText(promptPanel.transform, "Prompt", "ENTER   EXPLORE FIRST TOWN", 18, Cream, TextAnchor.MiddleCenter, FontStyle.Bold);
            Stretch(prompt.rectTransform);

            Text clock = MakeText(canvas.transform, "World Clock", "PRESENT  //  00:13", 14, Muted, TextAnchor.MiddleRight, FontStyle.Bold);
            SetAnchored(clock.rectTransform, new Vector2(1f, 0f), new Vector2(320f, 30f), new Vector2(-44f, 28f), new Vector2(1f, 0f));

            GameObject marker = MakePanel(canvas.transform, "Player Marker", Cyan);
            SetAnchored(marker.GetComponent<RectTransform>(), new Vector2(0.55f, 0.49f), new Vector2(22f, 22f), Vector2.zero, new Vector2(0.5f, 0.5f));
            marker.GetComponent<RectTransform>().localRotation = Quaternion.Euler(0f, 0f, 45f);
            AddOutline(marker, Cream, 2f);

            GameObject controllerGo = new GameObject("First Town Shell Controller");
            controllerGo.transform.SetParent(canvas.transform, false);
            FirstTownShellController controller = controllerGo.AddComponent<FirstTownShellController>();
            controller.clockLabel = clock;

            MakeEventSystem();
            EditorSceneManager.SaveScene(scene, TownScene);
        }

        private static void MakeBar(Transform parent, string label, float x, float y, float fill, Color color)
        {
            Text text = MakeText(parent, label + " Label", label, 13, Muted, TextAnchor.MiddleLeft, FontStyle.Bold);
            SetAnchored(text.rectTransform, new Vector2(0f, 1f), new Vector2(90f, 26f), new Vector2(x, y), new Vector2(0f, 1f));

            GameObject track = MakePanel(parent, label + " Track", new Color(0.15f, 0.18f, 0.25f, 0.90f));
            SetAnchored(track.GetComponent<RectTransform>(), new Vector2(0f, 1f), new Vector2(370f, 16f), new Vector2(x + 100f, y - 5f), new Vector2(0f, 1f));

            GameObject value = MakePanel(track.transform, label + " Value", color);
            RectTransform rt = value.GetComponent<RectTransform>();
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = new Vector2(fill, 1f);
            rt.offsetMin = new Vector2(2f, 2f);
            rt.offsetMax = new Vector2(-2f, -2f);
        }

        private static void MakeEventSystem()
        {
            GameObject go = new GameObject("EventSystem");
            go.AddComponent<EventSystem>();
            InputSystemUIInputModule module = go.AddComponent<InputSystemUIInputModule>();
            module.AssignDefaultActions();
        }

        private static Camera MakeCamera(string name, Color color)
        {
            GameObject go = new GameObject(name);
            go.tag = "MainCamera";
            Camera cam = go.AddComponent<Camera>();
            cam.clearFlags = CameraClearFlags.SolidColor;
            cam.backgroundColor = color;
            cam.orthographic = true;
            cam.orthographicSize = 5f;
            cam.nearClipPlane = 0.1f;
            cam.farClipPlane = 100f;
            return cam;
        }

        private static Canvas MakeCanvas(Camera camera)
        {
            GameObject go = new GameObject("Canvas");
            Canvas canvas = go.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceCamera;
            canvas.worldCamera = camera;
            canvas.planeDistance = 10f;

            CanvasScaler scaler = go.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920f, 1080f);
            scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            scaler.matchWidthOrHeight = 0.5f;
            go.AddComponent<GraphicRaycaster>();
            return canvas;
        }

        private static GameObject MakePanel(Transform parent, string name, Color color)
        {
            GameObject go = NewUI(name, parent);
            Image image = go.AddComponent<Image>();
            image.color = color;
            return go;
        }

        private static Image MakeImage(Transform parent, string name, Sprite sprite, Color color)
        {
            GameObject go = NewUI(name, parent);
            Image image = go.AddComponent<Image>();
            image.sprite = sprite;
            image.color = color;
            return image;
        }

        private static Text MakeText(Transform parent, string name, string value, int size, Color color, TextAnchor anchor, FontStyle style)
        {
            GameObject go = NewUI(name, parent);
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

        private static Button MakeButton(Transform parent, string name, string label, int fontSize, float x, float y)
        {
            GameObject go = NewUI(name, parent);
            RectTransform rt = go.GetComponent<RectTransform>();
            rt.anchorMin = new Vector2(0f, 1f);
            rt.anchorMax = new Vector2(0f, 1f);
            rt.pivot = new Vector2(0f, 1f);
            rt.sizeDelta = new Vector2(430f, 56f);
            rt.anchoredPosition = new Vector2(x, y);

            Image image = go.AddComponent<Image>();
            image.color = Hex("#17233AEF");

            Button button = go.AddComponent<Button>();
            ColorBlock colors = button.colors;
            colors.normalColor = Hex("#17233AEF");
            colors.highlightedColor = Hex("#25445BEF");
            colors.selectedColor = Hex("#25445BEF");
            colors.pressedColor = Hex("#E08F45F5");
            colors.disabledColor = Hex("#111725A8");
            colors.colorMultiplier = 1f;
            colors.fadeDuration = 0.08f;
            button.colors = colors;

            Outline outline = go.AddComponent<Outline>();
            outline.effectColor = new Color(Cyan.r, Cyan.g, Cyan.b, 0.38f);
            outline.effectDistance = new Vector2(1f, -1f);

            Text text = MakeText(go.transform, "Label", label, fontSize, Cream, TextAnchor.MiddleLeft, FontStyle.Bold);
            Stretch(text.rectTransform);
            text.rectTransform.offsetMin = new Vector2(22f, 0f);
            text.rectTransform.offsetMax = new Vector2(-12f, 0f);
            return button;
        }

        private static GameObject NewUI(string name, Transform parent)
        {
            GameObject go = new GameObject(name, typeof(RectTransform));
            go.transform.SetParent(parent, false);
            return go;
        }

        private static void Stretch(RectTransform rt)
        {
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.pivot = new Vector2(0.5f, 0.5f);
            rt.offsetMin = Vector2.zero;
            rt.offsetMax = Vector2.zero;
        }

        private static void SetAnchored(RectTransform rt, Vector2 anchor, Vector2 size, Vector2 position, Vector2 pivot)
        {
            rt.anchorMin = anchor;
            rt.anchorMax = anchor;
            rt.pivot = pivot;
            rt.sizeDelta = size;
            rt.anchoredPosition = position;
        }

        private static void AddOutline(GameObject go, Color color, float distance)
        {
            Outline outline = go.AddComponent<Outline>();
            outline.effectColor = color;
            outline.effectDistance = new Vector2(distance, -distance);
        }

        private static void GenerateBackground()
        {
            const int width = 960;
            const int height = 540;
            Color32[] p = new Color32[width * height];
            Color32 top = To32(Hex("#070D1D"));
            Color32 mid = To32(Hex("#171A38"));
            Color32 low = To32(Hex("#3B2A4C"));

            for (int y = 0; y < height; y++)
            {
                float t = (float)y / (height - 1);
                Color32 c = t < 0.58f ? Lerp32(low, mid, t / 0.58f) : Lerp32(mid, top, (t - 0.58f) / 0.42f);
                for (int x = 0; x < width; x++) p[y * width + x] = c;
            }

            System.Random rng = new System.Random(13013);
            for (int i = 0; i < 190; i++)
            {
                int x = rng.Next(0, width);
                int y = rng.Next(220, height - 18);
                int s = rng.NextDouble() > 0.86 ? 2 : 1;
                DrawRect(p, width, height, x, y, s, s, To32(rng.NextDouble() > 0.72 ? Cyan : Cream));
            }

            DrawCircle(p, width, height, 760, 416, 44, To32(Hex("#F7E7B8")));
            DrawCircle(p, width, height, 775, 428, 39, To32(Hex("#10152B")));

            Color32 farHill = To32(Hex("#11172E"));
            Color32 nearHill = To32(Hex("#0B1224"));
            for (int x = 0; x < width; x++)
            {
                int h1 = 168 + Mathf.RoundToInt(Mathf.Sin(x * 0.012f) * 18f + Mathf.Sin(x * 0.003f) * 28f);
                for (int y = 112; y < h1; y++) p[y * width + x] = farHill;
                int h2 = 125 + Mathf.RoundToInt(Mathf.Sin(x * 0.019f + 1.3f) * 14f);
                if (x < 390) h2 += Mathf.RoundToInt((390 - x) * 0.18f);
                for (int y = 88; y < h2; y++) p[y * width + x] = nearHill;
            }

            DrawRect(p, width, height, 0, 40, width, 66, To32(Hex("#0B2941")));
            for (int y = 50; y < 98; y += 8)
            {
                for (int x = 22; x < width - 22; x += 40 + rng.Next(0, 18))
                    DrawRect(p, width, height, x, y, 10 + rng.Next(8, 30), 2, To32(new Color(0.20f, 0.55f, 0.68f, 0.35f)));
            }

            int cursor = 438;
            Color32 buildingA = To32(Hex("#17233B"));
            Color32 buildingB = To32(Hex("#20314B"));
            Color32 roof = To32(Hex("#0D1629"));
            Color32 window = To32(Hex("#F3A65A"));

            for (int i = 0; i < 15; i++)
            {
                int w = rng.Next(30, 56);
                int h = rng.Next(58, 132);
                int baseY = 112 + rng.Next(-6, 10);
                DrawRect(p, width, height, cursor, baseY, w, h, i % 3 == 0 ? buildingB : buildingA);
                DrawRect(p, width, height, cursor - 3, baseY + h, w + 6, 7, roof);

                for (int wy = baseY + 18; wy < baseY + h - 12; wy += 22)
                    for (int wx = cursor + 8; wx < cursor + w - 8; wx += 17)
                        if (rng.NextDouble() > 0.38) DrawRect(p, width, height, wx, wy, 5, 8, window);

                cursor += w + rng.Next(6, 15);
                if (cursor > 930) break;
            }

            DrawRect(p, width, height, 690, 118, 72, 176, To32(Hex("#1D3047")));
            DrawRect(p, width, height, 681, 286, 90, 12, roof);
            DrawRect(p, width, height, 710, 298, 32, 68, To32(Hex("#17243A")));
            DrawCircle(p, width, height, 726, 325, 13, To32(Cream));
            DrawCircle(p, width, height, 726, 325, 9, To32(Hex("#20314B")));
            DrawRect(p, width, height, 724, 325, 3, 8, To32(Cream));
            DrawRect(p, width, height, 726, 323, 7, 3, To32(Cream));

            DrawRect(p, width, height, 528, 84, 132, 10, To32(Hex("#2C3748")));
            DrawRect(p, width, height, 542, 62, 10, 34, To32(Hex("#2C3748")));
            DrawRect(p, width, height, 632, 62, 10, 34, To32(Hex("#2C3748")));

            for (int y = 0; y < height; y += 2)
                for (int x = 0; x < width; x++)
                {
                    int idx = y * width + x;
                    Color32 c = p[idx];
                    p[idx] = new Color32((byte)(c.r * 0.94f), (byte)(c.g * 0.94f), (byte)(c.b * 0.94f), c.a);
                }

            SaveTexture(BackgroundPath, width, height, p);
        }

        private static void GenerateLogo()
        {
            const int width = 820;
            const int height = 150;
            Color32[] p = new Color32[width * height];
            for (int i = 0; i < p.Length; i++) p[i] = new Color32(0, 0, 0, 0);

            Dictionary<char, string[]> glyphs = new Dictionary<char, string[]>
            {
                ['D'] = new[] {"11110","10001","10001","10001","10001","10001","11110"},
                ['O'] = new[] {"01110","10001","10001","10001","10001","10001","01110"},
                ['L'] = new[] {"10000","10000","10000","10000","10000","10000","11111"},
                ['Z'] = new[] {"11111","00001","00010","00100","01000","10000","11111"},
                ['R'] = new[] {"11110","10001","10001","11110","10100","10010","10001"},
                ['E'] = new[] {"11111","10000","10000","11110","10000","10000","11111"}
            };

            string word = "DOLZORE";
            int scale = 18;
            int glyphW = 5 * scale;
            int gap = 14;
            int total = word.Length * glyphW + (word.Length - 1) * gap;
            int startX = (width - total) / 2;
            int startY = 12;

            for (int i = 0; i < word.Length; i++)
            {
                char ch = word[i];
                string[] rows = glyphs[ch];
                Color32 main = To32(ch == 'Z' ? Orange : Cream);
                Color32 shadow = To32(Hex("#29475B"));
                int gx = startX + i * (glyphW + gap);

                for (int row = 0; row < 7; row++)
                    for (int col = 0; col < 5; col++)
                    {
                        if (rows[row][col] != '1') continue;
                        int px = gx + col * scale;
                        int py = startY + (6 - row) * scale;
                        DrawRect(p, width, height, px + 6, py - 6, scale - 2, scale - 2, shadow);
                        DrawRect(p, width, height, px, py, scale - 2, scale - 2, main);
                    }
            }

            SaveTexture(LogoPath, width, height, p);
        }

        private static void GenerateMinimap()
        {
            const int width = 320;
            const int height = 220;
            Color32[] p = new Color32[width * height];
            Color32 bg = To32(Hex("#0B1323"));
            for (int i = 0; i < p.Length; i++) p[i] = bg;

            Color32 road = To32(Hex("#354459"));
            Color32 water = To32(Hex("#154B63"));
            Color32 land = To32(Hex("#1A2A32"));
            DrawRect(p, width, height, 14, 14, 292, 192, land);
            DrawRect(p, width, height, 14, 78, 292, 36, water);
            DrawRect(p, width, height, 140, 18, 28, 188, road);
            DrawRect(p, width, height, 38, 145, 236, 24, road);
            DrawRect(p, width, height, 64, 34, 24, 136, road);
            DrawRect(p, width, height, 210, 100, 24, 98, road);
            DrawRect(p, width, height, 132, 80, 44, 32, To32(Hex("#66717D")));
            DrawRect(p, width, height, 48, 160, 12, 12, To32(Orange));
            DrawRect(p, width, height, 196, 162, 12, 12, To32(Cyan));
            DrawRect(p, width, height, 248, 130, 12, 12, To32(Orange));
            DrawRect(p, width, height, 258, 50, 12, 12, To32(Cyan));
            DrawCircle(p, width, height, 156, 157, 6, To32(Cream));
            SaveTexture(MinimapPath, width, height, p);
        }

        private static void SaveTexture(string assetPath, int width, int height, Color32[] pixels)
        {
            Texture2D tex = new Texture2D(width, height, TextureFormat.RGBA32, false);
            tex.SetPixels32(pixels);
            tex.Apply(false, false);
            File.WriteAllBytes(Abs(assetPath), tex.EncodeToPNG());
            UnityEngine.Object.DestroyImmediate(tex);

            AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceUpdate);
            TextureImporter importer = AssetImporter.GetAtPath(assetPath) as TextureImporter;
            if (importer != null)
            {
                importer.textureType = TextureImporterType.Sprite;
                importer.spriteImportMode = SpriteImportMode.Single;
                importer.mipmapEnabled = false;
                importer.filterMode = FilterMode.Point;
                importer.textureCompression = TextureImporterCompression.Uncompressed;
                importer.wrapMode = TextureWrapMode.Clamp;
                importer.alphaIsTransparency = true;
                importer.spritePixelsPerUnit = 1f;
                importer.SaveAndReimport();
            }
        }

        private static Sprite LoadSprite(string path)
        {
            Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path);
            if (sprite == null) throw new InvalidOperationException("Missing generated sprite: " + path);
            return sprite;
        }

        private static void RenderPreview(string scenePath, string outputRelative)
        {
            EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
            Camera cam = Camera.main;
            if (cam == null) throw new InvalidOperationException("Preview camera missing.");

            const int width = 1920;
            const int height = 1080;
            RenderTexture rt = new RenderTexture(width, height, 24, RenderTextureFormat.ARGB32);
            Texture2D tex = new Texture2D(width, height, TextureFormat.RGB24, false);
            cam.targetTexture = rt;
            RenderTexture.active = rt;
            Canvas.ForceUpdateCanvases();
            cam.Render();
            tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
            tex.Apply();

            string abs = Abs(outputRelative);
            Directory.CreateDirectory(Path.GetDirectoryName(abs));
            File.WriteAllBytes(abs, tex.EncodeToPNG());

            cam.targetTexture = null;
            RenderTexture.active = null;
            UnityEngine.Object.DestroyImmediate(rt);
            UnityEngine.Object.DestroyImmediate(tex);
            Debug.Log("DOLZORE_PREVIEW=" + abs);
        }

        private static void BuildWebGL()
        {
            string output = Abs("Builds/WebGL");
            Directory.CreateDirectory(output);
            BuildPlayerOptions options = new BuildPlayerOptions
            {
                scenes = new[] { TitleScene, TownScene },
                locationPathName = output,
                target = BuildTarget.WebGL,
                options = BuildOptions.None
            };

            BuildReport report = BuildPipeline.BuildPlayer(options);
            if (report.summary.result != BuildResult.Succeeded)
                throw new InvalidOperationException("WebGL build failed: " + report.summary.result);

            Debug.Log("DOLZORE_WEBGL_SIZE=" + report.summary.totalSize);
        }

        private static string Abs(string relative)
        {
            string root = Directory.GetParent(Application.dataPath).FullName;
            return Path.GetFullPath(Path.Combine(root, relative.Replace('/', Path.DirectorySeparatorChar)));
        }

        private static Color Hex(string hex)
        {
            Color c;
            if (!ColorUtility.TryParseHtmlString(hex, out c))
                throw new ArgumentException("Invalid color: " + hex);
            return c;
        }

        private static Color32 To32(Color c) { return (Color32)c; }

        private static Color32 Lerp32(Color32 a, Color32 b, float t)
        {
            t = Mathf.Clamp01(t);
            return new Color32(
                (byte)Mathf.RoundToInt(Mathf.Lerp(a.r, b.r, t)),
                (byte)Mathf.RoundToInt(Mathf.Lerp(a.g, b.g, t)),
                (byte)Mathf.RoundToInt(Mathf.Lerp(a.b, b.b, t)),
                (byte)Mathf.RoundToInt(Mathf.Lerp(a.a, b.a, t)));
        }

        private static void DrawRect(Color32[] p, int width, int height, int x, int y, int w, int h, Color32 color)
        {
            int x0 = Mathf.Clamp(x, 0, width);
            int x1 = Mathf.Clamp(x + w, 0, width);
            int y0 = Mathf.Clamp(y, 0, height);
            int y1 = Mathf.Clamp(y + h, 0, height);
            for (int yy = y0; yy < y1; yy++)
                for (int xx = x0; xx < x1; xx++)
                    p[yy * width + xx] = AlphaBlend(p[yy * width + xx], color);
        }

        private static void DrawCircle(Color32[] p, int width, int height, int cx, int cy, int radius, Color32 color)
        {
            int rr = radius * radius;
            for (int y = -radius; y <= radius; y++)
                for (int x = -radius; x <= radius; x++)
                {
                    if (x * x + y * y > rr) continue;
                    int px = cx + x;
                    int py = cy + y;
                    if (px < 0 || px >= width || py < 0 || py >= height) continue;
                    p[py * width + px] = AlphaBlend(p[py * width + px], color);
                }
        }

        private static Color32 AlphaBlend(Color32 dst, Color32 src)
        {
            if (src.a == 255) return src;
            float a = src.a / 255f;
            return new Color32(
                (byte)Mathf.RoundToInt(dst.r * (1f - a) + src.r * a),
                (byte)Mathf.RoundToInt(dst.g * (1f - a) + src.g * a),
                (byte)Mathf.RoundToInt(dst.b * (1f - a) + src.b * a),
                255);
        }
    }
}
