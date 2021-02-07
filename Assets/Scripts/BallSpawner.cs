using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    public GameObject ballPrefab;
    public Vector2 spawnLimits;

    void Start()
    {
        GameObject ball = Instantiate(ballPrefab);
        Vector2 spawnPosition = new Vector2(-0.13f, Random.Range(-spawnLimits.y, spawnLimits.y));
        ball.transform.position = spawnPosition;
    }
}
