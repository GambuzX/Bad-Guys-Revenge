using UnityEngine;
using System.Collections;

public class GokuShooter : MonoBehaviour {
	
	public float GokuSpeed;
	public float projectileSpeed;
	public float fireRate;
	public int HP;
	public GameObject EnergyBall;
	public GameObject EBallTrack;
	public AudioClip[] GokuSounds;
	public AudioClip Heal;
	
	private EnemySpawner enemySpawner;
	private ScoreKeeper scoreKeeper;
	private AudioSource sound;
	private bool playing = false;
		
	float xmin = 0.783f;
	float xmax = 15.233f;
	float limit = 1f;

	void Start() {
		float distance = this.transform.position.z - Camera.main.transform.position.z;
		Vector3 leftBoundary = Camera.main.ViewportToWorldPoint(new Vector3(0,0,distance));
		Vector3 rightBoundary = Camera.main.ViewportToWorldPoint(new Vector3(1,0,distance));
		xmin = leftBoundary.x + limit;
		xmax = rightBoundary.x - limit;
		enemySpawner = GameObject.FindObjectOfType<EnemySpawner>();
		scoreKeeper = GameObject.FindObjectOfType<ScoreKeeper>();
		sound = this.GetComponent<AudioSource>();
	}
	
	void Update() {
		MoveWithArrows();		
		ClampPosition();
		if (HP <= 0) {
			if (!playing) {			
				sound.Play();
				playing = true;
				}
			Destroy (this.GetComponent<SpriteRenderer>());
			Destroy (this.GetComponent<PolygonCollider2D>());
			if (!sound.isPlaying) { 
				Application.LoadLevel ("Lose");
				} 
			}
		if (enemySpawner.spawning || HP <= 0) {
			CancelInvoke("Fire");
			}
		else if (!enemySpawner.spawning && HP > 0) {
			ShootEnergyBall();
		}
	}
	
	void OnTriggerEnter2D(Collider2D col) {
		if (col.tag == "EnemyProjectile") {
			AudioSource.PlayClipAtPoint(GokuSounds[2], this.transform.position, 0.2f);
			Projectile missile = col.gameObject.GetComponent<Projectile>();
			HP -= missile.damage;
			HP = Mathf.Clamp (HP, 0, 1000);
			}
		else if (col.tag == "HPCapsule") {
			HP += 5;
			AudioSource.PlayClipAtPoint(Heal, this.transform.position, 0.4f);
			scoreKeeper.Score (50);
			Destroy (col.gameObject);
			}
		}
		
	void ClampPosition() {
		float newx = Mathf.Clamp (transform.position.x, xmin, xmax);
		transform.position = new Vector3(newx, transform.position.y, -3f);
	}
		
	void MoveByMouse() {
		float MousePositionInGM = (Input.mousePosition.x / Screen.width * 16);
		this.transform.position = new Vector3(Mathf.Clamp(MousePositionInGM, 0.783f, 15.233f), this.transform.position.y, -3f);
		}
	
	void MoveWithArrows() {
		if (Input.GetKey(KeyCode.RightArrow)) {
			this.transform.position += Vector3.right * GokuSpeed * Time.deltaTime;
		}
		else if (Input.GetKey(KeyCode.LeftArrow)) {
			this.transform.position += Vector3.left * GokuSpeed * Time.deltaTime;
		}
	}
	
	public void Fire() {
		GameObject EnergyShoot = Instantiate (EnergyBall, this.transform.position, EnergyBall.transform.rotation) as GameObject;
		EnergyShoot.rigidbody2D.velocity = new Vector2 (0f, projectileSpeed);
		AudioSource.PlayClipAtPoint(GokuSounds[0], this.transform.position, 0.3f);
		//AudioSource.PlayClipAtPoint(GokuSounds[1], this.transform.position, 0.2f);
		GameObject track = Instantiate(EBallTrack, transform.position, EBallTrack.transform.rotation) as GameObject;
		track.transform.parent = EnergyShoot.transform;
	}
	
	void ShootEnergyBall() {
		if (Input.GetKeyDown(KeyCode.Space)) { 
			InvokeRepeating("Fire", 0.000001f, fireRate);
		}
		if (Input.GetKeyUp(KeyCode.Space)) {
			CancelInvoke("Fire");
		}
	}

}