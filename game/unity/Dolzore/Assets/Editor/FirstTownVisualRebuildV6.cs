using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Dolzore.Editor
{
    public static class FirstTownVisualRebuildV6
    {
        private const string ArtRoot = "Assets/Art/Generated/TownV6";
        private const string TileRoot = "Assets/Tiles/Generated";

        private static readonly Color32 Grass = Hex("#22E96A");
        private static readonly Color32 GrassShade = Hex("#43C765");
        private static readonly Color32 Road = Hex("#C8D0A4");
        private static readonly Color32 RoadShade = Hex("#B5BF94");
        private static readonly Color32 Walk = Hex("#F1EBCB");
        private static readonly Color32 WalkShade = Hex("#DED7B7");
        private static readonly Color32 Water = Hex("#4BC7D8");
        private static readonly Color32 WaterShade = Hex("#2FA6C0");
        private static readonly Color32 Ink = Hex("#30304B");

        public static void Apply()
        {
            Directory.CreateDirectory(Abs(ArtRoot));
            AssetDatabase.Refresh();

            GenerateTiles();
            GenerateBuildings();
            GenerateProps();

            RetuneTiles();
            RebuildRoadsAndMarkings();
            ApplyDenseArchitecture();
            ApplyGreenStreetEdges();
            ShrinkFieldCharacters();

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private static void GenerateTiles()
        {
            SaveTile("grass_v6.png", Grass, GrassShade, 14);
            SaveTile("road_v6.png", Road, RoadShade, 8);
            SaveTile("walk_v6.png", Walk, WalkShade, 8);
            SaveTile("plaza_v6.png", Hex("#E9E1BF"), Hex("#D6CEAD"), 7);
            SaveWater("water_v6.png");
            SaveCrosswalk("crosswalk_h_v6.png", true);
            SaveCrosswalk("crosswalk_v_v6.png", false);
        }

        private static void GenerateBuildings()
        {
            SaveUrban("home_v6.png", 92, 104, Hex("#F3D06C"), Hex("#4C73C7"), Hex("#FFF1B2"), 2, false);
            SaveUrban("house_north_v6.png", 88, 100, Hex("#F7D57C"), Hex("#D6575E"), Hex("#FFF0B0"), 2, false);
            SaveUrban("cafe_v6.png", 96, 106, Hex("#F5C65C"), Hex("#D84E5B"), Hex("#65C8C4"), 2, true);
            SaveUrban("house_east_v6.png", 90, 100, Hex("#F0CE71"), Hex("#46A16E"), Hex("#FFF2B0"), 2, false);
            SaveUrban("bar_v6.png", 100, 110, Hex("#F5B95C"), Hex("#7651AA"), Hex("#F17668"), 2, true);
            SaveUrban("market_v6.png", 112, 108, Hex("#E9C052"), Hex("#CC6F3F"), Hex("#FFF0A1"), 2, true);
            SaveUrban("journal_v6.png", 98, 112, Hex("#EBD077"), Hex("#557DB8"), Hex("#6AD1C8"), 3, true);
            SaveUrban("civic_v6.png", 98, 118, Hex("#F2DB8D"), Hex("#7C8A98"), Hex("#F4E7BC"), 3, false);
            SaveUrban("workshop_v6.png", 102, 96, Hex("#D8C58F"), Hex("#7E6758"), Hex("#E8A94D"), 2, false);
            SaveUrban("kiosk_v6.png", 66, 70, Hex("#E7D27C"), Hex("#50A47D"), Hex("#FFF0A8"), 1, true);
            SaveUrban("station_v6.png", 124, 112, Hex("#F3D56E"), Hex("#526C9A"), Hex("#66C5D0"), 2, true);
            SaveUrban("depot_v6.png", 104, 94, Hex("#D7C69D"), Hex("#62556B"), Hex("#B58AB5"), 2, false);

            SaveUrban("facade_red_v6.png", 88, 122, Hex("#F5D178"), Hex("#D76062"), Hex("#FFF0AC"), 3, false);
            SaveUrban("facade_yellow_v6.png", 86, 116, Hex("#F6D86F"), Hex("#D79C42"), Hex("#F7E8A7"), 3, false);
            SaveUrban("facade_green_v6.png", 88, 120, Hex("#E9D27A"), Hex("#638E6A"), Hex("#F4ECB6"), 3, false);
            SaveUrban("facade_blue_v6.png", 86, 118, Hex("#EBD27B"), Hex("#5C79A6"), Hex("#75CBD0"), 3, false);
        }

        private static void GenerateProps()
        {
            SaveTree("tree_v6.png");
            SaveBush("bush_v6.png");
        }

        private static void RetuneTiles()
        {
            ReplaceTile("Grass", "grass_v6.png");
            ReplaceTile("Road", "road_v6.png");
            ReplaceTile("Sidewalk", "walk_v6.png");
            ReplaceTile("Plaza", "plaza_v6.png");
            ReplaceTile("Water", "water_v6.png");
            ReplaceTile("CrosswalkH", "crosswalk_h_v6.png");
            ReplaceTile("CrosswalkV", "crosswalk_v_v6.png");
        }

        private static void RebuildRoadsAndMarkings()
        {
            Tilemap roads = FindTilemap("Roads");
            Tilemap plazas = FindTilemap("Plazas");
            Tilemap markings = FindTilemap("Road Markings");
            if (roads == null || plazas == null || markings == null) return;

            roads.ClearAllTiles();
            plazas.ClearAllTiles();
            markings.ClearAllTiles();

            Tile road = LoadTile("Road");
            Tile walk = LoadTile("Sidewalk");
            Tile plaza = LoadTile("Plaza");
            Tile crossH = LoadTile("CrosswalkH");
            Tile crossV = LoadTile("CrosswalkV");

            // Commercial east-west corridor. Road stays visible but no longer consumes most of the view.
            Fill(roads, road, -22, 4, 44, 3);
            Fill(roads, walk, -22, 3, 44, 1);
            Fill(roads, walk, -22, 7, 44, 1);

            // Upper residential street.
            Fill(roads, road, -21, 10, 42, 2);
            Fill(roads, walk, -21, 9, 42, 1);
            Fill(roads, walk, -21, 12, 42, 1);

            // Main north-south route, reduced from broad rectangular band.
            Fill(roads, road, 0, -15, 3, 28);
            Fill(roads, walk, -1, -15, 1, 28);
            Fill(roads, walk, 3, -15, 1, 28);

            // Local market/civic lane.
            Fill(roads, road, -18, 0, 36, 2);
            Fill(roads, walk, -18, -1, 36, 1);
            Fill(roads, walk, -18, 2, 36, 1);

            // Station branch.
            Fill(roads, road, 14, -12, 3, 19);
            Fill(roads, walk, 13, -12, 1, 19);
            Fill(roads, walk, 17, -12, 1, 19);
            Fill(roads, road, 7, -11, 16, 3);
            Fill(roads, walk, 7, -12, 16, 1);
            Fill(roads, walk, 7, -8, 16, 1);

            // Small public spaces only; no broad yellow/tan bands.
            Fill(plazas, plaza, -10, 2, 5, 1);
            Fill(plazas, plaza, 7, 2, 5, 1);
            Fill(plazas, plaza, -4, 8, 7, 1);

            // Bridge.
            Fill(roads, road, 0, -6, 3, 5);

            // One correctly oriented crossing strip per intersection.
            Fill(markings, crossH, 1, 4, 1, 3);
            Fill(markings, crossH, 15, 4, 1, 3);
            Fill(markings, crossV, 14, -10, 3, 1);
        }

        private static void ApplyDenseArchitecture()
        {
            ReplaceSprite("HOME / RESIDENTIAL", "home_v6.png");
            ReplaceSprite("HOUSE / NORTH", "house_north_v6.png");
            ReplaceSprite("CAFE LUMA", "cafe_v6.png");
            ReplaceSprite("HOUSE / EAST", "house_east_v6.png");
            ReplaceSprite("BAR 13", "bar_v6.png");
            ReplaceSprite("MARKET HALL", "market_v6.png");
            ReplaceSprite("JOURNAL", "journal_v6.png");
            ReplaceSprite("CIVIC CLOCK", "civic_v6.png");
            ReplaceSprite("WORKSHOP", "workshop_v6.png");
            ReplaceSprite("RIVERSIDE KIOSK", "kiosk_v6.png");
            ReplaceSprite("STATION", "station_v6.png");
            ReplaceSprite("BACK ALLEY DEPOT", "depot_v6.png");

            // Tighter authored rows: architecture should read as town blocks, not isolated cards.
            Move("HOME / RESIDENTIAL", new Vector2(-16.2f, 14.1f));
            Move("HOUSE / NORTH", new Vector2(-9.2f, 14.0f));
            Move("CAFE LUMA", new Vector2(8.0f, 14.0f));
            Move("HOUSE / EAST", new Vector2(15.3f, 14.0f));

            Move("BAR 13", new Vector2(-13.8f, 8.0f));
            Move("JOURNAL", new Vector2(9.0f, 8.0f));
            Move("CIVIC CLOCK", new Vector2(16.0f, 8.0f));
            Move("MARKET HALL", new Vector2(-7.2f, 3.1f));
            Move("WORKSHOP", new Vector2(-14.5f, 3.0f));

            AddFacade("Urban Facade North West", "facade_red_v6.png", new Vector2(-21.0f, 14.1f));
            AddFacade("Urban Facade North Mid", "facade_yellow_v6.png", new Vector2(-2.4f, 14.2f));
            AddFacade("Urban Facade North East", "facade_green_v6.png", new Vector2(21.0f, 14.1f));

            AddFacade("Urban Facade Main West", "facade_blue_v6.png", new Vector2(-20.6f, 8.0f));
            AddFacade("Urban Facade Main Center", "facade_red_v6.png", new Vector2(-4.7f, 8.0f));
            AddFacade("Urban Facade Main East", "facade_yellow_v6.png", new Vector2(21.0f, 8.0f));

            AddFacade("Urban Facade Market West", "facade_green_v6.png", new Vector2(-20.8f, 2.8f));
            AddFacade("Urban Facade Civic East", "facade_blue_v6.png", new Vector2(20.7f, 2.9f));
        }

        private static void ApplyGreenStreetEdges()
        {
            AddTreeStrip("North Green Strip", new Vector2(-18.5f, 12.7f), 7, 5.9f);
            AddTreeStrip("Main Green Strip", new Vector2(-17.5f, 2.6f), 6, 6.6f);

            AddBushRow("BAR Shrubs V6", new Vector2(-11.4f, 6.9f), 4);
            AddBushRow("Journal Shrubs V6", new Vector2(7.1f, 6.9f), 4);
            AddBushRow("Station Shrubs V6", new Vector2(10.0f, -7.7f), 5);
        }

        private static void ShrinkFieldCharacters()
        {
            string[] residents = { "Player SORA", "MELO", "YUZU", "PON", "NAMI", "GARU", "MORI", "REI", "HANA" };
            for (int i = 0; i < residents.Length; i++)
            {
                GameObject go = GameObject.Find(residents[i]);
                if (go == null) continue;
                CapsuleCollider2D capsule = go.GetComponent<CapsuleCollider2D>();
                if (capsule != null)
                {
                    capsule.size = residents[i] == "Player SORA" ? new Vector2(0.56f, 0.72f) : new Vector2(0.45f, 0.58f);
                    capsule.offset = new Vector2(0f, -0.13f);
                }
            }
        }

        private static void AddFacade(string name, string file, Vector2 position)
        {
            GameObject go = new GameObject(name);
            go.transform.position = position;
            SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
            sr.sprite = Sprite(file);
            sr.sortingOrder = 28 - Mathf.RoundToInt(position.y);

            BoxCollider2D box = go.AddComponent<BoxCollider2D>();
            box.size = new Vector2(Mathf.Max(2.8f, sr.bounds.size.x * 0.70f), 1.55f);
            box.offset = new Vector2(0f, -1.25f);
        }

        private static void AddTreeStrip(string name, Vector2 start, int count, float spacing)
        {
            for (int i = 0; i < count; i++)
            {
                GameObject go = new GameObject(name + " " + i);
                go.transform.position = start + new Vector2(i * spacing, 0f);
                SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
                sr.sprite = Sprite("tree_v6.png");
                sr.sortingOrder = 34 - Mathf.RoundToInt(go.transform.position.y);
            }
        }

        private static void AddBushRow(string name, Vector2 start, int count)
        {
            for (int i = 0; i < count; i++)
            {
                GameObject go = new GameObject(name + " " + i);
                go.transform.position = start + new Vector2(i * 0.72f, 0f);
                SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
                sr.sprite = Sprite("bush_v6.png");
                sr.sortingOrder = 31 - Mathf.RoundToInt(go.transform.position.y);
            }
        }

        private static void Move(string name, Vector2 position)
        {
            GameObject go = GameObject.Find(name);
            if (go == null) return;
            Vector3 delta = new Vector3(position.x - go.transform.position.x, position.y - go.transform.position.y, 0f);
            go.transform.position = position;

            GameObject anchor = GameObject.Find(name + " Interaction");
            if (anchor != null) anchor.transform.position += delta;
        }

        private static void ReplaceSprite(string name, string file)
        {
            GameObject go = GameObject.Find(name);
            if (go == null) return;
            SpriteRenderer sr = go.GetComponent<SpriteRenderer>();
            if (sr != null) sr.sprite = Sprite(file);
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

        private static void ReplaceTile(string name, string spriteFile)
        {
            Tile tile = LoadTile(name);
            if (tile == null) return;
            tile.sprite = Sprite(spriteFile);
            tile.colliderType = Tile.ColliderType.None;
            EditorUtility.SetDirty(tile);
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

        private static void SaveTile(string file, Color32 a, Color32 b, int dots)
        {
            const int s = 16;
            Color32[] p = Solid(s * s, a);
            System.Random rng = new System.Random(file.GetHashCode());
            for (int i = 0; i < dots; i++)
            {
                int x = rng.Next(1, 15), y = rng.Next(1, 15);
                p[y * s + x] = b;
            }
            SaveSprite(file, s, s, p, 16f);
        }

        private static void SaveWater(string file)
        {
            const int s = 16;
            Color32[] p = Solid(s * s, Water);
            for (int y = 1; y < s; y += 5)
                for (int x = (y % 3); x < s; x += 5)
                    p[y * s + x] = WaterShade;
            SaveSprite(file, s, s, p, 16f);
        }

        private static void SaveCrosswalk(string file, bool horizontal)
        {
            const int s = 16;
            Color32[] p = Transparent(s * s);
            Color32 stripe = new Color32(252, 249, 226, 245);
            if (horizontal)
            {
                for (int y = 2; y <= 12; y += 5)
                    Rect(p, s, s, 1, y, 14, 2, stripe);
            }
            else
            {
                for (int x = 2; x <= 12; x += 5)
                    Rect(p, s, s, x, 1, 2, 14, stripe);
            }
            SaveSprite(file, s, s, p, 16f);
        }

        private static void SaveUrban(string file, int w, int h, Color32 wall, Color32 roof, Color32 accent, int stories, bool awning)
        {
            Color32[] p = Transparent(w * h);
            Color32 side = Darken(wall, 0.78f);
            Color32 glass = Hex("#88C7CC");
            Color32 door = Darken(roof, 0.55f);

            Rect(p, w, h, 7, 7, w - 18, h - 34, wall);
            Rect(p, w, h, w - 13, 10, 7, h - 38, side);

            // Flat/sloped symbolic roof with stronger horizontal mass.
            Rect(p, w, h, 4, h - 30, w - 10, 9, roof);
            for (int row = 0; row < 12; row++)
                Rect(p, w, h, 9 + row, h - 21 + row, Math.Max(1, w - 28 - row * 2), 1, roof);

            int floorHeight = Math.Max(16, (h - 42) / Math.Max(1, stories));
            for (int s = 0; s < stories; s++)
            {
                int y = 17 + s * floorHeight;
                for (int x = 14; x < w - 25; x += 22)
                {
                    Rect(p, w, h, x, y, 12, 11, Ink);
                    Rect(p, w, h, x + 3, y + 3, 6, 5, glass);
                    Rect(p, w, h, x + 5, y + 1, 2, 9, accent);
                }
            }

            Rect(p, w, h, w / 2 - 6, 7, 12, 24, door);
            Rect(p, w, h, w / 2 - 3, 11, 6, 17, accent);

            if (awning)
            {
                int y = 29;
                Rect(p, w, h, 10, y, w - 28, 8, accent);
                for (int x = 11; x < w - 18; x += 10)
                    Rect(p, w, h, x, y, 5, 8, roof);
            }

            // High-contrast base line.
            Rect(p, w, h, 6, 5, w - 16, 3, Ink);

            SaveSprite(file, w, h, p, 16f);
        }

        private static void SaveTree(string file)
        {
            const int w = 34, h = 46;
            Color32[] p = Transparent(w * h);
            Rect(p, w, h, 15, 4, 4, 17, Hex("#7A5A38"));
            Circle(p, w, h, 17, 29, 13, Hex("#2DB662"));
            Circle(p, w, h, 10, 25, 8, Hex("#16E56A"));
            Circle(p, w, h, 24, 25, 8, Hex("#16E56A"));
            Circle(p, w, h, 17, 37, 8, Hex("#68F07E"));
            SaveSprite(file, w, h, p, 16f);
        }

        private static void SaveBush(string file)
        {
            const int w = 20, h = 14;
            Color32[] p = Transparent(w * h);
            Circle(p, w, h, 6, 7, 5, Hex("#2DB662"));
            Circle(p, w, h, 12, 8, 6, Hex("#16E56A"));
            Circle(p, w, h, 16, 7, 4, Hex("#4BD66C"));
            SaveSprite(file, w, h, p, 16f);
        }

        private static void SaveSprite(string file, int w, int h, Color32[] pixels, float ppu)
        {
            pixels = AddOutline(pixels, w, h);
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

        private static Color32[] AddOutline(Color32[] src, int w, int h)
        {
            Color32[] result = (Color32[])src.Clone();
            for (int y = 0; y < h; y++)
                for (int x = 0; x < w; x++)
                {
                    int i = y * w + x;
                    if (src[i].a > 32) continue;
                    bool edge = false;
                    for (int oy = -1; oy <= 1 && !edge; oy++)
                        for (int ox = -1; ox <= 1; ox++)
                        {
                            if (ox == 0 && oy == 0) continue;
                            int nx = x + ox, ny = y + oy;
                            if (nx < 0 || nx >= w || ny < 0 || ny >= h) continue;
                            if (src[ny * w + nx].a > 200) { edge = true; break; }
                        }
                    if (edge) result[i] = Ink;
                }
            return result;
        }

        private static Color32[] Solid(int count, Color32 c)
        {
            Color32[] p = new Color32[count];
            for (int i = 0; i < count; i++) p[i] = c;
            return p;
        }

        private static Color32[] Transparent(int count)
        {
            return new Color32[count];
        }

        private static void Rect(Color32[] p, int w, int h, int x, int y, int rw, int rh, Color32 c)
        {
            int x0 = Mathf.Clamp(x, 0, w), x1 = Mathf.Clamp(x + rw, 0, w);
            int y0 = Mathf.Clamp(y, 0, h), y1 = Mathf.Clamp(y + rh, 0, h);
            for (int yy = y0; yy < y1; yy++)
                for (int xx = x0; xx < x1; xx++)
                    p[yy * w + xx] = c;
        }

        private static void Circle(Color32[] p, int w, int h, int cx, int cy, int r, Color32 c)
        {
            int rr = r * r;
            for (int y = -r; y <= r; y++)
                for (int x = -r; x <= r; x++)
                {
                    if (x * x + y * y > rr) continue;
                    int px = cx + x, py = cy + y;
                    if (px >= 0 && px < w && py >= 0 && py < h) p[py * w + px] = c;
                }
        }

        private static Color32 Darken(Color32 c, float f)
        {
            return new Color32(
                (byte)Mathf.Clamp(Mathf.RoundToInt(c.r * f), 0, 255),
                (byte)Mathf.Clamp(Mathf.RoundToInt(c.g * f), 0, 255),
                (byte)Mathf.Clamp(Mathf.RoundToInt(c.b * f), 0, 255),
                c.a);
        }

        private static Color32 Hex(string value)
        {
            Color c;
            if (!ColorUtility.TryParseHtmlString(value, out c)) return new Color32(255, 255, 255, 255);
            return (Color32)c;
        }

        private static string Abs(string relative)
        {
            string root = Directory.GetParent(Application.dataPath).FullName;
            return Path.GetFullPath(Path.Combine(root, relative.Replace('/', Path.DirectorySeparatorChar)));
        }
    }
}
