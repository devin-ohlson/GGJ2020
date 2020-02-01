using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayer : MonoBehaviour {
	public float speed;
	private Rigidbody2D rb;

	private void Start() {
		rb = GetComponent<Rigidbody2D>();
	}

	private void FixedUpdate() {
		Vector3 newTF = transform.position;
		newTF.x += Input.GetAxis("Horizontal") * speed;
		newTF.y += Input.GetAxis("Vertical") * speed;
		rb.MovePosition(newTF);
	}
}