using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public float minRangeToTransform;
    public float slerpSpeed;
    public static GameObject item;
    Vector3 startPosition;
    Transform startParent;

    private float initialZ;

    public void OnBeginDrag (PointerEventData eventData){

        item = gameObject;
        startPosition = transform.position;
        startParent = transform.parent;
        GetComponent<CanvasGroup>().blocksRaycasts = false;
        if(transform.parent is SlotHandler){
            transform.SetParent(transform.parent.parent);
        }
        else{
            transform.SetParent(transform.parent);
        }

    }

    public void OnDrag (PointerEventData eventData){
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag (PointerEventData eventData){
        Debug.Log("OnEndDrag");
        item = null;
        if (transform.parent.GetComponent<SlotHandler>() != null){
            Debug.Log("Transforming Position");
            StopAllCoroutines();
            StartCoroutine(SlerpToPositionCoroutine(transform.parent.position));
            //transform.position = Vector3.Slerp(transform.position, transform.parent.position, slerpSpeed);
        }
        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

    private IEnumerator SlerpToPositionCoroutine(Vector3 targetPosition){
        while(Vector2.Distance(transform.position, targetPosition) > minRangeToTransform) {
			transform.position = Vector3.Slerp(transform.position, targetPosition, slerpSpeed);

			Vector3 fixedZ = transform.position;
			fixedZ.z = initialZ;
			transform.position = fixedZ;
			yield return new WaitForEndOfFrame();
		}
    }

    public Image GetImage(){
        return GetComponent<Image>();
    }
}
