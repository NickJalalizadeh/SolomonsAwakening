using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour {

	void Start()
	{
		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.None;
	}

	public void StartGame()
	{
		SceneManager.LoadScene ("Main");
	}

	public void Quit()
	{
		Application.Quit ();
	}
}
