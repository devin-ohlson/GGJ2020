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

	public void SearchRoom(Room newRoom) {
		currentRoom = newRoom;
	}

	//call LeaveHouse() when happiness is 0
}
