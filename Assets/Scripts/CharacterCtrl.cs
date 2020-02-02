using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCtrl : MonoBehaviour, MovementFreezable
{
    private Rigidbody2D rb;
	private SpriteRenderer spriteRenderer;
    [SerializeField] private float walkSpeed = 5;

	public float wobTimeMod;
	public float wobSizeMod;
	private float wobRotationTimer;

	private bool frozen = false;

	void Start()
    {
        rb = GetComponent<Rigidbody2D>();
		spriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    void FixedUpdate()
    {
		if (!frozen) {
			float movement = (Input.GetAxis("Horizontal") != 0) ? Input.GetAxis("Horizontal") * walkSpeed : 0;

			if (Input.GetAxis("Horizontal") > 0) {
				movement = walkSpeed;
				spriteRenderer.flipX = true;
			}
			else if (Input.GetAxis("Horizontal") < 0) {
				movement = -walkSpeed;
				spriteRenderer.flipX = false;
			}
			rb.velocity = Vector2.up * rb.velocity + Vector2.right * movement;

			if (movement != 0) {
				float rotation = 0;
				wobRotationTimer += Time.deltaTime * wobTimeMod;
				rotation = Mathf.Sin(wobRotationTimer) * wobSizeMod;
				transform.Rotate(new Vector3(0, 0, rotation));
			}
			else {
				transform.rotation = new Quaternion();
			}
		}
	}

	public void FreezeMovement(bool freeze) {
		frozen = freeze;
	}

    private void OnTriggerStay2D(Collider2D collider)
    {
        Interactable interactable = collider.GetComponent<Interactable>();
        if (interactable != null)
        {
            interactable.TryInteract(this);
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        Interactable interactable = collider.GetComponent<Interactable>();
        if (interactable != null)
        {
            interactable.Reset();
        }
    }
}
