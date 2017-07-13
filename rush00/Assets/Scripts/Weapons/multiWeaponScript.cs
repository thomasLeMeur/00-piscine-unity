using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class multiWeaponScript : MonoBehaviour {

	public GameObject[] liste = { };

	void Start () {
		int r = Random.Range (0, 11);
		Instantiate (liste [r], transform.position, Quaternion.identity);
	}
	
	void Update () {
		
	}
}
