using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
	private Camera mainCamera;

	[SerializeField] private float minRangeToTransform;
	[SerializeField] private float lerpSpeed;
	private float initialZ;

	void Start() {
		mainCamera = GetComponent<Camera>();
		initialZ = transform.position.z;
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
}