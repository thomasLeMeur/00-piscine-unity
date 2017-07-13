using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour {
	public GameObject pipe;
	private Pipe p;

	private int score = 0;
	private float speed = 0.0f;
	private bool inPipe = false;
	private int nbFrames = 0;
	private bool finish = false;

	// Update is called once per frame
	void Update () {
		if (finish == false)
		{
			nbFrames++;
			if (pipe.transform.position.x - 1.2 <= transform.position.x && pipe.transform.position.x + 0.8 >= transform.position.x)
				inPipe = true;
			if (Input.GetKeyDown("space"))
				speed = 0.2f;
			speed -= 0.01f;
			float y = transform.position.y + speed;
			if (y >= 4.8)
				y = 4.8f;
			if (y < -3.8 || (pipe.transform.position.x - 1.2 <= transform.position.x && pipe.transform.position.x + 0.8 >= transform.position.x
				&& (y <= -1.6f || y >= 1.8f)))
			{
				finish = true;
				Pipe.finish();
				Debug.Log ("Score: " + score);
				Debug.Log("Time: " + Mathf.RoundToInt(nbFrames / 60f) + "s");
			}
			else
			{
				transform.position = new Vector3(transform.position.x, y, 0f);
				if (inPipe && pipe.transform.position.x + 0.8 < transform.position.x)
				{
					inPipe = false;
					score += 5;
				}
			}
		}
	}
}
