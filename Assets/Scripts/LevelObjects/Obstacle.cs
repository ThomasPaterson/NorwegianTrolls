using UnityEngine;
using System.Collections;

public class Obstacle : MonoBehaviour 
{
	public float yHeight;

	public void SetOffBaseGrid(GridLocation gridLoc)
	{
		Vector3 newPos = gridLoc.transform.position + Vector3.up * yHeight;
		transform.position = newPos;
	}

}
