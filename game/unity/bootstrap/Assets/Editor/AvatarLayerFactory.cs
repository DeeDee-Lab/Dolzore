using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Dolzore.Editor
{
    public static class AvatarLayerFactory
    {
        private const string Root = "Assets/Resources/Avatars/V1";
        private const int W = 32;
        private const int H = 40;
        private const float Ppu = 20f;

        public static void GenerateAll()
        {
            Directory.CreateDirectory(Abs(Root));
            AssetDatabase.Refresh();

            string[] dirs = { "down", "side", "up" };
            for (int d = 0; d < dirs.Length; d++)
            {
                for (int f = 0; f < 2; f++)
                {
                    string suffix = dirs[d] + "_" + f;
                    SaveMask("shadow_" + suffix, ShadowMask());
                    SaveMask("body_" + suffix, BodyMask(dirs[d], f));
                    SaveMask("face_" + suffix, FaceMask(dirs[d], f), false);

                    foreach (AvatarHairStyle style in Enum.GetValues(typeof(AvatarHairStyle)))
                        SaveMask("hair_" + style.ToString().ToLowerInvariant() + "_" + suffix, HairMask(style, dirs[d], f));

                    foreach (AvatarTopStyle style in Enum.GetValues(typeof(AvatarTopStyle)))
                    {
                        SaveMask("top_" + style.ToString().ToLowerInvariant() + "_" + suffix, TopMask(style, dirs[d], f));
                        SaveMask("topaccent_" + style.ToString().ToLowerInvariant() + "_" + suffix, TopAccentMask(style, dirs[d], f));
                    }

                    foreach (AvatarBottomStyle style in Enum.GetValues(typeof(AvatarBottomStyle)))
                        SaveMask("bottom_" + style.ToString().ToLowerInvariant() + "_" + suffix, BottomMask(style, dirs[d], f));

                    foreach (AvatarShoeStyle style in Enum.GetValues(typeof(AvatarShoeStyle)))
                        SaveMask("shoes_" + style.ToString().ToLowerInvariant() + "_" + suffix, ShoesMask(style, dirs[d], f));

                    foreach (AvatarAccessoryStyle style in Enum.GetValues(typeof(AvatarAccessoryStyle)))
                    {
                        if (style == AvatarAccessoryStyle.None) continue;
                        SaveMask("accessory_" + style.ToString().ToLowerInvariant() + "_" + suffix, AccessoryMask(style, dirs[d], f));
                    }
                }
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private static bool[] ShadowMask()
        {
            bool[] p = Empty();
            Ellipse(p, 16, 3, 9, 2);
            return p;
        }

        private static bool[] BodyMask(string dir, int frame)
        {
            bool[] p = Empty();

            if (dir == "side")
            {
                Ellipse(p, 16, 28, 7, 7);
                Rect(p, 13, 19, 8, 9);
                Rect(p, 11, 12, 3, 8);
                Rect(p, 20, 13, 3, 7);
            }
            else
            {
                Ellipse(p, 16, 28, 8, 7);
                Rect(p, 11, 19, 10, 9);
                Rect(p, 8, 13, 3, 7);
                Rect(p, 21, 13, 3, 7);
            }

            // Hands.
            Rect(p, 8, 10 + (frame == 0 ? 0 : 1), 3, 4);
            Rect(p, 21, 10 + (frame == 0 ? 1 : 0), 3, 4);
            return p;
        }

        private static bool[] FaceMask(string dir, int frame)
        {
            bool[] p = Empty();
            if (dir == "up") return p;

            if (dir == "side")
            {
                Rect(p, 18, 29, 2, 2);
                Rect(p, 20, 26, 1, 1);
            }
            else
            {
                Rect(p, 12, 29, 2, 2);
                Rect(p, 18, 29, 2, 2);
                Rect(p, 15, 25, 2, 1);
                if (frame == 1) Rect(p, 11, 32, 2, 1);
            }
            return p;
        }

        private static bool[] HairMask(AvatarHairStyle style, string dir, int frame)
        {
            bool[] p = Empty();

            switch (style)
            {
                case AvatarHairStyle.AsymBob:
                    Ellipse(p, 16, 33, 9, 6);
                    Rect(p, 7, 27, 5, 8);
                    Rect(p, 20, 25, 5, 10);
                    if (dir == "side") Rect(p, 21, 28, 5, 8);
                    break;

                case AvatarHairStyle.Wavy:
                    Ellipse(p, 16, 33, 10, 6);
                    Rect(p, 6, 27, 5, 8);
                    Rect(p, 21, 27, 5, 8);
                    Rect(p, 8, 24, 4, 5);
                    Rect(p, 20, 24, 4, 5);
                    break;

                case AvatarHairStyle.Tousled:
                    Ellipse(p, 16, 32, 8, 6);
                    Spike(p, 8, 35); Spike(p, 12, 38); Spike(p, 17, 37); Spike(p, 22, 35);
                    if (dir == "side") Spike(p, 24, 32);
                    break;

                case AvatarHairStyle.ShortCrop:
                    Ellipse(p, 16, 32, 8, 5);
                    Rect(p, 9, 28, 14, 5);
                    if (dir != "up") Rect(p, 10, 27, 4, 2);
                    break;

                default:
                    Ellipse(p, 16, 32, 9, 6);
                    Rect(p, 8, 27, 16, 5);
                    Rect(p, 17, 36, 3, 3);
                    break;
            }

            if (dir == "up")
                Rect(p, 8, 27, 16, 7);

            return p;
        }

        private static bool[] TopMask(AvatarTopStyle style, string dir, int frame)
        {
            bool[] p = Empty();

            int left = dir == "side" ? 11 : 9;
            int width = dir == "side" ? 11 : 14;

            switch (style)
            {
                case AvatarTopStyle.Overshirt:
                    Rect(p, left, 13, width, 11);
                    Rect(p, left - 2, 15, 3, 8);
                    Rect(p, left + width - 1, 15, 3, 8);
                    break;

                case AvatarTopStyle.Cardigan:
                    Rect(p, left, 12, width, 12);
                    Rect(p, left - 1, 15, 3, 7);
                    Rect(p, left + width - 2, 15, 3, 7);
                    break;

                case AvatarTopStyle.Hoodie:
                    Rect(p, left, 13, width, 10);
                    Rect(p, left + 2, 22, width - 4, 4);
                    Rect(p, left - 2, 15, 4, 7);
                    Rect(p, left + width - 2, 15, 4, 7);
                    break;

                case AvatarTopStyle.WorkVest:
                    Rect(p, left + 1, 13, width - 2, 10);
                    Rect(p, left - 2, 14, 4, 8);
                    Rect(p, left + width - 2, 14, 4, 8);
                    break;

                default:
                    Rect(p, left, 14, width, 9);
                    Rect(p, left - 2, 15, 4, 7);
                    Rect(p, left + width - 2, 15, 4, 7);
                    Rect(p, left + 1, 12, width - 2, 3);
                    break;
            }
            return p;
        }

        private static bool[] TopAccentMask(AvatarTopStyle style, string dir, int frame)
        {
            bool[] p = Empty();
            int left = dir == "side" ? 11 : 9;
            int width = dir == "side" ? 11 : 14;

            switch (style)
            {
                case AvatarTopStyle.Overshirt:
                    Rect(p, left + 3, 14, 2, 8);
                    Rect(p, left + width - 5, 14, 2, 8);
                    Rect(p, left + 2, 21, width - 4, 2);
                    break;

                case AvatarTopStyle.Cardigan:
                    Rect(p, left + width / 2, 13, 2, 10);
                    Rect(p, left + 2, 21, width - 4, 2);
                    break;

                case AvatarTopStyle.Hoodie:
                    Rect(p, left + 3, 18, width - 6, 2);
                    Rect(p, left + width / 2, 13, 1, 6);
                    break;

                case AvatarTopStyle.WorkVest:
                    Rect(p, left + 3, 15, width - 6, 2);
                    Rect(p, left + 3, 18, 4, 3);
                    break;

                default:
                    Rect(p, left + 2, 20, width - 4, 2);
                    Rect(p, left + width / 2, 13, 1, 7);
                    break;
            }
            return p;
        }

        private static bool[] BottomMask(AvatarBottomStyle style, string dir, int frame)
        {
            bool[] p = Empty();
            int step = frame == 0 ? 0 : 1;

            switch (style)
            {
                case AvatarBottomStyle.SkirtLeggings:
                    Rect(p, 10, 8, 12, 6);
                    Rect(p, 11 - step, 4, 4, 5);
                    Rect(p, 18 + step, 4, 4, 5);
                    break;

                case AvatarBottomStyle.LongSkirt:
                    Rect(p, 10, 5, 12, 9);
                    Rect(p, 9, 4, 14, 3);
                    break;

                case AvatarBottomStyle.Cargo:
                    Rect(p, 10 - step, 5, 5, 9);
                    Rect(p, 18 + step, 5, 5, 9);
                    Rect(p, 9, 10, 6, 3);
                    Rect(p, 19, 10, 5, 3);
                    break;

                default:
                    Rect(p, 10 - step, 5, 5, 9);
                    Rect(p, 18 + step, 5, 5, 9);
                    break;
            }
            return p;
        }

        private static bool[] ShoesMask(AvatarShoeStyle style, string dir, int frame)
        {
            bool[] p = Empty();
            int step = frame == 0 ? 0 : 1;

            if (style == AvatarShoeStyle.Boots)
            {
                Rect(p, 9 - step, 2, 6, 4);
                Rect(p, 18 + step, 2, 6, 4);
            }
            else if (style == AvatarShoeStyle.Flats)
            {
                Rect(p, 10 - step, 2, 5, 3);
                Rect(p, 18 + step, 2, 5, 3);
            }
            else
            {
                Rect(p, 9 - step, 2, 6, 3);
                Rect(p, 18 + step, 2, 6, 3);
                Rect(p, 10 - step, 1, 6, 1);
                Rect(p, 18 + step, 1, 6, 1);
            }
            return p;
        }

        private static bool[] AccessoryMask(AvatarAccessoryStyle style, string dir, int frame)
        {
            bool[] p = Empty();

            switch (style)
            {
                case AvatarAccessoryStyle.CrossbodyPouch:
                    if (dir == "up")
                    {
                        Diagonal(p, 11, 24, 20, 13, 2);
                        Rect(p, 9, 11, 6, 5);
                    }
                    else
                    {
                        Diagonal(p, 10, 23, 20, 13, 2);
                        Rect(p, 20, 10, 6, 6);
                    }
                    break;

                case AvatarAccessoryStyle.RecordBag:
                    Diagonal(p, 10, 23, 21, 12, 2);
                    Rect(p, dir == "side" ? 22 : 20, 9, 7, 8);
                    break;

                case AvatarAccessoryStyle.Notebook:
                    Rect(p, dir == "side" ? 22 : 5, 11, 5, 8);
                    Rect(p, dir == "side" ? 23 : 6, 12, 3, 6);
                    break;

                case AvatarAccessoryStyle.ScarfBackpack:
                    Rect(p, 9, 21, 14, 3);
                    if (dir == "up") Rect(p, 10, 12, 12, 9);
                    else if (dir == "side") Rect(p, 21, 12, 6, 10);
                    else Rect(p, 8, 11, 5, 8);
                    break;

                case AvatarAccessoryStyle.HairClip:
                    Rect(p, dir == "side" ? 20 : 21, 33, 3, 3);
                    break;

                case AvatarAccessoryStyle.Headphones:
                    Rect(p, 7, 29, 3, 6);
                    Rect(p, 22, 29, 3, 6);
                    Rect(p, 9, 35, 14, 2);
                    break;
            }

            return p;
        }

        private static void SaveMask(string name, bool[] mask, bool addOutline = true)
        {
            Color32[] fill = new Color32[W * H];
            for (int i = 0; i < fill.Length; i++)
                fill[i] = mask[i] ? new Color32(255, 255, 255, 255) : new Color32(0, 0, 0, 0);

            string path = Root + "/" + name + ".png";
            WriteSprite(path, fill);

            if (addOutline && name != null && !name.StartsWith("shadow_", StringComparison.Ordinal))
            {
                bool[] expanded = Dilate(mask);
                for (int i = 0; i < expanded.Length; i++)
                    if (mask[i]) expanded[i] = false;

                Color32[] outline = new Color32[W * H];
                for (int i = 0; i < outline.Length; i++)
                    outline[i] = expanded[i] ? new Color32(255, 255, 255, 255) : new Color32(0, 0, 0, 0);

                WriteSprite(Root + "/outline_" + name + ".png", outline);
            }
        }

        private static bool[] Dilate(bool[] source)
        {
            bool[] result = new bool[source.Length];
            for (int y = 0; y < H; y++)
                for (int x = 0; x < W; x++)
                {
                    if (!source[y * W + x]) continue;
                    for (int oy = -1; oy <= 1; oy++)
                        for (int ox = -1; ox <= 1; ox++)
                        {
                            int nx = x + ox, ny = y + oy;
                            if (nx >= 0 && nx < W && ny >= 0 && ny < H)
                                result[ny * W + nx] = true;
                        }
                }
            return result;
        }

        private static void WriteSprite(string assetPath, Color32[] pixels)
        {
            Texture2D tex = new Texture2D(W, H, TextureFormat.RGBA32, false);
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
                importer.spritePixelsPerUnit = Ppu;
                importer.mipmapEnabled = false;
                importer.filterMode = FilterMode.Point;
                importer.textureCompression = TextureImporterCompression.Uncompressed;
                importer.wrapMode = TextureWrapMode.Clamp;
                importer.alphaIsTransparency = true;
                importer.SaveAndReimport();
            }
        }

        private static bool[] Empty() { return new bool[W * H]; }

        private static void Rect(bool[] p, int x, int y, int w, int h)
        {
            for (int yy = Mathf.Max(0, y); yy < Mathf.Min(H, y + h); yy++)
                for (int xx = Mathf.Max(0, x); xx < Mathf.Min(W, x + w); xx++)
                    p[yy * W + xx] = true;
        }

        private static void Ellipse(bool[] p, int cx, int cy, int rx, int ry)
        {
            for (int y = -ry; y <= ry; y++)
                for (int x = -rx; x <= rx; x++)
                {
                    float nx = x / (float)Mathf.Max(1, rx);
                    float ny = y / (float)Mathf.Max(1, ry);
                    if (nx * nx + ny * ny > 1f) continue;
                    int px = cx + x, py = cy + y;
                    if (px >= 0 && px < W && py >= 0 && py < H) p[py * W + px] = true;
                }
        }

        private static void Spike(bool[] p, int x, int y)
        {
            Rect(p, x, y, 2, 2);
            Rect(p, x + 1, y + 2, 1, 2);
        }

        private static void Diagonal(bool[] p, int x0, int y0, int x1, int y1, int thickness)
        {
            int dx = Math.Abs(x1 - x0), sx = x0 < x1 ? 1 : -1;
            int dy = -Math.Abs(y1 - y0), sy = y0 < y1 ? 1 : -1;
            int err = dx + dy;
            while (true)
            {
                Rect(p, x0, y0, thickness, thickness);
                if (x0 == x1 && y0 == y1) break;
                int e2 = 2 * err;
                if (e2 >= dy) { err += dy; x0 += sx; }
                if (e2 <= dx) { err += dx; y0 += sy; }
            }
        }

        private static string Abs(string relative)
        {
            string root = Directory.GetParent(Application.dataPath).FullName;
            return Path.GetFullPath(Path.Combine(root, relative.Replace('/', Path.DirectorySeparatorChar)));
        }
    }
}
