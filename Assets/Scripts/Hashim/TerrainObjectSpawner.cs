using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TerrainObjectSpawner : MonoBehaviour {


	TerrainDetails building1, building2,terrain1,terrain2,commonDesert,woodland1,arctic1,desert1;
	public GameObject buildingObj1,buildingObj2,commonDesertObj;
	public GameObject woodlandObj1,arcticObj1,desertObj1;

	static private float leftBoundary,rightBoundary,topBoundary,bottomBoundary;
	public static Transform thisTransform;
	private int TerrainType;
	//------------------------------------------------------------------------------------------------------
	class TerrainDetails{


		public string name;
		int maxLimit;
		public int levelLimit;
		GameObject enemyObject;	//enemyGameObject

		public TerrainDetails(string name ,GameObject enemyObject,int maxLimit){

			this.name=name;
			this.maxLimit=maxLimit;
			this.enemyObject=enemyObject;				
			levelLimit=calculateLevelLimit();
			putObject();
		}
		int calculateLevelLimit(){
			int limit = maxLimit;
			return Random.Range(0,limit);
		}
		public void putObject(){
			for (int i = 0; i < levelLimit; i++) {
				var temp = Instantiate (enemyObject, Vector3.zero, Quaternion.identity);
				temp.transform.parent = thisTransform;
				spawnObject (temp);
			}

		}


		public void spawnObject(GameObject TerrainObj){
			Vector3 outsideGround;
			Vector3 orientation;
			bool loop = true;
			int counter = 0;
			RaycastHit2D hit,hitLeft,hitRight;
			while(loop){
				float TempX  =	Random.Range(leftBoundary+TerrainObj.GetComponent<BoxCollider2D> ().size.x / 2,
								rightBoundary-TerrainObj.GetComponent<BoxCollider2D> ().size.x/ 2);

				hit = Physics2D.Raycast (new Vector2 (TempX, 0), Vector2.down);
				hitLeft=Physics2D.Raycast (new Vector2 (TempX-TerrainObj.GetComponent<BoxCollider2D> ().size.x / 2, 0), Vector2.down);
				hitRight=Physics2D.Raycast (new Vector2 (TempX+TerrainObj.GetComponent<BoxCollider2D> ().size.x / 2, 0), Vector2.down);

				if (hit.collider.gameObject.CompareTag ("Ground") &&
				   hitLeft.collider.gameObject.CompareTag ("Ground") &&
					hitRight.collider.gameObject.CompareTag ("Ground")) {  //dont know
					outsideGround = new Vector3 (TempX, -hit.distance + TerrainObj.GetComponent<BoxCollider2D> ().size.y / 2, 0);
					if (outsideGround != Vector3.zero) { 
						loop = false;
					}
				} else {
					counter++;
				}

				if (counter > 4) {
					Destroy (TerrainObj);
					return;
					loop = false;
				}

			}

			TerrainObj.transform.position = outsideGround;
			//TerrainObj.transform.rotation = Quaternion.Euler (orientation);			
		}

	}
	//----------------------------------------------------------------------------------------------------------------------------------

	// Use this for initialization

	void Start () {
		thisTransform = this.transform;
		Vector3 MaxCamera = Camera.main.ViewportToWorldPoint (new Vector3 (1, 1, 0));
		Vector3 MinCamera = Camera.main.ViewportToWorldPoint (new Vector3 (0, 0, 0));
		leftBoundary = MinCamera.x;
		rightBoundary = MaxCamera.x;
		bottomBoundary = MinCamera.y;
		topBoundary = MaxCamera.y;
		
		building1 = new TerrainDetails ("building1", buildingObj1, 4);
		building2 = new TerrainDetails ("building2", buildingObj2, 4);
		commonDesert = new TerrainDetails ("commonTree", commonDesertObj, 3);

		///insert terrain wise here
		/// 

		switch(GameManager.value){
		case 0:
			arctic1=new TerrainDetails("arctic",arcticObj1,3);
			break;
		case 1://woodland
			woodland1=new TerrainDetails("woodland",woodlandObj1,3);	
			break;
		case 2://desert
			desert1=new TerrainDetails("desert",desertObj1,3);
			break;

		case 3://arctic
			arctic1=new TerrainDetails("arctic",arcticObj1,3);
			break;
		case 4:
			woodland1=new TerrainDetails("woodland",woodlandObj1,3);	
			break;
		default:
			print ("No terrain is selected");
			break;
		}
	}



}
