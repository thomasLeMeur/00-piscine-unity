using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript_ex02 : MonoBehaviour {

	private Rigidbody2D player;
	private CameraScript_ex02 camera;

	private int ind;
	private bool inJump = false;

	void Update () {
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

	public void setPlayer(Rigidbody2D play, CameraScript_ex02 cam) {
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
		camera.addFinish ();
	}

	void OnTriggerExit2D() {
		camera.removeFinish ();
	}
}
