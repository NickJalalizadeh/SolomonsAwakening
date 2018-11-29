using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
	public static PlayerHealth Instance;

    public int startingHealth = 50;                            // The amount of health the player starts the game with.
    public int currentHealth;                                   // The current health the player has.
    public Slider healthSlider;                                 // Reference to the UI's health bar.
    public Image damageImage;                                   // Reference to an image to flash on the screen on being hurt.
    public float flashSpeed = 3f;                               // The speed the damageImage will fade at.
    public Color flashColour = new Color(.604f, .216f, .216f, 0.361f);     // The colour the damageImage is set to, to flash.
	public AudioClip[] audioClips;
	public Canvas gameOverCanvas;
	public Text statsText;

    Animator anim;                                              // Reference to the Animator component.
	Animator gameOverAnim;
    AudioSource[] playerAudio;                                    // Reference to the AudioSource component.
    PlayerMovement playermovement;                              // Reference to the player's movement.
    ShootFireBolt shootFireBolt;                              // Reference to the shootFireBolt script.
	OrbControl orbControl;
    bool isDead;                                                // Whether the player is dead.
    bool damaged;                                               // True when the player gets damaged.

    void Awake ()
    {
		Instance = this;
        // Setting up the references.
        anim = GetComponent <Animator> ();
        playerAudio = GetComponents <AudioSource> ();
        playermovement = GetComponent <PlayerMovement> ();
        shootFireBolt = GetComponentInChildren <ShootFireBolt> ();
		orbControl = GetComponentInChildren<OrbControl> ();
		gameOverAnim = gameOverCanvas.GetComponent<Animator> ();

        // Set the initial health of the player.
        currentHealth = startingHealth;
    }


    void Update ()
    {
        // If the player has just been damaged...
        if(damaged)
        {
            // ... set the colour of the damageImage to the flash colour.
            damageImage.color = flashColour;
        }
        // Otherwise...
        else
        {
            // ... transition the colour back to clear.
            damageImage.color = Color.Lerp (damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
        }

        // Reset the damaged flag.
        damaged = false;
    }

	public void ResetHealth()
	{
		currentHealth = startingHealth;
		healthSlider.value = currentHealth;
	}

    public void TakeDamage (int amount)
    {
        // Set the damaged flag so the screen will flash.
        damaged = true;

        // Reduce the current health by the damage amount.
        currentHealth -= amount;

        // Set the health bar's value to the current health.
        healthSlider.value = currentHealth;

		playerAudio [1].clip = audioClips [Random.Range (0, 6)];
		playerAudio [1].Play ();
		anim.SetTrigger ("Hurt");


        // If the player has lost all it's health and the death flag hasn't been set yet...
        if(currentHealth <= 0 && !isDead)
        {
            // ... it should die.
            Death ();
        }
    }


    void Death ()
    {
        // Set the death flag so this function won't be called again.
        isDead = true;

        // Tell the animator that the player is dead.
        anim.SetTrigger ("Die");
		anim.SetInteger ("state", -1);

        // Turn off the movement and shooting scripts.
        playermovement.enabled = false;
        shootFireBolt.enabled = false;
		orbControl.enabled = false;

		gameOverCanvas.sortingOrder = 10;
		gameOverAnim.SetTrigger ("GameOver");
		statsText.text = "Level: " + PlayerLevel.Instance.currentLevel + "\r\n\nExperience Gained: " +
			PlayerExperience.Instance.currentExp + "\r\n\nEnemies Killed: " + PlayerLevel.Instance.enemiesKilled;
		Cursor.visible = !Cursor.visible;
		Cursor.lockState = Cursor.visible? CursorLockMode.None : CursorLockMode.Locked;
    }


    public void RestartLevel ()
    {
        SceneManager.LoadScene (0);
    }

	public void Quit()
	{
		Application.Quit ();
	}
}