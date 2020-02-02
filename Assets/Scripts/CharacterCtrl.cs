using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCtrl : MonoBehaviour, MovementFreezable
{
	private Rigidbody2D rb;
	private Vector3 roomPosition;
	private SpriteRenderer spriteRenderer;
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

	public float wobTimeMod;
	public float wobSizeMod;
	private float wobRotationTimer;

	private bool frozen = false;

	void Start()
	{
		Debug.Log(Camera.main.fieldOfView);
		rb = GetComponent<Rigidbody2D>();
		mainCam = Camera.main.gameObject.GetComponent<CameraController>();
		spriteRenderer = GetComponent<SpriteRenderer>();
		cameraZoom = Camera.main.gameObject.GetComponent<CameraZoom>();
	}
	
	void Update(){
		if (!frozen) {
			if (Input.GetKeyDown(KeyCode.Q)) {
				if (cameraAnchored) {
					Debug.Log("Moving to camera anchor");
					mainCam.LerpToPosition(roomPosition);
					cameraZoom.SetZoom(1);
					cameraAnchored = false;
				}
				else {
					Debug.Log("Moving to camera to character");
					roomPosition = mainCam.gameObject.transform.position;
					mainCam.LerpToPosition(anchor.transform.position);
					cameraZoom.SetZoom(3);
					cameraAnchored = true;
				}
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
