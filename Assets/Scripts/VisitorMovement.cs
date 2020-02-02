using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisitorMovement : MonoBehaviour {
	[SerializeField] private float walkSpeed;
	
	[SerializeField] private float timeInRoom;
	private float remainingTimeInRoom;
	[SerializeField] private float movementStateDuration;
	private float remainingStateTime;


	public float timeMod;
	public float sizeMod;
	private float rotationTimer;

	[SerializeField] private Room currentRoom;
	private StairCtrl roomStairs;

	private Rigidbody2D rb;
	private SpriteRenderer spriteRenderer;
	private VisitorEmotion emotions;

	private State currentState;
	private Direction desiredDirection = Direction.Right;

	private enum State {
		Idle,
		IdleWalk,
		EnteringRoom,
		ExitingRoom,
		Leaving
	}

	private void Start() {
		rb = GetComponent<Rigidbody2D>();
		spriteRenderer = GetComponent<SpriteRenderer>();
		emotions = GetComponent<VisitorEmotion>();

		currentState = State.IdleWalk;
		remainingTimeInRoom = timeInRoom;
		remainingStateTime = movementStateDuration;
	}

	public void SetRoom(Room room) {
		currentRoom = room;
	}

	private void FixedUpdate() {
		if (currentState == State.Leaving)
		{
			LeaveHouse();
		}
		else if (currentState == State.EnteringRoom || currentState == State.ExitingRoom)
			SwitchRooms();
		else {
			if (currentState == State.IdleWalk) {
				Walk(desiredDirection);
				IdleWalkCheckBounds();
			}
			else {
				rb.velocity = new Vector2(0, 0);
				transform.rotation = new Quaternion();
			}

			//Switches between stationary and walking within room
			remainingStateTime -= Time.deltaTime;
			if(remainingStateTime < 0) { 
				if (currentState == State.Idle)
					currentState = State.IdleWalk;
				else if (currentState == State.IdleWalk)
					currentState = State.Idle;
				remainingStateTime = movementStateDuration;
				desiredDirection = (Random.Range(0, 2) == 0 ? Direction.Left : Direction.Right);
			}

			remainingTimeInRoom -= Time.deltaTime;
			if(remainingTimeInRoom < 0) {
				desiredDirection = currentRoom.GetConnectingDirection();
				currentState = State.ExitingRoom;
			}
		}
	}

	private void SwitchRooms() {
		if (desiredDirection == Direction.Left || desiredDirection == Direction.Right) {
			Walk(desiredDirection);
		}
		else {
			if (transform.position.x < roomStairs.transform.position.x)
				Walk(Direction.Right);
			else
				Walk(Direction.Left);
		}

		//Once the player has entered the new room, walk close to the center
		if (currentState == State.EnteringRoom) {
			if (Mathf.Abs(transform.position.x - currentRoom.transform.position.x) < 0.5f) {
				currentState = State.Idle;
			}
		}
	}

	private void Walk(Direction direction) {
		switch (direction) {
			case Direction.Right:
				rb.velocity = new Vector2(walkSpeed, 0);
				spriteRenderer.flipX = true;
				break;
			case Direction.Left:
				rb.velocity = new Vector2(-walkSpeed, 0);
				spriteRenderer.flipX = false;
				break;
		}

		float rotation = 0;
		rotationTimer += Time.deltaTime * timeMod;
		rotation = Mathf.Sin(rotationTimer) * sizeMod;
		transform.Rotate(new Vector3(0, 0, rotation));
	}

	//Flip it around if it's too far from room center
	private void IdleWalkCheckBounds() {
		if (Vector2.Distance(transform.position, currentRoom.transform.position) > currentRoom.RoomWidth / 2) {
			desiredDirection = DirectionMethods.OppositeDirection(desiredDirection);
			rb.velocity = new Vector2(walkSpeed * (desiredDirection == Direction.Right ? 1 : -1) * 2, 0);
		}
	}

	public void EnterRoom(Room newRoom) {
		currentRoom = newRoom;
		roomStairs = currentRoom.GetStairs();
		remainingTimeInRoom = timeInRoom;
		remainingStateTime = movementStateDuration;
		emotions.SearchRoom(newRoom);

		if (transform.position.x < currentRoom.transform.position.x)
			desiredDirection = Direction.Right;
		else
			desiredDirection = Direction.Left;

		if (currentState != State.Leaving) currentState = State.EnteringRoom;
	}

	public void EnterStairs(StairCtrl stairs, bool canGoUp, bool canGoDown) {
		if (currentState == State.ExitingRoom) {
			if (desiredDirection == Direction.Up) {
				stairs.Travel(transform, true);
			}
			else if(desiredDirection == Direction.Down) {
				stairs.Travel(transform, false);
			}
		}
	}

	private void LeaveHouse() {
		List<Direction> roomConnections = currentRoom.ConnectionDirections;
		//Can only be > 1 if it has stairs in our house layout
		if(roomConnections.Count > 1) {
			desiredDirection = Direction.Down;
			SwitchRooms();
		}
		else {
			desiredDirection = roomConnections[0];
			SwitchRooms();
		}
		//Later will have to make it walk to the door
		if (currentRoom.IsStartingRoom) {
			GameManager.Instance().CompleteVisitor();
		}
	}

	public void FocusOnLeaving()
	{
		currentState = State.Leaving;
	}
}