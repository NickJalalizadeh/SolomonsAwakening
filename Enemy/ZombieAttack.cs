using UnityEngine;
using System.Collections;

public class ZombieAttack : MonoBehaviour
{
    public float timeBetweenAttacks = 1.0f;     // The time in seconds between each attack.
    public int attackDamage = 10;               // The amount of health taken away per attack.
	public AudioClip gruntSound, attackSound;

    Animator anim;                              // Reference to the animator component.
    GameObject player;                          // Reference to the player GameObject.
    PlayerHealth playerHealth;                  // Reference to the player's health.
    EnemyHealth enemyHealth;                    // Reference to this enemy's health.
	AudioSource zombieAudio;
    bool playerInRange;                         // Whether player is within the trigger collider and can be attacked.
	bool attackType;
    float timer, timer2;                                // Timer for counting up to the next attack.
	int rand1, rand2;

    void Awake ()
    {
        // Setting up the references.
        player = GameObject.FindGameObjectWithTag ("Player");
        playerHealth = player.GetComponent <PlayerHealth> ();
        enemyHealth = GetComponent<EnemyHealth>();
        anim = GetComponent <Animator> ();
		zombieAudio = gameObject.GetComponent<AudioSource> ();
    }

	void Start() {
		rand1 = Random.Range (5, 10);
		rand2 = Random.Range (0, 4);
	}

    void OnTriggerEnter (Collider other)
    {
        // If the entering collider is the player...
        if(other.gameObject == player)
        {
            // ... the player is in range.
            playerInRange = true;
        }
    }


    void OnTriggerExit (Collider other)
    {
        // If the exiting collider is the player...
        if(other.gameObject == player)
        {
            // ... the player is no longer in range.
            playerInRange = false;
        }
    }


    void Update ()
    {
        // Add the time since Update was last called to the timer.
        timer += Time.deltaTime;
		timer2 += Time.deltaTime;

        // If the timer exceeds the time between attacks, the player is in range and this enemy is alive...
        if(timer >= timeBetweenAttacks && playerInRange && enemyHealth.currentHealth > 0)
        {
            // ... attack.
            Attack ();
        }

        // If the player has zero or less health...
        if(playerHealth.currentHealth <= 0)
        {
            // ... tell the animator the player is dead.
            anim.SetTrigger ("PlayerDead");
        }

		if (enemyHealth.currentHealth > 0 && timer2 >= rand1 && rand2 == 3) {
			timer2 = 0;
			zombieAudio.clip = gruntSound;
			zombieAudio.Play ();
		}
    }


    void Attack ()
    {
        // Reset the timer.
        timer = 0f;
		attackType = !attackType; 

		anim.SetTrigger ("Attack");
		anim.SetBool ("attackType", attackType);

		if (rand2 < 2 && !zombieAudio.isPlaying && Random.Range(0,2) == 0) {
			zombieAudio.clip = attackSound;
			zombieAudio.Play ();
		}

        // If the player has health to lose...
        if(playerHealth.currentHealth > 0)
        {
            // ... damage the player.
            playerHealth.TakeDamage (attackDamage);
        }
    }
}