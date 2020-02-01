using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour {
	public bool IsStartingRoom;
	[SerializeField] private List<Direction> connectionDirections;
	private bool[] connectionsArray;
	public float RoomWidth = 3;

	private Breakable[] breakables;
	private CameraController mainCam;
	private StairCtrl stairs;

	private bool currentRoom;
	[SerializeField] private bool lightsPowered = true;

	//Stuff for blacking the room out
	[SerializeField] private SpriteRenderer blackOverlay;
	[SerializeField] private float blackoutDuration = 0.75f;
	[SerializeField] private float blackoutOpacity = 0.8f;
	[SerializeField] private float powerOutModifier = 0.3f;

	void Start() {
		breakables = GetComponentsInChildren<Breakable>();
		mainCam = Camera.main.gameObject.GetComponent<CameraController>();

		stairs = GetComponentInChildren<StairCtrl>();
		if(stairs != null) {
			if (stairs.up != null)
				connectionDirections.Add(Direction.Up);
			if (stairs.down != null)
				connectionDirections.Add(Direction.Down);
		}
		
		if (!IsStartingRoom) {
			Color blackedOut = blackOverlay.color;
			blackedOut.a = blackoutOpacity;
			blackOverlay.color = blackedOut;
		}
	}

	private void OnTriggerEnter2D(Collider2D collision) {
		if (collision.gameObject.tag == "Player") {
			mainCam.LerpToPosition(transform.position);
			currentRoom = true;
			StopAllCoroutines();
			StartCoroutine(RoomFade(false));
		}
		else if(collision.gameObject.tag == "Visitor")
			collision.gameObject.GetComponent<NPCVisitor>().EnterRoom(this);
	}
	private void OnTriggerExit2D(Collider2D collision) {
		if (collision.gameObject.tag == "Player") {
			currentRoom = false;
			StopAllCoroutines();
			StartCoroutine(RoomFade(true));
		}
	}

	private IEnumerator RoomFade(bool fadeOut) {
		float timer = 0;
		//It only turns half on if the power is out
		float lightModifier = 1;
		if (!fadeOut && !lightsPowered)
			lightModifier = powerOutModifier;

		while (timer < blackoutDuration && blackOverlay.color.a < blackoutOpacity) {
			Color newColor = blackOverlay.color;
			newColor.a += (Time.deltaTime * (blackoutOpacity / blackoutDuration) * (fadeOut ? 1 : -1)) * lightModifier;
			blackOverlay.color = newColor;

			timer += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}

		Color finalColor = blackOverlay.color;
		finalColor.a = (fadeOut ? blackoutOpacity : blackoutOpacity - blackoutOpacity * lightModifier);
		blackOverlay.color = finalColor;
	}

	//NPCs can use these to evaluate the room and navigate
	public Breakable[] GetBreakables() {
		return breakables;
	}

	public Direction GetConnectingDirection() {
		int index = Random.Range(0, connectionDirections.Count);
		return connectionDirections[index];
	}

	public StairCtrl GetStairs() {
		return stairs;
	}

	public void SetLightPower(bool on) {
		lightsPowered = on;
		if (currentRoom) {
			Color newBlack = blackOverlay.color;
			newBlack.a = blackoutOpacity / 2;
			blackOverlay.color = newBlack;
		}
	}
}