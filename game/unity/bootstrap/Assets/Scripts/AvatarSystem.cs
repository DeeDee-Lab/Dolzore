using System;
using System.Collections.Generic;
using UnityEngine;

namespace Dolzore
{
    public enum AvatarDirection
    {
        Down = 0,
        Left = 1,
        Right = 2,
        Up = 3
    }

    [Serializable]
    public sealed class AvatarAppearanceData
    {
        public int bodyShape = 0;
        public int faceStyle = 0;
        public int hairStyle = 0;
        public int topStyle = 0;
        public int bottomStyle = 0;
        public int shoeStyle = 0;
        public int accessoryStyle = 0;
        public int accentStyle = 0;

        public Color32 skin = new Color32(228, 180, 137, 255);
        public Color32 hair = new Color32(51, 42, 45, 255);
        public Color32 top = new Color32(88, 163, 148, 255);
        public Color32 bottom = new Color32(61, 75, 92, 255);
        public Color32 shoes = new Color32(239, 226, 190, 255);
        public Color32 accessory = new Color32(239, 224, 176, 255);
        public Color32 accent = new Color32(201, 110, 99, 255);

        public AvatarAppearanceData Clone()
        {
            return new AvatarAppearanceData
            {
                bodyShape = bodyShape,
                faceStyle = faceStyle,
                hairStyle = hairStyle,
                topStyle = topStyle,
                bottomStyle = bottomStyle,
                shoeStyle = shoeStyle,
                accessoryStyle = accessoryStyle,
                accentStyle = accentStyle,
                skin = skin,
                hair = hair,
                top = top,
                bottom = bottom,
                shoes = shoes,
                accessory = accessory,
                accent = accent
            };
        }
    }

    public static class AvatarPresets
    {
        public static AvatarAppearanceData Sora()
        {
            return new AvatarAppearanceData
            {
                bodyShape = 0,
                faceStyle = 0,
                hairStyle = 0,
                topStyle = 0,
                bottomStyle = 0,
                shoeStyle = 0,
                accessoryStyle = 1,
                accentStyle = 1,
                skin = Hex("#E5B58B"),
                hair = Hex("#332A2D"),
                top = Hex("#55B7A4"),
                bottom = Hex("#3D4B68"),
                shoes = Hex("#F0E4C7"),
                accessory = Hex("#EFE0B0"),
                accent = Hex("#E27367")
            };
        }

        public static AvatarAppearanceData Melo()
        {
            return new AvatarAppearanceData
            {
                bodyShape = 1,
                faceStyle = 1,
                hairStyle = 1,
                topStyle = 1,
                bottomStyle = 1,
                shoeStyle = 1,
                accessoryStyle = 2,
                accentStyle = 2,
                skin = Hex("#E2AF88"),
                hair = Hex("#7B453F"),
                top = Hex("#B65370"),
                bottom = Hex("#62405D"),
                shoes = Hex("#3E3442"),
                accessory = Hex("#F0E2BF"),
                accent = Hex("#F0BE4E")
            };
        }

        public static AvatarAppearanceData Yuzu()
        {
            return new AvatarAppearanceData
            {
                bodyShape = 0,
                faceStyle = 2,
                hairStyle = 2,
                topStyle = 2,
                bottomStyle = 2,
                shoeStyle = 2,
                accessoryStyle = 3,
                accentStyle = 3,
                skin = Hex("#E2B089"),
                hair = Hex("#332F35"),
                top = Hex("#E0B447"),
                bottom = Hex("#5F7B55"),
                shoes = Hex("#39393C"),
                accessory = Hex("#F0E4BF"),
                accent = Hex("#C45B6B")
            };
        }

        public static AvatarAppearanceData Pon()
        {
            return new AvatarAppearanceData
            {
                bodyShape = 2,
                faceStyle = 3,
                hairStyle = 3,
                topStyle = 0,
                bottomStyle = 3,
                shoeStyle = 3,
                accessoryStyle = 4,
                accentStyle = 4,
                skin = Hex("#DEAC84"),
                hair = Hex("#4A5E88"),
                top = Hex("#5A89CF"),
                bottom = Hex("#354B78"),
                shoes = Hex("#EEE0C4"),
                accessory = Hex("#B77A4D"),
                accent = Hex("#F28B42")
            };
        }

