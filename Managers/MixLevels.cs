using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MixLevels: MonoBehaviour {

	public Text valueText;

	public void ChangeSensitivity (float sens)
	{
		PlayerMovement.Instance.TurningSpeed = sens;
		OrbControl.Instance.TurningSpeed = sens;
		valueText.text = sens.ToString();
	}
}
