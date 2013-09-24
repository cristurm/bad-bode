using UnityEngine;
using System.Collections;

public class CamView : MonoBehaviour {
	// Public vars
	public float bottomDistance;
	public bool isPaused;
	
	// Private vars
	private GameObject player;
	private Vector3 camPos;
	
	// Use this for initialization
	void Start () {
		isPaused = false;
		player = GameObject.FindWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {		
		if (!player.GetComponent<BadBode>().GetIsDead()) {
			camPos = transform.position;
			camPos.y = player.transform.position.y + bottomDistance;
			transform.position = camPos;
		}
	}
}
