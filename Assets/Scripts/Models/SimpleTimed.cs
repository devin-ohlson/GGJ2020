using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleTimed : Simple
{
	public float holdDuration = 2.5f;

	[SerializeField] private CircleFillAnimation actionAnimation = null;

	private void Start()
	{
		actionAnimation.gameObject.SetActive(false);
	}

	public override bool TryInteract(CharacterCtrl controller)
	{
		actionAnimation.gameObject.SetActive(true);
		return base.TryInteract(controller);
	}

	protected override IEnumerator StartRepairing()
	{
		actionAnimation.SetDuration(holdDuration);
		actionAnimation.Listen(FinishRepairing);

		while (true)
		{
			actionAnimation.Activate(Input.GetButtonDown("Interact"));
			yield return null;
		}
	}

	protected bool FinishRepairing()
	{
		Repair();
		StopRepairing();
		return true;
	}

	protected override void StopRepairing()
	{
		StopCoroutine(StartRepairing());
		actionAnimation.Reset();
		actionAnimation.gameObject.SetActive(false);
	}

	public override BreakableLevel Level() => BreakableLevel.TIMED;
}
