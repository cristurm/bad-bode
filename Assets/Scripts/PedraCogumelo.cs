using UnityEngine;
using System.Collections;

public class PedraCogumelo : MonoBehaviour {
	
	private GameObject player;
	// Use this for initialization
	void Start () {
		player = GameObject.FindWithTag("Player");
	}
	
	void OnCollisionEnter(Collision what) {
		if (what.gameObject == player) {			
			transform.GetComponentInChildren<Animation>().Play();
			if(transform.GetComponent<AudioSource>()){
				audio.Play();
			}
		}
	}
}
