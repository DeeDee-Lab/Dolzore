using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Dolzore.Editor
{
    public static class FirstTownVisualRebuildV7
    {
        private const string ArtRoot = "Assets/Art/Generated/TownV6";
        private const string TileRoot = "Assets/Tiles/Generated";

        public static void Apply()
        {
            RemoveV3V6Decoration();
            RebuildRoadPlan();
            RecomposePrimaryBuildings();
            AddBackgroundTownWall();
            AddControlledGreenery();
            RepositionPeople();
            TuneCamera();
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private static void RemoveV3V6Decoration()
        {
            string[] prefixes =
            {
                "Urban Facade ",
                "North Green Strip ",
                "Main Green Strip ",
                "BAR Shrubs V6 ",
                "Journal Shrubs V6 ",
                "Station Shrubs V6 ",
                "Residential Water Tower V3",
                "Residential Mailbox",
                "Residential Bicycle",
                "Residential Bench",
                "Fence North ",
                "North Tree ",
                "Home Shrubs ",
                "Home Flowers",
                "Main Parked Car ",
                "Main Vending",
                "Main Bench",
                "Main Trash",
                "Main Hydrant",
                "Main Road Sign",
                "Main Bus Stop",
                "Main Crosswalk",
                "Main Manhole",
                "Main Pole ",
                "Cafe Planter",
                "Bar Planter",
                "Civic Bench",
                "Civic Bin",
                "Civic Tree",
                "Civic Flowers",
                "Civic Crosswalk",
                "Riverside Landmark Tree",
                "Riverside Bench V3",
                "Riverside Bicycle V3",
                "Riverside Flowers",
                "Riverside Drain",
                "Station Bus Stop",
                "Station Bench",
                "Station Vending",
                "Station Sign",
                "Station Pole",
                "Odd Cat V3",
                "Quiet Bench",
                "Quiet Edge ",
                "V7 "
            };

            GameObject[] all = UnityEngine.Object.FindObjectsOfType<GameObject>();
            List<GameObject> remove = new List<GameObject>();
            for (int i = 0; i < all.Length; i++)
            {
                string n = all[i].name;
                for (int p = 0; p < prefixes.Length; p++)
                {
                    if (n.StartsWith(prefixes[p], StringComparison.Ordinal))
                    {
                        remove.Add(all[i]);
                        break;
                    }
                }
            }

            for (int i = 0; i < remove.Count; i++)
                UnityEngine.Object.DestroyImmediate(remove[i]);
        }

        private static void RebuildRoadPlan()
        {
            Tilemap roads = FindTilemap("Roads");
            Tilemap plazas = FindTilemap("Plazas");
            Tilemap markings = FindTilemap("Road Markings");
            if (roads == null || plazas == null || markings == null)
                throw new InvalidOperationException("DOLZORE_V7_TILEMAP_MISSING");

            roads.ClearAllTiles();
            plazas.ClearAllTiles();
            markings.ClearAllTiles();

            Tile road = LoadTile("Road");
            Tile walk = LoadTile("Sidewalk");
            Tile plaza = LoadTile("Plaza");
            Tile crossH = LoadTile("CrosswalkH");
            Tile crossV = LoadTile("CrosswalkV");

            // One dominant urban street. This intentionally leaves real building blocks above/below it.
            Fill(roads, road, -23, 4, 46, 3);
            Fill(roads, walk, -23, 3, 46, 1);
            Fill(roads, walk, -23, 7, 46, 1);

            // Central route through the town and over the river.
            Fill(roads, road, 0, -16, 3, 33);
            Fill(roads, walk, -1, -16, 1, 33);
            Fill(roads, walk, 3, -16, 1, 33);

            // Station branch only in the south/east block.
            Fill(roads, road, 14, -15, 3, 21);
            Fill(roads, walk, 13, -15, 1, 21);
            Fill(roads, walk, 17, -15, 1, 21);
            Fill(roads, road, 8, -11, 15, 3);
            Fill(roads, walk, 8, -12, 15, 1);
            Fill(roads, walk, 8, -8, 15, 1);

            // Small civic pockets, never broad bands.
            Fill(plazas, plaza, -10, 8, 5, 1);
            Fill(plazas, plaza, 7, 8, 5, 1);
            Fill(plazas, plaza, -10, 2, 5, 1);
            Fill(plazas, plaza, 7, 2, 5, 1);

            // Bridge deck remains the central route.
            Fill(roads, road, 0, -6, 3, 5);

            // Three exact crossing strips.
            Fill(markings, crossH, 1, 4, 1, 3);
            Fill(markings, crossH, 15, 4, 1, 3);
            Fill(markings, crossV, 14, -10, 3, 1);
        }

        private static void RecomposePrimaryBuildings()
        {
            // North urban frontage: dense but separated. Collider footprints stay behind the north sidewalk.
            PlacePrimary("BAR 13", new Vector2(-18.2f, 10.45f), 0.72f, new Vector2(-18.2f, 7.85f));
            PlacePrimary("CAFE LUMA", new Vector2(-12.0f, 10.35f), 0.72f, new Vector2(-12.0f, 7.85f));
            PlacePrimary("JOURNAL", new Vector2(-5.8f, 10.55f), 0.72f, new Vector2(-5.8f, 7.85f));

            PlacePrimary("CIVIC CLOCK", new Vector2(6.0f, 10.55f), 0.72f, null);
            PlacePrimary("HOME / RESIDENTIAL", new Vector2(12.2f, 10.35f), 0.72f, new Vector2(12.2f, 7.85f));
            PlacePrimary("HOUSE / EAST", new Vector2(18.2f, 10.35f), 0.72f, null);

            // South frontage: slightly smaller to preserve river clearance and road view.
            PlacePrimary("MARKET HALL", new Vector2(-17.5f, 0.25f), 0.64f, new Vector2(-17.5f, 3.00f));
            PlacePrimary("WORKSHOP", new Vector2(-10.8f, 0.20f), 0.64f, null);
            PlacePrimary("BACK ALLEY DEPOT", new Vector2(-4.7f, 0.10f), 0.64f, null);

            // Riverside service is deliberately small and clear of the river edge.
            PlacePrimary("RIVERSIDE KIOSK", new Vector2(7.5f, -0.95f), 0.64f, new Vector2(7.5f, -2.15f));

            // Residential secondary house remains in the north-east block.
            PlacePrimary("HOUSE / NORTH", new Vector2(20.8f, 15.1f), 0.58f, null);

            // Station is a separate south-east destination, not stacked into the main street.
            PlacePrimary("STATION", new Vector2(19.4f, -13.0f), 0.67f, new Vector2(18.4f, -8.15f));
        }

        private static void PlacePrimary(string name, Vector2 position, float scale, Vector2? anchorPosition)
        {
            GameObject go = GameObject.Find(name);
            if (go == null)
                throw new InvalidOperationException("DOLZORE_V7_BUILDING_MISSING:" + name);

            go.transform.position = new Vector3(position.x, position.y, 0f);
            go.transform.localScale = new Vector3(scale, scale, 1f);

            if (anchorPosition.HasValue)
            {
                GameObject anchor = GameObject.Find(name + " Interaction");
                if (anchor != null)
                    anchor.transform.position = new Vector3(anchorPosition.Value.x, anchorPosition.Value.y, 0f);
            }
        }

        private static void AddBackgroundTownWall()
        {
            // Decorative rear row has NO colliders; it supplies city density without corrupting gameplay topology.
            AddBackdrop("V7 Backdrop Red A", "facade_red_v6.png", new Vector2(-20.7f, 15.25f), 0.54f);
            AddBackdrop("V7 Backdrop Yellow A", "facade_yellow_v6.png", new Vector2(-14.8f, 15.30f), 0.54f);
            AddBackdrop("V7 Backdrop Green A", "facade_green_v6.png", new Vector2(-8.9f, 15.28f), 0.54f);
            AddBackdrop("V7 Backdrop Blue B", "facade_blue_v6.png", new Vector2(7.4f, 15.30f), 0.54f);
            AddBackdrop("V7 Backdrop Red B", "facade_red_v6.png", new Vector2(13.3f, 15.25f), 0.54f);
            AddBackdrop("V7 Backdrop Yellow B", "facade_yellow_v6.png", new Vector2(19.0f, 15.30f), 0.54f);
        }

        private static void AddBackdrop(string name, string file, Vector2 position, float scale)
        {
            GameObject go = new GameObject(name);
            go.transform.position = new Vector3(position.x, position.y, 0f);
            go.transform.localScale = new Vector3(scale, scale, 1f);
            SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
            sr.sprite = Sprite(file);
            sr.sortingOrder = 8;
        }

        private static void AddControlledGreenery()
        {
            // Trees sit in intentional gaps, never on the road or over facades.
            AddTree("V7 Tree NW", new Vector2(-22.0f, 8.25f), 0.82f);
            AddTree("V7 Tree N Center", new Vector2(3.9f, 8.45f), 0.72f);
            AddTree("V7 Tree NE", new Vector2(21.6f, 8.30f), 0.80f);
            AddTree("V7 Tree South West", new Vector2(-21.2f, 2.0f), 0.78f);
            AddTree("V7 Riverside Tree", new Vector2(10.4f, -1.3f), 1.05f);

            AddBushRow("V7 BAR Bush", new Vector2(-16.7f, 7.55f), 3);
            AddBushRow("V7 Journal Bush", new Vector2(-4.2f, 7.55f), 3);
            AddBushRow("V7 Civic Bush", new Vector2(7.4f, 7.55f), 3);
            AddBushRow("V7 Market Bush", new Vector2(-16.3f, 2.72f), 3);
        }

        private static void AddTree(string name, Vector2 position, float scale)
        {
            GameObject go = new GameObject(name);
            go.transform.position = new Vector3(position.x, position.y, 0f);
            go.transform.localScale = new Vector3(scale, scale, 1f);
            SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
            sr.sprite = Sprite("tree_v6.png");
            sr.sortingOrder = 38 - Mathf.RoundToInt(position.y);
        }

        private static void AddBushRow(string name, Vector2 start, int count)
        {
            for (int i = 0; i < count; i++)
            {
                GameObject go = new GameObject(name + " " + i);
                go.transform.position = new Vector3(start.x + i * 0.62f, start.y, 0f);
                SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
                sr.sprite = Sprite("bush_v6.png");
                sr.sortingOrder = 42 - Mathf.RoundToInt(start.y);
            }
        }

        private static void RepositionPeople()
        {
            MovePerson("Player SORA", new Vector2(-2.6f, 5.15f));
            MovePerson("MELO", new Vector2(-15.2f, 7.45f));
            MovePerson("YUZU", new Vector2(-8.3f, 3.15f));
            MovePerson("PON", new Vector2(5.2f, 7.45f));
            MovePerson("NAMI", new Vector2(14.8f, 7.45f));
            MovePerson("GARU", new Vector2(-13.0f, 3.15f));
            MovePerson("MORI", new Vector2(-3.8f, 7.45f));
            MovePerson("REI", new Vector2(10.4f, -8.15f));
            MovePerson("HANA", new Vector2(8.7f, 3.15f));
        }

        private static void MovePerson(string name, Vector2 position)
        {
            GameObject go = GameObject.Find(name);
            if (go == null) return;
            go.transform.position = new Vector3(position.x, position.y, 0f);
        }

        private static void TuneCamera()
        {
            Camera cam = Camera.main;
            if (cam == null) return;
            cam.orthographicSize = 7.8f;
            cam.transform.position = new Vector3(-0.6f, 5.4f, -10f);

            TownCameraFollow follow = cam.GetComponent<TownCameraFollow>();
            if (follow != null)
            {
                follow.minBounds = new Vector2(-13.0f, -7.7f);
                follow.maxBounds = new Vector2(13.0f, 7.7f);
            }
        }

        private static Tilemap FindTilemap(string name)
        {
            GameObject go = GameObject.Find(name);
            return go != null ? go.GetComponent<Tilemap>() : null;
        }

        private static Tile LoadTile(string name)
        {
            return AssetDatabase.LoadAssetAtPath<Tile>(TileRoot + "/" + name + ".asset");
        }

        private static Sprite Sprite(string file)
        {
            return AssetDatabase.LoadAssetAtPath<Sprite>(ArtRoot + "/" + file);
        }

        private static void Fill(Tilemap map, Tile tile, int x, int y, int w, int h)
        {
            if (map == null || tile == null) return;
            for (int yy = y; yy < y + h; yy++)
                for (int xx = x; xx < x + w; xx++)
                    map.SetTile(new Vector3Int(xx, yy, 0), tile);
        }
    }
}
