using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneMotion : MonoBehaviour {

	private float pos;
	private float ang;

	private float leftBoundary,rightBoundary,topBoundary,bottomBoundary;

	// Use this for initialization
	void Start () {
		ang = 0f;
		pos = transform.position.x;

		Vector3 MaxCamera = Camera.main.ViewportToWorldPoint (new Vector3 (1, 1, 0));
		Vector3 MinCamera = Camera.main.ViewportToWorldPoint (new Vector3 (0, 0, 0));

		leftBoundary = MinCamera.x;
		rightBoundary = MaxCamera.x;
		bottomBoundary = MinCamera.y;
		topBoundary = MaxCamera.y;
	}
	
	// Update is called once per frame
	void Update () {

		if (transform.position.x > rightBoundary + 10f || transform.position.x < leftBoundary - 10f)
			Destroy (gameObject);

		if (pos < leftBoundary) {
			transform.position = new Vector3 (transform.position.x + 0.1f, transform.position.y + Mathf.Sin (ang)*0.1f, transform.position.z);
		} else {
			transform.position = new Vector3 (transform.position.x - 0.1f, transform.position.y + Mathf.Sin (ang)*0.1f, transform.position.z);
		}
		ang+=0.1f;
	}
}
