using UnityEngine;
using System.Collections;

public class BadBodeAreaColisor : MonoBehaviour {
	
	public DragShotMover dragShotMover;
	private GameObject player;
	
	// Use this for initialization
	void Start () {
		player = GameObject.FindWithTag("Player");
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(player != null){
			transform.position = player.transform.position;
		}
	}
	void  OnMouseDown (){
		dragShotMover.MouseDown();
	}
	
	void OnMouseDrag () {
		dragShotMover.MouseDrag();
	}
	
	void  OnMouseUp (){
		dragShotMover.MouseUp();
	}

}
