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

    
    void OnMouseUp()
    {
        buttonClick.Invoke();
    }

    
    public void setActiveButtonRed()
    {

            InkPlayer.GetComponent<InkPlayer>().SetActiveButtonRed();
        
    }

   
    public void setActiveButtonYellow()
    {
        InkPlayer.GetComponent<InkPlayer>().SetActiveButtonYellow();
    }

    public void setActiveButtonGreen()
    {
        InkPlayer.GetComponent<InkPlayer>().SetActiveButtonGreen();
    }

}
