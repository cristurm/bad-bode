using UnityEngine;
using System.Collections;

public class UIGuiScores : MonoBehaviour {
	
	private UILabel lbl;
	
	// Use this for initialization
	void Awake () {
		lbl = GetComponent<UILabel>();
		if(lbl == null){
			Debug.LogError("could not find UILabel");
		}
	}
	
	public string Text{
		get {
			return lbl.text;
		}
		set{
			lbl.text = value;
		}
	}
}
