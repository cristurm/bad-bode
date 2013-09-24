using UnityEngine;
using System.Collections;

public class Parenter : MonoBehaviour {	
	void OnTriggerEnter (Collider what) {
		 GameObject player = GameObject.FindWithTag("Player");
		
		if (what.gameObject == player) {
			player.transform.parent = transform;
		}
	}
	
	void OnTriggerExit () {
		transform.DetachChildren();
	}
}
