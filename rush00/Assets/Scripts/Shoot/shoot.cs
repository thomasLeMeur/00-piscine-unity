using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shoot : MonoBehaviour {

	public List<Sprite> sprites = new List<Sprite> ();
	public List<AudioClip> clips = new List<AudioClip> ();

	private bool isInit;
	private int indexOfWeapon;
	private bool isPlayerShoot;
	private int speed;
	private float offsetx;
	private float offsety;
	private int nbFrames;

	void Awake () {
		offsetx = 0;
		offsety = 0;
		isInit = false;
		isPlayerShoot = false;
		nbFrames = 0;
		speed = 6;
	}

	void Update () {
		if (GetComponent<SpriteRenderer> ().sprite != null) {
			nbFrames++;
			if (indexOfWeapon == 4 && nbFrames > 2)
				StartCoroutine (waitForSound ());
			if (isInit == true && indexOfWeapon != 4)
				transform.position += new Vector3 (offsetx * Time.deltaTime * speed, offsety * Time.deltaTime * speed, 0);
		}
	}

	public void init (int index, bool isPlayer, float angle = 0) {
		if (isPlayer)
			angle = (player.perso.viewAngle - 90);
		indexOfWeapon = index - 1;
		GetComponent<AudioSource> ().clip = clips [indexOfWeapon];
		GetComponent<AudioSource> ().Play ();
		GetComponent<SpriteRenderer> ().sprite = sprites [indexOfWeapon];
		transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
		offsetx = Mathf.Cos (angle * Mathf.Deg2Rad);
		offsety = Mathf.Sin (angle * Mathf.Deg2Rad);
		transform.position += new Vector3(offsetx * Time.deltaTime * speed * 5, offsety * Time.deltaTime * speed * 5, 0);
		isPlayerShoot = isPlayer;
		isInit = true;
		nbFrames = 0;
	}

	void OnCollisionEnter2D(Collision2D coll) {
		if (coll.gameObject.tag == "Enemy" && isPlayerShoot)
			coll.gameObject.GetComponent<EnemyController> ().takeDamage ();
		else if (coll.gameObject.tag == "Enemy")
			return;
		else if (coll.gameObject.tag == "weapon" || coll.gameObject.tag == "ammo")
			return;
		else {
			if (coll.gameObject.tag == "Player") {
				if (!isPlayerShoot)
					coll.gameObject.GetComponent<player> ().takeDamage ();
				else if (indexOfWeapon != 4)
					return;
			}
		}
		StartCoroutine (waitForSound ());
	}


	IEnumerator waitForSound() {
		GetComponent<SpriteRenderer> ().sprite = null;
		Destroy (GetComponent<Collider2D> ());
		while (GetComponent<AudioSource> ().isPlaying)
			yield return null;
		GameObject.Destroy (gameObject);
	}
}
