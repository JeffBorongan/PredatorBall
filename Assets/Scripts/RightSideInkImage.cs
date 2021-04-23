using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class RightSideInkImage : NetworkBehaviour
{
    public Sprite redInkSprite;
    public Sprite yellowInkSprite;
    public Sprite greenInkSprite;

    [ClientRpc]
    public void ChangeImageToRed(int redCurrentInk)
    {
        gameObject.GetComponent<Image>().transform.SetParent(GameObject.FindGameObjectWithTag("Right Side Bottle").transform, false);
        gameObject.GetComponent<Image>().sprite = redInkSprite;
        gameObject.GetComponent<Image>().type = Image.Type.Filled;
        gameObject.GetComponent<Image>().fillMethod = Image.FillMethod.Vertical;
        gameObject.GetComponent<Image>().fillOrigin = 0;
        gameObject.GetComponent<Image>().fillAmount = Mathf.Clamp(redCurrentInk / 100.0f, 0.0f, 1.0f);
    }

    [ClientRpc]
    public void ChangeImageToYellow(int yellowCurrentInk)
    {
        gameObject.GetComponent<Image>().transform.SetParent(GameObject.FindGameObjectWithTag("Right Side Bottle").transform, false);
        gameObject.GetComponent<Image>().sprite = yellowInkSprite;
        gameObject.GetComponent<Image>().type = Image.Type.Filled;
        gameObject.GetComponent<Image>().fillMethod = Image.FillMethod.Vertical;
        gameObject.GetComponent<Image>().fillOrigin = 0;
        gameObject.GetComponent<Image>().fillAmount = Mathf.Clamp(yellowCurrentInk / 100.0f, 0.0f, 1.0f);
    }

    [ClientRpc]
    public void ChangeImageToGreen(int greenCurrentInk)
    {
        gameObject.GetComponent<Image>().transform.SetParent(GameObject.FindGameObjectWithTag("Right Side Bottle").transform, false);
        gameObject.GetComponent<Image>().sprite = greenInkSprite;
        gameObject.GetComponent<Image>().type = Image.Type.Filled;
        gameObject.GetComponent<Image>().fillMethod = Image.FillMethod.Vertical;
        gameObject.GetComponent<Image>().fillOrigin = 0;
        gameObject.GetComponent<Image>().fillAmount = Mathf.Clamp(greenCurrentInk / 100.0f, 0.0f, 1.0f);
    }
}