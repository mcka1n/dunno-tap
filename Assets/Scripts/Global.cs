﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Global : MonoBehaviour {

	private static int initialTime = 10;

	public static int score;
	private static int time = initialTime;
	private static int goal = 40;
	private static int level = 1;
	private static bool boxTaken = false;
	private static float rotatingTime = 0.000000005f;

	private static UnityEngine.UI.Text timeLabel;
	private static UnityEngine.UI.Text goalLabel;
	private static UnityEngine.UI.Text levelLabel;
	private static UnityEngine.UI.Text scoreLabel;
	private static UnityEngine.UI.Text gameOver;
	private static UnityEngine.UI.Button restart;

	private static Vector3 v0 = Vector3.zero;
	private static Vector3 imageVisible = new Vector3(4.5f,4.5f,4.5f);

	// Use this for initialization
	void Start () {
		Screen.orientation = ScreenOrientation.LandscapeRight;
		timeLabel = GameObject.Find("Time").GetComponent<Text>();
		goalLabel = GameObject.Find("Goal").GetComponent<Text>();
		levelLabel = GameObject.Find("Level").GetComponent<Text>();
		gameOver = GameObject.Find("GameOver").GetComponent<Text>();
		restart = GameObject.Find ("Restart").GetComponent<Button> ();
		scoreLabel = GameObject.Find("Score").GetComponent<Text>();

		levelLabel.text = "Level: " + level;
		goalLabel.text = "Goal: " + goal;
		hideGameOverObjects ();
	}
	
	// Update is called once per frame
	void Update () {

		if (time == 0 && score < goal) {
			//GameOver
			showGameOverObjects();
		}

		scoreLabel.text = "Score: " + score.ToString();
		if (!boxTaken && time > 0) {
			StartCoroutine (takeRandomBox ());
		}
		if (time > 0) {
			time = initialTime - (int)Time.timeSinceLevelLoad;
			timeLabel.text = "Time: " + time.ToString ();
			if (time < 4) {
				timeLabel.SendMessage ("ChangeColor");
			}
		} 
		//Check if we can level up or game over
		if (score >= goal && time == 0) {
			changeLevel ();
		}
	}

	void hideGameOverObjects() {
		gameOver.transform.localScale = v0;
		restart.transform.localScale = v0;
	}

	void showGameOverObjects() {
		Vector3 newPosition = new Vector3(2.0f, 2.0f, 2.0f);
		gameOver.transform.localScale = newPosition;
		restart.transform.localScale = newPosition;
	}

	IEnumerator takeRandomBox()
	{
		boxTaken = true;
		GameObject[] arrayofboxes = GameObject.FindGameObjectsWithTag("box");
		int boxNumber = (int)Random.Range (0.0f, 9.0f);
		GameObject boxup = arrayofboxes[boxNumber];
		string boxupName = boxup.gameObject.name;
		float counter = 0f;

		//0
		GameObject addPointSelected = GameObject.Find ("goldie" + boxupName);
		//1
		GameObject substPointSelected = GameObject.Find ("hammer" + boxupName);

		int iconVisible = (int)Random.Range (0.4f, 1.4f);

		while (counter < 13.5f) {
			boxup.gameObject.transform.transform.Rotate (Vector3.up * counter);
			yield return  new WaitForSeconds (rotatingTime);
			counter = counter + 1f;
		}

		if (iconVisible == 0) {
			showButton (addPointSelected, substPointSelected);
			addPointSelected.SendMessage ("updateValue", 15);
		} else {
			showButton (substPointSelected, addPointSelected);
			substPointSelected.SendMessage ("updateValue", -17);
		}

		yield return  new WaitForSeconds (0.5f);

		if (iconVisible == 0) {
			hideButton (addPointSelected);
		} else {
			hideButton (substPointSelected);
		}

		counter = 0;
		while (counter < 13.5f) {
			boxup.gameObject.transform.transform.Rotate (Vector3.down * counter);
			yield return  new WaitForSeconds (rotatingTime);
			counter = counter + 1f;
		}
		boxTaken = false;
	}

	public void restartScene(){
		goal = 40;
		level = 1;
		score = 0;
		rotatingTime = 0.000000005f;
		Scene scene = SceneManager.GetActiveScene(); 
		SceneManager.LoadScene(scene.name);

		hideGameOverObjects ();
		scoreLabel.text = "Score: " + score;
	}

	void changeLevel() {
		//Reset time
		time = initialTime;
		rotatingTime = rotatingTime / 100;
		//Calculate next goal;
		goal = (int) (goal + goal*0.5);
		goalLabel.text = "Goal: " + goal;

		//uplevel
		level++;
		levelLabel.text = "Level: " + level;

		Scene scene = SceneManager.GetActiveScene(); 
		SceneManager.LoadScene(scene.name);
	}

	protected void Awake(){
		time = initialTime;
		boxTaken = false;
	}

	void showButton(GameObject shownButton, GameObject unshownButton) {
		Vector3 visiblePosition = new Vector3 (0.0f, 0.0f, -0.5f);
		shownButton.transform.localScale = imageVisible;
		shownButton.transform.localPosition = visiblePosition;
		shownButton.gameObject.GetComponent<Button> ().interactable = true;
		unshownButton.gameObject.GetComponent<Button> ().interactable = false;
	}

	void hideButton(GameObject shownButton) {
		shownButton.transform.localPosition = v0;
		shownButton.transform.localScale = v0;
		shownButton.gameObject.GetComponent<Button> ().interactable = false;
		shownButton.SendMessage ("resetValue");
	}
}
