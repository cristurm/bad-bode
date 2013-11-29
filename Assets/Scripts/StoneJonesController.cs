using UnityEngine;
using System.Collections;

public class StoneJonesController : MonoBehaviour {
	// Private vars
	private Transform player;
	private float timer, random, xPosition, actualChanceOfStones, rotateRocks;
	private Vector3 newPosition, initPosition, stoneRotation;
	private enum stoneStatuses {counting, preFall, falling};
	private stoneStatuses stoneStatus;
	private int level;
	
	
	// Public vars
	public Transform stoneJonesFBX, stoneJones;
	public GameObject[] fallingRocks;
	public Vector3[] firstPosition;
	public float timeOfWarning = 3.0f;
	public float gapBetweenStones = 10.0f;
	public float chanceOfStones = 0.1f;
	

	// Use this for initialization
	
	void Start () {		
		player = GameObject.FindWithTag("Player").transform;
		fallingRocks = GameObject.FindGameObjectsWithTag("FallingRocks");
		initPosition = stoneJones.localPosition;
		newPosition = stoneJones.localPosition;
		stoneRotation = stoneJonesFBX.eulerAngles;
		timer = 0;
		stoneStatus = stoneStatuses.counting;
		firstPosition[0] = fallingRocks[0].transform.localPosition;
		firstPosition[1] = fallingRocks[1].transform.localPosition;
		firstPosition[2] = fallingRocks[2].transform.localPosition;
		firstPosition[3] = fallingRocks[3].transform.localPosition;
		firstPosition[4] = fallingRocks[4].transform.localPosition;
		firstPosition[5] = fallingRocks[5].transform.localPosition;
	}
	
	// Update is called once per frame
	void Update () {
		switch (stoneStatus) {
		case stoneStatuses.counting :
			Counting();
			break;
			
		case stoneStatuses.preFall :
			PreFall();
			break;
			
		case stoneStatuses.falling :
			Falling();
			break;
		}
	}
	
	void Counting () {
		timer += Time.deltaTime;
		
		if (timer > gapBetweenStones) {
			random = Random.value;
			
			//the stone will have +1% each level
			level = player.GetComponent<BadBode>().GetLevel();
			actualChanceOfStones = chanceOfStones + (float)level * 0.01f;
			
			if (random < actualChanceOfStones) {
				//Debug.Log(random);
				//Debug.Log(actualChanceOfStones);
				/**
				 * left: -5
				 * center: 0
				 * right: 5
				 **/
				random = Random.value;
				
				if (random < 0.33f) {
					xPosition = -5.0f;
					
				} else if (random >= 0.33f & random < 0.66f) {
					xPosition = 0;
					
				} else if (random >= 0.66f) {
					xPosition = 5.0f;
					
				}
				
				newPosition = stoneJones.localPosition;
				newPosition.x = xPosition;
				stoneJones.localPosition = newPosition;
				
				stoneStatus = stoneStatuses.preFall;
			}
			
			timer = 0;
		}
	}
	
	void PreFall () {
		if(timer == 0){
			stoneJones.audio.Play();
		}
		
		
		timer += Time.deltaTime;
		
		foreach(GameObject fallRock in fallingRocks) {
			fallRock.transform.parent = null;
			fallRock.transform.rigidbody.useGravity = true;
			rotateRocks += 50.0f * Time.deltaTime;
			fallRock.transform.eulerAngles = new Vector3(rotateRocks,rotateRocks,rotateRocks);
		}
		
		if (timer > timeOfWarning) {
			ResetFallingRocks();
			
			timer = 0;
			stoneStatus = stoneStatuses.falling;
		}
	}
	
	void Falling () {
		/*foreach(GameObject fallRock in fallingRocks) {
			//fallRock.transform.GetComponentInChildren<MeshRenderer>().enabled = false;
		}*/
		stoneJones.parent = null;
		stoneJones.rigidbody.useGravity = true;
		
		stoneRotation.x += 100.0f * Time.deltaTime;
		stoneJonesFBX.eulerAngles = stoneRotation;
		
		if (stoneJones.position.y < (player.position.y - 20.0f)) {
			ResetStoneJones();
			ResetFallingRocks();
			
			stoneStatus = stoneStatuses.counting;
		}
	}
	
	void ResetStoneJones () {
		stoneJones.parent = transform;
		stoneJones.localPosition = initPosition;
		stoneJones.rigidbody.velocity = Vector3.zero;
		stoneJones.rigidbody.useGravity = false;
	}
	
	void ResetFallingRocks () {
		foreach(GameObject fallRock in fallingRocks) {
			fallRock.transform.parent = stoneJones;
			//fallRock.transform.localPosition = Vector3.zero;
			fallRock.transform.rigidbody.velocity = Vector3.zero;
			fallRock.transform.rigidbody.useGravity = false;
			//fallRock.transform.GetComponentInChildren<Renderer>().enabled = true;
		}
		fallingRocks[0].transform.localPosition = firstPosition[0];
		fallingRocks[1].transform.localPosition = firstPosition[1];
		fallingRocks[2].transform.localPosition = firstPosition[2];
		fallingRocks[3].transform.localPosition = firstPosition[3];
		fallingRocks[4].transform.localPosition = firstPosition[4];
		fallingRocks[5].transform.localPosition = firstPosition[5];
		
	}
}
