using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerManager : MonoBehaviour 
{

	public static PlayerManager instance;

	public HandUIController[] handControllers;
	public FactionData[] factionData;
	public GameObject finishText;

	private List<GameObject> players = new List<GameObject>();

	private bool startedGame = false;
	private bool finishedGame = false;



	void Awake()
	{
		instance = this;
	}

	void Update()
	{
		if (startedGame && !finishedGame)
			CheckFinishedGame();
	}

	void CheckFinishedGame()
	{

		int count = 0;
		List<int> factions = new List<int>();
		
		foreach (GameObject player in players)
		{
			if (player != null)
			{
				if (!factions.Contains(player.GetComponent<PlayerCharacter>().playerInstance.faction))
					factions.Add(player.GetComponent<PlayerCharacter>().playerInstance.faction);
			}
		}

		if (factions.Count <= 1 && !LevelManager.instance.dataHolder.singleplayer)
			StartCoroutine(FinishGame());

		if (factions.Count == 0 && LevelManager.instance.dataHolder.singleplayer)
			Scenes.LoadLevel(Scenes.END_GAME_SCREEN);

	}

	IEnumerator FinishGame()
	{
		finishText.SetActive(true);
		int winningFaction = -1;

		yield return new WaitForSeconds(1f);

		foreach (GameObject player in players)
		{

			if (player != null)
			{
				winningFaction = player.GetComponent<PlayerCharacter>().playerInstance.faction;
				break;
			}

		}

		foreach (PlayerInstance playerInstance in PlayerInstanceManager.instance.players)
		{
			if (playerInstance.faction == winningFaction)
				playerInstance.won = true;
			else
				playerInstance.won = false;
		}

		Scenes.LoadLevel(Scenes.END_GAME_SCREEN);

	}

	public void SpawnPlayer()
	{

		for (int i = 0; i < PlayerInstanceManager.instance.players.Length; i++)
		{
			if (PlayerInstanceManager.instance.players[i].active)
			{
				SpawnPlayer (PlayerInstanceManager.instance.players[i], i);
			}
			else
			{
				Destroy(handControllers[i].gameObject);
			}
		}

		startedGame = true;
	}

	public void SpawnPlayer(PlayerInstance playerInstance, int playerNum)
	{
		playerInstance.won = false;

		GameObject player = (GameObject)Instantiate(PlayerConfig.instance.playerPrefab, GetSpawnLocation(playerNum), PlayerConfig.instance.playerPrefab.transform.rotation);
		player.GetComponent<ThirdPersonUserControl>().playerNum = playerNum+1;
		player.GetComponent<Character>().faction = playerInstance.faction;
		player.GetComponent<PlayerCharacter>().Init (playerInstance);
		player.GetComponent<ThirdPersonUserControl>().AssignHandController(handControllers[playerNum]);


		players.Add(player);
	}

	public Vector3 GetSpawnLocation(int playerNum)
	{
		if (Grid.instance.spawnPoints.Count > playerNum)
			return Grid.instance.spawnPoints[playerNum].transform.position + Vector3.up;

		return Vector3.up;
	}

}

[System.Serializable]
public class FactionData
{
	public Color color;
}