        public static AvatarAppearanceData Child()
        {
            AvatarAppearanceData a = Sora();
            a.bodyShape = 1;
            a.faceStyle = 1;
            a.hairStyle = 4;
            a.topStyle = 4;
            a.bottomStyle = 4;
            a.accessoryStyle = 0;
            a.accentStyle = 5;
            a.hair = Hex("#5B4238");
            a.top = Hex("#6A90C8");
            a.bottom = Hex("#D79B57");
            a.accent = Hex("#F3D76F");
            return a;
        }

        public static AvatarAppearanceData Worker()
        {
            AvatarAppearanceData a = Sora();
            a.bodyShape = 2;
            a.faceStyle = 2;
            a.hairStyle = 6;
            a.topStyle = 5;
            a.bottomStyle = 3;
            a.accessoryStyle = 0;
            a.accentStyle = 6;
            a.hair = Hex("#3A3A3E");
            a.top = Hex("#7F6D60");
            a.bottom = Hex("#4D5962");
            a.accent = Hex("#E3B74D");
            return a;
        }

        public static AvatarAppearanceData Elder()
        {
            AvatarAppearanceData a = Yuzu();
            a.bodyShape = 1;
            a.faceStyle = 0;
            a.hairStyle = 5;
            a.hair = Hex("#D8D5CF");
            a.top = Hex("#70806F");
            a.bottom = Hex("#4F5A56");
            a.accessoryStyle = 5;
            a.accessory = Hex("#D9C8A1");
            return a;
        }

        public static AvatarAppearanceData Visitor()
        {
            AvatarAppearanceData a = Melo();
            a.bodyShape = 0;
            a.hairStyle = 7;
            a.topStyle = 3;
            a.bottomStyle = 0;
            a.accessoryStyle = 6;
            a.accentStyle = 0;
            a.hair = Hex("#55495E");
            a.top = Hex("#5F7993");
            a.bottom = Hex("#433D5E");
            a.accessory = Hex("#99C8C5");
            return a;
        }

        private static Color32 Hex(string hex)
        {
            Color c;
            if (!ColorUtility.TryParseHtmlString(hex, out c))
                return new Color32(255, 255, 255, 255);
            return (Color32)c;
        }
    }

    public static class AvatarPixelComposer
    {
        public const int Width = 32;
        public const int Height = 40;
        public const float PixelsPerUnit = 30f;

        private static readonly Color32 Ink = new Color32(48, 48, 93, 255);
        private static readonly Color32 Eye = new Color32(42, 37, 49, 255);
        private static readonly Color32 Mouth = new Color32(124, 72, 75, 255);

        public static Color32[] ComposePixels(AvatarAppearanceData appearance, AvatarDirection direction, int walkFrame)
        {
            if (appearance == null) appearance = AvatarPresets.Sora();

            Color32[] p = Transparent(Width * Height);
            bool side = direction == AvatarDirection.Left || direction == AvatarDirection.Right;
            bool back = direction == AvatarDirection.Up;
            int step = walkFrame & 1;

            DrawGroundShadow(p, side, step);
            DrawLegs(p, appearance, side, step);
            DrawTorso(p, appearance, side, step);
            DrawArms(p, appearance, side, step);
            DrawHead(p, appearance, side, back);
            DrawHair(p, appearance, direction);
            DrawAccessory(p, appearance, direction);
            DrawAccent(p, appearance, direction);

            p = AddSilhouetteOutline(p, Width, Height, Ink);

            if (direction == AvatarDirection.Right)
                p = MirrorHorizontal(p, Width, Height);

            return p;
        }

        public static Texture2D CreateTexture(AvatarAppearanceData appearance, AvatarDirection direction, int walkFrame)
        {
            Texture2D tex = new Texture2D(Width, Height, TextureFormat.RGBA32, false);
            tex.name = "DOLZORE_Avatar";
            tex.filterMode = FilterMode.Point;
            tex.wrapMode = TextureWrapMode.Clamp;
            tex.SetPixels32(ComposePixels(appearance, direction, walkFrame));
            tex.Apply(false, false);
            return tex;
        }

