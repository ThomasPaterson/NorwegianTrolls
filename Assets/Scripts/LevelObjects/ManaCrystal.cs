using UnityEngine;
using System.Collections;

public class ManaCrystal : MonoBehaviour 
{
	private bool hasFired = false;
	public int value = 21;

	public void Spawn()
	{

	}

	public void FixedUpdate()
	{
		if (transform.position.y < -1f)
			Destroy (gameObject);
	}


	void OnCollisionEnter (Collision col)
	{

		if (!hasFired && col.transform.GetComponent<PlayerCharacter>() != null)
		{
			col.transform.GetComponent<PlayerCharacter>().PickupMana(value);

			Destroy(gameObject);
		}

	}
}
