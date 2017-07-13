using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayersManager : MonoBehaviour {

	public List<PlayerScript> players = new List<PlayerScript> ();
	public bool isAttack = false;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonUp (0) && !Input.GetKey (KeyCode.LeftControl) && players.Count > 0) {
			MusicManager.instance.Play (MusicManager.instance.source1);
			if (isAttack) {
				isAttack = false;
			}
			else {
				foreach (PlayerScript player in players) {
					player.updatePlayer (Input.mousePosition, false);
				}
			}
		} else if (Input.GetMouseButtonDown (1)) {
			players.Clear ();
		}
	}

	public void Add (PlayerScript other) {
		bool clue = true;
		foreach (PlayerScript player in players)
			if (other == player)
				clue = false;
		if (clue == true)
			players.Add (other);
	}

	public void Remove(PlayerScript other = null) {
		if (other == null)
			players.Clear ();
		else {
			foreach (PlayerScript player in players)
				if (other == player)
					players.Remove (other);
		}
	}

	public void Attack(TownScript ennemy)
	{
		isAttack = true;
		foreach (PlayerScript player in players) {
			player.updatePlayer (Input.mousePosition, true, ennemy);
		}
	}

	public void Kill(TownScript ennemy)
	{
		foreach (PlayerScript player in players) {
			player.Kill ();
		}
		ennemy.Kill();
	}
}
