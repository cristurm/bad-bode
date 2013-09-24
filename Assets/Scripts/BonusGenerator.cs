using UnityEngine;

/** TODO List:
 * - make configurable ranges!
 **/

public class BonusGenerator : MonoBehaviour {
	void Start () {
		// Randomize which bonus is gonna show up.
		float randomBonus = Random.value;
		float range0 = 0.4f;
		float range1 = range0 + 0.2f;
		float range2 = range1 + 0.2f;
		GameObject content = null;
		
		if(randomBonus > range0 && randomBonus <= range1) {
			GameObject titaniumHorn = (GameObject)Resources.Load("Bonus/TitaniumHorn");
			content = (GameObject)Instantiate(titaniumHorn, transform.position, transform.rotation);
		} else if (randomBonus > range1 && randomBonus <= range2) {
			GameObject gravityControl = (GameObject)Resources.Load("Bonus/GravityControl");
			content = (GameObject)Instantiate(gravityControl, transform.position, transform.rotation);
		} else if (randomBonus > range2) {
			GameObject drinkJump = (GameObject)Resources.Load("Bonus/DrinkJump");
			content = (GameObject)Instantiate(drinkJump, transform.position, transform.rotation);
		}
		
		if (content != null) {
			content.transform.parent = transform;
		}
	}
}