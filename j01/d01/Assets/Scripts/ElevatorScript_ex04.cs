using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorScript_ex04 : MonoBehaviour {

	private Vector3 init;

	public int width = 0;
	public int height = 0;

	void Start () {
		init = transform.position;
	}

	void Update () {
		if (width > 0)
			transform.position = new Vector3(Mathf.PingPong(Time.time, width) - init.x, transform.position.y, transform.position.z);
		if (height > 0)
			transform.position = new Vector3(transform.position.x, Mathf.PingPong(Time.time, height) - init.y, transform.position.z);
	}
}
