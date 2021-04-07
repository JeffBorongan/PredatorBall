using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InkGoal : MonoBehaviour
{
    public string GoalTag = "LeftSideGoal";
    public InkPlayer player;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponent<Ball>().BallTag == "ball")
        {
            player.GetComponent<InkPlayer>().AddScore();
        }
    }

}
