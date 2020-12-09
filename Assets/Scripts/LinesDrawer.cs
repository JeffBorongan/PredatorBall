using UnityEngine;

public class LinesDrawer : MonoBehaviour
{
	public GameObject linePrefab;
	public LayerMask cantDrawOverLayer;
	int cantDrawOverLayerIndex;
	[Space(30f)]
	public Gradient lineColor;
	public float linePointsMinDistance;
	public float lineWidth;
	Line currentLine;
	Line newLine;
	int currentInk;
	public int maximumInkLength = 10;
	public int totalMaximumInk = 100;
	Camera cam;

	void Start()
	{
		cam = Camera.main;
		cantDrawOverLayerIndex = LayerMask.NameToLayer("CantDrawOver");
	}

	void Update()
	{
		if (Input.GetMouseButtonDown(0))
			BeginDraw();

		if (currentLine != null)
			Draw();

		if ((Input.GetMouseButtonUp(0)) || (currentLine.pointsCount > maximumInkLength))
			EndDraw();
	}

	// Begin Draw ----------------------------------------------
	void BeginDraw()
	{
		currentLine = Instantiate(linePrefab, this.transform).GetComponent<Line>();
		//Set line properties
		currentLine.UsePhysics(false);
		currentLine.SetLineColor(lineColor);
		currentLine.SetPointsMinDistance(linePointsMinDistance);
		currentLine.SetLineWidth(lineWidth);
	}
	// Draw ----------------------------------------------------
	void Draw()
	{
		if (currentInk <= totalMaximumInk)
		{
			Vector2 mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);
			//Check if mousePos hits any collider with layer "CantDrawOver", if true cut the line by calling EndDraw( )
			RaycastHit2D hit = Physics2D.CircleCast(mousePosition, lineWidth / 3f, Vector2.zero, 1f, cantDrawOverLayer);
			if (currentLine.pointsCount > 1)
			{
				if (newLine != null)
				{
					Destroy(newLine.gameObject);

					newLine = null;
				}
			}

			if (hit)
				EndDraw();
			else
				currentLine.AddPoint(mousePosition);
		}
	}
	// End Draw ------------------------------------------------
	void EndDraw()
	{
		currentInk += currentLine.pointsCount;
		if (currentLine != null)
		{
			if (currentLine.pointsCount < 2)
			{
				//If line has one point
				Destroy(currentLine.gameObject);
			}
			else
			{
				//Add the line to "CantDrawOver" layer
				currentLine.gameObject.layer = cantDrawOverLayerIndex;

				newLine = currentLine;

				if (newLine != null)
				{
					currentLine = null;
				}
			}
		}
	}
}
