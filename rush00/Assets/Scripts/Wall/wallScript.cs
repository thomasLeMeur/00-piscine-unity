using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wallScript : MonoBehaviour {

	void OnTriggerEnter2D (Collider2D coll) {
		if (coll.tag == "weapon")
			coll.gameObject.GetComponent<weaponScript> ().stopWeapon ();
	}
}
