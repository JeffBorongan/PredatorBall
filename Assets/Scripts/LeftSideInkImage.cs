using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class LeftSideInkImage : NetworkBehaviour
{
    public Sprite redInkSprite;
    public Sprite blueInkSprite;
    public Sprite yellowInkSprite;

    [ClientRpc]
    public void ChangeImageToRed()
    {
        gameObject.GetComponent<Image>().transform.SetParent(GameObject.FindGameObjectWithTag("Left Side Bottle").transform, false);
        gameObject.GetComponent<Image>().sprite = redInkSprite;
        gameObject.GetComponent<Image>().type = Image.Type.Filled;
        gameObject.GetComponent<Image>().fillMethod = Image.FillMethod.Vertical;
        gameObject.GetComponent<Image>().fillOrigin = 0;
        gameObject.GetComponent<Image>().fillAmount = 100f;
    }

    [ClientRpc]
    public void ChangeImageToBlue()
    {
        gameObject.GetComponent<Image>().transform.SetParent(GameObject.FindGameObjectWithTag("Left Side Bottle").transform, false);
        gameObject.GetComponent<Image>().sprite = blueInkSprite;
        gameObject.GetComponent<Image>().type = Image.Type.Filled;
        gameObject.GetComponent<Image>().fillMethod = Image.FillMethod.Vertical;
        gameObject.GetComponent<Image>().fillOrigin = 0;
        gameObject.GetComponent<Image>().fillAmount = 100f;
    }

    [ClientRpc]
    public void ChangeImageToYellow()
    {
        gameObject.GetComponent<Image>().transform.SetParent(GameObject.FindGameObjectWithTag("Left Side Bottle").transform, false);
        gameObject.GetComponent<Image>().sprite = yellowInkSprite;
        gameObject.GetComponent<Image>().type = Image.Type.Filled;
        gameObject.GetComponent<Image>().fillMethod = Image.FillMethod.Vertical;
        gameObject.GetComponent<Image>().fillOrigin = 0;
        gameObject.GetComponent<Image>().fillAmount = 100f;
    }
}
