using UnityEngine;
using Mirror;

public class LineDrawer : NetworkBehaviour
{
	public LayerMask cantDrawOverLayer;
	public GameObject linePrefab;
	public GameObject leftSideInkImage;
	public GameObject rightSideInkImage;
	public float pointsMinDistance;
	public float lineWidth;
	public float maximumLineLength;
	public int maximumLinePoints;

	[SyncVar] private GameObject leftSideInkImageGameObject;
	[SyncVar] private string lineColor;
	private GameObject currentLine;
	private GameObject rightSideInkImageGameObject;
	private Vector2 serverMousePosition;

    #region
    [Command]
	public void LeftSideRedButton()
	{
		lineColor = "Red";
		leftSideInkImageGameObject.GetComponent<LeftSideInkImage>().ChangeImageToRed();
	}

	[Command]
	public void LeftSideYellowButton()
	{
		lineColor = "Yellow";
		leftSideInkImageGameObject.GetComponent<LeftSideInkImage>().ChangeImageToYellow();
	}

	[Command]
	public void LeftSideGreenButton()
	{
		lineColor = "Green";
		leftSideInkImageGameObject.GetComponent<LeftSideInkImage>().ChangeImageToGreen();
	}

	[Command]
	public void RightSideRedButton()
	{
		lineColor = "Red";
		rightSideInkImageGameObject.GetComponent<RightSideInkImage>().ChangeImageToRed();
	}

	[Command]
	public void RightSideYellowButton()
	{
		lineColor = "Yellow";
		rightSideInkImageGameObject.GetComponent<RightSideInkImage>().ChangeImageToYellow();
	}

	[Command]
	public void RightSideGreenButton()
	{
		lineColor = "Green";
		rightSideInkImageGameObject.GetComponent<RightSideInkImage>().ChangeImageToGreen();
	}
	#endregion

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

	[Command]
	private void BeginDraw()
	{
		GameObject newline = Instantiate(linePrefab);
		NetworkServer.Spawn(newline);
		newline.GetComponent<Line>().SetLineColor(lineColor);
		newline.GetComponent<Line>().SetLinePointsMinDistance(pointsMinDistance);
		newline.GetComponent<Line>().SetLineWidth(lineWidth);
		newline.GetComponent<Line>().CreateLine(serverMousePosition);
		SetCurrentLine(newline);
	}

	[Command]
	private void SetServerMousePosition(Vector2 mousePosition)
	{
		serverMousePosition = mousePosition;
	}

	[Command]
	private void Draw()
	{
		if (Vector2.Distance(currentLine.GetComponent<Line>().fingerPositions[0], currentLine.GetComponent<Line>().fingerPositions[currentLine.GetComponent<Line>().fingerPositions.Count - 1]) <= maximumLineLength && currentLine.GetComponent<Line>().pointsCount <= maximumLinePoints)
		{
			currentLine.GetComponent<Line>().AddPoint(serverMousePosition);
		}
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

	[ClientRpc]
	public void SetCurrentLine(GameObject newline)
	{
		currentLine = newline;
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