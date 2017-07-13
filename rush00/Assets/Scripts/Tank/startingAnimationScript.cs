using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class startingAnimationScript : MonoBehaviour {

	private float speed = 2.1f;
	private bool isStarted = false;

	public GameObject player;

	void Start () {
		
	}
	
	void Update () {
		transform.position = Vector3.MoveTowards (transform.position, new Vector3 (0, transform.position.y, transform.position.z), speed * Time.deltaTime);
		if (transform.position.x == 0 && isStarted == false) {
			Instantiate (player, transform.position + new Vector3(1, 3.6f, 0), Quaternion.identity);
			isStarted = true;
			GetComponentInChildren<turretScript> ().stopRotation ();
			GetComponent<AudioSource> ().Stop ();
		}
	}
}
