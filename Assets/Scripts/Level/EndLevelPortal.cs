using UnityEngine;
using System.Collections;

public class EndLevelPortal : MonoBehaviour 
{

	void OnTriggerEnter(Collider other) 
	{
		if (other.GetComponent<PlayerCharacter>() != null)
			StartCoroutine(LoadLevelAfterDelay());
	}

	IEnumerator LoadLevelAfterDelay()
	{
		LevelManager.instance.endLevelText.SetActive(true);

		yield return new WaitForSeconds(1f);

		Scenes.LoadLevel(Scenes.SINGLE_PLAYER);
	}
}
