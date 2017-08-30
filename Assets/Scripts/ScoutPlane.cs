using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoutPlane : MonoBehaviour {

	public float speed;

	// Use this for initialization
	void Start () {
		if (transform.position.x < -50f) {
			GetComponent<Rigidbody2D> ().velocity = Vector2.right * speed;
		} else{
			GetComponent<Rigidbody2D> ().velocity = -Vector2.right * speed;
		}
	}
	
	// Update is called once per frame
	void Update () {

	}
}
