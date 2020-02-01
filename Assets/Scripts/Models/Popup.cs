using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Popup : Breakable
{

	public Canvas canvas;

	// Starting the repair happens instantly, but the repair should be called 
	protected override IEnumerator StartRepairing()
	{
		canvas.gameObject.SetActive(true);
		// Need to do popup specific data initializing
		return null;
	}

	protected override void StopRepairing()
	{
		canvas.gameObject.SetActive(false);
		// Need to do popup specific data reseting
	}

	public override bool TryInteract(CharacterCtrl controller)
	{
		if (Input.GetAxisRaw("Interact") != 0)
		{
			Interact(controller);
			return true;
		}
		return false;
	}
}
