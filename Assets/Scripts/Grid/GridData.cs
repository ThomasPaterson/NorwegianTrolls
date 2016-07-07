using UnityEngine;
using System.Collections;

public class GridData : ScriptableObject 
{
	public int width;
	public int height;
	public GridLocationInfo[] locInfo;


	public void SaveGrid(Grid grid)
	{
		width = grid.width;
		height = grid.height;

		locInfo = new GridLocationInfo[width*height];

		int index = 0;

		for (int x = 0; x < width; x++)
			for (int y = 0; y < height; y++)
				SaveGridLoc(x, y, grid.gridLocations[x,y]);


	}

	public GridLocationInfo GetGridLocationInfo(int x, int y)
	{
		if (x*height + y < locInfo.Length)
			return locInfo[x*height + y];
		else
			return null;
	}

	void SaveGridLoc(int x, int y, GridLocation gridLocation)
	{
		locInfo[x*height + y] = new GridLocationInfo(gridLocation);
	}

}


[System.Serializable]
public class GridLocationInfo
{
	public GridLocation.Type type;
	public int matIndex;
	public bool spawnPoint;
	public bool endLevel;
	public bool hasObstacle;
	public int obstacleType;
	public bool hasManaGenerator;

	public GridLocationInfo(GridLocation gridLocation)
	{
		this.type = gridLocation.type;
		this.matIndex = gridLocation.matIndex;

		if (this.type != GridLocation.Type.Water)
		{
			this.spawnPoint = gridLocation.spawnPoint;
			this.endLevel = gridLocation.endLevel;
			this.hasObstacle = gridLocation.hasObstacle;
			this.hasManaGenerator = gridLocation.hasManaGenerator;
			this.obstacleType = gridLocation.obstacleType;
		}
	}

	public GridLocationInfo()
	{
	}

}