using UnityEngine;
using System.Collections;

public class Drink : MonoBehaviour {

	public Transform bonusSound;
	

	
	void OnTriggerEnter (Collider what) {
		GameObject player = GameObject.FindWithTag("Player");
		if (what.gameObject == player) {
			player.GetComponent<DragShotMover>().Drink();
			Instantiate(bonusSound);
			Destroy(gameObject);
		}
	}
}
