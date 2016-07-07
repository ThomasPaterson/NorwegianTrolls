using UnityEngine;
using System.Collections;

public class GridDataHolder : MonoBehaviour 
{
	public GridData[] data;
	public Grid grid;
	public Generator generator;
	public bool singleplayer;


	public void SaveGrid()
	{
		data[0].SaveGrid(grid);
	}



	public GridData GetRandomLevel() 
	{

		int index = (int)Random.Range (0, data.Length);
		Debug.Log ("randomizing.." + index.ToString());
		return data [index];
	}

	public void GenerateNewGrid()
	{

		grid.GenerateGrid();
	}

	public void GridPrefabLoad()
	{
		
		grid.ConstructGrid(data[0]);
	}


}
