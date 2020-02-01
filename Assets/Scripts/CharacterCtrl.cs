using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCtrl : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private float walkSpeed = 5;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    
    void FixedUpdate()
    {
        float movement = (Input.GetAxis("Horizontal") != 0) ? Input.GetAxis("Horizontal") * walkSpeed : 0;
        
        rb.velocity = Vector2.up * rb.velocity + Vector2.right * movement;
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
