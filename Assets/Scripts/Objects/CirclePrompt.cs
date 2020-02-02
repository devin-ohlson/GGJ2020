using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CirclePrompt : Prompt
{
	[SerializeField] private float radialMod = 3f;
	private Vector2 previousMouse;
	private Vector3 previousPromptPos;

	protected override IEnumerator StartRepairing()
	{
		previousMouse = WorldMouse();
		previousPromptPos = prompt.transform.position;
		return base.StartRepairing();
	}

	protected override void UpdateProgress()
	{
		Vector3 delta = MouseDelta();

		Vector3 previousVec = previousPromptPos * radialMod;

		Vector3 a = previousVec + delta;

		previousPromptPos = a / radialMod;

		float deltaTheta = Vector3.Angle(previousVec, a);

		if (deltaTheta > 0)
		{
			MoveRotator(deltaTheta);
			progress += deltaTheta / 360;
		}
	}

	private Vector2 WorldMouse()
	{
		return Camera.main.ScreenToWorldPoint(Input.mousePosition);
	}

	// Mouse delta updates the previous mouse position, so any successive call in one frame will return 0 delta.
	private Vector2 MouseDelta()
	{
		Vector2 delta = WorldMouse() - previousMouse;
		previousMouse = WorldMouse();
		return delta;
	}

	private void MoveRotator(float deltaTheta)
	{
		prompt.transform.Rotate(Vector3.forward * deltaTheta);
	}
}
