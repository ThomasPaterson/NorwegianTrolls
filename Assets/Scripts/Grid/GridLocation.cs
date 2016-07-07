using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class GridLocation : MonoBehaviour, IDragHandler, IPointerClickHandler
{
	public const float WATER_HEIGHT = -0.5f;
	public const float SAND_HEIGHT = -0.1f;

	public enum Type {Grass,Sand, Dirt, Water, Rock}

	public Type type;
	public int matIndex;
	public Vector2 coord;
	public bool spawnPoint;
	public bool enemySpawn;
	public bool endLevel;
	public bool hasObstacle;
	public int obstacleType;
	public bool hasManaGenerator;
	public GameObject obstacleIndicator;
	public GameObject spawnIndicator;
	public GameObject manaIndicator;

	void Start()
	{
		if (Grid.instance.testScene)
			SetupTestScene();
	}


	public void SetType(Type newType, int newMatIndex)
	{
		this.type = newType;
		this.matIndex = newMatIndex;

		GetComponent<Renderer>().material = GridLocationConfig.instance.GetMaterial(type, newMatIndex);

		if (newType == Type.Water && !Grid.instance.testScene)
		{
			transform.position = (transform.position + Vector3.up * WATER_HEIGHT);
			Destroy(GetComponentInChildren<Collider>());
		}
		else if ((newType == Type.Dirt || newType == Type.Sand) && !Grid.instance.testScene)
		{
			transform.position = (transform.position + Vector3.up * SAND_HEIGHT);
		}
	}

	public void SetupGridLocationData(GridLocationInfo locData)
	{
		if (locData == null)
			return;

		hasObstacle = locData.hasObstacle;
		hasManaGenerator = locData.hasManaGenerator;
		spawnPoint = locData.spawnPoint;
		obstacleType = locData.obstacleType;
	}

	public void OnDrag(PointerEventData eventData)
	{

        if (MainCameraController.instance != null)
		    MainCameraController.instance.Move(eventData.delta);
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if (GridTypeButtonHolder.instance != null && GridTypeButtonHolder.instance.selected != null)
		{
			if (type != Type.Water && GridTypeButtonHolder.instance.selected.isObstacle)
			{
				hasObstacle = !hasObstacle;

				if (hasObstacle)
					obstacleIndicator = (GameObject)Instantiate(LevelConfig.instance.obstacleIndicator, transform.position + Vector3.up * 2f, Quaternion.identity);
				else
					Destroy (obstacleIndicator);
			}
				
			else if (type != Type.Water && GridTypeButtonHolder.instance.selected.isSpawnPoint)
			{
				spawnPoint = !spawnPoint;

				if (spawnPoint)
					spawnIndicator = (GameObject)Instantiate(LevelConfig.instance.spawnIndicator, transform.position + Vector3.up * 1.5f, Quaternion.identity);
				else
					Destroy (spawnIndicator);
			}


			else if (type != Type.Water && GridTypeButtonHolder.instance.selected.isManaGenerator)
			{
				hasManaGenerator = !hasManaGenerator;
				if (hasManaGenerator)
					manaIndicator = (GameObject)Instantiate(LevelConfig.instance.manaIndicator, transform.position + Vector3.up * 1.7f, Quaternion.identity);
				else
					Destroy (manaIndicator);
			}

			else
				SetType(GridTypeButtonHolder.instance.selected.data.type, GridTypeButtonHolder.instance.selected.index);
		}
	}

	public void SetupTestScene()
	{

			if (hasObstacle)
				obstacleIndicator = (GameObject)Instantiate(LevelConfig.instance.obstacleIndicator, transform.position + Vector3.up * 2f, Quaternion.identity);

			if (spawnPoint)
				spawnIndicator = (GameObject)Instantiate(LevelConfig.instance.spawnIndicator, transform.position + Vector3.up * 1.5f, Quaternion.identity);

			if (hasManaGenerator)
				manaIndicator = (GameObject)Instantiate(LevelConfig.instance.manaIndicator, transform.position + Vector3.up * 1.7f, Quaternion.identity);
			
	}

	public void AddEndLevel()
	{
		Instantiate(LevelConfig.instance.endLevelPrefab, transform.position + Vector3.up, Quaternion.identity);
	}

	public void AddObstacle()
	{
		int obType = Random.Range (0, 2);
		GameObject obstacle = (GameObject) Instantiate(LevelConfig.instance.obstacleConfig.obstacleTypes[obType]);
		obstacle.GetComponent<Obstacle>().SetOffBaseGrid(this);
	}


	public void AddMana()
	{
		gameObject.AddComponent<ManaGenerator>();
	}


}
