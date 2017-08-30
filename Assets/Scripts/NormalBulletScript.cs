using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalBulletScript : MonoBehaviour {

	public GameObject bullet;
	private float prev;

	// Use this for initialization
	void Start () {

		prev = Time.time;
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D col){
		if(col.CompareTag("Player") && Time.time - prev > 0.3f){
			GameObject bull = Instantiate(bullet,transform.position,Quaternion.LookRotation(col.gameObject.transform.forward,-col.gameObject.transform.up));
			bull.SetActive (true);
			Vector3 dir = (transform.position - col.gameObject.transform.position);
			dir.Normalize ();
			bull.GetComponent<Rigidbody2D> ().velocity = (Vector2)dir*-3f;
			prev = Time.time;
		}
	}
		
}
