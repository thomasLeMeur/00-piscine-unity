using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript_ex04 : MonoBehaviour {

	private Rigidbody2D player;
	private CameraScript_ex04 camera;
	private GameObject exit;

	private int ind;
	private bool inJump = false;

	void Start () {
		exit = GameObject.FindGameObjectWithTag("Respawn");
	}

	public void jump () {
		if (!inJump) {
			player.AddForce (transform.up * 400f);
			inJump = true;
		}
	}

	public void left () {
		player.AddForce (transform.right * -15f);
	}

	public void right () {
		player.AddForce (transform.right * 15f);
	}

	public void setPlayer(Rigidbody2D play, CameraScript_ex04 cam) {
		player = play;
		camera = cam;
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		if(collision.contacts.Length > 0)
		{
			ContactPoint2D contact = collision.contacts[0];
			if (Vector3.Dot (contact.normal, Vector3.up) > 0.9) {
				inJump = false;
			}
		}
	}

	void OnTriggerEnter2D(Collider2D collider) {
		if (collider.tag == "Finish")
			camera.addFinish ();
		else if (collider.tag != "porte" && collider.tag != "portePlat")
			transform.position = exit.transform.position;
	}

	void OnTriggerExit2D(Collider2D collider) {
		if (collider.tag == "Finish")
			camera.removeFinish ();
	}
}
