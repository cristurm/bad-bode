using UnityEngine;
using System.Collections;

public class SoundController : MonoBehaviour {	
	// Private vars
	private int isMute;

	void Start () {
		isMute = PlayerPrefs.GetInt("Mute");
		
		if (isMute == 0 || isMute == 2) {
			AudioListener.volume = 1;
			
			// tweak into NGUI Image Button script from the Pause Button
			transform.GetComponent<UIImageButton>().target.spriteName = transform.GetComponent<UIImageButton>().normalSprite;
		} else if (isMute == 1) {
			AudioListener.volume = 0;
			
			// tweak into NGUI Image Button script from the Pause Button
			transform.GetComponent<UIImageButton>().target.spriteName = transform.GetComponent<UIImageButton>().pressedSprite;
		}
	}
	
	void OnClick () {
		/*
		 * isMute:
		 * 0 = noPrefs
		 * 1 = mute
		 * 2 = notMute
		 */
		
		if (isMute == 0) {
			AudioListener.volume = 0;
			PlayerPrefs.SetInt("Mute", 1);
			isMute = 1;
			
			// tweak into NGUI Image Button script from the Pause Button
			transform.GetComponent<UIImageButton>().target.spriteName = transform.GetComponent<UIImageButton>().pressedSprite;
		} else {
			if (isMute == 1) {
				AudioListener.volume = 1;
				PlayerPrefs.SetInt("Mute", 2);
				isMute = 2;
				
				// tweak into NGUI Image Button script from the Pause Button
				transform.GetComponent<UIImageButton>().target.spriteName = transform.GetComponent<UIImageButton>().normalSprite;
			} else if (isMute == 2) {
				AudioListener.volume = 0;
				PlayerPrefs.SetInt("Mute", 1);
				isMute = 1;
				
				// tweak into NGUI Image Button script from the Pause Button
				transform.GetComponent<UIImageButton>().target.spriteName = transform.GetComponent<UIImageButton>().pressedSprite;
			}
		}
	}
}
