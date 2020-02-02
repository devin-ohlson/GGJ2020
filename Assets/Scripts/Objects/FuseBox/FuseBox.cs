using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuseBox : MonoBehaviour, Interactable {
	private Canvas canvas;

	private void Start() {
		canvas = GetComponentInChildren<Canvas>();
	}

	public void Interact(CharacterCtrl controller) {
		canvas.enabled = !canvas.enabled;
	}

	public bool Reset() {
		canvas.enabled = false;
		return true;
	}

	public bool TryInteract(CharacterCtrl controller) {
		if (Input.GetButtonDown("Interact") == true) {
			Interact(controller);
			return true;
		}
		return false;
	}
}
