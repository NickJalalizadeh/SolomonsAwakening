using UnityEngine;
using System.Collections;

public class ShootFireBolt : MonoBehaviour {

	public static ShootFireBolt Instance;

	public int spellDamage = 20;											// Set the number of hitpoints that this gun will take away from shot objects with a health script
	public double fireRate = 1.0;										// Number in seconds which controls how often the player can fire
	public int spellRange = 20;										// Distance in Unity units over which the player can fire
	public float fireBoltSpeed = 25;
	public float explosionRadius;
	public float manaCost = 2;
	public Transform fireBoltTrans;
	public Transform Orb;
	public Camera fpsCam;												// Holds a reference to the first person camera
	public ParticleSystem[] fireBoltPS;
	public ParticleSystemRenderer psRend;
	public Light lightSource;
	public SphereCollider fireBoltCollider;
	public Rigidbody fireBoltRigid;
	public PlayerMana playerMana;

	AudioSource spellAudio;										// Reference to the audio source which will play our shooting sound effect
	Animator anim;
	PlayerMovement playermovement;
	EnemyHealth enemyhealth;
	AudioSource[] fireBoltAudio;

	float nextFire;												// Float to store the time the player will be allowed to fire again, after firing
	float timer;
	float collisionDistance;									//Distance to the collision Point
	bool fireBoltShot;
	Vector3 targetVector;
	Vector3 distance;
	Vector3 collisionPoint;
	RaycastHit hit;

	void Awake() {
		Instance = this;
	}
	void Start () 
	{
		fireBoltAudio = fireBoltTrans.gameObject.GetComponents<AudioSource> ();
		playermovement = GetComponentInParent<PlayerMovement> ();
		anim = GetComponentInParent<Animator> ();
		explosionRadius = 0.5f;
		timer = (float)fireRate;
	}

	void Update ()
	{
		timer += Time.deltaTime;

		if (fireBoltShot == true) {
			distance = fireBoltTrans.position - transform.position;
			if (distance.magnitude >= spellRange) {
				ResetFireBolt ();
			}
		}
		else if (fireBoltShot == false && fireBoltPS [1].isEmitting == true) 
		{
			fireBoltTrans.position = collisionPoint;
		}
		else if (fireBoltShot == false && fireBoltPS [1].isEmitting == false) 
		{
			ResetFireBolt ();
		}

		// Check if the player has pressed the fire button and if enough time has elapsed since they last fired
		if (Input.GetButton ("Fire1") && timer > fireRate && !ShootFireBall.isCasting && playerMana.currentMana >= manaCost) {

			fireBoltTrans.position = transform.position;
			playerMana.ReduceMana (manaCost);
			fireBoltShot = true;
			timer = 0;

			Vector3 rayOrigin = fpsCam.ViewportToWorldPoint (new Vector3 (0.5f, 0.5f, 0.0f));

			if (Physics.Raycast (rayOrigin, fpsCam.transform.forward, out hit, 25)) {
				targetVector = hit.point - transform.position;
			} 
			else {
				targetVector = fpsCam.transform.position + fpsCam.transform.forward * spellRange - transform.position;
			}

			targetVector.Normalize ();
			fireBoltTrans.forward = targetVector;

			psRend.maxParticleSize = 0.5f;
			lightSource.enabled = true;
			fireBoltCollider.enabled = true;
			fireBoltAudio[0].Play ();
			fireBoltAudio [1].Stop ();
			fireBoltPS [0].Play ();
			fireBoltPS [1].Stop ();

			fireBoltRigid.velocity = targetVector * fireBoltSpeed;
			//Instantiate (fireBoltObject, Spells);
		}
	}

	public void HandleCollision (Collider other)
	{
		if ((transform.position - fireBoltTrans.position).magnitude >= .01f) 
		{
			fireBoltShot = false;

			enemyhealth = other.GetComponent<EnemyHealth> ();
			//collisionDistance = Vector3.Distance(other.transform.position, fireBoltTrans.position);

			if (enemyhealth != null) {
				enemyhealth.TakeDamage (spellDamage);
			}

			collisionPoint = fireBoltTrans.position;
			fireBoltCollider.radius = explosionRadius;
			psRend.maxParticleSize = 0;
			if (!fireBoltAudio[1].isPlaying)
				fireBoltAudio[1].Play ();
			fireBoltPS [0].Stop ();
			fireBoltPS [1].Play ();

			fireBoltRigid.velocity = Vector3.zero;
			fireBoltRigid.angularVelocity = Vector3.zero;
		}
	}

	private void ResetFireBolt()
	{
		fireBoltShot = false;

		lightSource.enabled = false;
		fireBoltCollider.radius = .25f;
		fireBoltCollider.enabled = false;
		fireBoltRigid.velocity = Vector3.zero;
		fireBoltRigid.angularVelocity = Vector3.zero;
	}
}