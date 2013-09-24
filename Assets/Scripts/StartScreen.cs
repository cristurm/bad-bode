using UnityEngine;
using System.Collections;

public class StartScreen : MonoBehaviour {
	private UIGuiScores bestScoreGUI;
	private GameObject player;
	private float timer;
	
	void Start () {
		bestScoreGUI = GameObject.FindWithTag("BestScore").GetComponent<UIGuiScores>();
		bestScoreGUI.Text = PlayerPrefs.GetInt("BestScore").ToString();
		
		player = GameObject.FindWithTag("Player");
	}
	
	void FixedUpdate(){
		timer += Time.deltaTime;
		
		if(timer > 10.0f){
			player.GetComponentInChildren<Animation>().CrossFade("jump");
			player.GetComponentInChildren<Animation>().PlayQueued("idle");
			timer = 0.0f;
		}
	}
}