        public static Sprite CreateRuntimeSprite(AvatarAppearanceData appearance, AvatarDirection direction, int walkFrame)
        {
            Texture2D tex = CreateTexture(appearance, direction, walkFrame);
            Sprite sprite = Sprite.Create(
                tex,
                new Rect(0f, 0f, Width, Height),
                new Vector2(0.5f, 0.08f),
                PixelsPerUnit,
                0,
                SpriteMeshType.FullRect);
            sprite.name = "DOLZORE_Avatar_" + direction + "_" + walkFrame;
            return sprite;
        }

        private static void DrawGroundShadow(Color32[] p, bool side, int step)
        {
            Color32 shadow = new Color32(43, 40, 78, 72);
            int cx = side ? 15 : 16;
            for (int y = -2; y <= 2; y++)
            {
                for (int x = -8; x <= 8; x++)
                {
                    float nx = x / 8f;
                    float ny = y / 2f;
                    if (nx * nx + ny * ny <= 1f)
                        Set(p, cx + x, 2 + y, shadow);
                }
            }
        }

        private static void DrawLegs(Color32[] p, AvatarAppearanceData a, bool side, int step)
        {
            int lead = step == 0 ? 0 : 1;
            int trail = step == 0 ? 1 : 0;

            if (a.bottomStyle == 2) // long skirt
            {
                Rect(p, 11, 6, 10, 8, a.bottom);
                Rect(p, 10, 7, 12, 4, Lighten(a.bottom, 1.05f));
                Rect(p, 12, 4, 8, 3, Darken(a.bottom, 0.82f));
            }
            else if (a.bottomStyle == 4) // shorts
            {
                Rect(p, 10, 10, 5, 4, a.bottom);
                Rect(p, 17, 10, 5, 4, a.bottom);
                Rect(p, 11 - lead, 5, 3, 6, a.skin);
                Rect(p, 18 + trail, 5, 3, 6, a.skin);
            }
            else if (a.bottomStyle == 3) // cargo
            {
                Rect(p, 9 - lead, 5, 5, 9, a.bottom);
                Rect(p, 18 + trail, 5, 5, 9, a.bottom);
                Rect(p, 8, 10, 4, 3, Darken(a.bottom, 0.74f));
                Rect(p, 21, 10, 4, 3, Darken(a.bottom, 0.74f));
                Set(p, 10, 11, Lighten(a.bottom, 1.16f));
                Set(p, 22, 11, Lighten(a.bottom, 1.16f));
            }
            else if (a.bottomStyle == 1) // skirt + leggings
            {
                Rect(p, 10, 10, 12, 5, a.bottom);
                Rect(p, 9, 9, 14, 2, Lighten(a.bottom, 1.08f));
                Rect(p, 11 - lead, 4, 4, 7, Darken(a.bottom, 0.66f));
                Rect(p, 18 + trail, 4, 4, 7, Darken(a.bottom, 0.66f));
            }
            else // tapered pants
            {
                Rect(p, 10 - lead, 5, 4, 9, a.bottom);
                Rect(p, 18 + trail, 5, 4, 9, a.bottom);
                Rect(p, 11, 12, 10, 2, Lighten(a.bottom, 1.06f));
            }

            DrawShoes(p, a, side, step);
        }

        private static void DrawShoes(Color32[] p, AvatarAppearanceData a, bool side, int step)
        {
            int lead = step == 0 ? 0 : 1;
            int trail = step == 0 ? 1 : 0;

            if (a.shoeStyle == 1) // boots
            {
                Rect(p, 9 - lead, 2, 6, 4, a.shoes);
                Rect(p, 18 + trail, 2, 6, 4, a.shoes);
                Rect(p, 10 - lead, 2, 6, 1, Darken(a.shoes, 0.52f));
                Rect(p, 19 + trail, 2, 6, 1, Darken(a.shoes, 0.52f));
            }
            else if (a.shoeStyle == 2) // flats
            {
                Rect(p, 10 - lead, 2, 5, 3, a.shoes);
                Rect(p, 18 + trail, 2, 5, 3, a.shoes);
                Set(p, 10 - lead, 4, Lighten(a.shoes, 1.12f));
                Set(p, 18 + trail, 4, Lighten(a.shoes, 1.12f));
            }
            else if (a.shoeStyle == 3) // high-top
            {
                Rect(p, 9 - lead, 2, 6, 4, a.shoes);
                Rect(p, 18 + trail, 2, 6, 4, a.shoes);
                Rect(p, 11 - lead, 4, 3, 1, Ink);
                Rect(p, 20 + trail, 4, 3, 1, Ink);
            }
            else // sneaker
            {
                Rect(p, 9 - lead, 2, 6, 3, a.shoes);
                Rect(p, 18 + trail, 2, 6, 3, a.shoes);
                Rect(p, 10 - lead, 2, 6, 1, Ink);
                Rect(p, 19 + trail, 2, 6, 1, Ink);
                Set(p, 13 - lead, 4, Lighten(a.shoes, 1.18f));
                Set(p, 22 + trail, 4, Lighten(a.shoes, 1.18f));
            }
        }

