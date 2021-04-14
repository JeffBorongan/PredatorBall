using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class InkPlayer : NetworkBehaviour
{
    public GameObject PlayerUIPrefab;
    public GameObject LRedButtonPrefab;
    public GameObject LYellowButtonPrefab;
    public GameObject LGreenButtonPrefab;
    public GameObject RRedButtonPrefab;
    public GameObject RYellowButtonPrefab;
    public GameObject RGreenButtonPrefab;

    [SyncVar]
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
            Redbutton = Instantiate(LRedButtonPrefab);
            NetworkServer.Spawn(Redbutton, connectionToClient);
            Redbutton.transform.SetParent(GameObject.FindGameObjectWithTag("LeftSidePlayer").transform, false);
            Redbutton.GetComponent<SpeedButton>().SetPlayerInk(this);

            Yellowbutton = Instantiate(LYellowButtonPrefab);
            NetworkServer.Spawn(Yellowbutton, connectionToClient);
            Yellowbutton.transform.SetParent(GameObject.FindGameObjectWithTag("LeftSidePlayer").transform, false);
            Yellowbutton.GetComponent<SpeedButton>().SetPlayerInk(this);

            Greenbutton = Instantiate(LGreenButtonPrefab);
            NetworkServer.Spawn(Greenbutton, connectionToClient);
            Greenbutton.transform.SetParent(GameObject.FindGameObjectWithTag("LeftSidePlayer").transform, false);
            Greenbutton.GetComponent<SpeedButton>().SetPlayerInk(this);
        }

        if(PlayerNumber == 2)
        {
            Redbutton = Instantiate(RRedButtonPrefab);
            NetworkServer.Spawn(Redbutton, connectionToClient);
            Redbutton.transform.SetParent(GameObject.FindGameObjectWithTag("RightSidePlayer").transform, false);
            Redbutton.GetComponent<SpeedButton>().SetPlayerInk(this);

            Yellowbutton = Instantiate(RYellowButtonPrefab);
            NetworkServer.Spawn(Yellowbutton, connectionToClient);
            Yellowbutton.transform.SetParent(GameObject.FindGameObjectWithTag("RightSidePlayer").transform, false);
            Yellowbutton.GetComponent<SpeedButton>().SetPlayerInk(this);

            Greenbutton = Instantiate(RGreenButtonPrefab);
            NetworkServer.Spawn(Greenbutton, connectionToClient);
            Greenbutton.transform.SetParent(GameObject.FindGameObjectWithTag("RightSidePlayer").transform, false);
            Greenbutton.GetComponent<SpeedButton>().SetPlayerInk(this);
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

    public void LSetActiveButtonRed()
    {
       gameObject.GetComponent<LineDrawer>().LeftSideRedButton();
    }

    public void LSetActiveButtonYellow()
    {
       gameObject.GetComponent<LineDrawer>().LeftSideYellowButton();
    }

    public void LSetActiveButtonGreen()
    {
        gameObject.GetComponent<LineDrawer>().LeftSideGreenButton();
    }

    public void RSetActiveButtonRed()
    {
        gameObject.GetComponent<LineDrawer>().RightSideRedButton();
    }

    public void RSetActiveButtonYellow()
    {
        gameObject.GetComponent<LineDrawer>().RightSideYellowButton();
    }

    public void RSetActiveButtonGreen()
    {
        gameObject.GetComponent<LineDrawer>().RightSideGreenButton();
    }


}
