using System;

namespace Dolzore
{
    public static class DolzoreFfxiDerivedMath
    {
        public static int SupportVocationEffectiveLevel(int mainLevel, int supportNativeLevel)
        {
            if (mainLevel < 1) return 0;
            int cap = Math.Max(1, mainLevel / 2);
            return Math.Min(Math.Max(0, supportNativeLevel), cap);
        }

        public static int AccuracyFromSkill(int skill)
        {
            if (skill <= 0) return 0;
            if (skill <= 200) return skill;
            if (skill <= 400) return (int)Math.Floor((skill - 200) * 0.9) + 200;
            if (skill <= 600) return (int)Math.Floor((skill - 400) * 0.8) + 380;
            return (int)Math.Floor((skill - 600) * 0.9) + 540;
        }

        public static int EvasionFromSkill(int skill)
        {
            if (skill <= 0) return 0;
            if (skill <= 200) return skill;
            return (int)Math.Floor((skill - 200) * 0.9) + 200;
        }

        public static int Accuracy(int dex, int weaponSkill, int flatBonus)
        {
            return (int)Math.Floor(dex * 0.75) + AccuracyFromSkill(weaponSkill) + flatBonus;
        }

        public static int Evasion(int agi, int evasionSkill, int flatBonus)
        {
            return (int)Math.Floor(agi * 0.5) + EvasionFromSkill(evasionSkill) + flatBonus;
        }

        public static int HitRatePercent(int accuracy, int evasion, int attackerLevel, int targetLevel, int minPercent = 20, int maxPercent = 95)
        {
            int dLvl = Math.Max(0, targetLevel - attackerLevel);
            int rate = 75 + FloorDiv(accuracy - evasion, 2) - (2 * dLvl);
            return Clamp(rate, minPercent, maxPercent);
        }

        public static int Attack(int strength, int combatSkill, int flatBonus = 0)
        {
            return 8 + combatSkill + strength + flatBonus;
        }

        public static int Defense(int vitality, int level, int flatArmorDefense = 0)
        {
            int baseDefense;
            if (level <= 50)
                baseDefense = (int)Math.Floor(vitality * 1.5) + level + 8;
            else if (level <= 60)
                baseDefense = (int)Math.Floor(vitality * 1.5) + (2 * level) - 42;
            else if (level <= 89)
                baseDefense = (int)Math.Floor(vitality * 1.5) + level + 18;
            else
                baseDefense = (int)Math.Floor(vitality * 1.5) + level + 18 + ((level - 89) / 2);

            return Math.Max(1, baseDefense + flatArmorDefense);
        }

        public static int WeaponRank(int weaponDamage)
        {
            return Math.Max(0, weaponDamage / 9);
        }

        public static int FStr(int attackerStr, int targetVit, int weaponDamage)
        {
            int dStr = attackerStr - targetVit;
            int raw;

            if (dStr >= 12) raw = FloorDiv(dStr + 4, 4);
            else if (dStr >= 6) raw = FloorDiv(dStr + 6, 4);
            else if (dStr >= 1) raw = FloorDiv(dStr + 7, 4);
            else if (dStr >= -2) raw = FloorDiv(dStr + 8, 4);
            else if (dStr >= -7) raw = FloorDiv(dStr + 9, 4);
            else if (dStr >= -15) raw = FloorDiv(dStr + 10, 4);
            else if (dStr >= -21) raw = FloorDiv(dStr + 12, 4);
            else raw = FloorDiv(dStr + 13, 4);

            int rank = WeaponRank(weaponDamage);
            int min = rank == 0 ? -1 : -rank;
            int max = rank + 8;
            return Clamp(raw, min, max);
        }

        public static double CriticalRatePercent(int attackerDex, int targetAgi, double flatBonusPercent = 0.0)
        {
            int dDex = attackerDex - targetAgi;
            double bonus = 0.0;

            if (dDex >= 40) bonus = Math.Min(15.0, dDex - 35.0);
            else if (dDex >= 30) bonus = 4.0;
            else if (dDex >= 20) bonus = 3.0;
            else if (dDex >= 14) bonus = 2.0;
            else if (dDex >= 7) bonus = 1.0;

            return ClampDouble(5.0 + bonus + flatBonusPercent, 0.0, 100.0);
        }

        public static int TechniqueBaseDamage(int weaponDamage, int fStr, int wsc, double fTp)
        {
            return Math.Max(0, (int)Math.Floor((weaponDamage + fStr + wsc) * fTp));
        }

