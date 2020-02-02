using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCtrl : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector3 roomPosition;
    private bool cameraAnchored = false;
    public GameObject anchor;
    public CameraZoom cameraZoom;
	[SerializeField] private float minRangeToTransform;
	[SerializeField] private float lerpSpeed;
	private float initialZ;
    private CameraController mainCam;
    [SerializeField]
    float zoomFactor = 1.0f;

    [SerializeField]
    float zoomSpeed = 5.0f;
    private float originalSize = 0f;


    [SerializeField] private float walkSpeed = 5;
    
    void Start()
    {
        Debug.Log(Camera.main.fieldOfView);
        rb = GetComponent<Rigidbody2D>();
        mainCam = Camera.main.gameObject.GetComponent<CameraController>();
        cameraZoom = Camera.main.gameObject.GetComponent<CameraZoom>();
    }
    
    void Update(){
        if(Input.GetKeyDown(KeyCode.Q)){
            if(cameraAnchored){
                Debug.Log("Moving to camera anchor");
                mainCam.LerpToPosition(roomPosition);
                cameraZoom.SetZoom(1);
                cameraAnchored = false;
            }else{
                Debug.Log("Moving to camera to character");
                roomPosition = mainCam.gameObject.transform.position;
                mainCam.LerpToPosition(anchor.transform.position);
                cameraZoom.SetZoom(3);
                cameraAnchored = true;
            }
        }
    }

    public void LerpToPosition(Vector3 target, int fov) {
		StopAllCoroutines();
		StartCoroutine(LerpToPositionCoroutine(target, fov));
	}
	
	private IEnumerator LerpToPositionCoroutine(Vector3 targetPosition, int fov) {
		while(Vector3.Distance(transform.position, targetPosition) > minRangeToTransform) {
			transform.position = Vector3.Lerp(transform.position, targetPosition, lerpSpeed);

            Camera.main.fieldOfView = fov;

			Vector3 fixedZ = transform.position;
			fixedZ.z = initialZ;
			transform.position = fixedZ;
			yield return new WaitForEndOfFrame();
		}
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
