//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2012 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;

/// <summary>
/// Sample script showing how easy it is to implement a standard button that swaps sprites.
/// </summary>

[ExecuteInEditMode]
[AddComponentMenu("NGUI/UI/Image Button")]
public class UIImageButton : MonoBehaviour
{
	
	
	public bool isOn; // Clicking will toggle the button on and off. Starts off
	//public string spriteName_On; // The name of the sprite when it's on
	//public string spriteName_Off; // The name of the sprite when it's off
	public UISprite target;
	public string normalSprite;
	public string hoverSprite;
	public string pressedSprite;


	void Start ()
	{
		isOn = true;
		if (target == null) target = GetComponentInChildren<UISprite>();
	}

	
	public void OnClick()
	{
	    if (isOn)
	    {
	        // Is on, turn off
	        target.spriteName = pressedSprite;
	    }
	    else
	    {
	        // Is off, turn on
	        target.spriteName = normalSprite;
	    }
	    
	    isOn = !isOn; // Toggle it so it flips between on and off
	}
}