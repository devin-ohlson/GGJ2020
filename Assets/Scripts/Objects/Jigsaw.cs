using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Jigsaw : Popup
{
    [SerializeField] public Sprite[] images;
    [SerializeField] public GameObject pieces;
    private Sprite[] solution;

    public void Awake(){
        solution = images;
        GenerateJigsaw();
    }

    public void Update(){
        bool isSolved = true;
        for(int i = 0; i < images.Length; i++){

        }
        if (isSolved){
            Repair();
        }
    }

    public void GenerateJigsaw(){
        int j;
        Sprite temp;
        Debug.Log("Randomizing photos: " + images.Length);
        for (int i = images.Length - 1; i > 0; i--){
            j = Random.Range(0, i);
            temp = images[i];
            images[i] = images[j];
            images[j] = temp;
        }

        DragHandler[] slots = pieces.GetComponentsInChildren<DragHandler>();
        Debug.Log("Putting sprites in slot images: " + slots.Length);
        for (int i = 0; i < images.Length - 1; i++){
            Image slotImage = slots[i].GetImage();
            slotImage.sprite = images[i];
        }
    }
}