        private static void DrawTorso(Color32[] p, AvatarAppearanceData a, bool side, int step)
        {
            int shoulderWidth = a.bodyShape == 1 ? 12 : (a.bodyShape == 2 ? 16 : 14);
            int shoulderLeft = 16 - shoulderWidth / 2;
            int waistWidth = Mathf.Max(9, shoulderWidth - 3);
            int waistLeft = 16 - waistWidth / 2;
            int hemY = a.topStyle == 5 ? 11 : 13;
            int shoulderY = 22;

            // Shoulder -> waist taper is more character-like than one rectangle.
            Rect(p, shoulderLeft, shoulderY - 3, shoulderWidth, 4, a.top);
            Rect(p, shoulderLeft + 1, shoulderY - 7, shoulderWidth - 2, 4, a.top);
            Rect(p, waistLeft, hemY, waistWidth, Mathf.Max(3, shoulderY - 7 - hemY + 1), a.top);

            // Neck/collar keeps head connected to the body.
            Rect(p, 14, 23, 5, 2, a.skin);

            if (a.topStyle == 0) // short jacket
            {
                Rect(p, shoulderLeft + 2, 20, shoulderWidth - 4, 2, Lighten(a.top, 1.15f));
                Rect(p, 15, 14, 2, 7, Darken(a.top, 0.70f));
                Rect(p, 17, 14, 1, 7, a.accent);
                Rect(p, waistLeft + 1, hemY, waistWidth - 2, 2, Darken(a.top, 0.80f));
            }
            else if (a.topStyle == 1) // overshirt
            {
                Rect(p, shoulderLeft + 1, 19, shoulderWidth - 2, 3, Lighten(a.top, 1.10f));
                Rect(p, 13, 17, 6, 5, new Color32(244, 231, 198, 255));
                Rect(p, 15, hemY + 1, 2, 8, Darken(a.top, 0.72f));
            }
            else if (a.topStyle == 2) // cardigan
            {
                Rect(p, 13, 18, 6, 4, new Color32(246, 234, 202, 255));
                Rect(p, 14, hemY + 1, 2, 9, Darken(a.top, 0.72f));
                Rect(p, 17, hemY + 1, 2, 9, Darken(a.top, 0.72f));
                Set(p, 16, 17, a.accent);
            }
            else if (a.topStyle == 3) // hoodie
            {
                Rect(p, shoulderLeft + 2, 21, shoulderWidth - 4, 3, Darken(a.top, 0.76f));
                Rect(p, 12, 21, 8, 2, Lighten(a.top, 1.14f));
                Rect(p, 13, hemY + 2, 7, 3, Lighten(a.top, 1.08f));
                Set(p, 15, 19, a.accent);
                Set(p, 18, 19, a.accent);
            }
            else if (a.topStyle == 4) // casual tee
            {
                Rect(p, shoulderLeft + 2, 19, shoulderWidth - 4, 2, Lighten(a.top, 1.18f));
                Rect(p, 13, 17, 6, 2, a.accent);
            }
            else // work vest
            {
                Rect(p, shoulderLeft + 1, 20, shoulderWidth - 2, 2, Lighten(a.top, 1.10f));
                Rect(p, 15, hemY + 1, 2, 10, Lighten(a.top, 1.12f));
                Rect(p, 11, hemY + 3, 4, 3, Darken(a.top, 0.72f));
                Rect(p, 18, hemY + 3, 4, 3, Darken(a.top, 0.72f));
            }
        }

