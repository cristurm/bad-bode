using UnityEngine;
using System.Collections;

public class BadBodeAnimationandSound : MonoBehaviour {
	//pubic vars
	public MeshRenderer horn, titaniumHorn, matrixGlasses, ssj2, ssj3;//SuperSayaJin Hair, changed by DragShotMover Drink() - hasDrink
	public bool hasHorn, hasmatrixGlasses; //Called to verify if has or not horn active	
	public ParticleSystem hornHit; //Called when hasHorn and lose it
	public Transform ssjKI; //Called by DragShotMover Drink();
	public AudioClip idle;
	public AudioClip down;
	public AudioClip jump;
	public AudioClip doubleJump;
	public AudioClip died;
	public AudioClip eat;
	//sound ssj called by DragShotMover Drink() - hasDrink
	//public AudioClip ssj;
	public AudioClip matrixGlassesJump;
	
	//private vars
	private bool isDead;
	
	void Start(){
		hasHorn = false;
		hasmatrixGlasses = false;
		isDead = false;
		horn.enabled = true;
		titaniumHorn.enabled = false;
	}
	
	public void ToggleTitaniumHorn(){
		if(!hasHorn){
			titaniumHorn.enabled = true;
			horn.enabled = false;
			hasHorn = true;
		}else{
			hornHit.Play();
			titaniumHorn.enabled = false;
			horn.enabled = true;
			hasHorn = false;
		}
	}
	public void ToogleMatrixGlasses(){
		if(!hasmatrixGlasses){
			matrixGlasses.enabled = true;
			hasmatrixGlasses = true;
		}else{
			matrixGlasses.enabled = false;
			hasmatrixGlasses = false;
		}
	}
	
	public void Idle(){
		//called from DragShotMover - OnCollisionEnter
		if(!isDead){
			transform.GetComponentInChildren<Animation>().Play("idle");
			audio.PlayOneShot(idle);
		}
	}
	
	public void Down(){
		//called from DragShotMover - OnMouseDown
		if(!isDead){
			transform.GetComponentInChildren<Animation>().Play("down");
			audio.PlayOneShot(down);
		}
	}
	
	public void Jump(){
		//called from DragShotMover - OnMouseUp case jump 0
		if(!isDead){
			transform.GetComponentInChildren<Animation>().Play("jump");
			if(!hasmatrixGlasses){
				audio.PlayOneShot(jump);
			}else{
				audio.PlayOneShot(matrixGlassesJump);
			}
		}
	}
	
	public void DoubleJump(){
		//called from DragShotMover - OnMouseUp case jump 1
		if(!isDead){
			transform.GetComponentInChildren<Animation>().Play("doubleJump");
			if(!hasmatrixGlasses){
				audio.PlayOneShot(doubleJump);
			}else{
				audio.PlayOneShot(matrixGlassesJump);
			}
		}
	}
	
	public void Die(){
		//called from BadBode - updateIsDead
		isDead = true;
		transform.GetComponentInChildren<Animation>().Play("die");
		audio.PlayOneShot(died);
	}
	
	public void Eat(){
		//called from Moss - OnTriggerEnter
		audio.PlayOneShot(eat);
	}
}
