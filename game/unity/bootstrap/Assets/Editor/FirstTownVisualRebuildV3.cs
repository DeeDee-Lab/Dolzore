using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Dolzore.Editor
{
    public static class FirstTownVisualRebuildV3
    {
        private const string ArtRoot = "Assets/Art/Generated/TownV3";
        private const string TileRoot = "Assets/Tiles/Generated";

        private static readonly Color32 Grass = C("#78C85A");
        private static readonly Color32 Grass2 = C("#91DB73");
        private static readonly Color32 Road = C("#7275A4");
        private static readonly Color32 Road2 = C("#8589B8");
        private static readonly Color32 Walk = C("#F0D783");
        private static readonly Color32 Walk2 = C("#FFE9AA");
        private static readonly Color32 Water = C("#32A8C5");
        private static readonly Color32 Water2 = C("#62D0DE");
        private static readonly Color32 Ink = C("#30305D");
        private static readonly Color32 Shadow = new Color32(43, 43, 79, 92);

        public static void Apply()
        {
            Directory.CreateDirectory(Abs(ArtRoot));
            AssetDatabase.Refresh();

            GenerateTiles();
            GenerateBuildings();
            GenerateCharacters();
            GenerateProps();

            RetuneTileAssets();
            RebuildRoadComposition();
            RecomposeCoreBuildings();
            ReplaceCoreBuildingSprites();
            RemoveLegacyVisualObjects();
            ReplacePlayer();
            AddResidents();
            AddResidentialLife();
            AddMainStreetLife();
            AddCivicLife();
            AddRiversideLife();
            AddStationLife();
            AddOddityPocket();
            TuneCamera();

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private static void GenerateTiles()
        {
            SaveTexturedTile("grass_v3.png", Grass, Grass2, 17, false);
            SaveTexturedTile("road_v3.png", Road, Road2, 10, true);
            SaveTexturedTile("sidewalk_v3.png", Walk, Walk2, 13, false);
            SaveTexturedTile("plaza_v3.png", C("#E6C766"), C("#F5DD8C"), 19, false);
            SaveWaterTile("water_v3.png");
        }

        private static void GenerateBuildings()
        {
            SaveHouse("home_v3.png", 80, 72, C("#4B74CE"), C("#F4C95D"), C("#FFF0A6"), C("#7791D8"), true);
            SaveHouse("house_north_v3.png", 76, 68, C("#E75A55"), C("#F7D97D"), C("#FFF0B3"), C("#D77A69"), false);
            SaveHouse("house_east_v3.png", 78, 70, C("#48A874"), C("#F5D475"), C("#FFF0AA"), C("#5B8F72"), true);

            SaveShop("cafe_v3.png", 88, 70, C("#E24D59"), C("#FFCE67"), C("#FFF0A5"), C("#72D0C8"), "CAFE", true);
            SaveShop("bar_v3.png", 92, 74, C("#6F4BA8"), C("#F1766B"), C("#FFD65A"), C("#6DD2D0"), "BAR", true);
            SaveMarket("market_v3.png");
            SaveJournal("journal_v3.png");
            SaveCivic("civic_v3.png");
            SaveWorkshop("workshop_v3.png");
            SaveKiosk("kiosk_v3.png");
            SaveStation("station_v3.png");
            SaveDepot("depot_v3.png");
        }

        private static void GenerateCharacters()
        {
            AvatarAssetGenerator.GenerateWardrobeSample();
            AvatarAssetGenerator.GeneratePreview("sora", AvatarPresets.Sora());
            AvatarAssetGenerator.GeneratePreview("melo", AvatarPresets.Melo());
            AvatarAssetGenerator.GeneratePreview("yuzu", AvatarPresets.Yuzu());
            AvatarAssetGenerator.GeneratePreview("pon", AvatarPresets.Pon());
            AvatarAssetGenerator.GeneratePreview("child", AvatarPresets.Child());
            AvatarAssetGenerator.GeneratePreview("worker", AvatarPresets.Worker());
            AvatarAssetGenerator.GeneratePreview("elder", AvatarPresets.Elder());
            AvatarAssetGenerator.GeneratePreview("visitor", AvatarPresets.Visitor());
        }

        private static void GenerateProps()
        {
            SaveTree("tree_v3.png");
            SaveBush("bush_v3.png");
            SaveFence("fence_v3.png");
            SaveBench("bench_v3.png");
            SaveVending("vending_v3.png");
            SaveMailbox("mailbox_v3.png");
            SaveHydrant("hydrant_v3.png");
            SaveTrash("trash_v3.png");
            SaveBicycle("bicycle_v3.png");
            SaveCar("car_v3.png");
            SavePole("pole_v3.png");
            SaveFlowers("flowers_v3.png");
            SaveCat("cat_v3.png");
            SaveWaterTower("water_tower_v3.png");
            SaveCrosswalk("crosswalk_v3.png");
            SaveMarkingTile("marking_crosswalk_h.png", true);
            SaveMarkingTile("marking_crosswalk_v.png", false);
            SaveManhole("manhole_v3.png");
            SaveRoadSign("road_sign_v3.png");
            SaveBusStop("bus_stop_v3.png");
            SavePlanter("planter_v3.png");
            SaveDrain("drain_v3.png");
        }

        private static void RetuneTileAssets()
        {
            ReplaceTileSprite("Grass", "grass_v3.png");
            ReplaceTileSprite("Road", "road_v3.png");
            ReplaceTileSprite("Sidewalk", "sidewalk_v3.png");
            ReplaceTileSprite("Water", "water_v3.png");
            ReplaceTileSprite("Plaza", "plaza_v3.png");
            CreateOrReplaceTileAsset("CrosswalkH", "marking_crosswalk_h.png");
            CreateOrReplaceTileAsset("CrosswalkV", "marking_crosswalk_v.png");
        }

        private static void ReplaceTileSprite(string tileName, string spriteFile)
        {
            Tile tile = AssetDatabase.LoadAssetAtPath<Tile>(TileRoot + "/" + tileName + ".asset");
            if (tile == null) return;
            tile.sprite = Sprite(spriteFile);
            EditorUtility.SetDirty(tile);
        }

        private static void RebuildRoadComposition()
        {
            Tilemap roads = FindTilemap("Roads");
            Tilemap plazas = FindTilemap("Plazas");
            Tilemap markings = EnsureRoadMarkingsTilemap();
            if (roads == null || plazas == null || markings == null) return;

            roads.ClearAllTiles();
            plazas.ClearAllTiles();
            markings.ClearAllTiles();

            Tile road = AssetDatabase.LoadAssetAtPath<Tile>(TileRoot + "/Road.asset");
            Tile walk = AssetDatabase.LoadAssetAtPath<Tile>(TileRoot + "/Sidewalk.asset");
            Tile plaza = AssetDatabase.LoadAssetAtPath<Tile>(TileRoot + "/Plaza.asset");
            Tile crossH = AssetDatabase.LoadAssetAtPath<Tile>(TileRoot + "/CrosswalkH.asset");
            Tile crossV = AssetDatabase.LoadAssetAtPath<Tile>(TileRoot + "/CrosswalkV.asset");

            // Main civic street: readable, but deliberately narrower than the previous prototype.
            Fill(roads, road, -21, 4, 42, 3);
            Fill(roads, walk, -21, 3, 42, 1);
            Fill(roads, walk, -21, 7, 42, 1);

            // Residential upper street.
            Fill(roads, road, -20, 10, 39, 2);
            Fill(roads, walk, -20, 9, 39, 1);
            Fill(roads, walk, -20, 12, 39, 1);

            // Central north/south spine.
            Fill(roads, road, 0, -15, 3, 28);
            Fill(roads, walk, -1, -15, 1, 28);
            Fill(roads, walk, 3, -15, 1, 28);

            // Market/civic side lane.
            Fill(roads, road, -18, 0, 36, 2);
            Fill(roads, walk, -18, -1, 36, 1);
            Fill(roads, walk, -18, 2, 36, 1);

            // East station connector.
            Fill(roads, road, 14, -12, 3, 19);
            Fill(roads, walk, 13, -12, 1, 19);
            Fill(roads, walk, 17, -12, 1, 19);
            Fill(roads, road, 7, -11, 16, 3);
            Fill(roads, walk, 7, -12, 16, 1);
            Fill(roads, walk, 7, -8, 16, 1);

            // Social pockets and small squares.
            Fill(plazas, plaza, -11, 2, 7, 2);
            Fill(plazas, plaza, 6, 2, 7, 2);
            Fill(plazas, plaza, -5, 8, 9, 2);

            // Bridge deck path remains centered over the river.
            Fill(roads, road, 0, -6, 3, 5);

            // Crosswalks are authored in road-grid space so they can never drift off the roadway.
            Fill(markings, crossH, 0, 4, 3, 3);
            Fill(markings, crossH, 14, 4, 3, 3);
            Fill(markings, crossV, 14, -11, 3, 3);
        }

        private static Tilemap FindTilemap(string name)
        {
            GameObject go = GameObject.Find(name);
            return go != null ? go.GetComponent<Tilemap>() : null;
        }

        private static Tilemap EnsureRoadMarkingsTilemap()
        {
            GameObject existing = GameObject.Find("Road Markings");
            if (existing != null) return existing.GetComponent<Tilemap>();

            GameObject grid = GameObject.Find("Town Grid");
            if (grid == null) return null;

            GameObject go = new GameObject("Road Markings", typeof(Tilemap), typeof(TilemapRenderer));
            go.transform.SetParent(grid.transform, false);
            TilemapRenderer renderer = go.GetComponent<TilemapRenderer>();
            renderer.sortingOrder = 5;
            return go.GetComponent<Tilemap>();
        }

        private static void CreateOrReplaceTileAsset(string tileName, string spriteFile)
        {
            string path = TileRoot + "/" + tileName + ".asset";
            Tile tile = AssetDatabase.LoadAssetAtPath<Tile>(path);
            if (tile == null)
            {
                tile = ScriptableObject.CreateInstance<Tile>();
                tile.name = tileName;
                AssetDatabase.CreateAsset(tile, path);
            }

            tile.sprite = Sprite(spriteFile);
            tile.colliderType = Tile.ColliderType.None;
            EditorUtility.SetDirty(tile);
        }

        private static void RecomposeCoreBuildings()
        {
            MoveBuilding("HOME / RESIDENTIAL", new Vector2(-14.5f, 14.0f), new Vector2(3.7f, 2.0f), -2.05f);
            MoveBuilding("HOUSE / NORTH", new Vector2(-7.3f, 14.2f), new Vector2(3.5f, 1.9f), null);
            MoveBuilding("CAFE LUMA", new Vector2(8.0f, 13.9f), new Vector2(4.2f, 2.0f), -2.05f);
            MoveBuilding("HOUSE / EAST", new Vector2(15.2f, 14.0f), new Vector2(3.7f, 1.9f), null);

            MoveBuilding("BAR 13", new Vector2(-13.0f, 7.9f), new Vector2(4.3f, 2.0f), -2.15f);
            MoveBuilding("MARKET HALL", new Vector2(-7.4f, 3.1f), new Vector2(5.1f, 1.9f), -2.05f);
            MoveBuilding("JOURNAL", new Vector2(9.2f, 7.9f), new Vector2(4.1f, 2.0f), -2.10f);
            MoveBuilding("CIVIC CLOCK", new Vector2(16.0f, 7.7f), new Vector2(3.8f, 2.1f), null);

            MoveBuilding("WORKSHOP", new Vector2(-14.6f, 2.9f), new Vector2(4.8f, 1.8f), null);
            MoveBuilding("RIVERSIDE KIOSK", new Vector2(10.5f, -0.2f), new Vector2(2.7f, 1.5f), -1.65f);
            MoveBuilding("STATION", new Vector2(15.1f, -12.7f), new Vector2(5.5f, 2.0f), -2.20f);
            MoveBuilding("BACK ALLEY DEPOT", new Vector2(-14.8f, -11.7f), new Vector2(4.6f, 1.8f), null);

            GameObject bridge = GameObject.Find("Riverside Bridge");
            if (bridge != null) bridge.transform.position = new Vector3(1f, -4.5f, 0f);
        }

        private static void MoveBuilding(string name, Vector2 position, Vector2 colliderSize, float? interactionYOffset)
        {
            GameObject go = GameObject.Find(name);
            if (go == null) return;
            go.transform.position = position;

            BoxCollider2D box = go.GetComponent<BoxCollider2D>();
            if (box != null)
            {
                box.size = colliderSize;
                box.offset = new Vector2(0f, -0.55f);
            }

            if (interactionYOffset.HasValue)
            {
                GameObject anchor = GameObject.Find(name + " Interaction");
                if (anchor != null)
                    anchor.transform.position = new Vector3(position.x, position.y + interactionYOffset.Value, 0f);
            }
        }

        private static void ReplaceCoreBuildingSprites()
        {
            ReplaceSprite("HOME / RESIDENTIAL", "home_v3.png");
            ReplaceSprite("HOUSE / NORTH", "house_north_v3.png");
            ReplaceSprite("CAFE LUMA", "cafe_v3.png");
            ReplaceSprite("HOUSE / EAST", "house_east_v3.png");
            ReplaceSprite("BAR 13", "bar_v3.png");
            ReplaceSprite("MARKET HALL", "market_v3.png");
            ReplaceSprite("JOURNAL", "journal_v3.png");
            ReplaceSprite("CIVIC CLOCK", "civic_v3.png");
            ReplaceSprite("WORKSHOP", "workshop_v3.png");
            ReplaceSprite("RIVERSIDE KIOSK", "kiosk_v3.png");
            ReplaceSprite("STATION", "station_v3.png");
            ReplaceSprite("BACK ALLEY DEPOT", "depot_v3.png");
        }

        private static void ReplaceSprite(string name, string file)
        {
            GameObject go = GameObject.Find(name);
            if (go == null) return;
            SpriteRenderer sr = go.GetComponent<SpriteRenderer>();
            if (sr != null) sr.sprite = Sprite(file);
        }

        private static void RemoveLegacyVisualObjects()
        {
            string[] exact =
            {
                "MELO","YUZU","PON","NAMI","GARU","REI",
                "Bench North","Bench Riverside","Vending Machine","Mailbox","Hydrant","Trash Bin",
                "Bicycle","Parked Car","Utility Pole A","Utility Pole B","Flowers A","Flowers B","Flowers C",
                "Odd Cat","Residential Water Tower","Station Road Sign","Main Street Road Sign",
                "Crosswalk North","Crosswalk Central","Crosswalk South","Manhole Main","Manhole South"
            };
            for (int i = 0; i < exact.Length; i++) DestroyNamed(exact[i]);

            for (int i = 1; i <= 12; i++) DestroyNamed("Tree " + i);
            for (int i = 1; i <= 8; i++) DestroyNamed("Street Lamp " + i);
        }

        private static void DestroyNamed(string name)
        {
            GameObject go = GameObject.Find(name);
            if (go != null) UnityEngine.Object.DestroyImmediate(go);
        }

        private static void ReplacePlayer()
        {
            GameObject player = GameObject.Find("Player SORA");
            if (player == null) return;
            player.transform.position = new Vector3(-4.5f, 8.8f, 0f);
            SpriteRenderer sr = player.GetComponent<SpriteRenderer>();
            if (sr != null) sr.sortingOrder = 90;
            AvatarAssetGenerator.ApplyAvatar(player, "sora", AvatarPresets.Sora(), true);

            PlayerStateComponent playerState = player.GetComponent<PlayerStateComponent>();
            if (playerState != null && playerState.State != null)
                playerState.State.appearance = AvatarPresets.Sora();
        }

        private static void AddResidents()
        {
            AddResident("MELO", "melo", AvatarPresets.Melo(), new Vector2(-9.8f, 5.1f), "entity.npc.melo");
            AddResident("YUZU", "yuzu", AvatarPresets.Yuzu(), new Vector2(7.2f, 5.0f), "entity.npc.yuzu");
            AddResident("PON", "pon", AvatarPresets.Pon(), new Vector2(4.4f, 1.0f), "entity.npc.pon");
            AddResident("NAMI", "child", AvatarPresets.Child(), new Vector2(-9.2f, 10.8f), "entity.npc.nami");
            AddResident("GARU", "worker", AvatarPresets.Worker(), new Vector2(-13.1f, 1.0f), "entity.npc.garu");
            AddResident("MORI", "elder", AvatarPresets.Elder(), new Vector2(-3.2f, 5.0f), "entity.npc.mori");
            AddResident("REI", "visitor", AvatarPresets.Visitor(), new Vector2(10.2f, -9.5f), "entity.npc.rei");

            AvatarAppearanceData hana = AvatarPresets.Visitor();
            hana.hairStyle = 5;
            hana.topStyle = 2;
            hana.bottomStyle = 2;
            hana.accessoryStyle = 8;
            hana.hair = C("#6A4C62");
            hana.top = C("#F0A4B7");
            hana.bottom = C("#557E72");
            hana.accessory = C("#F7D463");
            AddResident("HANA", "hana", hana, new Vector2(12.1f, 5.4f), "entity.npc.hana");
        }

        private static void AddResident(string name, string assetKey, AvatarAppearanceData appearance, Vector2 position, string entityId)
        {
            GameObject go = new GameObject(name);
            go.transform.position = position;
            SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
            sr.sortingOrder = 95 - Mathf.RoundToInt(position.y);

            AvatarAssetGenerator.ApplyAvatar(go, assetKey, appearance, false);

            CapsuleCollider2D capsule = go.AddComponent<CapsuleCollider2D>();
            capsule.size = new Vector2(0.48f, 0.62f);
            capsule.offset = new Vector2(0f, -0.14f);

            EntityIdentity identity = go.AddComponent<EntityIdentity>();
            identity.Configure(entityId, DolzoreIds.FirstTownRegion, "npc.resident");
        }

        private static void AddResidentialLife()
        {
            AddProp("Residential Water Tower V3", "water_tower_v3.png", new Vector2(-20.0f, 14.0f), false, 26);
            AddProp("Residential Mailbox", "mailbox_v3.png", new Vector2(-12.0f, 10.0f), false);
            AddProp("Residential Bicycle", "bicycle_v3.png", new Vector2(-5.0f, 12.6f), false);
            AddProp("Residential Bench", "bench_v3.png", new Vector2(3.8f, 9.0f), false);

            AddFenceRow("Fence North A", new Vector2(-16.8f, 12.7f), 3);
            AddFenceRow("Fence North B", new Vector2(-10.3f, 12.7f), 2);
            AddFenceRow("Fence North C", new Vector2(10.8f, 12.7f), 3);

            AddTree("North Tree A", new Vector2(-18.3f, 10.0f), 1.0f);
            AddTree("North Tree B", new Vector2(4.8f, 11.7f), 0.95f);
            AddTree("North Tree C", new Vector2(19.2f, 10.4f), 1.0f);
            AddBushCluster("Home Shrubs", new Vector2(-11.3f, 13.0f), 3);
            AddFlowers("Home Flowers", new Vector2(-16.2f, 12.8f));
        }

        private static void AddMainStreetLife()
        {
            AddProp("Main Parked Car A", "car_v3.png", new Vector2(-2.5f, 6.0f), false);
            AddProp("Main Parked Car B", "car_v3.png", new Vector2(12.0f, 4.9f), false);
            AddProp("Main Vending", "vending_v3.png", new Vector2(-7.0f, 5.0f), false);
            AddProp("Main Bench", "bench_v3.png", new Vector2(-5.0f, 3.0f), false);
            AddProp("Main Trash", "trash_v3.png", new Vector2(-3.7f, 3.0f), false);
            AddProp("Main Hydrant", "hydrant_v3.png", new Vector2(5.0f, 3.0f), false);
            AddProp("Main Road Sign", "road_sign_v3.png", new Vector2(4.7f, 7.4f), false);
            AddProp("Main Bus Stop", "bus_stop_v3.png", new Vector2(17.8f, 3.2f), false);
            AddProp("Main Manhole", "manhole_v3.png", new Vector2(-1.4f, 5.4f), false, 9);

            AddPole("Main Pole A", new Vector2(-18.8f, 3.2f));
            AddPole("Main Pole B", new Vector2(18.5f, 7.2f));
            AddPlanter("Cafe Planter", new Vector2(7.0f, 11.8f));
            AddPlanter("Bar Planter", new Vector2(-10.2f, 6.6f));
        }

        private static void AddCivicLife()
        {
            AddProp("Civic Bench", "bench_v3.png", new Vector2(7.5f, 2.1f), false);
            AddProp("Civic Bin", "trash_v3.png", new Vector2(9.4f, 2.1f), false);
            AddTree("Civic Tree", new Vector2(5.4f, 1.5f), 0.9f);
            AddFlowers("Civic Flowers", new Vector2(12.4f, 2.0f));

        }

        private static void AddRiversideLife()
        {
            AddTree("Riverside Landmark Tree", new Vector2(7.8f, -1.8f), 1.35f);
            AddProp("Riverside Bench V3", "bench_v3.png", new Vector2(5.7f, -1.3f), false);
            AddProp("Riverside Bicycle V3", "bicycle_v3.png", new Vector2(12.0f, -1.4f), false);
            AddFlowers("Riverside Flowers", new Vector2(9.5f, -1.4f));
            AddProp("Riverside Drain", "drain_v3.png", new Vector2(-5.0f, -1.3f), false, 7);
        }

        private static void AddStationLife()
        {
            AddProp("Station Bus Stop", "bus_stop_v3.png", new Vector2(10.2f, -8.3f), false);
            AddProp("Station Bench", "bench_v3.png", new Vector2(12.2f, -8.2f), false);
            AddProp("Station Vending", "vending_v3.png", new Vector2(18.6f, -8.4f), false);
            AddProp("Station Sign", "road_sign_v3.png", new Vector2(8.3f, -9.0f), false);
            AddPole("Station Pole", new Vector2(20.2f, -8.2f));
        }

        private static void AddOddityPocket()
        {
            AddProp("Odd Cat V3", "cat_v3.png", new Vector2(-9.0f, -8.3f), false);
            AddProp("Quiet Bench", "bench_v3.png", new Vector2(-6.8f, -8.6f), false);
            AddTree("Quiet Edge Tree A", new Vector2(-18.5f, -8.2f), 1.05f);
            AddTree("Quiet Edge Tree B", new Vector2(-12.0f, -14.0f), 1.0f);
            AddBushCluster("Quiet Edge Bushes", new Vector2(-10.5f, -8.0f), 4);
            AddProp("Quiet Edge Vending", "vending_v3.png", new Vector2(-16.0f, -8.2f), false);
        }

        private static void AddFenceRow(string name, Vector2 start, int count)
        {
            for (int i = 0; i < count; i++)
                AddProp(name + " " + i, "fence_v3.png", start + new Vector2(i * 2.8f, 0f), false, 24);
        }

        private static void AddBushCluster(string name, Vector2 start, int count)
        {
            for (int i = 0; i < count; i++)
                AddProp(name + " " + i, "bush_v3.png", start + new Vector2(i * 0.75f, (i % 2) * 0.12f), false, 27);
        }

        private static void AddFlowers(string name, Vector2 position)
        {
            AddProp(name, "flowers_v3.png", position, false, 25);
        }

        private static void AddTree(string name, Vector2 position, float scale)
        {
            GameObject go = AddProp(name, "tree_v3.png", position, false, 32 - Mathf.RoundToInt(position.y));
            if (go != null) go.transform.localScale = new Vector3(scale, scale, 1f);
        }

        private static void AddPole(string name, Vector2 position)
        {
            AddProp(name, "pole_v3.png", position, false, 40 - Mathf.RoundToInt(position.y));
        }

        private static void AddPlanter(string name, Vector2 position)
        {
            AddProp(name, "planter_v3.png", position, false);
        }

        private static GameObject AddProp(string name, string file, Vector2 position, bool collider, int sortingOrder = -1)
        {
            GameObject go = new GameObject(name);
            go.transform.position = position;
            SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
            sr.sprite = Sprite(file);
            sr.sortingOrder = sortingOrder >= 0 ? sortingOrder : 45 - Mathf.RoundToInt(position.y);

            if (collider)
            {
                BoxCollider2D box = go.AddComponent<BoxCollider2D>();
                box.size = new Vector2(Mathf.Max(0.3f, sr.bounds.size.x * 0.65f), Mathf.Max(0.2f, sr.bounds.size.y * 0.28f));
                box.offset = new Vector2(0f, -sr.bounds.size.y * 0.25f);
            }
            return go;
        }

        private static void TuneCamera()
        {
            Camera cam = Camera.main;
            if (cam != null)
            {
                cam.orthographicSize = 8.35f;
                cam.backgroundColor = Hex("#78C85A");
                cam.transform.position = new Vector3(-1.5f, 5.7f, -10f);

                TownCameraFollow follow = cam.GetComponent<TownCameraFollow>();
                if (follow != null)
                {
                    follow.minBounds = new Vector2(-13.8f, -8.2f);
                    follow.maxBounds = new Vector2(13.8f, 8.2f);
                }
            }
        }

        private static void Fill(Tilemap map, Tile tile, int x, int y, int w, int h)
        {
            if (map == null || tile == null) return;
            for (int yy = y; yy < y + h; yy++)
                for (int xx = x; xx < x + w; xx++)
                    map.SetTile(new Vector3Int(xx, yy, 0), tile);
        }

        private static Sprite Sprite(string file)
        {
            return AssetDatabase.LoadAssetAtPath<Sprite>(ArtRoot + "/" + file);
        }

        private static void SaveTexturedTile(string file, Color32 a, Color32 b, int dots, bool lines)
        {
            const int s = 16;
            Color32[] p = new Color32[s * s];
            for (int i = 0; i < p.Length; i++) p[i] = a;
            System.Random rng = new System.Random(file.GetHashCode());
            for (int i = 0; i < dots; i++)
            {
                int x = rng.Next(1, s - 1);
                int y = rng.Next(1, s - 1);
                p[y * s + x] = b;
            }
            if (lines)
            {
                for (int x = 0; x < s; x += 5)
                    p[3 * s + x] = b;
            }
            SaveSprite(file, s, s, p, 16f);
        }

        private static void SaveWaterTile(string file)
        {
            const int s = 16;
            Color32[] p = new Color32[s * s];
            for (int y = 0; y < s; y++)
                for (int x = 0; x < s; x++)
                    p[y * s + x] = ((y + (x / 4)) % 6 == 0) ? Water2 : Water;
            SaveSprite(file, s, s, p, 16f);
        }

        private static void SaveHouse(string file, int w, int h, Color32 roof, Color32 wall, Color32 trim, Color32 side, bool porch)
        {
            Color32[] p = Transparent(w * h);
            ShortShadow(p, w, h, 9, 5, w - 18, 10);
            Rect(p, w, h, 10, 10, w - 24, 26, wall);
            Rect(p, w, h, w - 14, 12, 7, 24, side);
            Roof(p, w, h, 6, 34, w - 12, 29, roof, trim);
            Rect(p, w, h, 17, 17, 12, 12, Ink);
            Rect(p, w, h, 20, 20, 6, 6, C("#9AC5C7"));
            Rect(p, w, h, w - 34, 10, 11, 22, Ink);
            Rect(p, w, h, w - 31, 13, 5, 17, C("#926C52"));
            if (porch)
            {
                Rect(p, w, h, w - 39, 7, 23, 4, C("#8B7356"));
                Rect(p, w, h, w - 37, 4, 19, 3, C("#6D5B47"));
            }
            SaveSprite(file, w, h, p, 16f);
        }

        private static void SaveShop(string file, int w, int h, Color32 roof, Color32 wall, Color32 trim, Color32 glass, string sign, bool awning)
        {
            Color32[] p = Transparent(w * h);
            ShortShadow(p, w, h, 8, 5, w - 17, 9);
            Rect(p, w, h, 8, 9, w - 20, 30, wall);
            Rect(p, w, h, w - 12, 11, 6, 27, Darker(wall, 0.72f));
            Roof(p, w, h, 4, 38, w - 8, 26, roof, trim);
            Rect(p, w, h, 14, 17, 24, 16, Ink);
            Rect(p, w, h, 17, 20, 18, 10, glass);
            Rect(p, w, h, w - 36, 9, 12, 26, Ink);
            Rect(p, w, h, w - 33, 13, 6, 20, trim);
            if (awning)
            {
                Rect(p, w, h, 10, 34, w - 25, 7, trim);
                for (int x = 11; x < w - 16; x += 9) Rect(p, w, h, x, 34, 4, 7, roof);
            }
            DrawTinySign(p, w, h, 15, 43, Math.Min(40, w - 30), 8, sign, Ink, trim);
            SaveSprite(file, w, h, p, 16f);
        }

        private static void SaveMarket(string file)
        {
            const int w = 110, h = 70;
            Color32[] p = Transparent(w * h);
            ShortShadow(p, w, h, 8, 5, 95, 9);
            Rect(p, w, h, 8, 8, 94, 29, C("#E99A54"));
            Rect(p, w, h, 94, 10, 8, 26, C("#C0694B"));
            Roof(p, w, h, 4, 36, 102, 26, C("#D94C59"), C("#FFD871"));
            Rect(p, w, h, 13, 16, 22, 16, Ink);
            Rect(p, w, h, 42, 16, 22, 16, Ink);
            Rect(p, w, h, 71, 16, 19, 16, Ink);
            Rect(p, w, h, 15, 19, 18, 10, C("#79D0C9"));
            Rect(p, w, h, 44, 19, 18, 10, C("#79D0C9"));
            Rect(p, w, h, 73, 19, 15, 10, C("#79D0C9"));
            Rect(p, w, h, 45, 7, 13, 27, C("#7B4551"));
            DrawTinySign(p, w, h, 34, 42, 42, 8, "MARKET", Ink, C("#FFE07A"));
            SaveSprite(file, w, h, p, 16f);
        }

        private static void SaveJournal(string file)
        {
            const int w = 86, h = 76;
            Color32[] p = Transparent(w * h);
            ShortShadow(p, w, h, 8, 5, 70, 9);
            Rect(p, w, h, 9, 9, 66, 30, C("#5E84DA"));
            Rect(p, w, h, 69, 11, 8, 27, C("#4266B3"));
            Roof(p, w, h, 5, 38, 73, 29, C("#3F5FAB"), C("#F7D76C"));
            Rect(p, w, h, 15, 17, 15, 16, Ink);
            Rect(p, w, h, 18, 20, 9, 10, C("#72D7DB"));
            Rect(p, w, h, 48, 17, 15, 16, Ink);
            Rect(p, w, h, 51, 20, 9, 10, C("#72D7DB"));
            Rect(p, w, h, 33, 8, 11, 27, Ink);
            DrawTinySign(p, w, h, 24, 44, 38, 8, "JOURNAL", Ink, C("#FFE493"));
            SaveSprite(file, w, h, p, 16f);
        }

        private static void SaveCivic(string file)
        {
            const int w = 78, h = 92;
            Color32[] p = Transparent(w * h);
            ShortShadow(p, w, h, 9, 5, 60, 10);
            Rect(p, w, h, 10, 9, 55, 35, C("#A990D0"));
            Rect(p, w, h, 59, 11, 8, 31, C("#7A64A9"));
            Roof(p, w, h, 7, 43, 61, 24, C("#6E58A0"), C("#FFD66D"));
            Rect(p, w, h, 25, 61, 29, 14, C("#6E58A0"));
            Roof(p, w, h, 22, 73, 35, 13, C("#58458D"), C("#FFD66D"));
            Circle(p, w, h, 39, 63, 7, C("#FFF0A4"));
            Circle(p, w, h, 39, 63, 4, Ink);
            Rect(p, w, h, 38, 63, 2, 4, C("#FFF0A4"));
            Rect(p, w, h, 39, 62, 4, 2, C("#FFF0A4"));
            Rect(p, w, h, 16, 18, 12, 15, Ink);
            Rect(p, w, h, 48, 18, 12, 15, Ink);
            Rect(p, w, h, 33, 9, 11, 29, Ink);
            SaveSprite(file, w, h, p, 16f);
        }

        private static void SaveWorkshop(string file)
        {
            const int w = 90, h = 66;
            Color32[] p = Transparent(w * h);
            ShortShadow(p, w, h, 8, 4, 76, 9);
            Rect(p, w, h, 7, 8, 73, 29, C("#D58A58"));
            Rect(p, w, h, 74, 10, 8, 26, C("#A7624A"));
            Roof(p, w, h, 3, 36, 81, 23, C("#95505A"), C("#F1C568"));
            Rect(p, w, h, 12, 13, 38, 20, C("#343B42"));
            for (int y = 16; y < 31; y += 5) Rect(p, w, h, 15, y, 32, 2, C("#8C7A8D"));
            Rect(p, w, h, 60, 12, 12, 22, Ink);
            SaveSprite(file, w, h, p, 16f);
        }

        private static void SaveKiosk(string file)
        {
            const int w = 58, h = 54;
            Color32[] p = Transparent(w * h);
            ShortShadow(p, w, h, 6, 4, 45, 8);
            Rect(p, w, h, 7, 7, 42, 25, C("#55B894"));
            Rect(p, w, h, 43, 9, 7, 23, C("#38866F"));
            Roof(p, w, h, 4, 31, 48, 18, C("#3D9276"), C("#F1CE66"));
            Rect(p, w, h, 12, 13, 31, 13, Ink);
            Rect(p, w, h, 15, 16, 25, 7, C("#78D4B9"));
            SaveSprite(file, w, h, p, 16f);
        }

        private static void SaveStation(string file)
        {
            const int w = 120, h = 76;
            Color32[] p = Transparent(w * h);
            ShortShadow(p, w, h, 8, 5, 105, 9);
            Rect(p, w, h, 7, 9, 104, 30, C("#5B86D5"));
            Rect(p, w, h, 104, 11, 8, 27, C("#3C64A8"));
            Roof(p, w, h, 3, 38, 112, 26, C("#405D9B"), C("#FFD15C"));
            Rect(p, w, h, 14, 17, 24, 16, Ink);
            Rect(p, w, h, 17, 20, 18, 10, C("#70C8D4"));
            Rect(p, w, h, 80, 17, 24, 16, Ink);
            Rect(p, w, h, 83, 20, 18, 10, C("#70C8D4"));
            Rect(p, w, h, 50, 8, 18, 29, Ink);
            Rect(p, w, h, 55, 12, 8, 23, C("#DDAA5A"));
            Circle(p, w, h, 59, 50, 7, C("#FFF0AC"));
            Circle(p, w, h, 59, 50, 4, Ink);
            DrawTinySign(p, w, h, 37, 43, 44, 7, "STATION", Ink, C("#FFE58A"));
            SaveSprite(file, w, h, p, 16f);
        }

        private static void SaveDepot(string file)
        {
            const int w = 92, h = 62;
            Color32[] p = Transparent(w * h);
            ShortShadow(p, w, h, 7, 4, 79, 9);
            Rect(p, w, h, 6, 8, 78, 27, C("#7962A0"));
            Rect(p, w, h, 78, 10, 8, 24, C("#584779"));
            Roof(p, w, h, 3, 34, 84, 21, C("#4E3A73"), C("#E6C66C"));
            Rect(p, w, h, 12, 12, 44, 20, C("#33323B"));
            for (int x = 15; x < 54; x += 8) Rect(p, w, h, x, 15, 3, 15, C("#786A9A"));
            Rect(p, w, h, 66, 12, 10, 21, Ink);
            SaveSprite(file, w, h, p, 16f);
        }

        private static void SaveCharacter(string file, Color32 hair, Color32 skin, Color32 clothes, Color32 accent, int variant)
        {
            const int w = 24, h = 32;
            Color32[] p = Transparent(w * h);

            EllipseShadow(p, w, h, 12, 3, 8, 2);
            Rect(p, w, h, 7, 22, 10, 6, hair);
            Rect(p, w, h, 5 + (variant % 2), 20, 14 - (variant % 2), 4, hair);
            Rect(p, w, h, 8, 16, 8, 7, skin);
            Rect(p, w, h, 9, 21, 7, 3, hair);
            Rect(p, w, h, 9, 18, 1, 1, Ink);
            Rect(p, w, h, 14, 18, 1, 1, Ink);

            Rect(p, w, h, 7, 8, 10, 9, clothes);
            Rect(p, w, h, 6, 13, 12, 3, accent);
            Rect(p, w, h, 5, 9, 3, 7, skin);
            Rect(p, w, h, 16, 9, 3, 7, skin);

            Rect(p, w, h, 8, 3, 3, 6, Ink);
            Rect(p, w, h, 13, 3, 3, 6, Ink);
            Rect(p, w, h, 7, 2, 5, 2, C("#F0E5CE"));
            Rect(p, w, h, 12, 2, 5, 2, C("#F0E5CE"));

            if (variant == 1) Rect(p, w, h, 17, 22, 3, 3, accent);
            if (variant == 2) Rect(p, w, h, 17, 7, 4, 8, C("#EEE5CC"));
            if (variant == 3) { Rect(p, w, h, 18, 8, 4, 8, C("#9B7950")); Rect(p, w, h, 6, 14, 12, 2, accent); }
            if (variant == 4) Rect(p, w, h, 5, 7, 14, 3, accent);
            if (variant == 5) Rect(p, w, h, 3, 15, 4, 4, C("#D3AD59"));
            if (variant == 6) Rect(p, w, h, 6, 24, 12, 2, C("#E2DDD1"));
            if (variant == 7) Rect(p, w, h, 17, 8, 5, 7, C("#6B5A7B"));

            SaveSprite(file, w, h, p, 16f);
        }

        private static void SaveTree(string file)
        {
            const int w = 34, h = 46;
            Color32[] p = Transparent(w * h);
            EllipseShadow(p, w, h, 17, 4, 12, 3);
            Rect(p, w, h, 15, 5, 4, 18, C("#7B563B"));
            Circle(p, w, h, 17, 29, 13, C("#48A951"));
            Circle(p, w, h, 10, 25, 8, C("#62C35E"));
            Circle(p, w, h, 24, 25, 8, C("#62C35E"));
            Circle(p, w, h, 17, 37, 8, C("#83D66F"));
            SaveSprite(file, w, h, p, 16f);
        }

        private static void SaveBush(string file)
        {
            const int w = 20, h = 14;
            Color32[] p = Transparent(w * h);
            EllipseShadow(p, w, h, 10, 2, 8, 2);
            Circle(p, w, h, 6, 7, 5, C("#51B054"));
            Circle(p, w, h, 12, 8, 6, C("#6ACB67"));
            Circle(p, w, h, 16, 7, 4, C("#41994A"));
            SaveSprite(file, w, h, p, 16f);
        }

        private static void SaveFence(string file)
        {
            const int w = 44, h = 14;
            Color32[] p = Transparent(w * h);
            Rect(p, w, h, 2, 5, 40, 3, C("#F0D890"));
            for (int x = 4; x < 42; x += 9) Rect(p, w, h, x, 2, 3, 10, C("#DDBE6A"));
            SaveSprite(file, w, h, p, 16f);
        }

        private static void SaveBench(string file)
        {
            const int w = 34, h = 18;
            Color32[] p = Transparent(w * h);
            EllipseShadow(p, w, h, 17, 2, 14, 2);
            Rect(p, w, h, 4, 9, 26, 4, C("#B9794E"));
            Rect(p, w, h, 6, 13, 22, 3, C("#DA965B"));
            Rect(p, w, h, 8, 3, 3, 7, Ink);
            Rect(p, w, h, 23, 3, 3, 7, Ink);
            SaveSprite(file, w, h, p, 16f);
        }

        private static void SaveVending(string file)
        {
            const int w = 18, h = 34;
            Color32[] p = Transparent(w * h);
            ShortShadow(p, w, h, 2, 2, 14, 5);
            Rect(p, w, h, 2, 3, 14, 28, C("#EE5C64"));
            Rect(p, w, h, 4, 20, 10, 8, C("#FFF1B3"));
            Rect(p, w, h, 5, 21, 8, 6, C("#67CFD1"));
            Rect(p, w, h, 5, 7, 8, 10, Ink);
            SaveSprite(file, w, h, p, 16f);
        }

        private static void SaveMailbox(string file)
        {
            const int w = 18, h = 27;
            Color32[] p = Transparent(w * h);
            Rect(p, w, h, 5, 6, 9, 14, C("#4F8CD0"));
            Rect(p, w, h, 4, 18, 11, 4, C("#3E6FB0"));
            Rect(p, w, h, 7, 2, 5, 5, Ink);
            Rect(p, w, h, 6, 11, 7, 2, C("#FFE48A"));
            SaveSprite(file, w, h, p, 16f);
        }

        private static void SaveHydrant(string file)
        {
            const int w = 14, h = 20;
            Color32[] p = Transparent(w * h);
            Rect(p, w, h, 5, 3, 5, 12, C("#F05855"));
            Rect(p, w, h, 3, 12, 9, 4, C("#D74449"));
            Rect(p, w, h, 2, 7, 11, 3, C("#FF8068"));
            SaveSprite(file, w, h, p, 16f);
        }

        private static void SaveTrash(string file)
        {
            const int w = 16, h = 22;
            Color32[] p = Transparent(w * h);
            Rect(p, w, h, 4, 3, 8, 15, C("#657B88"));
            Rect(p, w, h, 3, 17, 10, 3, C("#3E5160"));
            SaveSprite(file, w, h, p, 16f);
        }

        private static void SaveBicycle(string file)
        {
            const int w = 36, h = 20;
            Color32[] p = Transparent(w * h);
            Ring(p, w, h, 8, 7, 5, Ink);
            Ring(p, w, h, 28, 7, 5, Ink);
            Line(p, w, h, 8, 7, 18, 13, C("#E88349"));
            Line(p, w, h, 18, 13, 28, 7, C("#E88349"));
            Line(p, w, h, 8, 7, 22, 7, C("#E88349"));
            Line(p, w, h, 22, 7, 18, 13, C("#E88349"));
            SaveSprite(file, w, h, p, 16f);
        }

        private static void SaveCar(string file)
        {
            const int w = 50, h = 25;
            Color32[] p = Transparent(w * h);
            EllipseShadow(p, w, h, 25, 2, 21, 2);
            Rect(p, w, h, 4, 6, 42, 10, C("#5CA1D4"));
            Rect(p, w, h, 12, 16, 28, 6, C("#3F78B5"));
            Rect(p, w, h, 16, 17, 10, 4, C("#A5E1D8"));
            Rect(p, w, h, 28, 17, 9, 4, C("#A5E1D8"));
            Circle(p, w, h, 13, 5, 4, Ink);
            Circle(p, w, h, 38, 5, 4, Ink);
            SaveSprite(file, w, h, p, 16f);
        }

        private static void SavePole(string file)
        {
            const int w = 16, h = 52;
            Color32[] p = Transparent(w * h);
            Rect(p, w, h, 7, 3, 3, 42, C("#6B513D"));
            Rect(p, w, h, 2, 40, 13, 3, C("#584332"));
            Rect(p, w, h, 3, 44, 3, 5, Ink);
            Rect(p, w, h, 11, 44, 3, 5, Ink);
            SaveSprite(file, w, h, p, 16f);
        }

        private static void SaveFlowers(string file)
        {
            const int w = 24, h = 12;
            Color32[] p = Transparent(w * h);
            for (int x = 2; x < 23; x += 5)
            {
                Rect(p, w, h, x, 1, 1, 7, C("#42A94D"));
                Circle(p, w, h, x, 8, 2, (x % 10 == 2) ? C("#FF8EA4") : C("#FFD75E"));
            }
            SaveSprite(file, w, h, p, 16f);
        }

        private static void SaveCat(string file)
        {
            const int w = 18, h = 12;
            Color32[] p = Transparent(w * h);
            Rect(p, w, h, 4, 3, 9, 5, C("#E8A45E"));
            Rect(p, w, h, 12, 6, 4, 3, C("#E8A45E"));
            Rect(p, w, h, 13, 9, 1, 2, C("#E8A45E"));
            Rect(p, w, h, 15, 9, 1, 2, C("#E8A45E"));
            Rect(p, w, h, 2, 4, 3, 2, C("#E8A45E"));
            SaveSprite(file, w, h, p, 16f);
        }

        private static void SaveWaterTower(string file)
        {
            const int w = 42, h = 78;
            Color32[] p = Transparent(w * h);
            Rect(p, w, h, 10, 3, 3, 42, C("#576978"));
            Rect(p, w, h, 29, 3, 3, 42, C("#576978"));
            Line(p, w, h, 11, 13, 31, 40, C("#576978"));
            Line(p, w, h, 31, 13, 11, 40, C("#576978"));
            Rect(p, w, h, 8, 44, 26, 18, C("#5AA8AD"));
            Rect(p, w, h, 5, 50, 32, 10, C("#5AA8AD"));
            Rect(p, w, h, 11, 62, 20, 4, C("#447E89"));
            SaveSprite(file, w, h, p, 16f);
        }

        private static void SaveMarkingTile(string file, bool horizontal)
        {
            const int s = 16;
            Color32[] p = Transparent(s * s);
            Color32 stripe = new Color32(245, 240, 218, 220);

            if (horizontal)
            {
                for (int y = 2; y < 15; y += 5)
                    Rect(p, s, s, 1, y, 14, 2, stripe);
            }
            else
            {
                for (int x = 2; x < 15; x += 5)
                    Rect(p, s, s, x, 1, 2, 14, stripe);
            }

            SaveSprite(file, s, s, p, 16f);
        }

        private static void SaveCrosswalk(string file)
        {
            const int w = 48, h = 24;
            Color32[] p = Transparent(w * h);
            for (int x = 3; x < 46; x += 8) Rect(p, w, h, x, 2, 5, 20, new Color32(232, 225, 201, 205));
            SaveSprite(file, w, h, p, 16f);
        }

        private static void SaveManhole(string file)
        {
            const int w = 14, h = 14;
            Color32[] p = Transparent(w * h);
            Circle(p, w, h, 7, 7, 5, C("#55577B"));
            Ring(p, w, h, 7, 7, 4, C("#8B8AB3"));
            SaveSprite(file, w, h, p, 16f);
        }

        private static void SaveRoadSign(string file)
        {
            const int w = 24, h = 39;
            Color32[] p = Transparent(w * h);
            Rect(p, w, h, 11, 2, 2, 24, C("#5B6180"));
            Rect(p, w, h, 3, 25, 18, 9, C("#4A9BC1"));
            Rect(p, w, h, 6, 28, 12, 2, C("#FFF0A1"));
            SaveSprite(file, w, h, p, 16f);
        }

        private static void SaveBusStop(string file)
        {
            const int w = 22, h = 42;
            Color32[] p = Transparent(w * h);
            Rect(p, w, h, 10, 2, 3, 30, C("#5C6280"));
            Rect(p, w, h, 3, 27, 17, 10, C("#559DC1"));
            Rect(p, w, h, 6, 30, 11, 2, C("#FFE493"));
            Rect(p, w, h, 7, 33, 8, 2, C("#FFE493"));
            SaveSprite(file, w, h, p, 16f);
        }

        private static void SavePlanter(string file)
        {
            const int w = 26, h = 18;
            Color32[] p = Transparent(w * h);
            Rect(p, w, h, 3, 2, 20, 7, C("#B87950"));
            Rect(p, w, h, 5, 8, 16, 3, C("#55AB52"));
            Circle(p, w, h, 8, 12, 3, C("#FF94A8"));
            Circle(p, w, h, 14, 13, 3, C("#FFD85C"));
            Circle(p, w, h, 19, 12, 3, C("#E88CC8"));
            SaveSprite(file, w, h, p, 16f);
        }

        private static void SaveDrain(string file)
        {
            const int w = 20, h = 10;
            Color32[] p = Transparent(w * h);
            Rect(p, w, h, 2, 2, 16, 6, C("#5B5E7B"));
            for (int x = 4; x < 18; x += 3) Rect(p, w, h, x, 3, 1, 4, C("#303251"));
            SaveSprite(file, w, h, p, 16f);
        }

        private static void Roof(Color32[] p, int w, int h, int x, int y, int rw, int rh, Color32 roof, Color32 trim)
        {
            for (int row = 0; row < rh; row++)
            {
                float t = row / (float)Math.Max(1, rh - 1);
                int inset = Mathf.RoundToInt(Mathf.Lerp(0f, 9f, t));
                Rect(p, w, h, x + inset, y + row, Math.Max(1, rw - inset * 2), 1, roof);
            }
            Rect(p, w, h, x, y, rw, 3, Darker(roof, 0.78f));
            Rect(p, w, h, x + 5, y + 4, rw - 10, 2, trim);
            for (int yy = y + 9; yy < y + rh - 2; yy += 6)
                Rect(p, w, h, x + 9, yy, Math.Max(1, rw - 18), 1, Darker(roof, 0.88f));
        }

        private static void DrawTinySign(Color32[] p, int w, int h, int x, int y, int rw, int rh, string word, Color32 bg, Color32 fg)
        {
            Rect(p, w, h, x, y, rw, rh, bg);
            int bars = Math.Min(word.Length, Math.Max(1, rw / 5));
            for (int i = 0; i < bars; i++)
            {
                int xx = x + 3 + i * 5;
                if (xx + 1 < x + rw - 2) Rect(p, w, h, xx, y + 3, 2, Math.Max(1, rh - 5), fg);
            }
        }

        private static void ShortShadow(Color32[] p, int w, int h, int x, int y, int rw, int rh)
        {
            Rect(p, w, h, x + 4, y, rw, rh, Shadow);
        }

        private static void EllipseShadow(Color32[] p, int w, int h, int cx, int cy, int rx, int ry)
        {
            for (int y = -ry; y <= ry; y++)
                for (int x = -rx; x <= rx; x++)
                {
                    float nx = x / (float)Math.Max(1, rx);
                    float ny = y / (float)Math.Max(1, ry);
                    if (nx * nx + ny * ny > 1f) continue;
                    int px = cx + x, py = cy + y;
                    if (px >= 0 && px < w && py >= 0 && py < h) p[py * w + px] = Shadow;
                }
        }

        private static void SaveSprite(string file, int w, int h, Color32[] pixels, float ppu)
        {
            if (!file.StartsWith("grass_", StringComparison.Ordinal) &&
                !file.StartsWith("road_", StringComparison.Ordinal) &&
                !file.StartsWith("sidewalk_", StringComparison.Ordinal) &&
                !file.StartsWith("plaza_", StringComparison.Ordinal) &&
                !file.StartsWith("water_", StringComparison.Ordinal) &&
                !file.StartsWith("marking_", StringComparison.Ordinal))
            {
                pixels = AddReadableOutline(pixels, w, h);
            }

            string assetPath = ArtRoot + "/" + file;
            Texture2D tex = new Texture2D(w, h, TextureFormat.RGBA32, false);
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

        private static Color32[] AddReadableOutline(Color32[] source, int w, int h)
        {
            Color32[] result = (Color32[])source.Clone();
            Color32 outline = C("#30305D");

            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    int index = y * w + x;
                    if (source[index].a > 48) continue;

                    bool neighbor = false;
                    for (int oy = -1; oy <= 1 && !neighbor; oy++)
                    {
                        for (int ox = -1; ox <= 1; ox++)
                        {
                            if (ox == 0 && oy == 0) continue;
                            int nx = x + ox, ny = y + oy;
                            if (nx < 0 || nx >= w || ny < 0 || ny >= h) continue;
                            if (source[ny * w + nx].a >= 220)
                            {
                                neighbor = true;
                                break;
                            }
                        }
                    }

                    if (neighbor) result[index] = outline;
                }
            }

            return result;
        }

        private static Color32[] Transparent(int count)
        {
            Color32[] p = new Color32[count];
            for (int i = 0; i < count; i++) p[i] = new Color32(0, 0, 0, 0);
            return p;
        }

        private static void Rect(Color32[] p, int w, int h, int x, int y, int rw, int rh, Color32 color)
        {
            int x0 = Mathf.Clamp(x, 0, w), x1 = Mathf.Clamp(x + rw, 0, w);
            int y0 = Mathf.Clamp(y, 0, h), y1 = Mathf.Clamp(y + rh, 0, h);
            for (int yy = y0; yy < y1; yy++)
                for (int xx = x0; xx < x1; xx++)
                    p[yy * w + xx] = color;
        }

        private static void Circle(Color32[] p, int w, int h, int cx, int cy, int r, Color32 color)
        {
            int rr = r * r;
            for (int y = -r; y <= r; y++)
                for (int x = -r; x <= r; x++)
                {
                    if (x * x + y * y > rr) continue;
                    int px = cx + x, py = cy + y;
                    if (px >= 0 && px < w && py >= 0 && py < h) p[py * w + px] = color;
                }
        }

        private static void Ring(Color32[] p, int w, int h, int cx, int cy, int r, Color32 color)
        {
            int outer = r * r, inner = (r - 2) * (r - 2);
            for (int y = -r; y <= r; y++)
                for (int x = -r; x <= r; x++)
                {
                    int d = x * x + y * y;
                    if (d > outer || d < inner) continue;
                    int px = cx + x, py = cy + y;
                    if (px >= 0 && px < w && py >= 0 && py < h) p[py * w + px] = color;
                }
        }

        private static void Line(Color32[] p, int w, int h, int x0, int y0, int x1, int y1, Color32 color)
        {
            int dx = Math.Abs(x1 - x0), sx = x0 < x1 ? 1 : -1;
            int dy = -Math.Abs(y1 - y0), sy = y0 < y1 ? 1 : -1;
            int err = dx + dy;
            while (true)
            {
                if (x0 >= 0 && x0 < w && y0 >= 0 && y0 < h) p[y0 * w + x0] = color;
                if (x0 == x1 && y0 == y1) break;
                int e2 = 2 * err;
                if (e2 >= dy) { err += dy; x0 += sx; }
                if (e2 <= dx) { err += dx; y0 += sy; }
            }
        }

        private static Color32 Darker(Color32 c, float factor)
        {
            return new Color32(
                (byte)Mathf.Clamp(Mathf.RoundToInt(c.r * factor), 0, 255),
                (byte)Mathf.Clamp(Mathf.RoundToInt(c.g * factor), 0, 255),
                (byte)Mathf.Clamp(Mathf.RoundToInt(c.b * factor), 0, 255),
                c.a);
        }

        private static string Abs(string relative)
        {
            string root = Directory.GetParent(Application.dataPath).FullName;
            return Path.GetFullPath(Path.Combine(root, relative.Replace('/', Path.DirectorySeparatorChar)));
        }

        private static Color Hex(string hex)
        {
            Color c;
            if (!ColorUtility.TryParseHtmlString(hex, out c)) throw new ArgumentException(hex);
            return c;
        }

        private static Color32 C(string hex) { return (Color32)Hex(hex); }
    }
}
