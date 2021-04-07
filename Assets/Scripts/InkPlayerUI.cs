using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class InkPlayerUI : MonoBehaviour
{
    public Text PlayerScore;


    public void SetPlayer(InkPlayer player)
    {
        player.OnPlayerScored += OnPlayerScored;
    }

    private void OnPlayerScored(int score)
    {
        PlayerScore.text = score.ToString();
    }
}
