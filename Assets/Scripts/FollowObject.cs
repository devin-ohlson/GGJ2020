﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObject : MonoBehaviour
{
	[SerializeField] public Transform toFollow = null;
	[SerializeField] private Vector3 diff = Vector3.up;

    void LateUpdate()
    {
		if (toFollow == null)
			GameObject.Destroy(this.gameObject);
		else
			transform.position = toFollow.position + diff;
    }
}
