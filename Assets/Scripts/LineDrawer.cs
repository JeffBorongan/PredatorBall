using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class LineDrawer : NetworkBehaviour
{
	public LayerMask cantDrawOverLayer;
	public GameObject linePrefab;
	public Sprite redInkSprite;
	public Sprite blueInkSprite;
	public Sprite yellowInkSprite;
	public Image leftSideInkImage;
	public Image rightSideInkImage;
	[HideInInspector] public Gradient lineColor;
	public float pointsMinDistance;
	public float lineWidth;
	public float maximumLineLength;
	public int maximumLinePoints;

	GameObject currentLine;
	int currentInk;

	int yellowCurrentInk;

	Color yellowInk = new Color(0.8352942f, 0.7568628f, 0.2235294f, 1.0f);

	[Command]
	public void YellowButton()
	{
		lineColor.SetKeys(
			new GradientColorKey[] { new GradientColorKey(yellowInk, 1.0f) },
			new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 1.0f) }
		);
	}

	[Command]
	void CmdBeginDraw()
	{
		GameObject newline = Instantiate(linePrefab, this.transform);
		NetworkServer.Spawn(newline,connectionToClient);
		currentLine = newline;
		//currentLine.SetLineColor(lineColor);
		currentLine.GetComponent<Line>().SetLinePointsMinDistance(pointsMinDistance);
		currentLine.GetComponent<Line>().SetLineWidth(lineWidth);
		currentLine.GetComponent<Line>().CreateLine();
	}

	[Command]
	void CmdDraw()
	{

		 if (lineColor.colorKeys[0].color == yellowInk)
		{
			currentInk = yellowCurrentInk;
		}

		if (currentInk > 0)
		{
			Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			RaycastHit2D hit = Physics2D.CircleCast(mousePosition, lineWidth / 3f, Vector2.zero, 1f, cantDrawOverLayer);

			if (hit)
			{
				CmdEndDraw();
				
			}
			else
				if (Vector2.Distance(currentLine.GetComponent<Line>().fingerPositions[0], currentLine.GetComponent<Line>().fingerPositions[currentLine.GetComponent<Line>().fingerPositions.Count - 1]) <= maximumLineLength && currentLine.GetComponent<Line>().pointsCount <= maximumLinePoints)
            {
				currentLine.GetComponent<Line>().AddPoint(mousePosition);
			}
					
			
		}
	}

	[Command]
	void CmdEndDraw()
	{
		currentInk = 0;
		if (currentLine != null)
		{
			if (currentLine.GetComponent<Line>().pointsCount < 2)
			{
				currentLine.GetComponent<Line>().DestroySelf();
			}
			else
			{

                currentLine.GetComponent<Line>().DestroySelfAfter(3);
				currentLine.gameObject.layer = LayerMask.NameToLayer("Obstacle");
				currentLine = null;
			}
		}
		else
        {
			print("null ang current line");
        }
	}

	void Start()
	{
		if(!isLocalPlayer)
        {
			return;
        }
		yellowCurrentInk = 100;
		YellowButton();
	}
	
	void Update()
	{
		if(!isLocalPlayer) { return; }

		if (Input.GetMouseButtonDown(0))
        {
			CmdBeginDraw();
		}
			

		if (currentLine != null)
			CmdDraw();

		if (Input.GetMouseButtonUp(0))
			CmdEndDraw();

		if (Input.GetKeyDown(KeyCode.X))
        {
			currentLine.transform.position = currentLine.transform.position + new Vector3(1, 0, 0);
        }
	}
}