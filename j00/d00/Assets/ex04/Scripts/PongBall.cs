using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PongBall : MonoBehaviour {

	public GameObject p1;
	public GameObject p2;

	private int scoreP1 = 0;
	private int scoreP2 = 0;

	private float sensX;
	private float sensY;

	// Use this for initialization
	void Start () {
		reset ();
	}
	
	// Update is called once per frame
	void Update () {
		float x = transform.position.x + sensX;
		float y = transform.position.y + sensY;
		if (x - 0.2 < -5) {
			scoreP2++;
			Debug.Log ("Player 1: " + scoreP1 + " | Player 2: " + scoreP2);
			reset ();
		}
		else if (x + 0.2 > 5) {
			scoreP1++;
			Debug.Log ("Player 1: " + scoreP1 + " | Player 2: " + scoreP2);
			transform.position = new Vector3 (0, 0, 0);
			reset ();
		}
		else {
			if (y + 0.2 > 4.5 || y - 0.2 < -4.5)
				sensY *= -1;
			else if ((x - 0.2 >= p1.transform.position.x - 0.1 && x - 0.2 <= p1.transform.position.x + 0.1
			         && ((y + 0.2 <= p1.transform.position.y + 0.8 && y + 0.2 >= p1.transform.position.y - 0.8)
			         || (y - 0.2 <= p1.transform.position.y + 0.8 && y - 0.2 >= p1.transform.position.y - 0.8)))) {
				sensX *= -1;
				x = transform.position.x + sensX;
				y = transform.position.y + sensY;
			} else if ((x + 0.2 >= p2.transform.position.x - 0.1 && x + 0.2 <= p2.transform.position.x + 0.1
			         && ((y + 0.2 <= p2.transform.position.y + 0.8 && y + 0.2 >= p2.transform.position.y - 0.8)
			         || (y - 0.2 <= p2.transform.position.y + 0.8 && y - 0.2 >= p2.transform.position.y - 0.8)))) {
				sensX *= -1;
				x = transform.position.x + sensX;
				y = transform.position.y + sensY;
			}
			transform.position = new Vector3 (x, y, 0);
		}
	}

	void reset() {
		transform.position = new Vector3 (0, 0, 0);
		sensX = Random.Range (0, 2);
		sensY = Random.Range (0, 2);
		if (sensX < 1)
			sensX = -0.1f;
		else
			sensX = 0.1f;
		if (sensY < 1)
			sensY = -0.1f;
		else
			sensY = 0.1f;
	}
}
