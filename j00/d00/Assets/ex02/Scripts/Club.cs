using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Club : MonoBehaviour {
	private Ball ball;

	static private bool wi = false;
	private int vel = 0;
	static private int score = -15;
	static private float y = -2f;
	static private bool todo = false;

	// Update is called once per frame
	void Update () {
		if (wi == false) {
			if (todo == true) {
				transform.position = new Vector3 (-0.15f, y, 0f);
				todo = false;
			}
			if (Input.GetKey (KeyCode.Space)) {
				vel++;
				transform.position -= new Vector3 (0f, 0.01f, 0f);
			} else if (vel != 0) {
				Ball.getMove (vel);
				vel = 0;
				transform.position = new Vector3 (-0.15f, y, 0f);
			}
		}
	}

	static public void win()
	{
		if (wi == false)
			Debug.Log ("Score: " + score);
		wi = true;
	}

	static public void loose(float posy)
	{
		y = posy - 0.15f;
		todo = true;
		if (score < 0)
			score += 5;
		if (wi == false)
			Debug.Log ("Score: " + score);
	}
}
