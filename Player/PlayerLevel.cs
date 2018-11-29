using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.Audio;

public class PlayerLevel : MonoBehaviour {

	public static PlayerLevel Instance;

	/*	Increase on level Up: 
		enemy speed / animation speed
		enemy attack rate
		enemy spawn rate
		max number of enemies
		number of enemies per spawn
		type of enemies
	*//*
		Level 1-5: Easy 
		Level 5-15: Medium
		Level 15-30: Hard
		
		change every 5 levels:
		Level 5: Add Zombie02, Increase Zombie01 speed
		Level 10: Increase Zombie02 speed, Increase Zombie01 firerate
		Level 15: Add Skeleton, Increase Zombie02 firerate, Increase Skeleton speed
		Level 20: Increase Skeleton firerate

		Starting firerates:
		Zombie01: 1
		Zombie02: 0.5
		Skeleton: 2

		Starting speeds:
		Zombie01: 2.5
		Zombie02: 3.5
		Skeleton: 2
		
		decrease skill gain per skill level
	*/

	public int currentLevel = 1;
	public int enemiesKilled;
	public EnemyManager enemyManager;
	public Dictionary<string, int> skills;
	public Canvas canvas;
	public GameObject Skill1, Skill2, Skill3;
	public GameObject spellOrigin;
	public AudioSource levelUpSound;
	public AudioMixerSnapshot paused, unpaused;

	EnemyHealth[] allEnemyHealth;
	PlayerHealth playerHealth;
	PlayerMana playerMana;
	PlayerMovement playerMovement;
	OrbControl orbControl;
	ShootFireBall shootFireBall;
	Text[] skill1Text, skill2Text, skill3Text;
	Image skill1Image, skill2Image, skill3Image;
	Button skill1Button, skill2Button, skill3Button;
	int value, rand1, rand2, rand3;
	bool eventAdded;

	List<KeyValuePair<string, int>> skillValues = new List <KeyValuePair<string, int>> ();
	string[] skillText;

	// Use this for initialization
	void Awake () {
		Instance = this;
		shootFireBall = GetComponentInChildren<ShootFireBall> ();

		playerHealth = GetComponent<PlayerHealth> ();
		playerMana = GetComponent<PlayerMana> ();
		playerMovement = GetComponent<PlayerMovement> ();
		orbControl = GetComponentInChildren<OrbControl> ();
		skill1Text = Skill1.GetComponentsInChildren<Text> ();
		skill2Text = Skill2.GetComponentsInChildren<Text> ();
		skill3Text = Skill3.GetComponentsInChildren<Text> ();
		skill1Image = Skill1.GetComponentInChildren<Image> ();
		skill2Image = Skill2.GetComponentInChildren<Image> ();
		skill3Image = Skill3.GetComponentInChildren<Image> ();
		skill1Button = Skill1.GetComponent<Button> ();
		skill2Button = Skill2.GetComponent<Button> ();
		skill3Button = Skill3.GetComponent<Button> ();

		skillValues.Add (new KeyValuePair<string, int> ("Explosion Size", 1));
		skillValues.Add (new KeyValuePair<string, int> ("FireBolt Damage", 1));
		skillValues.Add (new KeyValuePair<string, int> ("FireBall", 0));
		skillValues.Add (new KeyValuePair<string, int> ("Health Up", 1));
		skillValues.Add (new KeyValuePair<string, int> ("Mana Up", 1));
		skillValues.Add (new KeyValuePair<string, int> ("Mana Recovery Rate", 1));
		skillValues.Add (new KeyValuePair<string, int> ("Walking Speed", 1));

		skillText = new string[] {"Radius +0.5 \r\nMana Cost +0.1", "SpellDamage +10 \r\nManaCost +0.2", 
			"New Skill: FireBall\r\nPress 1 to use", "Max Health +20", "Max Mana +20", "Recovery Rate +0.2", "Speed +0.2"};

		eventAdded = false;
		currentLevel = 1;
	}

	public void LevelUp(int newLevel)
	{
		currentLevel = newLevel;
        
		allEnemyHealth = Object.FindObjectsOfType<EnemyHealth> ();
		foreach (EnemyHealth enemyhealth in allEnemyHealth) {
			enemyhealth.currentHealth = 0;
			enemyhealth.Death ();
			//Destroy(enemyhealth.gameObject);
		}

		StartCoroutine(WaitSeconds(3));

		enemyManager.InstantiateZombie01 (currentLevel);
		if (currentLevel >= 5)
			enemyManager.InstantiateZombie02 (currentLevel);
		if (currentLevel >= 10)
			enemyManager.InstantiateSkeleton (currentLevel);
	}

	IEnumerator WaitSeconds(int seconds)
	{
		yield return new WaitForSeconds (seconds);
		levelUpSound.Play ();
		ChooseSkills ();
		EnableSkillUpScreen ();
	}

