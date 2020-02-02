using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StairCtrl : MonoBehaviour, Interactable
{
    public StairCtrl up = null;
    public StairCtrl down = null;
	
	void Start()
	{
		if ((null != up && this != up.down) || (null != down && this != down.up))
			throw new ArgumentException(this.name + " has mismatched stairs.");
	}

	public void Travel(Transform obj, bool isGoingUp)
	{
		StairCtrl destination = (isGoingUp) ? up : down;

		if (destination != null)
		{
			Vector3 diff = obj.position - transform.position;

			obj.position = diff + destination.transform.position;
		}
	}

	private void OnTriggerStay2D(Collider2D collision) {
		if(collision.gameObject.tag == "Visitor") {
			collision.gameObject.GetComponent<VisitorMovement>().EnterStairs(this, up != null, down != null);
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
