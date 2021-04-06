using System.Collections;
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
        playerUI = Instantiate(PlayerUIPrefab,gameObject.transform);
        playerUI.GetComponent<InkPlayerUI>().SetPlayer(this);

        // Invoke all event handlers with the current data
        OnPlayerScored.Invoke(playerScore);
    }

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.J))
        {
            AddScore();
            print("balbal");
        }
    }

    [Server]
    void AddScore()
    {
        playerScore = playerScore + 1;
    }
}
