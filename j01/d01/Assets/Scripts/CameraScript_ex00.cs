using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript_ex00 : MonoBehaviour {

	private Rigidbody2D[] players;
	private PlayerScript_ex00[] scripts;

	private int ind = 0;

	void Start () {
		GameObject[] objs = GameObject.FindGameObjectsWithTag ("Player");
		players = new Rigidbody2D[objs.Length];
		scripts = new PlayerScript_ex00[objs.Length];
		for (int i = 0; i < objs.Length ; i++) {
			players [i] = objs [i].GetComponent<Rigidbody2D> ();
			scripts [i] = objs [i].GetComponent<PlayerScript_ex00> ();
			scripts[i].setPlayer(players [i]);
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
			Application.LoadLevel ("ex00");
		} else if (Input.GetKey (KeyCode.Space)) {
			scripts [ind].jump ();
		} else if (Input.GetKey (KeyCode.RightArrow)) {
			scripts [ind].right ();
		} else if (Input.GetKey (KeyCode.LeftArrow)) {
			scripts [ind].left ();
		}
	}

}
