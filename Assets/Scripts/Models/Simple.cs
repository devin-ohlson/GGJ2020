using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Simple : Breakable
{
    // Immediately repair the object
    protected override IEnumerator StartRepairing()
    {
        Repair();
        return null;
    }

    protected override void StopRepairing()
    {
        // No way to stop repairing, since repair happens immediately
    }

    public override bool TryInteract(CharacterCtrl controller)
    {
        if (Input.GetAxisRaw("Interact") != 0)
        {
            Debug.Log(Input.GetAxisRaw("Interact"));
            Interact(controller);
            return true;
        }
        return false;
    }
}
