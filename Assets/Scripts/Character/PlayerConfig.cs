using UnityEngine;
using System.Collections;

public class PlayerConfig : MonoBehaviour 
{

	public static PlayerConfig instance;

	public GameObject playerPrefab;
	public Color[] factionColor;


	void Awake()
	{
		instance = this;
	}

}

