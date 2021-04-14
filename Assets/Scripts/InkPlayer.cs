using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class InkPlayer : NetworkBehaviour
{
    public GameObject PlayerUIPrefab;
    public GameObject RedButtonPrefab;
    public GameObject YellowButtonPrefab;
    public GameObject GreenButtonPrefab;
    public int PlayerNumber;

    GameObject playerUI;
    GameObject Redbutton;
    GameObject Yellowbutton;
    GameObject Greenbutton;
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

        if(PlayerNumber == 1)
        {
            Redbutton = Instantiate(RedButtonPrefab);
            NetworkServer.Spawn(Redbutton, connectionToClient);
            Redbutton.transform.SetParent(GameObject.FindGameObjectWithTag("LeftSidePlayer").transform, false);
            Redbutton.GetComponent<SpeedButton>().InkPlayer = this;

            Yellowbutton = Instantiate(YellowButtonPrefab);
            NetworkServer.Spawn(Yellowbutton, connectionToClient);
            Yellowbutton.transform.SetParent(GameObject.FindGameObjectWithTag("LeftSidePlayer").transform, false);
            Yellowbutton.GetComponent<SpeedButton>().InkPlayer = this;

            Greenbutton = Instantiate(GreenButtonPrefab);
            NetworkServer.Spawn(Greenbutton, connectionToClient);
            Greenbutton.transform.SetParent(GameObject.FindGameObjectWithTag("LeftSidePlayer").transform, false);
            Greenbutton.GetComponent<SpeedButton>().InkPlayer = this;
        }
        

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

    public void SetActiveButtonRed()
    {
        if(PlayerNumber == 1)
        {
            gameObject.GetComponent<LineDrawer>().LeftSideRedButton();
        }
        
    }

    public void SetActiveButtonYellow()
    {
        gameObject.GetComponent<LineDrawer>().LeftSideYellowButton();
    }

    public void SetActiveButtonGreen()
    {
        gameObject.GetComponent<LineDrawer>().LeftSideGreenButton();
    }


}
