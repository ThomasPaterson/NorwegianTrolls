using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class WatchForReturnToHome : MonoBehaviour 
{
	public Text startText;

	void Update () 
	{

		if (CrossPlatformInputManager.GetButton("Submit"))
			Scenes.LoadLevel(Scenes.MAIN_MENU);

		if (CrossPlatformInputManager.GetButton("Select"))
			Scenes.LoadLevel(Scenes.MAIN_MENU);
	
			
	}


}
