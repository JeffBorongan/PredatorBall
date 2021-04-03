using UnityEngine;
using System.Collections.Generic;
using Mirror;

public class Line : NetworkBehaviour
{

	public List<Vector2> fingerPositions;
	[HideInInspector] public int pointsCount = 0;
	public string LineTag = "GreenInk";

	[ClientRpc]
	public void drawBaseOnMouse(Vector2 mousePosition)
	{
		pointsCount++;
		gameObject.GetComponent<LineRenderer>().positionCount = pointsCount;
		gameObject.GetComponent<LineRenderer>().SetPosition(pointsCount - 1, mousePosition);
		fingerPositions.Add(mousePosition);

		gameObject.GetComponents<EdgeCollider2D>()[0].points = fingerPositions.ToArray();
		gameObject.GetComponents<EdgeCollider2D>()[1].points = fingerPositions.ToArray();
	}

	public void RunDrawBaseMouse(Vector2 mousePosition)
    {
		drawBaseOnMouse(mousePosition);
    }

}

