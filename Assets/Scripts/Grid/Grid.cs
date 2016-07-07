using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Grid : MonoBehaviour 
{
	public static Grid instance;

	public int width;
	public int height;
	public float gridGap;
	public bool testScene = false;
	public GridLocation[,] gridLocations;
	public List<GameObject> spawnPoints;

	public void ConstructGrid(int width, int height)
	{
		instance = this;

		this.width = width;
		this.height = height;
		gridLocations = new GridLocation[width,height];


		for (int x = 0; x < width; x++)
			for (int y = 0; y < height; y++)
				AddGridLocation(x, y);

	}

	public void ConstructGrid(GridData gridData)
	{
		Debug.Log ("using grid data: " + gridData.name);

		instance = this;
		spawnPoints = new List<GameObject>();
		this.width = gridData.width;
		this.height = gridData.height;
		gridLocations = new GridLocation[width,height];
		
		
		for (int x = 0; x < width; x++)
			for (int y = 0; y < height; y++)
				AddGridLocation(x, y, gridData.GetGridLocationInfo(x,y));


		
	}

	public void ConstructGrid(Generator generator)
	{
		spawnPoints = new List<GameObject>();

		this.width = generator.config.mapWidth;
		this.height = generator.config.mapHeight;
		gridLocations = new GridLocation[width,height];
		instance = this;

		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				GridLocationInfo locInfo = new GridLocationInfo();
				locInfo.type = (GridLocation.Type)generator.map[x + y * width];
				locInfo.matIndex = 0;
				locInfo.hasManaGenerator = generator.manaGenerators[x + y * width];
				if (generator.obstacles[x + y * width] > 0){
					locInfo.hasObstacle = true;
					locInfo.obstacleType = generator.obstacles[x + y * width] - 1;
				}
				foreach (Vector2 spawn in generator.spawns) {
					if( (int)spawn.x == x && (int)spawn.y == y)
						locInfo.spawnPoint = true;
				}
				if( (int)generator.end.x == x && (int)generator.end.y == y)
					locInfo.endLevel = true;

				AddGridLocation (x, y, locInfo);
			}
		}



	}


	void AddGridLocation(int x, int y, GridLocationInfo gridLocInfo = null)
	{

		GameObject loc = (GameObject) Instantiate(GridLocationConfig.instance.gridLocationPrefab, CalculateLocation(x, y), Quaternion.identity);
		gridLocations[x, y] = loc.GetComponent<GridLocation>();

		SetGridLocationType(loc.GetComponent<GridLocation>(), gridLocInfo);

		loc.GetComponent<GridLocation>().coord = new Vector2((float)x, (float)y);
		loc.transform.parent = transform;
	}


	void SetGridLocationType(GridLocation loc, GridLocationInfo gridLocInfo)
	{
		if (gridLocInfo != null)
		{
			loc.GetComponent<GridLocation>().SetType(gridLocInfo.type, gridLocInfo.matIndex);


			if (!testScene)
			{
				if (gridLocInfo.endLevel)
					loc.GetComponent<GridLocation>().AddEndLevel();
				else if (gridLocInfo.hasObstacle)
					loc.GetComponent<GridLocation>().AddObstacle();
				else if (gridLocInfo.hasManaGenerator)
					loc.GetComponent<GridLocation>().AddMana();
				
				if (gridLocInfo.spawnPoint)
					spawnPoints.Add (loc.gameObject);

			}
			else
			{
				loc.GetComponent<GridLocation>().SetupGridLocationData(gridLocInfo);
			}



		}
		else
		{
			GridLocation.Type gridType = GridLocationConfig.instance.GetRandomType();
			int matIndex = Mathf.FloorToInt(Random.Range(0f, GridLocationConfig.instance.locationDict[gridType].materials.Length));
			loc.GetComponent<GridLocation>().SetType(gridType, matIndex);
		}
	}



	Vector3 CalculateLocation(int x, int y)
	{
		Vector3 location = transform.position;
		float xChange = x - (width/2f);
		float yChange = y - (height/2f);
		location += new Vector3(xChange * gridGap, 0f, yChange * gridGap);

		return location;
	}

	public void GenerateGrid()
	{
		DestroyGrid();
		ConstructGrid(width, height);
	}

	public void DestroyGrid()
	{
		if (gridLocations != null)
			foreach (GridLocation loc in gridLocations)
				Destroy (loc.gameObject);

		gridLocations = null;
		instance = null;
	}

}
