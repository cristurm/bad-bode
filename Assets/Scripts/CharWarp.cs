using UnityEngine;
using System.Collections;

public class CharWarp : MonoBehaviour {
	//private vars
	private GameObject myView;
	private Vector3 myPos;
	private Vector3 screenPos;
	

	// Use this for initialization
	void Start () {
		myView = GameObject.FindWithTag("MainCamera");
		
	}
	
	// Update is called once per frame
	void Update () {
		myPos = transform.position;
		screenPos = myView.camera.WorldToScreenPoint(myPos);
		
		if (screenPos.x < 0) {
			myPos = myView.camera.ScreenToWorldPoint(new Vector3(myView.camera.pixelWidth, screenPos.y, screenPos.z));
			transform.position = myPos;
		}

		if (screenPos.x > myView.camera.pixelWidth) {
			myPos = myView.camera.ScreenToWorldPoint(new Vector3(0, screenPos.y, screenPos.z));
			transform.position = myPos;
		}
	}
}
