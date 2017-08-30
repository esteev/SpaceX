using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuRoller : MonoBehaviour {

//	private const float timeToLerp = 100f; //2 seconds
	public const float width = 3000f;
	private const int no_of_screens = 3;
	private const int no_of_colors = 7;
	public const float zAhead = -150f;
	private const float intensity = 1f;
	private const float slower = 0.1f;
	public float rollSpeed=1.0f;
	private Renderer rend;
	private Vector3 oldy, newy,min,max;
	private float rate=0.001f;
	private Vector3[] colors;
	private int index=0,incy=1;
	private bool frontEndReached=true, backEndReached=false;
	//private float timeLerped = 0f;

	void Start()
	{
		rend = GetComponent<Renderer> ();
		colors = new Vector3[no_of_colors];
		colors[0]=new Vector3 (0f, 0f, 0f);
		colors[1]=new Vector3 (1f, 0f, 0f);
		colors[2]=new Vector3 (0f, 0f, 0f);
		colors[3]=new Vector3 (0f, 1f, 0f);
		colors[4]=new Vector3 (0f, 0f, 0f);
		colors[5]=new Vector3 (1f, 0f, 0f);
		colors[6]=new Vector3 (0f, 0f, 0f);
		oldy=min = colors[index];
		newy=max = colors[index+incy];
	//	print (colors[8]);
	}

	void Update()
	{
	//	timeLerped += Time.deltaTime;
		if(gameObject.CompareTag("Panel"))
			transform.position += new Vector3 (rollSpeed,0f,0f);

		if (transform.position.x > ((no_of_screens*width/2))) {
			transform.position = new Vector3 (-(no_of_screens*width/2),0,zAhead);
		}

		if (oldy == newy) {
			//timeLerped = 0;
			if (backEndReached && index == 1) {
				backEndReached = false;
				frontEndReached = true;
				index = 0;
				incy = 1;
			} else if (frontEndReached && index == no_of_colors - 2) {
				backEndReached = true;
				frontEndReached = false;
				index = no_of_colors - 1;
				incy = -1;
			} else if (frontEndReached) {
				index++;
			} else if (backEndReached) {
				index--;
			}
			//print (index);
			oldy = colors[index];
			newy = colors[index+incy];
		} else {
			oldy=(Vector3.MoveTowards (oldy, newy, rate));
			//oldy = Vector3.Lerp(oldy,newy,timeLerped/timeToLerp);
		}
		rend.material.SetColor ("_EmissionColor", new Color (oldy.x,oldy.y,oldy.z));
		DynamicGI.SetEmissive (rend, new Color (oldy.x,oldy.y,oldy.z));
	//	print (oldy);
	}
}
