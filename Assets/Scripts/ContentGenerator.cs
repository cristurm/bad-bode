using UnityEngine;

/** TODO List:
 * - make configurable ranges!
 **/

public class ContentGenerator : MonoBehaviour {
	void Start () {
		Vector3 contentPosition;
		float myHeight, randomFoe, range1, range2;
		GameObject content;
		
		myHeight = transform.renderer.bounds.size.y;
		contentPosition = transform.position;
		contentPosition.y = transform.position.y + (myHeight * 0.5f);
		
		// randomize which foe is gonna show up.
		randomFoe = Random.value;
		range1 = 0.55f;
		range2 = range1 + 0.3f;
		content = null;
		
		if (randomFoe <= range1) {
			GameObject bat = (GameObject)Resources.Load("Enemies/Bat");
			float batHeight = bat.renderer.bounds.size.y;
			contentPosition.y = contentPosition.y + (batHeight * 0.8f);
			content = (GameObject)Instantiate(bat, contentPosition, transform.rotation);
		} else if (randomFoe > range1 && randomFoe <= range2) {
			GameObject bode = (GameObject)Resources.Load("Enemies/Bode");
			float bodeHeight = bode.renderer.bounds.size.y;
			contentPosition.y = contentPosition.y + (bodeHeight * 0.5f);
			content = (GameObject)Instantiate(bode, contentPosition, transform.rotation);
		} else if (randomFoe > range2) {
			GameObject yeti = (GameObject)Resources.Load("Enemies/Yeti");
			float yetiHeight = yeti.renderer.bounds.size.y;
			contentPosition.y = contentPosition.y + (yetiHeight * 0.5f);
			content = (GameObject)Instantiate(yeti, contentPosition, transform.rotation);
		}
		
		if (content != null) {
			content.transform.parent = transform;
		}
	}
}