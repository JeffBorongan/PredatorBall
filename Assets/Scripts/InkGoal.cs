using UnityEngine;

public class InkGoal : MonoBehaviour
{
    public InkPlayer player;
    public string GoalTag = "LeftSideGoal";

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponent<Ball>().BallTag == "ball")
        {
            player.GetComponent<InkPlayer>().AddScore();
        }
    }
}