using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

namespace Dolzore.Editor
{
    public static class FirstTownBuilder
    {
        private const string ArtRoot = "Assets/Art/Generated/Town";
        private const string TileRoot = "Assets/Tiles/Generated";

        private static readonly Color Cream = Hex("#F5E9CE");
        private static readonly Color Cyan = Hex("#76D7D2");
        private static readonly Color Orange = Hex("#F3A65A");
        private static readonly Color Ink = Hex("#0A1020");
        private static readonly Color Panel = Hex("#0D1728E8");
        private static readonly Color Muted = Hex("#94A0B8");
        private static List<InteractionAnchor> ActiveLandmarkAnchors = new List<InteractionAnchor>();

        public static void Build(string scenePath)
        {
            EnsureDirectories();
            GenerateTownArt();
            GenerateTownMinimap();
            ActiveLandmarkAnchors = new List<InteractionAnchor>();

            Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            Camera camera = BuildCamera();

            Grid grid = new GameObject("Town Grid", typeof(Grid)).GetComponent<Grid>();
            grid.cellSize = Vector3.one;

            Tilemap ground = MakeTilemap(grid.transform, "Ground", 0);
            Tilemap roads = MakeTilemap(grid.transform, "Roads", 1);
            Tilemap water = MakeTilemap(grid.transform, "River", 2);
            Tilemap details = MakeTilemap(grid.transform, "Plazas", 3);

            Tile grass = MakeTile("Grass", ArtRoot + "/tile_grass.png");
            Tile road = MakeTile("Road", ArtRoot + "/tile_road.png");
            Tile sidewalk = MakeTile("Sidewalk", ArtRoot + "/tile_sidewalk.png");
            Tile river = MakeTile("Water", ArtRoot + "/tile_water.png");
            Tile plaza = MakeTile("Plaza", ArtRoot + "/tile_plaza.png");

            FillRect(ground, grass, -24, -17, 48, 34);
            FillRiver(water, river);
            DrawRoadPlan(roads, road, sidewalk);
            DrawPlazas(details, plaza);

            List<Transform> landmarks = new List<Transform>();
            List<string> landmarkNames = new List<string>();
            List<InteractionAnchor> landmarkAnchors = ActiveLandmarkAnchors;

            AddBuilding("HOME / RESIDENTIAL", "house_blue.png", new Vector2(-15f, 12.5f), new Vector2(3.4f, 2.3f), "#5B7CA6", landmarks, landmarkNames, "HOME");
            AddBuilding("HOUSE / NORTH", "house_cream.png", new Vector2(-8.5f, 13f), new Vector2(3.2f, 2.2f), "#B79268", null, null, null);
            AddBuilding("CAFE LUMA", "cafe.png", new Vector2(8.5f, 12.5f), new Vector2(3.6f, 2.4f), "#A85F56", landmarks, landmarkNames, "CAFE LUMA");
            AddBuilding("HOUSE / EAST", "house_green.png", new Vector2(15f, 12.5f), new Vector2(3.3f, 2.3f), "#5F866B", null, null, null);

            AddBuilding("BAR 13", "bar.png", new Vector2(-14.5f, 5.6f), new Vector2(4.0f, 2.6f), "#7C4D6B", landmarks, landmarkNames, "ENTER BAR 13");
            AddBuilding("MARKET HALL", "market.png", new Vector2(-8f, 4.7f), new Vector2(4.4f, 2.7f), "#A66A47", landmarks, landmarkNames, "MARKET HALL");
            AddBuilding("JOURNAL", "journal.png", new Vector2(10.5f, 5.7f), new Vector2(4.1f, 2.6f), "#436C87", landmarks, landmarkNames, "DOLZORE JOURNAL");
            AddBuilding("CIVIC CLOCK", "civic.png", new Vector2(16f, 5.2f), new Vector2(3.6f, 2.5f), "#68738B", null, null, null);

            AddBuilding("WORKSHOP", "workshop.png", new Vector2(-15.5f, -0.7f), new Vector2(4.1f, 2.5f), "#6D645F", null, null, null);
            AddBuilding("RIVERSIDE KIOSK", "kiosk.png", new Vector2(11.5f, -0.3f), new Vector2(3.1f, 2.0f), "#4E7C75", landmarks, landmarkNames, "RIVERSIDE");
            AddBuilding("STATION", "station.png", new Vector2(15f, -12.5f), new Vector2(5.2f, 2.8f), "#43546E", landmarks, landmarkNames, "EAST STATION");
            AddBuilding("BACK ALLEY DEPOT", "depot.png", new Vector2(-15.5f, -12.5f), new Vector2(4.4f, 2.6f), "#51465D", null, null, null);

            AddProps();
            AddBridge();

            AddWorldBlocker("West Boundary", new Vector2(-24.7f, 0f), new Vector2(1f, 35f));
            AddWorldBlocker("East Boundary", new Vector2(24.7f, 0f), new Vector2(1f, 35f));
            AddWorldBlocker("North Boundary", new Vector2(0f, 17.7f), new Vector2(50f, 1f));
            AddWorldBlocker("South Boundary", new Vector2(0f, -17.7f), new Vector2(50f, 1f));
            AddWorldBlocker("River West", new Vector2(-13.5f, -4.5f), new Vector2(21f, 3.2f));
            AddWorldBlocker("River East", new Vector2(13.5f, -4.5f), new Vector2(21f, 3.2f));

            GameObject playerGo = BuildPlayer();
            camera.transform.position = new Vector3(0f, 5.4f, -10f);
            TownCameraFollow follow = camera.gameObject.AddComponent<TownCameraFollow>();
            follow.target = playerGo.transform;
            follow.minBounds = new Vector2(-14.5f, -8.5f);
            follow.maxBounds = new Vector2(14.5f, 8.5f);

            Canvas canvas = BuildHudCanvas(camera);
            Text playerNameLabel;
            Text metaLabel;
            Text districtLabel;
            Text statusTargetLabel;
            Text promptLabel;
            RectTransform marker;
            RectTransform mapRect;
            BuildHud(
                canvas.transform,
                out playerNameLabel,
                out metaLabel,
                out districtLabel,
                out statusTargetLabel,
                out promptLabel,
                out marker,
                out mapRect);

            GameObject runtimeGo = new GameObject("First Town Runtime");
            WorldStateService worldState = runtimeGo.AddComponent<WorldStateService>();
            FirstTownRuntime runtime = runtimeGo.AddComponent<FirstTownRuntime>();
            runtime.player = playerGo.GetComponent<DolzorePlayerController>();
            runtime.playerState = playerGo.GetComponent<PlayerStateComponent>();
            runtime.worldState = worldState;
            runtime.playerNameLabel = playerNameLabel;
            runtime.metaLabel = metaLabel;
            runtime.districtLabel = districtLabel;
            runtime.statusTargetLabel = statusTargetLabel;
            runtime.interactionLabel = promptLabel;
            runtime.minimapMarker = marker;
            runtime.minimapRect = mapRect;
            runtime.landmarks = landmarks.ToArray();
            runtime.landmarkNames = landmarkNames.ToArray();
            runtime.landmarkAnchors = landmarkAnchors.ToArray();

            FirstTownShellController shell = runtimeGo.AddComponent<FirstTownShellController>();
            shell.clockLabel = canvas.transform.Find("World Clock")?.GetComponent<Text>();

            FirstTownVisualEnhancer.Apply();
            MakeEventSystem();
            EditorSceneManager.SaveScene(scene, scenePath);
        }

