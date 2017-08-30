using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GO : MonoBehaviour {

	void Start () {
		MenuSound.Instance.sond ();
		PreLoader.Instance.startGame = false;
	}
	
	void Update () {
		if (Input.GetKeyDown (KeyCode.Escape)) {
			back ();
		}
	}

	public void restart()
	{
		FX.Instance.play();
		SceneManager.LoadScene ("Game");
	}

	public void back()
	{
		FX.Instance.play();
		SceneManager.LoadScene ("Main Screen");
	}

	public void LeaderBoard()
	{
		FX.Instance.play();
	}
}
