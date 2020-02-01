using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public interface Interactable
{
    void Interact(CharacterCtrl controller); // Called by the player to interact with this interactable
}
