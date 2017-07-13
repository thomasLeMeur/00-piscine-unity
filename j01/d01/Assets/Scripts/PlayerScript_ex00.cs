using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript_ex00 : MonoBehaviour {

	private Rigidbody2D player;

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

	public void setPlayer(Rigidbody2D play) {
		player = play;
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
}
