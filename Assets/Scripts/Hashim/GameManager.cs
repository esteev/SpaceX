using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	public static GameManager instance;
	public GameObject explosion;
	public GameObject ground;
	public Sprite[] Texture_sprite;
	public string[] Terrain = { "ground1", "ground2", "ground3", "ground4", "ground5" };
	public static int value;
	private static int score;
	public Text scoreText;

	public GameObject c1,c2;

	private int Lives;
	private int LevelNo=1;

	public static GameObject enemySpawner,terrainSpawner;
	public static Transform[] enemyList;

	public GameObject powerMissile,powerMOAB;

	void Start () {
		instance = this;
//		score = PreLoader.Instance.currentScore;
		LevelNo=PreLoader.Instance.levelNo;
		value = (int)Random.Range (0, 5);
		InvokeRepeating ("winCheck", 1f, 1f);
		enemySpawner = GameObject.Find ("EnemySpawner");
		terrainSpawner = GameObject.Find ("TerrainSpawner");
	}
	
	// Update is called once per frame
	void Update () {
		scoreText.text = "Score : " + score;
	}

	public void winCheck(){
		
		bool enemyDestroyed,buildingDestroyed=false;
		enemyDestroyed = enemySpawner.transform.childCount == 0;
		foreach (Transform t in terrainSpawner.transform.GetComponentsInChildren<Transform>()) {
			if (t.tag == "building") {
				buildingDestroyed = false;
				break;
			} else {
				buildingDestroyed = true;	
			}
		}
		if (enemyDestroyed && buildingDestroyed) {
			print ("Level is over");
			PreLoader.Instance.levelNo++;
			PreLoader.Instance.moabTotal = planeController.instance.MoabTotal;
			PreLoader.Instance.ammoTotal = planeController.instance.AmmoTotal;
			PreLoader.Instance.Save ();
			value = (int)Random.Range (0, 5);
			//ground.GetComponent<SpriteRenderer> ().sprite = Texture_sprite [value];
			Debug.Log ("done changing");
			SceneManager.LoadScene (SceneManager.GetActiveScene().name);
		}
	}

	static public void  ScoreIncrease(int inc=1){
		score += inc;

//		PreLoader.Instance.currentScore=score;
//		PreLoader.Instance.Save ();
		print ("current score is " + score);
	}

	public void destroyNearByEnemies(Vector2 colPoint,float damageRadius){
		enemyList = enemySpawner.transform.GetComponentsInChildren<Transform> ();
		Vector3 newPoint = new Vector3 (colPoint.x, colPoint.y, 0);
		foreach (Transform t in enemyList) {
			if (Vector3.Distance (newPoint, t.position) < damageRadius && t.tag == "enemy") {
				if (t.tag == "enemy") {
					if (t.name.Contains ("eSTAMGround")) {
						var temp = t.GetChild (0);
						temp.parent = null;
						print (temp.name);
						if (!temp.GetComponent<eSTAM> ().enabled) {
							Destroy (temp.gameObject);
						} else {
							temp.GetComponent<eSTAM> ().isDead ();
						}
					} 
				}
				Destroy (Instantiate (explosion, t.gameObject.transform.position, Quaternion.identity),0.5f);
				Destroy (t.gameObject);
				ScoreIncrease ();

			}
		}

		enemyList = terrainSpawner.transform.GetComponentsInChildren<Transform> ();
		newPoint = new Vector3 (colPoint.x, colPoint.y, 0);
		foreach (Transform t in enemyList) {
			if (Vector3.Distance (newPoint, t.position) < damageRadius) {
				if (t.tag == "building") {
					GameManager.instance.createPowerUp (t.position);
					ScoreIncrease ();
				}	
				Destroy (t.gameObject);
			}
		}
	}

	public void createPowerUp(Vector3 origin){
		if (Random.Range (0, 2) % 2 == 0) {
			var temp = Instantiate (powerMissile, origin, Quaternion.identity);
			temp.name = "PU_missileInc";
		} else {
			var temp = Instantiate (powerMOAB, origin, Quaternion.identity);
			temp.name = "PU_moabInc";
		}
	}

	public void GameOver(){
		PreLoader.Instance.levelNo = 1;
		c1.SetActive (false);
		c2.SetActive (true);
		FX.Instance.play (PreLoader.Instance.fail);
		Invoke ("go", 2f);
		//print ("Game is over ");
	}

	void go()
	{
		SceneManager.LoadScene ("GO");
	}
}
