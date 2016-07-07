using UnityEngine;
using System.Collections;

public class PlayerInstanceManager : MonoBehaviour 
{

	public static PlayerInstanceManager instance;

	public PlayerInstance[] players = new PlayerInstance[4];

	void Awake()
	{
		if (instance == null)
		{
			instance = this;
			DontDestroyOnLoad(gameObject);


		}
		else
			Destroy (gameObject);
	}

	public void AddPlayer(int index, int faction)
	{
		if (index >= players.Length)
			return;

		players[index].active = true;
		players[index].faction = faction;
	}

	public void RemovePlayer(int index)
	{
		if (index >= players.Length)
			return;
		
		players[index].active = false;
	}

	public void IncrementDeck(int index, int value)
	{
		players[index].chosenDeck += value;
		
		if (players[index].chosenDeck >=  CardConfig.instance.decks.Length)
			players[index].chosenDeck = 0;
		else if (players[index].chosenDeck < 0)
			players[index].chosenDeck = CardConfig.instance.decks.Length-1;

	}

	public int ChangeFaction(int index)
	{

		players[index].faction += 1;

		if (players[index].faction >= 4)
			players[index].faction = 0;

		return players[index].faction;

	}
	
}
