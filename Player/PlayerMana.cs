using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMana : MonoBehaviour {

	public static PlayerMana Instance;

	public float startingMana = 50f;
	public float currentMana;
	public float speed = 1.5f;
	public Slider manaSlider;
	public bool resetMana;

	void Awake() {
		Instance = this;
		currentMana = startingMana;
	}

	public void ResetMana()
	{
		foreach (iTween itween in GetComponents<iTween>())
			Destroy (itween);
		currentMana = startingMana;
		manaSlider.value = currentMana;
	}

	void IncreaseMana (float speed)
	{
		Hashtable param = new Hashtable();
		param.Add("from", currentMana);
		param.Add("to", startingMana);
		param.Add("speed", speed);
		param.Add("onupdate", "TweenedSomeValue1");
		iTween.ValueTo(gameObject, param);
	}

	public void TweenedSomeValue1 (float val) {
		if (resetMana)
			val = startingMana;
		
		manaSlider.value = val;
		currentMana = val;
	}

	public void ReduceMana(float amount) {
		currentMana -= amount;
		manaSlider.value = currentMana;	
		IncreaseMana (speed);
	}
}
