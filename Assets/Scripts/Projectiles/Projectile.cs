using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour 
{

	public int damage;
	public CardInstance cardInstance = null;
	public int faction;
	public bool hasFired = false;
	public AudioClip destructionSound;

	void Start()
	{
		if (GetComponent<Renderer>() != null)
			GetComponent<Renderer>().material.color = (PlayerConfig.instance.factionColor[faction]/3f + GetComponent<Renderer>().material.color*2f/3f);
	}

	void OnCollisionEnter (Collision col)
	{
		if (hasFired)
			return;

		if (cardInstance != null && cardInstance.card != null)
		{
			if (cardInstance.card.impactSound != null)
				AudioSource.PlayClipAtPoint(cardInstance.card.impactSound, col.transform.position);

			cardInstance.HandleImpact(col, gameObject, faction);
		}
		else
		{
			if (destructionSound != null)
				AudioSource.PlayClipAtPoint(destructionSound, transform.position);

			if(col.gameObject.GetComponent<Character>() != null)
				if (col.gameObject.GetComponent<Character>().faction != faction)
					col.gameObject.GetComponent<Character>().TakeDamage(damage);
		}

		hasFired = true;

		Destroy(gameObject);
	}
}
