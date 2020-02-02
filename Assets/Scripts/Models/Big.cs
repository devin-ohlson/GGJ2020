using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Big : Interactable
{
	public void Interact(CharacterCtrl controller){

	}

	public void Break(){

	}

	public bool Reset()
	{
		throw new System.NotImplementedException();
	}

	public bool TryInteract(CharacterCtrl controller)
	{
		if (Input.GetButtonDown("Interact"))
		{
			Interact(controller);
			return true;
		}
		return false;
	}
}
