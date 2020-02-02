using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BackAndForthPrompt : Prompt
{
	[SerializeField] private Transform leftEdge = null;
	[SerializeField] private Transform rightEdge = null;
	[SerializeField] private Transform ball = null;
	[Tooltip("Note: For optimal fun, bar should be a root object to the left and right edges!")]
	[SerializeField] private SpriteRenderer bar = null;
	[Range(0, 1)]
	[SerializeField] private float barSizeMod = 0.5f;
	
	private float previousMouseX;
	private Vector3 previousBallPos;

	protected override void Start()
	{
		if (null == leftEdge || null == rightEdge || null == bar || null == ball)
			throw new ArgumentException(this.name + " needs both edges, the bar, and the ball for the prompt to be specified!");

		if (rightEdge.position.x < leftEdge.position.x)
		{
			Transform tmp = leftEdge;
			leftEdge = rightEdge;
			rightEdge = tmp;
		}
		base.Start();
	}

	protected override IEnumerator StartRepairing()
	{
		previousMouseX = WorldMouseX();
		previousBallPos = ball.transform.position;
		return base.StartRepairing();
	}

	protected override void StopRepairing()
	{
		base.StopRepairing();
		UpdateBar();
	}

	protected override void UpdateProgress()
	{
		float distance = MouseDelta();

		MoveBall(distance);

		progress += Mathf.Abs(BallDelta());

		// Additional stuffs
		UpdateBar();
	}

	private float WorldMouseX()
	{
		return Camera.main.ScreenToWorldPoint(Input.mousePosition).x;
	}

	// Mouse delta updates the previous mouse position, so any successive call in one frame will return 0 delta.
	private float MouseDelta()
	{
		float delta = WorldMouseX() - previousMouseX;
		previousMouseX = WorldMouseX();
		return delta;
	}

	// Ball delta updates the previous ball position, so any successive call in one frame will return 0 delta.
	private float BallDelta()
	{
		float delta = ball.position.x - previousBallPos.x;
		previousBallPos = ball.position;
		return delta;
	}

	private void MoveBall(float distance)
	{
		Vector3 position = ball.transform.position;
		position.x = Mathf.Clamp(position.x + distance, leftEdge.position.x, rightEdge.position.x);
		ball.transform.position = position;
	}

	private void UpdateBar()
	{
		bar.transform.localScale = barSizeMod * Vector3.right * (progress / neededProgress) + Vector3.one;

		float val = (progress / neededProgress);
		bar.color = new Color(val, val, val);
	}
}
