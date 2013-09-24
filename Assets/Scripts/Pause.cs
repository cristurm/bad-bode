using UnityEngine;
using System.Collections;

public class Pause : MonoBehaviour {
	private GameObject pauseGUI;
	private BadBode playerScript;
	private CamView gameCamScript;
	private Transform pauseButton;
	
	void Start () {
		playerScript = GameObject.FindWithTag("Player").GetComponent<BadBode>();
		gameCamScript = GameObject.FindWithTag("MainCamera").GetComponent<CamView>();
		pauseGUI = GameObject.FindWithTag("PauseScreen");
		pauseButton = GameObject.FindWithTag("Pause").transform;
		pauseGUI.transform.localPosition = new Vector3(-1200.0f,0,0);
	}
	
	void OnClick () {
		if (!playerScript.GetIsDead()) {
			TogglePauseGame(!gameCamScript.isPaused);
		}
	}
	
	// public functions
	public bool GetIsOn () {
		return gameCamScript.isPaused;
	}
	
	// private functions
	private void TogglePauseGame (bool pause) {		
		if (pause) {			
			Time.timeScale = 0;
			pauseGUI.transform.localPosition = new Vector3(0,0,0);
			
			// tweak into NGUI Image Button script from the Pause Button
			pauseButton.GetComponent<UIImageButton>().target.spriteName = pauseButton.GetComponent<UIImageButton>().pressedSprite;
		} else {
			Time.timeScale = 1;
			pauseGUI.transform.localPosition = new Vector3(-1200.0f,0,0);
			
			// tweak into NGUI Image Button script from the Pause Button
			pauseButton.GetComponent<UIImageButton>().target.spriteName = pauseButton.GetComponent<UIImageButton>().normalSprite;
		}
		
		// tweak into NGUI Image Button script from the Pause Button
		pauseButton.GetComponent<UIImageButton>().isOn = pause;
		gameCamScript.isPaused = pause;
	}
	
	
}
