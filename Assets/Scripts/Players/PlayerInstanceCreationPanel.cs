using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerInstanceCreationPanel : MonoBehaviour 
{
	public int playerNum;
	public int faction;

	public GameObject joinObject;
	public GameObject availableObject;
	public Text selectedDeck;

	public Image factionColor;
	public string changeDeckAxis;
	public string joinButton;
	public string changeFactionButton;
	public string quitButton;

	private bool movedHorizontal = false;

	void Update () 
	{
		if (CrossPlatformInputManager.GetButtonDown(joinButton))
			PlayerInstanceManager.instance.AddPlayer(playerNum, faction);

		if (PlayerInstanceManager.instance.players[playerNum].active && CrossPlatformInputManager.GetButtonDown(changeFactionButton))
			faction = PlayerInstanceManager.instance.ChangeFaction(playerNum);

		if (CrossPlatformInputManager.GetButtonDown(quitButton))
			PlayerInstanceManager.instance.RemovePlayer(playerNum);

		if (PlayerInstanceManager.instance.players[playerNum].active && CrossPlatformInputManager.GetAxis(changeDeckAxis) > 0.3f && !movedHorizontal)
		{
			PlayerInstanceManager.instance.IncrementDeck(playerNum, 1);
			movedHorizontal = true;
		}

		if (PlayerInstanceManager.instance.players[playerNum].active && CrossPlatformInputManager.GetAxis(changeDeckAxis) < -0.3f && !movedHorizontal)
		{
			PlayerInstanceManager.instance.IncrementDeck(playerNum, -1);
			movedHorizontal = true;
		}

		if (movedHorizontal && Mathf.Abs(CrossPlatformInputManager.GetAxis(changeDeckAxis)) < 0.3f)
			movedHorizontal = false;

		if (PlayerInstanceManager.instance.players[playerNum].active)
		{
			availableObject.SetActive(true);
			joinObject.SetActive(false);
			factionColor.color = PlayerConfig.instance.factionColor[faction];
			selectedDeck.text = CardConfig.instance.decks[PlayerInstanceManager.instance.players[playerNum].chosenDeck].deckName;
		}
		else
		{
			availableObject.SetActive(false);
			joinObject.SetActive(true);
		}	

	}
}
