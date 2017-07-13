using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter(Collision coll) {
		//if (coll.gameObject.tag == "Player") { 
		//	Debug.Log ("In");
		//	coll.gameObject.GetComponent<CharacterController> ().attachedRigidbody.velocity = Vector3.zero;
		}
}
