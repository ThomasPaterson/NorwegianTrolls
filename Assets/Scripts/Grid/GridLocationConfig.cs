using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GridLocationConfig : MonoBehaviour 
{

	public static GridLocationConfig instance;

	public GridLocationData[] locationData;
	public GameObject gridLocationPrefab;
	
	public Dictionary<GridLocation.Type, GridLocationData> locationDict = new Dictionary<GridLocation.Type, GridLocationData>();

	void Awake()
	{
		instance = this;

		foreach (GridLocationData data in locationData)
			locationDict.Add(data.type, data);
	}

	public GridLocation.Type GetRandomType()
	{
		return locationData[Mathf.FloorToInt( Random.Range(0f, locationData.Length))].type;
	}


	public Material GetMaterial(GridLocation.Type type, int matIndex)
	{
		return locationDict[type].materials[Mathf.Min (matIndex, locationDict[type].materials.Length-1)];
	}

}


[System.Serializable]
public class GridLocationData
{
	public GridLocation.Type type;
	public Material[] materials;

}