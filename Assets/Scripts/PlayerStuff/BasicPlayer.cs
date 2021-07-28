using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KnightsMobile
{
    public class BasicPlayer : IPlayer
    {
        private string playerName = "";
        private Dictionary<MainData, int> mainData = new Dictionary<MainData, int>();
        private Dictionary<Statistic, int> statistics = new Dictionary<Statistic, int>();
        private Dictionary<Statistic, int> bonusStatistics = new Dictionary<Statistic, int>();
        private Dictionary<ItemType, Item> items = new Dictionary<ItemType, Item>();
        private List<HitZone> hitZones = new List<HitZone>();
        private List<BlockZone> blockZones = new List<BlockZone>();

        public Dictionary<MainData, int> GetPlayerMainData()
        {
            return mainData;
        }

        public void SetPlayerMainData(Dictionary<MainData, int> mainData)
        {
            this.mainData = mainData;
        }

        public Dictionary<Statistic, int> GetPlayerStatistics()
        {
            return statistics;
        }

        public void SetPlayerStatistics(Dictionary<Statistic, int> statistics)
        {
            this.statistics = statistics;
        }

        public Dictionary<ItemType, Item> GetPlayerItems()
        {
            return items;
        }

        public void SetPlayerItems(Dictionary<ItemType, Item> items)
        {
            this.items = items;
        }

        #region Statistics

        public int GetTotalStatistic(Statistic statistic)
        {
            if (Statistics.IsBasicStatistic(statistic))
            {
                return StatisticPercentMultiplier(statistics[statistic] + bonusStatistics[statistic], statistic);
            }
            else
            {
                Debug.LogError("Wrong basic statistic: " + statistic.ToString());
                return 0;
            }
        }

        public int GetMaxHealth()
        {
            return 25 * GetTotalStatistic(Statistic.Constitution);
        }

        public int GetBasicRegeneration()
        {
            return 10 + GetTotalStatistic(Statistic.Constitution) / 2;
        }

        public int GetTotalMinDamage()
        {
            return StatisticPercentMultiplier(GetTotalStatistic(Statistic.Strength) / 4 + bonusStatistics[Statistic.DamageMin], Statistic.DamageMin);
        }

        public int GetTotalMaxDamage()
        {
            return StatisticPercentMultiplier(GetTotalStatistic(Statistic.Strength) / 4 + bonusStatistics[Statistic.DamageMax], Statistic.DamageMax);
        }

        public int GetTotalArmour()
        {
            return StatisticPercentMultiplier(GetTotalStatistic(Statistic.Aptitude) / 4 + bonusStatistics[Statistic.Armour], Statistic.Armour);
        }

        public int StatisticPercentMultiplier(int value, Statistic statistic)
        {
            int multiplier = 100 + bonusStatistics[Statistics.GetPercentStatistic(statistic)];
            int total = value * multiplier;
            return total / 100;
        }

        public string GetPlayerName()
        {
            return playerName;
        }

        #endregion
    }
}


