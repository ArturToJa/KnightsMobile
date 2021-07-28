using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace KnightsMobile
{
    public interface IPlayer
    {
        public Dictionary<MainData, int> GetPlayerMainData();
        public void SetPlayerMainData(Dictionary<MainData, int> mainData);
        public Dictionary<Statistic, int> GetPlayerStatistics();
        public void SetPlayerStatistics(Dictionary<Statistic, int> statistics);
        public Dictionary<ItemType, Item> GetPlayerItems();
        public void SetPlayerItems(Dictionary<ItemType, Item> items);


        public int GetTotalStatistic(Statistic statistic);
        public int GetMaxHealth();
        public int GetBasicRegeneration();
        public int GetTotalMinDamage();
        public int GetTotalMaxDamage();
        public int GetTotalArmour();
        public int StatisticPercentMultiplier(int value, Statistic statistic);
        public string GetPlayerName();
    }
}