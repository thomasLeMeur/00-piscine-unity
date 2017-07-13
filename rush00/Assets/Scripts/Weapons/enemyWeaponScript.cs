using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyWeaponScript : MonoBehaviour {

	private int frames = 0;
	private int rifleOnFire = 0;
	private float shootAngle = 0;

	public int index = 0;
	public int fireRate = 0;
	public GameObject projectile;

	void Start () {

	}

	void Update () {
		if (rifleOnFire != 0 && frames > 5) {
			onFire (shootAngle);
		}
		frames++;
	}

	public void onFire (float angle) {
		shootAngle = angle;
		if (frames > fireRate) {
			if (index == 6 || index == 1) {
				if (rifleOnFire == 0)
					rifleOnFire = 2;
				else
					rifleOnFire--;
			}
			frames = 0;
			Instantiate (projectile, transform.position, Quaternion.identity).GetComponent<shoot> ().init (index, false, angle);
		}
	}

	public void init (int id) {
		index = id;
		Mathf.Clamp (id, 1, 12);
		if (id == 6 || id == 4)
			fireRate = 120;
		else if (id == 10 || id == 9 || id == 3 || id == 5 || id == 1)
			fireRate = 30;
		else if (id == 7 || id == 12)
			fireRate = 45;
		else if (id == 2 || id == 11)
			fireRate = 60;
		else
			fireRate = 5;
	}
}
