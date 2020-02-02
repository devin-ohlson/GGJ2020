using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StairCtrl : MonoBehaviour, Interactable
{
    public StairCtrl up = null;
    public StairCtrl down = null;

	//For fading
	[SerializeField] private float travelFadeTime = 0.5f;
	private Color fadeOutColor, fadeInColor;

	void Start()
	{
		if ((null != up && this != up.down) || (null != down && this != down.up))
			throw new ArgumentException(this.name + " has mismatched stairs.");

		fadeOutColor = new Color(1, 1, 1, 0);
		fadeInColor = new Color(1, 1, 1, 1);
	}

	public void Travel(Transform obj, bool isGoingUp)
	{
		StairCtrl destination = (isGoingUp) ? up : down;

		if (destination != null)
		{
			StartCoroutine(TravelDelay(obj, destination.transform));
		}
	}

	private IEnumerator TravelDelay(Transform obj, Transform destination) {
		float fadeTimer = 0;
		SpriteRenderer sRenderer = obj.GetComponent<SpriteRenderer>();
		MovementFreezable freezable = obj.GetComponent<MovementFreezable>();
		freezable.FreezeMovement(true);

		while(fadeTimer < travelFadeTime) {
			sRenderer.color = Color.Lerp(sRenderer.color, fadeOutColor, fadeTimer / travelFadeTime);
			fadeTimer += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}

		Vector3 diff = obj.position - transform.position;
		obj.position = diff + destination.position;
		yield return new WaitForSeconds(travelFadeTime);

		fadeTimer = 0;
		while (fadeTimer < travelFadeTime) {
			sRenderer.color = Color.Lerp(sRenderer.color, fadeInColor, fadeTimer / travelFadeTime);
			fadeTimer += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}

		freezable.FreezeMovement(false);
	}

	private void OnTriggerStay2D(Collider2D collision) {
		if(collision.gameObject.tag == "Visitor") {
			collision.gameObject.GetComponent<NPCVisitor>().EnterStairs(this, up != null, down != null);
		}
	}
	public void Interact(CharacterCtrl controller)
	{
		Travel(controller.transform, Input.GetAxisRaw("Vertical") > 0);
	}

	public bool Reset()
	{
		return false; // Interactions can't be stopped.
	}

	public bool TryInteract(CharacterCtrl controller)
	{
		if (Input.GetButtonDown("Vertical"))
		{
			Travel(controller.transform, Input.GetAxisRaw("Vertical") > 0);
			return true;
		}
		else
			return false;
	}
}