        public static int WeightedStatContribution(
            InternalStatBlock stats,
            double strWeight,
            double dexWeight,
            double vitWeight,
            double agiWeight,
            double intWeight,
            double mndWeight,
            double chrWeight)
        {
            double total =
                stats.str * strWeight +
                stats.dex * dexWeight +
                stats.vit * vitWeight +
                stats.agi * agiWeight +
                stats.intel * intWeight +
                stats.mnd * mndWeight +
                stats.chr * chrWeight;
            return (int)Math.Floor(total);
        }

        public static double FtpByTp(int tp, double at1000, double at2000, double at3000)
        {
            int clamped = Clamp(tp, 1000, 3000);
            if (clamped <= 2000)
                return Lerp(at1000, at2000, (clamped - 1000) / 1000.0);
            return Lerp(at2000, at3000, (clamped - 2000) / 1000.0);
        }

        public static int TpPerHit(int modifiedDelay, int storeTp = 0)
        {
            int d = Math.Max(0, modifiedDelay);
            double baseTp;

            if (d <= 180) baseTp = 61.0 + ((d - 180) * 63.0 / 360.0);
            else if (d <= 540) baseTp = 61.0 + ((d - 180) * 88.0 / 360.0);
            else if (d <= 630) baseTp = 149.0 + ((d - 540) * 20.0 / 360.0);
            else if (d <= 720) baseTp = 154.0 + ((d - 630) * 28.0 / 360.0);
            else if (d <= 900) baseTp = 161.0 + ((d - 720) * 24.0 / 360.0);
            else baseTp = 173.0 + ((d - 900) * 28.0 / 360.0);

            int floored = (int)Math.Floor(baseTp);
            return floored + (int)Math.Floor(floored * (storeTp / 100.0));
        }

        public static int SkillCapEarlyLevel(SkillRank rank, int level)
        {
            int l = Clamp(level, 1, 30);
            int[] curve = GetEarlySkillCurve(rank);
            return curve[l - 1];
        }

        public static int PhysicalDamage(
            int baseDamage,
            int attack,
            int defense,
            int attackerLevel,
            int targetLevel,
            WeaponClass weaponClass,
            bool critical,
            double qRatioSample01,
            double finalRandomizerSample01,
            double directModifier = 1.0)
        {
            double ratio = attack / (double)Math.Max(1, defense);
            int dLvl = Math.Max(0, targetLevel - attackerLevel);
            double cRatio = Math.Max(0.0, ratio - (dLvl * 0.05));
            double wRatio = cRatio + (critical ? 1.0 : 0.0);

            double lower = PdIFLower(wRatio);
            double upper = PdIFUpper(wRatio);
            double cap = PdIFCap(weaponClass, critical);

            double q = Lerp(lower, upper, ClampDouble(qRatioSample01, 0.0, 1.0));
            q = ClampDouble(q, 0.0, cap);

            double randomizer = 1.0 + 0.05 * ClampDouble(finalRandomizerSample01, 0.0, 1.0);
            int step1 = (int)Math.Floor(baseDamage * q);
            int step2 = (int)Math.Floor(step1 * randomizer);
            return Math.Max(0, (int)Math.Floor(step2 * directModifier));
        }

        public static int MagicDamage(
            int basePower,
            int casterStat,
            int targetStat,
            double dStatMultiplier,
            int magicDamageStat,
            ResistTier resistTier,
            double affinity,
            int chainSteps,
            double magicBurstBonus,
            double dayWeather,
            int magicAttackBonus,
            int targetMagicDefenseBonus,
            double targetMagicDamageAdjustment)
        {
            int dStat = casterStat - targetStat;
            int damage = (int)Math.Floor(basePower + magicDamageStat + (dStat * dStatMultiplier));
            damage = FloorMultiply(damage, ResistMultiplier(resistTier));
            damage = FloorMultiply(damage, affinity);
            damage = FloorMultiply(damage, MagicBurstMultiplier(chainSteps));
            damage = FloorMultiply(damage, Math.Max(0.0, 1.0 + magicBurstBonus));
            damage = FloorMultiply(damage, dayWeather);

            double mab = 1.0 + (magicAttackBonus / 100.0);
            double mdb = 1.0 + (targetMagicDefenseBonus / 100.0);
            damage = FloorMultiply(damage, mab / Math.Max(0.01, mdb));
            damage = FloorMultiply(damage, targetMagicDamageAdjustment);
            return Math.Max(0, damage);
        }

        public static int MagicAccuracy(int relevantMagicSkill, int flatMagicAccuracy, int dStatContribution)
        {
            return Math.Max(0, relevantMagicSkill) + flatMagicAccuracy + dStatContribution;
        }

        public static double MagicBurstMultiplier(int chainSteps)
        {
            if (chainSteps < 2) return 1.0;
            return 1.35 + (0.10 * (chainSteps - 2));
        }

