using UnityEngine;
using UnityEngine.AI;

public class EnemyHealth : MonoBehaviour
{
    public int startingHealth = 100;            // The amount of health the enemy starts the game with.
    public int currentHealth;                   // The current health the enemy has.
    public float sinkSpeed = 2.5f;              // The speed at which the enemy sinks through the floor when dead.
	public float onFireTime = 5f;					//Time for which the enemy is on fire
	public float timeBetweenFireDamage = 1f;
	public int onFireDamage = 3;
	public float enemyExp = 5f;
	public NavMeshAgent navMeshAgent;
	public PlayerMana playerMana;
	public AudioClip zombie1Death, zombie2Death, skeletonDeath;

    Animator anim;                              // Reference to the animator.
    AudioSource enemyAudio;                     // Reference to the audio source.
    CapsuleCollider capsuleCollider;            // Reference to the capsule collider.
	ParticleSystem onFirePS;             // Reference to the particle system that plays when the enemy is damaged.
	Light onFireLight;

    bool isDead;                                // Whether the enemy is dead.
    bool isSinking;                             // Whether the enemy has started sinking through the floor.
	float timer1, timer2;

    void Awake ()
    {
        // Setting up the references.
        anim = GetComponent <Animator> ();
        enemyAudio = GetComponent <AudioSource> ();
        capsuleCollider = GetComponent <CapsuleCollider> ();
		onFirePS = GetComponentInChildren <ParticleSystem> ();
		onFireLight = GetComponentInChildren <Light> ();

        // Setting the current health when the enemy first spawns.
        currentHealth = startingHealth;
    }


    void Update ()
    {
		if (onFirePS.isPlaying) {
			timer1 += Time.deltaTime;
			timer2 += Time.deltaTime;
		}
		if (timer2 >= timeBetweenFireDamage) {
			timer2 = 0;
			TakeDamage (onFireDamage);
		}
		if (timer1 >= onFireTime) {
			timer1 = 0;
			onFirePS.Stop ();
			onFireLight.enabled = false;
		}

        // If the enemy should be sinking...
        if(isSinking)
        {
            // ... move the enemy down by the sinkSpeed per second.
            transform.Translate (-Vector3.up * sinkSpeed * Time.deltaTime);
        }
    }

	public void TakeDamage (int amount)//, Vector3 hitPoint)
    {
        if(isDead)
            return;

		anim.SetTrigger ("Hurt");

        currentHealth -= amount;

        if(currentHealth <= 0)
        {
			PlayerExperience.Instance.IncreaseExp (enemyExp);
			PlayerLevel.Instance.enemiesKilled++;
            Death ();
        }
    }


    public void Death ()
    {
        isDead = true;
        capsuleCollider.isTrigger = true;
		navMeshAgent.enabled = false;

        anim.SetTrigger ("Dead");

		if (Random.Range (0, 3) == 0) {
			if (gameObject.tag == "Enemy1")
				enemyAudio.clip = zombie1Death;
			else if (gameObject.tag == "Enemy2") {
				enemyAudio.volume /= 2;
				enemyAudio.pitch = 1.2f;
				enemyAudio.clip = zombie2Death;
			}
			else 
				enemyAudio.clip = skeletonDeath;
			enemyAudio.Play ();
		}
    }


    public void StartSinking ()
    {
        // Find the rigidbody component and make it kinematic (since we use Translate to sink the enemy).
        GetComponent <Rigidbody> ().isKinematic = true;

        // The enemy should now sink.
        isSinking = true;

        // After 2 seconds destory the enemy.
        Destroy (gameObject, 2f);
    }
}