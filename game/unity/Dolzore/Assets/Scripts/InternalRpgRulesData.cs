using System;
using System.Collections.Generic;

namespace Dolzore
{
    public enum CoreStat { HP, MP, STR, DEX, VIT, AGI, INT, MND, CHR }
    public enum LineageArchetype { Balanced, Vanguard, Mystic, Agile, Stalwart }
    public enum VocationId { Warden, Striker, Weaver, Lantern, Trace, Echo }
    public enum SkillRank { None, F, E, D, CMinus, C, CPlus, BMinus, B, BPlus, AMinus, A, APlus }
    public enum SkillFamily
    {
        Martial, Blade, Heavy, Polearm, Shield, Ranged, FocusTool,
        Evasion, Guard, Parry,
        Resonance, Restoration, Enhancement, Disruption, Shade, Revelation
    }
    public enum WeaponClass { OneHanded, Martial, TwoHanded, Reaper, Ranged }
    public enum ResonanceElement { Heat, Flow, Gale, Stone, Light, Shade, Pulse, Stillness }
    public enum ResistTier { Full, Half, Quarter, Eighth }

    [Serializable]
    public sealed class InternalStatBlock
    {
        public int hp;
        public int mp;
        public int str;
        public int dex;
        public int vit;
        public int agi;
        public int intel;
        public int mnd;
        public int chr;

        public InternalStatBlock Clone()
        {
            return (InternalStatBlock)MemberwiseClone();
        }

        public int Get(CoreStat stat)
        {
            switch (stat)
            {
                case CoreStat.HP: return hp;
                case CoreStat.MP: return mp;
                case CoreStat.STR: return str;
                case CoreStat.DEX: return dex;
                case CoreStat.VIT: return vit;
                case CoreStat.AGI: return agi;
                case CoreStat.INT: return intel;
                case CoreStat.MND: return mnd;
                case CoreStat.CHR: return chr;
                default: return 0;
            }
        }
    }

    [Serializable]
    public sealed class LineageProfile
    {
        public string lineageId;
        public string displayName;
        public float hp;
        public float mp;
        public float str;
        public float dex;
        public float vit;
        public float agi;
        public float intel;
        public float mnd;
        public float chr;
    }

    [Serializable]
    public sealed class VocationProfile
    {
        public VocationId id;
        public string vocationId;
        public string displayName;
        public Dictionary<SkillFamily, SkillRank> skillRanks = new Dictionary<SkillFamily, SkillRank>();

        public SkillRank Rank(SkillFamily family)
        {
            SkillRank rank;
            return skillRanks.TryGetValue(family, out rank) ? rank : SkillRank.None;
        }
    }

    public static class DolzoreInternalRulesDatabase
    {
        public static readonly LineageProfile Balanced = MakeLineage(
            "lineage.balance", "BALANCED", 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f);

        public static readonly LineageProfile Vanguard = MakeLineage(
            "lineage.vanguard", "VANGUARD", 1.08f, 0.90f, 1.12f, 0.92f, 1.08f, 0.90f, 0.90f, 1.10f, 1.00f);

        public static readonly LineageProfile Mystic = MakeLineage(
            "lineage.mystic", "MYSTIC", 0.84f, 1.24f, 0.88f, 1.00f, 0.90f, 1.08f, 1.15f, 1.00f, 1.00f);

        public static readonly LineageProfile Agile = MakeLineage(
            "lineage.agile", "AGILE", 1.00f, 1.00f, 0.96f, 1.12f, 0.94f, 1.12f, 1.00f, 0.96f, 0.94f);

        public static readonly LineageProfile Stalwart = MakeLineage(
            "lineage.stalwart", "STALWART", 1.18f, 0.72f, 1.10f, 1.00f, 1.15f, 0.94f, 0.92f, 1.00f, 0.94f);

        public static readonly VocationProfile Warden = Vocation(VocationId.Warden, "vocation.warden", "WARDEN",
            Pair(SkillFamily.Blade, SkillRank.A),
            Pair(SkillFamily.Heavy, SkillRank.BPlus),
            Pair(SkillFamily.Shield, SkillRank.APlus),
            Pair(SkillFamily.Guard, SkillRank.A),
            Pair(SkillFamily.Evasion, SkillRank.C),
            Pair(SkillFamily.Enhancement, SkillRank.C),
            Pair(SkillFamily.Restoration, SkillRank.D));

