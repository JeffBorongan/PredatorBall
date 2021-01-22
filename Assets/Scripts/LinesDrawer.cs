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
	Color redInk = new Color(0.9490197f, 0.1882353f, 0.2039216f, 1.0f);
	Color blueInk = new Color(0.2235294f, 0.6f, 0.8352942f, 1.0f);
	Color yellowInk = new Color(0.8352942f, 0.7568628f, 0.2235294f, 1.0f);


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

	public void RedButton()
    {
		lineColor.SetKeys(
			new GradientColorKey[] { new GradientColorKey(redInk, 1.0f) },
			new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 1.0f) }
		);
	}

	public void BlueButton()
	{
		lineColor.SetKeys(
			new GradientColorKey[] { new GradientColorKey(blueInk, 1.0f) },
			new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 1.0f) }
		);
	}

	public void YellowButton()
	{
		lineColor.SetKeys(
			new GradientColorKey[] { new GradientColorKey(yellowInk, 1.0f) },
			new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 1.0f) }
		);
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
				if (lineColor.colorKeys[0].color == redInk)
                {
					gameObject.tag = "Red Ink";
                } else if (lineColor.colorKeys[0].color == blueInk)
                {
					gameObject.tag = "Blue Ink";
                } else if (lineColor.colorKeys[0].color == yellowInk)
                {
					gameObject.tag = "Yellow Ink";
                }

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
