using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SlotHandler : MonoBehaviour, IDropHandler
{
    public GameObject item {
        get {
            if (transform.childCount > 0){
                return transform.GetChild(0).gameObject;
            }
            else{
                return null;
            }
        }
    }

    public void OnDrop(PointerEventData eventData){
        Debug.Log("OnDrop");
        if(!item) {
            Debug.Log("New Parent Set: " + transform);
            DragHandler.item.transform.SetParent(transform);
        }
    }
}
