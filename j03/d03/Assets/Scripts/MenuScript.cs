using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour {

	private Text playerHp;
	private Text playerEnergy;
	private Text[] tower0 = new Text[4];
	private Text[] tower1 = new Text[4];
	private Text[] tower2 = new Text[4];
	private bool isFinished = false;

	static MenuScript menu;
	static MenuScript pause;
	static MenuScript end;
	static MenuScript last;
	static ButtonScript action;
	static GameObject[] spawners;

	public enum InfosTower
	{
		energy,
		damage,
		range,
		firaRate
	};

	// Use this for initialization
	void Start () {
		spawners = GameObject.FindGameObjectsWithTag ("spawner");
		action = GameObject.Find ("RealEnd/ActionButton").GetComponent<ButtonScript> ();
		menu = GameObject.Find ("Menu").GetComponent<MenuScript> ();
		pause = GameObject.Find ("Pause").GetComponent<MenuScript> ();
		end = GameObject.Find ("End").GetComponent<MenuScript> ();
		last = GameObject.Find ("RealEnd").GetComponent<MenuScript> ();
		if (this == pause)
			pause.transform.position += new Vector3 (1000, 0, 0);
		else if (this == end)
			end.transform.position += new Vector3 (1000, 0, 0);
		else if (this == last) {
			last.gameObject.GetComponent<Canvas> ().planeDistance = 0;
		}

		playerHp = GameObject.Find ("MenuPlayer/LifePlayer/Text").GetComponent<Text> ();
		playerEnergy = GameObject.Find ("MenuPlayer/EnergyPlayer/Text").GetComponent<Text> ();
		tower0[0] = GameObject.Find ("MenuTowers/CanvasTower1/InfosTower/EnergyTower/Text").GetComponent<Text> ();
		tower0[1] = GameObject.Find ("MenuTowers/CanvasTower1/InfosTower/DamageTower/Text").GetComponent<Text> ();
		tower0[2] = GameObject.Find ("MenuTowers/CanvasTower1/InfosTower/RangeTower/Text").GetComponent<Text> ();
		tower0[3] = GameObject.Find ("MenuTowers/CanvasTower1/InfosTower/WaitTower/Text").GetComponent<Text> ();
		tower1[0] = GameObject.Find ("MenuTowers/CanvasTower2/InfosTower/EnergyTower/Text").GetComponent<Text> ();
		tower1[1] = GameObject.Find ("MenuTowers/CanvasTower2/InfosTower/DamageTower/Text").GetComponent<Text> ();
		tower1[2] = GameObject.Find ("MenuTowers/CanvasTower2/InfosTower/RangeTower/Text").GetComponent<Text> ();
		tower1[3] = GameObject.Find ("MenuTowers/CanvasTower2/InfosTower/WaitTower/Text").GetComponent<Text> ();
		tower2[0] = GameObject.Find ("MenuTowers/CanvasTower0/InfosTower/EnergyTower/Text").GetComponent<Text> ();
		tower2[1] = GameObject.Find ("MenuTowers/CanvasTower0/InfosTower/DamageTower/Text").GetComponent<Text> ();
		tower2[2] = GameObject.Find ("MenuTowers/CanvasTower0/InfosTower/RangeTower/Text").GetComponent<Text> ();
		tower2[3] = GameObject.Find ("MenuTowers/CanvasTower0/InfosTower/WaitTower/Text").GetComponent<Text> ();


		towerScript gat1 = GameObject.Find ("gatling_1").GetComponent<towerScript> ();
		towerScript roc1 = GameObject.Find ("rocket_1").GetComponent<towerScript> ();
		towerScript can1 = GameObject.Find ("canon_1").GetComponent<towerScript> ();


		playerHp.text = gameManager.gm.playerHp.ToString();
		playerEnergy.text = gameManager.gm.playerEnergy.ToString();
		tower0[0].text = gat1.energy.ToString();
		tower0[1].text = gat1.damage.ToString();
		tower0[2].text = gat1.range.ToString();
		tower0[3].text = gat1.fireRate.ToString();
		tower1[0].text = roc1.energy.ToString();
		tower1[1].text = roc1.damage.ToString();
		tower1[2].text = roc1.range.ToString();
		tower1[3].text = roc1.fireRate.ToString();
		tower2[0].text = can1.energy.ToString();
		tower2[1].text = can1.damage.ToString();
		tower2[2].text = can1.range.ToString();
		tower2[3].text = can1.fireRate.ToString();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Escape) && this == pause && pause.transform.position.x > 10) {
			gameManager.gm.pause (true);
			menu.transform.position += new Vector3 (1000, 0, 0);
			pause.transform.position -= new Vector3 (1000, 0, 0);
		}
		else if (this == menu) {
			if (isFinished == false && gameManager.gm.playerHp <= 0) {
				isFinished = true;
			}
			else {
				playerHp.text = (gameManager.gm.playerHp <= 0) ? "0" : gameManager.gm.playerHp.ToString ();
				playerEnergy.text = gameManager.gm.playerEnergy.ToString ();
			}
		}
	}

	public void Continue(MenuScript come, MenuScript go, bool cont) {
		come.transform.position -= new Vector3 (1000, 0, 0);
		go.transform.position += new Vector3 (1000, 0, 0);
		if (cont)
			gameManager.gm.pause (false);
	}

	public void Finish (bool isWin) {
		if (last != null) {
			foreach (GameObject to in ButtonScript.towers)
				GameObject.Destroy (to);
			GameObject t = GameObject.Find ("map00");
			if (t == null)
				t = GameObject.Find ("map02");
			t.transform.localScale = new Vector3 (0, 0, 0);
			last.gameObject.GetComponent<Canvas> ().planeDistance = 100;
			last.gameObject.transform.position += new Vector3 (0, 0, -1000);
			string[] tab = {"A", "B", "C", "D", "E", "F", "G", "H", "I", "J"};
			int index = (gameManager.gm.playerHp + gameManager.gm.playerEnergy / 5) / 10;
			if (index > 9)
				index = 9;
			GameObject.Find ("RealEnd/Text").GetComponent<Text> ().text = "Your range is " + tab[index] + "_" + gameManager.gm.score.ToString();
			ButtonScript.isWin = isWin;
			if (isWin) {
				GameObject.Find ("RealEnd/ActionButton/Text").GetComponent<Text> ().text = "Next Level";
			} else {
				GameObject.Find ("RealEnd/ActionButton/Text").GetComponent<Text> ().text = "Retry";
			}
		}
	}
}
