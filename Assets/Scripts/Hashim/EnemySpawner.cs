using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {
	
	private const int ENEMY_MAX_LIMIT = 1000;		//
	private const float EMPTY_ENEMY_DURATION_MULTIPLIER=2f;

	enemyDetails eSTAM,eTR,eTS,eTG,eAAT,eAD,eADST,eASP,eAVP;
	enemyDetails[] e=new enemyDetails[9];

	static private int levelNo;
	static private float emptyEnemyDuration;
	static public GameObject enemySpawner;
	static public bool eADCanSpawn;
	static public bool eADSTCanSpawn;//	have to change by ourselves
	static private float leftBoundary,rightBoundary,topBoundary,bottomBoundary;

	public GameObject enemyTank;
	public GameObject eSTAMObj, eTRObj, eTSObj, eTGObj, eAATObj, eADObj, eADSTObj, eASPObj, eAVPObj;

	GameObject[] eSTAMArray,eTRArray,eTSArray,eTGArray,eAATArray,eADArray,eADSTArray,eASPArray,eAVPArray;
	GameObject[][] eArray= new GameObject[9][];


	private float startTime;

	//------------------------------------------------------------------------------------------------------
	class enemyDetails{
		float MIN_DURATION=1f;
		float INCREASE_DURATION=3f;
		public string name;
		int minLevel;
		int maxLimit;
		public int levelLimit;
		float perLevelIncrease;
		bool spawnArea;			//true for ground and false for air
		public List<float> spawnTime;
		GameObject enemyObject;	//enemyGameObject

		public enemyDetails(string name ,bool spawnArea,GameObject enemyObject,int minLevel,float perLevelIncrease,int maxLimit=ENEMY_MAX_LIMIT){
			
			this.name=name;
			this.minLevel=minLevel;
			this.perLevelIncrease=perLevelIncrease;
			this.maxLimit=maxLimit;
			this.spawnArea=spawnArea;
			this.enemyObject=enemyObject;				
			if(minLevel<=levelNo){
				levelLimit=calculateLevelLimit();
			}else{
				levelLimit=0;
			}

			this.spawnTime=new List<float>();
			spawnTimeGenerator();
		}
		int calculateLevelLimit(){
			int limit = (int)(1 + (levelNo - minLevel) * perLevelIncrease);
			limit = limit > maxLimit ? maxLimit : limit;
			return limit;
		}

		void spawnTimeGenerator(){
			for (int i = 0; i < levelLimit; i++) {
				spawnTime.Add (Random.Range (0.0f, emptyEnemyDuration));
				
			}

			spawnTimeSorting ();
			for (int i = levelLimit - 1; i > 0; i--) {
				if (spawnTime [i - 1] - spawnTime [i] < MIN_DURATION||
					spawnTime [i - 1] < spawnTime [i]) {
					spawnTime [i - 1] += Random.Range (MIN_DURATION,INCREASE_DURATION );
				}
			}

			for (int i = 0; i < levelLimit; i++) {
				//print (name+" "+spawnTime [i]);
			}

		}

		public float currentSpawnEnemyTime(){
			if (spawnTime.Count > 0) {
				return spawnTime [spawnTime.Count - 1];
			} else {
				return -1f;						//no enemies for given type
			}
		}


		public void dispatchCurrentEnemy(GameObject enemy){
			//
			//print("Enemy "+name+" at "+currentSpawnEnemyTime()+" is dispatched");
			if (name != "eSTAM" && name != "eAD" && name != "eADST") {
				spawnEnemy (enemy);
				
			} else {
				if (name == "eAD" && eADCanSpawn || name == "eADST" && eADSTCanSpawn) {
					spawnEnemy (enemy);

				} else {
					spawnTime [spawnTime.Count - 1] += 1f;
					spawnTimeSorting ();
				}

				if(name=="eSTAM") {
					spawnTime.Remove(currentSpawnEnemyTime());
					if (enemy.transform.GetChild(0)!=null) {
						enemy.transform.GetChild (0).GetComponent<eSTAM> ().enabled = true;
					}
				}
			}

		}

		public GameObject[] CreateEnemy(){
			GameObject[] tempEnemyArray = new GameObject[levelLimit];
			for (int i = 0; i < levelLimit; i++) {


				var tempEnemy = Instantiate (enemyObject, Vector3.zero, Quaternion.identity);
				tempEnemy.SetActive (false);
				tempEnemy.name=spawnArea?name+"Ground":name+"Air";
				tempEnemy.transform.parent = enemySpawner.transform;
				tempEnemyArray[i]=tempEnemy;
				//print (name + " " + i + " is created ");

				if (name == "eSTAM") {
					spawnEnemy (tempEnemy, false);
					//tempEnemy.SetActive (true);
				}
			}
			return tempEnemyArray;
		}


		public void spawnEnemy(GameObject enemy,bool removeTime=true){
			if (removeTime) {
				spawnTime.Remove (currentSpawnEnemyTime ());
			}
			if (spawnArea) {
				
				Vector3 outsideGround;
				Vector3 orientation;
				RaycastHit2D hit;

				if (Random.Range (0, 2) % 2 == 0) {
					outsideGround=new Vector3(leftBoundary-1,0,0);
					orientation =Vector3.zero;
				} 
				else {
					outsideGround=new Vector3(rightBoundary+1,0,0);
					orientation = new Vector3 (0, 180, 0);
				}
				if (name == "eSTAM") {
					bool loop = true;
					while(loop){
						float TempX  =	Random.Range(leftBoundary,rightBoundary);
						hit = Physics2D.Raycast (new Vector2 (TempX, 0), Vector2.down);

						if(hit.collider.gameObject.CompareTag("Ground")){
							outsideGround = new Vector3 (TempX,- hit.distance + enemy.GetComponent<BoxCollider2D> ().size.y / 2, 0);
							loop = false;
						}

					}
				}
				enemy.transform.position=outsideGround;
				enemy.transform.rotation = Quaternion.Euler(orientation);			
				//Instantiate (enemyObject, outsideGround, Quaternion.Euler(orientation));

			}else{
				Vector3 outsideAir;
				Vector3 orientation;
				float height = Random.Range (0f, topBoundary - 1f);
				if (Random.Range (0, 2) % 2 == 0) {
					outsideAir=new Vector3(leftBoundary-1,height,0);
					orientation =Vector3.zero;
				} 
				else {
					outsideAir=new Vector3(rightBoundary+1,height,0);
					orientation = new Vector3 (0, 180, 0);
				}
				enemy.transform.position=outsideAir;
				enemy.transform.rotation = Quaternion.Euler(orientation);			
			}

			enemy.SetActive(true);

		}

		private void spawnTimeSorting(){				//sorting in decreasing order
			spawnTime.Sort ();
			spawnTime.Reverse ();
		}
	}
	//----------------------------------------------------------------------------------------------------------------------------------

	// Use this for initialization

	void Start () {
		startTime = Time.time ;
		enemySpawner = this.gameObject;
		Vector3 MaxCamera = Camera.main.ViewportToWorldPoint (new Vector3 (1, 1, 0));
		Vector3 MinCamera = Camera.main.ViewportToWorldPoint (new Vector3 (0, 0, 0));

	
		leftBoundary = MinCamera.x;
		rightBoundary = MaxCamera.x;
		bottomBoundary = MinCamera.y;
		topBoundary = MaxCamera.y;

		eADCanSpawn = true;
		eADSTCanSpawn = true;
		levelNo = PreLoader.Instance.levelNo;


		emptyEnemyDuration = levelNo * EMPTY_ENEMY_DURATION_MULTIPLIER;

		e[0]=eSTAM = new enemyDetails ("eSTAM",true,eSTAMObj,3,0.5f, 8);
		e[1]=eTR = new enemyDetails ("eTR",true,eTRObj,1,0.8f);
		e[2]=eTS = new enemyDetails ("eTS",true,eTSObj,2,0.6f, 3);
		e[3]=eTG = new enemyDetails ("eTG",true,eTGObj,5,0.3f, 3);
		e[4]=eAAT = new enemyDetails ("eAAT",true,eAATObj,7,0.3f, 2);
		e[5]=eAD = new enemyDetails ("eAD",false,eADObj,5, 0.4f,4);//1 in a scene
		e[6]=eADST=new enemyDetails ("eADST",false,eADSTObj,15,0.3f,3);//1 in a scene
		e[7]=eASP = new enemyDetails ("eASP",false,eASPObj,8,0.3f);
		e[8]=eAVP = new enemyDetails ("eAVP",false,eAVPObj,12,0.3f);

		eArray[0] = eSTAMArray = eSTAM.CreateEnemy ();
		eArray[1] = eTRArray = eTR.CreateEnemy ();
		eArray[2] = eTSArray = eTS.CreateEnemy ();
		eArray[3] = eTGArray = eTG.CreateEnemy ();
		eArray[4] = eAATArray = eAAT.CreateEnemy ();
		eArray[5] = eADArray = eAD.CreateEnemy ();
		eArray[6] = eADSTArray = eADST.CreateEnemy ();
		eArray[7] = eASPArray = eASP.CreateEnemy ();			//ganda error
		eArray[8] = eAVPArray = eAVP.CreateEnemy ();

		InvokeRepeating ("spawnCheck", 1f, 1f);
	}

	// Update is called once per frame
	void Update () {
		//print (e [0].currentSpawnEnemyTime ());
		for (int i = 0; i < e.Length; i++) {
			if (e [i].currentSpawnEnemyTime() != -1f&&Time.time - startTime >= e [i].currentSpawnEnemyTime()) {
				if (eArray [i] [(e [i].spawnTime.Count - 1)] != null) {
					e [i].dispatchCurrentEnemy (eArray [i] [(e [i].spawnTime.Count - 1)]); //pta nhi chalega
				}
			}
		}
	}

	void spawnCheck(){
		eADCanSpawn = eADSTCanSpawn = true;

		foreach(Transform go in enemySpawner.transform.GetComponentsInChildren<Transform> ()){
			if (go.name == "eADAir")
				eADCanSpawn = false;
			else if (go.name == "eADSTAir")
				eADSTCanSpawn = false;
		}
	//	print ("eADCanSpawn" + eADCanSpawn);
	//	print ("eADSTCanSpawn" + eADSTCanSpawn);
	}

}
