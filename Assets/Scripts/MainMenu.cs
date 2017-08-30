using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

	public Button music, sound, dpad, exit, start;
	public GameObject quitPanel;

	void Start () {
		PreLoader.Instance.startGame = false;
		MenuSound.Instance.sond ();
		if (PreLoader.Instance.musicState) {
			music.GetComponent<Image> ().sprite = PreLoader.Instance.MusicOn;
		} else {
			music.GetComponent<Image> ().sprite = PreLoader.Instance.MusicOff;
		}
		if (PreLoader.Instance.soundState) {
			sound.GetComponent<Image> ().sprite = PreLoader.Instance.SoundOn;
		} else {
			sound.GetComponent<Image> ().sprite = PreLoader.Instance.SoundOff;
		}
		if (PreLoader.Instance.dpadState) {
			dpad.GetComponent<Image> ().sprite = PreLoader.Instance.PadOn;
		} else {
			dpad.GetComponent<Image> ().sprite = PreLoader.Instance.PadOff;
		}
	}
	

	void Update () {

		if (Input.GetKeyDown (KeyCode.Escape)) {
			if (quitPanel.activeInHierarchy)
				exitKarde ();
			else
				confirmExit ();
		}
	}

	public void musicChanger()
	{
	//	print ("here1");
		FX.Instance.play();
		if (PreLoader.Instance.musicState) {
			music.GetComponent<Image> ().sprite = PreLoader.Instance.MusicOff;
			PreLoader.Instance.musicState = false;
		} else {
			music.GetComponent<Image> ().sprite = PreLoader.Instance.MusicOn;
			PreLoader.Instance.musicState = true;
		}
		MenuSound.Instance.switcher ();
		PreLoader.Instance.Save ();
	//	print ("here2");
	}

	public void soundChanger()
	{
		if (PreLoader.Instance.soundState) {
			sound.GetComponent<Image> ().sprite = PreLoader.Instance.SoundOff;
			PreLoader.Instance.soundState=false;
		} else {
			sound.GetComponent<Image> ().sprite = PreLoader.Instance.SoundOn;
			PreLoader.Instance.soundState=true;
		}
		FX.Instance.switcher ();
		FX.Instance.play();
		PreLoader.Instance.Save ();
	}

	public void dpadChanger()
	{
		FX.Instance.play();
		if (PreLoader.Instance.dpadState) {
			dpad.GetComponent<Image> ().sprite = PreLoader.Instance.PadOff;
			PreLoader.Instance.dpadState = false;;
		} else {
			dpad.GetComponent<Image> ().sprite = PreLoader.Instance.PadOn;
			PreLoader.Instance.dpadState=true;
		}
		PreLoader.Instance.Save ();
	}

	public void confirmExit()
	{
		FX.Instance.play();
		quitPanel.SetActive (true);
	}

	public void quitDenied()
	{
		FX.Instance.play();
		quitPanel.SetActive (false);
	}

	public void exitKarde()
	{
		FX.Instance.play();
		Application.Quit ();
	}

	public void play()
	{
		FX.Instance.play();
		SceneManager.LoadScene ("Game");	
	}
}
