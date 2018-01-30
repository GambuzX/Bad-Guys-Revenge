using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

	public int HP;
	
	public AudioClip HitSound;
	public AudioClip DeathSound;
	public AudioClip ArrivalSound;
	public AudioClip projectileSound;
	public GameObject Projectile;
	public GameObject HPCapsule;
	public float FireRate;
	public float FireSpeed;
	public int points;
	
	private EnemySpawner enemySpawner;
	private ScoreKeeper scoreKeeper;
	
	void OnTriggerEnter2D(Collider2D col) {
		if (col.tag == "GokuProjectile") {
			Projectile missile = col.gameObject.GetComponent<Projectile>();
			HP -= missile.damage;
			HP = Mathf.Clamp (HP, 0 , 1000);
			if (HP > 0) {
				AudioSource.PlayClipAtPoint(HitSound, this.transform.position, 0.2f);
				if (HP == 1) {
					this.GetComponent<SpriteRenderer>().color = new Color(0.8f, 0.3f, 0.3f, 1f);
					}
				}
			else if (HP <= 0) {
				AudioSource.PlayClipAtPoint(DeathSound, this.transform.position, 0.3f);
				int chance = Random.Range(1, 4);
				if (chance == 1) {
					GameObject capsule = Instantiate(HPCapsule, this.transform.position, Quaternion.identity) as GameObject;
					capsule.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, -5f);
					}
				Destroy(gameObject);
				scoreKeeper.Score(points);
				}
			}
		}
	void Start() {
		enemySpawner = GameObject.FindObjectOfType<EnemySpawner>();
		scoreKeeper = GameObject.FindObjectOfType<ScoreKeeper>();
		AudioSource.PlayClipAtPoint(ArrivalSound, transform.position, 0.2f);
	}
	
	void Update () {
		if (enemySpawner.spawning) {
			CancelInvoke("ShootProjectile");
			}
		else { 
			float probability = Time.deltaTime * FireRate;
			if(Random.value < probability) {
				ShootProjectile();
				}
			}
	}

	void ShootProjectile() {
		GameObject projectile = Instantiate (Projectile, new Vector3(this.transform.position.x, this.transform.position.y - 0.5f, 0f), Projectile.transform.rotation) as GameObject;
		projectile.rigidbody2D.velocity = new Vector3 (0f, -FireSpeed, 0f);
		AudioSource.PlayClipAtPoint(projectileSound, this.transform.position, 0.15f);
	}
}
