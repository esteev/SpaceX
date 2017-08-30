using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FX : MonoBehaviour {

	private static FX instance;
	public static FX Instance{get{ return instance; }}

	public AudioClip cl;
	AudioSource asource;

	void Start () {
		instance = this;
		DontDestroyOnLoad (gameObject);

		asource = GetComponent<AudioSource>();

		if (PreLoader.Instance.soundState)
			this.gameObject.GetComponent<AudioSource> ().enabled = true;
		else
			this.gameObject.GetComponent<AudioSource> ().enabled = false;
	}

	public void play(AudioClip cl)
	{
		asource.PlayOneShot (cl);
	}

	public void play()
	{
		asource.PlayOneShot (cl);
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
