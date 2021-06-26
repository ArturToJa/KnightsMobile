using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

namespace KnightsMobile
{
    public class Player : NetworkBehaviour
    {
        Dictionary<Statistic, int> statistics = new Dictionary<Statistic, int>();
        int gold = 0;


        void Start()
        {
            foreach (Statistic stat in (Statistic[])Enum.GetValues(typeof(Statistic)))
            {
                statistics.Add(stat, 3);
            }
            SetPlayerInUIManager();
        }

        // Server doesn't have UIManager
        [ClientCallback]
        void SetPlayerInUIManager()
        {
            UIManager.instance.SetPlayer(this);
        }

        [Command]
        void UpgradeStatistic(Statistic whichStat)
        {
            int statValue = statistics[whichStat];
            int cost = statValue * statValue;
            if(gold >= cost)
            {
                statistics[whichStat]++;
                gold -= cost;
            }
            else
            {
                Debug.LogError("Not enough gold to upgrade statistic");
            }
        }
    }
}
