using UnityEngine;
using UnityEngine.Events;

public class SpeedButton : MonoBehaviour
{
    public UnityEvent buttonClick;

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
}
