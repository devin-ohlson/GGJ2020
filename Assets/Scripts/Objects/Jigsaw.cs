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

    protected override void Awake(){
        solution = (Sprite[])images.Clone();
        for(int i = 0; i < solution.Length; i++){
            Debug.Log(solution[i].name);
        }
        GenerateJigsaw();
        base.Awake();
    }

    public void Update(){
        if(isInteracting){
            DragHandler[] currentPuzzle = assembledBoard.GetComponentsInChildren<DragHandler>();
            for (int i = 0; i < currentPuzzle.Length; i++){
                Debug.Log(currentPuzzle[i].GetImage().sprite.name);
            }

            if(currentPuzzle.Length == solution.Length){
                if(isPuzzleSolved(currentPuzzle, solution)){
                    Debug.Log("Puzzle was solved!/n*/n*/n*/n*/n*/n*/n*/n*/n*/n*/n*");
                    Repair();
                }
            }
        }
    }

    bool isPuzzleSolved(DragHandler[] currentPuzzle, Sprite[] solution){
        for(int i = 0; i < solution.Length; i++){
            if(currentPuzzle[i].GetImage().sprite != solution[i]){
                Debug.Log("Current puzzle: " + currentPuzzle[i].GetImage().sprite.name);
                Debug.Log("Solution puzzle: " + solution[i].name);
                Debug.Log("Puzzle is not solved");
                return false;
            }
        }
        return true;
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
        for (int i = 0; i < images.Length; i++){
            Image slotImage = slots[i].GetImage();
            slotImage.sprite = images[i];
        }
    }
}
