using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour {
	private const int fps = 60;
	private int nbFrames = 0;
	private float speed;

	private string key;

	// Use this for initialization
	void Start () {
		speed = Random.Range (0.01f, 0.1f);
		if (transform.position.x == 0)
			key = "s";
		else if (transform.position.x == -0.94f)
			key = "a";
		else
			key = "d";
	}
	
	// Update is called once per frame
	void Update () {
		nbFrames++;
		float norm = 0.0f;
		if (Input.GetKeyDown (key))
			norm = transform.position.y + 3.0f;
		if (Input.GetKeyDown (key) && norm < 1.0f && norm > -1.0f)
		{
			Debug.Log ("Precision: " + norm);
			GameObject.Destroy (transform.root.gameObject);
		}
		else
		{
			transform.position -= new Vector3 (0.0f, speed, 0.0f);
			if (transform.position.y <= -6)
				GameObject.Destroy (transform.root.gameObject);
		}
	}
}
