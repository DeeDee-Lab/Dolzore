using System;
using UnityEngine;

namespace Dolzore
{
    public enum AvatarFacing { Down, Side, Up }
    public enum AvatarHairStyle { SoftRound, AsymBob, Wavy, Tousled, ShortCrop }
    public enum AvatarTopStyle { ShortJacket, Overshirt, Cardigan, Hoodie, WorkVest }
    public enum AvatarBottomStyle { Tapered, SkirtLeggings, LongSkirt, Cargo }
    public enum AvatarShoeStyle { Sneakers, Boots, Flats }
    public enum AvatarAccessoryStyle { None, CrossbodyPouch, RecordBag, Notebook, ScarfBackpack, HairClip, Headphones }

    [Serializable]
    public sealed class AvatarAppearanceData
    {
        public string avatarId = "avatar.custom";
        public Color skinColor = new Color(0.88f, 0.65f, 0.48f, 1f);
        public AvatarHairStyle hairStyle = AvatarHairStyle.SoftRound;
        public Color hairColor = new Color(0.18f, 0.14f, 0.16f, 1f);
        public AvatarTopStyle topStyle = AvatarTopStyle.ShortJacket;
        public Color topColor = new Color(0.34f, 0.64f, 0.58f, 1f);
        public Color topAccentColor = new Color(0.93f, 0.85f, 0.66f, 1f);
        public AvatarBottomStyle bottomStyle = AvatarBottomStyle.Tapered;
        public Color bottomColor = new Color(0.20f, 0.27f, 0.37f, 1f);
        public AvatarShoeStyle shoeStyle = AvatarShoeStyle.Sneakers;
        public Color shoeColor = new Color(0.94f, 0.91f, 0.82f, 1f);
        public AvatarAccessoryStyle accessoryStyle = AvatarAccessoryStyle.CrossbodyPouch;
        public Color accessoryColor = new Color(0.89f, 0.39f, 0.35f, 1f);
    }

    public static class AvatarProfileLibrary
    {
        public static AvatarAppearanceData Sora => new AvatarAppearanceData
        {
            avatarId = "avatar.sora",
            skinColor = Hex("#E6B18A"),
            hairStyle = AvatarHairStyle.SoftRound,
            hairColor = Hex("#352B35"),
            topStyle = AvatarTopStyle.ShortJacket,
            topColor = Hex("#4FB4A6"),
            topAccentColor = Hex("#F4E0A8"),
            bottomStyle = AvatarBottomStyle.Tapered,
            bottomColor = Hex("#354763"),
            shoeStyle = AvatarShoeStyle.Sneakers,
            shoeColor = Hex("#FFF1D3"),
            accessoryStyle = AvatarAccessoryStyle.CrossbodyPouch,
            accessoryColor = Hex("#E86E68")
        };

        public static AvatarAppearanceData Melo => new AvatarAppearanceData
        {
            avatarId = "avatar.melo",
            skinColor = Hex("#E6AE87"),
            hairStyle = AvatarHairStyle.AsymBob,
            hairColor = Hex("#8C493F"),
            topStyle = AvatarTopStyle.Overshirt,
            topColor = Hex("#B64965"),
            topAccentColor = Hex("#F4E6BF"),
            bottomStyle = AvatarBottomStyle.SkirtLeggings,
            bottomColor = Hex("#543B55"),
            shoeStyle = AvatarShoeStyle.Flats,
            shoeColor = Hex("#2F2A38"),
            accessoryStyle = AvatarAccessoryStyle.RecordBag,
            accessoryColor = Hex("#F0B347")
        };

        public static AvatarAppearanceData Yuzu => new AvatarAppearanceData
        {
            avatarId = "avatar.yuzu",
            skinColor = Hex("#E1AD84"),
            hairStyle = AvatarHairStyle.Wavy,
            hairColor = Hex("#342E3C"),
            topStyle = AvatarTopStyle.Cardigan,
            topColor = Hex("#E2B84E"),
            topAccentColor = Hex("#FFF0C0"),
            bottomStyle = AvatarBottomStyle.LongSkirt,
            bottomColor = Hex("#65794E"),
            shoeStyle = AvatarShoeStyle.Flats,
            shoeColor = Hex("#312E38"),
            accessoryStyle = AvatarAccessoryStyle.Notebook,
            accessoryColor = Hex("#F7EDCF")
        };

