using UnityEngine;
using System.Collections;

public class StoneJones : MonoBehaviour {
	private GameObject player;

	void Start () {
		player = GameObject.FindWithTag("Player");
	}
	
	void OnTriggerEnter (Collider what) {	
		if (player != null && what.gameObject == player) {	
			if (player.GetComponent<BadBodeAnimationandSound>().hasHorn) {
				player.GetComponent<BadBodeAnimationandSound>().ToggleTitaniumHorn();
			} else {			
				player.GetComponent<BadBode>().Die();
			}
		}
	}
}
