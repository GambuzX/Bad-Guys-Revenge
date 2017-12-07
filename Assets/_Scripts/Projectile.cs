using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

	public int damage;
	private EnemySpawner spawner;
	
	void Start() {
		spawner = GameObject.FindObjectOfType<EnemySpawner>();
		}
	
	void Update () {
		if (spawner.spawning) {
			Destroy (gameObject);
			}
		if (this.tag == "GokuProjectile" && this.transform.position.y > 10) {
			Destroy(gameObject);
			}
		else if (this.tag == "EnemyProjectile" && this.transform.position.y < -2) {
			Destroy(gameObject);
			}
	}
	
	void OnTriggerEnter2D(Collider2D col) {
		if (this.tag == "GokuProjectile" && col.tag == "Enemy") {
			Destroy(gameObject);
			}
		else if (this.tag == "EnemyProjectile" && col.tag == "Goku") {
			Destroy(gameObject);
			}
		}
}
