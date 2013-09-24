using UnityEngine;
using System.Collections;

public class BadBode : MonoBehaviour {
	// Public vars
	public enum ScoreIncreaser {Add, Multiply};
	public int levelStep;
	
	// Private vars
	private int height, score, bestScore, level, prevLevel, jumpInt;
	private bool isDead, oldTime, doubleJump;
	//private GameObject gameCam;
	private GameObject finishGUI;
	private UIGuiScores heightGUI, scoreGUI, finalScoreGUI, bestScoreGUI;
	private float deathPoint, oldTimeScale;	
	private ScoreIncreaser scoreIncreaser;

	// Use this for initialization
	void Start () {
		isDead = false;
		height = 0;
		score = 0;
		level = (height / levelStep) + 1;
		prevLevel = level;
		//gameCam = GameObject.FindWithTag("MainCamera");
		finishGUI = (GameObject)Resources.Load("UI/Finish");
		heightGUI = GameObject.FindWithTag("Height").GetComponent<UIGuiScores>();
		scoreGUI = GameObject.FindWithTag("Score").GetComponent<UIGuiScores>();
		
		//mini doubleJump tutorial
		/*oldTime = false;
		doubleJump = false;
		oldTimeScale = 1.0f;
		jumpInt = PlayerPrefs.GetInt("tutoDoubleJump");*/
	}
	
	// Update is called once per frame
	void Update () {
		if (!isDead) {
			if (height < (int)transform.position.y) {
				height = (int)transform.position.y;
				
				/*if (prevHeight < height) {
					score += height - prevHeight;
				}*/
				
				//prevHeight = height;
				
				//mini doubleJump tutorial
				/*if(PlayerPrefs.GetInt("tutoDoubleJump") <= 2){
					if(height > 15 && GetComponent<DragShotMover>().movementStatus == DragShotMover.moveStatus.jump1 && !doubleJump){
						if(!oldTime){//seta somente 1 vez o timeScale atual
							oldTimeScale = Time.timeScale;
							oldTime = true;
						}
						Time.timeScale = 0.25F;//diminui a velocidade do jogo
						GetComponent<UIPanel>().enabled = true;//mostra label doubleJump
					}else if(height > 15 && GetComponent<DragShotMover>().movementStatus == DragShotMover.moveStatus.jump2 && !doubleJump){
						jumpInt += 1;
						PlayerPrefs.SetInt("tutoDoubleJump", jumpInt);
						Time.timeScale = oldTimeScale;//volta ao timeScale anterior
						GetComponent<UIPanel>().enabled = false;
						doubleJump = true;
					}
				}*/
			}
			
			heightGUI.Text =  height.ToString() + "m";
			scoreGUI.Text = score.ToString();
			level = (height / levelStep) + 1;
			
			if (level != prevLevel) {
				transform.GetComponent<PlatLoader>().GetNewPlatforms(level);
				prevLevel = level;
			}
		}
	}
	
	// IncreaseScore will raise the current score according to the parameters, this is called by the Mosses in "Moss.cs"
	public void IncreaseScore(int plusScore, ScoreIncreaser method) {
		switch (method) {
		case ScoreIncreaser.Add :
			score += plusScore;
			break;
		case ScoreIncreaser.Multiply :
			score *= plusScore;
			break;
		}
	}
	
	// Die will perform all actions needed when the player dies, such as showing the GameOver screen and saving the final score
	public void Die () {
		 // just to prevent everything to happen twice when your dead body hits something harmful
		if (!isDead) {
			isDead = true;
			
			// Instantiate the "Game Over" screen
			finishGUI = (GameObject)Instantiate(finishGUI);
			finishGUI.transform.parent = GameObject.FindWithTag("UI").transform;
			finishGUI.transform.localPosition = Vector3.zero;
			finishGUI.transform.localScale = new Vector3(1,1,1);
			finishGUI.transform.localRotation = new Quaternion(0,0,0,0);
			
			// Set the final score
			int finalScore = height + score; 
			finalScoreGUI = GameObject.FindWithTag("FinalScore").GetComponent<UIGuiScores>();
			finalScoreGUI.Text = finalScore.ToString();
			
			// Set the best score
			bestScoreGUI = GameObject.FindWithTag("BestScore").GetComponent<UIGuiScores>();
			bestScoreGUI.Text = PlayerPrefs.GetInt("BestScore").ToString();
			
			// Animate
			transform.GetComponent<BadBodeAnimationandSound>().Die();
			
			// Save player preferences
			if (finalScore > PlayerPrefs.GetInt("BestScore")) {
				PlayerPrefs.SetInt("BestScore", finalScore);
				bestScoreGUI.Text = finalScore.ToString();
			}
		}
	}
	
	// GetIsDead returns if the player is dead or not
	public bool GetIsDead () {
		return isDead;
	}
	
	// GetLevel returns the current level
	public int GetLevel () {
		return level;
	}
}