        private static void DrawArms(Color32[] p, AvatarAppearanceData a, bool side, int step)
        {
            Color32 sleeveLight = Lighten(a.top, 1.08f);
            Color32 sleeveShade = Darken(a.top, 0.76f);

            int leftY = 15 + (step == 0 ? 1 : 0);
            int rightY = 15 + (step == 0 ? 0 : 1);

            if (side)
            {
                Rect(p, 8, leftY, 3, 7, sleeveLight);
                Rect(p, 21, rightY, 3, 7, sleeveShade);
                Rect(p, 8, leftY - 3, 3, 4, a.skin);
                Rect(p, 21, rightY - 3, 3, 4, a.skin);
                Set(p, 9, leftY - 3, Lighten(a.skin, 1.06f));
            }
            else
            {
                Rect(p, 7, leftY, 3, 7, sleeveLight);
                Rect(p, 22, rightY, 3, 7, sleeveShade);
                Rect(p, 7, leftY - 3, 3, 4, a.skin);
                Rect(p, 22, rightY - 3, 3, 4, a.skin);
                Set(p, 8, leftY - 3, Lighten(a.skin, 1.06f));
                Set(p, 23, rightY - 3, Lighten(a.skin, 1.04f));
            }
        }

        private static void DrawHead(Color32[] p, AvatarAppearanceData a, bool side, bool back)
        {
            int cx = side ? 15 : 16;

            // Rounded head with a slightly narrower jaw.
            Circle(p, cx, 29, 7, a.skin);
            Rect(p, 11, 26, 10, 6, a.skin);
            Rect(p, 13, 23, 6, 4, a.skin);

            if (!side)
            {
                Rect(p, 9, 28, 2, 3, a.skin);
                Rect(p, 21, 28, 2, 3, a.skin);
            }

            if (back) return;

            Color32 brow = Darken(a.hair, 0.62f);
            Color32 cheek = Lighten(a.skin, 1.06f);

            if (side)
            {
                Set(p, 11, 30, brow);
                Set(p, 11, 28, Eye);
                Set(p, 12, 28, Lighten(a.skin, 1.12f));
                Set(p, 10, 26, Mouth);
                Set(p, 12, 25, cheek);
                return;
            }

            // Brows create expression; one-pixel eyes avoid the previous square-black look.
            Set(p, 12, 31, brow);
            Set(p, 19, 31, brow);
            Set(p, 13, 29, Eye);
            Set(p, 19, 29, Eye);
            Set(p, 14, 29, Lighten(a.skin, 1.12f));
            Set(p, 20, 29, Lighten(a.skin, 1.12f));
            Set(p, 16, 27, Darken(a.skin, 0.92f));
            Set(p, 11, 27, cheek);
            Set(p, 21, 27, cheek);

            if (a.faceStyle == 1) // warm smile
            {
                Set(p, 15, 25, Mouth);
                Set(p, 16, 24, Mouth);
                Set(p, 17, 25, Mouth);
            }
            else if (a.faceStyle == 2) // calm
            {
                Set(p, 15, 25, Mouth);
                Set(p, 16, 25, Mouth);
            }
            else if (a.faceStyle == 3) // focused / adventurous
            {
                Set(p, 12, 31, Darken(brow, 0.84f));
                Set(p, 20, 31, Darken(brow, 0.84f));
                Set(p, 16, 25, Mouth);
            }
            else
            {
                Set(p, 16, 25, Mouth);
            }
        }

