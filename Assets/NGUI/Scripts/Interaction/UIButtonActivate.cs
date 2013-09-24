//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2012 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;

/// <summary>
/// Very basic script that will activate or deactivate an object (and all of its children) when clicked.
/// </summary>

[AddComponentMenu("NGUI/Interaction/Button Activate")]
public class UIButtonActivate : MonoBehaviour
{
    public GameObject target;
    public bool state;
	
	void Start(){
		state = false;
		target = GameObject.FindWithTag("TutoHand");
	}

    void OnMouseDrag () { 
		if (target != null && !state) {
			NGUITools.SetActive(target, state);
			state = true;
		}
	}
}