using UnityEngine;
using System.Collections;

public class ShootFireBall : MonoBehaviour {

	public static ShootFireBall Instance;

	public int spellDamage = 40;											// Set the number of hitpoints that this gun will take away from shot objects with a health script
	public double fireRate = 10;										// Number in seconds which controls how often the player can fire
	public int spellRange = 35;										// Distance in Unity units over which the player can fire
	public int manaCost = 12; 
	public float fireBallSpeed = 25;
	public Transform fireBallTrans;
	public GameObject Orb;
	public OrbControl orbControl;
	public Camera fpsCam;												// Holds a reference to the first person camera
	public ParticleSystem fireBallPS;
	public Light lightSource;
	public SphereCollider fireBallCollider;
	public GameObject progressBar;
	public Rigidbody fireBallRigid;

	PlayerMana playerMana;
	PlayerMovement playerMovement;
	AudioSource spellAudio;										// Reference to the audio source which will play our shooting sound effect
	Animator anim;
	EnemyHealth enemyhealth;
	ParticleSystem enemyOnFire;
	Light enemyOnFireLight;

	float castTime = 1f;
	float nextFire;												// Float to store the time the player will be allowed to fire again, after firing
	float timer;
	float collisionDistance;									//Distance to the collision Point
	bool fireBallShot;
	Vector3 targetVector;
	Vector3 distance;
	public static bool isCasting;

	void Start () 
	{
		Instance = this;
		// Get and store a reference to our AudioSource component
		spellAudio = GetComponent<AudioSource>();

		playerMovement = GetComponentInParent<PlayerMovement> ();

		playerMana = GetComponentInParent<PlayerMana> ();

		anim = GetComponentInParent<Animator> ();

		timer = (float)fireRate;
	}

	void Update ()
	{
		timer += Time.deltaTime;

		if (fireBallShot == true) {
			distance = fireBallTrans.position - transform.position;
			if (distance.magnitude >= spellRange) {
				ResetFireBall ();
			}
		} 
		else
		{
			fireBallTrans.position = transform.position;
		}

		// Check if the player has pressed the fire button and if enough time has elapsed since they last fired
		if (Input.GetKey (KeyCode.Alpha1) && timer > fireRate && playerMana.currentMana >= manaCost && !isCasting) {

			isCasting = true;
			playerMovement.enabled = false;
			orbControl.enabled = false;
			anim.SetInteger ("state", -1);	
			anim.SetTrigger ("ShootFireBall");
			progressBar.SetActive (true);
			ProgressBarManager.Instance.UpdateProgress ("Casting FireBall", castTime);
		}
	}

	public void ShootFB () {
		
		playerMana.ReduceMana (manaCost);
		fireBallShot = true;
		isCasting = false;
		progressBar.SetActive (false);
		timer = 0;

		targetVector = fpsCam.transform.position + fpsCam.transform.forward * spellRange - transform.position;
		targetVector.Normalize ();
		fireBallTrans.forward = targetVector;

		lightSource.enabled = true;
		fireBallCollider.enabled = true;
		playerMovement.enabled = true;
		playerMovement.state = 0;
		orbControl.enabled = true;
		fireBallPS.Play ();

		fireBallRigid.velocity = targetVector * fireBallSpeed;
	}

	public void HandleCollision (Collider other)
	{
		enemyhealth = other.GetComponent<EnemyHealth> ();
		enemyOnFire = other.GetComponentInChildren<ParticleSystem> ();
		enemyOnFireLight = other.GetComponentInChildren<Light> ();

		if (enemyhealth != null) {
			enemyhealth.TakeDamage (spellDamage);
			enemyOnFireLight.enabled = true;
			enemyOnFire.Play ();
		}

	}

	private void ResetFireBall()
	{
		fireBallShot = false;
		lightSource.enabled = false;
		fireBallCollider.enabled = false;
		fireBallPS.Stop ();

		fireBallRigid.velocity = Vector3.zero;
		fireBallRigid.angularVelocity = Vector3.zero;
	}
}