        private static void DrawHair(Color32[] p, AvatarAppearanceData a, AvatarDirection direction)
        {
            bool back = direction == AvatarDirection.Up;
            int s = a.hairStyle;

            if (s == 1)
            {
                Circle(p, 15, 34, 8, a.hair);
                Rect(p, 7, 28, 6, 9, a.hair);
                Rect(p, 19, 26, 7, 11, a.hair);
                Rect(p, 22, 23, 4, 7, a.hair);
            }
            else if (s == 2)
            {
                Circle(p, 16, 34, 8, a.hair);
                Rect(p, 7, 28, 5, 10, a.hair);
                Rect(p, 20, 28, 5, 10, a.hair);
                Circle(p, 8, 26, 3, a.hair);
                Circle(p, 24, 26, 3, a.hair);
            }
            else if (s == 3)
            {
                Circle(p, 16, 34, 7, a.hair);
                Rect(p, 9, 32, 3, 7, a.hair);
                Rect(p, 20, 31, 3, 8, a.hair);
                Rect(p, 12, 37, 3, 3, a.hair);
                Rect(p, 17, 38, 3, 2, a.hair);
                Rect(p, 22, 35, 3, 3, a.hair);
            }
            else if (s == 4)
            {
                Circle(p, 16, 34, 7, a.hair);
                Rect(p, 9, 31, 4, 5, a.hair);
                Rect(p, 20, 31, 3, 5, a.hair);
            }
            else if (s == 5)
            {
                Circle(p, 16, 34, 7, a.hair);
                Circle(p, 21, 38, 4, a.hair);
                Rect(p, 9, 30, 4, 6, a.hair);
            }
            else if (s == 6)
            {
                Circle(p, 16, 34, 7, a.hair);
                Rect(p, 9, 30, 5, 7, a.hair);
                Rect(p, 14, 34, 9, 4, Lighten(a.hair, 1.08f));
            }
            else if (s == 7)
            {
                Circle(p, 16, 34, 8, a.hair);
                Circle(p, 10, 31, 4, a.hair);
                Circle(p, 22, 31, 4, a.hair);
                Circle(p, 13, 38, 4, a.hair);
                Circle(p, 20, 38, 4, a.hair);
            }
            else
            {
                Circle(p, 16, 34, 7, a.hair);
                Rect(p, 9, 30, 5, 7, a.hair);
                Rect(p, 18, 31, 5, 6, a.hair);
                Rect(p, 12, 37, 3, 3, a.hair);
                Rect(p, 15, 38, 3, 2, a.hair);
            }

            Color32 hairLight = Lighten(a.hair, 1.18f);
            if (s == 1) { Rect(p, 10, 36, 5, 1, hairLight); Rect(p, 21, 31, 2, 3, hairLight); }
            else if (s == 2) { Rect(p, 12, 38, 7, 1, hairLight); Set(p, 8, 29, hairLight); }
            else if (s == 3) { Rect(p, 13, 37, 4, 1, hairLight); Set(p, 21, 34, hairLight); }
            else if (s == 5) { Rect(p, 13, 37, 5, 1, hairLight); Set(p, 22, 38, hairLight); }
            else if (s == 7) { Set(p, 10, 35, hairLight); Set(p, 20, 37, hairLight); Set(p, 23, 31, hairLight); }
            else { Rect(p, 13, 37, 5, 1, hairLight); }

            if (!back)
            {
                Rect(p, 10, 31, 12, 4, a.hair);
                if (s == 0)
                    Rect(p, 17, 34, 4, 5, a.hair);
            }
            else
            {
                Rect(p, 9, 27, 14, 8, a.hair);
                Rect(p, 10, 27, 12, 3, Darken(a.hair, 0.72f));
                Rect(p, 12, 32, 8, 2, hairLight);
                Set(p, 9, 30, Darken(a.hair, 0.66f));
                Set(p, 22, 30, Darken(a.hair, 0.66f));
            }
        }

        private static void DrawAccessory(Color32[] p, AvatarAppearanceData a, AvatarDirection direction)
        {
            int s = a.accessoryStyle;
            if (s == 1)
            {
                Line(p, 10, 22, 20, 11, a.accessory);
                Line(p, 11, 22, 21, 11, a.accessory);
            }
            else if (s == 2)
            {
                if (direction == AvatarDirection.Up)
                {
                    Line(p, 21, 23, 11, 12, a.accessory);
                    Rect(p, 7, 10, 8, 9, a.accessory);
                    Rect(p, 9, 12, 4, 5, Lighten(a.accessory, 1.10f));
                }
                else
                {
                    Line(p, 12, 23, 22, 11, a.accessory);
                    Rect(p, 20, 9, 7, 9, a.accessory);
                }
            }
            else if (s == 3)
            {
                Rect(p, 22, 10, 7, 10, a.accessory);
                Rect(p, 23, 12, 5, 6, Lighten(a.accessory, 1.12f));
            }
            else if (s == 4)
            {
                if (direction == AvatarDirection.Up)
                    Rect(p, 10, 13, 13, 13, a.accessory);
                else
                {
                    Rect(p, 22, 10, 8, 13, a.accessory);
                    Line(p, 18, 23, 25, 17, a.accessory);
                }
            }
            else if (s == 5)
            {
                Rect(p, 11, 29, 5, 3, a.accessory);
                Rect(p, 18, 29, 5, 3, a.accessory);
                Rect(p, 16, 30, 2, 1, a.accessory);
            }
            else if (s == 6)
            {
                Circle(p, 9, 30, 3, a.accessory);
                Circle(p, 23, 30, 3, a.accessory);
                Rect(p, 9, 34, 15, 2, a.accessory);
            }
            else if (s == 7)
            {
                Rect(p, 8, 35, 16, 4, a.accessory);
                Rect(p, 18, 33, 9, 3, a.accessory);
            }
            else if (s == 8)
            {
                Rect(p, 20, 35, 5, 3, a.accessory);
                Rect(p, 23, 33, 3, 5, a.accessory);
            }
        }