        public static AvatarAppearanceData Pon => new AvatarAppearanceData
        {
            avatarId = "avatar.pon",
            skinColor = Hex("#DFAA82"),
            hairStyle = AvatarHairStyle.Tousled,
            hairColor = Hex("#536A96"),
            topStyle = AvatarTopStyle.ShortJacket,
            topColor = Hex("#3E75B4"),
            topAccentColor = Hex("#F38A3E"),
            bottomStyle = AvatarBottomStyle.Cargo,
            bottomColor = Hex("#30486E"),
            shoeStyle = AvatarShoeStyle.Sneakers,
            shoeColor = Hex("#F2E1BC"),
            accessoryStyle = AvatarAccessoryStyle.ScarfBackpack,
            accessoryColor = Hex("#F1873F")
        };

        public static AvatarAppearanceData ResidentChild => new AvatarAppearanceData
        {
            avatarId = "avatar.resident.child",
            skinColor = Hex("#E7B28A"),
            hairStyle = AvatarHairStyle.ShortCrop,
            hairColor = Hex("#5A4035"),
            topStyle = AvatarTopStyle.Hoodie,
            topColor = Hex("#6E8DD1"),
            topAccentColor = Hex("#FFD36A"),
            bottomStyle = AvatarBottomStyle.Cargo,
            bottomColor = Hex("#40537A"),
            shoeStyle = AvatarShoeStyle.Sneakers,
            shoeColor = Hex("#FFF0CE"),
            accessoryStyle = AvatarAccessoryStyle.Headphones,
            accessoryColor = Hex("#F26E91")
        };

        public static AvatarAppearanceData Worker => new AvatarAppearanceData
        {
            avatarId = "avatar.resident.worker",
            skinColor = Hex("#D5A47A"),
            hairStyle = AvatarHairStyle.ShortCrop,
            hairColor = Hex("#3E414A"),
            topStyle = AvatarTopStyle.WorkVest,
            topColor = Hex("#69746E"),
            topAccentColor = Hex("#F2B94A"),
            bottomStyle = AvatarBottomStyle.Cargo,
            bottomColor = Hex("#42505B"),
            shoeStyle = AvatarShoeStyle.Boots,
            shoeColor = Hex("#5B4637"),
            accessoryStyle = AvatarAccessoryStyle.None,
            accessoryColor = Color.white
        };

        public static AvatarAppearanceData Elder => new AvatarAppearanceData
        {
            avatarId = "avatar.resident.elder",
            skinColor = Hex("#D9AA84"),
            hairStyle = AvatarHairStyle.Wavy,
            hairColor = Hex("#E3DDD1"),
            topStyle = AvatarTopStyle.Cardigan,
            topColor = Hex("#7F9A72"),
            topAccentColor = Hex("#F1E1AE"),
            bottomStyle = AvatarBottomStyle.Tapered,
            bottomColor = Hex("#5C6170"),
            shoeStyle = AvatarShoeStyle.Flats,
            shoeColor = Hex("#3B3540"),
            accessoryStyle = AvatarAccessoryStyle.None,
            accessoryColor = Color.white
        };

        public static AvatarAppearanceData Visitor => new AvatarAppearanceData
        {
            avatarId = "avatar.resident.visitor",
            skinColor = Hex("#E0AC84"),
            hairStyle = AvatarHairStyle.AsymBob,
            hairColor = Hex("#4F405D"),
            topStyle = AvatarTopStyle.Overshirt,
            topColor = Hex("#617FB2"),
            topAccentColor = Hex("#7FD1C4"),
            bottomStyle = AvatarBottomStyle.Tapered,
            bottomColor = Hex("#414E68"),
            shoeStyle = AvatarShoeStyle.Sneakers,
            shoeColor = Hex("#F3E5C8"),
            accessoryStyle = AvatarAccessoryStyle.CrossbodyPouch,
            accessoryColor = Hex("#B67ED1")
        };

        public static Color Hex(string value)
        {
            Color c;
            return ColorUtility.TryParseHtmlString(value, out c) ? c : Color.white;
        }
    }
}
