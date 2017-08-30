using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class powerUp : MonoBehaviour {

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {

		transform.Translate (Vector3.up * Time.deltaTime * 1);
	}
	void OnTriggerEnter2D(Collider2D other){
		if (other.gameObject.CompareTag ("Player")) {
			planeController.instance.powerUpCaught (name);

			Destroy (this.gameObject);
		}
	}	
}