        public static double ResistMultiplier(ResistTier tier)
        {
            switch (tier)
            {
                case ResistTier.Half: return 0.5;
                case ResistTier.Quarter: return 0.25;
                case ResistTier.Eighth: return 0.125;
                default: return 1.0;
            }
        }

        private static double PdIFUpper(double wRatio)
        {
            if (wRatio < 0.5) return wRatio + 0.5;
            if (wRatio < 0.7) return 1.0;
            if (wRatio < 1.2) return wRatio + 0.3;
            if (wRatio < 1.5) return (wRatio * 0.25) + wRatio;
            return wRatio + 0.375;
        }

        private static double PdIFLower(double wRatio)
        {
            if (wRatio < 0.38) return 0.0;
            if (wRatio < 1.25) return (wRatio * (1176.0 / 1024.0)) - (448.0 / 1024.0);
            if (wRatio < 1.51) return 1.0;
            if (wRatio < 2.44) return (wRatio * (1176.0 / 1024.0)) - (755.0 / 1024.0);
            return wRatio - 0.375;
        }

        private static double PdIFCap(WeaponClass weaponClass, bool critical)
        {
            switch (weaponClass)
            {
                case WeaponClass.Martial: return critical ? 4.5 : 3.5;
                case WeaponClass.TwoHanded: return critical ? 4.75 : 3.75;
                case WeaponClass.Reaper: return critical ? 5.0 : 4.0;
                case WeaponClass.Ranged: return critical ? 4.0625 : 3.25;
                default: return critical ? 4.25 : 3.25;
            }
        }

        private static int[] GetEarlySkillCurve(SkillRank rank)
        {
            switch (rank)
            {
                case SkillRank.APlus:
                case SkillRank.A:
                case SkillRank.AMinus:
                    return new[] {6,9,12,15,18,21,24,27,30,33,36,39,42,45,48,51,54,57,60,63,66,69,72,75,78,81,84,87,90,93};
                case SkillRank.BPlus:
                case SkillRank.B:
                case SkillRank.BMinus:
                    return new[] {5,7,10,13,16,19,22,25,28,31,34,36,39,42,45,48,51,54,57,60,63,65,68,71,74,77,80,83,86,89};
                case SkillRank.CPlus:
                case SkillRank.C:
                case SkillRank.CMinus:
                    return new[] {5,7,10,13,16,19,21,24,27,30,33,35,38,41,44,47,49,52,55,58,61,63,66,69,72,75,77,80,83,86};
                case SkillRank.D:
                    return new[] {4,6,9,12,14,17,20,22,25,28,31,33,36,39,41,44,47,49,52,55,58,60,63,66,68,71,74,76,79,82};
                case SkillRank.E:
                    return new[] {4,6,9,11,14,16,19,21,24,26,29,31,34,36,39,41,44,46,49,51,54,56,59,61,64,66,69,71,74,76};
                case SkillRank.F:
                    return new[] {4,6,8,10,13,15,17,20,22,24,27,29,31,33,36,38,40,43,45,47,50,52,54,56,59,61,63,66,68,70};
                default:
                    return new int[30];
            }
        }

        private static int FloorMultiply(int value, double multiplier)
        {
            return (int)Math.Floor(value * multiplier);
        }

        private static int FloorDiv(int value, int divisor)
        {
            return (int)Math.Floor(value / (double)divisor);
        }

        private static double Lerp(double a, double b, double t)
        {
            return a + ((b - a) * t);
        }

        private static int Clamp(int v, int min, int max)
        {
            return Math.Max(min, Math.Min(max, v));
        }

        private static double ClampDouble(double v, double min, double max)
        {
            return Math.Max(min, Math.Min(max, v));
        }
    }

    [Serializable]
    public sealed class EnmityState
    {
        public int volatileThreat;
        public int durableThreat;

        public int Total => Math.Max(0, volatileThreat) + Math.Max(0, durableThreat);

        public void AddFixed(int volatileAmount, int durableAmount)
        {
            volatileThreat = Math.Max(0, volatileThreat + volatileAmount);
            durableThreat = Math.Max(0, durableThreat + durableAmount);
        }

        public void AddDamageReference(int damage, int standardDamage)
        {
            if (damage <= 0 || standardDamage <= 0) return;
            volatileThreat += (int)Math.Floor(240.0 * damage / standardDamage);
            durableThreat += (int)Math.Floor(80.0 * damage / standardDamage);
        }

        public void Decay(double seconds, int volatileDecayPerSecond = 60)
        {
            int decay = (int)Math.Floor(Math.Max(0.0, seconds) * Math.Max(0, volatileDecayPerSecond));
            volatileThreat = Math.Max(0, volatileThreat - decay);
        }

        public void OnDamageTaken(int durableLoss)
        {
            durableThreat = Math.Max(0, durableThreat - Math.Max(0, durableLoss));
        }
    }
}
