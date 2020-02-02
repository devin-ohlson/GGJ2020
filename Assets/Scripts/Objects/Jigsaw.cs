using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Jigsaw : Popup
{
    [SerializeField] public Sprite[] images;
    [SerializeField] public GameObject pieces;
    [SerializeField] public GameObject assembledBoard;
    private Sprite[] solution;

    protected override void Start(){  
        base.Start();
        Debug.Log("Jigsaw Awake");
        solution = (Sprite[])images.Clone();
        for(int i = 0; i < solution.Length; i++){
            Debug.Log(solution[i].name);
        }
        GenerateJigsaw(); 
    }

    public void Update(){
        if(IsInteracting){
            DragHandler[] currentPuzzle = assembledBoard.GetComponentsInChildren<DragHandler>();
            for (int i = 0; i < currentPuzzle.Length; i++){
                Debug.Log(currentPuzzle[i].GetImage().sprite.name);
            }

            if(currentPuzzle.Length == solution.Length){
                if(isPuzzleSolved(currentPuzzle, solution)){
                    Repair();
                }
            }
        }
    }

    bool isPuzzleSolved(DragHandler[] currentPuzzle, Sprite[] solution){
        for(int i = 0; i < solution.Length; i++){
            if(currentPuzzle[i].GetImage().sprite != solution[i]){
                return false;
            }
        }
        return true;
    }

    public void GenerateJigsaw(){
        int j;
        Sprite temp;
        for (int i = images.Length - 1; i > 0; i--){
            j = Random.Range(0, i);
            temp = images[i];
            images[i] = images[j];
            images[j] = temp;
        }

        DragHandler[] slots = pieces.GetComponentsInChildren<DragHandler>();
        for (int i = 0; i < images.Length; i++){
            Image slotImage = slots[i].GetImage();
            slotImage.sprite = images[i];
        }
    }
}