        private static Camera BuildCamera()
        {
            GameObject go = new GameObject("First Town Camera");
            go.tag = "MainCamera";
            Camera cam = go.AddComponent<Camera>();
            cam.clearFlags = CameraClearFlags.SolidColor;
            cam.backgroundColor = Hex("#0A1322");
            cam.orthographic = true;
            cam.orthographicSize = 8.5f;
            cam.nearClipPlane = 0.1f;
            cam.farClipPlane = 100f;
            return cam;
        }

        private static Tilemap MakeTilemap(Transform parent, string name, int order)
        {
            GameObject go = new GameObject(name, typeof(Tilemap), typeof(TilemapRenderer));
            go.transform.SetParent(parent, false);
            TilemapRenderer renderer = go.GetComponent<TilemapRenderer>();
            renderer.sortingOrder = order;
            return go.GetComponent<Tilemap>();
        }

        private static Tile MakeTile(string name, string spritePath)
        {
            string assetPath = TileRoot + "/" + name + ".asset";
            Tile existing = AssetDatabase.LoadAssetAtPath<Tile>(assetPath);
            if (existing != null) AssetDatabase.DeleteAsset(assetPath);

            Tile tile = ScriptableObject.CreateInstance<Tile>();
            tile.name = name;
            tile.sprite = AssetDatabase.LoadAssetAtPath<Sprite>(spritePath);
            tile.colliderType = Tile.ColliderType.None;
            AssetDatabase.CreateAsset(tile, assetPath);
            return tile;
        }

        private static void FillRect(Tilemap map, Tile tile, int x, int y, int w, int h)
        {
            for (int yy = y; yy < y + h; yy++)
                for (int xx = x; xx < x + w; xx++)
                    map.SetTile(new Vector3Int(xx, yy, 0), tile);
        }

        private static void FillRiver(Tilemap map, Tile water)
        {
            for (int y = -6; y <= -3; y++)
                for (int x = -24; x < 24; x++)
                    map.SetTile(new Vector3Int(x, y, 0), water);
        }

