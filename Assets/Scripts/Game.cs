using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour {

	public GameObject gamePanel, pausePanel, joyStickPanel, quitConfirmPanel, aircraftPanel;
	public Button music, sound, AX, AY;

	void Awake()
	{
		Time.timeScale = 1f;
	}

	void Start () {
		if (PreLoader.Instance.startGame == true) {
			aircraftPanel.SetActive (false);
		} else {
			Time.timeScale = 0f;
		}
		PreLoader.Instance.startGame = true;
		MenuSound.Instance.sond ();
		if (PreLoader.Instance.soundState) {
			sound.GetComponent<Image> ().sprite = PreLoader.Instance.SoundOn;
		} else {
			sound.GetComponent<Image> ().sprite = PreLoader.Instance.SoundOff;
		}
		if (PreLoader.Instance.dpadState)
			joyStickPanel.SetActive (true);
		else
			joyStickPanel.SetActive (false);
	}
	
	void Update () {
		if (Input.GetKeyDown (KeyCode.Escape)) {
			if (pausePanel.activeInHierarchy) {
				quitConfirm ();
			} else if (quitConfirmPanel.activeInHierarchy||aircraftPanel.activeInHierarchy) {
				quit ();
			}
			else {
				pause ();
			}
		}
	}

	public void pause()
	{
		FX.Instance.play();
		Time.timeScale = 0f;	
		pausePanel.SetActive (true);
		gamePanel.SetActive (false);
	}

	public void unpause()
	{
		Time.timeScale = 1f;
		FX.Instance.play();
		pausePanel.SetActive (false);
		gamePanel.SetActive (true);
	}

	public void quitConfirm()
	{
		FX.Instance.play();
		gamePanel.SetActive (false);
		pausePanel.SetActive (false);
		quitConfirmPanel.SetActive (true);
	}

	public void quitDenied()
	{
		FX.Instance.play();
		gamePanel.SetActive (true);
		pausePanel.SetActive (false);
		quitConfirmPanel.SetActive (false);
	}

	public void quit()
	{
		FX.Instance.play();
		quitConfirmPanel.SetActive (false);
		SceneManager.LoadScene ("Main Screen");
	}

	public void musicChanger()
	{
		FX.Instance.play();
	//	print ("here1");
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
		FX.Instance.play();
		if (PreLoader.Instance.soundState) {
			sound.GetComponent<Image> ().sprite = PreLoader.Instance.SoundOff;
			PreLoader.Instance.soundState=false;
		} else {
			sound.GetComponent<Image> ().sprite = PreLoader.Instance.SoundOn;
			PreLoader.Instance.soundState=true;
		}
		PreLoader.Instance.Save ();
	}

	public void AXx()
	{
		FX.Instance.play();
		PreLoader.Instance.aircraftType = 0;
		aircraftPanel.SetActive (false);
		planeController.instance.gameObject.transform.GetChild (0).gameObject.GetComponent<SpriteRenderer> ().sprite= PreLoader.Instance.shipX;
		Time.timeScale = 1;
	}

	public void AYy()
	{
		FX.Instance.play();
		PreLoader.Instance.aircraftType = 1;
		aircraftPanel.SetActive (false);
		planeController.instance.gameObject.transform.GetChild (0).gameObject.GetComponent<SpriteRenderer> ().sprite = PreLoader.Instance.shipY;
		Time.timeScale = 1;
	}
}
