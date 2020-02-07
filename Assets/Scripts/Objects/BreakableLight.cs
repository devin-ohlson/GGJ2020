using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableLight : Breakable {
	protected override void Start() {
		base.Start();
		fixedSprite = spriteRenderer.sprite;
		brokenSprite = spriteRenderer.sprite;
	}

	public override void Break() {
		base.Break();
		parentRoom.SetLightPower(false);
	}

	protected override void Repair() {
		base.Repair();
		parentRoom.SetLightPower(true);
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

	public override BreakableLevel Level() => BreakableLevel.PUZZLE; //???
}
