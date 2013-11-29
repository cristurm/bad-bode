using UnityEngine;
using System.Collections;

public class Bode : MonoBehaviour {
	// Public vars
	public float speed, timeStopped, distanceAttack;
	public LayerMask playerLayer;
	
	public AudioClip beh;
	public AudioClip walk;
	public AudioClip die;
	
	// Private vars
	private GameObject myParent;
	private Vector3 position1, position2;
	private float parentWidthOffset, myWidthOffset, timer, tempSpeed;
	private enum status {toP1, toP2, stopped}
	private status bodeStatus;
	private GameObject player;
	private bool turnFlag, rageMode, soundBool, died;

	void Start () {
		transform.GetComponentInChildren<Animation>().Play("walk");
		bodeStatus = status.toP1;
		died = false;
		turnFlag = true;
		rageMode = false;
		soundBool = false;
		myParent = transform.parent.gameObject;
		parentWidthOffset = myParent.renderer.bounds.size.x * 0.5f;
		myWidthOffset = renderer.bounds.size.x * 0.5f;
		
		position1 = transform.position;
		position1.x = (position1.x + parentWidthOffset) - myWidthOffset;
		
		position2 = transform.position;
		position2.x = (position2.x - parentWidthOffset) + myWidthOffset;
		
		tempSpeed = speed;
		
		player = GameObject.FindWithTag("Player");
	}
	
	void FixedUpdate () {
		if(died){
			return;
		}
		
		RaycastHit rayHit;
		Vector3 dir = transform.TransformDirection(Vector3.right);
		Vector3 pos = new Vector3(transform.position.x,transform.position.y - 0.3f,transform.position.z);
		Physics.Raycast(pos, dir, out rayHit, distanceAttack, playerLayer.value);		
		
		if (rageMode) {
			if(!soundBool){
				audio.PlayOneShot(beh);
				soundBool = true;
			}
			transform.GetComponentInChildren<Animation>().Play("attack");
			tempSpeed = speed * 3;
		} else if(!turnFlag){
			transform.GetComponentInChildren<Animation>().Play("walk");
			soundBool = false;
			tempSpeed = speed;
		}
		
		// The bode will walk from one edge of the platform to another, and stop for a while at each edge
		if (turnFlag && (transform.position == position1 || transform.position == position2)) {
			bodeStatus = status.stopped;
			audio.Stop();
			transform.GetComponentInChildren<Animation>().Play("idle");
		}
		
		if (bodeStatus == status.toP1) {
			transform.rotation = new Quaternion(0,0,0,0);
			transform.position = Vector3.MoveTowards(transform.position, position1, tempSpeed);
			turnFlag = true;
			if(!audio.isPlaying){
				audio.Play();
			}
			transform.GetComponentInChildren<Animation>().PlayQueued("walk");
		} else if (bodeStatus == status.toP2) {
			transform.rotation = new Quaternion(0,180.0f,0,0);
			transform.position = Vector3.MoveTowards(transform.position, position2, tempSpeed);
			turnFlag = true;
			if(!audio.isPlaying){
				audio.Play();
			}
			transform.GetComponentInChildren<Animation>().PlayQueued("walk");
		} else {
			timer += Time.deltaTime;
			
			// This is the amount of time the bode will remain stopped on the edge
			if (timer > timeStopped) {
				turnFlag = false;
				
				if (transform.position == position1) {
					bodeStatus = status.toP2;
					timer = 0;
				}
				
				if (transform.position == position2) {
					bodeStatus = status.toP1;
					timer = 0;
				}
			}
		}
		
		if (rageMode && player != null && rayHit.transform != player.transform) {
			// If the Bode cannot see the BadBode anymore, he'll get back to his routine.
			rageMode = false;
		} else if (!rageMode && player != null && rayHit.transform == player.transform) {
			// If the BadBode crosses the Bode's sight, he'll rush in its direction!
			rageMode = true;
		}
		
		Debug.DrawRay(pos,dir*distanceAttack,Color.green);
	}
	
	void OnCollisionEnter(Collision what) {
		if (player != null && what.gameObject == player && (player.GetComponent<DragShotMover>().GetActualForce() > 9.0f || player.GetComponent<DragShotMover>().GetActualForce() < -9.0f)) {
			died = true;
			transform.GetComponentInChildren<Animation>().Play("die");
			transform.GetComponent<CapsuleCollider>().isTrigger = true;
			audio.PlayOneShot(die);
			Destroy(transform.gameObject, 2f);
		}
	}
}
