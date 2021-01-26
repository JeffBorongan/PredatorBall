using UnityEngine;

public class LineDrawer : MonoBehaviour
{
	public GameObject linePrefab;
	public LayerMask cantDrawOverLayer;
	int cantDrawOverLayerIndex;
	[Space(30f)]
	public Gradient lineColor;
	public float linePointsMinDistance;
	public float lineWidth;
	Line currentLine;
	int currentInk;
	int redCurrentInk;
	int blueCurrentInk;
	int yellowCurrentInk;
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
		YellowButton();
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
		if (lineColor.colorKeys[0].color == redInk)
		{
			currentInk = redCurrentInk;
		}
		else if (lineColor.colorKeys[0].color == blueInk)
		{
			currentInk = blueCurrentInk;
		}
		else if (lineColor.colorKeys[0].color == yellowInk)
		{
			currentInk = yellowCurrentInk;
		}

		if (currentInk <= totalMaximumInk)
		{
			Vector2 mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);
			//Check if mousePos hits any collider with layer "CantDrawOver", if true cut the line by calling EndDraw( )
			RaycastHit2D hit = Physics2D.CircleCast(mousePosition, lineWidth / 3f, Vector2.zero, 1f, cantDrawOverLayer);
			
			if (hit)
				EndDraw();
			else
				currentLine.AddPoint(mousePosition);
		}
	}
	// End Draw ------------------------------------------------
	void EndDraw()
	{
		currentInk = 0;
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
					redCurrentInk += currentLine.pointsCount;
					currentLine.gameObject.tag = "Red Ink";
                } else if (lineColor.colorKeys[0].color == blueInk)
                {
					blueCurrentInk += currentLine.pointsCount;
					currentLine.gameObject.tag = "Blue Ink";
                } else if (lineColor.colorKeys[0].color == yellowInk)
                {
					yellowCurrentInk += currentLine.pointsCount;
					currentLine.gameObject.tag = "Yellow Ink";
                }
				Destroy(currentLine.gameObject, 3.0f);

				//Add the line to "CantDrawOver" layer
				currentLine.gameObject.layer = cantDrawOverLayerIndex;

				currentLine = null;
			}
		}
	}
}
