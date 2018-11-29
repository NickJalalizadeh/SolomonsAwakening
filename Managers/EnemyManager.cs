using UnityEngine;
using UnityEngine.AI;

public class EnemyManager : MonoBehaviour
{
	//TODO: fix enemies being spawned outside navmesh 
    public PlayerHealth playerHealth;       // Reference to the player's heatlh.
    public GameObject zombie01, zombie02, skeleton, skeletonGameObject;                // The enemy prefab to be spawned.
	public float zombie01SpawnTime, zombie02SpawnTime, skeletonSpawnTime;            // How long between each spawn.
	//public int zombie01Max, zombie02Max, skeletonMax;
    public Transform[] spawnPoints;         // An array of the spawn points this enemy can spawn from.
	public Transform spawnPosition;

	float initialPosition;
	int randomRotation, randomPosition;
	float zombie01Speed, zombie02Speed, skeletonSpeed;
	float zombie01Rate, zombie02Rate, skeletonRate;
	GameObject[] zombie01Count, zombie02Count, skeletonCount;
	NavMeshAgent zombie01Agent, zombie02Agent, skeletonAgent;
	ZombieAttack zombie01Attack, zombie02Attack;
	SkeletonAttack skeletonAttack;

    void Awake ()
    {
		zombie01Agent = zombie01.GetComponent<NavMeshAgent> ();
		zombie02Agent = zombie02.GetComponent<NavMeshAgent> ();
		skeletonAgent = skeleton.GetComponent<NavMeshAgent> ();
		zombie01Attack = zombie01.GetComponent<ZombieAttack> ();
		zombie02Attack = zombie02.GetComponent<ZombieAttack> ();
		skeletonAttack = skeletonGameObject.GetComponent<SkeletonAttack> ();

		zombie01Speed = 2f;
		zombie02Speed = 2.5f;
		skeletonSpeed = 2f;
		zombie01Rate = 1f;
		zombie02Rate = .75f;
		skeletonRate = 2f;

		InstantiateZombie01 (1);
    }

	public void InstantiateZombie01(int currentLevel) 
	{ 
		zombie01SpawnTime = 8 * Mathf.Pow(.2f, .05f * (currentLevel - 1));
		//zombie01Max = (int)Mathf.Round(8 + currentLevel * Mathf.Log10(currentLevel));	
		if (currentLevel % 5 == 0) {
			zombie01Speed += 0.5f;
			zombie01Rate *= .75f;		
		}
		zombie01Attack.timeBetweenAttacks = zombie01Rate;

		InvokeRepeating ("SpawnZombie01", zombie01SpawnTime, zombie01SpawnTime); 
	}
	public void InstantiateZombie02(int currentLevel)
	{ 
		zombie02SpawnTime = 12 * Mathf.Pow(.2f, .05f * (currentLevel - 1));
		//zombie02Max = (int)Mathf.Round(4 + 0.5f * currentLevel * Mathf.Log10(currentLevel));
		if (currentLevel % 5 == 0) {
			zombie02Speed += 0.5f;
			zombie02Rate *= .75f;		
		}
		zombie02Attack.timeBetweenAttacks = zombie02Rate;

		InvokeRepeating ("SpawnZombie02", zombie02SpawnTime, zombie02SpawnTime); 
	}
	public void InstantiateSkeleton(int currentLevel)
	{ 
		skeletonSpawnTime = 20 * Mathf.Pow(.2f, .05f * (currentLevel - 1));
		//skeletonMax = (int)Mathf.Round(0.333f * currentLevel * Mathf.Log10(currentLevel));
		if (currentLevel % 5 == 0) {
			skeletonSpeed += 0.5f;
			skeletonRate *= .667f;		
		}
		skeletonAttack.timeBetweenAttacks = skeletonRate;

		InvokeRepeating ("SpawnSkeleton", skeletonSpawnTime, skeletonSpawnTime); 
	}

    void SpawnZombie01 ()
    {
        if(playerHealth.currentHealth <= 0f)
            return;
        	
		zombie01Count = GameObject.FindGameObjectsWithTag ("Enemy1");

		//if (zombie01Count.Length < zombie01Max) {
			CalculateRandom ();

			zombie01Agent.speed = Random.Range(zombie01Speed, zombie01Speed + 1f);

			spawnPoints [0].position = spawnPosition.position + spawnPosition.forward * (initialPosition + randomPosition);

			Instantiate (zombie01, spawnPoints [0].position, spawnPoints [0].rotation);
			/*if (rand1 < 2)
				Instantiate (zombie01, spawnPoints [0].position, spawnPoints [0].rotation);
			else if (rand1 == 2) {
				Instantiate (zombie01, spawnPoints [0].position, spawnPoints [0].rotation);
				Instantiate (zombie01, spawnPoints [0].position, spawnPoints [0].rotation);
			}*/
		//}
    }

	void SpawnZombie02()
	{
		if(playerHealth.currentHealth <= 0f)
			return;
		
		zombie02Count = GameObject.FindGameObjectsWithTag ("Enemy2");

		//if (zombie02Count.Length < zombie02Max) {
			CalculateRandom ();

			zombie02Agent.speed = Random.Range(zombie02Speed, zombie02Speed + 1f);
			
			spawnPoints [1].position = spawnPosition.position + spawnPosition.forward * (initialPosition + randomPosition);

			Instantiate (zombie02, spawnPoints [1].position, spawnPoints [1].rotation);
			/*if (rand2 == 2)
				Instantiate (zombie02, spawnPoints [1].position, spawnPoints [1].rotation);
			else if (rand2 == 3) {
				Instantiate (zombie02, spawnPoints [1].position, spawnPoints [1].rotation);
				Instantiate (zombie02, spawnPoints [1].position, spawnPoints [1].rotation);
			}*/
		//}
	}

	void SpawnSkeleton()
	{
		if(playerHealth.currentHealth <= 0f)
			return;

		skeletonCount = GameObject.FindGameObjectsWithTag ("Enemy3");

		//if (skeletonCount.Length < skeletonMax) {
			CalculateRandom ();
			
			skeletonAgent.speed = Random.Range(skeletonSpeed, skeletonSpeed + 1f);
			
			spawnPoints [2].position = spawnPosition.position + spawnPosition.forward * (initialPosition + randomPosition);

			Instantiate (skeleton, spawnPoints [2].position, spawnPoints [2].rotation);
			/*if (rand3 == 1)
				Instantiate (zombie02, spawnPoints [1].position, spawnPoints [1].rotation);*/
		//}
	} 

	void CalculateRandom()
	{
		if (Random.Range(0,3) < 2)
			randomRotation = Random.Range (60, 300);
		else
			randomRotation = Random.Range (300, 420);

		randomPosition = Random.Range (0, 15);

		if (randomRotation >= 300)
			initialPosition = 25 + .333f * Mathf.Abs (randomRotation - 360);
		else
			initialPosition = 10;

		spawnPosition.localRotation = Quaternion.Euler (0, randomRotation, 0);
	}
}