        private static void DrawAccent(Color32[] p, AvatarAppearanceData a, AvatarDirection direction)
        {
            int s = a.accentStyle;
            if (s == 1)
            {
                Rect(p, 20, 10, 6, 6, a.accent);
            }
            else if (s == 2)
            {
                Rect(p, 23, 34, 4, 3, a.accent);
            }
            else if (s == 3)
            {
                Rect(p, 27, 12, 2, 4, a.accent);
            }
            else if (s == 4)
            {
                Rect(p, 8, 22, 16, 3, a.accent);
                Rect(p, 21, 17, 4, 7, a.accent);
            }
            else if (s == 5)
            {
                Circle(p, 15, 19, 2, a.accent);
            }
            else if (s == 6)
            {
                Rect(p, 5, 18, 4, 5, a.accent);
            }
        }

        private static Color32[] AddSilhouetteOutline(Color32[] source, int w, int h, Color32 outline)
        {
            Color32[] result = (Color32[])source.Clone();
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
                            int nx = x + ox;
                            int ny = y + oy;
                            if (nx < 0 || nx >= w || ny < 0 || ny >= h) continue;
                            if (source[ny * w + nx].a > 200)
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

        private static Color32[] MirrorHorizontal(Color32[] source, int w, int h)
        {
            Color32[] result = new Color32[source.Length];
            for (int y = 0; y < h; y++)
                for (int x = 0; x < w; x++)
                    result[y * w + (w - 1 - x)] = source[y * w + x];
            return result;
        }

        private static void Set(Color32[] p, int x, int y, Color32 c)
        {
            if (x < 0 || x >= Width || y < 0 || y >= Height) return;
            p[y * Width + x] = c;
        }

        private static void Rect(Color32[] p, int x, int y, int w, int h, Color32 c)
        {
            for (int yy = y; yy < y + h; yy++)
                for (int xx = x; xx < x + w; xx++)
                    Set(p, xx, yy, c);
        }

        private static void Circle(Color32[] p, int cx, int cy, int r, Color32 c)
        {
            int rr = r * r;
            for (int y = -r; y <= r; y++)
                for (int x = -r; x <= r; x++)
                    if (x * x + y * y <= rr)
                        Set(p, cx + x, cy + y, c);
        }

        private static void Line(Color32[] p, int x0, int y0, int x1, int y1, Color32 c)
        {
            int dx = Math.Abs(x1 - x0);
            int sx = x0 < x1 ? 1 : -1;
            int dy = -Math.Abs(y1 - y0);
            int sy = y0 < y1 ? 1 : -1;
            int err = dx + dy;
            while (true)
            {
                Set(p, x0, y0, c);
                if (x0 == x1 && y0 == y1) break;
                int e2 = 2 * err;
                if (e2 >= dy) { err += dy; x0 += sx; }
                if (e2 <= dx) { err += dx; y0 += sy; }
            }
        }

        private static Color32[] Transparent(int count)
        {
            Color32[] p = new Color32[count];
            for (int i = 0; i < count; i++) p[i] = new Color32(0, 0, 0, 0);
            return p;
        }

        private static Color32 Darken(Color32 c, float factor)
        {
            return new Color32(
                (byte)Mathf.Clamp(Mathf.RoundToInt(c.r * factor), 0, 255),
                (byte)Mathf.Clamp(Mathf.RoundToInt(c.g * factor), 0, 255),
                (byte)Mathf.Clamp(Mathf.RoundToInt(c.b * factor), 0, 255),
                c.a);
        }

        private static Color32 Lighten(Color32 c, float factor)
        {
            return new Color32(
                (byte)Mathf.Clamp(Mathf.RoundToInt(c.r * factor), 0, 255),
                (byte)Mathf.Clamp(Mathf.RoundToInt(c.g * factor), 0, 255),
                (byte)Mathf.Clamp(Mathf.RoundToInt(c.b * factor), 0, 255),
                c.a);
        }
    }

