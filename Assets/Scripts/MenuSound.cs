using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuSound : MonoBehaviour {

	private static MenuSound instance;
	public static MenuSound Instance{get{ return instance; }}

	public AudioSource asource;
	public AudioClip menu, game;

	void Start () {
		instance = this;
		DontDestroyOnLoad (gameObject);

		asource = GetComponent<AudioSource> ();

		if (PreLoader.Instance.musicState)
			asource.enabled = true;
		else
			asource.enabled = false;
	}

	public void sond()
	{
		if (SceneManager.GetActiveScene ().name == "Game")
			asource.clip = game;
		else
			asource.clip = menu;
	}

	public void play()
	{
		asource.Play ();
	}

	public void switcher()
	{
	//	print ("here3");
		if(PreLoader.Instance.musicState)
			asource.enabled = true;
		else
			asource.enabled = false;
	//	print ("here4");
	}
}
