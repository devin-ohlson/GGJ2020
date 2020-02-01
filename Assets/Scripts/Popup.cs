using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Popup : MonoBehaviour, Breakable
{

    public Canvas canvas;

    public void Interact(CharacterCtrl controller){
        canvas.gameObject.SetActive(true);
    }

    public void Break(){

    }
}
