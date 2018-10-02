using UnityEngine;
using System.Collections;

public class StrategyCamera : MonoBehaviour
{

	private Game Game;

	private int scrollDistance = 4; 
	private float scrollSpeed = 10;
    private Vector3 inputVector = Vector3.zero;
    private Vector3 cameraPos = Vector3.zero;

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

        /* Mouse camera movement */
		// Left / Right
		if (Input.mousePosition.x < scrollDistance) 
            transform.Translate(Vector3.right * -scrollSpeed * Time.deltaTime); 
		else if (Input.mousePosition.x >= Screen.width - scrollDistance) 
            transform.Translate(Vector3.right * scrollSpeed * Time.deltaTime); 

		// Forward / Backward
		if (Input.mousePosition.y < scrollDistance) 
            transform.Translate((Quaternion.Euler(0, -35, 0) * Vector3.forward) * -scrollSpeed * Time.deltaTime, Space.World); 
		else if (Input.mousePosition.y >= Screen.height - scrollDistance) 
            transform.Translate((Quaternion.Euler(0, -35, 0) * Vector3.forward) * scrollSpeed * Time.deltaTime, Space.World);

        /* Arrows/WASD camera movement */
        inputVector = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        // Left / Right
        if (inputVector.x != 0)
            transform.Translate(Vector3.right * inputVector.x * scrollSpeed * Time.deltaTime, Space.Self);
        if (inputVector.z != 0)
            transform.Translate((Quaternion.Euler(0, -35, 0) * Vector3.forward) * inputVector.z * scrollSpeed * Time.deltaTime, Space.World);

        // Zooming
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
            transform.Translate(Vector3.forward * -scrollSpeed * Time.deltaTime, Space.Self);
        else if (Input.GetAxis("Mouse ScrollWheel") > 0)
            transform.Translate(Vector3.forward * scrollSpeed * Time.deltaTime, Space.Self);

        //Keep camera in game area
        cameraPos = new Vector3(Mathf.Clamp(transform.position.x, 0, 15), 
                                Mathf.Clamp(transform.position.y, 1, 10), 
                                Mathf.Clamp(transform.position.z, 0, 15));
        transform.position = cameraPos;
	}
}
