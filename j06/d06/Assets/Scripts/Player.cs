using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {

	[HideInInspector] public Player perso;
	public AudioClip[] clips = {};
	public AudioSource music;
	public AudioSource alarm;
	public Slider s;
	public Text t;
	public Text middle;
	public float IncrRun = 15f;
	private GameObject door;

	private bool isWalking;
	private bool isLight;
	private bool isFumee;
	private bool isCamera;
	private bool wasWalking;
	private int nbFrames1 = 0;
	private int nbFrames2 = 0;
	private int nbFrames3 = 0;
	private const int nbFramesBeforeUp = 3;
	private const int nbFramesBeforeLight = 2;
	private bool hasKey;
	private bool isKeyTrigger;
	private bool isDoorTrigger;
	private bool isPaperTrigger;

	void Awake() {
		perso = this;
		isWalking = true;
		wasWalking = true;
		isLight = false;
		isFumee = false;
		isCamera = false;
		hasKey = false;
		isKeyTrigger = false;
		isDoorTrigger = false;
		isPaperTrigger = false;
		door = GameObject.FindGameObjectWithTag ("door");
		s.transform.GetChild (1).GetComponentInChildren<Image> ().color = new Color(103, 98, 98, 255);
	}

	void Update () {
		wasWalking = isWalking;
		isWalking = !Input.GetKey(KeyCode.LeftShift);
		if (wasWalking != isWalking)
			nbFrames1 = 0;
		else {
			if (++nbFrames1 > nbFramesBeforeUp) {
				nbFrames1 = 0;
				updateUI ();
			}
		}
		if (isLight) {
			if (++nbFrames2 > nbFramesBeforeLight) {
				nbFrames2 = 0;
				updateUI ();
			}
			if (isWalking)
				nbFrames1 = 0;
		}
		if (isCamera) {
			if (++nbFrames3 > nbFramesBeforeLight) {
				nbFrames3 = 0;
				updateUI ();
			}
			if (isWalking)
				nbFrames1 = 0;
		}
		if (Input.GetKeyDown (KeyCode.E)) {
			Debug.Log (isKeyTrigger + " " + isDoorTrigger + " " + hasKey + " " + isPaperTrigger);
			if (isKeyTrigger) {
				GetComponent<AudioSource> ().PlayOneShot (clips [2]);
				Destroy (GameObject.FindGameObjectWithTag ("key"));
				hasKey = true;
				isKeyTrigger = false;
				middle.text = "";
			} else if (isDoorTrigger) {
				if (!hasKey)
					GetComponent<AudioSource> ().PlayOneShot (clips [3]);
				else {
					if (door.active) {
						GetComponent<AudioSource> ().PlayOneShot (clips [4]);
						door.SetActive (false);
					}
					else
						door.SetActive (true);
				}
			} else if (isPaperTrigger) {
				Application.LoadLevel (Application.loadedLevel);
			}
		}
	}

	void OnTriggerEnter(Collider coll) {
		if (coll.tag == "light") {
			isLight = true;
		}
		if (coll.tag == "fumee") {
			isFumee = true;
		}
		if (coll.tag == "camera") {
			isCamera = true;
		}
		if (coll.tag == "key") {
			isKeyTrigger = true;
			middle.text = "Press E to get the key";
		}
		if (coll.tag == "switch") {
			isDoorTrigger = true;
			if (!hasKey)
				middle.text = "You need the key to open the door";
			else
				middle.text = "Press E to turn on/off the door";
		}
		if (coll.tag == "papers") {
			isPaperTrigger = true;
			middle.text = "Press E to get the papers";
		}
	}

	void OnTriggerExit(Collider coll) {
		if (coll.tag == "light") {
			isLight = false;
			nbFrames2 = 0;
		}
		if (coll.tag == "fumee") {
			isFumee = false;
		}
		if (coll.tag == "camera") {
			isCamera = false;
			nbFrames3 = 0;
		}
		if (coll.tag == "key") {
			isKeyTrigger = false;
			middle.text = "";
		}
		if (coll.tag == "switch") {
			isDoorTrigger = false;
			middle.text = "";
		}
		if (coll.tag == "papers") {
			isPaperTrigger = false;
			middle.text = "";
		}
	}

	void updateUI (){
		float rep = IncrRun * ((isCamera) ? 3 : 1) / (float)((isFumee) ? 2 : 1);
		s.value = Mathf.Clamp(s.value + ((isWalking && !isLight && !isCamera) ? -rep : rep), 0, 1000);
		t.text = (s.value / 10f).ToString() + "%";
		if (s.value == 1000)
			Application.LoadLevel (Application.loadedLevel);
		if (s.value >= 750) {
			s.transform.GetChild (1).GetComponentInChildren<Image> ().color = new Color (116, 0, 0, 255);
			if (music.isPlaying && music.clip == clips [0]) {
				music.Stop ();
				music.clip = clips [1];
				music.Play ();
			}
			if (!alarm.isPlaying)
				alarm.Play ();
		} else {
			s.transform.GetChild (1).GetComponentInChildren<Image> ().color = new Color (103, 98, 98, 255);
			if (music.isPlaying && music.clip == clips [1]) {
				music.Stop ();
				music.clip = clips [0];
				music.Play ();
			}
			if (alarm.isPlaying)
				alarm.Stop ();
		}
	}
}