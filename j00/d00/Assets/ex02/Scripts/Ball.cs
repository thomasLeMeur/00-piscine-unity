using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {
	private Club club;
	static public int vel = 0;
	private float sens = 0.003f;

	// Update is called once per frame
	void Update () {
		if (vel > 0) {
			if (transform.position.y >= 4.7f || transform.position.y <= -4.7f)
				sens *= -1;
			transform.position += new Vector3 (0.0f, sens * vel, 0.0f);
			float diff = transform.position.y - 3.0f;
			if (diff < 0)
				diff *= -1;
			if (vel < 10 && diff < 0.3f)
				Club.win ();
			vel--;
			if (vel <= 0.0f) {
				Club.loose (transform.position.y);
				sens = 0.003f;
			}
		}
	}

	static public void getMove(int speed){
		vel = speed;
	}
}
