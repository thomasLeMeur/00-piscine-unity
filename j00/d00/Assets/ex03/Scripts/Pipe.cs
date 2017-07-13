using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pipe : MonoBehaviour {
	static private bool stop = false;
	private float speed = 0.0f;
	private int nbFrames = 0;
	private const int nbFramesBeforeUpdate = 10;

	// Update is called once per frame
	void Update () {
		if (stop == false) {
			nbFrames++;
			transform.position -= new Vector3 (0.02f + speed, 0f, 0f);
			if (transform.position.x + 1.2 < -4f)
				transform.position = new Vector3 (4.5f, 0.3f, 0f);
			if (nbFrames >= nbFramesBeforeUpdate) {
				nbFrames = 0;
				speed += 0.001f;
			}
		}
	}

	static public void finish()
	{
		stop = true;
	}
}
