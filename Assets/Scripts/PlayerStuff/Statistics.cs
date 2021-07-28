using UnityEngine;

namespace KnightsMobile
{
    public enum Statistic
    {
        Strength = 0,
        Aptitude = 1,
        Constitution = 2,
        Luck = 3,
        WeaponSkill = 4,
        DefenceSkill = 5,
        CriticalDamage = 6,
        Speed = 7,
        DamageMin = 8,
        DamageMax = 9,
        Armour = 10,
        FireDamage = 11,
        FireResistancePercent = 12,
        ShockDamage = 13,
        ShockResistancePercent = 14,
        IceDamage = 15,
        IceResistancePercent = 16,
        PoisonDamage = 17,
        PoisonResistancePercent = 18,
        StrengthPercent = 19,
        AptitudePercent = 20,
        ConstitutionPercent = 21,
        LuckPercent = 22,
        WeaponSkillPercent = 23,
        DefenceSkillPercent = 24,
        MissionGoldBonusPercent = 25,
        DamagePercent = 26,
        ArmourPercent = 27
    }
    public static class Statistics
    {
        public static Statistic GetPercentStatistic(Statistic statistic)
        {
            switch(statistic)
            {
                case Statistic.Strength:
                    return Statistic.StrengthPercent;
                case Statistic.Aptitude:
                    return Statistic.AptitudePercent;
                case Statistic.Constitution:
                    return Statistic.ConstitutionPercent;
                case Statistic.Luck:
                    return Statistic.LuckPercent;
                case Statistic.WeaponSkill:
                    return Statistic.WeaponSkillPercent;
                case Statistic.DefenceSkill:
                    return Statistic.DefenceSkillPercent;
                case Statistic.Armour:
                    return Statistic.ArmourPercent;
                case Statistic.DamageMax:
                    return Statistic.DamagePercent;
                case Statistic.DamageMin:
                    return Statistic.DamagePercent;
                default:
                    Debug.LogError("This statistic does not have percent value");
                    return statistic;
            }
        }

        public static bool IsBasicStatistic(Statistic statistic)
        {
            return (int)statistic >= 0 && (int)statistic <= 5;
        }
    }
}