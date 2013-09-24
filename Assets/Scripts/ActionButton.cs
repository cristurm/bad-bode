using UnityEngine;
using System.Collections;

public class ActionButton : MonoBehaviour {
	// Public vars
	public enum ActionList {LoadScene, ExitGame};
	public ActionList action;
	public string sceneToLoadName;

	public void OnClick () {		
		switch (action) {
			case ActionList.LoadScene:
				Application.LoadLevel(sceneToLoadName);
			break;
			
			case ActionList.ExitGame:
				Application.Quit();
			break;
		}
	}
}
