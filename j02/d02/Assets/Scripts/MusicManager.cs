using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour {

	public static MusicManager	instance { get; private set;}
	public AudioSource source1;
	public AudioSource source2;
	public AudioSource source3;

	// Use this for initialization
	void Awake () {
		instance = this;
	}
	
	// Update is called once per frame
	public void Play (AudioSource sourceTmp) {
		sourceTmp.Play();
	}
}
