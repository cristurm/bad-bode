using UnityEngine;
using System.Collections;

/** TODO List:
 * - make configurable ranges!
 **/

public class Moss : MonoBehaviour {
	// Public vars
	public Material materialGreen, materialYellow, materialRed;
	public int scoreGreen, scoreYellow, scoreRed;
	
	// Private vars
	private enum mossTypes {Green, Yellow, Red}
	private mossTypes mossType;
	private float randomMoss, range1, range2;
	
	// Use this for initialization
	void Start () {		
		// Randomize which moss is gonna show up.
		randomMoss = Random.value;
		range1 = 0.70f;
		range2 = range1 + 0.25f;
		
		if (randomMoss <= range1) {
			mossType = mossTypes.Green;
			transform.GetComponentInChildren<SkinnedMeshRenderer>().renderer.material = materialGreen;
		} else if (randomMoss > range1 && randomMoss <= range2) {
			mossType = mossTypes.Yellow;
			transform.GetComponentInChildren<SkinnedMeshRenderer>().renderer.material = materialYellow;
		} else if (randomMoss > range2) {
			mossType = mossTypes.Red;
			transform.GetComponentInChildren<SkinnedMeshRenderer>().renderer.material = materialRed;
		}
	}
	
	void OnTriggerEnter (Collider what) {
		GameObject player = GameObject.FindWithTag("Player");
		
		if (what.gameObject == player) {
			switch (mossType) {
			case mossTypes.Green :
				player.GetComponent<BadBode>().IncreaseScore(scoreGreen, BadBode.ScoreIncreaser.Add);
				break;
			case mossTypes.Yellow	 :
				player.GetComponent<BadBode>().IncreaseScore(scoreYellow, BadBode.ScoreIncreaser.Add);
				break;
			case mossTypes.Red :
				player.GetComponent<BadBode>().IncreaseScore(scoreRed, BadBode.ScoreIncreaser.Multiply);
				break;
			}
			player.GetComponent<BadBodeAnimationandSound>().Eat();
			Destroy(gameObject);
		}
	}
}
