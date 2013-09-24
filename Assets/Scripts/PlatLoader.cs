using UnityEngine;
using System.Collections.Generic;

public class PlatLoader : MonoBehaviour {
	// Public vars
	public int doNotRepeat = 3;
	public int numberOfPlatforms = 10;
	public int numberOfPlatLevels = 5;
	public List<GameObject> gamePlatforms, currentPlatforms;
	public List<int> unpickableIndexes;
	
	// Private vars
	private string basicDir;
	private Vector3 initPos = Vector3.zero;
	private GameObject floor, newPlatform;
	private float prefabHeight;
	private int updateOn;
	private Snow snowScript;
	private BadBode badBodeScript;
	private bool changeFlag;
	System.Random rand = new System.Random();

	// Use this for initialization
	void Start () {
		badBodeScript = transform.GetComponent<BadBode>();
		snowScript = GameObject.FindWithTag("Snow").GetComponent<Snow>();
		GetNewPlatforms(badBodeScript.GetLevel());
		floor = GameObject.FindWithTag("Floor");
		changeFlag = false;
		
		// Set our initial Platforms
		// [0] (it's the floor -Chao-)
		currentPlatforms.Add(floor);
		initPos = floor.transform.position;
		// [1]
		UpdateUnpickablesLine(0);
		prefabHeight = gamePlatforms[0].renderer.bounds.size.y;
		initPos = new Vector3(initPos.x, initPos.y + prefabHeight, -0.5f);
		currentPlatforms.Add((GameObject)Instantiate(gamePlatforms[0], initPos, gamePlatforms[0].transform.rotation));
		// [2]
		UpdateUnpickablesLine(1);
		prefabHeight = gamePlatforms[1].renderer.bounds.size.y;
		initPos = new Vector3(initPos.x, initPos.y + prefabHeight, -0.5f);
		currentPlatforms.Add((GameObject)Instantiate(gamePlatforms[1], initPos, gamePlatforms[1].transform.rotation));
		// [3]
		/*UpdateUnpickablesLine(2);
		prefabHeight = gamePlatforms[2].renderer.bounds.size.y;
		initPos = new Vector3(initPos.x, initPos.y + prefabHeight, -0.5f);
		currentPlatforms.Add((GameObject)Instantiate(gamePlatforms[2], initPos, gamePlatforms[2].transform.rotation));
		// [4]
		UpdateUnpickablesLine(3);
		prefabHeight = gamePlatforms[3].renderer.bounds.size.y;
		initPos = new Vector3(initPos.x, initPos.y + prefabHeight, -0.5f);
		currentPlatforms.Add((GameObject)Instantiate(gamePlatforms[3], initPos, gamePlatforms[3].transform.rotation));*/
		
		/** 
		 * currentPlatforms: will be used to map the current/previous/next platforms and
		 * test if the player touched the latest platform in order to load a new one
		 **/
		
		//updateOn = currentPlatforms.Count - 2;
		updateOn = currentPlatforms.Count-1;
	}
	
	// Update is called once per frame
	void Update () {		
		if (transform.position.y > currentPlatforms[updateOn].transform.position.y - (gamePlatforms[updateOn].renderer.bounds.size.y * 0.5f)) {
			LoadNextPlatform();
			
			float newSnowPos = currentPlatforms[0].transform.position.y - (gamePlatforms[0].renderer.bounds.size.y * 0.5f);
			snowScript.UpdatePositionY(newSnowPos);
			
		}
	}
	
	// Private functions
	private void LoadNextPlatform () {
		int randIndex = rand.Next(gamePlatforms.Count);
		
		while (unpickableIndexes.Contains(randIndex)) {
			randIndex = rand.Next(gamePlatforms.Count);
		}		
		
		UpdateUnpickablesLine(randIndex);
		prefabHeight = gamePlatforms[randIndex].renderer.bounds.size.y;
		initPos = new Vector3(0, initPos.y + prefabHeight, -0.5f);
		newPlatform = (GameObject)Instantiate(gamePlatforms[randIndex], initPos, gamePlatforms[randIndex].transform.rotation);
		
		// If we are loading new platforms, we must then set a intermediate background material so it doesn't look funky
		if (changeFlag) {
			int prevLevel = badBodeScript.GetLevel() - 1;
			Material intermediateMaterial = (Material)Resources.Load("Material/fundo" + prevLevel + "-" + badBodeScript.GetLevel());
			
			newPlatform.transform.FindChild("back").renderer.material = intermediateMaterial;
			changeFlag = false;
		}
		
		// First we get rid of that old platform, we are not getting there anymore
		Destroy(currentPlatforms[0]);
		
		/*
		 * Second we shift our platforms in the currentPlatforms array
		 * Read line 55 for more details. :)
		 */
		currentPlatforms[0] = currentPlatforms[1];
		currentPlatforms[1] = currentPlatforms[2];
		currentPlatforms[2] = newPlatform;
		/*currentPlatforms[2] = currentPlatforms[3];
		currentPlatforms[3] = currentPlatforms[4];
		currentPlatforms[4] = newPlatform;*/	
	}
	
	// UpdateUnpickablesLine will keep a line of indexes that cannot be picked on the next rows
	private void UpdateUnpickablesLine (int index) {
		unpickableIndexes.Add(index);
		
		if (unpickableIndexes.Count > doNotRepeat) {
			unpickableIndexes.RemoveAt(0);
		}
	}
	
	// GetPrefabs will look in a directory for all prefabs in it and add them to the prefabsArray
	private List<GameObject> GetPrefabs (int level) {
		string platName, folderName;
		List<GameObject> prefabsArray = new List<GameObject>();
		folderName = "Level" + level;
		
		for(int i = 1; i <= numberOfPlatforms; i++) {
			platName = "Plat" + i;
			prefabsArray.Add((GameObject)Resources.Load("Platforms/" + folderName + "/" + platName));
		}
		
		return prefabsArray;		
	}
	
	// ShuffleGOList will shuffle our platforms array in order to start a new game with random platforms
	private List<GameObject> ShuffleGOList (List<GameObject> list) {
		int listSize = list.Count;
		List<GameObject> shuffledList = list;
		
		while (listSize > 1) {
			listSize--;
			int randIndex = rand.Next(listSize + 1);
			GameObject obj =  shuffledList[randIndex];
			shuffledList[randIndex] = shuffledList[listSize];
			shuffledList[listSize] = obj;
		}
		
		
		return shuffledList;
	}
	
	// Public functions
	// GetNewPlatforms is called by the "BadBode" script in order to load platforms from the current levels
	public void GetNewPlatforms (int level) {
		if (level <= numberOfPlatLevels) {
			gamePlatforms = GetPrefabs(badBodeScript.GetLevel());
			gamePlatforms = ShuffleGOList(gamePlatforms);
			changeFlag = true;
		}
	}
}
