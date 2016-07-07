using UnityEngine;
using System.Collections;
using Pathfinding;

public class LevelManager : MonoBehaviour 
{

	public static LevelManager instance;

	public Grid grid;
	public GridDataHolder dataHolder;
	public GameObject endLevelText;
	public GameObject[] monsters;

	void Awake()
	{
		instance = this;
	}

	IEnumerator Start()
	{

		if (dataHolder.singleplayer)
		{
			dataHolder.generator.GenerateMap ();
			grid.ConstructGrid (dataHolder.generator);
		}
		else
			grid.ConstructGrid(dataHolder.GetRandomLevel());

		yield return null;

		if (dataHolder.singleplayer) {
			yield return null;
			dataHolder.generator.SpawnMonsters();
		}

		AstarPath.active.Scan();

		yield return null;

		PlayerManager.instance.SpawnPlayer();

	}
}
