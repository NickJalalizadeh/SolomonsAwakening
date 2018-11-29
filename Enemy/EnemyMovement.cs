using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour
{
    Transform player;               // Reference to the player's position.
	PlayerHealth playerHealth;      // Reference to the player's health.
    EnemyHealth enemyHealth;        // Reference to this enemy's health.
    UnityEngine.AI.NavMeshAgent nav;               // Reference to the nav mesh agent.

	public Transform destinationPlayer;

    void Awake ()
    {
        // Set up the references.
        player = GameObject.FindGameObjectWithTag ("Player").transform;
        playerHealth = player.GetComponent <PlayerHealth> ();
        enemyHealth = GetComponentInChildren <EnemyHealth> ();
        nav = GetComponent <UnityEngine.AI.NavMeshAgent> ();
    }

	void Start()
	{
		int rand = Random.Range (0, 4);
		if (rand == 0)
			destinationPlayer = player;
		else if (rand == 1)
			destinationPlayer = GameObject.FindGameObjectWithTag ("PlayerRight").transform;
		else if (rand == 2)
			destinationPlayer = GameObject.FindGameObjectWithTag ("PlayerLeft").transform;
		else
			destinationPlayer = GameObject.FindGameObjectWithTag ("PlayerBack").transform;
	}

    void Update ()
    {
        // If the enemy and the player have health left...
		if(enemyHealth.currentHealth > 0 && playerHealth.currentHealth > 0)
        {
            // ... set the destination of the nav mesh agent to the player.
            nav.SetDestination (destinationPlayer.position);
        }
        // Otherwise...
       	else
        {
            // ... disable the nav mesh agent.
            nav.enabled = false;
        }
    }
}