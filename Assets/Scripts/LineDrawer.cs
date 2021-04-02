using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class LineDrawer : NetworkBehaviour
{
	public LayerMask cantDrawOverLayer;
	public GameObject linePrefab;
	public GameObject leftSideImage;
	public GameObject rightSideImage;
	public Sprite redInkSprite;
	public Sprite blueInkSprite;
	public Sprite yellowInkSprite;
	[HideInInspector] public Gradient lineColor;
	[SerializeField] private GameObject currentLine;

	[SerializeField]
	private Image leftSideInkImage;

	[SerializeField]
	GameObject LeftSideInkImageGameObject;

	private Vector2 ServermousePosition;
	private Color redInk = new Color(0.9490197f, 0.1882353f, 0.2039216f, 1.0f);
	private Color blueInk = new Color(0.2235294f, 0.6f, 0.8352942f, 1.0f);
	private Color yellowInk = new Color(0.8352942f, 0.7568628f, 0.2235294f, 1.0f);

	[Command]
	public void RedButton()
	{
		lineColor.SetKeys(
			new GradientColorKey[] { new GradientColorKey(redInk, 1.0f) },
			new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 1.0f) }
		);

		LeftSideInkImageGameObject.GetComponent<LeftSideInkScript>().changeImageToRed();
	}

	[Command]
	public void BlueButton()
	{
		lineColor.SetKeys(
			new GradientColorKey[] { new GradientColorKey(blueInk, 1.0f) },
			new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 1.0f) }
		);

		LeftSideInkImageGameObject.GetComponent<LeftSideInkScript>().changeImageToBlue();

	}

	[Command]
	public void YellowButton()
	{
		lineColor.SetKeys(
			new GradientColorKey[] { new GradientColorKey(yellowInk, 1.0f) },
			new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 1.0f) }
		);

		LeftSideInkImageGameObject.GetComponent<LeftSideInkScript>().changeImageToYellow();
	}

	[Command]
	void BeginDraw()
	{
		GameObject newline = Instantiate(linePrefab);
		NetworkServer.Spawn(newline);
		SetCurrentLine(newline);
	}

	[Command]
	void Draw()
	{
		currentLine.GetComponent<Line>().RunDrawBaseMouse(ServermousePosition);
	}

	[Command]
	void SetServerMousePosition(Vector2 mouseposition)
    {
		ServermousePosition = mouseposition;
	}

	[ClientRpc]
	public void SetCurrentLine(GameObject newline)
	{
		currentLine = newline;
	}

	[Client]
	void Start()
    {
		if (!hasAuthority)
		{
			return;
		}
		SpawnLeftInkImage();
		
	}

	[Command]
    private void SpawnLeftInkImage()
    {

        GameObject LLeftSideInkImageGameObject = Instantiate(leftSideImage);
        NetworkServer.Spawn(LLeftSideInkImageGameObject);
		SetLeftSideImageGameObject(LLeftSideInkImageGameObject);
		//RedButton();


	}

	[ClientRpc]
    private void SetLeftSideImageGameObject(GameObject LLeftSideInkImageGameObject)
    {
        LeftSideInkImageGameObject = LLeftSideInkImageGameObject;
    }

    void Update()
	{
		if(!hasAuthority) 
		{ 
			return; 
		}

		//Vector3 mouseposition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		//SetServerMousePosition(mouseposition);

		if (Input.GetMouseButtonDown(0))
        {
			//BeginDraw();
		}

		if (this.currentLine != null)
		{
			//Draw();
		}	

		if(Input.GetKeyDown(KeyCode.Z))
        {
			RedButton();
        }

		if (Input.GetKeyDown(KeyCode.X))
		{
			BlueButton();
		}

		if (Input.GetKeyDown(KeyCode.C))
		{
			YellowButton();
		}
	}
}