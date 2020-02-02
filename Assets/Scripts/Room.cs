using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour {
	public bool IsStartingRoom;
	[SerializeField] private List<Direction> connectionDirections;
	private bool[] connectionsArray;
	public float RoomWidth = 3;

	private Breakable[] breakables;
	private BreakableLight breakableLight;
	
	private CameraController mainCam;
	private StairCtrl stairs;

	public bool CurrentRoom;
	public bool HasVisitor;
	public bool LightsPowered = true;

	//Stuff for blacking the room out
	[SerializeField] private SpriteRenderer blackOverlay;
	[SerializeField] private float blackoutDuration = 0.75f;
	[SerializeField] private float blackoutOpacity = 0.8f;
	[SerializeField] private float powerOutModifier = 0.3f;

	void Start() {
		breakables = GetComponentsInChildren<Breakable>();
		breakableLight = GetComponentInChildren<BreakableLight>();
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
			CurrentRoom = true;
			StopAllCoroutines();
			StartCoroutine(RoomFade(false));
		}
		else if (collision.gameObject.tag == "Visitor") {
			collision.gameObject.GetComponent<NPCVisitor>().EnterRoom(this);
			HasVisitor = true;
		}
	}
	private void OnTriggerExit2D(Collider2D collision) {
		if (collision.gameObject.tag == "Player") {
			CurrentRoom = false;
			StopAllCoroutines();
			StartCoroutine(RoomFade(true));
		}
		else if(collision.gameObject.tag == "Visitor") {
			HasVisitor = false;
		}
	}

	private IEnumerator RoomFade(bool fadeOut) {
		float timer = 0;
		//It only turns half on if the power is out
		float lightModifier = 1;
		if (!fadeOut && !LightsPowered)
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

	//Stuff for the fuses:
	public void SetLightPower(bool on) {
		LightsPowered = on;
		if (CurrentRoom) {
			Color newBlack = blackOverlay.color;
			if (LightsPowered)
				newBlack.a = 0;
			else
				newBlack.a = blackoutOpacity / 2;
			blackOverlay.color = newBlack;
		}
	}
}