using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreLoader : MonoBehaviour {

	private static PreLoader instance;
	public static PreLoader Instance{get{ return instance; }}

	public Sprite MusicOn, MusicOff, SoundOn, SoundOff, PadOn, PadOff,shipX,shipY;
	public AudioClip moabExp, bullExp, missExp, fail;

	public int highScore=0;
	public int currentScore=0;
	public bool musicState=true;
	public bool soundState=true;
	public bool dpadState=false;
	public int aircraftType;			//0 for X 1 for Y
	public bool playdone=false;
	public bool startGame = false;
	//by hash
	public int levelNo = 1;
	public int ammoTotal = 1;
	public int moabTotal = 0;
	//
	void Start()
	{
		instance = this;
		DontDestroyOnLoad (gameObject);
		if (PlayerPrefs.HasKey ("highScore")) {
			highScore = PlayerPrefs.GetInt ("highScore");
			currentScore = PlayerPrefs.GetInt ("currentScore");
			musicState = (PlayerPrefs.GetInt ("musicState")==1)?true:false;
			soundState = (PlayerPrefs.GetInt ("soundState")==1)?true:false;
			dpadState = (PlayerPrefs.GetInt ("dpadState")==1)?true:false;
			
		} else {
			Save ();
		}
	}

	public void Save()
	{	PlayerPrefs.SetInt ("levelNo", levelNo);
		PlayerPrefs.SetInt ("highScore",highScore);
		PlayerPrefs.SetInt ("currentScore",currentScore);
		PlayerPrefs.SetInt ("musicState",(musicState)?1:0);
		PlayerPrefs.SetInt ("soundState",(soundState)?1:0);
		PlayerPrefs.SetInt ("dpadState",(dpadState)?1:0);
	}
}
