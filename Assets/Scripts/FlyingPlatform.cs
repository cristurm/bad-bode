using UnityEngine;
using System.Collections;

public class FlyingPlatform : MonoBehaviour {
	// Public vars
	public float speed;
	public Transform destination;
	
	// Private vars
	private Vector3 origin;
	private bool forwards;

	// Use this for initialization
	void Start () {
		origin = transform.position;
		forwards = true;
	}
	
	// Update is called once per frame
	void FixedUpdate () {		
		if (transform.position == destination.position) {
			forwards = false;
		}
		
		if (transform.position == origin) {
			forwards = true;
		}
		
		if (forwards) {
			transform.position = Vector3.MoveTowards(transform.position, destination.position, speed);
		} else {
			transform.position = Vector3.MoveTowards(transform.position, origin, speed);
		}
	}
}
