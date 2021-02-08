using UnityEngine;
using UnityEngine.UI;

public class LineDrawer : MonoBehaviour
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
	Line currentLine;
	int currentInk;
	int redCurrentInk;
	int blueCurrentInk;
	int yellowCurrentInk;
	Color redInk = new Color(0.9490197f, 0.1882353f, 0.2039216f, 1.0f);
	Color blueInk = new Color(0.2235294f, 0.6f, 0.8352942f, 1.0f);
	Color yellowInk = new Color(0.8352942f, 0.7568628f, 0.2235294f, 1.0f);

	public void RedButton()
	{
		lineColor.SetKeys(
			new GradientColorKey[] { new GradientColorKey(redInk, 1.0f) },
			new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 1.0f) }
		);

		leftSideInkImage.sprite = redInkSprite;
		leftSideInkImage.type = Image.Type.Filled;
		leftSideInkImage.fillMethod = Image.FillMethod.Vertical;
		leftSideInkImage.fillOrigin = 0;
		leftSideInkImage.fillAmount = Mathf.Clamp(redCurrentInk / 100.0f, 0.0f, 1.0f);
	}

	public void BlueButton()
	{
		lineColor.SetKeys(
			new GradientColorKey[] { new GradientColorKey(blueInk, 1.0f) },
			new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 1.0f) }
		);

		leftSideInkImage.sprite = blueInkSprite;
		leftSideInkImage.type = Image.Type.Filled;
		leftSideInkImage.fillMethod = Image.FillMethod.Vertical;
		leftSideInkImage.fillOrigin = 0;
		leftSideInkImage.fillAmount = Mathf.Clamp(blueCurrentInk / 100.0f, 0.0f, 1.0f);
	}

	public void YellowButton()
	{
		lineColor.SetKeys(
			new GradientColorKey[] { new GradientColorKey(yellowInk, 1.0f) },
			new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 1.0f) }
		);

		leftSideInkImage.sprite = yellowInkSprite;
		leftSideInkImage.type = Image.Type.Filled;
		leftSideInkImage.fillMethod = Image.FillMethod.Vertical;
		leftSideInkImage.fillOrigin = 0;
		leftSideInkImage.fillAmount = Mathf.Clamp(yellowCurrentInk / 100.0f, 0.0f, 1.0f);
	}

	void BeginDraw()
	{
		currentLine = Instantiate(linePrefab, this.transform).GetComponent<Line>();
		currentLine.SetLineColor(lineColor);
		currentLine.SetLinePointsMinDistance(pointsMinDistance);
		currentLine.SetLineWidth(lineWidth);
		currentLine.CreateLine();
	}

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

		if (currentInk > 0)
		{
			Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			RaycastHit2D hit = Physics2D.CircleCast(mousePosition, lineWidth / 3f, Vector2.zero, 1f, cantDrawOverLayer);

			if (hit)
				EndDraw();
			else
				if (Vector2.Distance(currentLine.fingerPositions[0], currentLine.fingerPositions[currentLine.fingerPositions.Count - 1]) <= maximumLineLength && currentLine.pointsCount <= maximumLinePoints)
					currentLine.AddPoint(mousePosition);
		}
	}

	void EndDraw()
	{
		currentInk = 0;
		if (currentLine != null)
		{
			if (currentLine.pointsCount < 2)
			{
				Destroy(currentLine.gameObject);
			}
			else
			{
				if (lineColor.colorKeys[0].color == redInk)
				{
					redCurrentInk -= currentLine.pointsCount;
					leftSideInkImage.fillAmount = Mathf.Clamp(redCurrentInk / 100.0f, 0.0f, 1.0f);
					currentLine.gameObject.tag = "Red Ink";
				}
				else if (lineColor.colorKeys[0].color == blueInk)
				{
					blueCurrentInk -= currentLine.pointsCount;
					leftSideInkImage.fillAmount = Mathf.Clamp(blueCurrentInk / 100.0f, 0.0f, 1.0f);
					currentLine.gameObject.tag = "Blue Ink";
				}
				else if (lineColor.colorKeys[0].color == yellowInk)
				{
					yellowCurrentInk -= currentLine.pointsCount;
					leftSideInkImage.fillAmount = Mathf.Clamp(yellowCurrentInk / 100.0f, 0.0f, 1.0f);
					currentLine.gameObject.tag = "Yellow Ink";
				}

				Destroy(currentLine.gameObject, 3.0f);
				currentLine.gameObject.layer = LayerMask.NameToLayer("Obstacle");
				currentLine = null;
			}
		}
	}

	void Start()
	{
		redCurrentInk = 100;
		blueCurrentInk = 100;
		yellowCurrentInk = 100;
		YellowButton();
	}

	void Update()
	{
		if (Input.GetMouseButtonDown(0))
			BeginDraw();

		if (currentLine != null)
			Draw();

		if (Input.GetMouseButtonUp(0))
			EndDraw();
	}
}