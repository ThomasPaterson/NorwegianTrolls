using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EndGamePlayerPanel : MonoBehaviour 
{

	public int playerNum;
	public Image backgroundImage;
	public Text wonText;

	void Start()
	{
		PlayerInstance playerInstance = PlayerInstanceManager.instance.players[playerNum];

		if (!playerInstance.active)
		{
			gameObject.SetActive(false);
			return;
		}

		Color backgroundColor = PlayerConfig.instance.factionColor[playerInstance.faction];
		backgroundColor.a = 0.3f;
		backgroundImage.color = backgroundColor;

		wonText.text = playerInstance.won ? "Won!" : "Lost";
	}

}
