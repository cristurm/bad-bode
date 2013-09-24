using UnityEngine;
using System.Collections;

public class FallingPlatform : MonoBehaviour {
	public Transform stones;
	//public float timeBeforeFall, timeAfterFall;
	private GameObject player;
	private GameObject myParent;
	private bool willFall, falling;
	private float timer, shakes;
	private Vector3 myParentPosition, myParentShaking;

	// Use this for initialization
	void Start () {
		myParent = transform.parent.gameObject;
		myParentPosition = myParent.transform.position;
		myParentShaking = myParentPosition;
		timer = 0;
		willFall = false;
		falling = false;
		shakes = 0.05f;
		player = GameObject.FindWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {
		if (willFall) {
			timer += Time.deltaTime;
			
			// Do The Harlem Shake! \o/
			shakes = shakes * -1;
			myParentShaking.x = myParentPosition.x + shakes;
			myParent.transform.position = myParentShaking;
			
			if (timer >= 2 && !falling) {
				stones.GetComponent<Animation>().Play("Fall");
				// Hide the platform, remove it`s collider
				falling = true;
				//myParent.renderer.enabled = false;
				myParent.collider.enabled = false;
				transform.collider.enabled = false;
				transform.collider.isTrigger = false;
				if(transform.GetComponent<AudioSource>()){
					audio.Stop();
				}
			}
			
			if (timer >= 5) {				
				// Reset the flags, get our platform and it's collider back to their places
				stones.GetComponent<Animation>().Play("Stay");
				timer = 0;
				willFall = false;
				falling = false;
				myParent.transform.position = myParentPosition;
				//myParent.renderer.enabled = true;
				myParent.collider.enabled = true;
				transform.collider.enabled = true;
				transform.collider.isTrigger = true;
			}
		}
	}
	
	void OnTriggerEnter (Collider what) {
		if (what.gameObject == player) {			
			willFall = true;
			timer = 0;
			if(transform.GetComponent<AudioSource>()){
				if(!audio.isPlaying){
					audio.Play();
				}
			}
		}
	}
}
