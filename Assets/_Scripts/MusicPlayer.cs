using UnityEngine;
using System.Collections;

public class MusicPlayer : MonoBehaviour {
	static MusicPlayer Instance = null;
	void Awake() { 
		if (Instance == null) { 
			Instance = this;
			GameObject.DontDestroyOnLoad(gameObject);
		}
		else { Destroy (gameObject); }
	}
}
