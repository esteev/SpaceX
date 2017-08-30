using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoBehaviour : MonoBehaviour {

	public GameObject explosion;
	public GameObject explosionMoab;

	private float leftBoundary,rightBoundary,topBoundary,bottomBoundary;
	private bool scriptActive;

	void Start () {
		Vector3 MaxCamera = Camera.main.ViewportToWorldPoint (new Vector3 (1, 1, 0));
		Vector3 MinCamera = Camera.main.ViewportToWorldPoint (new Vector3 (0, 0, 0));
		scriptActive = true;

		leftBoundary = MinCamera.x;
		rightBoundary = MaxCamera.x;
		bottomBoundary = MinCamera.y;
		topBoundary = MaxCamera.y;

	}

	// Update is called once per frame
	void Update () {
		if (transform.position.y < bottomBoundary||transform.position.y>topBoundary||
			transform.position.x < leftBoundary||transform.position.x>rightBoundary	) {
			DestroyAmmo();
		}

	}
	void OnTriggerEnter2D(Collider2D col){
		if (scriptActive) {
			
			if (col.tag == "Player" && tag=="enemyAmmo") {
				//kill player
				Destroy (col.gameObject);
				DestroyAmmo ();
				GameManager.instance.GameOver();

			}
			if (col.tag == "Ground") {
				if (gameObject.name.Contains ("Missile") || gameObject.name.Contains ("MOAB")) {

					if (gameObject.name.Contains ("Missile")) {
						GameManager.instance.destroyNearByEnemies (col.transform.position, 1f);
					} else {
						GameManager.instance.destroyNearByEnemies (col.transform.position, 4f);
					}

				}
				if (gameObject.name.Contains ("MOAB")) {
					CameraShake.shakeDuration = 1f;
					Destroy (Instantiate (explosionMoab, gameObject.transform.position, Quaternion.identity), 1f);
				}
				TerrainDestruction.instance.MyDestroyGround (transform.position.x, transform.position.y);
				DestroyAmmo ();
			} else if ((col.tag == "enemy"||col.tag == "building")&& gameObject.tag!="enemyAmmo") {
				
				if (tag=="playerAmmo") {
					GameManager.ScoreIncrease ();
				}
				if (col.tag == "building") {
					GameManager.instance.createPowerUp (col.transform.position);
				
				}
				if (col.gameObject.name=="eSTAMGround") {
					var temp = col.transform.GetChild (0);
					temp.parent = null;
					if (!temp.GetComponent<eSTAM> ().enabled) {
						Destroy (temp.gameObject);
					} else {
						temp.GetComponent<eSTAM> ().isDead ();
					}
				}
				if (gameObject.name.Contains ("MOAB")) {
					GameManager.instance.destroyNearByEnemies (col.transform.position, 4f);
				}
				if (gameObject.name.Contains ("MOAB")) {
					CameraShake.shakeDuration = 1f;
					Destroy (Instantiate (explosionMoab, gameObject.transform.position, Quaternion.identity), 1f);
					FX.Instance.play (PreLoader.Instance.moabExp);
				} else {
					Destroy (Instantiate (explosion, gameObject.transform.position, Quaternion.identity), 0.5f);
					if(this.name.Contains("ssile"))
						FX.Instance.play (PreLoader.Instance.missExp);
					else if(this.name.Contains("llet"))
						FX.Instance.play (PreLoader.Instance.bullExp);
				}
				Destroy (col.gameObject);
				DestroyAmmo ();
			}
		}

	}
	void OnCollisionEnter2D(Collision2D coll){
		//when the bullet collides with anotherbody other than the Player
		if (scriptActive) {
			
			if (coll.collider.tag == "Player" && tag=="enemyAmmo") {
				//kill player
				GameManager.instance.GameOver();
				Destroy (Instantiate (explosion, coll.gameObject.transform.position, Quaternion.identity),0.5f);
				Destroy (coll.gameObject);
			}
			if (coll.collider.tag == "Ground") {
				if (gameObject.name.Contains ("Missile") || gameObject.name.Contains ("MOAB")) {
					
					if (gameObject.name.Contains ("Missile")) {
						GameManager.instance.destroyNearByEnemies (coll.contacts [0].point, 1f);
					} else {
						GameManager.instance.destroyNearByEnemies (coll.contacts [0].point, 4f);
					}

				}
				if (gameObject.name.Contains ("MOAB")) {
					CameraShake.shakeDuration = 1f;
					Destroy (Instantiate (explosionMoab, gameObject.transform.position, Quaternion.identity), 1f);
					FX.Instance.play (PreLoader.Instance.moabExp);
				}

				TerrainDestruction.instance.MyDestroyGround (transform.position.x, transform.position.y);
				if(this.name.Contains("ssile"))
					FX.Instance.play (PreLoader.Instance.missExp);
				else if(this.name.Contains("llet"))
					FX.Instance.play (PreLoader.Instance.bullExp);
				DestroyAmmo ();
			} else if ((coll.collider.tag == "enemy"||coll.collider.tag == "building") && gameObject.tag!="enemyAmmo") {

				if (tag=="playerAmmo") {
					GameManager.ScoreIncrease ();
				}
				if (coll.gameObject.name=="eSTAMGround") {
					var temp = coll.transform.GetChild (0);
					temp.parent = null;
					if (!temp.GetComponent<eSTAM> ().enabled) {
						Destroy (temp.gameObject);
					} else {
						temp.GetComponent<eSTAM> ().isDead ();
					}
				}
				if (gameObject.name.Contains ("MOAB")) {
					CameraShake.shakeDuration = 1f;
					Destroy (Instantiate (explosionMoab, gameObject.transform.position, Quaternion.identity), 1f);
					FX.Instance.play (PreLoader.Instance.moabExp);
				} else {
					Destroy (Instantiate (explosion, coll.gameObject.transform.position, Quaternion.identity), 0.5f);
					if(this.name.Contains("ssile"))
						FX.Instance.play (PreLoader.Instance.missExp);
					else if(this.name.Contains("llet"))
						FX.Instance.play (PreLoader.Instance.bullExp);
				}
				Destroy (coll.gameObject);
				DestroyAmmo ();
			} 
		}
	}
	void DestroyAmmo(){
		if (this.name.Contains("P_Missile")|| this.name.Contains("P_Bullet")) {
			planeController.instance.AmmoCount++;
		}
		if (transform.parent!=null&&transform.parent.name=="eSTAMGround") {
			Destroy (transform.parent.gameObject);
		} else {
			Destroy (gameObject);
		}

	}

	IEnumerator shake(){
		int t = 1000;
		Quaternion camData = Camera.main.transform.rotation;
		while (t != 0) {
			Camera.main.transform.rotation = Quaternion.Slerp (Camera.main.transform.rotation, Random.rotation,
				0.1f * Time.deltaTime);
			t--;
			yield return new WaitForSeconds(0.1f);
		}
		Camera.main.transform.rotation = camData;
	}
		
}
