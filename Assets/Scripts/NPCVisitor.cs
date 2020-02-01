using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCVisitor : MonoBehaviour {
	[SerializeField] private float walkSpeed;

	[SerializeField] private float timeInRoom;
	private float remainingTimeInRoom;
	[SerializeField] private float movementStateDuration;
	private float remainingStateTime;

	[SerializeField] private Room currentRoom;

	private Rigidbody2D rb;

	private bool exitingRoom = false, switchingRooms, idleWalking = true;
	private bool walkingRight = true;

	private void Start() {
		rb = GetComponent<Rigidbody2D>();

		remainingTimeInRoom = timeInRoom;
		remainingStateTime = movementStateDuration;
	}

	private void FixedUpdate() {
		if (switchingRooms)
			SwitchRooms();
		else {
			if (idleWalking)
				IdleWalk();

			//Switches between stationary and walking within room
			remainingStateTime -= Time.deltaTime;
			if(remainingStateTime < 0) {
				rb.velocity = new Vector2(0, 0);
				idleWalking = !idleWalking;
				remainingStateTime = movementStateDuration;
				walkingRight = Random.Range(0, 2) == 1;
			}

			remainingTimeInRoom -= Time.deltaTime;
			if(remainingTimeInRoom < 0) {
				walkingRight = currentRoom.GetConnectingDirection();
				exitingRoom = true;
				switchingRooms = true;
			}
		}
	}

	private void SwitchRooms() {
		rb.velocity = new Vector2(walkSpeed * (walkingRight ? 1 : -1), 0);

		//Once the player has entered the new room, walk close to the center
		if (!exitingRoom) {
			if (Mathf.Abs(transform.position.x - currentRoom.transform.position.x) < 0.5f) {
				switchingRooms = false;
				idleWalking = true;
			}
		}
	}

	private void IdleWalk() {
		rb.velocity = new Vector2(walkSpeed * (walkingRight ? 1 : -1), 0);
		//Flip it around if it's too far from center
		if (Vector2.Distance(transform.position, currentRoom.transform.position) > currentRoom.RoomWidth / 2) {
			walkingRight = !walkingRight;
			rb.velocity = new Vector2(walkSpeed * (walkingRight ? 1 : -1), 0);
		}
	}

	public void EnterRoom(Room newRoom) {
		currentRoom = newRoom;
		remainingTimeInRoom = timeInRoom;
		remainingStateTime = movementStateDuration;
		exitingRoom = false;
	}
}