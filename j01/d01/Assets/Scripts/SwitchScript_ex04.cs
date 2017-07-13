using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchScript_ex04 : MonoBehaviour {

	private GameObject porteThomas;
	private GameObject porteClaire;
	private GameObject porteJohn;
	private GameObject plat;
	private Animator tmp;

	void Start() {
		porteThomas = GameObject.FindGameObjectWithTag ("porteThomas");
		porteClaire = GameObject.FindGameObjectWithTag ("porteClaire");
		porteJohn = GameObject.FindGameObjectWithTag ("porteJohn");
		plat = GameObject.FindGameObjectWithTag ("plateform");
	}

	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D collider) {
		if (tag == "porte") {
			if (collider.gameObject.layer == 8) {
				porteThomas.GetComponent<SpriteRenderer> ().color *= new Color (1, 1, 1, 0);
				porteThomas.layer = 18;
			} else if (collider.gameObject.layer == 9) {
				porteJohn.GetComponent<SpriteRenderer> ().color *= new Color (1, 1, 1, 0);
				porteJohn.layer = 18;
			} else if (collider.gameObject.layer == 10) {
				porteClaire.GetComponent<SpriteRenderer> ().color *= new Color (1, 1, 1, 0);
				porteClaire.layer = 18;
			}
		} else {
			if (collider.gameObject.layer == 8) {
				plat.GetComponent<SpriteRenderer> ().color = porteThomas.GetComponent<SpriteRenderer>().color;
				plat.layer = 14;
			} else if (collider.gameObject.layer == 9) {
				plat.GetComponent<SpriteRenderer> ().color = porteJohn.GetComponent<SpriteRenderer>().color;
				plat.layer = 15;
			} else if (collider.gameObject.layer == 10) {
				plat.GetComponent<SpriteRenderer> ().color = porteClaire.GetComponent<SpriteRenderer>().color;
				plat.layer = 16;
			}
		}
	}

	void OnTriggerExit2D(Collider2D collider) {
		if (tag == "porte") {
			if (collider.gameObject.layer == 8) {
				porteThomas.GetComponent<SpriteRenderer> ().color += new Color (0, 0, 0, 1);
				porteThomas.layer = 19;
			} else if (collider.gameObject.layer == 9) {
				porteJohn.GetComponent<SpriteRenderer> ().color += new Color (0, 0, 0, 1);
				porteJohn.layer = 19;
			} else if (collider.gameObject.layer == 10) {
				porteClaire.GetComponent<SpriteRenderer> ().color += new Color (0, 0, 0, 1);
				porteClaire.layer = 19;
			}
		}
	}
}
