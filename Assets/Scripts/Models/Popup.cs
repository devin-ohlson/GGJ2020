using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Popup : Breakable
{
	private Canvas canvas;

	protected override void Start() {
		base.Start();
		Debug.Log("Start of Popup called");
		canvas = GetComponentInChildren<Canvas>();
	}

	// Starting the repair happens instantly, but the repair should be called 
	protected override IEnumerator StartRepairing()
	{
		canvas.enabled = true;
		// Need to do popup specific data initializing
		yield return null;
	}

	protected override void StopRepairing()
	{
		canvas.enabled = false;
		// Need to do popup specific data reseting
	}

	public override bool TryInteract(CharacterCtrl controller)
	{
		if (Input.GetButtonDown("Interact"))
		{
			Interact(controller);
			return true;
		}
		return false;
	}

	protected override void Repair(){
		base.Repair();
		canvas.enabled = false;
	}

	public override BreakableLevel Level() => BreakableLevel.PUZZLE;
}
