using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class weaponDisplay : MonoBehaviour {

	public Text weaponName;
	public Text weaponAmmo;
	private string _spriteName;
	private string _weaponType;
	private GameObject _weapon;

	void Start () {
		_weapon = player.weapon;
		weaponName.text = "no weapon";
		weaponAmmo.text = "-";
	}
	
	void Update () {
		_weapon = player.weapon;
		if (_weapon == null) {
			weaponName.text = "no weapon";
			weaponAmmo.text = "-";
		} else {
			_spriteName = _weapon.GetComponent<SpriteRenderer>().sprite.name;
			weaponName.text = _spriteName.Split('-')[1];
			_weaponType = _spriteName.Split('-')[0];
			switch (_weaponType)
			{
			case "1": case "6":
				 weaponAmmo.text = _weapon.GetComponent<weaponScript>().ammo.ToString() + "   " + "rifle";
				break;
			case "5": case "12":
				 weaponAmmo.text = "inf";
				break;
			case "8":
				 weaponAmmo.text = _weapon.GetComponent<weaponScript>().ammo.ToString() + "   " + "auto";
				break;
			default:
				 weaponAmmo.text = _weapon.GetComponent<weaponScript>().ammo.ToString() + "   " + "slow";
				break;
			}
		}
	}
}