        private static void DrawRoadPlan(Tilemap roads, Tile road, Tile sidewalk)
        {
            FillRect(roads, road, -2, -17, 5, 34);
            FillRect(roads, road, -20, 8, 41, 4);
            FillRect(roads, road, -20, 2, 41, 4);
            FillRect(roads, road, -20, -1, 41, 3);
            FillRect(roads, road, -20, -12, 41, 4);
            FillRect(roads, road, -17, -12, 4, 24);
            FillRect(roads, road, 14, -12, 4, 24);

            FillRect(roads, sidewalk, -20, 12, 41, 1);
            FillRect(roads, sidewalk, -20, 7, 41, 1);
            FillRect(roads, sidewalk, -20, 6, 41, 1);
            FillRect(roads, sidewalk, -20, 1, 41, 1);
            FillRect(roads, sidewalk, -20, -2, 41, 1);
            FillRect(roads, sidewalk, -20, -13, 41, 1);

            for (int y = -6; y <= -3; y++)
                for (int x = -2; x <= 2; x++)
                    roads.SetTile(new Vector3Int(x, y, 0), road);
        }

        private static void DrawPlazas(Tilemap details, Tile plaza)
        {
            FillRect(details, plaza, -5, 6, 11, 7);
            FillRect(details, plaza, -11, 2, 7, 4);
            FillRect(details, plaza, 6, 2, 8, 4);
            FillRect(details, plaza, 10, -14, 9, 5);
        }

        private static void AddBridge()
        {
            GameObject bridge = new GameObject("Riverside Bridge");
            EntityIdentity identity = bridge.AddComponent<EntityIdentity>();
            identity.Configure(DolzoreIds.FirstTownBridge, DolzoreIds.FirstTownRegion, "world.bridge");
            SpriteRenderer sr = bridge.AddComponent<SpriteRenderer>();
            sr.sprite = AssetDatabase.LoadAssetAtPath<Sprite>(ArtRoot + "/bridge.png");
            sr.sortingOrder = 9;
            bridge.transform.position = new Vector3(0.5f, -4.5f, 0f);
            AddWorldLabel("RIVERSIDE BRIDGE", new Vector2(0.5f, -2.35f), Cyan, 0.14f);
        }

        private static void AddBuilding(
            string objectName,
            string spriteName,
            Vector2 position,
            Vector2 colliderSize,
            string accentHex,
            List<Transform> landmarks,
            List<string> landmarkNames,
            string interactionName)
        {
            GameObject go = new GameObject(objectName);
            go.transform.position = new Vector3(position.x, position.y, 0f);
            SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
            sr.sprite = AssetDatabase.LoadAssetAtPath<Sprite>(ArtRoot + "/" + spriteName);
            sr.sortingOrder = 20 - Mathf.RoundToInt(position.y);

            BoxCollider2D collider = go.AddComponent<BoxCollider2D>();
            collider.size = colliderSize;
            collider.offset = new Vector2(0f, -0.95f);

            string stableEntityId = StableEntityIdFor(objectName);
            EntityIdentity identity = go.AddComponent<EntityIdentity>();
            identity.Configure(stableEntityId, DolzoreIds.FirstTownRegion, ArchetypeIdFor(objectName));

            string destinationRegionId = InteriorRegionIdFor(stableEntityId);
            if (!string.IsNullOrEmpty(destinationRegionId))
            {
                WorldPortal portal = go.AddComponent<WorldPortal>();
                portal.Configure(
                    "portal." + stableEntityId,
                    destinationRegionId,
                    InteriorKindFor(stableEntityId));
            }

            AddWorldLabel(objectName, position + new Vector2(0f, -3.0f), Hex(accentHex), 0.12f);

            if (landmarks != null && landmarkNames != null && !string.IsNullOrEmpty(interactionName))
            {
                GameObject point = new GameObject(objectName + " Interaction");
                point.transform.position = new Vector3(position.x, position.y - 2.35f, 0f);
                InteractionAnchor anchor = point.AddComponent<InteractionAnchor>();
                anchor.Configure(stableEntityId);
                landmarks.Add(point.transform);
                landmarkNames.Add(interactionName);
                ActiveLandmarkAnchors.Add(anchor);
            }
        }

        private static string StableEntityIdFor(string objectName)
        {
            switch (objectName)
            {
                case "HOME / RESIDENTIAL": return DolzoreIds.FirstTownHome;
                case "CAFE LUMA": return DolzoreIds.FirstTownCafe;
                case "BAR 13": return DolzoreIds.FirstTownBar;
                case "MARKET HALL": return DolzoreIds.FirstTownMarket;
                case "JOURNAL": return DolzoreIds.FirstTownJournal;
                case "CIVIC CLOCK": return DolzoreIds.FirstTownCivic;
                case "WORKSHOP": return DolzoreIds.FirstTownWorkshop;
                case "RIVERSIDE KIOSK": return DolzoreIds.FirstTownRiverside;
                case "STATION": return DolzoreIds.FirstTownStation;
                case "BACK ALLEY DEPOT": return DolzoreIds.FirstTownDepot;
                case "HOUSE / NORTH": return "entity.first_town.house_north";
                case "HOUSE / EAST": return "entity.first_town.house_east";
                default: return "entity.first_town.unknown";
            }
        }

        private static string ArchetypeIdFor(string objectName)
        {
            if (objectName.Contains("HOUSE") || objectName.Contains("HOME")) return "world.building.residential";
            if (objectName.Contains("BAR")) return "world.building.bar";
            if (objectName.Contains("MARKET")) return "world.building.market";
            if (objectName.Contains("JOURNAL")) return "world.building.journal";
            if (objectName.Contains("STATION")) return "world.building.station";
            if (objectName.Contains("WORKSHOP")) return "world.building.workshop";
            return "world.building.civic";
        }

