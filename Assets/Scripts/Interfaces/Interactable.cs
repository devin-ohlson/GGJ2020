using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public interface Interactable
{
    void Interact(CharacterCtrl controller); // Called by the player to interact with this interactable

    /*
     * Called by the player to reset/stop an interaction with this interactable. Returns true if
     * interaction was previously occuring and is now reset/stopped.
     */
    bool Reset();

    /*
     * Called by the player to poll the interactable for its interaction method. Returns true if
     * interaction started.
     * 
     * For interactables that require specific inputs to start an interaction, TryInteract is responsible
     * for checking these inputs before interaction.
     * 
     * For interactables that automatically start an interaction, TryInteract should always succeed.
     */
    bool TryInteract(CharacterCtrl controller);
}
