using UnityEngine;
using System.Collections;

public class Snow : MonoBehaviour {
	// Public vars
	public float speed = 0.02f;
	public float initialTimer = 10.0f;
	public float finalTimer = 1.0f;
	
	// Private vars
	private Vector3 destination;
	private float timer, moveSteps;
	private bool move, count;
	private GameObject player;
	private int level;

	void Start () {
		player = GameObject.FindWithTag("Player");
		destination = transform.position;
		timer = 0;
		move = false;
		count = true;
		moveSteps = initialTimer;
	}
	
	void FixedUpdate () {
		timer += Time.deltaTime;
		
		if (moveSteps > finalTimer) {
			/**
			 * moveSteps is the leap of time the snow will take to move to the next position.
			 * finalTimer is the final leap of time: last 'level' or, as they say, Hard Core mode.
			 * initialTimer is the first leap: first 'level' or, as they say, easy peasy.
			 **/
			level = player.GetComponent<BadBode>().GetLevel();
			moveSteps = (initialTimer - level) + 1;
		}
		
		if (count && timer > moveSteps) {
			//count until it's time to move, then update our flasg to start moving
			move = true;
			count = false;
			timer = 0;
			destination.y++;
		}
		
		if (move && player != null && !player.GetComponent<BadBode>().GetIsDead()) {
			transform.position = Vector3.MoveTowards(transform.position, destination, speed);
			
			if (transform.position == destination) {
				//if we reach our destination, get back on counting
				move = false;
				count = true;
			}
		}
	}
	
	// Public functions
	
	// UpdatePositionY is called by the "PlatLoader" script in order to set it's position to the end of the available platforms
	public void UpdatePositionY (float newY) {		
		Vector3 newPosition = transform.position;
		newPosition.y = newY;
		transform.position = newPosition;
		destination.y = newY;
	}
	
	void OnTriggerEnter (Collider what) {	
		if (player != null && what.gameObject == player) {	
			player.GetComponent<BadBode>().Die();
		}
	}
}
