using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;
using Newtonsoft.Json;

namespace KnightsMobile
{
    public enum MainData
    {
        Level,
        Health,
        Experience,
        Gold,
        Rubies,
        Victories,
        Draws,
        Defeats,
        WonTournaments,
        LootLooted,
        LootLost
    }

    public class Player : NetworkBehaviour, IPlayer
    {
        [SyncVar(hook = nameof(OnNameUpdated))] public string playerName = "";
        readonly public SyncDictionary<MainData, int> mainData = new SyncDictionary<MainData, int>();
        readonly public SyncDictionary<Statistic, int> statistics = new SyncDictionary<Statistic, int>();
        readonly public SyncDictionary<Statistic, int> bonusStatistics = new SyncDictionary<Statistic, int>();
        readonly public SyncDictionary<ItemType, Item> items = new SyncDictionary<ItemType, Item>();
        readonly public SyncList<HitZone> hitZones = new SyncList<HitZone>();
        readonly public SyncList<BlockZone> blockZones = new SyncList<BlockZone>();
        

        public override void OnStartServer()
        {
            Server_InitializeMainData();
            Server_InitializeStatistics();
            Server_InitializeItems();
            statistics.Callback += OnStatisticsUpdated;
            mainData.Callback += OnMainDataUpdated;
        }

        public override void OnStartClient()
        {
            Client_SetPlayerInUIManager();
            statistics.Callback += OnStatisticsUpdated;
            InitializeUIData();
        }

        #region SyncVarCallbacks

        [ClientCallback]
        void OnNameUpdated(string oldValue, string newValue)
        {
            UIManager.instance.UpdatePlayername(newValue);
        }

        [ClientCallback]
        void OnMainDataUpdated(SyncDictionary<MainData, int>.Operation op, MainData key, int value)
        {
            if(op != SyncDictionary<MainData, int>.Operation.OP_SET)
            {
                // error, we support only set
                Debug.LogError("Performed different operation than set");
                return;
            }
            UIManager.instance.UpdateMainData(key, value);
        }

        [ClientCallback]
        void OnStatisticsUpdated(SyncDictionary<Statistic, int>.Operation op, Statistic key, int value)
        {
            if(op != SyncDictionary<Statistic, int>.Operation.OP_SET)
            {
                // error, we support only set
                Debug.LogError("Performed different operation than set");
                return;
            }
            UIManager.instance.UpdateStatistic(key, value, bonusStatistics[key], CalculateStatisticCost(value));
        }

        #endregion

        #region ServerCallbacks

        [ServerCallback]
        void Server_InitializeMainData()
        {
            foreach (MainData data in (MainData[])Enum.GetValues(typeof(MainData)))
            {
                mainData.Add(data, 0);
            }
            mainData[MainData.Level] = 1;
            mainData[MainData.Health] = 75;
        }

        [ServerCallback]
        void Server_InitializeStatistics()
        {
            for (int i = 0; i <= 5; ++i)
            {
                statistics.Add((Statistic)i, 3);
            }
            foreach (Statistic statistic in (Statistic[])Enum.GetValues(typeof(Statistic)))
            {
                bonusStatistics.Add(statistic, 0);
            }
        }
        [ServerCallback]
        void Server_InitializeItems()
        {
            foreach (ItemType itemType in (ItemType[])Enum.GetValues(typeof(ItemType)))
            {
                items.Add(itemType, null);
            }
        }

        [ServerCallback]
        void Server_ResetBonusStatistics()
        {
            foreach (Statistic statistic in (Statistic[])Enum.GetValues(typeof(Statistic)))
            {
                bonusStatistics[statistic] = 0;
            }
        }

        [ServerCallback]
        void Server_SumBonusStatistics()
        {
            Server_ResetBonusStatistics();

            foreach (KeyValuePair<ItemType, Item> item in items)
            {
                if(item.Value != null && item.Key != ItemType.Lance)
                {
                    foreach (KeyValuePair<Statistic, int> statistic in item.Value.statistics)
                    {
                        bonusStatistics[statistic.Key] += statistic.Value;
                    }
                }
            }
        }

