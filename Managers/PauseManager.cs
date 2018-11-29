using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class PauseManager : MonoBehaviour {
	
	public AudioMixerSnapshot paused;
	public AudioMixerSnapshot unpaused;
	public PlayerMovement playerMovement;
	public GameObject spellOrigin;
	//public GameObject backgroundMusic;
	public OrbControl orbControl;
	public Canvas levelUpCanvas;
	public Canvas menuCanvas;

	AudioSource[] backgroundSource;
	Canvas pauseCanvas;
	
	void Start()
	{
		pauseCanvas = GetComponent<Canvas>();
		//backgroundSource = backgroundMusic.GetComponents<AudioSource> ();
		Time.timeScale = 1;
	}
		
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape) && levelUpCanvas.enabled == false){
			PauseGame();
		}
	}
	
	public void PauseGame()
	{
		playerMovement.enabled = !playerMovement.enabled;
		orbControl.enabled = !orbControl.enabled;
		pauseCanvas.enabled = !pauseCanvas.enabled;
		spellOrigin.SetActive (spellOrigin.activeSelf? false : true);
		Cursor.visible = !Cursor.visible;
		Cursor.lockState = Cursor.visible? CursorLockMode.None : CursorLockMode.Locked;
		Time.timeScale = Time.timeScale == 0? 1 : 0;
		if (Time.timeScale == 0) {
			pauseCanvas.sortingOrder = 5;
			paused.TransitionTo (.01f);
		} else {
			pauseCanvas.sortingOrder = 0;
			unpaused.TransitionTo (.01f);
		}
	}
	
	public void MainMenu()
	{
		unpaused.TransitionTo (.01f);
		SceneManager.LoadScene ("MainMenu");
	}
}