        public static readonly VocationProfile Striker = Vocation(VocationId.Striker, "vocation.striker", "STRIKER",
            Pair(SkillFamily.Martial, SkillRank.APlus),
            Pair(SkillFamily.Blade, SkillRank.B),
            Pair(SkillFamily.Guard, SkillRank.BPlus),
            Pair(SkillFamily.Evasion, SkillRank.BPlus),
            Pair(SkillFamily.Parry, SkillRank.B));

        public static readonly VocationProfile Weaver = Vocation(VocationId.Weaver, "vocation.weaver", "WEAVER",
            Pair(SkillFamily.FocusTool, SkillRank.B),
            Pair(SkillFamily.Resonance, SkillRank.CPlus),
            Pair(SkillFamily.Enhancement, SkillRank.A),
            Pair(SkillFamily.Disruption, SkillRank.A),
            Pair(SkillFamily.Restoration, SkillRank.B),
            Pair(SkillFamily.Evasion, SkillRank.D));

        public static readonly VocationProfile Lantern = Vocation(VocationId.Lantern, "vocation.lantern", "LANTERN",
            Pair(SkillFamily.FocusTool, SkillRank.B),
            Pair(SkillFamily.Restoration, SkillRank.APlus),
            Pair(SkillFamily.Enhancement, SkillRank.A),
            Pair(SkillFamily.Revelation, SkillRank.A),
            Pair(SkillFamily.Shield, SkillRank.C),
            Pair(SkillFamily.Evasion, SkillRank.D));

        public static readonly VocationProfile Trace = Vocation(VocationId.Trace, "vocation.trace", "TRACE",
            Pair(SkillFamily.Ranged, SkillRank.APlus),
            Pair(SkillFamily.Blade, SkillRank.B),
            Pair(SkillFamily.Evasion, SkillRank.A),
            Pair(SkillFamily.Parry, SkillRank.BPlus),
            Pair(SkillFamily.Disruption, SkillRank.C));

        public static readonly VocationProfile Echo = Vocation(VocationId.Echo, "vocation.echo", "ECHO",
            Pair(SkillFamily.FocusTool, SkillRank.B),
            Pair(SkillFamily.Resonance, SkillRank.APlus),
            Pair(SkillFamily.Shade, SkillRank.A),
            Pair(SkillFamily.Disruption, SkillRank.BPlus),
            Pair(SkillFamily.Evasion, SkillRank.E));

        public static LineageProfile GetLineage(LineageArchetype id)
        {
            switch (id)
            {
                case LineageArchetype.Vanguard: return Vanguard;
                case LineageArchetype.Mystic: return Mystic;
                case LineageArchetype.Agile: return Agile;
                case LineageArchetype.Stalwart: return Stalwart;
                default: return Balanced;
            }
        }

        public static VocationProfile GetVocation(VocationId id)
        {
            switch (id)
            {
                case VocationId.Striker: return Striker;
                case VocationId.Weaver: return Weaver;
                case VocationId.Lantern: return Lantern;
                case VocationId.Trace: return Trace;
                case VocationId.Echo: return Echo;
                default: return Warden;
            }
        }

        private static LineageProfile MakeLineage(string id, string display, float hp, float mp, float str, float dex, float vit, float agi, float intel, float mnd, float chr)
        {
            return new LineageProfile
            {
                lineageId = id, displayName = display,
                hp = hp, mp = mp, str = str, dex = dex, vit = vit, agi = agi, intel = intel, mnd = mnd, chr = chr
            };
        }

        private static KeyValuePair<SkillFamily, SkillRank> Pair(SkillFamily family, SkillRank rank)
        {
            return new KeyValuePair<SkillFamily, SkillRank>(family, rank);
        }

        private static VocationProfile Vocation(VocationId id, string vocationId, string display, params KeyValuePair<SkillFamily, SkillRank>[] ranks)
        {
            VocationProfile profile = new VocationProfile { id = id, vocationId = vocationId, displayName = display };
            for (int i = 0; i < ranks.Length; i++)
                profile.skillRanks[ranks[i].Key] = ranks[i].Value;
            return profile;
        }
    }
}
