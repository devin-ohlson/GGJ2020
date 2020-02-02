using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
	private Camera mainCamera;

	[SerializeField] private float minRangeToTransform;
	[SerializeField] private float lerpSpeed;
	[SerializeField] private float zoomFactor = 1.0f;
	[SerializeField] private float zoomSpeed = 5.0f;

	private float initialZ;
	private float initialZoom;

	public bool CurrentlyZooming = false;

	void Start() {
		mainCamera = GetComponent<Camera>();
		initialZ = transform.position.z;
		initialZoom = mainCamera.orthographicSize;
	}

	public void LerpToPosition(Vector2 target) {
		StopAllCoroutines();
		StartCoroutine(LerpToPositionCoroutine(target));
	}
	
	private IEnumerator LerpToPositionCoroutine(Vector2 targetPosition) {
		while(Vector2.Distance(transform.position, targetPosition) > minRangeToTransform) {
			transform.position = Vector2.Lerp(transform.position, targetPosition, lerpSpeed);

			Vector3 fixedZ = transform.position;
			fixedZ.z = initialZ;
			transform.position = fixedZ;
			yield return new WaitForEndOfFrame();
		}
	}

	public void Zoom(float zoomFactor) {
		StartCoroutine(ZoomCoroutine(initialZoom * zoomFactor));
	}

	private IEnumerator ZoomCoroutine(float zoom) {
		CurrentlyZooming = true;
		while (Mathf.Abs(zoom - mainCamera.orthographicSize) > 0.05f) {
			mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, zoom, Time.deltaTime * zoomSpeed);
			yield return new WaitForEndOfFrame();
		}
		CurrentlyZooming = false;
	}
}