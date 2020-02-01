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
        if (Input.GetAxisRaw("Vertical") != 0)
        {
            StairCtrl stair = collider.GetComponent<StairCtrl>();
            if (stair != null)
            {
                stair.Travel(transform, Input.GetAxisRaw("Vertical") > 0);
            }
        }
        if (Input.GetMouseButtonDown(0)){
            Interactable interactable = collider.GetComponent<Interactable>();
            if (interactable != null){
                interactable.Interact(this);
            }
        }
    }
}
