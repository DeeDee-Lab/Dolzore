using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

namespace Dolzore.Editor
{
    public static class AvatarQualityVerifier
    {
        public static void AssertAndWriteReceipt()
        {
            Dictionary<string, AvatarAppearanceData> core = new Dictionary<string, AvatarAppearanceData>
            {
                { "SORA", AvatarPresets.Sora() },
                { "MELO", AvatarPresets.Melo() },
                { "YUZU", AvatarPresets.Yuzu() },
                { "PON", AvatarPresets.Pon() }
            };

            Dictionary<string, string> signatures = new Dictionary<string, string>();
            Dictionary<string, int> opaqueCounts = new Dictionary<string, int>();

            foreach (KeyValuePair<string, AvatarAppearanceData> item in core)
            {
                Color32[] pixels = AvatarPixelComposer.ComposePixels(item.Value, AvatarDirection.Down, 0);
                string sig = SilhouetteSha(pixels);
                int opaque = CountOpaque(pixels);

                if (opaque < 160)
                    throw new InvalidOperationException("DOLZORE_AVATAR_TOO_SPARSE:" + item.Key);

                foreach (KeyValuePair<string, string> prior in signatures)
                {
                    if (prior.Value == sig)
                        throw new InvalidOperationException("DOLZORE_AVATAR_DUPLICATE_SILHOUETTE:" + prior.Key + ":" + item.Key);
                }

                signatures[item.Key] = sig;
                opaqueCounts[item.Key] = opaque;
                Debug.Log("DOLZORE_AVATAR_SILHOUETTE_" + item.Key + "=PASS sha=" + sig);
            }

            // The canonical four must also differ in at least hair, top/bottom, and accessory grammar.
            AssertDifferent("SORA_MELO_HAIR", core["SORA"].hairStyle, core["MELO"].hairStyle);
            AssertDifferent("SORA_YUZU_HAIR", core["SORA"].hairStyle, core["YUZU"].hairStyle);
            AssertDifferent("SORA_PON_HAIR", core["SORA"].hairStyle, core["PON"].hairStyle);
            AssertDifferent("MELO_YUZU_BOTTOM", core["MELO"].bottomStyle, core["YUZU"].bottomStyle);
            AssertDifferent("YUZU_PON_ACCESSORY", core["YUZU"].accessoryStyle, core["PON"].accessoryStyle);

            string projectRoot = Directory.GetParent(Application.dataPath).FullName;
            string artifactDir = Path.Combine(projectRoot, "BuildArtifacts");
            Directory.CreateDirectory(artifactDir);
            string path = Path.Combine(artifactDir, "avatar-quality.json");

            StringBuilder sb = new StringBuilder();
            sb.Append("{\n");
            sb.Append("  \"schema\": \"dolzore.avatar.quality.v1\",\n");
            sb.Append("  \"native_size\": [32, 40],\n");
            sb.Append("  \"core\": {\n");

            int index = 0;
            foreach (KeyValuePair<string, AvatarAppearanceData> item in core)
            {
                sb.Append("    \"").Append(item.Key).Append("\": { ");
                sb.Append("\"silhouette_sha256\": \"").Append(signatures[item.Key]).Append("\", ");
                sb.Append("\"opaque_pixels\": ").Append(opaqueCounts[item.Key]).Append(", ");
                sb.Append("\"hair_style\": ").Append(item.Value.hairStyle).Append(", ");
                sb.Append("\"top_style\": ").Append(item.Value.topStyle).Append(", ");
                sb.Append("\"bottom_style\": ").Append(item.Value.bottomStyle).Append(", ");
                sb.Append("\"accessory_style\": ").Append(item.Value.accessoryStyle).Append(" }");
                index++;
                sb.Append(index < core.Count ? ",\n" : "\n");
            }

            sb.Append("  },\n");
            sb.Append("  \"result\": \"PASS\"\n");
            sb.Append("}\n");

            File.WriteAllText(path, sb.ToString(), new UTF8Encoding(false));
            Debug.Log("DOLZORE_AVATAR_QUALITY_RECEIPT=" + path);
            Debug.Log("DOLZORE_AVATAR_QUALITY_SUCCESS");
        }

        private static void AssertDifferent(string name, int a, int b)
        {
            if (a == b)
                throw new InvalidOperationException("DOLZORE_AVATAR_STYLE_NOT_DISTINCT:" + name);
        }

        private static int CountOpaque(Color32[] pixels)
        {
            int count = 0;
            for (int i = 0; i < pixels.Length; i++)
                if (pixels[i].a > 48) count++;
            return count;
        }

        private static string SilhouetteSha(Color32[] pixels)
        {
            byte[] mask = new byte[pixels.Length];
            for (int i = 0; i < pixels.Length; i++)
                mask[i] = pixels[i].a > 48 ? (byte)1 : (byte)0;

            using (SHA256 sha = SHA256.Create())
            {
                byte[] hash = sha.ComputeHash(mask);
                StringBuilder sb = new StringBuilder(hash.Length * 2);
                for (int i = 0; i < hash.Length; i++)
                    sb.Append(hash[i].ToString("x2"));
                return sb.ToString();
            }
        }
    }
}
