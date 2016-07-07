using UnityEngine;
using System.Collections;

public class LevelConfig : MonoBehaviour 
{
	public static LevelConfig instance;

	public GridData levelData;
	public GameObject manaCrystalPrefab;
	public float manaRechargeRate;
	public float firstManaRechargeRate;
	public ObstacleConfig obstacleConfig;
	public GameObject manaIndicator;
	public GameObject spawnIndicator;
	public GameObject obstacleIndicator;
	public GameObject endLevelPrefab;

	void Awake()
	{
		instance = this;

	}
}
