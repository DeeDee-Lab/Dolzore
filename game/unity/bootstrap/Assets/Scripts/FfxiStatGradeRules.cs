using System;

namespace Dolzore
{
    public enum StatGrade { None, A, B, C, D, E, F, G }

    [Serializable]
    public sealed class StatGradeVector
    {
        public StatGrade hp;
        public StatGrade mp;
        public StatGrade str;
        public StatGrade dex;
        public StatGrade vit;
        public StatGrade agi;
        public StatGrade intel;
        public StatGrade mnd;
        public StatGrade chr;

        public StatGrade Get(CoreStat stat)
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
                default: return StatGrade.None;
            }
        }
    }

    public static class DolzoreFfxiStatGradeRules
    {
        // Original DOLZORE lineages retain the five FFXI-style stat-grade archetypes.
        public static StatGradeVector LineageGrades(LineageArchetype lineage)
        {
            switch (lineage)
            {
                case LineageArchetype.Vanguard:
                    return G(StatGrade.C, StatGrade.E, StatGrade.B, StatGrade.E, StatGrade.C, StatGrade.F, StatGrade.F, StatGrade.B, StatGrade.D);
                case LineageArchetype.Mystic:
                    return G(StatGrade.G, StatGrade.A, StatGrade.F, StatGrade.D, StatGrade.E, StatGrade.C, StatGrade.A, StatGrade.E, StatGrade.D);
                case LineageArchetype.Agile:
                    return G(StatGrade.D, StatGrade.D, StatGrade.E, StatGrade.A, StatGrade.E, StatGrade.B, StatGrade.D, StatGrade.E, StatGrade.F);
                case LineageArchetype.Stalwart:
                    return G(StatGrade.A, StatGrade.G, StatGrade.C, StatGrade.D, StatGrade.A, StatGrade.E, StatGrade.E, StatGrade.D, StatGrade.F);
                default:
                    return G(StatGrade.D, StatGrade.D, StatGrade.D, StatGrade.D, StatGrade.D, StatGrade.D, StatGrade.D, StatGrade.D, StatGrade.D);
            }
        }

        // Six initial DOLZORE vocations preserve the broad grade structure of the six basic FFXI jobs.
        public static StatGradeVector VocationGrades(VocationId vocation)
        {
            switch (vocation)
            {
                case VocationId.Striker:
                    return G(StatGrade.A, StatGrade.None, StatGrade.C, StatGrade.B, StatGrade.A, StatGrade.F, StatGrade.G, StatGrade.D, StatGrade.E);
                case VocationId.Lantern:
                    return G(StatGrade.E, StatGrade.C, StatGrade.D, StatGrade.F, StatGrade.D, StatGrade.E, StatGrade.E, StatGrade.A, StatGrade.C);
                case VocationId.Echo:
                    return G(StatGrade.F, StatGrade.B, StatGrade.F, StatGrade.C, StatGrade.F, StatGrade.C, StatGrade.A, StatGrade.E, StatGrade.D);
                case VocationId.Weaver:
                    return G(StatGrade.D, StatGrade.D, StatGrade.D, StatGrade.D, StatGrade.E, StatGrade.E, StatGrade.C, StatGrade.C, StatGrade.D);
                case VocationId.Trace:
                    return G(StatGrade.D, StatGrade.None, StatGrade.D, StatGrade.A, StatGrade.D, StatGrade.B, StatGrade.C, StatGrade.G, StatGrade.G);
                default:
                    return G(StatGrade.B, StatGrade.None, StatGrade.A, StatGrade.C, StatGrade.D, StatGrade.C, StatGrade.F, StatGrade.F, StatGrade.E);
            }
        }

        public static bool VocationHasMp(VocationId vocation)
        {
            return vocation == VocationId.Weaver || vocation == VocationId.Lantern || vocation == VocationId.Echo;
        }

        public static InternalStatBlock CalculateBaseStats(
            LineageArchetype lineage,
            VocationId mainVocation,
            int mainLevel,
            VocationId supportVocation,
            int supportNativeLevel,
            bool supportEnabled)
        {
            int mlvl = Math.Max(1, mainLevel);
            int slvl = supportEnabled
                ? DolzoreFfxiDerivedMath.SupportVocationEffectiveLevel(mlvl, supportNativeLevel)
                : 0;

            StatGradeVector race = LineageGrades(lineage);
            StatGradeVector main = VocationGrades(mainVocation);
            StatGradeVector sub = VocationGrades(supportVocation);

            InternalStatBlock result = new InternalStatBlock();

            result.hp = RaceHp(race.hp, mlvl) + JobHp(main.hp, mlvl);
            if (supportEnabled && slvl > 0)
                result.hp += SubJobHp(sub.hp, slvl);

            bool mainHasMp = VocationHasMp(mainVocation);
            bool subHasMp = supportEnabled && VocationHasMp(supportVocation) && slvl > 0;

            int raceMp = 0;
            if (mainHasMp)
                raceMp = RaceMp(race.mp, mlvl);
            else if (subHasMp)
                raceMp = SubJobMp(race.mp, slvl);

            result.mp = raceMp;
            if (mainHasMp)
                result.mp += JobMp(main.mp, mlvl);
            if (subHasMp)
                result.mp += SubJobMp(sub.mp, slvl);

            result.str = Attribute(CoreStat.STR, race, main, sub, mlvl, slvl, supportEnabled);
            result.dex = Attribute(CoreStat.DEX, race, main, sub, mlvl, slvl, supportEnabled);
            result.vit = Attribute(CoreStat.VIT, race, main, sub, mlvl, slvl, supportEnabled);
            result.agi = Attribute(CoreStat.AGI, race, main, sub, mlvl, slvl, supportEnabled);
            result.intel = Attribute(CoreStat.INT, race, main, sub, mlvl, slvl, supportEnabled);
            result.mnd = Attribute(CoreStat.MND, race, main, sub, mlvl, slvl, supportEnabled);
            result.chr = Attribute(CoreStat.CHR, race, main, sub, mlvl, slvl, supportEnabled);
            return result;
        }

        private static int Attribute(CoreStat stat, StatGradeVector race, StatGradeVector main, StatGradeVector sub, int mlvl, int slvl, bool supportEnabled)
        {
            int total = GradeAttribute(race.Get(stat), mlvl) + GradeAttribute(main.Get(stat), mlvl);
            if (supportEnabled && slvl > 0)
                total += (int)Math.Floor(GradeAttributeRaw(sub.Get(stat), slvl) / 2.0);
            return total;
        }

        private static int RaceHp(StatGrade grade, int level)
        {
            GradeNumbers n = Numbers(grade);
            if (!n.valid) return 0;
            double v = n.hpBase + n.hpScale1 * (level - 1)
                + 2 * Math.Max(0, level - 10)
                + n.hpScale2 * Math.Max(0, level - 30);
            return (int)Math.Floor(v);
        }

        private static int JobHp(StatGrade grade, int level)
        {
            GradeNumbers n = Numbers(grade);
            if (!n.valid) return 0;
            return (int)Math.Floor(n.hpBase + n.hpScale1 * (level - 1) + n.hpScale2 * Math.Max(0, level - 30));
        }

        private static int SubJobHp(StatGrade grade, int level)
        {
            GradeNumbers n = Numbers(grade);
            if (!n.valid) return 0;
            double v = n.hpBase + n.hpScale1 * (level - 1) + Math.Max(0, level - 10);
            return (int)Math.Floor(v / 2.0);
        }

        private static int RaceMp(StatGrade grade, int level)
        {
            GradeNumbers n = Numbers(grade);
            if (!n.valid) return 0;
            return (int)Math.Floor(n.mpBase + n.mpScale * (level - 1));
        }

        private static int JobMp(StatGrade grade, int level)
        {
            return RaceMp(grade, level);
        }

        private static int SubJobMp(StatGrade grade, int level)
        {
            GradeNumbers n = Numbers(grade);
            if (!n.valid) return 0;
            return (int)Math.Floor((n.mpBase + n.mpScale * (level - 1)) / 2.0);
        }

        private static int GradeAttribute(StatGrade grade, int level)
        {
            return (int)Math.Floor(GradeAttributeRaw(grade, level));
        }

        private static double GradeAttributeRaw(StatGrade grade, int level)
        {
            GradeNumbers n = Numbers(grade);
            if (!n.valid) return 0.0;
            return n.attrBase + n.attrScale * (level - 1);
        }

        private static GradeNumbers Numbers(StatGrade grade)
        {
            switch (grade)
            {
                case StatGrade.A: return new GradeNumbers(true, 19, 9, 1, 16, 6, 5, 0.50);
                case StatGrade.B: return new GradeNumbers(true, 17, 8, 1, 14, 5, 4, 0.45);
                case StatGrade.C: return new GradeNumbers(true, 16, 7, 1, 12, 4, 4, 0.40);
                case StatGrade.D: return new GradeNumbers(true, 14, 6, 0, 10, 3, 3, 0.35);
                case StatGrade.E: return new GradeNumbers(true, 13, 5, 0, 8, 2, 3, 0.30);
                case StatGrade.F: return new GradeNumbers(true, 11, 4, 0, 6, 1, 2, 0.25);
                case StatGrade.G: return new GradeNumbers(true, 10, 3, 0, 4, 0.5, 2, 0.20);
                default: return new GradeNumbers(false, 0, 0, 0, 0, 0, 0, 0);
            }
        }

        private static StatGradeVector G(StatGrade hp, StatGrade mp, StatGrade str, StatGrade dex, StatGrade vit, StatGrade agi, StatGrade intel, StatGrade mnd, StatGrade chr)
        {
            return new StatGradeVector
            {
                hp = hp, mp = mp, str = str, dex = dex, vit = vit,
                agi = agi, intel = intel, mnd = mnd, chr = chr
            };
        }

        private readonly struct GradeNumbers
        {
            public readonly bool valid;
            public readonly double hpBase;
            public readonly double hpScale1;
            public readonly double hpScale2;
            public readonly double mpBase;
            public readonly double mpScale;
            public readonly double attrBase;
            public readonly double attrScale;

            public GradeNumbers(bool valid, double hpBase, double hpScale1, double hpScale2, double mpBase, double mpScale, double attrBase, double attrScale)
            {
                this.valid = valid;
                this.hpBase = hpBase;
                this.hpScale1 = hpScale1;
                this.hpScale2 = hpScale2;
                this.mpBase = mpBase;
                this.mpScale = mpScale;
                this.attrBase = attrBase;
                this.attrScale = attrScale;
            }
        }
    }
}
