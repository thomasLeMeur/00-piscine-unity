using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class menuManager : MonoBehaviour {

	public Button startButton;
	public Button exitButton;

	void Start()
	{
		Button start = startButton.GetComponent<Button>();
		start.onClick.AddListener(startAction);
		Button exit = exitButton.GetComponent<Button>();
		exit.onClick.AddListener(exitAction);
	}

	void startAction() {
//		Debug.Log ("to level 1");
		Application.LoadLevel (1);
	}

	void exitAction() {
//		Debug.Log ("quit");
		Application.Quit ();
	}
}
