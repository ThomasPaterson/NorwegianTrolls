using UnityEngine;
using System.Collections;

public class DestroyAfterDelay : MonoBehaviour 
{

	public float timeToLive;

	IEnumerator Start()
	{
		yield return new WaitForSeconds(timeToLive);

		Destroy (gameObject);
	}
}
