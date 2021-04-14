using UnityEngine;
using System.Collections.Generic;
using Mirror;

[AddComponentMenu("")]
public class InkNetworkManager : NetworkManager
{
    public Vector2 spawnLimits;
    GameObject ball;
    public GameObject ballToManipulate;

    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        GameObject player = Instantiate(playerPrefab);
        NetworkServer.AddPlayerForConnection(conn, player);
        player.GetComponent<InkPlayer>().PlayerNumber = numPlayers;

        if (numPlayers == 2)
        {
            ball = Instantiate(ballToManipulate);
            Vector2 spawnPosition = new Vector2(-0.13f, Random.Range(-spawnLimits.y, spawnLimits.y));
            ball.transform.position = spawnPosition;
            NetworkServer.Spawn(ball);
        }
    }

    public override void OnServerDisconnect(NetworkConnection conn)
    {
        if (ball != null)
            NetworkServer.Destroy(ball);

        base.OnServerDisconnect(conn);
    }
}