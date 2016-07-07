using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GridTypeButton : MonoBehaviour, IPointerClickHandler
{

	public GridLocationData data;
	public int index;
	public bool isObstacle;
	public bool isSpawnPoint;
	public bool isManaGenerator;

	

	public void SetType(GridLocationData data, int index)
	{
		this.data = data;
		this.index = index;

		if (isObstacle)
			GetComponentInChildren<Text>().text = "Add Obstacle";
		else if (isSpawnPoint)
			GetComponentInChildren<Text>().text = "Add Spawn";
		else if (isManaGenerator)
			GetComponentInChildren<Text>().text = "Add Mana";
		else
			GetComponentInChildren<Text>().text = data.type.ToString() + " " + index.ToString();

	}

	public void OnPointerClick(PointerEventData eventData)
	{
		GridTypeButtonHolder.instance.Select(this);
	}

}
