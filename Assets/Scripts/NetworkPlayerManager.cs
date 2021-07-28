using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkPlayerManager : MonoBehaviour
{
    public static NetworkPlayerManager instance { get; protected set; }

    private Dictionary<string, KnightsMobile.Player> connectedPlayers = new Dictionary<string, KnightsMobile.Player>();

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
            throw new System.Exception("An instance of this singleton already exists.");
        }
        else
        {
            Debug.Log("Singleton for NetworkPlayerManager is ready");
            instance = this;
        }
    }

    public void AddPlayer(string id, KnightsMobile.Player player)
    {
        if(connectedPlayers.ContainsKey(id) || connectedPlayers.ContainsValue(player))
        {
            Debug.LogWarning("This player is already added to the dictionary");
            return;
        }
        connectedPlayers.Add(id, player);
    }

    public void RemovePlayer(string id)
    {
        if(!connectedPlayers.ContainsKey(id))
        {
            Debug.LogWarning("This id is not in the dictionary");
            return;
        }
        connectedPlayers.Remove(id);
    }

    public KnightsMobile.IPlayer GetPlayer(string id)
    {
        if(connectedPlayers.ContainsKey(id))
        {
            return connectedPlayers[id];
        }
        else
        {
            KnightsMobile.BasicPlayer tempPlayer = new KnightsMobile.BasicPlayer();
            FirebaseManager.instance.UpdateUserData(id, tempPlayer);
            return tempPlayer;
        }
    }
}