        private static string InteriorRegionIdFor(string stableEntityId)
        {
            if (stableEntityId == DolzoreIds.FirstTownHome) return "private.first_town.player_home";
            if (stableEntityId == DolzoreIds.FirstTownBar) return "interior.first_town.bar_13";
            if (stableEntityId == DolzoreIds.FirstTownCafe) return "interior.first_town.cafe_luma";
            if (stableEntityId == DolzoreIds.FirstTownMarket) return "interior.first_town.market_hall";
            if (stableEntityId == DolzoreIds.FirstTownJournal) return "interior.first_town.journal";
            if (stableEntityId == DolzoreIds.FirstTownStation) return "interior.first_town.east_station";
            return "";
        }

        private static WorldSpaceKind InteriorKindFor(string stableEntityId)
        {
            return stableEntityId == DolzoreIds.FirstTownHome
                ? WorldSpaceKind.PrivateInstance
                : WorldSpaceKind.Interior;
        }

        private static void AddWorldLabel(string value, Vector2 position, Color color, float size)
        {
            GameObject go = new GameObject(value + " Label");
            go.transform.position = new Vector3(position.x, position.y, 0f);
            TextMesh mesh = go.AddComponent<TextMesh>();
            mesh.text = value;
            mesh.anchor = TextAnchor.MiddleCenter;
            mesh.alignment = TextAlignment.Center;
            mesh.characterSize = size;
            mesh.fontSize = 48;
            mesh.fontStyle = FontStyle.Bold;
            mesh.color = color;
            Font font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            mesh.font = font;
            MeshRenderer renderer = go.GetComponent<MeshRenderer>();
            renderer.sharedMaterial = font.material;
            renderer.sortingOrder = 60;
        }

        private static void AddProps()
        {
            Sprite tree = AssetDatabase.LoadAssetAtPath<Sprite>(ArtRoot + "/tree.png");
            Sprite lamp = AssetDatabase.LoadAssetAtPath<Sprite>(ArtRoot + "/lamp.png");

            Vector2[] trees =
            {
                new Vector2(-21f, 14f), new Vector2(-18f, 14f), new Vector2(-4f, 14f),
                new Vector2(4f, 14f), new Vector2(19f, 14f), new Vector2(21f, 6.5f),
                new Vector2(-21f, 0f), new Vector2(20f, 0f), new Vector2(-21f, -9f),
                new Vector2(7f, -9f), new Vector2(-8f, -15f), new Vector2(4f, -15f)
            };

            for (int i = 0; i < trees.Length; i++)
            {
                GameObject go = new GameObject("Tree " + (i + 1));
                go.transform.position = trees[i];
                SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
                sr.sprite = tree;
                sr.sortingOrder = 18 - Mathf.RoundToInt(trees[i].y);
                CircleCollider2D c = go.AddComponent<CircleCollider2D>();
                c.radius = 0.55f;
                c.offset = new Vector2(0f, -0.65f);
            }

            Vector2[] lamps =
            {
                new Vector2(-5f, 10f), new Vector2(5f, 10f),
                new Vector2(-5f, 4f), new Vector2(5f, 4f),
                new Vector2(-5f, 0f), new Vector2(5f, 0f),
                new Vector2(-5f, -10f), new Vector2(5f, -10f)
            };

            for (int i = 0; i < lamps.Length; i++)
            {
                GameObject go = new GameObject("Street Lamp " + (i + 1));
                go.transform.position = lamps[i];
                SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
                sr.sprite = lamp;
                sr.sortingOrder = 35 - Mathf.RoundToInt(lamps[i].y);
            }
        }

        private static GameObject BuildPlayer()
        {
            GameObject go = new GameObject("Player SORA");
            go.transform.position = new Vector3(0f, 9.5f, 0f);
            SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
            sr.sprite = AssetDatabase.LoadAssetAtPath<Sprite>(ArtRoot + "/player_sora.png");
            sr.sortingOrder = 80;

            Rigidbody2D rb = go.AddComponent<Rigidbody2D>();
            rb.gravityScale = 0f;
            rb.freezeRotation = true;
            rb.interpolation = RigidbodyInterpolation2D.Interpolate;
            rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

            CapsuleCollider2D capsule = go.AddComponent<CapsuleCollider2D>();
            capsule.size = new Vector2(0.68f, 0.92f);
            capsule.offset = new Vector2(0f, -0.26f);

            EntityIdentity identity = go.AddComponent<EntityIdentity>();
            identity.Configure("entity.player.local.review", DolzoreIds.FirstTownRegion, "player.avatar");

            PlayerStateComponent playerState = go.AddComponent<PlayerStateComponent>();
            playerState.ConfigureForFirstTown();

            go.AddComponent<DolzorePlayerController>();
            AddWorldLabel("SORA", new Vector2(0f, 8.45f), Cream, 0.10f);
            return go;
        }

