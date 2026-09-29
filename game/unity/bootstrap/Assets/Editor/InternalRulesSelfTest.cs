using System;
using UnityEngine;

namespace Dolzore.Editor
{
    public static class InternalRulesSelfTest
    {
        public static void AssertReferenceContracts()
        {
            Eq("support-level", 15, DolzoreFfxiDerivedMath.SupportVocationEffectiveLevel(30, 99));
            Eq("accuracy", 285, DolzoreFfxiDerivedMath.Accuracy(100, 200, 10));
            Eq("evasion", 260, DolzoreFfxiDerivedMath.Evasion(100, 200, 10));
            Eq("hit-rate", 87, DolzoreFfxiDerivedMath.HitRatePercent(285, 260, 30, 30));
            Eq("fstr", 4, DolzoreFfxiDerivedMath.FStr(112, 100, 45));
            Eq("tp-450", 127, DolzoreFfxiDerivedMath.TpPerHit(450));
            Eq("skill-a30", 93, DolzoreFfxiDerivedMath.SkillCapEarlyLevel(SkillRank.APlus, 30));
            Eq("skill-b30", 89, DolzoreFfxiDerivedMath.SkillCapEarlyLevel(SkillRank.B, 30));
            Eq("skill-d30", 82, DolzoreFfxiDerivedMath.SkillCapEarlyLevel(SkillRank.D, 30));

            Near("mb-2", 1.35, DolzoreFfxiDerivedMath.MagicBurstMultiplier(2));
            Near("mb-4", 1.55, DolzoreFfxiDerivedMath.MagicBurstMultiplier(4));
            Near("resist-quarter", 0.25, DolzoreFfxiDerivedMath.ResistMultiplier(ResistTier.Quarter));

            int techniqueBase = DolzoreFfxiDerivedMath.TechniqueBaseDamage(45, 4, 20, 1.5);
            Eq("technique-base", 103, techniqueBase);

            int physical = DolzoreFfxiDerivedMath.PhysicalDamage(
                techniqueBase, 250, 200, 30, 30, WeaponClass.OneHanded,
                false, 0.5, 0.5);
            True("physical-positive", physical > 0);

            int magic = DolzoreFfxiDerivedMath.MagicDamage(
                100, 90, 70, 1.0, 10, ResistTier.Full,
                1.0, 2, 0.0, 1.0, 25, 10, 1.0);
            True("magic-positive", magic > 100);

            EnmityState enmity = new EnmityState();
            enmity.AddDamageReference(100, 100);
            Eq("enmity-before-decay", 320, enmity.Total);
            enmity.Decay(1.0);
            Eq("enmity-after-decay", 260, enmity.Total);

            True("lineage-hp", DolzoreInternalRulesDatabase.Stalwart.hp > DolzoreInternalRulesDatabase.Mystic.hp);
            True("lineage-mp", DolzoreInternalRulesDatabase.Mystic.mp > DolzoreInternalRulesDatabase.Stalwart.mp);
            True("lineage-dex", DolzoreInternalRulesDatabase.Agile.dex > DolzoreInternalRulesDatabase.Vanguard.dex);
            True("vocation-echo-magic", DolzoreInternalRulesDatabase.Echo.Rank(SkillFamily.Resonance) == SkillRank.APlus);
            True("vocation-lantern-heal", DolzoreInternalRulesDatabase.Lantern.Rank(SkillFamily.Restoration) == SkillRank.APlus);

            Debug.Log("DOLZORE_INTERNAL_RULES_REFERENCE_TESTS=PASS");
        }

        private static void Eq(string name, int expected, int actual)
        {
            if (expected != actual)
                throw new InvalidOperationException(name + " expected=" + expected + " actual=" + actual);
        }

        private static void Near(string name, double expected, double actual)
        {
            if (Math.Abs(expected - actual) > 0.0001)
                throw new InvalidOperationException(name + " expected=" + expected + " actual=" + actual);
        }

        private static void True(string name, bool value)
        {
            if (!value)
                throw new InvalidOperationException(name + " expected=true");
        }
    }
}
