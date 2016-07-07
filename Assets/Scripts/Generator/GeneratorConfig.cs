using UnityEngine;
using System.Collections;

public class GeneratorConfig : MonoBehaviour {
	
	public static GeneratorConfig instance;

	public int mapHeight;
	public int mapWidth;

	public int maxRooms;
	public int minRoomSize;
	public int maxRoomSize;

	public int minCorridorWidth;
	public int maxCorridorWidth;

	public float standardDeviation;
	public int errorLimit;

	void Awake () {
		instance = this;
	}

}
