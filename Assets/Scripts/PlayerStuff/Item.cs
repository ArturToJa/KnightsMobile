using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System;
using Mirror;

namespace KnightsMobile
{
    public enum ItemRank
    {
        Common,
        Rare,
        Epic,
        Legendary
    }

    public enum ItemType
    {
        Weapon,
        Lance,
        Shield,
        Helm,
        ChestArmour,
        Gaiters,
        Amulet,
        Companion,
        Horse
    }

    [Serializable]
    public class Item : IStorable
    {
        [JsonProperty] public ItemRank rank;
        [JsonProperty] public ItemType type;
        [JsonProperty] public int icon;
        [JsonProperty] public int specialName;
        [JsonProperty] public int speciality;
        [JsonProperty] public Dictionary<Statistic, int> statistics;
        [JsonProperty] public List<int> runes;
    }
}
