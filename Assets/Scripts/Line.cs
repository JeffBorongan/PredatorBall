using UnityEngine;
using System.Collections.Generic;

public class Line : MonoBehaviour
{
	public LineRenderer lineRenderer;
	public EdgeCollider2D edgeCollider;
	public EdgeCollider2D edgeColliderTriggerable;
	[HideInInspector] public List<Vector2> fingerPositions;
	[HideInInspector] public int pointsCount = 0;
	float linePointsMinDistance;

	public void SetLineColor(Gradient colorGradient)
	{
		lineRenderer.colorGradient = colorGradient;
	}

	public void SetLinePointsMinDistance(float distance)
	{
		linePointsMinDistance = distance;
	}

	public void SetLineWidth(float width)
	{
		lineRenderer.startWidth = width;
		lineRenderer.endWidth = width;
	}

	public void CreateLine()
	{
		fingerPositions.Clear();
		fingerPositions.Add(Camera.main.ScreenToWorldPoint(Input.mousePosition));
		fingerPositions.Add(Camera.main.ScreenToWorldPoint(Input.mousePosition));
		lineRenderer.SetPosition(0, fingerPositions[0]);
		lineRenderer.SetPosition(1, fingerPositions[1]);
		edgeCollider.points = fingerPositions.ToArray();
		edgeColliderTriggerable.points = fingerPositions.ToArray();
	}

	public void AddPoint(Vector2 newFingerPosition)
	{
		if (pointsCount >= 1 && Vector2.Distance(newFingerPosition, fingerPositions[fingerPositions.Count - 1]) < linePointsMinDistance)
			return;

		fingerPositions.Add(newFingerPosition);
		pointsCount++;
		lineRenderer.positionCount = pointsCount;
		lineRenderer.SetPosition(pointsCount - 1, newFingerPosition);

		if (pointsCount > 1)
			edgeCollider.points = fingerPositions.ToArray();
			edgeColliderTriggerable.points = fingerPositions.ToArray();
	}
}