using UnityEngine;
using System.Collections;

public class ShootFlameStrike : MonoBehaviour {

	public int spellDamage = 10;											// Set the number of hitpoints that this gun will take away from shot objects with a health script
	public double fireRate = 5.0;										// Number in seconds which controls how often the player can fire
	public int manaCost = 10; 
	public Transform flameStrikeTrans;
	public GameObject Orb;
	public OrbControl orbControl;
	public ParticleSystem flameStrike;
	public ParticleSystem flameStikeExplosion;
	public Light lightSource;
	public SphereCollider flameStrikeCollider;
	public GameObject progressBar;

	PlayerMana playerMana;
	PlayerMovement playerMovement;
	AudioSource spellAudio;										// Reference to the audio source which will play our shooting sound effect
	Animator anim;
	EnemyHealth enemyhealth;
	Transform playerTrans;

	Vector3 StartingPosition;
	float castTime = 2f;
	float nextFire;												// Float to store the time the player will be allowed to fire again, after firing
	float timer;
	float horizontal;
	bool flameStrikeShot;
	public static bool isCasting;

	void Start () 
	{
		// Get and store a reference to our AudioSource component
		//spellAudio = GetComponent<AudioSource>();

		playerMovement = GetComponentInParent<PlayerMovement> ();

		playerMana = GetComponentInParent<PlayerMana> ();

		playerTrans = GetComponentInParent<Transform> ();

		anim = GetComponentInParent<Animator> ();

		timer = (float)fireRate;
	}

	void Update ()
	{
		timer += Time.deltaTime;

		if (flameStrikeShot)
			flameStrikeTrans.position = StartingPosition;
		else {
			flameStrikeTrans.position = Orb.transform.position + playerTrans.forward * 10;
			flameStrikeTrans.position = new Vector3 (flameStrikeTrans.position.x, 0, flameStrikeTrans.position.z);
		}

		// Check if the player has pressed the fire button and if enough time has elapsed since they last fired
		if (Input.GetKey (KeyCode.Alpha2) && timer > fireRate && playerMana.currentMana >= manaCost && !isCasting) {
			
			isCasting = true;
			playerMovement.enabled = false;
			orbControl.enabled = false;
			anim.SetInteger ("state", -1);	
			anim.SetTrigger ("ShootFlameStrike");
			progressBar.SetActive (true);
			ProgressBarManager.Instance.UpdateProgress ("Casting Flame Strike", castTime);
		}
	}

	public void ShootFS () {

		flameStrikeShot = true;
		playerMana.ReduceMana (manaCost);
		isCasting = false;
		progressBar.SetActive (false);
		timer = 0;
		StartingPosition = flameStrikeTrans.position;

		lightSource.enabled = true;
		playerMovement.enabled = true;
		playerMovement.state = 0;
		orbControl.enabled = true;
		flameStrike.Play ();

		StartCoroutine(PlayPS (1.0f, 0.5f));
	}

	IEnumerator PlayPS(float duration1, float duration2) {
		yield return new WaitForSeconds (duration1);
		flameStrikeCollider.enabled = true;
		flameStikeExplosion.Play ();
		yield return new WaitForSeconds (duration2);
		ResetFlameStrike ();
	}

	public void HandleCollision (Collider other)
	{
		enemyhealth = other.GetComponent<EnemyHealth> ();

		if (enemyhealth != null) {
			enemyhealth.TakeDamage (spellDamage);
		}

	}

	private void ResetFlameStrike()
	{
		flameStrikeShot = false;
		lightSource.enabled = false;
		flameStrikeCollider.enabled = false;
		flameStrike.Stop ();
		flameStikeExplosion.Stop ();
	}
}