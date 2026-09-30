using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Dolzore.Editor
{
    public static class AvatarAssetGenerator
    {
        private const string OutputRoot = "Assets/Art/Generated/Avatars";

        public static Sprite GeneratePreview(string key, AvatarAppearanceData appearance)
        {
            Directory.CreateDirectory(Abs(OutputRoot));
            AssetDatabase.Refresh();

            string assetPath = OutputRoot + "/" + key + "_down_0.png";
            Texture2D tex = AvatarPixelComposer.CreateTexture(appearance, AvatarDirection.Down, 0);
            File.WriteAllBytes(Abs(assetPath), tex.EncodeToPNG());
            UnityEngine.Object.DestroyImmediate(tex);

            AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceUpdate);
            TextureImporter importer = AssetImporter.GetAtPath(assetPath) as TextureImporter;
            if (importer != null)
            {
                importer.textureType = TextureImporterType.Sprite;
                importer.spriteImportMode = SpriteImportMode.Single;
                importer.spritePixelsPerUnit = AvatarPixelComposer.PixelsPerUnit;
                importer.mipmapEnabled = false;
                importer.filterMode = FilterMode.Point;
                importer.textureCompression = TextureImporterCompression.Uncompressed;
                importer.wrapMode = TextureWrapMode.Clamp;
                importer.alphaIsTransparency = true;
                importer.spritePivot = new Vector2(0.5f, 0.08f);
                importer.SaveAndReimport();
            }

            return AssetDatabase.LoadAssetAtPath<Sprite>(assetPath);
        }

        public static void ApplyAvatar(GameObject go, string key, AvatarAppearanceData appearance, bool animate)
        {
            if (go == null) return;

            SpriteRenderer renderer = go.GetComponent<SpriteRenderer>();
            if (renderer == null)
                renderer = go.AddComponent<SpriteRenderer>();

            renderer.sprite = GeneratePreview(key, appearance);

            AvatarRuntimeRenderer runtime = go.GetComponent<AvatarRuntimeRenderer>();
            if (runtime == null)
                runtime = go.AddComponent<AvatarRuntimeRenderer>();

            runtime.appearance = appearance.Clone();
            runtime.targetRenderer = renderer;
            runtime.movementBody = animate ? go.GetComponent<Rigidbody2D>() : null;
            runtime.direction = AvatarDirection.Down;
        }

        public static void GenerateWardrobeSample()
        {
            GeneratePreview("wardrobe_sora", AvatarPresets.Sora());
            GeneratePreview("wardrobe_melo", AvatarPresets.Melo());
            GeneratePreview("wardrobe_yuzu", AvatarPresets.Yuzu());
            GeneratePreview("wardrobe_pon", AvatarPresets.Pon());

            AvatarAppearanceData sporty = AvatarPresets.Sora();
            sporty.hairStyle = 4;
            sporty.topStyle = 3;
            sporty.bottomStyle = 4;
            sporty.shoeStyle = 3;
            sporty.accessoryStyle = 6;
            sporty.accentStyle = 5;
            sporty.hair = Hex("#503E69");
            sporty.top = Hex("#F06D6A");
            sporty.bottom = Hex("#365A91");
            sporty.shoes = Hex("#F8E8B8");
            sporty.accessory = Hex("#52D2C0");
            sporty.accent = Hex("#F4D45F");
            GeneratePreview("wardrobe_sporty", sporty);

            AvatarAppearanceData street = AvatarPresets.Pon();
            street.hairStyle = 7;
            street.topStyle = 1;
            street.bottomStyle = 3;
            street.accessoryStyle = 7;
            street.accentStyle = 0;
            street.hair = Hex("#2E5768");
            street.top = Hex("#FF8A59");
            street.bottom = Hex("#5E4395");
            street.shoes = Hex("#F7D969");
            street.accessory = Hex("#4AB5D2");
            GeneratePreview("wardrobe_street", street);

            AvatarAppearanceData soft = AvatarPresets.Yuzu();
            soft.hairStyle = 5;
            soft.topStyle = 2;
            soft.bottomStyle = 2;
            soft.accessoryStyle = 8;
            soft.accentStyle = 2;
            soft.hair = Hex("#6A4C62");
            soft.top = Hex("#F1A8B8");
            soft.bottom = Hex("#557E72");
            soft.shoes = Hex("#7A536B");
            soft.accessory = Hex("#FFD56C");
            soft.accent = Hex("#5ECCE0");
            GeneratePreview("wardrobe_soft", soft);

            AvatarAppearanceData utility = AvatarPresets.Worker();
            utility.hairStyle = 6;
            utility.topStyle = 5;
            utility.bottomStyle = 0;
            utility.accessoryStyle = 4;
            utility.accentStyle = 6;
            utility.hair = Hex("#3D425A");
            utility.top = Hex("#63A75D");
            utility.bottom = Hex("#45526E");
            utility.shoes = Hex("#D8B16B");
            utility.accessory = Hex("#B86A4A");
            utility.accent = Hex("#F0CB55");
            GeneratePreview("wardrobe_utility", utility);

            GenerateShowcase(new[]
            {
                AvatarPresets.Sora(),
                AvatarPresets.Melo(),
                AvatarPresets.Yuzu(),
                AvatarPresets.Pon(),
                sporty,
                street,
                soft,
                utility
            });
        }

        public static void GenerateShowcase(AvatarAppearanceData[] appearances)
        {
            if (appearances == null || appearances.Length == 0) return;

            const int scale = 7;
            const int cardW = 208;
            const int cardH = 292;
            const int cols = 4;
            const int rows = 2;
            const int margin = 22;
            int width = margin * 2 + cardW * cols;
            int height = margin * 2 + cardH * rows;

            Color32 bg = Hex("#F8E8A8");
            Color32 ink = Hex("#34335F");
            Color32[] cardColors =
            {
                Hex("#BFE8C5"), Hex("#F8B6B8"), Hex("#F7D76E"), Hex("#BFCDF6"),
                Hex("#F7A277"), Hex("#81D8D2"), Hex("#E8B6D6"), Hex("#9BCB75")
            };

            Color32[] canvas = new Color32[width * height];
            for (int i = 0; i < canvas.Length; i++) canvas[i] = bg;

            for (int i = 0; i < appearances.Length && i < 8; i++)
            {
                int col = i % cols;
                int row = i / cols;
                int ox = margin + col * cardW;
                int oy = margin + (rows - 1 - row) * cardH;

                FillRect(canvas, width, height, ox + 4, oy + 4, cardW - 8, cardH - 8, cardColors[i % cardColors.Length]);
                FrameRect(canvas, width, height, ox + 4, oy + 4, cardW - 8, cardH - 8, ink, 4);

                Color32[] avatar = AvatarPixelComposer.ComposePixels(appearances[i], AvatarDirection.Down, 0);
                int avatarW = AvatarPixelComposer.Width * scale;
                int avatarH = AvatarPixelComposer.Height * scale;
                int ax = ox + (cardW - avatarW) / 2;
                int ay = oy + 28;

                BlitNearest(canvas, width, height, avatar, AvatarPixelComposer.Width, AvatarPixelComposer.Height, ax, ay, scale);

                // Small palette swatches communicate customization dimensions without text.
                Color32[] swatches =
                {
                    appearances[i].hair,
                    appearances[i].top,
                    appearances[i].bottom,
                    appearances[i].shoes,
                    appearances[i].accessory,
                    appearances[i].accent
                };
                int sw = 22;
                int sx = ox + 27;
                int sy = oy + cardH - 34;
                for (int s = 0; s < swatches.Length; s++)
                {
                    FillRect(canvas, width, height, sx + s * 27, sy, sw, 12, swatches[s]);
                    FrameRect(canvas, width, height, sx + s * 27, sy, sw, 12, ink, 1);
                }
            }

            Texture2D output = new Texture2D(width, height, TextureFormat.RGBA32, false);
            output.filterMode = FilterMode.Point;
            output.SetPixels32(canvas);
            output.Apply(false, false);

            string projectRoot = Directory.GetParent(Application.dataPath).FullName;
            string artifacts = Path.Combine(projectRoot, "BuildArtifacts");
            Directory.CreateDirectory(artifacts);
            File.WriteAllBytes(Path.Combine(artifacts, "avatar-showcase.png"), output.EncodeToPNG());
            UnityEngine.Object.DestroyImmediate(output);
        }

        private static void BlitNearest(Color32[] dst, int dw, int dh, Color32[] src, int sw, int sh, int ox, int oy, int scale)
        {
            for (int y = 0; y < sh; y++)
            {
                for (int x = 0; x < sw; x++)
                {
                    Color32 c = src[y * sw + x];
                    if (c.a == 0) continue;
                    for (int yy = 0; yy < scale; yy++)
                    {
                        for (int xx = 0; xx < scale; xx++)
                        {
                            int dx = ox + x * scale + xx;
                            int dy = oy + y * scale + yy;
                            if (dx >= 0 && dx < dw && dy >= 0 && dy < dh)
                                dst[dy * dw + dx] = c;
                        }
                    }
                }
            }
        }

        private static void FillRect(Color32[] p, int w, int h, int x, int y, int rw, int rh, Color32 c)
        {
            int x0 = Mathf.Clamp(x, 0, w);
            int x1 = Mathf.Clamp(x + rw, 0, w);
            int y0 = Mathf.Clamp(y, 0, h);
            int y1 = Mathf.Clamp(y + rh, 0, h);
            for (int yy = y0; yy < y1; yy++)
                for (int xx = x0; xx < x1; xx++)
                    p[yy * w + xx] = c;
        }

        private static void FrameRect(Color32[] p, int w, int h, int x, int y, int rw, int rh, Color32 c, int thickness)
        {
            FillRect(p, w, h, x, y, rw, thickness, c);
            FillRect(p, w, h, x, y + rh - thickness, rw, thickness, c);
            FillRect(p, w, h, x, y, thickness, rh, c);
            FillRect(p, w, h, x + rw - thickness, y, thickness, rh, c);
        }

        private static string Abs(string relative)
        {
            string root = Directory.GetParent(Application.dataPath).FullName;
            return Path.GetFullPath(Path.Combine(root, relative.Replace('/', Path.DirectorySeparatorChar)));
        }

        private static Color32 Hex(string hex)
        {
            Color c;
            if (!ColorUtility.TryParseHtmlString(hex, out c))
                return new Color32(255, 255, 255, 255);
            return (Color32)c;
        }
    }
}
