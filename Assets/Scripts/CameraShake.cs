using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour {

	public Transform camTransform;
	public static float shakeDuration = 0f;
	public float shakeAmount = 0.0001f;
	public float decreaseFactor = 1.0f;

	Vector3 origPos;

	void Awake(){
		if (camTransform == null) {
			camTransform = GetComponent (typeof(Transform)) as Transform;
		}
	}

	void onEnable(){
		origPos = new Vector3 (0f, 1f, -10f);
	}

	void Update(){
		if (shakeDuration > 0f) {
			camTransform.localPosition = origPos + Random.insideUnitSphere * shakeAmount;
			shakeDuration -= Time.deltaTime * decreaseFactor;
		} else {
			shakeDuration = 0f;
			camTransform.position = new Vector3 (0f, 1f, -10f);
		}
	}
		
}
