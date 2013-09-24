using UnityEngine;
using System.Collections;

public class Bat : MonoBehaviour {
	// Public vars
	public float speed;
	
	// Private vars
	private GameObject myParent;
	private Vector3 position1, position2;
	private float parentWidthOffset;
	private bool forwards, isDead;
	private GameObject player;
	private Transform bat;

	void Start () {
		bat = transform.Find("morcegoFBX");
		bat.rotation =  Quaternion.Euler(0, 50, 0);
		forwards = true;
		isDead = false;
		myParent = transform.parent.gameObject;
		parentWidthOffset = myParent.renderer.bounds.size.x * 0.5f;
		
		position1 = transform.position;
		position1.x = position1.x + parentWidthOffset + 1.0f;
		
		position2 = transform.position;
		position2.x = position2.x - parentWidthOffset - 1.0f;
		
		player = GameObject.FindWithTag("Player");
	}
	
	void FixedUpdate () {
		if(isDead){
			return;
		}
		
		// The bat will be basically flying from position1 to position2
		if (transform.position == position1) {
			forwards = false;
			transform.GetComponentInChildren<Animation>().PlayQueued("idle");
			bat.rotation =  Quaternion.Euler(0, -50, 0);
		}
		
		if (transform.position == position2) {
			forwards = true;
			transform.GetComponentInChildren<Animation>().PlayQueued("idle");
			bat.rotation =  Quaternion.Euler(0, 50, 0);
		}
		
		if (forwards) {
			transform.position = Vector3.MoveTowards(transform.position, position1, speed);
		} else {
			transform.position = Vector3.MoveTowards(transform.position, position2, speed);
		}
	}
	
	void OnCollisionEnter(Collision what) {		
		if (what.gameObject == player) {
			if (player.GetComponent<BadBodeAnimationandSound>().hasHorn) {
				//player.GetComponent<BadBodeAnimationandSound>().TitaniumHorn();
				rigidbody.useGravity = true;
				rigidbody.isKinematic = false;
				isDead = true;
				transform.GetComponentInChildren<Animation>().Play("die");
				//transform.GetComponent<BoxCollider>().isTrigger = true;
				Destroy(gameObject, 2);
			} else {
				if (isDead) {
					return;
				}
				
				audio.Play();
				foreach (ContactPoint contact in what.contacts) {
					if (contact.normal.x > 0) {
						player.rigidbody.AddForce(Vector3.left * 50);
					} else {
						player.rigidbody.AddForce(Vector3.right * 50);
					}
					transform.GetComponentInChildren<Animation>().Play("attack");
					transform.GetComponentInChildren<Animation>().PlayQueued("idle");
					if (contact.normal.y < 1) {
						player.rigidbody.AddForce(Vector3.up * 50);
					}
				}
			}
		}
	}
}
