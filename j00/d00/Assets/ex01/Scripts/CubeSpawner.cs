using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSpawner : MonoBehaviour {
	private float[] xtabs = {-0.94f, 0.0f, 0.94f};
	public GameObject[] prefabs = {};

	private const int fps = 60;
	private int nbFrames = 0;

	private float nbSecondsBeforeCreate;

	// Use this for initialization
	void Start () {
		nbSecondsBeforeCreate = Random.Range (1.0f, 4.0f);
	}
	
	// Update is called once per frame
	void Update () {
		nbFrames++;
		if (nbFrames >= nbSecondsBeforeCreate * fps) {
			nbFrames = 0;
			int prefab;
			while (true) {
				prefab = (int)Random.Range (0.0f, 2.99f);
					break;
			}
			Vector3 newPos = new Vector3 (xtabs[prefab], transform.position.y, 0);
			GameObject.Instantiate (prefabs[prefab], newPos, Quaternion.identity);
			nbSecondsBeforeCreate = Random.Range (0.01f, 1.0f);
		}
	}
}
