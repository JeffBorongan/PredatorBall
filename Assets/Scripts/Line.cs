using UnityEngine;
using System.Collections.Generic;
using Mirror;

public class Line : NetworkBehaviour
{
	[HideInInspector] public List<Vector2> fingerPositions;
	[HideInInspector] public int pointsCount = 0;
	public string LineTag = "GreenInk";
	private float linePointsMinDistance;

	[ClientRpc]
	public void SetLineColor(Gradient colorGradient)
	{
		gameObject.GetComponent<LineRenderer>().colorGradient = colorGradient;
	}

	[ClientRpc]
	public void SetLinePointsMinDistance(float distance)
	{
		linePointsMinDistance = distance;
	}

	[ClientRpc]
	public void SetLineWidth(float width)
	{
		gameObject.GetComponent<LineRenderer>().startWidth = width;
		gameObject.GetComponent<LineRenderer>().endWidth = width;
	}

	[ClientRpc]
	public void CreateLine(Vector2 newFingerPosition)
	{
		fingerPositions.Clear();
		fingerPositions.Add(newFingerPosition);
		fingerPositions.Add(newFingerPosition);
		gameObject.GetComponent<LineRenderer>().SetPosition(0, fingerPositions[0]);
		gameObject.GetComponent<LineRenderer>().SetPosition(1, fingerPositions[1]);
		gameObject.GetComponents<EdgeCollider2D>()[0].points = fingerPositions.ToArray();
		gameObject.GetComponents<EdgeCollider2D>()[1].points = fingerPositions.ToArray();
	}

	[ClientRpc]
	public void AddPoint(Vector2 newFingerPosition)
	{
		if (pointsCount >= 1 && Vector2.Distance(newFingerPosition, fingerPositions[fingerPositions.Count - 1]) < linePointsMinDistance)
		{
			return;
		}

		fingerPositions.Add(newFingerPosition);
		pointsCount++;
		gameObject.GetComponent<LineRenderer>().positionCount = pointsCount;
		gameObject.GetComponent<LineRenderer>().SetPosition(pointsCount - 1, newFingerPosition);

		if (pointsCount > 1)
		{
			gameObject.GetComponents<EdgeCollider2D>()[0].points = fingerPositions.ToArray();
			gameObject.GetComponents<EdgeCollider2D>()[1].points = fingerPositions.ToArray();
		}
	}
}