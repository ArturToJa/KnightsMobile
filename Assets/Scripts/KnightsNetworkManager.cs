using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightsNetworkManager : NetworkManager
{
    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        Debug.Log("Client connects");
        GameObject player = Instantiate(playerPrefab);
        NetworkServer.AddPlayerForConnection(conn, player);
    }
    public override void OnServerDisconnect(NetworkConnection conn)
    {
        KnightsMobile.Player player = conn.identity.gameObject.GetComponent<KnightsMobile.Player>();
        if(player != null)
        {
            // do something in Player script
        }
        // call base functionality (actually destroys the player)
        base.OnServerDisconnect(conn);
    }

    public override void OnClientConnect(NetworkConnection conn)
    {
        Debug.Log("I connected to the server");
    }
}
