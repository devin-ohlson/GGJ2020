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
	private StairCtrl roomStairs;

	private Rigidbody2D rb;

	private State currentState;
	private Direction walkingDirection = Direction.Right;

	private enum State {
		Idle,
		IdleWalk,
		EnteringRoom,
		ExitingRoom,
		Leaving
	}

	private void Start() {
		rb = GetComponent<Rigidbody2D>();

		currentState = State.IdleWalk;
		remainingTimeInRoom = timeInRoom;
		remainingStateTime = movementStateDuration;
	}

	private void FixedUpdate() {
		if (currentState == State.EnteringRoom || currentState == State.ExitingRoom)
			SwitchRooms();
		else {
			if (currentState == State.IdleWalk)
				IdleWalk();
			else
				rb.velocity = new Vector2(0, 0);

			//Switches between stationary and walking within room
			remainingStateTime -= Time.deltaTime;
			if(remainingStateTime < 0) { 
				if (currentState == State.Idle)
					currentState = State.IdleWalk;
				else if (currentState == State.IdleWalk)
					currentState = State.Idle;
				remainingStateTime = movementStateDuration;
				walkingDirection = (Random.Range(0, 2) == 0 ? Direction.Left : Direction.Right);
			}

			remainingTimeInRoom -= Time.deltaTime;
			if(remainingTimeInRoom < 0) {
				walkingDirection = currentRoom.GetConnectingDirection();
				currentState = State.ExitingRoom;
			}
		}
	}

	private void SwitchRooms() {
		if (walkingDirection == Direction.Right || walkingDirection == Direction.Left) {
			rb.velocity = new Vector2(walkSpeed * (walkingDirection == Direction.Right ? 1 : -1), 0);
		}
		else {
			if (transform.position.x < roomStairs.transform.position.x)
				rb.velocity = new Vector2(walkSpeed, 0);
			else
				rb.velocity = new Vector2(-walkSpeed, 0);
		}

		//Once the player has entered the new room, walk close to the center
		if (currentState == State.EnteringRoom) {
			if (Mathf.Abs(transform.position.x - currentRoom.transform.position.x) < 0.5f) {
				currentState = State.Idle;
			}
		}
	}

	private void IdleWalk() {
		rb.velocity = new Vector2(walkSpeed * (walkingDirection == Direction.Right ? 1 : -1), 0);
		//Flip it around if it's too far from center
		if (Vector2.Distance(transform.position, currentRoom.transform.position) > currentRoom.RoomWidth / 2) {
			walkingDirection = DirectionMethods.OppositeDirection(walkingDirection);
			rb.velocity = new Vector2(walkSpeed * (walkingDirection == Direction.Right ? 1 : -1) * 2, 0);
		}
	}

	public void EnterRoom(Room newRoom) {
		currentRoom = newRoom;
		roomStairs = currentRoom.GetStairs();
		remainingTimeInRoom = timeInRoom;
		remainingStateTime = movementStateDuration;

		if (transform.position.x < currentRoom.transform.position.x)
			walkingDirection = Direction.Right;
		else
			walkingDirection = Direction.Left;

		currentState = State.EnteringRoom;
	}

	public void EnterStairs(StairCtrl stairs, bool canGoUp, bool canGoDown) {
		if (currentState == State.ExitingRoom) {
			if (walkingDirection == Direction.Up) {
				stairs.Travel(transform, true);
			}
			else if(walkingDirection == Direction.Down) {
				stairs.Travel(transform, false);
			}
		}
	}
}