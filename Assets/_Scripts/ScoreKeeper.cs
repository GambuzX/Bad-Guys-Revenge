using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreKeeper : MonoBehaviour {
	
	private Text textBox;
	
	public static int score;
	
	void Start() {
		textBox = GetComponent<Text>();
		Reset();
	}

	public void Score(int points){
		score += points;
		textBox.text = score.ToString ();	
	}

	public void Reset(){
		score = 0;
		textBox.text = score.ToString ();
	}
}
