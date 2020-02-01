using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fuse : MonoBehaviour {
	[SerializeField] private Room myRoom;
	private AudioSource sounder;
	[SerializeField] private AudioClip correctSound, wrongSound;

	private void Start() {
		sounder = GetComponent<AudioSource>();
	}

	public void OnClick() {
		if (myRoom.LightsPowered) {
			myRoom.SetLightPower(false);
			sounder.clip = wrongSound;
			sounder.Play();
		}
		else {
			myRoom.SetLightPower(true);
			sounder.clip = correctSound;
			sounder.Play();
		}
	}
}
