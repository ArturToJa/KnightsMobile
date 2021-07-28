using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KnightsMobile
{
    public class PlayerDisplayData
    {
        public string playerName { get; private set; }
        public Dictionary<MainData, int> mainData = new Dictionary<MainData, int>();
        public Dictionary<Statistic, int> statistics = new Dictionary<Statistic, int>();
        public Dictionary<ItemType, Item> items;

        public PlayerDisplayData(IPlayer player)
        {
            playerName = player.GetPlayerName();

            mainData.Add(MainData.Level, player.GetPlayerMainData()[MainData.Level]);
            mainData.Add(MainData.Defeats, player.GetPlayerMainData()[MainData.Defeats]);
            mainData.Add(MainData.Draws, player.GetPlayerMainData()[MainData.Draws]);
            mainData.Add(MainData.Victories, player.GetPlayerMainData()[MainData.Victories]);
            mainData.Add(MainData.LootLooted, player.GetPlayerMainData()[MainData.LootLooted]);
            mainData.Add(MainData.LootLost, player.GetPlayerMainData()[MainData.LootLost]);
            mainData.Add(MainData.WonTournaments, player.GetPlayerMainData()[MainData.WonTournaments]);

            statistics.Add(Statistic.Strength, player.GetTotalStatistic(Statistic.Strength));
            statistics.Add(Statistic.Aptitude, player.GetTotalStatistic(Statistic.Aptitude));
            statistics.Add(Statistic.Constitution, player.GetTotalStatistic(Statistic.Constitution));
            statistics.Add(Statistic.Luck, player.GetTotalStatistic(Statistic.Luck));
            statistics.Add(Statistic.WeaponSkill, player.GetTotalStatistic(Statistic.WeaponSkill));
            statistics.Add(Statistic.DefenceSkill, player.GetTotalStatistic(Statistic.DefenceSkill));
            statistics.Add(Statistic.DamageMin, player.GetTotalMinDamage());
            statistics.Add(Statistic.DamageMax, player.GetTotalMaxDamage());
            statistics.Add(Statistic.Armour, player.GetTotalArmour());

            items = new Dictionary<ItemType, Item>(player.GetPlayerItems());
        }
    }
}
