using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weaponScript : MonoBehaviour {

	private bool triggered = false;

	private Collider2D hit;
	private float droppedAngle;
	private float droppedForce;
	private int frames = 0;
	private float droppedx = 0;
	private float droppedy = 0;
	private int rifleOnFire = 0;

	public bool active = false;
	public AudioClip empty;
	public AudioClip pickUp;
	public int index = 0;
	public int ammo = 0;
	public int fireRate = 0;
	public GameObject projectile;

	void Start () {
		
	}

	void Update () {
		if (player.hasWon || player.isDead)
			return;
		if (Input.GetKeyDown ("e")) {
			if (triggered && !active)
				onPickUp ();
		}
		if (Input.GetKey (KeyCode.Mouse1) && active) {
			onDrop ();
		}
		if (active) {
			transform.position = hit.transform.position;
		}
		if (droppedForce != 0) {
			transform.position += new Vector3 (droppedForce * droppedx * 0.02f, droppedForce * droppedy * 0.02f, 0);
			transform.Rotate (0, 0, droppedForce * -10);
			droppedForce -= 0.5f;
		} else {
			transform.position = new Vector3 (transform.position.x, transform.position.y, 0);
		}
		if (Input.GetKey (KeyCode.Mouse0) && active && frames > fireRate) {
			if (ammo != 0)
				onFire ();
			else {
				GetComponent<AudioSource> ().clip = empty;
				GetComponent<AudioSource> ().Play ();
				frames = 0;
			}
		}
		if (rifleOnFire != 0 && frames > 5) {
			onFire ();
		}
		frames++;
	}

	void onFire () {
		if (index == 6 || index == 1) {
			if (rifleOnFire == 0)
				rifleOnFire = 2;
			else
				rifleOnFire--;
		}
		ammo--;
		frames = 0;
		Instantiate (projectile, transform.position, Quaternion.identity).GetComponent<shoot> ().init (index, true);
	}

	void onPickUp () {
		GameObject [] list = GameObject.FindGameObjectsWithTag ("weapon");
		int nb = 0;
		foreach (GameObject go in list) {
			weaponScript w = go.GetComponent<weaponScript> ();
			if (w.active)
				nb++;
		}
		if (nb == 0) {
			GetComponent<AudioSource> ().clip = pickUp;
			GetComponent<AudioSource> ().Play ();
			active = true;
			GetComponent<SpriteRenderer> ().color *= new Color (1, 1, 1, 0);
			player.perso.getWeapon (gameObject, index);
		}
	}

	void onDrop () {
		active = false;
		GetComponent<SpriteRenderer> ().color += new Color (0, 0, 0, 1);
		droppedAngle = 0.5f;
		droppedForce = 10;
		findAngle(Input.mousePosition, Camera.main.WorldToScreenPoint(transform.position));
		player.perso.dropWeapon ();
	}

	void findAngle (Vector2 mouse, Vector2 player) {
		droppedAngle = Mathf.Atan2 (mouse.y - player.y, mouse.x - player.x);
		droppedx = Mathf.Cos (droppedAngle);
		droppedy = Mathf.Sin (droppedAngle);
	}

	void OnTriggerEnter2D (Collider2D collision) {
		if (collision.tag == "Player") {
			triggered = true;
			hit = collision;
		}
	}

	void OnTriggerExit2D (Collider2D collision) {
		if (collision.tag == "Player") {
			triggered = false;
		}
	}

	public void stopWeapon() {
		droppedx = 0;
		droppedy = 0;
	}
}
