using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownScript : MonoBehaviour {

	public string camp;
	public int hp;
	public int HP;
	public bool isSpawn = false;
	static private PlayersManager manager;
	static TownScript Orcspawner;

	public GameObject[] prefabs = {};

	private const int fps = 60;
	private int nbFrames = 0;

	public float nbSecondsBeforeCreate = 10;

	void Start() {
		manager = Camera.main.GetComponent<PlayersManager> ();
		HP = hp;
		if (tag == "orcTown" && isSpawn)
			Orcspawner = this;
	}

	void Update () {
		if (isSpawn) {
			nbFrames++;
			if (nbFrames >= nbSecondsBeforeCreate * fps) {
				nbFrames = 0;
				int prefab;
				Vector3 newPos;
				if (camp == "Human") {
					prefab = 0;
					newPos = new Vector3 (-3f, 2f, 0);
				}
				else{
					prefab = 1;
					newPos = new Vector3 (4f, 0, 0);
				}
				GameObject.Instantiate (prefabs [prefab], newPos, Quaternion.identity);
			}
		}
	}

	void OnMouseDown() {
		if (camp != "Human")
			if (Input.GetKey (KeyCode.Mouse0))
				manager.Attack (this);
	}

	public void Kill() {
		MusicManager.instance.Play (MusicManager.instance.source3);
		gameObject.SetActive(false);
		if (tag == "orcTown") {
			if (isSpawn) {
				Debug.Log ("The Human Team wins.");
				Time.timeScale = 0;
			} else {
				Orcspawner.nbSecondsBeforeCreate += 2.5f;
			}
		}
	}
}
