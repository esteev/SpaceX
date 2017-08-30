using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class planeController : MonoBehaviour {
	private const float BULLET_THROW_FORCE = 5f;
	private const float BOMB_THROW_FORCE = 2f;
	private const float CLOSE_TOUCH_DISTANCE = 1f;
	private const float BOMB_X_FORCE_MULTIPLIER = 2f;
	private const float DURATION_BETWEEN_BULLETS = 0.1f;
	private const int NO_OF_BULLETS = 3;

	public static planeController instance;

	public float speed=5;
	public float deltaAngle=5f;
	public float angle;

	public int AmmoTotal=1;
	public int AmmoCount=1;
	public int MoabTotal = 0;
	public int MoabCount = 0;

	public Text ammoLeft, moabLeft;
	public Joystick joystick;

	public GameObject missile,bullet,bulletChota,moab;
	struct fireDetails{
		public float fireTime;
		public float fireDuration;
		public fireDetails(float dur){
			fireTime=Time.time;
			fireDuration=dur;
		}
	};

	fireDetails primaryFD,secondaryFD,moabFD;

	private float leftBoundary,rightBoundary,topBoundary,bottomBoundary;
	private BoxCollider2D planeCollider;

	private bool rotatePlane;
	Vector3 newTouch;

	private float justBounce;
	private float bounceBetweenDuration=0.1f;

	void Start () {
		instance = this;
		Vector3 MaxCamera = Camera.main.ViewportToWorldPoint (new Vector3 (1, 1, 0));
		Vector3 MinCamera = Camera.main.ViewportToWorldPoint (new Vector3 (0, 0, 0));
		if (PreLoader.Instance.levelNo == 1) {
			PreLoader.Instance.ammoTotal = 1;
			PreLoader.Instance.moabTotal = 0;
		} else {
			AmmoTotal = PreLoader.Instance.ammoTotal;
			MoabTotal = PreLoader.Instance.moabTotal;
			AmmoCount = PreLoader.Instance.ammoTotal;
			MoabCount = PreLoader.Instance.moabTotal;
		}
		primaryFD = new fireDetails(0.2f) ;
		secondaryFD = new fireDetails(0.5f);
		moabFD = new fireDetails(0.2f);

		leftBoundary = MinCamera.x;
		rightBoundary = MaxCamera.x;
		bottomBoundary = MinCamera.y;
		topBoundary = MaxCamera.y;

		print ("l " + leftBoundary);
		print ("r " + rightBoundary);
		print ("u " + topBoundary);
		print ("d " + bottomBoundary);

		planeCollider = transform.GetChild (0).GetComponent<BoxCollider2D> ();

	}

	// Update is called once per frame
	void Update () {
		ammoLeft.text = "Ammo " + AmmoCount;
		moabLeft.text = "MOAB " + MoabCount;
		if (PreLoader.Instance.dpadState) {
			JoystickControl ();
		} else {
			MouseControl ();
			TouchControl ();
		}
	CommonControl ();


		//angle += deltaAngle;
		transform.GetChild(0).rotation = Quaternion.Euler (new Vector3 (0, 0, angle+90));

		Vector3 toMove = new Vector3 (Mathf.Cos (Mathf.Deg2Rad * angle), Mathf.Sin (Mathf.Deg2Rad * angle), 0) * Time.deltaTime*speed;
		transform.position += toMove;

		PlayerRestriction ();


	}

	public void FirePrimary(){
		print (AmmoCount);
		if (Time.time - primaryFD.fireTime > primaryFD.fireDuration) {
			if (PreLoader.Instance.aircraftType==0) {
				if (AmmoCount > 0) {
					FireMissiles ();
					AmmoCount--;
				}
			} else {
				if (AmmoCount  > 0) {
					StartCoroutine (FireGun ());
					AmmoCount -= 3;
				}
			}
			primaryFD.fireTime = Time.time;
		}
	}
	public void FireSecondary(){
		if (Time.time - secondaryFD.fireTime > secondaryFD.fireDuration) {
				Firing (bulletChota, 90, BULLET_THROW_FORCE);
				secondaryFD.fireTime = Time.time;

		}

	}

	public void FireMOAB(){
		
		if (Time.time - moabFD.fireTime > moabFD.fireDuration && MoabCount>0) {
			Firing (moab, 90, BULLET_THROW_FORCE, 1/BOMB_X_FORCE_MULTIPLIER);
			moabFD.fireTime = Time.time;
			MoabCount--;
			MoabTotal--;
		}
	}

	private void FireMissiles(){
			Firing (missile, 90, BOMB_THROW_FORCE,BOMB_X_FORCE_MULTIPLIER);
			
	}
	IEnumerator FireGun(){
		for (int i = 0; i < NO_OF_BULLETS; i++) {

			Firing (bullet,90, BULLET_THROW_FORCE);

			yield return new WaitForSeconds(DURATION_BETWEEN_BULLETS);
		}
	}


	private void Firing(GameObject ammo,float orirntationAngle,float forwardForce,float horizontalMultiplier=1){
		var ammoObj=Instantiate (ammo, transform.position,Quaternion.Euler(0,0,angle-orirntationAngle));
		ammoObj.GetComponent<Rigidbody2D> ().AddForce (
			new Vector2 (
				Mathf.Cos (Mathf.Deg2Rad * angle)*horizontalMultiplier,
				Mathf.Sin (Mathf.Deg2Rad * angle)
			) * forwardForce,
			ForceMode2D.Impulse
		);

	}

	void PlayerRestriction(){
		if (newTouch.x < leftBoundary ||
			newTouch.x > rightBoundary ||
			newTouch.y < bottomBoundary ||			//touch point is out of boundary then dont rotate
			newTouch.y > topBoundary) {
			rotatePlane = false;
		}



		if (transform.position.y >= topBoundary-planeCollider.size.y/2) {		//bounce on top
			angle = 360f-angle;
			justBounce = Time.time;
		}

		if (transform.position.x < leftBoundary-planeCollider.size.x/2) {
			transform.position = new Vector3 (rightBoundary, transform.position.y, 0);
		}else if (transform.position.x > rightBoundary+planeCollider.size.x/2) {
			transform.position = new Vector3 (leftBoundary, transform.position.y, 0);
		}
	}

	void TouchControl(){
		if (Input.touchCount > 0) {
			// The screen has been touched so store the touch
			Touch touch = Input.GetTouch (0);

			if (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved) {
				// If the finger is on the screen, move the object smoothly to the touch position
				newTouch=new Vector3(touch.position.x,touch.position.y,0);
				newTouch = Camera.main.ScreenToWorldPoint (newTouch);
				newTouch = new Vector3 (newTouch.x, newTouch.y, 0);
				rotatePlane = true;
			}
			if (EventSystem.current.IsPointerOverGameObject (touch.fingerId)) {
				rotatePlane = false;
				return;
			}

		}


		/*if (Input.touchCount > 1) {
//			FireBomb ();
		}*/

	}

	void MouseControl(){
		if (Input.GetButton ("Fire1")) {
			newTouch=new Vector3(Input.mousePosition.x,Input.mousePosition.y,0);
			newTouch = Camera.main.ScreenToWorldPoint (newTouch);
			newTouch = new Vector3 (newTouch.x, newTouch.y, 0);
			rotatePlane = true;

		}


		if (Input.GetButton ("Fire2")) {
//			FireBomb ();

		}
	}
	void JoystickControl(){

		if (Mathf.Abs(joystick.Horizontal ()) > 0.05 || Mathf.Abs(joystick.Vertical ()) > 0.05) {

			newTouch = new Vector3 (joystick.Horizontal () + transform.position.x, joystick.Vertical () + transform.position.y, 0);
			rotatePlane = true;
		} 
	}

	void CommonControl(){
		if (rotatePlane&&Time.time-justBounce>bounceBetweenDuration) {
//			print (newTouch + " " + transform.position);;
			Vector3 targetDir = newTouch - transform.position;
			float newAngle = Angle360 (Vector3.left, targetDir, Vector3.forward);

			newAngle -= angle;		//reference to angle
			newAngle = (int)newAngle;
			float deltaAngle = 2f;
			if (newAngle > 0+deltaAngle && newAngle <= 180f || newAngle <= -180f) {
				angle += deltaAngle;	
			} else if (newAngle >=180f || newAngle < 0-deltaAngle && newAngle >= -180f) {
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


	public void powerUpCaught(string name){
		print (name);
		if (name == "PU_missileInc") {
			AmmoTotal++;
			AmmoCount++;
			PreLoader.Instance.ammoTotal = AmmoTotal;
		} else if (name == "PU_moabInc") {
			MoabTotal++;
			MoabCount++;
			PreLoader.Instance.moabTotal = MoabTotal;
		}
	}
	void OnTriggerEnter2D(Collider2D col){
		print ("oye");
		if (col.tag == "enemy" || col.tag == "Ground" || col.tag == "building" || col.tag == "terrain") {
			//kill player
			GameManager.instance.GameOver();
			Destroy (this.gameObject);
		}
	}
	void OnCollisionEnter2D(Collision2D coll){
		print ("oye");
		if (coll.gameObject.tag == "enemy" || coll.gameObject.tag == "Ground" ||
			coll.gameObject.tag == "building" || coll.gameObject.tag == "terrain" || coll.gameObject.tag == "enemyAmmo") {
			//kill player
			GameManager.instance.GameOver();
			if (coll.gameObject.tag != "terrain" || coll.gameObject.tag == "enemyAmmo") {
				Destroy (coll.gameObject);
			}
			Destroy (this.gameObject);
		}
	}
}

