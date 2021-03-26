using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class LineDrawer : NetworkBehaviour
{
	public LayerMask cantDrawOverLayer;
	public GameObject linePrefab;
	GameObject currentLine;

	[Command]
	void BeginDraw()
	{
		GameObject newline = Instantiate(linePrefab);
		NetworkServer.Spawn(newline);
		currentLine = newline;
	}

	[Command]
	void Draw()
	{
		Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		currentLine.GetComponent<Line>().drawBaseOnMouse(mousePosition);
	}

	
	void Update()
	{
		if(!hasAuthority) 
		{ 
			return; 
		}

		if (Input.GetMouseButtonDown(0))
        {
			BeginDraw();
		}

		if (currentLine != null)
			Draw();

    }
}