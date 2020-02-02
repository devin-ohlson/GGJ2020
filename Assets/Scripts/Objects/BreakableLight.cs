﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableLight : Breakable {
	private Room parentRoom;

	void Start() {
		parentRoom = GetComponentInParent<Room>();
	}

	protected override void SetBroken(bool isBroken) {
		base.SetBroken(isBroken);
		parentRoom.SetLightPower(!isBroken);
	}

	protected override IEnumerator StartRepairing() {
		Debug.Log("Player shouldn't touch light!");
		yield return new WaitForEndOfFrame();
	}
	protected override void StopRepairing() { }
	public override bool TryInteract(CharacterCtrl controller) {
		Debug.Log("Player shouldn't interact with lights!");
		return false;
	}
}