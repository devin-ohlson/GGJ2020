using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prompt : Breakable
{
	protected override IEnumerator StartRepairing()
	{
		throw new System.NotImplementedException();
	}

	protected override void StopRepairing()
	{
		throw new System.NotImplementedException();
	}

	// Prompts always start their interaction
	public override bool TryInteract(CharacterCtrl controller)
	{
		Interact(controller);
		return true;
	}
}
