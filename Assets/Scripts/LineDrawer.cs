﻿using UnityEngine;
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
	private int currentInk;
	private int redCurrentInk = 100;
	private int yellowCurrentInk = 100;
	private int greenCurrentInk = 100;

	#region
	[Command]
	public void LeftSideRedButton()
	{
		lineColor = "Red";
		leftSideInkImageGameObject.GetComponent<LeftSideInkImage>().ChangeImageToRed(redCurrentInk);
	}

	[Command]
	public void LeftSideYellowButton()
	{
		lineColor = "Yellow";
		leftSideInkImageGameObject.GetComponent<LeftSideInkImage>().ChangeImageToYellow(yellowCurrentInk);
	}

	[Command]
	public void LeftSideGreenButton()
	{
		lineColor = "Green";
		leftSideInkImageGameObject.GetComponent<LeftSideInkImage>().ChangeImageToGreen(greenCurrentInk);
	}

	[Command]
	public void RightSideRedButton()
	{
		lineColor = "Red";
		rightSideInkImageGameObject.GetComponent<RightSideInkImage>().ChangeImageToRed(redCurrentInk);
	}

	[Command]
	public void RightSideYellowButton()
	{
		lineColor = "Yellow";
		rightSideInkImageGameObject.GetComponent<RightSideInkImage>().ChangeImageToYellow(yellowCurrentInk);
	}

	[Command]
	public void RightSideGreenButton()
	{
		lineColor = "Green";
		rightSideInkImageGameObject.GetComponent<RightSideInkImage>().ChangeImageToGreen(greenCurrentInk);
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
		if (lineColor == "Red")
		{
			currentInk = redCurrentInk;
		}

		else if (lineColor == "Yellow")
		{
			currentInk = yellowCurrentInk;
		}

		else if (lineColor == "Green")
		{
			currentInk = greenCurrentInk;
		}

		if (currentInk > 0)
        {
			RaycastHit2D hit = Physics2D.CircleCast(serverMousePosition, lineWidth / 3f, Vector2.zero, 1f, cantDrawOverLayer);

			if (hit)
            {
				EndDraw();
            }
			else
            {
				if (Vector2.Distance(this.currentLine.GetComponent<Line>().fingerPositions[0], this.currentLine.GetComponent<Line>().fingerPositions[this.currentLine.GetComponent<Line>().fingerPositions.Count - 1]) <= maximumLineLength && this.currentLine.GetComponent<Line>().pointsCount <= maximumLinePoints)
				{
					this.currentLine.GetComponent<Line>().AddPoint(serverMousePosition);
				}
			}
		}
	}

	[Command]
	void EndDraw()
	{
		currentInk = 0;
		if (this.currentLine != null)
		{
			if (this.currentLine.GetComponent<Line>().pointsCount < 2)
			{
				//Destroy(this.currentLine.gameObject);
			}
			else
			{
				if (lineColor == "Red")
				{
					redCurrentInk -= this.currentLine.GetComponent<Line>().pointsCount;
					if (!isClientOnly)
					{
						leftSideInkImageGameObject.GetComponent<LeftSideInkImage>().ChangeImageToRed(redCurrentInk);
					}

					if (isClientOnly)
					{
						rightSideInkImageGameObject.GetComponent<LeftSideInkImage>().ChangeImageToRed(redCurrentInk);
					}
				}
				else if (lineColor == "Yellow")
				{
					yellowCurrentInk -= this.currentLine.GetComponent<Line>().pointsCount;
					if (!isClientOnly)
					{
						leftSideInkImageGameObject.GetComponent<LeftSideInkImage>().ChangeImageToYellow(yellowCurrentInk);
					}

					if (isClientOnly)
					{
						rightSideInkImageGameObject.GetComponent<LeftSideInkImage>().ChangeImageToYellow(yellowCurrentInk);
					}
				}
				else if (lineColor == "Green")
				{
					greenCurrentInk -= this.currentLine.GetComponent<Line>().pointsCount;
					if (!isClientOnly)
					{
						leftSideInkImageGameObject.GetComponent<LeftSideInkImage>().ChangeImageToGreen(greenCurrentInk);
					}

					if (isClientOnly)
					{
						rightSideInkImageGameObject.GetComponent<LeftSideInkImage>().ChangeImageToGreen(greenCurrentInk);
					}
				}

				Destroy(this.currentLine.gameObject, 3.0f);
				this.currentLine.gameObject.layer = LayerMask.NameToLayer("Obstacle");
				this.currentLine = null;
			}
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

		if (Input.GetMouseButtonUp(0))
        {
			EndDraw();
        }
	}
}