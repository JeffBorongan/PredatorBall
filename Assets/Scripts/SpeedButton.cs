using UnityEngine;
using UnityEngine.Events;
using Mirror;

public class SpeedButton : NetworkBehaviour
{
    public UnityEvent buttonClick;
    public InkPlayer InkPlayer;
    void Awake()
    {
        if (buttonClick == null)
        {
            buttonClick = new UnityEvent();
        }
    }

    [ClientRpc]
    public void SetPlayerInk(InkPlayer inkplayer)
    {
        InkPlayer = inkplayer;
    }
    void OnMouseUp()
    {
        buttonClick.Invoke();
    }

    
    public void LsetActiveButtonRed()
    {
       InkPlayer.GetComponent<InkPlayer>().LSetActiveButtonRed(); 
    }

   
    public void LsetActiveButtonYellow()
    {
        InkPlayer.GetComponent<InkPlayer>().LSetActiveButtonYellow();
    }

    public void LsetActiveButtonGreen()
    {
        InkPlayer.GetComponent<InkPlayer>().LSetActiveButtonGreen();
    }

    public void RsetActiveButtonRed()
    {
        InkPlayer.GetComponent<InkPlayer>().RSetActiveButtonRed();
    }


    public void RsetActiveButtonYellow()
    {
        InkPlayer.GetComponent<InkPlayer>().RSetActiveButtonYellow();
    }

    public void RsetActiveButtonGreen()
    {
        InkPlayer.GetComponent<InkPlayer>().RSetActiveButtonGreen();
    }


}
