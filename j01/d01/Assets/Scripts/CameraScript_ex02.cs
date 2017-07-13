using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript_ex02 : MonoBehaviour {

	private Rigidbody2D[] players;
	private PlayerScript_ex02[] scripts;

	private int ind = 0;
	private int nbFinish = 0;

	void Start () {
		GameObject[] objs = GameObject.FindGameObjectsWithTag ("Player");
		players = new Rigidbody2D[objs.Length];
		scripts = new PlayerScript_ex02[objs.Length];
		for (int i = 0; i < objs.Length ; i++) {
			players [i] = objs [i].GetComponent<Rigidbody2D> ();
			scripts [i] = objs [i].GetComponent<PlayerScript_ex02> ();
			scripts[i].setPlayer(players [i], this);
		}
		ind = 0;
	}
	
	void Update () {
		transform.position = new Vector3(players [ind].position.x, players [ind].position.y, transform.position.z);
		if (Input.GetKeyDown ("1")) {
			ind = 0;
		} else if (Input.GetKeyDown ("2")) {
			ind = 1;
		} else if (Input.GetKeyDown ("3")) {
			ind = 2;
		} else if (Input.GetKeyDown ("r")) {
			Application.LoadLevel ("ex02");
		} else if (Input.GetKey (KeyCode.Space)) {
			scripts [ind].jump ();
		} else if (Input.GetKey (KeyCode.RightArrow)) {
			scripts [ind].right ();
		} else if (Input.GetKey (KeyCode.LeftArrow)) {
			scripts [ind].left ();
		}
	}

	public void addFinish() {
		if (++nbFinish == players.Length) {
			Debug.Log ("You win !");
			Application.LoadLevel ("ex03");
		}
	}

	public void removeFinish() {
		--nbFinish;
	}
}
