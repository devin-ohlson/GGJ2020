using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//For Jackson
public class VisitorEmotion : MonoBehaviour
{
	private VisitorMovement movement;
	private Room currentRoom;

    void Start()
    {
		movement = GetComponent<VisitorMovement>();
    }
	
    void Update()
    {
        
    }

	//VisitorMovement calls this when it enters a new room
	public void SearchRoom(Room newRoom) {
		currentRoom = newRoom;
	}

	//call LeaveHouse() when happiness is 0
}