    public sealed class AvatarRuntimeRenderer : MonoBehaviour
    {
        public AvatarAppearanceData appearance = new AvatarAppearanceData();
        public SpriteRenderer targetRenderer;
        public Rigidbody2D movementBody;
        public AvatarDirection direction = AvatarDirection.Down;
        public float animationStepSeconds = 0.18f;

        private int walkFrame;
        private float stepTimer;
        private Vector2 lastPosition;
        private readonly Dictionary<string, Sprite> cache = new Dictionary<string, Sprite>();

        private void Awake()
        {
            if (targetRenderer == null)
                targetRenderer = GetComponent<SpriteRenderer>();
            if (movementBody == null)
                movementBody = GetComponent<Rigidbody2D>();
            lastPosition = transform.position;
            RefreshSprite();
        }

        private void Update()
        {
            Vector2 velocity = movementBody != null
                ? movementBody.linearVelocity
                : ((Vector2)transform.position - lastPosition) / Mathf.Max(Time.deltaTime, 0.0001f);

            lastPosition = transform.position;

            if (velocity.sqrMagnitude > 0.01f)
            {
                if (Mathf.Abs(velocity.x) > Mathf.Abs(velocity.y))
                    direction = velocity.x < 0f ? AvatarDirection.Left : AvatarDirection.Right;
                else
                    direction = velocity.y < 0f ? AvatarDirection.Down : AvatarDirection.Up;

                stepTimer += Time.deltaTime;
                if (stepTimer >= animationStepSeconds)
                {
                    stepTimer = 0f;
                    walkFrame = 1 - walkFrame;
                    RefreshSprite();
                }
            }
            else if (walkFrame != 0)
            {
                walkFrame = 0;
                stepTimer = 0f;
                RefreshSprite();
            }
        }

        public void SetAppearance(AvatarAppearanceData newAppearance)
        {
            appearance = newAppearance != null ? newAppearance.Clone() : AvatarPresets.Sora();
            ClearCache();
            RefreshSprite();
        }

        public void RefreshSprite()
        {
            if (targetRenderer == null) return;
            string key = AppearanceKey(appearance) + "|" + direction + "|" + walkFrame;

            Sprite sprite;
            if (!cache.TryGetValue(key, out sprite) || sprite == null)
            {
                sprite = AvatarPixelComposer.CreateRuntimeSprite(appearance, direction, walkFrame);
                cache[key] = sprite;
            }
            targetRenderer.sprite = sprite;
        }

        private void ClearCache()
        {
            foreach (KeyValuePair<string, Sprite> item in cache)
            {
                if (item.Value != null)
                {
                    Texture texture = item.Value.texture;
                    Destroy(item.Value);
                    if (texture != null) Destroy(texture);
                }
            }
            cache.Clear();
        }

        private static string AppearanceKey(AvatarAppearanceData a)
        {
            return string.Join(":",
                a.bodyShape, a.faceStyle, a.hairStyle, a.topStyle, a.bottomStyle,
                a.shoeStyle, a.accessoryStyle, a.accentStyle,
                ColorUtility.ToHtmlStringRGBA(a.skin),
                ColorUtility.ToHtmlStringRGBA(a.hair),
                ColorUtility.ToHtmlStringRGBA(a.top),
                ColorUtility.ToHtmlStringRGBA(a.bottom),
                ColorUtility.ToHtmlStringRGBA(a.shoes),
                ColorUtility.ToHtmlStringRGBA(a.accessory),
                ColorUtility.ToHtmlStringRGBA(a.accent));
        }
    }
}
