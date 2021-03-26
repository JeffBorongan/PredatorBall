using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class LineDrawer : NetworkBehaviour
{
	public LayerMask cantDrawOverLayer;
	public GameObject linePrefab;

	[SerializeField]
	private GameObject currentLine;
	Vector2 ServermousePosition;

	[Command]
	void BeginDraw()
	{
		GameObject newline = Instantiate(linePrefab);
		NetworkServer.Spawn(newline);
		setCurrentLine(newline);
	}

	[ClientRpc]
	public void setCurrentLine(GameObject newline)
    {
		currentLine = newline;
    }

	[Command]
	void Draw()
	{
		currentLine.GetComponent<Line>().RunDrawBaseMouse(ServermousePosition);
	}

	[Command]
	void setServerMousePosition(Vector2 mouseposition)
    {
		ServermousePosition = mouseposition;
	}


	
	void Update()
	{
		if(!hasAuthority) 
		{ 
			return; 
		}

		Vector3 mouseposition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		setServerMousePosition(mouseposition);


		if (Input.GetMouseButtonDown(0))
        {
			BeginDraw();
			
		}

		if (this.currentLine != null)
		{
			Draw();
		}	

    }
}