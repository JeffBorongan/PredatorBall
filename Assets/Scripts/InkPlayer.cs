﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class InkPlayer : NetworkBehaviour
{
    public GameObject PlayerUIPrefab;
    GameObject playerUI;
    // Events that the UI will subscribe to
    public event System.Action<int> OnPlayerScored;

    [SyncVar(hook = nameof(PlayerScored))]
    public int playerScore = 100;

    // This is called by the hook of playerScored SyncVar above
    void PlayerScored(int _, int newScore)
    {
        OnPlayerScored?.Invoke(newScore);
    }

    public override void OnStartClient()
    {
        playerUI = Instantiate(PlayerUIPrefab);
        playerUI.transform.SetParent(GameObject.FindGameObjectWithTag("Left Side Bottle").transform, false);
        playerUI.GetComponent<InkPlayerUI>().SetPlayer(this);

        GameObject LeftSideGoal =  GameObject.Find("LeftSideGoal");
        LeftSideGoal.GetComponent<InkGoal>().player = this;


        // Invoke all event handlers with the current data
        OnPlayerScored.Invoke(playerScore);
    }

    [Server]
    public void AddScore()
    {
        playerScore = playerScore + 1;
    }
}