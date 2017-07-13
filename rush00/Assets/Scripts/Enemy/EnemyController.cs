using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

	[HideInInspector] public float	speed;
	[HideInInspector] public enum 	Status {idle, patrol, pursuingPlayer, goingToLastLoc, search, backToStart, goingToSound, dead};
	[HideInInspector] public 		Vector3 startPosition;
	[HideInInspector] public 		Quaternion startRotation;
	[HideInInspector] public 		Status startStatus;
	[HideInInspector] public 		Vector3 targetPosition;

	public Sprite[]			heads;
	public Sprite[]			bodies;
	public List<Sprite> weaponSprites = new List<Sprite>();
	public float 			startSpeed;
	public float 			pursuitSpeed;
	public float 			soundRadius;
	public Status 			currentStatus = Status.idle;
	public float 			range;
	public float 			fireRange;

	GameObject 				playerObj;
	Rigidbody2D 			rid;
	RaycastHit2D 			hit;
	IEnumerator				coroutineLookAround = null;
	int 					layerMask = 1 << 8 | 1 << 9;
	int 					weapon;
	bool					playerInRange = false;

	// Use this for initialization
	void Start () {
		
		startPosition = transform.position;
		startRotation = transform.rotation;
		startStatus = currentStatus;
		speed = startSpeed;
		int weapon = Random.Range (1, 12);
		if (weapon == 5)
			fireRange = 1;
		transform.GetChild(3).gameObject.GetComponent<SpriteRenderer> ().sprite = weaponSprites [weapon - 1];
		GetComponent<enemyWeaponScript> ().init(weapon);

		playerObj = GameObject.FindGameObjectWithTag ("Player");
		player.nbEnemies++;
		targetPosition = transform.position;
		rid = GetComponent<Rigidbody2D> ();
		SpriteRenderer[] children = gameObject.GetComponentsInChildren<SpriteRenderer>();
		foreach (SpriteRenderer child in children)
			if (child.tag == "Head")
				child.sprite = heads[Random.Range (0, heads.Length)];
		gameObject.GetComponent<SpriteRenderer>().sprite = bodies[Random.Range(0, bodies.Length)];
	}

	void Update () {

		if (playerObj == null)
			playerObj = GameObject.FindGameObjectWithTag ("Player");
		
		Movement ();
		PlayerDetect ();
		if (currentStatus == Status.pursuingPlayer || currentStatus == Status.goingToLastLoc || currentStatus == Status.goingToSound)
			transform.GetChild (0).GetComponent<SpriteRenderer> ().color = Color.white;
		else
			transform.GetChild (0).GetComponent<SpriteRenderer> ().color = new Color(1f, 1f, 1f, 0f);

		if (currentStatus == Status.idle || currentStatus != Status.search)
			isStaying ();
		else
			isMoving ();
	}

	void OnTriggerEnter2D(Collider2D coll)
	{
		if (coll.gameObject.tag == "Player")
			playerInRange = true;
	}

	void OnTriggerExit2D(Collider2D coll)
	{
		if (coll.gameObject.tag == "Player")
			playerInRange = false;
	}

	public void Movement() {

		if (!playerObj)
			return;
		float dist = Vector3.Distance (playerObj.transform.position, transform.position);
		Vector3 dir = playerObj.transform.position - transform.position;

		hit = Physics2D.Raycast (new Vector2 (transform.position.x, transform.position.y), new Vector2 (dir.x, dir.y), dist, layerMask);
		Debug.DrawRay (transform.position, dir, Color.red);

		Vector3 fwt = transform.TransformDirection (Vector3.down);
		Debug.DrawRay (new Vector2 (transform.position.x, transform.position.y), new Vector2 (fwt.x, fwt.y), Color.cyan);

		if (currentStatus == Status.patrol || currentStatus == Status.backToStart) {
			speed = startSpeed;
			transform.position = Vector2.MoveTowards (transform.position, targetPosition, speed * Time.deltaTime);
		}
		else if (currentStatus == Status.pursuingPlayer) {
			speed = pursuitSpeed;

			rid.transform.eulerAngles = new Vector3 (0, 0, Mathf.Atan2 ((targetPosition.y - transform.position.y), (targetPosition.x - transform.position.x)) * Mathf.Rad2Deg + 90f);
			if (hit && hit.collider && hit.collider.gameObject.tag == "Player")
				targetPosition = playerObj.transform.position;

			if (Vector3.Distance (transform.position, playerObj.transform.position) > fireRange)
				transform.position = Vector2.MoveTowards (transform.position, targetPosition, speed * Time.deltaTime);
		}
		else if (currentStatus == Status.goingToLastLoc) {
			if (transform.position == targetPosition) {
				currentStatus = Status.search;
				coroutineLookAround = LookatCoroutine ();
				StartCoroutine (coroutineLookAround);
			} else {
				transform.position = Vector2.MoveTowards (transform.position, targetPosition, speed * Time.deltaTime);
			}
		}
	}

	public void PlayerDetect() {
		if (hit.collider) {
			if (hit.collider.gameObject.tag == "Player" && Vector3.Distance (transform.position, playerObj.transform.position) < range && playerInRange) {				
				playerInRange = true;
				currentStatus = Status.pursuingPlayer;

				if (Vector3.Distance (transform.position, playerObj.transform.position) < fireRange)
					GetComponent<enemyWeaponScript> ().onFire (computeAngle(transform.position, playerObj.transform.position));
				if (coroutineLookAround != null)
					StopCoroutine (coroutineLookAround);
			}
			else {
				if (currentStatus == Status.pursuingPlayer) {
					currentStatus = Status.goingToLastLoc;
					playerInRange = false;
				}
			}
		}
	}

	float computeAngle(Vector3 me, Vector3 play) {
		return Mathf.Atan2(play.y - me.y, play.x - me.x) * Mathf.Rad2Deg;
	}

	void isMoving()
	{
//		GetComponent<Animator>().SetBool ("isMoving", true);
//		feet.GetComponent<Animator> ().speed = speed;
	}

	void isStaying()
	{		
//		GetComponent<Animator>().SetBool ("isMoving", false);
	}

	public void Lookat(Vector3 at)
	{
		transform.rotation = Quaternion.LookRotation (Vector3.forward, at);
	}

	public void takeDamage () {
		if (currentStatus != Status.dead) {
			currentStatus = Status.dead;
			Destroy (GetComponent<Rigidbody2D> ());
			player.nbEnemies--;
			StartCoroutine (DeadCoroutine ());
		}
	}

	IEnumerator LookatCoroutine()
	{
		int i = 0;
//		for (int i=0; i<5; i++) {
		while(true) {
			switch(i)
			{
			case 0:
				Lookat(transform.rotation * Vector3.left);
				break;
			case 1:
				Lookat(transform.rotation * Vector3.right);
				break;
			case 2:
				Lookat(transform.rotation * Vector3.down);
				break;
			case 3:
				Lookat(transform.rotation * Vector3.up);
				break;
			case 4:
				if (currentStatus == Status.search)
				{
					i = 0;
//					currentStatus = Status.backToStart;
//					GetComponent<PathfindingScript> ().GoToTarget (startPosition);
				}
				break;
			}
			i++;
			yield return new WaitForSeconds (1);
		}
	}

	IEnumerator DeadCoroutine()
	{
		GetComponent<AudioSource> ().Play ();
		transform.position = new Vector2 (10000, 10000);

		Destroy (GetComponent<SpriteRenderer> ());
		while (GetComponent<AudioSource> ().isPlaying)
			yield return null;		
		Destroy (gameObject);
	}

}