using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class eSTAM: MonoBehaviour {
	
	public float speed=4.9f;
	public float deltaAngle=2f;
	public float angle;


	private float leftBoundary,rightBoundary,topBoundary,bottomBoundary;
	private BoxCollider2D planeCollider;

	private bool rotatePlane;
	Vector3 newTouch;
	private GameObject player;
	private float justBounce;
	private float bounceBetweenDuration=0.1f;
	private LineRenderer lr;
	private Transform neighbour;
	private bool baseDestroyed;
	void Start () {
		neighbour = this.transform.parent.GetChild (1);
		Vector3 MaxCamera = Camera.main.ViewportToWorldPoint (new Vector3 (1, 1, 0));
		Vector3 MinCamera = Camera.main.ViewportToWorldPoint (new Vector3 (0, 0, 0));
		lr = neighbour.GetComponent<LineRenderer>();
	
		leftBoundary = MinCamera.x;
		rightBoundary = MaxCamera.x;
		bottomBoundary = MinCamera.y;
		topBoundary = MaxCamera.y;

		print ("l " + leftBoundary);
		print ("r " + rightBoundary);
		print ("u " + topBoundary);
		print ("d " + bottomBoundary);
		player = GameObject.FindGameObjectWithTag ("Player");
		angle = 90;
	}

	// Update is called once per frame
	void Update () {

		newTouch = player.transform.position;
//		print (newTouch);
		if (!baseDestroyed) {
			CommonControl ();
			drawMyLine(transform.parent.position, transform.position,Color.red,0.0f);
		}

		//angle += deltaAngle;

		transform.rotation = Quaternion.Euler (new Vector3 (0, 0, angle-90));

		Vector3 toMove = new Vector3 (Mathf.Cos (Mathf.Deg2Rad * angle), Mathf.Sin (Mathf.Deg2Rad * angle), 0) * Time.deltaTime*speed;
		transform.position += toMove;
		rotatePlane = true;
		///LineRenderer lR = new LineRenderer ();

		//Debug.DrawLine (transform.parent.position, transform.position,Color.red);
	}



	void CommonControl(){
		if (rotatePlane&&Time.time-justBounce>bounceBetweenDuration) {
			Vector3 targetDir = newTouch - transform.position;
			float newAngle = Angle360 (Vector3.left, targetDir, Vector3.forward);
		//	print (newAngle);
			newAngle -= angle;		//reference to angle
			newAngle = (int)newAngle;

			if (newAngle > 0 && newAngle <= 180 || newAngle <= -180f) {
				angle += deltaAngle;	
			} else if (newAngle > 180 || newAngle < 0 && newAngle > -180f) {
				angle -= deltaAngle;
			} else {
				rotatePlane = false;
			}
			if (Vector3.Distance (newTouch, transform.position) < 1f) {
				rotatePlane = false;
			}
			//angle = angle < 0 ? angle + 360f : angle;
			angle += 360f;
			angle %= 360f;
		}
	}

	float Angle360(Vector3 v1, Vector3 v2, Vector3 n)
	{
		//  Acute angle [0,180]
		float angle = Vector3.Angle(v1,v2);

		//  -Acute angle [180,-179]
		float sign = Mathf.Sign(Vector3.Dot(n, Vector3.Cross(v1, v2)));
		float signed_angle = angle * sign;

		//  360 angle
		return (signed_angle + 180) % 360;
	}
	void drawMyLine(Vector3 start , Vector3 end, Color color,float duration = 0.2f){

		StartCoroutine( drawLine(start, end, color,duration));

	}


	IEnumerator drawLine(Vector3 start , Vector3 end, Color color,float duration = 0.2f){
		GameObject myLine = new GameObject ();
		//myLine.transform.position = start;
		neighbour.position=start;
		//myLine.AddComponent<LineRenderer> ();
		//LineRenderer lr = myLine.GetComponent<LineRenderer> ();
		//lr.material //= new Material (Shader.Find ("Particles/Additive"));
		lr.SetColors (color,color);
		lr.SetWidth (0.05f,0.05f);
		lr.SetPosition (0, start);
		lr.SetPosition (1, end);
		yield return new WaitForSeconds(duration);
		GameObject.Destroy (myLine);
	}
	public void isDead(){
		baseDestroyed = true;
	}
}

