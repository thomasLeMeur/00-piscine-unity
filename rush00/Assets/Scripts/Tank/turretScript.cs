using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class turretScript : MonoBehaviour {

	private float speed = 0.26f;
	private bool isStarted = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (isStarted == false)
			transform.Rotate (0, 0, speed * 1);
	}

	public void stopRotation () {
		isStarted = true;
	}
}
