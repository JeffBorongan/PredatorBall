using UnityEngine;
using Mirror;

[AddComponentMenu("")]
public class InkNetworkManager : NetworkManager
{
    public GameObject ballToManipulate;
    public Vector2 spawnLimits;

    private GameObject ball;

    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        GameObject player = Instantiate(playerPrefab);
        NetworkServer.AddPlayerForConnection(conn, player);
        player.GetComponent<InkPlayer>().PlayerNumber = numPlayers;

        if (numPlayers == 1)
        {
            ball = Instantiate(ballToManipulate);
            Vector2 spawnPosition = new Vector2(-0.13f, Random.Range(-spawnLimits.y, spawnLimits.y));
            ball.transform.position = spawnPosition;
            NetworkServer.Spawn(ball);
            player.GetComponent<InkPlayer>().PlayerNumber = numPlayers;
        }
    }

    public override void OnServerDisconnect(NetworkConnection conn)
    {
        if (ball != null)
            NetworkServer.Destroy(ball);

        base.OnServerDisconnect(conn);
    }
}