using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StairCtrl : MonoBehaviour
{
    [SerializeField] private StairCtrl up = null;
    [SerializeField] private StairCtrl down = null;
    
    void Start()
    {
        if ((null != up && this != up.down) || (null != down && this != down.up))
            throw new ArgumentException(this.name + " has mismatched stairs.");
    }

    public void Travel(Transform obj, bool isGoingUp)
    {
        StairCtrl destination = (isGoingUp) ? up : down;

        if (destination != null)
        {
            Vector3 diff = obj.position - transform.position;

            obj.position = diff + destination.transform.position;
        }
    }
}
