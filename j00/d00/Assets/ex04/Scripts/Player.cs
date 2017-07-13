using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	private KeyCode keyU;
	private KeyCode keyD;

	// Use this for initialization
	void Start () {
		if (transform.position.x < 0) {
			keyU = KeyCode.W;
			keyD = KeyCode.S;
		} else {
			keyU = KeyCode.UpArrow;
			keyD = KeyCode.DownArrow;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey (keyU))
			transform.position = new Vector3 (transform.position.x, (transform.position.y + 0.05f > 3.75f) ? 3.75f : transform.position.y + 0.05f , 0);
		else if (Input.GetKey (keyD))
			transform.position = new Vector3 (transform.position.x, (transform.position.y - 0.05f < -3.75f) ? -3.75f : transform.position.y - 0.05f , 0);
	}
}
