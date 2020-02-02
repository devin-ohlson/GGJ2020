using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Prompt : Breakable
{
	[SerializeField] protected GameObject prompt = null;

	[SerializeField] protected float neededProgress = 5;
	protected float progress = 0;
	
	protected override IEnumerator StartRepairing()
	{
		progress = 0;

		while (progress < neededProgress)
		{
			UpdateProgress();
			yield return null;
		}

		FinishRepairing();
	}

	protected void FinishRepairing()
	{
		Repair();
		StopRepairing();
	}

	protected override void StopRepairing()
	{
		StopCoroutine(StartRepairing());
		prompt.SetActive(false);
		progress = 0;
	}

	// Prompts always start their interaction
	public override bool TryInteract(CharacterCtrl controller)
	{
		prompt.SetActive(true);
		Interact(controller);
		return true;
	}

	protected abstract void UpdateProgress();
}
