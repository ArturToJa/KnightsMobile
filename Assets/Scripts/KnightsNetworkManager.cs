using Firebase.Auth;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightsNetworkManager : NetworkManager
{
    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        Debug.Log("Client connects");
        GameObject playerObject = Instantiate(playerPrefab);
        KnightsMobile.Player player = playerObject.GetComponent<KnightsMobile.Player>();

        if(player == null)
        {
            Debug.LogError("Player prefab doesn't contain KnightsMobile.Player script");
            return;
        }

        FirebaseUser user = (FirebaseUser)conn.authenticationData;

        if(user == null)
        {
            Debug.LogError("Authentication data is not of FirebaseUser type");
            return;
        }

        player.playerName = user.DisplayName;
        FirebaseManager.instance.ReadUserData(user.UserId, player);
        NetworkServer.AddPlayerForConnection(conn, playerObject);
        NetworkPlayerManager.instance.AddPlayer(user.UserId, player);
    }

    public override void OnServerDisconnect(NetworkConnection conn)
    {
        Debug.Log("Client disconnected");

        FirebaseUser user = (FirebaseUser)conn.authenticationData;

        if (user == null)
        {
            Debug.LogError("Authentication data is not of FirebaseUser type");
            return;
        }

        NetworkPlayerManager.instance.RemovePlayer(user.UserId);

        if (conn.identity == null)
        {
            Debug.LogError("NetworkConnection doesn't have identity");
            return;
        }

        KnightsMobile.Player player = conn.identity.gameObject.GetComponent<KnightsMobile.Player>();

        if (player == null)
        {
            Debug.LogError("NetworkConnection identity doesn't have Player script");
            return;
        }

        // do something in Player script
        FirebaseManager.instance.UpdateUserData(user.UserId, player);

        // call base functionality (actually destroys the player)
        base.OnServerDisconnect(conn);
    }

    public override void OnClientConnect(NetworkConnection conn)
    {
        Debug.Log("I connected to the server");
        base.OnClientConnect(conn);
    }

    public override void OnClientDisconnect(NetworkConnection conn)
    {
        Debug.Log("I disconnected from the server");
        base.OnClientDisconnect(conn);
    }
}