        private static void AddWorldBlocker(string name, Vector2 position, Vector2 size)
        {
            GameObject go = new GameObject(name);
            go.transform.position = position;
            BoxCollider2D box = go.AddComponent<BoxCollider2D>();
            box.size = size;
        }

        private static Canvas BuildHudCanvas(Camera camera)
        {
            GameObject go = new GameObject("HUD Canvas");
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

        private static void BuildHud(
            Transform parent,
            out Text playerNameLabel,
            out Text metaLabel,
            out Text districtLabel,
            out Text statusTargetLabel,
            out Text promptLabel,
            out RectTransform marker,
            out RectTransform mapRect)
        {
            GameObject zonePanel = PanelUi(parent, "Zone Header", new Color(Panel.r, Panel.g, Panel.b, 0.93f));
            Anchor(zonePanel.GetComponent<RectTransform>(), new Vector2(0f, 1f), new Vector2(520f, 94f), new Vector2(34f, -34f), new Vector2(0f, 1f));
            AddOutline(zonePanel, Cyan, 1f);
            Text zone = TextUi(zonePanel.transform, "FIRST TOWN", 27, Cream, TextAnchor.MiddleLeft, FontStyle.Bold);
            Anchor(zone.rectTransform, new Vector2(0f, 1f), new Vector2(450f, 36f), new Vector2(24f, -18f), new Vector2(0f, 1f));
            districtLabel = TextUi(zonePanel.transform, "CENTRAL MAIN STREET  //  PRESENT", 14, Cyan, TextAnchor.MiddleLeft, FontStyle.Bold);
            Anchor(districtLabel.rectTransform, new Vector2(0f, 1f), new Vector2(450f, 26f), new Vector2(24f, -56f), new Vector2(0f, 1f));

            GameObject hud = PanelUi(parent, "Player HUD", new Color(Panel.r, Panel.g, Panel.b, 0.93f));
            Anchor(hud.GetComponent<RectTransform>(), new Vector2(0f, 0f), new Vector2(430f, 150f), new Vector2(34f, 34f), new Vector2(0f, 0f));
            AddOutline(hud, Cyan, 1f);
            playerNameLabel = TextUi(hud.transform, "SORA", 23, Cream, TextAnchor.MiddleLeft, FontStyle.Bold);
            Anchor(playerNameLabel.rectTransform, new Vector2(0f, 1f), new Vector2(190f, 34f), new Vector2(22f, -18f), new Vector2(0f, 1f));
            metaLabel = TextUi(hud.transform, "LV 01   VOCATION WANDERER", 13, Muted, TextAnchor.MiddleLeft, FontStyle.Bold);
            Anchor(metaLabel.rectTransform, new Vector2(0f, 1f), new Vector2(330f, 24f), new Vector2(22f, -49f), new Vector2(0f, 1f));
            statusTargetLabel = TextUi(hud.transform, "STATUS CLEAR   TARGET --", 11, Cyan, TextAnchor.MiddleLeft, FontStyle.Bold);
            Anchor(statusTargetLabel.rectTransform, new Vector2(0f, 1f), new Vector2(360f, 20f), new Vector2(22f, -70f), new Vector2(0f, 1f));
            BarUi(hud.transform, "HEART", 22f, -99f, 0.82f, Orange);
            BarUi(hud.transform, "FOCUS", 22f, -131f, 0.64f, Cyan);

            GameObject mapPanel = PanelUi(parent, "Minimap Panel", new Color(Panel.r, Panel.g, Panel.b, 0.93f));
            Anchor(mapPanel.GetComponent<RectTransform>(), new Vector2(1f, 1f), new Vector2(330f, 258f), new Vector2(-34f, -34f), new Vector2(1f, 1f));
            AddOutline(mapPanel, Cyan, 1f);
            Text mt = TextUi(mapPanel.transform, "FIRST TOWN / MAP", 14, Cyan, TextAnchor.MiddleLeft, FontStyle.Bold);
            Anchor(mt.rectTransform, new Vector2(0f, 1f), new Vector2(280f, 24f), new Vector2(18f, -16f), new Vector2(0f, 1f));

            GameObject map = UiObject("Map", mapPanel.transform);
            Image mi = map.AddComponent<Image>();
            mi.sprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Art/Generated/first_town_minimap.png");
            mi.color = Color.white;
            mapRect = map.GetComponent<RectTransform>();
            Anchor(mapRect, new Vector2(0.5f, 0.5f), new Vector2(288f, 198f), new Vector2(0f, -18f), new Vector2(0.5f, 0.5f));

            GameObject markerGo = PanelUi(map.transform, "Player Marker", Cyan);
            marker = markerGo.GetComponent<RectTransform>();
            marker.anchorMin = marker.anchorMax = new Vector2(0.5f, 0.5f);
            marker.pivot = new Vector2(0.5f, 0.5f);
            marker.sizeDelta = new Vector2(14f, 14f);
            marker.anchoredPosition = new Vector2(0f, 44f);
            marker.localRotation = Quaternion.Euler(0f, 0f, 45f);
            AddOutline(markerGo, Cream, 1f);

            GameObject promptPanel = PanelUi(parent, "Interaction Prompt", new Color(Panel.r, Panel.g, Panel.b, 0.94f));
            Anchor(promptPanel.GetComponent<RectTransform>(), new Vector2(0.5f, 0f), new Vector2(560f, 58f), new Vector2(0f, 34f), new Vector2(0.5f, 0f));
            AddOutline(promptPanel, Orange, 1f);
            promptLabel = TextUi(promptPanel.transform, "EXPLORE   FIRST TOWN", 16, Cream, TextAnchor.MiddleCenter, FontStyle.Bold);
            Stretch(promptLabel.rectTransform);

            Text clock = TextUi(parent, "PRESENT  //  00:13", 13, Muted, TextAnchor.MiddleRight, FontStyle.Bold);
            clock.gameObject.name = "World Clock";
            Anchor(clock.rectTransform, new Vector2(1f, 0f), new Vector2(280f, 24f), new Vector2(-30f, 20f), new Vector2(1f, 0f));
        }

        private static void BarUi(Transform parent, string label, float x, float y, float fill, Color color)
        {
            Text text = TextUi(parent, label, 12, Muted, TextAnchor.MiddleLeft, FontStyle.Bold);
            Anchor(text.rectTransform, new Vector2(0f, 1f), new Vector2(72f, 20f), new Vector2(x, y), new Vector2(0f, 1f));

            GameObject track = PanelUi(parent, label + " Track", Hex("#263044"));
            Anchor(track.GetComponent<RectTransform>(), new Vector2(0f, 1f), new Vector2(285f, 13f), new Vector2(x + 82f, y - 2f), new Vector2(0f, 1f));

            GameObject value = PanelUi(track.transform, label + " Value", color);
            RectTransform rt = value.GetComponent<RectTransform>();
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = new Vector2(fill, 1f);
            rt.offsetMin = new Vector2(1f, 1f);
            rt.offsetMax = new Vector2(-1f, -1f);
        }

        private static GameObject PanelUi(Transform parent, string name, Color color)
        {
            GameObject go = UiObject(name, parent);
            Image image = go.AddComponent<Image>();
            image.color = color;
            return go;
        }

        private static Text TextUi(Transform parent, string value, int size, Color color, TextAnchor align, FontStyle style)
        {
            GameObject go = UiObject(value, parent);
            Text text = go.AddComponent<Text>();
            text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            text.text = value;
            text.fontSize = size;
            text.fontStyle = style;
            text.color = color;
            text.alignment = align;
            text.raycastTarget = false;
            text.horizontalOverflow = HorizontalWrapMode.Overflow;
            text.verticalOverflow = VerticalWrapMode.Overflow;
            return text;
        }

        private static GameObject UiObject(string name, Transform parent)
        {
            GameObject go = new GameObject(name, typeof(RectTransform));
            go.transform.SetParent(parent, false);
            return go;
        }

        private static void AddOutline(GameObject go, Color color, float distance)
        {
            Outline outline = go.AddComponent<Outline>();
            outline.effectColor = color;
            outline.effectDistance = new Vector2(distance, -distance);
        }

        private static void Anchor(RectTransform rt, Vector2 anchor, Vector2 size, Vector2 position, Vector2 pivot)
        {
            rt.anchorMin = rt.anchorMax = anchor;
            rt.pivot = pivot;
            rt.sizeDelta = size;
            rt.anchoredPosition = position;
        }

        private static void Stretch(RectTransform rt)
        {
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.offsetMin = Vector2.zero;
            rt.offsetMax = Vector2.zero;
            rt.pivot = new Vector2(0.5f, 0.5f);
        }

        private static void MakeEventSystem()
        {
            GameObject go = new GameObject("EventSystem");
            go.AddComponent<UnityEngine.EventSystems.EventSystem>();
            UnityEngine.EventSystems.StandaloneInputModule module = go.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();
            module.forceModuleActive = true;
        }

        private static void EnsureDirectories()
        {
            Directory.CreateDirectory(Abs(ArtRoot));
            Directory.CreateDirectory(Abs(TileRoot));
            AssetDatabase.Refresh();
        }

        private static void GenerateTownArt()
        {
            SaveTile(ArtRoot + "/tile_grass.png", "#284A3B", "#315542", 41);
            SaveTile(ArtRoot + "/tile_road.png", "#4D5260", "#5B606D", 73);
            SaveTile(ArtRoot + "/tile_sidewalk.png", "#7A7E83", "#8D918F", 19);
            SaveWater(ArtRoot + "/tile_water.png");
            SaveTile(ArtRoot + "/tile_plaza.png", "#9D8D70", "#B09E7D", 29);

            SaveBuilding("house_blue.png", "#3D5B78", "#7391AA", "#D7C79D", false);
            SaveBuilding("house_cream.png", "#715A4B", "#BEA77D", "#E0D0A6", false);
            SaveBuilding("house_green.png", "#49634E", "#799575", "#E4D3A9", false);
            SaveBuilding("cafe.png", "#6E3D40", "#A85F56", "#F0C77D", true);
            SaveBuilding("bar.png", "#3C253B", "#7C4D6B", "#F3A65A", true);
            SaveBuilding("market.png", "#62402E", "#A66A47", "#F0D18C", true);
            SaveBuilding("journal.png", "#27485E", "#436C87", "#76D7D2", true);
            SaveBuilding("civic.png", "#4B5363", "#68738B", "#E3D7B6", true);
            SaveBuilding("workshop.png", "#443D3A", "#6D645F", "#D8B06B", true);
            SaveBuilding("kiosk.png", "#315B56", "#4E7C75", "#95D0B8", false);
            SaveBuilding("station.png", "#29394E", "#43546E", "#F0C36B", true);
            SaveBuilding("depot.png", "#362E3E", "#51465D", "#BC9CB6", false);

            SaveTree();
            SaveLamp();
            SaveBridge();
            SavePlayer();
        }

        private static void SaveTile(string path, string baseHex, string detailHex, int seed)
        {
            const int s = 16;
            Color32[] p = new Color32[s * s];
            Color32 baseColor = To32(Hex(baseHex));
            Color32 detail = To32(Hex(detailHex));
            System.Random rng = new System.Random(seed);
            for (int i = 0; i < p.Length; i++) p[i] = baseColor;
            for (int i = 0; i < 18; i++)
            {
                int x = rng.Next(0, s);
                int y = rng.Next(0, s);
                p[y * s + x] = detail;
            }
            SaveSprite(path, s, s, p, 16f);
        }

        private static void SaveWater(string path)
        {
            const int s = 16;
            Color32[] p = new Color32[s * s];
            Color32 a = To32(Hex("#1D6176"));
            Color32 b = To32(Hex("#287E8F"));
            Color32 c = To32(Hex("#55A4AC"));
            for (int y = 0; y < s; y++)
                for (int x = 0; x < s; x++)
                {
                    Color32 v = ((x + y) % 7 == 0) ? c : ((y % 5 == 0) ? b : a);
                    p[y * s + x] = v;
                }
            SaveSprite(path, s, s, p, 16f);
        }

        private static void SaveBuilding(string file, string roofHex, string wallHex, string lightHex, bool sign)
        {
            const int w = 64;
            const int h = 80;
            Color32[] p = Transparent(w * h);
            Color32 roof = To32(Hex(roofHex));
            Color32 wall = To32(Hex(wallHex));
            Color32 dark = To32(Hex("#172033"));
            Color32 trim = To32(Hex("#D7C9A6"));
            Color32 light = To32(Hex(lightHex));

            Rect(p, w, h, 7, 12, 50, 46, wall);
            Rect(p, w, h, 3, 52, 58, 11, roof);
            Rect(p, w, h, 10, 63, 44, 6, roof);
            Rect(p, w, h, 15, 18, 12, 18, dark);
            Rect(p, w, h, 37, 18, 12, 18, dark);
            Rect(p, w, h, 18, 21, 6, 10, light);
            Rect(p, w, h, 40, 21, 6, 10, light);
            Rect(p, w, h, 27, 12, 10, 24, dark);
            Rect(p, w, h, 29, 14, 6, 19, roof);
            Rect(p, w, h, 7, 10, 50, 4, dark);
            Rect(p, w, h, 9, 39, 46, 3, trim);
            if (sign)
            {
                Rect(p, w, h, 20, 43, 24, 8, dark);
                Rect(p, w, h, 23, 46, 18, 2, light);
            }
            SaveSprite(ArtRoot + "/" + file, w, h, p, 16f);
        }

        private static void SaveTree()
        {
            const int w = 32, h = 48;
            Color32[] p = Transparent(w * h);
            Rect(p, w, h, 13, 2, 6, 20, To32(Hex("#70523E")));
            Circle(p, w, h, 16, 30, 13, To32(Hex("#315D49")));
            Circle(p, w, h, 10, 26, 8, To32(Hex("#3E7658")));
            Circle(p, w, h, 22, 27, 9, To32(Hex("#3E7658")));
            Circle(p, w, h, 16, 37, 8, To32(Hex("#4B8661")));
            SaveSprite(ArtRoot + "/tree.png", w, h, p, 16f);
        }

        private static void SaveLamp()
        {
            const int w = 16, h = 48;
            Color32[] p = Transparent(w * h);
            Rect(p, w, h, 7, 3, 3, 31, To32(Hex("#30394B")));
            Rect(p, w, h, 4, 33, 9, 4, To32(Hex("#30394B")));
            Rect(p, w, h, 5, 37, 7, 7, To32(Hex("#F3A65A")));
            Rect(p, w, h, 6, 38, 5, 5, To32(Hex("#FFE3A7")));
            SaveSprite(ArtRoot + "/lamp.png", w, h, p, 16f);
        }

        private static void SaveBridge()
        {
            const int w = 80, h = 64;
            Color32[] p = Transparent(w * h);
            Color32 wood = To32(Hex("#876A4E"));
            Color32 light = To32(Hex("#B49368"));
            Rect(p, w, h, 18, 0, 44, 64, wood);
            for (int y = 4; y < 64; y += 8) Rect(p, w, h, 20, y, 40, 3, light);
            Rect(p, w, h, 14, 0, 4, 64, To32(Hex("#4E4239")));
            Rect(p, w, h, 62, 0, 4, 64, To32(Hex("#4E4239")));
            SaveSprite(ArtRoot + "/bridge.png", w, h, p, 16f);
        }

        private static void SavePlayer()
        {
            const int w = 32, h = 48;
            Color32[] p = Transparent(w * h);
            Color32 hair = To32(Hex("#26384C"));
            Color32 skin = To32(Hex("#E7B98E"));
            Color32 coat = To32(Hex("#507C86"));
            Color32 scarf = To32(Hex("#F3A65A"));
            Color32 dark = To32(Hex("#182133"));
            Rect(p, w, h, 10, 33, 12, 10, hair);
            Rect(p, w, h, 9, 28, 14, 10, skin);
            Rect(p, w, h, 8, 16, 16, 14, coat);
            Rect(p, w, h, 7, 25, 18, 4, scarf);
            Rect(p, w, h, 9, 5, 6, 13, dark);
            Rect(p, w, h, 17, 5, 6, 13, dark);
            Rect(p, w, h, 7, 13, 4, 13, skin);
            Rect(p, w, h, 21, 13, 4, 13, skin);
            Rect(p, w, h, 12, 32, 2, 2, dark);
            Rect(p, w, h, 18, 32, 2, 2, dark);
            SaveSprite(ArtRoot + "/player_sora.png", w, h, p, 16f);
        }

        private static void GenerateTownMinimap()
        {
            const int w = 320, h = 220;
            Color32[] p = new Color32[w * h];
            Color32 bg = To32(Hex("#101A27"));
            for (int i = 0; i < p.Length; i++) p[i] = bg;

            Rect(p, w, h, 10, 10, 300, 200, To32(Hex("#284A3B")));
            Rect(p, w, h, 10, 80, 300, 28, To32(Hex("#1D6176")));
            Rect(p, w, h, 150, 10, 28, 200, To32(Hex("#555B67")));
            Rect(p, w, h, 28, 155, 260, 24, To32(Hex("#555B67")));
            Rect(p, w, h, 28, 120, 260, 24, To32(Hex("#555B67")));
            Rect(p, w, h, 28, 60, 260, 18, To32(Hex("#555B67")));
            Rect(p, w, h, 58, 42, 20, 138, To32(Hex("#555B67")));
            Rect(p, w, h, 246, 42, 20, 138, To32(Hex("#555B67")));
            Rect(p, w, h, 147, 80, 34, 28, To32(Hex("#B49368")));

            Rect(p, w, h, 48, 145, 12, 12, To32(Orange));
            Rect(p, w, h, 226, 145, 12, 12, To32(Cyan));
            Rect(p, w, h, 72, 112, 12, 12, To32(Orange));
            Rect(p, w, h, 250, 30, 12, 12, To32(Cyan));
            Circle(p, w, h, 164, 165, 5, To32(Cream));

            SaveSprite("Assets/Art/Generated/first_town_minimap.png", w, h, p, 1f);
        }

        private static void SaveSprite(string assetPath, int width, int height, Color32[] pixels, float ppu)
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
                importer.spritePixelsPerUnit = ppu;
                importer.mipmapEnabled = false;
                importer.filterMode = FilterMode.Point;
                importer.textureCompression = TextureImporterCompression.Uncompressed;
                importer.wrapMode = TextureWrapMode.Clamp;
                importer.alphaIsTransparency = true;
                importer.SaveAndReimport();
            }
        }

        private static Color32[] Transparent(int count)
        {
            Color32[] p = new Color32[count];
            for (int i = 0; i < p.Length; i++) p[i] = new Color32(0, 0, 0, 0);
            return p;
        }

        private static void Rect(Color32[] p, int w, int h, int x, int y, int rw, int rh, Color32 color)
        {
            int x0 = Mathf.Clamp(x, 0, w);
            int x1 = Mathf.Clamp(x + rw, 0, w);
            int y0 = Mathf.Clamp(y, 0, h);
            int y1 = Mathf.Clamp(y + rh, 0, h);
            for (int yy = y0; yy < y1; yy++)
                for (int xx = x0; xx < x1; xx++)
                    p[yy * w + xx] = color;
        }

        private static void Circle(Color32[] p, int w, int h, int cx, int cy, int radius, Color32 color)
        {
            int rr = radius * radius;
            for (int y = -radius; y <= radius; y++)
                for (int x = -radius; x <= radius; x++)
                {
                    if (x * x + y * y > rr) continue;
                    int px = cx + x, py = cy + y;
                    if (px < 0 || px >= w || py < 0 || py >= h) continue;
                    p[py * w + px] = color;
                }
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
    }
}
