using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class TankScript : MonoBehaviour {
	[HideInInspector] static public TankScript player;
	public List<GameObject> tanks = new List<GameObject>();

	public TextMesh life;
	public TextMesh ammo;

	private const short GUN_SHOT = 0;
	private const short MISSILE_SHOT = 1;
	private const short MISSILE_TAKEN = 2;

	public AudioClip[] clips = {};
	public GameObject gunOnFloor;
	public GameObject gunOnTank;
	public GameObject MissileOnFloor;
	public GameObject MissileOnTank;

	public float thrust;
	public float speed;
	public float boostMultiplicator = 1f;
	public int boostMax = 250;
	public int missilesAmmo = 10;
	public int hp = 100;
	public float gunRange = 100;
	public float missileRange = 300;

	private Transform tank;
	private Transform canon;
	private float yaw = 0;
	private bool inBoost = false;
	private bool inBoostLoad = false;
	private int curBoost;

	private GameObject end;
	public Image Viseur;

	void Awake () {
		player = this;
		end = GameObject.FindGameObjectWithTag ("victory");
		end.SetActive (false);
	}

	void Start () {
		curBoost = boostMax;
		tank = transform.GetChild (0);
		canon = transform.GetChild (1);
	}
	
	void Update () {
		if (Input.GetKeyDown (KeyCode.R))
			Application.LoadLevel (Application.loadedLevel);
		BoostManager ();
		MovementManager ();
		ShootManager ();
		GUIManager ();
	}

	void GUIManager() {
		if (hp < 0)
			hp = 0;
		life.text = hp.ToString ();
		ammo.text = missilesAmmo.ToString ();
		if (tanks.Count == 1)
			end.SetActive (true);
		RaycastHit rayHit;
		if (Physics.Raycast (canon.position, canon.TransformDirection (Vector3.forward), out rayHit, 10000) && rayHit.collider.tag != "map")
			Viseur.color = Color.red;
		else
			Viseur.color = Color.black;
	}

	void ShootManager() {
		if (Input.GetKeyDown (KeyCode.Mouse0)) {
			canon.gameObject.GetComponent<AudioSource> ().PlayOneShot(clips [GUN_SHOT]);
			RaycastHit rayHit;
			if (Physics.Raycast (canon.position, canon.TransformDirection (Vector3.forward), out rayHit, gunRange)) {
				if (rayHit.collider.tag != "map")
					rayHit.transform.gameObject.GetComponent<EnemyScript>().takeDamage(false);
				Instantiate (rayHit.collider.tag == "map" ? gunOnFloor : gunOnTank, rayHit.point, Quaternion.identity);
			}
		}
		else if (Input.GetKeyDown (KeyCode.Mouse1) && missilesAmmo > 0) {
			canon.gameObject.GetComponent<AudioSource> ().PlayOneShot(clips [MISSILE_SHOT]);
			missilesAmmo--;
			RaycastHit rayHit;
			if (Physics.Raycast (canon.position, canon.TransformDirection (Vector3.forward), out rayHit, missileRange)) {
				if (rayHit.collider.tag != "map")
					rayHit.transform.gameObject.GetComponent<EnemyScript>().takeDamage(true);
				Instantiate (rayHit.collider.tag == "map" ? MissileOnFloor : MissileOnTank, rayHit.point, Quaternion.identity);
			}
		}
	}

	void BoostManager () {
		if (Input.GetKeyDown (KeyCode.LeftShift))
			inBoost = true;
		else if (Input.GetKeyUp (KeyCode.LeftShift))
			inBoost = false;
		if (!inBoost && curBoost < boostMax)
			curBoost++;
		else if (inBoost) {
			if (curBoost <= 0) {
				inBoostLoad = true;
				inBoost = false;
			} else
				curBoost--;
		}
	}

	void MovementManager() {
		if (Input.GetKeyDown (KeyCode.LeftShift))
			inBoost = true;
		else if (Input.GetKeyUp (KeyCode.LeftShift))
			inBoost = false;
		if (Input.GetKey (KeyCode.W) || Input.GetKey (KeyCode.UpArrow)) {
			transform.Translate (tank.forward * Time.deltaTime * thrust * (inBoost ? boostMultiplicator : 1));
		}
		if (Input.GetKey (KeyCode.S) || Input.GetKey (KeyCode.DownArrow)) {
			transform.Translate (tank.forward * Time.deltaTime * -thrust * (inBoost ? boostMultiplicator : 1));
		}
		if (Input.GetKey (KeyCode.A) || Input.GetKey (KeyCode.LeftArrow)) {
			tank.Rotate (0, Time.deltaTime * -speed, 0);
		}
		if (Input.GetKey (KeyCode.D) || Input.GetKey (KeyCode.RightArrow)) {
			tank.Rotate (0, Time.deltaTime * speed, 0);
		}
		yaw += 5 * Input.GetAxis("Mouse X");
		canon.eulerAngles = new Vector3(0, yaw, 0);
	}

	public bool takeDamage(bool isMissile) {
		if (isMissile) {
			hp -= 45;
			tank.gameObject.GetComponent<AudioSource> ().PlayOneShot(clips [MISSILE_TAKEN]);
		} else
			hp -= 5;
		if (hp <= 0) {
			TankScript.player.tanks.Remove (gameObject);
			Application.LoadLevel (Application.loadedLevel);
			return true;
		}
		return false;
	}
}
