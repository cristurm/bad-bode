using UnityEngine;
using System.Collections;

public class SpikyPlatform : MonoBehaviour {
	private GameObject player;

	// Use this for initialization
	void Start () {
		player = GameObject.FindWithTag("Player");
	}
	
	void OnTriggerEnter (Collider what) {
		if (what.gameObject == player) {
			if (player.GetComponent<BadBodeAnimationandSound>().hasHorn == true) {
				player.GetComponent<BadBodeAnimationandSound>().ToggleTitaniumHorn();
			} else {
				player.GetComponent<BadBode>().Die();
			}
		}
	}
}
