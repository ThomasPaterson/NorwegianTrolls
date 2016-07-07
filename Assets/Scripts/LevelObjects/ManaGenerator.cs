using UnityEngine;
using System.Collections;

public class ManaGenerator : MonoBehaviour 
{
	private GameObject generatedMana = null;
	private bool generating = false;
	private bool hasGenerated = false;

	void Update () 
	{

		if (generatedMana == null && !generating)
			StartCoroutine(GenerateMana());
	
	}

	IEnumerator GenerateMana()
	{
		generating = true;

		yield return new WaitForSeconds(DetermineWait());

		generatedMana = (GameObject) Instantiate(LevelConfig.instance.manaCrystalPrefab, transform.position + Vector3.up, Quaternion.identity);
		generatedMana.GetComponent<ManaCrystal>().Spawn();

		generating = false;
		hasGenerated = true;

	}


	float DetermineWait()
	{
		if (hasGenerated)
			return LevelConfig.instance.manaRechargeRate;
		else
			return LevelConfig.instance.firstManaRechargeRate;
	}
}
