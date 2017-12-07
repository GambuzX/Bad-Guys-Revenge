using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour {
		
	public float width = 10f;
	public float height = 4f;
	public float speed = 5f;
	public float spawnDelay;
	public GameObject[] enemies;
	public GameObject levelTransition;
	public Sprite[] backgrounds;
	
	private GameObject minion;
	private bool right = true;
	private int enemyIndex = -1;
	private int backgroundIndex = 0;
	private Background background;
	private Animator anim;
	
	public bool spawning = true;
	
	float xmin;
	float xmax;
	
	void Start() {
		SpawnUntilFull();
		float limit = (width / 2);	
		float distance = transform.position.z - Camera.main.transform.position.z;
		Vector3 leftBoundary = Camera.main.ViewportToWorldPoint(new Vector3(0,0, distance));
		Vector3 rightBoundary = Camera.main.ViewportToWorldPoint(new Vector3(1,0, distance));
		xmin = leftBoundary.x + limit;
		xmax = rightBoundary.x - limit;
		background = GameObject.FindObjectOfType<Background>();
		anim = GameObject.Find("GokuShooter").GetComponent<Animator>();
	}
	
	void Update() {
		if (!spawning) { 
			if (right) {
				this.transform.position += new Vector3(speed * Time.deltaTime, 0f, 0f);
				if (this.transform.position.x >= xmax) {
					right = false;
					}
				}
			else {
				this.transform.position -= new Vector3(speed * Time.deltaTime, 0f, 0f);
				if (this.transform.position.x <= xmin) {
					right = true;
				}
			}
			if (AllEnemiesDead()) {
				if (backgroundIndex <= 7) {
					background.GetComponent<SpriteRenderer>().sprite = backgrounds[backgroundIndex];
					background.GetComponent<Animator>().Play("Fade");
					spawning = true;
					backgroundIndex++;
					enemyIndex++;
					SpawnUntilFull();
					Instantiate (levelTransition, levelTransition.transform.position, levelTransition.transform.rotation);
					anim.Play ("Dash");
					}
				else {
					background.GetComponent<SpriteRenderer>().sprite = backgrounds[8];
					spawning = true;
					SpawnRandom();
					anim.Play ("Dash");
					}
				}
			}
	}
	
	public bool AllEnemiesDead(){
		foreach(Transform Position in transform) {
			if(Position.childCount > 0) {
				return false; 
				}
			}
			return true;
	}
	
	void SpawnUntilFull() {
		if (spawning) {
			Transform freePosition = NextFreePosition();
			if (freePosition) {
				minion = Instantiate(enemies[enemyIndex], freePosition.position, Quaternion.identity) as GameObject;
				minion.transform.parent = freePosition;
				}
			if (NextFreePosition()) {
				Invoke ("SpawnUntilFull", spawnDelay);
				}
			else {
				spawning = false;
			}
		}
	}
	
	void SpawnRandom() {
		if (spawning) {
			Transform freePosition = NextFreePosition();
			if (freePosition) {
				minion = Instantiate(enemies[Random.Range(0, 7)], freePosition.position, Quaternion.identity) as GameObject;
				minion.transform.parent = freePosition;
			}
			if (NextFreePosition()) {
				Invoke ("SpawnRandom", spawnDelay);
			}
			else {
				spawning = false;
			}
		}
	}
	
	Transform NextFreePosition() {
		foreach(Transform pos in transform) {
			if(pos.childCount == 0) {
				return pos;
			}
		}
		return null;
	}
	
	public void OnDrawGizmos() {
		Gizmos.DrawWireCube(transform.position, new Vector3(width, height, 0f));
	}
}
