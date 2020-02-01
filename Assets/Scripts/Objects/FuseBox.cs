using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuseBox : MonoBehaviour, Interactable {
	private Canvas canvas;

	[SerializeField] private Room[] rooms;
	[SerializeField] private Fuse[] fuses;

	private void Start() {
		canvas = GetComponentInChildren<Canvas>();

		for(int i = 0; i < rooms.Length; i++) {
			fuses[i].SetRoom(rooms[i]);
		}
	}

	public void Interact(CharacterCtrl controller) {
		canvas.enabled = true;
	}

	public bool Reset() {
		canvas.enabled = false;
		return true;
	}

	public bool TryInteract(CharacterCtrl controller) {
		if (Input.GetAxisRaw("Interact") != 0) {
			Interact(controller);
			return true;
		}
		return false;
	}
}
