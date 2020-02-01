﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour {
	public bool IsStartingRoom;
	public bool HasLeftDoor, HasRightDoor;

	private Breakable[] breakables;

	private CameraController mainCam;

	//Stuff for blacking the room out
	[SerializeField] private SpriteRenderer blackOverlay;
	[SerializeField] private float blackoutDuration = 0.75f;
	[SerializeField] private float blackoutOpacity = 0.8f;

	void Start() {
		breakables = GetComponentsInChildren<Breakable>();
		mainCam = Camera.main.gameObject.GetComponent<CameraController>();

		if (!IsStartingRoom) {
			Color blackedOut = blackOverlay.color;
			blackedOut.a = blackoutOpacity;
			blackOverlay.color = blackedOut;
		}
	}

	private void OnTriggerEnter2D(Collider2D collision) {
		if (collision.gameObject.tag == "Player") {
			StartCoroutine(ActivateRoom());
		}
	}
	private void OnTriggerExit2D(Collider2D collision) {
		if (collision.gameObject.tag == "Player") {
			StartCoroutine(BlackoutRoom());
		}
	}

	public IEnumerator ActivateRoom() {
		mainCam.LerpToPosition(transform.position);
		float timer = 0;

		while(timer < blackoutDuration) {
			Color newColor = blackOverlay.color;
			newColor.a -= Time.deltaTime * (blackoutOpacity / blackoutDuration);
			blackOverlay.color = newColor;

			timer += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}

		Color finalColor = blackOverlay.color;
		finalColor.a = 0;
		blackOverlay.color = finalColor;
	}
	public IEnumerator BlackoutRoom() {
		float timer = 0;

		while(timer < blackoutDuration) {
			Color newColor = blackOverlay.color;
			newColor.a += Time.deltaTime * (blackoutOpacity / blackoutDuration);
			blackOverlay.color = newColor;

			timer += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}

		Color finalColor = blackOverlay.color;
		finalColor.a = blackoutOpacity;
		blackOverlay.color = finalColor;
	}

	//NPCs can use these to evaluate the room and navigate
	public Breakable[] GetBreakables() {
		return breakables;
	}
}