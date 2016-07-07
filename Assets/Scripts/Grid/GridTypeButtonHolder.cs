using UnityEngine;
using System.Collections;

public class GridTypeButtonHolder : MonoBehaviour 
{
	public static GridTypeButtonHolder instance;

	public GameObject buttonPrefab;
	public GridTypeButton selected;

	void Awake()
	{
		instance = this;
	}

	void Start()
	{
		GameObject button;
		int index = 0;

		foreach (GridLocationData data in GridLocationConfig.instance.locationData)
		{
			for (int i = 0; i < data.materials.Length; i++)
			{
				button = (GameObject) Instantiate(buttonPrefab);
				button.transform.parent = transform;
				button.GetComponent<GridTypeButton>().SetType(data, i);
				index++;
			}
		}

		button = (GameObject) Instantiate(buttonPrefab);
		button.transform.parent = transform;
		button.GetComponent<GridTypeButton>().isObstacle = true;
		button.GetComponent<GridTypeButton>().SetType(null, index);
		index++;

		button = (GameObject) Instantiate(buttonPrefab);
		button.transform.parent = transform;
		button.GetComponent<GridTypeButton>().isSpawnPoint = true;
		button.GetComponent<GridTypeButton>().SetType(null, index);
		index++;

		button = (GameObject) Instantiate(buttonPrefab);
		button.transform.parent = transform;
		button.GetComponent<GridTypeButton>().isManaGenerator = true;
		button.GetComponent<GridTypeButton>().SetType(null, index);
		index++;
	}

	public void Select(GridTypeButton button)
	{
		selected = button;
	}

}
