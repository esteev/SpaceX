using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tankMovementController : MonoBehaviour {

	public float speed;
	public GameObject[] enemy;
	private float weight;
	private Rigidbody2D rb;
	private Vector3 targetNormal;
	Quaternion fromRotation,toRotation;

	private float leftBoundary,rightBoundary,topBoundary,bottomBoundary;


	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D> ();
		targetNormal = transform.up;
		for (int i = 0; i < enemy.Length; i++) {
			Physics2D.IgnoreCollision (enemy[i].gameObject.GetComponent<Collider2D> (), GetComponent<Collider2D> ());
		}
		weight = 1f;

		Vector3 MaxCamera = Camera.main.ViewportToWorldPoint (new Vector3 (1, 1, 0));
		Vector3 MinCamera = Camera.main.ViewportToWorldPoint (new Vector3 (0, 0, 0));

		leftBoundary = MinCamera.x;
		rightBoundary = MaxCamera.x;
		bottomBoundary = MinCamera.y;
		topBoundary = MaxCamera.y;
	}
	
	// Update is called once per frame
	void Update () {

		if (transform.position.x < leftBoundary) {
			setSpeedPos ();
			transform.rotation = Quaternion.Euler (0, 0, 0);
		} else if (transform.position.x > rightBoundary) {
			setSpeedNeg ();
			transform.rotation = Quaternion.Euler (0, 180, 0);
		}

		RaycastHit2D hit = Physics2D.Raycast ((Vector2)transform.position, -Vector2.up,1.0f);
		Debug.DrawLine (transform.position, -Vector3.up,Color.black);
		if (hit.collider != null) {
			Quaternion.FromToRotation (transform.up, hit.normal);
			transform.position = new Vector3 (transform.position.x, transform.position.y - hit.distance, transform.position.z);

			Vector3 myRotation = transform.rotation.eulerAngles;
			if (myRotation.z > 270f) {
				myRotation.z = Mathf.Clamp (myRotation.z, 350f, 360f);
			} else if (myRotation.z < 90f) {
				myRotation.z = Mathf.Clamp (myRotation.z, 0f, 10f);
			} 
			transform.rotation = Quaternion.Euler (myRotation);
		}
			

		rb.velocity = new Vector2 (speed, rb.velocity.y);

		/*Vector3 myRotation = transform.rotation.eulerAngles;
		if (myRotation.z > 270f) {
			myRotation.z = Mathf.Clamp (myRotation.z, 270f, 360f);
		} else if (myRotation.z < 90f) {
			myRotation.z = Mathf.Clamp (myRotation.z, 0f, 90f);
		} else {
			myRotation.z = Mathf.Clamp (myRotation.z, 0f, 30f);

		}
		//myRotation.z = Mathf.Clamp (myRotation.z , -90f, 90f);
		transform.rotation = Quaternion.Euler (myRotation);*/
	}

	void setSpeedNeg(){
		switch (gameObject.name) {

		case "eTGGround":
			speed = -1f;
			break;
		case "eTRGround":
			speed = -1.5f;
			break;
		case "eTSGround":
			speed = -0.8f;
			break;
		case "eAATGround":
			speed = -0.5f;
			break;
		default:
			speed = -1f;
			break;
	

		}
	}

	void setSpeedPos(){
		switch (gameObject.name) {

		case "eTGGround":
			speed = 1f;
			break;
		case "eTRGround":
			speed = 1.5f;
			break;
		case "eTSGround":
			speed = 0.8f;
			break;
		case "eAATGround":
			speed = 0.5f;
			break;
		default:
			speed = 1f;
			break;

		}
	}

	void OnCollisionEnter2D(Collision2D coll){

		if (coll.collider.CompareTag ("enemy")) {
			Physics2D.IgnoreCollision (coll.collider, GetComponent<Collider2D> ());
		}

	}
		
}
