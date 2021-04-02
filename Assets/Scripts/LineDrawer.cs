using UnityEngine;
using Mirror;

public class LineDrawer : NetworkBehaviour
{
	public LayerMask cantDrawOverLayer;
	public GameObject linePrefab;
	public GameObject leftSideInkImage;
	public GameObject rightSideInkImage;
	[HideInInspector] public Gradient lineColor;
	private GameObject currentLine;
	private GameObject leftSideInkImageGameObject;
	private GameObject rightSideInkImageGameObject;
	private Vector2 serverMousePosition;
	private Color redInk = new Color(0.9490197f, 0.1882353f, 0.2039216f, 1.0f);
	private Color blueInk = new Color(0.2235294f, 0.6f, 0.8352942f, 1.0f);
	private Color yellowInk = new Color(0.8352942f, 0.7568628f, 0.2235294f, 1.0f);

	[Command]
	public void LeftSideRedButton()
	{
		lineColor.SetKeys(
			new GradientColorKey[] { new GradientColorKey(redInk, 1.0f) },
			new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 1.0f) }
		);

		leftSideInkImageGameObject.GetComponent<LeftSideInkImage>().ChangeImageToRed();
	}

	[Command]
	public void LeftSideBlueButton()
	{
		lineColor.SetKeys(
			new GradientColorKey[] { new GradientColorKey(blueInk, 1.0f) },
			new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 1.0f) }
		);

		leftSideInkImageGameObject.GetComponent<LeftSideInkImage>().ChangeImageToBlue();
	}

	[Command]
	public void LeftSideYellowButton()
	{
		lineColor.SetKeys(
			new GradientColorKey[] { new GradientColorKey(yellowInk, 1.0f) },
			new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 1.0f) }
		);

		leftSideInkImageGameObject.GetComponent<LeftSideInkImage>().ChangeImageToYellow();
	}

	[Command]
	public void RightSideRedButton()
	{
		lineColor.SetKeys(
			new GradientColorKey[] { new GradientColorKey(redInk, 1.0f) },
			new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 1.0f) }
		);

		rightSideInkImageGameObject.GetComponent<RightSideInkImage>().ChangeImageToRed();
	}

	[Command]
	public void RightSideBlueButton()
	{
		lineColor.SetKeys(
			new GradientColorKey[] { new GradientColorKey(blueInk, 1.0f) },
			new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 1.0f) }
		);

		rightSideInkImageGameObject.GetComponent<RightSideInkImage>().ChangeImageToBlue();
	}

	[Command]
	public void RightSideYellowButton()
	{
		lineColor.SetKeys(
			new GradientColorKey[] { new GradientColorKey(yellowInk, 1.0f) },
			new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 1.0f) }
		);

		rightSideInkImageGameObject.GetComponent<RightSideInkImage>().ChangeImageToYellow();
	}

	[Command]
	private void BeginDraw()
	{
		GameObject newline = Instantiate(linePrefab);
		NetworkServer.Spawn(newline);
		SetCurrentLine(newline);
	}

	[Command]
	private void Draw()
	{
		currentLine.GetComponent<Line>().RunDrawBaseMouse(serverMousePosition);
	}

	[Command]
	private void SetServerMousePosition(Vector2 mouseposition)
    {
		serverMousePosition = mouseposition;
	}

	[Command]
	private void SpawnLeftSideInkImage()
	{
		GameObject localLeftSideInkImageGameObject = Instantiate(leftSideInkImage);
		NetworkServer.Spawn(localLeftSideInkImageGameObject);
		SetLeftSideImageGameObject(localLeftSideInkImageGameObject);
	}

	[Command]
	private void SpawnRightSideInkImage()
	{
		GameObject localRightSideInkImageGameObject = Instantiate(rightSideInkImage);
		NetworkServer.Spawn(localRightSideInkImageGameObject);
		SetRightSideImageGameObject(localRightSideInkImageGameObject);
	}



	[ClientRpc]
	public void SetCurrentLine(GameObject newline)
	{
		currentLine = newline;
	}

	[ClientRpc]
	private void SetLeftSideImageGameObject(GameObject localLeftSideInkImageGameObject)
	{
		leftSideInkImageGameObject = localLeftSideInkImageGameObject;
	}

	[ClientRpc]
	private void SetRightSideImageGameObject(GameObject localRightSideInkImageGameObject)
	{
		rightSideInkImageGameObject = localRightSideInkImageGameObject;
	}



	[Client]
	void Start()
    {
		if (!isClientOnly)
		{
			SpawnLeftSideInkImage();
		}

		if (isClientOnly)
		{
			SpawnRightSideInkImage();
		}
	}

	[Client]
    void Update()
	{
		if (!hasAuthority) 
		{ 
			return; 
		}

		//Vector3 mouseposition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		///SetServerMousePosition(mouseposition);

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
			if (!isClientOnly)
			{
				LeftSideRedButton();
			}

			if (isClientOnly)
			{
				RightSideRedButton();
			}
		}

		if (Input.GetKeyDown(KeyCode.X))
		{
			if (!isClientOnly)
			{
				LeftSideBlueButton();
			}

			if (isClientOnly)
			{
				RightSideBlueButton();
			}
		}

		if (Input.GetKeyDown(KeyCode.C))
		{
			if (!isClientOnly)
			{
				LeftSideYellowButton();
			}

			if (isClientOnly)
			{
				RightSideYellowButton();
			}
		}
	}
}