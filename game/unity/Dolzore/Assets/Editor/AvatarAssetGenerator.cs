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
