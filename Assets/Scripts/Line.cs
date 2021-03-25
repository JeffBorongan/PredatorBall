using UnityEngine;
using System.Collections.Generic;
using Mirror;
using System.Collections;

public class Line : NetworkBehaviour
{
	public LineRenderer lineRenderer;
	public EdgeCollider2D edgeCollider;
	public EdgeCollider2D edgeColliderTriggerable;
	[HideInInspector] public List<Vector2> fingerPositions;
	[HideInInspector] public int pointsCount = 0;
	float linePointsMinDistance;
	[Command]
	public void SetLineColor(Gradient colorGradient)
	{
		lineRenderer.colorGradient = colorGradient;
	}
	[Command]
	public void SetLinePointsMinDistance(float distance)
	{
		linePointsMinDistance = distance;
	}
	[Command]
	public void SetLineWidth(float width)
	{
		lineRenderer.startWidth = width;
		lineRenderer.endWidth = width;
	}
	[Command]
	public void CreateLine()
	{
		fingerPositions.Clear();
		fingerPositions.Add(Camera.main.ScreenToWorldPoint(Input.mousePosition));
		fingerPositions.Add(Camera.main.ScreenToWorldPoint(Input.mousePosition));
		//lineRenderer.SetPosition(0, fingerPositions[0]);
		//lineRenderer.SetPosition(1, fingerPositions[1]);
		edgeCollider.points = fingerPositions.ToArray();
		edgeColliderTriggerable.points = fingerPositions.ToArray();
	}
	[Command]
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

	[Server]
	public void DestroySelf()
    {
		Destroy(gameObject);
		NetworkServer.Destroy(gameObject);
    }

	[Server]
	public IEnumerator DestroySelfAfter(float seconds)
    {
		yield return new WaitForSeconds(seconds);
		NetworkServer.Destroy(gameObject);
	}


}