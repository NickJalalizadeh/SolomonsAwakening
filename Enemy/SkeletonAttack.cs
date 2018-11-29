using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class SkeletonAttack : MonoBehaviour
{
	public float timeBetweenAttacks = 2.0f;     // The time in seconds between each attack.
	public int attackDamage = 10;               // The amount of health taken away per attack.
	public float arrowForce = 35;
	public GameObject arrow;
	public GameObject bow;
	public Rigidbody arrowRigidbody;
	public Transform arrowColliderTrans;
	public Transform arrowOrigin;
	public AudioClip rattlingBones;
	public AudioSource arrowAudio;

	Animator anim;                              // Reference to the animator component.
	GameObject player;                          // Reference to the player GameObject.
	PlayerHealth playerHealth;                  // Reference to the player's health.
	EnemyHealth skeletonHealth;  	                  // Reference to this enemy's health.
	Transform skeleton;
	NavMeshAgent navMeshAgent;
	Rigidbody skeletonRigidbody;
	AudioSource skeletonAudio;
	bool playerInRange;                         // Whether player is within the trigger collider and can be attacked.
	float timer, timer2;                                // Timer for counting up to the next attack.
	int rand1, rand2;

	Quaternion newPosition;
	public Transform testRotation;

	void Awake ()
	{
		// Setting up the references.
		player = GameObject.FindGameObjectWithTag ("Player");
		playerHealth = player.GetComponent <PlayerHealth> ();
		skeletonHealth = GetComponent<EnemyHealth>();
		anim = GetComponent <Animator> ();
		skeleton = GetComponentInParent<Transform> ();
		navMeshAgent = GetComponentInParent<NavMeshAgent> ();
		skeletonRigidbody = GetComponent<Rigidbody> ();
		skeletonAudio = GetComponent<AudioSource> ();

		arrow.SetActive (false);
		bow.SetActive (false);
		rand1 = Random.Range (0, 3);
		rand2 = Random.Range (5, 10);
	}

	void OnTriggerEnter (Collider other)
	{
		// If the entering collider is the player...
		if(other.gameObject == player && skeletonHealth.currentHealth > 0)
		{
			// ... the player is in range.
			playerInRange = true;
			anim.SetTrigger ("equipBow");
			bow.SetActive (true);
			navMeshAgent.Stop ();
			skeletonRigidbody.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
		}
	}

	void rotateSkeleton()
	{
		//Make up for animation rotations
		transform.localRotation = Quaternion.AngleAxis (80, Vector3.up);
		anim.SetBool ("bowEquipped", true);
	}

	void OnTriggerExit (Collider other)
	{
		// If the exiting collider is the player...
		if(other.gameObject == player && skeletonHealth.currentHealth > 0)
		{
			// ... the player is no longer in range.
			playerInRange = false;
			skeletonRigidbody.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
			navMeshAgent.Resume ();
			anim.SetTrigger ("Walk");
			//anim.SetBool ("bowEquipped", false);
		}
	}

	void Update ()
	{
		// Add the time since Update was last called to the timer.
		timer += Time.deltaTime;	
		timer2 += Time.deltaTime;

		if (playerInRange) {
			skeleton.position = transform.position;
			testRotation.LookAt (player.transform);
			newPosition = testRotation.rotation * Quaternion.Euler(0, 80, 0);

			skeleton.rotation = Quaternion.Lerp(skeleton.rotation, newPosition, Time.deltaTime * 2f);
		}

		// If the timer exceeds the time between attacks, the player is in range and this enemy is alive...
		if(timer >= timeBetweenAttacks && playerInRange && skeletonHealth.currentHealth > 0)
		{
			// ... attack.
			Attack ();
		}

		if (arrow.transform.position.y <= .01f) {
			arrow.SetActive (false);
			arrow.transform.position = arrowOrigin.position;
			arrowRigidbody.isKinematic = true;
		}

		if (rand1 == 1 && skeletonHealth.currentHealth > 0 && timer2 >= rand2) {
			timer2 = 0;
			skeletonAudio.clip = rattlingBones;
			skeletonAudio.Play ();
		}

		// If the player has zero or less health...
		if(playerHealth.currentHealth <= 0)
		{
			// ... tell the animator the player is dead.
			anim.SetTrigger ("PlayerDead");
		}
	}


	void Attack ()
	{
		// If the player has health to lose...
		if(playerHealth.currentHealth > 0)
		{
			// Reset the timer.
			timer = 0f;
			anim.SetTrigger ("Attack");
		}
	}

	void ShootArrow ()
	{
		//transform.localRotation = Quaternion.AngleAxis (90, Vector3.up);
		arrow.SetActive (true);
		arrowRigidbody.isKinematic = false;
		arrowRigidbody.AddForce (arrowColliderTrans.forward * arrowForce);
		arrowAudio.Play ();
	}

	public void HandleCollision(Collider other)
	{
		if (other.gameObject == player)
		{
			if(playerHealth.currentHealth > 0)
			{
				playerHealth.TakeDamage (attackDamage);
			}

			arrow.SetActive (false);
			arrow.transform.position = arrowOrigin.position;
			arrowRigidbody.isKinematic = true;
		}
	}
}