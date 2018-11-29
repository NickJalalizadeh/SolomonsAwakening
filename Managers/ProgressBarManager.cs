using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarManager : MonoBehaviour {

	public static ProgressBarManager Instance;

	Slider progressSlider;
	Text progressText; 

	void Awake () {
		Instance = this;
		progressSlider = GetComponent<Slider> ();
		progressText = GetComponentInChildren<Text> ();
	}
	
	public void UpdateProgress(string message, float time)
	{
		progressText.text = message;

		Hashtable param = new Hashtable();
		param.Add("from", 0.0f);
		param.Add("to", 100);
		param.Add("time", time);
		param.Add("onupdate", "TweenedSomeValue");
		iTween.ValueTo(gameObject, param);
	}

	public void TweenedSomeValue(int val){
		progressSlider.value = val;
	}

}