        [ServerCallback]
        public Dictionary<MainData, int> GetPlayerMainData()
        {
            Dictionary<MainData, int> mainData = new Dictionary<MainData, int>();
            foreach (KeyValuePair<MainData, int> item in this.mainData)
            {
                mainData.Add(item.Key, item.Value);
            }
            return mainData;
        }

        [ServerCallback]
        public void SetPlayerMainData(Dictionary<MainData, int> mainData)
        {
            foreach (KeyValuePair<MainData, int> item in mainData)
            {
                this.mainData[item.Key] = item.Value;
            }
        }

        [ServerCallback]
        public Dictionary<Statistic, int> GetPlayerStatistics()
        {
            Dictionary<Statistic, int> statistics = new Dictionary<Statistic, int>();
            foreach (KeyValuePair<Statistic, int> item in this.statistics)
            {
                statistics.Add(item.Key, item.Value);
            }
            return statistics;
        }

        [ServerCallback]
        public void SetPlayerStatistics(Dictionary<Statistic, int> statistics)
        {
            foreach (KeyValuePair<Statistic, int> item in statistics)
            {
                this.statistics[item.Key] = item.Value;
            }
        }

        [ServerCallback]
        public Dictionary<ItemType, Item> GetPlayerItems()
        {
            Dictionary<ItemType, Item> items = new Dictionary<ItemType, Item>();
            foreach(KeyValuePair<ItemType, Item> item in this.items)
            {
                items.Add(item.Key, item.Value);
            }
            return items;
        }

        [ServerCallback]
        public void SetPlayerItems(Dictionary<ItemType, Item> items)
        {
            foreach(KeyValuePair<ItemType, Item> item in items)
            {
                this.items[item.Key] = item.Value;
            }
        }

        #endregion

        #region ClientCallbacks

        [ClientCallback]
        void InitializeUIData()
        {
            OnNameUpdated("", playerName);
            foreach (KeyValuePair<Statistic, int> statistic in statistics)
            {
                OnStatisticsUpdated(SyncDictionary<Statistic, int>.Operation.OP_SET, statistic.Key, statistic.Value);
            }
            foreach (KeyValuePair<MainData, int> data in mainData)
            {
                OnMainDataUpdated(SyncDictionary<MainData, int>.Operation.OP_SET, data.Key, data.Value);
            }
        }

        // Server doesn't have UIManager
        [ClientCallback]
        void Client_SetPlayerInUIManager()
        {
            UIManager.instance.SetPlayer(this);
        }
        #endregion

        #region Commands

        [Command]
        public void UpgradeStatistic(Statistic whichStat)
        {
            if(statistics.ContainsKey(whichStat))
            {
                int statValue = statistics[whichStat];
                int cost = CalculateStatisticCost(statValue);
                if (mainData[MainData.Gold] >= cost)
                {
                    statistics[whichStat]++;
                    mainData[MainData.Gold] -= cost;
                }
                else
                {
                    Debug.LogError("Not enough gold to upgrade statistic");
                }
            }
            else
            {
                Debug.LogError("You are trying to upgrade wrong statistic");
            }
            
        }

        public void AttackPlayer(string otherPlayerId)
        {
            IPlayer otherPlayer = NetworkPlayerManager.instance.GetPlayer(otherPlayerId);
        }

        #endregion

        #region ClientRPCs
        #endregion

        #region Statistics

        public int GetTotalStatistic(Statistic statistic)
        {
            if(Statistics.IsBasicStatistic(statistic))
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

        public int GetMinExperience()
        {
            return GetExperience(mainData[MainData.Level] - 1);
        }

        public int GetMaxExperience()
        {
            return GetExperience(mainData[MainData.Level]);
        }

        public int GetExperience(int level)
        {
            if (level < 1) return 0;
            int experienceDifference = 5 + (level - 1) * 10;
            return ((5 + experienceDifference) / 2) * level;
        }

        public int CalculateStatisticCost(int value)
        {
            return value * value;
        }

        public string GetPlayerName()
        {
            return playerName;
        }

        #endregion
    }
}
