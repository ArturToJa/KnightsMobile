using System.Collections.Generic;

namespace KnightsMobile
{
    public enum HitZone
    {
        Head,
        RightShoulder,
        LeftShoulder,
        Chest,
        RightArm,
        LeftArm,
        Centre,
        RightLeg,
        LeftLeg
    }

    public enum BlockZone
    {
        RightUpperBody,
        LeftUpperBody,
        Centre,
        RightSide,
        LeftSide
    }

    static class BlockZones
    {
        private static Dictionary<BlockZone, List<HitZone>> zones = new Dictionary<BlockZone, List<HitZone>>
    {
        { BlockZone.RightUpperBody, new List<HitZone>{ HitZone.RightShoulder, HitZone.RightArm } },
        { BlockZone.LeftUpperBody, new List<HitZone>{ HitZone.LeftShoulder, HitZone.LeftArm } },
        { BlockZone.Centre, new List<HitZone>{ HitZone.Head, HitZone.Chest } },
        { BlockZone.RightSide, new List<HitZone>{ HitZone.RightArm, HitZone.RightLeg, HitZone.Chest, HitZone.Centre } },
        { BlockZone.LeftSide, new List<HitZone>{ HitZone.LeftArm, HitZone.LeftLeg, HitZone.Chest, HitZone.Centre } }
    };

        public static bool IsBlocked(BlockZone block, HitZone hit)
        {
            return zones[block].Contains(hit);
        }
    }
}
