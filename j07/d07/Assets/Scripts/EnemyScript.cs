using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyScript : MonoBehaviour {
	private const short GUN_SHOT = 0;
	private const short MISSILE_SHOT = 1;
	private const short MISSILE_TAKEN = 2;

	public AudioClip[] clips = {};
	public GameObject gunOnFloor;
	public GameObject gunOnTank;
	public GameObject MissileOnFloor;
	public GameObject MissileOnTank;

	private GameObject target;

	public float thrust;
	public float speed;
	public float boostMultiplicator = 1f;
	public int boostMax = 250;
	public int missilesAmmo = 10;
	public int hp = 100;
	public float gunRange = 100;
	public float missileRange = 300;
	public int missileRate = 200;
	public int gunRate = 20;

	private Transform tank;
	private Transform canon;
	private int nbFrames;
	private int lastShoot;

	void Start () {
		nbFrames = 0;
		lastShoot = -2000;
		tank = transform.GetChild (0);
		canon = transform.GetChild (1);
	}

	void Update () {
		nbFrames++;
		getTarget ();
		GetComponent<NavMeshAgent> ().destination = target.transform.position;
		int ret = canShoot ();
		if (ret < 0)
			GetComponent<NavMeshAgent> ().destination = target.transform.position;
		else {
			GetComponent<NavMeshAgent> ().destination = transform.position;
			if (isShootReady(ret))
				shoot (ret);
		}
	}

	void getTarget() {
		target = TankScript.player.tanks [0];
		foreach (GameObject cur in TankScript.player.tanks) {
			if (target.name == gameObject.name || (Vector3.Distance(cur.transform.position, transform.position) < Vector3.Distance(target.transform.position, transform.position) && cur.name != gameObject.name)) {
				target = cur;
			}
		}
	}

	int canShoot() {
		RaycastHit rayHit;
		if (missilesAmmo > 0) {
			if (Physics.Raycast (canon.position, canon.TransformDirection (Vector3.forward), out rayHit, missileRange)) {
				if (rayHit.collider.tag != "map") {
					return MISSILE_SHOT;
				}
			}
		}
		if (Physics.Raycast (canon.position, canon.TransformDirection (Vector3.forward), out rayHit, gunRange)) {
			if (rayHit.collider.tag != "map") {
				return GUN_SHOT;
			}
		}
		return -1;
	}

	bool isShootReady(int type) {
		if ((type == MISSILE_SHOT && nbFrames - lastShoot < missileRate) || (type == GUN_SHOT && nbFrames - lastShoot < gunRate))
			return false;
		if (type == MISSILE_SHOT)
			missilesAmmo--;
		lastShoot = nbFrames;
		return true;
	}

	void shoot(int type) {
		float angle = Random.Range (-10f, 10f);
		transform.eulerAngles += new Vector3 (0, angle, 0);

		RaycastHit rayHit;
		Physics.Raycast (transform.position, transform.TransformDirection (Vector3.forward), out rayHit, (type == GUN_SHOT) ? gunRange : missileRange);
		if (rayHit.collider.tag == "map")
			Instantiate (type == GUN_SHOT ? gunOnFloor : MissileOnFloor, rayHit.point, Quaternion.identity);
		else if (rayHit.collider.tag == "Player") {
			rayHit.transform.gameObject.GetComponent<TankScript> ().takeDamage ((type == GUN_SHOT) ? false : true);
			Instantiate (type == GUN_SHOT ? gunOnTank : MissileOnTank, rayHit.point, Quaternion.identity);
		} else {
			rayHit.transform.gameObject.GetComponent<EnemyScript> ().takeDamage ((type == GUN_SHOT) ? false : true);
			Instantiate (type == GUN_SHOT ? gunOnTank : MissileOnTank, rayHit.point, Quaternion.identity);
		}

		transform.eulerAngles -= new Vector3 (0, angle, 0);
	}

	public void takeDamage(bool isMissile) {
		if (isMissile) {
			hp -= 45;
			tank.gameObject.GetComponent<AudioSource> ().PlayOneShot(clips [MISSILE_TAKEN]);
		} else
			hp -= 5;
		if (hp <= 0) {
			TankScript.player.tanks.Remove (gameObject);
			Destroy (gameObject);
		}
	}
}
