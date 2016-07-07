using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class WatchForStartGame : MonoBehaviour 
{
	public Text startText;

	void Update () 
	{
		if (ValidStartState())
		{
			startText.text = "Start game with start button or back for coop";

			if (CrossPlatformInputManager.GetButton("Submit"))
				Scenes.LoadLevel(Scenes.VERSUS);

			if (CrossPlatformInputManager.GetButton("Select"))
			{
				foreach (PlayerInstance playerInstance in PlayerInstanceManager.instance.players)
					playerInstance.faction = PlayerInstanceManager.instance.players[0].faction;

				Scenes.LoadLevel(Scenes.SINGLE_PLAYER);
			}
		}
		else if (ValidForSinglePlayer())
		{
			startText.text = "Need two or more teams or press back for singleplayer";

			if (CrossPlatformInputManager.GetButton("Submit") || CrossPlatformInputManager.GetButton("Select"))
				Scenes.LoadLevel(Scenes.SINGLE_PLAYER);
		}
		else
		{
			startText.text = "Add a player to play";
		}
			
	}

	bool ValidStartState()
	{
		int count = 0;
		List<int> factions = new List<int>();

		foreach (PlayerInstance playerInstance in PlayerInstanceManager.instance.players)
		{
			if (playerInstance.active && !factions.Contains(playerInstance.faction))
			{
				count++;
				factions.Add(playerInstance.faction);
			}
				
		}
			

		return count >= 2;

	}

	bool ValidForSinglePlayer()
	{

		foreach (PlayerInstance playerInstance in PlayerInstanceManager.instance.players)
			if (playerInstance.active)
				return true;

		return false;
		
	}
}
