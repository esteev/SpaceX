using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class missileBehaviour : MonoBehaviour {
	private Rigidbody2D rb;
	public static TerrainDestruction groundController;
	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D> ();
	}

	// Update is called once per frame
	void Update () {
		float val = rb.velocity.normalized.x;
		float theta = Mathf.Acos (val);
		theta = Mathf.Rad2Deg * theta;

		transform.rotation = Quaternion.Euler(new Vector3(0,0,270-theta));

	}
}
