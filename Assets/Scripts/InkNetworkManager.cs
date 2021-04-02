using UnityEngine;
using Mirror;

[AddComponentMenu("")]
public class InkNetworkManager : NetworkManager
{
    public Vector2 spawnLimits;
    GameObject ball;

    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        GameObject player = Instantiate(playerPrefab);
        NetworkServer.AddPlayerForConnection(conn, player);

        if (numPlayers == 2)
        {
            //ball = Instantiate(spawnPrefabs.Find(prefab => prefab.name == "Ball"));
            //Vector2 spawnPosition = new Vector2(-0.13f, Random.Range(-spawnLimits.y, spawnLimits.y));
            //ball.transform.position = spawnPosition;
            //NetworkServer.Spawn(ball);
        }
    }

    public override void OnServerDisconnect(NetworkConnection conn)
    {
        if (ball != null)
            NetworkServer.Destroy(ball);

        base.OnServerDisconnect(conn);
    }
}