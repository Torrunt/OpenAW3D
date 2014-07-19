using UnityEngine;
using System.Collections;

public class StrategyCamera : MonoBehaviour
{

	private Game Game;

	private int scrollDistance = 2; 
	private float scrollSpeed = 10;

	// Use this for initialization
	void Start ()
	{
		Game = GameObject.Find("Game").GetComponent<Game>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Game.Selector.CurrentUnit != null && (Game.Selector.CurrentUnit.IsMoving() || Game.Selector.CurrentUnit.IsWaitingForMoveAccept()))
			return;

		// Left / Right
		if (Input.mousePosition.x < scrollDistance) 
			transform.Translate(Vector3.right * -scrollSpeed * Time.deltaTime); 
		else if (Input.mousePosition.x >= Screen.width - scrollDistance) 
			transform.Translate(Vector3.right * scrollSpeed * Time.deltaTime); 

		// Forward / Backward
		if (Input.mousePosition.y < scrollDistance) 
			transform.Translate((Vector3.forward - transform.forward) * -scrollSpeed * Time.deltaTime); 
		else if (Input.mousePosition.y >= Screen.height - scrollDistance) 
			transform.Translate((Vector3.forward - transform.forward) * scrollSpeed * Time.deltaTime);

		// Zooming
		if (Input.GetAxis("Mouse ScrollWheel") < 0)
			transform.Translate((Vector3.forward + transform.forward) * -scrollSpeed * Time.deltaTime);
		else if (Input.GetAxis("Mouse ScrollWheel") > 0)
			transform.Translate((Vector3.forward + transform.forward) * scrollSpeed * Time.deltaTime);
	}
}