	public void SkillUp(string skill)
	{
		switch (skill) {
		case "Explosion Size":
			ShootFireBolt.Instance.explosionRadius += 0.5f;
			ShootFireBolt.Instance.manaCost += 0.1f;
			ParticleSystem.MainModule PS = ShootFireBolt.Instance.fireBoltPS [1].main;
			PS.startSize = new ParticleSystem.MinMaxCurve (PS.startSize.constantMin + 1, PS.startSize.constantMax + 1);
			break;

		case "FireBolt Damage":	
			ShootFireBolt.Instance.spellDamage += 10;
			ShootFireBolt.Instance.manaCost += 0.2f;
			break;

		case "FireBall":
			if (skillValues [2].Value > 0)
				shootFireBall.spellDamage += 10;
			else {
				shootFireBall.enabled = true;
				skillText[2] = "SpellDamage +10";
			}
			break;

		case "Health Up":
			RectTransform rt1 = PlayerHealth.Instance.healthSlider.gameObject.GetComponent (typeof(RectTransform)) as RectTransform;
			rt1.anchoredPosition = new Vector2 (rt1.anchoredPosition.x + 0.5f * rt1.rect.width * (20f / PlayerHealth.Instance.startingHealth), rt1.anchoredPosition.y);
			rt1.sizeDelta = new Vector2 (rt1.rect.width + rt1.rect.width * (20f / PlayerHealth.Instance.startingHealth), rt1.rect.height);
			PlayerHealth.Instance.startingHealth += 20;
			PlayerHealth.Instance.healthSlider.maxValue += 20;
			break;

		case "Mana Up":
			RectTransform rt2 = PlayerMana.Instance.manaSlider.gameObject.GetComponent (typeof(RectTransform)) as RectTransform;
			rt2.anchoredPosition = new Vector2 (rt2.anchoredPosition.x + 0.5f * rt2.rect.width * (20f / PlayerMana.Instance.startingMana), rt2.anchoredPosition.y);
			rt2.sizeDelta = new Vector2 (rt2.rect.width + rt2.rect.width * (20f / PlayerMana.Instance.startingMana), rt2.rect.height);
			PlayerMana.Instance.startingMana += 20;
			PlayerMana.Instance.manaSlider.maxValue += 20;
			break;

		case "Mana Recovery Rate":
			PlayerMana.Instance.speed += 0.2f;
			break;

		case "Walking Speed":
			PlayerMovement.Instance.speed += 0.2f;
			break;

		default:
			Debug.Log ("invalid skill string");
			return;
		}
	}

	public void ChooseSkills()
	{
		if (eventAdded) {
			skill1Button.onClick.RemoveAllListeners();
			skill2Button.onClick.RemoveAllListeners();
			skill3Button.onClick.RemoveAllListeners();
		}
		else
			eventAdded = true;

		rand1 = Random.Range (0, skillValues.Count);
		skill1Text [0].text = skillValues [rand1].Key + " " + (skillValues [rand1].Value + 1);
		skill1Text [1].text = skillText [rand1];
		skill1Button.onClick.AddListener (() => IncreaseSkill(rand1));

		rand2 = Random.Range (0, skillValues.Count);
		while (rand2 == rand1) {
			rand2 = Random.Range (0, skillValues.Count);
		}
		skill2Text [0].text = skillValues [rand2].Key + " " + (skillValues [rand2].Value + 1);
		skill2Text [1].text = skillText [rand2];
		skill2Button.onClick.AddListener (() => IncreaseSkill(rand2));

		rand3 = Random.Range (0, skillValues.Count);
		while (rand3 == rand1 || rand3 == rand2) {
			rand3 = Random.Range (0, skillValues.Count);
		}
		skill3Text [0].text = skillValues [rand3].Key + " " + (skillValues [rand3].Value + 1);
		skill3Text [1].text = skillText [rand3];
		skill3Button.onClick.AddListener (() => IncreaseSkill(rand3));
	}

	public void IncreaseSkill (int index)
	{
		SkillUp (skillValues [index].Key);
		value = skillValues [index].Value;
		skillValues [index] = new KeyValuePair<string, int> (skillValues [index].Key, value + 1);
		EnableSkillUpScreen ();
		playerHealth.ResetHealth();
		playerMana.ResetMana();
	}

	void EnableSkillUpScreen()
	{
		playerMovement.enabled = !playerMovement.enabled;
		orbControl.enabled = !orbControl.enabled;
		spellOrigin.SetActive (spellOrigin.activeSelf? false : true);
		canvas.enabled = !canvas.enabled;
		Cursor.visible = !Cursor.visible;
		Cursor.lockState = Cursor.visible? CursorLockMode.None : CursorLockMode.Locked;
		Time.timeScale = Time.timeScale == 0? 1 : 0;
		if (Time.timeScale == 0) {
			canvas.sortingOrder = 5;
			paused.TransitionTo (.01f);
		} else {
			canvas.sortingOrder = 0;
			unpaused.TransitionTo (.01f);
		}
	}
}
