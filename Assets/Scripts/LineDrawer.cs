using UnityEngine;
using Mirror;

public class LineDrawer : NetworkBehaviour
{
	public LayerMask cantDrawOverLayer;
	public GameObject linePrefab;
	public GameObject leftSideInkImage;
	public GameObject rightSideInkImage;
	[HideInInspector] public Gradient lineColor;
	public float pointsMinDistance;
	public float lineWidth;
	public float maximumLineLength;
	public int maximumLinePoints;

	private GameObject currentLine;
	[SyncVar] private GameObject leftSideInkImageGameObject;
	private GameObject rightSideInkImageGameObject;
	private Vector2 serverMousePosition;
	private Color redInk = new Color(0.9490197f, 0.1882353f, 0.2039216f, 1.0f);
	private Color yellowInk = new Color(0.8352942f, 0.7568628f, 0.2235294f, 1.0f);
	private Color greenInk = new Color(0.3803922f, 0.8352942f, 0.1568628f, 1.0f);

    #region
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
	public void LeftSideYellowButton()
	{
		lineColor.SetKeys(
			new GradientColorKey[] { new GradientColorKey(yellowInk, 1.0f) },
			new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 1.0f) }
		);

		leftSideInkImageGameObject.GetComponent<LeftSideInkImage>().ChangeImageToYellow();
	}

	[Command]
	public void LeftSideGreenButton()
	{
		lineColor.SetKeys(
			new GradientColorKey[] { new GradientColorKey(greenInk, 1.0f) },
			new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 1.0f) }
		);

		leftSideInkImageGameObject.GetComponent<LeftSideInkImage>().ChangeImageToGreen();
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
	public void RightSideYellowButton()
	{
		lineColor.SetKeys(
			new GradientColorKey[] { new GradientColorKey(yellowInk, 1.0f) },
			new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 1.0f) }
		);

		rightSideInkImageGameObject.GetComponent<RightSideInkImage>().ChangeImageToYellow();
	}

	[Command]
	public void RightSideGreenButton()
	{
		lineColor.SetKeys(
			new GradientColorKey[] { new GradientColorKey(greenInk, 1.0f) },
			new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 1.0f) }
		);

		rightSideInkImageGameObject.GetComponent<RightSideInkImage>().ChangeImageToGreen();
	}
    #endregion

	public void RedButtonPressed()
    {
		redbuttontestpress();
    }

	private void redbuttontestpress()
    {
		print("iam pressed");
		LeftSideRedButton();
	}

    [Command]
	private void BeginDraw()
	{
		GameObject newline = Instantiate(linePrefab);
		NetworkServer.Spawn(newline);
		newline.GetComponent<Line>().SetLinePointsMinDistance(pointsMinDistance);
		newline.GetComponent<Line>().SetLineWidth(lineWidth);
		newline.GetComponent<Line>().CreateLine(serverMousePosition); 
		SetCurrentLine(newline);
	}

	[Command]
	private void Draw()
	{
		if (Vector2.Distance(currentLine.GetComponent<Line>().fingerPositions[0], currentLine.GetComponent<Line>().fingerPositions[currentLine.GetComponent<Line>().fingerPositions.Count - 1]) <= maximumLineLength && currentLine.GetComponent<Line>().pointsCount <= maximumLinePoints)
		{
			currentLine.GetComponent<Line>().AddPoint(serverMousePosition);
		}
	}

	[Command]
	private void SetServerMousePosition(Vector2 mousePosition)
    {
		serverMousePosition = mousePosition;
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

		Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		SetServerMousePosition(mousePosition);

		if (Input.GetMouseButtonDown(0))
        {
			BeginDraw();
		}

		if (this.currentLine != null)
		{
			Draw();
		}	
	}
}