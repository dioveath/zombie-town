using UnityEngine;



public class Spawner : MonoBehaviour {

	public Enemy[] enemies;
    public Transform[] spawnPlaces;
	public Wave[] waves;
	Wave currentWave;
	int currentWaveIndex = -1;

	public float numberOfEnemiesToSpawn;
	public float numberOfEnemiesAlive;
	public float nextSpawnTime;

	Player player;

    public System.Action OnEnemyKilled;
	public System.Action OnNextWaveEvent;


	public void Awake(){
		player = FindObjectOfType<Player> ();
	}

	void Start(){
		NextWave ();
	}

	void Update(){
		if (Time.time > nextSpawnTime && numberOfEnemiesToSpawn > 0 && player != null) {
			int randomIndex = Random.Range (0, enemies.GetLength (0));
            int randomIndex2 = Random.Range(0, spawnPlaces.GetLength(0));
			Enemy newEnemy = Instantiate (enemies[randomIndex], spawnPlaces[randomIndex2].position, Quaternion.identity);
			newEnemy.target = player;
			newEnemy.OnDeath += OnEnemyDeath;
			newEnemy.SetCharacteristics (currentWave.enemySpeed, currentWave.enemyAngularSpeed, currentWave.enemyFieldOfView, currentWave.enemyDamage, currentWave.enemyHealth);
			numberOfEnemiesToSpawn--;
			nextSpawnTime = Time.time + currentWave.spawnRate;
		}
	}

	private void NextWave(){
		if (currentWaveIndex >= waves.Length - 1)
			return;
		currentWaveIndex++;
		currentWave = waves [currentWaveIndex];
		numberOfEnemiesToSpawn = currentWave.enemyCount;
		numberOfEnemiesAlive = currentWave.enemyCount;
		if (OnNextWaveEvent != null) {
			OnNextWaveEvent();
		}
	}

	public int GetCurrentWaveIndex(){
		return currentWaveIndex;
	}

	[System.Serializable]
	public class Wave {
		public float enemyCount = 10;
		public float spawnRate = .5f;
		public float enemySpeed = 6f;
		public float enemyAngularSpeed = 360;
		public float enemyFieldOfView = 30;
		public float enemyDamage = 100;
		public float enemyHealth = 3;
	}

	public void OnEnemyDeath(){
		numberOfEnemiesAlive--;
        if(OnEnemyKilled != null) {
            OnEnemyKilled();
        }
		if (numberOfEnemiesAlive == 0) { 
			//each time an enemy dies we check to see if there is any alive so next wave can be called
			NextWave ();
		}
	}
}
