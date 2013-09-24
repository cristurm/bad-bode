using UnityEngine;
using System.Collections;

public class GravityControl : MonoBehaviour {
	
	public Transform bonusSound;
	
	
	void OnTriggerEnter (Collider what) {
		GameObject player = GameObject.FindWithTag("Player");
		if (what.gameObject == player) {
			player.GetComponent<BadBodeAnimationandSound>().hasmatrixGlasses = false;//garante que vai ficar com matrixGlasses
			player.GetComponent<BadBodeAnimationandSound>().ToogleMatrixGlasses();
			player.GetComponent<DragShotMover>().GravityControl();
			player.GetComponent<GravityTrail>().Emit = true;
			Instantiate(bonusSound);
			Destroy(gameObject);
		}
	}
}

