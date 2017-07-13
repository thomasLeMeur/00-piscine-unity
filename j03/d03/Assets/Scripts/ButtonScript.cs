using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonScript : MonoBehaviour {

	Button myButton;
	GameObject tower = null;
	GameObject tmp;
	bool isClickable;
	bool isMenu = true;
	bool isClicked = false;
	static public List<GameObject> towers;
	MenuScript menu;
	MenuScript pause;
	MenuScript end;
	static int speed = 1;
	static public bool isWin = false;

	void Start () {
	
		towers = new List<GameObject>();
		isClickable = true;
		myButton = GetComponent<Button>();
		if (gameObject.name == "ExitButton")
			myButton.onClick.AddListener (exit);
		else if (gameObject.name == "StartButton")
			myButton.onClick.AddListener (start);
		else if (gameObject.name == "ActionButton")
			myButton.onClick.AddListener (action);
		else if (gameObject.name == "Speed")
			myButton.onClick.AddListener (Speed);
		else if (gameObject.name == "Slow")
			myButton.onClick.AddListener (Slow);
		else if (gameObject.name == "ContinueButton") {
			myButton.onClick.AddListener (Continue);
			menu = GameObject.Find ("Menu").GetComponent<MenuScript> ();
			pause = GameObject.Find ("Pause").GetComponent<MenuScript> ();
			end = GameObject.Find ("End").GetComponent<MenuScript> ();
		} else if (gameObject.name == "FalseExitButton") {
			myButton.onClick.AddListener (FalseExit);
			menu = GameObject.Find ("Menu").GetComponent<MenuScript> ();
			pause = GameObject.Find ("Pause").GetComponent<MenuScript> ();
			end = GameObject.Find ("End").GetComponent<MenuScript> ();
		}
		else if (gameObject.name == "ReturnButton") {
			myButton.onClick.AddListener (Return);
			menu = GameObject.Find ("Menu").GetComponent<MenuScript> ();
			pause = GameObject.Find ("Pause").GetComponent<MenuScript> ();
			end = GameObject.Find ("End").GetComponent<MenuScript> ();
		}else if (gameObject.name == "Image") {
			if (gameObject.GetComponent<Image> ().sprite.name.IndexOf ("gatling") > -1)
				tower = GameObject.Find ("gatling_1");
			else if (gameObject.GetComponent<Image> ().sprite.name.IndexOf ("rocket") > -1)
				tower = GameObject.Find ("rocket_1");
			else if (gameObject.GetComponent<Image> ().sprite.name.IndexOf ("canon") > -1)
				tower = GameObject.Find ("canon_1");
			if (gameManager.gm.playerEnergy < tower.GetComponent<towerScript> ().energy)
				isClickable = false;
			isMenu = false;
		}
	}
	
	void Update () {
		if (!isMenu && tower != null) {
			if (isClickable == true && gameManager.gm.playerEnergy < tower.GetComponent<towerScript> ().energy) {
				isClickable = false;
				gameObject.GetComponent<Button> ().interactable = false;
				gameObject.GetComponent<Image> ().color = new Color (0, 0, 0);
			}
			else if (isClickable == false && gameManager.gm.playerEnergy >= tower.GetComponent<towerScript> ().energy) {
				isClickable = true;
				gameObject.GetComponent<Button> ().interactable = true;
				gameObject.GetComponent<Image> ().color = new Color (255, 255, 255);
			}
		}
	}

	void exit() {
		Application.Quit();
	}

	void start() {
		Application.LoadLevel ("ex01");
	}

	void Continue() {
		if (pause != null)
			pause.Continue (menu, pause, true);
	}

	void FalseExit() {
		if (pause != null && end != null)
			pause.Continue (end, pause, false);
	}

	void Return() {
		if (end != null)
			end.Continue (menu, end, true);
	}

	void action() {
		if (!isWin) {
			Application.LoadLevel (Application.loadedLevel);
		} else {
			Application.LoadLevel ((Application.loadedLevel + 1 >= 2) ? 2 : Application.loadedLevel);
		}
	}

	void Speed() {
		if (gameManager.gm.playerHp > 0) {
			speed += 1;
			if (speed > 20)
				speed = 20;
			gameManager.gm.changeSpeed (speed);
		}
	}

	void Slow() {
		if (gameManager.gm.playerHp > 0) {
			speed -= 1;
			if (speed < 0)
				speed = 0;
			gameManager.gm.changeSpeed (speed);
		}
	}

	void OnMouseDown() {
		if (!isMenu && isClickable) {
			isClicked = true;
			tmp = Instantiate (tower, transform.position, Quaternion.identity);
			tmp.GetComponent<Rigidbody2D> ().simulated = false;
		}
	}

	void OnMouseUp() {
		if (!isMenu && isClickable) {
			isClicked = false;
			RaycastHit2D hit = Physics2D.Raycast (Camera.main.ScreenToWorldPoint (Input.mousePosition), Vector2.zero);
			if (hit && hit.collider.gameObject.name.IndexOf ("empty") > -1) {
				tmp.transform.position = hit.transform.position + new Vector3 (0, 0, 80f);
				foreach (GameObject tow in towers) {
					if (tow.transform.position == tmp.transform.position) {
						GameObject.Destroy (tmp);
						return;
					}
				}
				tmp.GetComponent<towerScript> ().range = tower.GetComponent<towerScript> ().range;
				tmp.GetComponent<Rigidbody2D> ().simulated = true;
				towers.Add (tmp);
				gameManager.gm.playerEnergy -= tmp.GetComponent<towerScript> ().energy;
			} else
				GameObject.Destroy (tmp);
		}
	}

	void OnMouseDrag() {
		if (!isMenu && isClickable) {
			tmp.transform.position = (Camera.main.ScreenToWorldPoint(Input.mousePosition)) + new Vector3(0, 0, 100f);
		}
	}
}
