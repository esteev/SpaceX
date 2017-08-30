using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainDestruction : MonoBehaviour {

	private SpriteRenderer sr;
	private float widthWorld, heightWorld;
	private int widthPixel, heightPixel;
	private Color transp;
	public static TerrainDestruction instance;

	// Use this for initialization
	void Start () {
		instance = this;
		sr = GetComponent <SpriteRenderer> ();
		// sr: Global GroundController variable, ref for the Ground SpriteRenderer
		//Texture2D tex = (Texture2D) Resources.Load ("ground2");
		Texture2D tex = (Texture2D)Resources.Load ("ground1");
		switch(GameManager.value){

		case 0:
			tex = (Texture2D)Resources.Load ("ground1");
			break;
		case 1:
			tex = (Texture2D)Resources.Load ("ground2");
			break;
		case 2:
			tex = (Texture2D)Resources.Load ("ground3");
			break;
		case 3:
			tex = (Texture2D)Resources.Load ("ground4");
			break;
		case 4:
			tex = (Texture2D)Resources.Load ("ground5");
			break;
		
		}
		Debug.Log (GameManager.value + "    Terrain type Selection");
		//sr.sprite = GameManager.instance.Texture_sprite[GameManager.value];			
		//Texture2D tex = (Texture2D) Resources.Load (GameManager.instance.Terrain[GameManager.instance.value]);
		// Resources.Load ("filename") loads a file located
		// in Assets / Resources
		Texture2D tex_clone = (Texture2D) Instantiate (tex);
		// We created a Texture2D clone of tex so we did not change the original image
		sr.sprite = Sprite.Create (tex_clone,
			new Rect (0f, 0f, tex_clone.width, tex_clone.height),
			new Vector2 (0.5f, 0.5f), 100f);
		Destroy (GetComponent <PolygonCollider2D> ());
		gameObject.AddComponent <PolygonCollider2D>();
		transp = new Color (0f, 0f, 0f, 0f);
		initSpriteDimensions ();



	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void initSpriteDimensions () {
		widthWorld = sr.bounds.size.x;
		heightWorld = sr.bounds.size.y;
		widthPixel = sr.sprite.texture.width;
		heightPixel = sr.sprite.texture.height;
		Debug.Log (gameObject.name + " " + widthPixel + " " + widthWorld);
	}
		public void MyDestroyGround(float posX,float posY){
		/*Vector2 target = new Vector2 (posX, posY);
		Vector3 pc = cam.WorldToScreenPoint (target);
		Vector3 pixelCenter = new Vector3 (pc.x, pc.y, 0f);*/
		V2int c = World2Pixel (posX, posY);
		int r = (int)(widthPixel / widthWorld);

		Debug.Log (c.x + "  " + c.y + " " + r);
		//make alpha zero
		int x, y, px, nx, py, ny, d;

		for (x = 0; x <= r; x ++)
		{
			d = (int) Mathf.RoundToInt (Mathf.Sqrt ((r * r) - (x * x)));

			for (y = 0; y <= 3*d; y ++)
			{
				px = (int)c.x + x;
				nx = (int)c.x - x;
				py = (int)c.y + y;
				ny = (int)c.y - y;

				//Debug.Log (px + " " + py + " " + nx + " " + ny);

				sr.sprite.texture.SetPixel (px, py, transp);
				sr.sprite.texture.SetPixel (nx, py, transp);
				sr.sprite.texture.SetPixel (px, ny, transp);
				sr.sprite.texture.SetPixel (nx, ny, transp);
			}
		}
		sr.sprite.texture.Apply ();
		Destroy (GetComponent <PolygonCollider2D> ());
		gameObject.AddComponent <PolygonCollider2D>();
	}

	public void DestroyGround (CircleCollider2D cc) {
		Debug.Log ("called DestroyGround");

		V2int c = World2Pixel (cc.bounds.center.x, cc.bounds.center.y);
		// c => center of the circle of destruction in pixels
		int r = Mathf.RoundToInt (cc.bounds.size.x * widthPixel / widthWorld);
		// r => radius of the circle of destruction in

		Debug.Log (c.x + " " + c.y + " " + r);

		int x, y, px, nx, py, ny, d;

		for (x = 0; x <= r; x ++)
		{
			d = (int) Mathf.RoundToInt (Mathf.Sqrt ((r * r) - (x * x)));

			for (y = 0; y <= d; y ++)
			{
				px = c.x + x;
				nx = c.x-x;
				py = c.y + y;
				ny = c.y - y;

				sr.sprite.texture.SetPixel (px, py, transp);
				sr.sprite.texture.SetPixel (nx, py, transp);
				sr.sprite.texture.SetPixel (px, ny, transp);
				sr.sprite.texture.SetPixel (nx, ny, transp);
			}
		}
		sr.sprite.texture.Apply ();
		Destroy (GetComponent <PolygonCollider2D> ());
		gameObject.AddComponent <PolygonCollider2D>();
	}

	private V2int World2Pixel (float x, float y) {
		V2int v = new V2int ();

		float dx = x-transform.position.x;
		v.x = Mathf.RoundToInt (0.5f * widthPixel + (dx * (widthPixel / widthWorld)));

		float dy = y - transform.position.y;
		v.y = Mathf.RoundToInt (0.5f * heightPixel + (dy * (heightPixel / heightWorld)));

		return v;
	}
}
