using UnityEngine;
using System.Collections;

public class EnergyTrack : MonoBehaviour {
	
	void Update() {
		this.transform.position = this.transform.parent.position;
	}
}
