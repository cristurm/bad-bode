using UnityEngine;
using System.Collections;

public class Yeti : MonoBehaviour {
	// Public vars
	public float speed, sightDistance;
	public LayerMask playerLayer;
	public AudioClip idle, groar;
	public float idleAudioTime; //time in seconds that sound is played
	
	// Private vars
	private GameObject myParent;
	private Vector3 position1, position2, tempPosition;
	private float parentWidthOffset, myWidthOffset, timer, timerConfused;
	private GameObject player;
	private bool isAttacking, isConfused;
	
	
	private Transform yeti;

	void Start () {
		yeti = transform.Find("YetiFBX");
		myParent = transform.parent.gameObject;
		sightDistance = myParent.renderer.bounds.size.x * 0.45f;
		parentWidthOffset = myParent.renderer.bounds.size.x * 0.5f;
		myWidthOffset = renderer.bounds.size.x * 0.5f;
		
		position1 = transform.position;
		position1.x = (position1.x + parentWidthOffset) - myWidthOffset;
		
		position2 = transform.position;
		position2.x = (position2.x - parentWidthOffset) + myWidthOffset;
		
		player = GameObject.FindWithTag("Player");
		
		
		 StartCoroutine(PlaySoundAfterDelay( idle, idleAudioTime ));
	}
	
	void OnCollisionEnter(Collision what) {
		if (player != null && what.gameObject == player && player.GetComponent<BadBodeAnimationandSound>().hasHorn) {
			isConfused = true;
			player.GetComponent<BadBodeAnimationandSound>().ToggleTitaniumHorn();
		}
	}
	
	void FixedUpdate () {
		if (isConfused) {
			transform.GetComponentInChildren<Animation>().Play("dizzy");
			
			timerConfused += Time.deltaTime;
			if(timerConfused > 2){
				isConfused = false;
				timerConfused = 0;
				transform.GetComponentInChildren<Animation>().Play("idle");
			}
		} else {
			RaycastHit rayHit1, rayHit2;
			
			Vector3 dir1 = transform.TransformDirection(Vector3.right);
			Vector3 dir2 = transform.TransformDirection(Vector3.left);
			Vector3 pos = new Vector3(transform.position.x,transform.position.y - 0.3f,transform.position.z);
			Physics.Raycast(pos, dir1, out rayHit1, sightDistance, playerLayer.value);
			Physics.Raycast(pos, dir2, out rayHit2, sightDistance, playerLayer.value);
			
			if (player != null) {
				if (rayHit1.transform == player.transform && transform.position.x < position1.x) {
					//at 9 o'clock!
					audio.volume = 0.3f;
					transform.position = Vector3.MoveTowards(transform.position, new Vector3(player.transform.position.x, transform.position.y, transform.position.z), speed);
					if (!audio.isPlaying) {
						audio.PlayOneShot(groar);
					}
					transform.GetComponentInChildren<Animation>().Play("attack");
					yeti.rotation =  Quaternion.Euler(0, 90, 0);
					isAttacking = true;
				}else{
					if(isAttacking){
						timer += Time.deltaTime;
						if(timer > 2){
							transform.GetComponentInChildren<Animation>().PlayQueued("idle");
							audio.volume = 1f;
							audio.PlayOneShot(idle);
							yeti.rotation =  Quaternion.Euler(0, 0, 0);
							timer = 0;
							isAttacking = false;
						}
					}
				}
				
				if (rayHit2.transform == player.transform && transform.position.x > position2.x) {
					//at 3 o'clock!
					audio.volume = 0.3f;
					transform.position = Vector3.MoveTowards(transform.position, new Vector3(player.transform.position.x, transform.position.y, transform.position.z), speed);
					if (!audio.isPlaying) {
						audio.PlayOneShot(groar);
					}
					transform.GetComponentInChildren<Animation>().Play("attack");
					yeti.rotation =  Quaternion.Euler(0, 270, 0);
					isAttacking = true;
				}
			}
			Debug.DrawRay(pos,dir1*sightDistance,Color.green);
			Debug.DrawRay(pos,dir2*sightDistance,Color.green);
		}
	}
	IEnumerator PlaySoundAfterDelay( AudioClip audioPlayed, float delay ){
	    if( audioPlayed == null){
	    	yield break;
		}
	    yield return new WaitForSeconds( delay );
		if(!audio.isPlaying){
			audio.PlayOneShot(audioPlayed);
		}
		StartCoroutine(PlaySoundAfterDelay( idle, idleAudioTime ));	
	    
    }
}
