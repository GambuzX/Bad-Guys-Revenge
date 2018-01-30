using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HP : MonoBehaviour {

	public Text text;
	
	private GokuShooter player;
	
	void Start() {
		player = GameObject.FindObjectOfType<GokuShooter>();
		}
		
	void Update() {
		text.text = "HP: " + player.HP;
		}
	
}
