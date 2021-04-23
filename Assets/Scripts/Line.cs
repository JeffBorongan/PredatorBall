using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Line : NetworkBehaviour
{
	[HideInInspector] public List<Vector2> fingerPositions;
	[HideInInspector] public int pointsCount = 0;
	[HideInInspector] public Gradient lineColor;
	public string lineTag = "YellowInk";

	private float linePointsMinDistance;
	private Color redInk = new Color(0.9490197f, 0.1882353f, 0.2039216f, 1.0f);
	private Color yellowInk = new Color(0.8352942f, 0.7568628f, 0.2235294f, 1.0f);
	private Color greenInk = new Color(0.3803922f, 0.8352942f, 0.1568628f, 1.0f);

	[ClientRpc]
	public void SetLineColor(string color)
	{
		if (color == "Red")
        {
			lineColor.SetKeys(
				new GradientColorKey[] { new GradientColorKey(redInk, 1.0f) },
				new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 1.0f) }
			);

			lineTag = "RedInk";
			gameObject.GetComponent<LineRenderer>().colorGradient = lineColor;
		}

		else if (color == "Yellow")
        {
			lineColor.SetKeys(
				new GradientColorKey[] { new GradientColorKey(yellowInk, 1.0f) },
				new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 1.0f) }
			);

			lineTag = "YellowInk";
			gameObject.GetComponent<LineRenderer>().colorGradient = lineColor;
		}

		else if (color == "Green")
		{
			lineColor.SetKeys(
				new GradientColorKey[] { new GradientColorKey(greenInk, 1.0f) },
				new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 1.0f) }
			);

			lineTag = "GreenInk";
			gameObject.GetComponent<LineRenderer>().colorGradient = lineColor;
		}
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