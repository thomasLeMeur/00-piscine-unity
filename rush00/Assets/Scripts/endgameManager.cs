using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class endgameManager : MonoBehaviour {

	public Image mainImage;
	public Text mainText;
	public Button button1;
	public Button button2;
	public GameObject endgameMenu;
	public AudioClip audioVictory;
	public AudioClip audioGameOver;
	private AudioClip _audio;
	private bool _victory;
	private bool _gameOver;
	private bool isPlayingEnd;

	void Start () {
		_audio = GetComponent<AudioSource> ().clip;
		_victory = false;
		_gameOver = false;
		isPlayingEnd = false;
		Button b1 = button1.GetComponent<Button>();
		b1.onClick.AddListener(button1Action);
		Button b2 = button2.GetComponent<Button>();
		b2.onClick.AddListener(button2Action);
	}

	void Update () {
		if (player.hasWon || player.nbEnemies <= 0) {
			_victory = true;
		}
		_gameOver = player.isDead;
		if (_victory && !isPlayingEnd) {
			mainText.text = "victory";
			isPlayingEnd = true;
			GetComponent<AudioSource> ().Stop ();
			GetComponent<AudioSource> ().clip = audioVictory;
			GetComponent<AudioSource> ().Play ();
		} else if (_gameOver && !isPlayingEnd) {
			mainText.text = "game over";
			isPlayingEnd = true;
			GetComponent<AudioSource> ().Stop ();
			GetComponent<AudioSource> ().clip = audioGameOver;
			GetComponent<AudioSource> ().Play ();
			mainImage.gameObject.SetActive (false);
		}
		if (_victory || _gameOver) {
			Text text1 = button1.GetComponentInChildren (typeof(Text)) as Text;
			text1.text = "restart";
			Text text2 = button2.GetComponentInChildren (typeof(Text)) as Text;
			text2.text = "back to menu";
			endgameMenu.gameObject.SetActive (true);
		}
	}

	void button1Action() {
		endgameMenu.gameObject.SetActive (false);
		player.weapon = null;
		player.hasWon = false;
		player.isDead = false;
		Application.LoadLevel(Application.loadedLevel);
	}

	void button2Action() {
		endgameMenu.gameObject.SetActive (false);
		player.weapon = null;
		player.hasWon = false;
		player.isDead = false;
		Application.LoadLevel(0);
	}

}
