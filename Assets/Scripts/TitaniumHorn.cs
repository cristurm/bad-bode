using UnityEngine;
using System.Collections;

public class TitaniumHorn : MonoBehaviour {

	// Use this for initialization
	public Transform bonusSound;
	void Start () {
	
	}
	
	
	void OnTriggerEnter (Collider what) {
		GameObject player = GameObject.FindWithTag("Player");
		
		if (what.gameObject == player) {
			if(player.GetComponent<BadBodeAnimationandSound>().hasHorn == false){
				player.GetComponent<BadBodeAnimationandSound>().ToggleTitaniumHorn();
				Instantiate(bonusSound);
				Destroy(gameObject);
			}else{
				Instantiate(bonusSound);
				Destroy(gameObject);
			}
		}
	}
}
