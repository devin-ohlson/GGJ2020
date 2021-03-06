﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JigsawPiece : MonoBehaviour
{
    private bool dragging = false;
    private float distance;

    void OnMouseDown(){
        distance = Vector3.Distance(transform.position, Camera.main.transform.position);
        Debug.Log(distance);
        dragging = true;
    }
    void OnMouseUp(){
        dragging = false;
    }

    void Update(){
        if (dragging){
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector3 rayPoint = ray.GetPoint(distance);
            transform.position = rayPoint;
        }
    }   
}
