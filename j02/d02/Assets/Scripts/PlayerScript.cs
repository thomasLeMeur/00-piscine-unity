using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PlayerScript : MonoBehaviour {

	public enum PlayerAction
	{
		MOVE,
		FIGHT,
		STAY
	};

	static private PlayersManager manager;
	public PlayerAction action = PlayerAction.STAY;
	private float angle;
	private const float speed = 2f;
	public Vector2 dest;
	private Animator anim;
	public int hp;
	private const int attack = 1;
	private bool isAttack = false;
	private TownScript ennemy;
	private int nbFrames = 0;
	private const int nbFramesBeforeAttack = 50;

	void Start () {
		anim = GetComponent<Animator>();
		manager = Camera.main.GetComponent<PlayersManager> ();
	}

	// Update is called once per frame
	void Update () {
		if (ennemy != null && !ennemy.gameObject.activeSelf) {
			ennemy = null;
			anim.SetBool ("humanFight", false);
			action =  PlayerAction.STAY;
		}
		nbFrames++;
		if (action == PlayerAction.MOVE) {
			Vector3 oldpos = transform.position;
			transform.position = Vector3.MoveTowards (transform.position, dest, speed * Time.deltaTime);
			if (oldpos == transform.position) {
				anim.SetBool ("humanMove", false);
				action = PlayerAction.STAY;
			}
		}
		if (action == PlayerAction.FIGHT) {
			if (ennemy == null) {
				action = PlayerAction.STAY;
			}
			else if (nbFrames >= nbFramesBeforeAttack) {
				nbFrames = 0;
				ennemy.hp -= attack;
				Debug.Log ("Orc Unit [" + ennemy.hp + "/" + ennemy.HP + "] has been attacked.");
				if (ennemy.hp <= 0) {
					ennemy.hp = 0;
					ennemy.Kill();
				}
			}
		}
	}

	public void updatePlayer(Vector3 mouse, bool attack, TownScript other = null) {
		dest = Camera.main.ScreenToWorldPoint(mouse);
		findDirection (mouse, Camera.main.WorldToScreenPoint(transform.position));
		if (action == PlayerScript.PlayerAction.STAY)
			action = PlayerScript.PlayerAction.MOVE;
		isAttack = attack;
		ennemy = other;
		if (isAttack == false)
			anim.SetBool ("humanFight", false);
		if (action != PlayerScript.PlayerAction.FIGHT)
			anim.SetBool("humanMove", true);

	}

	public void findDirection (Vector3 me, Vector3 mouse)
	{
		angle = Mathf.Atan2 (mouse.y - me.y, mouse.x - me.x) * Mathf.Rad2Deg - 90f;
		transform.rotation = Quaternion.Euler (new Vector3 (transform.rotation.x, transform.rotation.y, angle));
	}

	void OnMouseDown() {
		if (Input.GetKey (KeyCode.LeftControl)) {
			manager.Add (this);
		}
		else if (Input.GetKey (KeyCode.Mouse0)) {
			manager.Remove ();
			manager.Add (this);
		}
	}

	public void Kill()
	{
		ennemy = null;
		isAttack = false;
		anim.SetBool ("humanFight", false);
	}

	void OnCollisionEnter2D(Collision2D col) {
		anim.SetBool ("humanMove", false);
		action =  PlayerAction.STAY;
		if (isAttack && col.collider.usedByEffector) {
			action = PlayerAction.FIGHT;
			anim.SetBool ("humanFight", true);
			nbFrames = 0;
			MusicManager.instance.Play (MusicManager.instance.source2);
		}
	}

}
