using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour {

	[HideInInspector] static public player perso;
	[HideInInspector] public static GameObject weapon;
	[HideInInspector] public static bool isDead;
	[HideInInspector] public static bool hasWon;
	[HideInInspector] public static int nbEnemies = 0;

	public GameObject cam;
	public List<Sprite> bodySprites = new List<Sprite>();
	public List<Sprite> weaponSprites = new List<Sprite>();
	public List<Sprite> headSprites = new List<Sprite>();

	//inventaire

	private int speed;
	private int keydowns;
	private bool isMoving;
	private bool canMove;
	[HideInInspector] public float viewAngle;
	private SpriteRenderer bodySprite;
	private SpriteRenderer weaponSprite;
	private SpriteRenderer headSprite;

	void Awake () {
		perso = this;
		speed = 8;
		keydowns = 0;
		viewAngle = 0;
		weapon = null;
		canMove = true;
		isDead = false;
		hasWon = false;
		cam = GameObject.FindGameObjectWithTag ("MainCamera");
		bodySprite = GameObject.Find (gameObject.name + "/body").GetComponent<SpriteRenderer> ();
		weaponSprite = GameObject.Find (gameObject.name + "/weapon").GetComponent<SpriteRenderer> ();
		headSprite = GameObject.Find (gameObject.name + "/head").GetComponent<SpriteRenderer> ();
	}

	void Start () {
		//generation aleatoire des sprites du player
		bodySprite.sprite = bodySprites[(int)Random.Range(0, bodySprites.Count - 0.0001f)];
		headSprite.sprite = headSprites[(int)Random.Range(0, headSprites.Count - 0.0001f)];
	}
	
	void Update () {
		if (!hasWon && !isDead)
		{
			//Gestion du restart
			if (Input.GetKeyDown (KeyCode.R))
				Application.LoadLevel (Application.loadedLevel);

			//Gestion des KeyInputs pour les deplacements
			if (Input.GetKey (KeyCode.W) || Input.GetKey (KeyCode.UpArrow))
				moveKeyDown (1, Vector3.up);
			if (Input.GetKey (KeyCode.A) || Input.GetKey (KeyCode.LeftArrow))
				moveKeyDown (2, Vector3.left);
			if (Input.GetKey (KeyCode.S) || Input.GetKey (KeyCode.DownArrow))
				moveKeyDown (4, Vector3.down);
			if (Input.GetKey (KeyCode.D) || Input.GetKey (KeyCode.RightArrow))
				moveKeyDown (8, Vector3.right);

			//Gestion de l'animation en deplacement qui depent des touches pressees
			if (Input.GetKeyUp (KeyCode.W) || Input.GetKeyUp (KeyCode.UpArrow))
				moveKeyUp (1);
			else if (Input.GetKeyUp (KeyCode.A) || Input.GetKeyUp (KeyCode.LeftArrow))
				moveKeyUp (2);
			else if (Input.GetKeyUp (KeyCode.S) || Input.GetKeyUp (KeyCode.DownArrow))
				moveKeyUp (4);
			else if (Input.GetKeyUp (KeyCode.D) || Input.GetKeyUp (KeyCode.RightArrow))
				moveKeyUp (8);

			//Gestion de la direction de la souris
			transform.rotation = Quaternion.Euler(new Vector3(0, 0, computeAngle(transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition))));

			//Update de la camera pour qu'elle suive le joueur s'il bouge
			cam.transform.position = new Vector3 (transform.position.x, transform.position.y, cam.transform.position.z);

//			if (Input.GetMouseButtonDown (0))
//				shoot (transform.position, viewAngle);
		}
		else
			GetComponent<Animator> ().SetBool ("isPlayerMoving", false);
	}

	//Getteur de l'angle de vue
	public float getAngle () {
		return viewAngle;
	}

	//Fonction appelee par le script d'arme pour indiquer que le joueur la tient desormais
	public void getWeapon (GameObject weap, int indexOfSprite) {
		weapon = weap;
		weaponSprite.sprite = weaponSprites[indexOfSprite - 1];

		//mettre a jour l'inventaire
	}

	//Fonction appelee par le script d'arme pour indiquer que le joueur ne la tient plus
	public void dropWeapon () {
		weapon = null;
		weaponSprite.sprite = null;

		//mettre a jour l'inventaire
	}

	//Fonction qui fait jouer le son de mort ...
	public void takeDamage () {
		isDead = true;
		GetComponent<AudioSource> ().Play ();
	}

	void moveKeyUp (int keyVal) {
		keydowns &= ~keyVal;
		if (keydowns == 0)
			GetComponent<Animator> ().SetBool ("isPlayerMoving", false);
	}

	void moveKeyDown (int keyVal, Vector3 way) {
		if (keydowns == 0)
			GetComponent<Animator> ().SetBool ("isPlayerMoving", true);
		keydowns |= keyVal;
		transform.Translate (way * Time.deltaTime * speed, Camera.main.transform);
		cam.transform.position = new Vector3 (transform.position.x, transform.position.y, cam.transform.position.z);
	}
	
	float computeAngle(Vector3 me, Vector3 mouse) {
		viewAngle = Mathf.Atan2(mouse.y - me.y, mouse.x - me.x) * Mathf.Rad2Deg + 90f;
		return viewAngle;
	}

	void OnTriggerEnter2D(Collider2D coll) {
		if (coll.gameObject.tag == "exit")
			hasWon = true;
	}
}
