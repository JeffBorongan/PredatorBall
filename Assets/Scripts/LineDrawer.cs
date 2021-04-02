﻿using UnityEngine;
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
	public void RedButton()
	{
		if (!hasAuthority)
		{
			return;
		}

		lineColor.SetKeys(
			new GradientColorKey[] { new GradientColorKey(redInk, 1.0f) },
			new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 1.0f) }
		);

		leftSideInkImageGameObject.GetComponent<LeftSideInkImage>().ChangeImageToRed();
	}

	[Command]
	public void BlueButton()
	{
		if (!hasAuthority)
		{
			return;
		}

		lineColor.SetKeys(
			new GradientColorKey[] { new GradientColorKey(blueInk, 1.0f) },
			new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 1.0f) }
		);

		leftSideInkImageGameObject.GetComponent<LeftSideInkImage>().ChangeImageToBlue();
	}

	[Command]
	public void YellowButton()
	{
		if (!hasAuthority)
		{
			return;
		}

		lineColor.SetKeys(
			new GradientColorKey[] { new GradientColorKey(yellowInk, 1.0f) },
			new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 1.0f) }
		);

		leftSideInkImageGameObject.GetComponent<LeftSideInkImage>().ChangeImageToYellow();
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
		if (!hasAuthority)
		{
			return;
		}

		GameObject localLeftSideInkImageGameObject = Instantiate(leftSideInkImage);
		NetworkServer.Spawn(localLeftSideInkImageGameObject);
		SetLeftSideImageGameObject(localLeftSideInkImageGameObject);
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



	[Client]
	void Start()
    {
		if (!hasAuthority)
		{
			return;
		}

		SpawnLeftSideInkImage();
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