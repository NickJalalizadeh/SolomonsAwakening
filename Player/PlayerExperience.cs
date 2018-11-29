using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerExperience : MonoBehaviour {

	public static PlayerExperience Instance;
	public Slider experienceSlider;

	Text levelText;
	float startingExp = 0;
	int newLevel;
	public float currentExp;

	void Awake () {
		Instance = this;
		currentExp = startingExp;
		levelText = experienceSlider.GetComponentInChildren<Text> ();
		 	 
		experienceSlider.minValue = 0;
		experienceSlider.maxValue = (2 / .365f) * (2 / .365f);
	}

	public void IncreaseExp(float amount)
	{
		Hashtable param = new Hashtable();
		param.Add("from", currentExp);
		param.Add("to", currentExp + amount);
		param.Add("speed", 5);
		param.Add("onupdate", "TweenedSomeValue");
		iTween.ValueTo(gameObject, param);
		currentExp += amount;

		newLevel = ((int)(.365f * Mathf.Sqrt (currentExp)));

		if (newLevel > 1 && newLevel != PlayerLevel.Instance.currentLevel) {
			PlayerLevel.Instance.LevelUp (newLevel);
			levelText.text = newLevel.ToString();
			experienceSlider.minValue = ((newLevel) / .365f) * ((newLevel) / .365f);
			experienceSlider.maxValue = ((newLevel + 1) / .365f) * ((newLevel + 1) / .365f);
		}
	}

	void Update () {

	}

	public void TweenedSomeValue(int val){
		experienceSlider.value = val;
	}
}
