using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class CardInstance 
{

	public Card card = null;

	public CardInstance(Card card)
	{
		this.card = card;
	}

	public float CardForceModifier(float chargePercent)
	{
		return card.chargeForce.Evaluate(chargePercent);
	}

	public void HandleImpact(Collision col, GameObject proj, int faction)
	{
		if (card is SummonCard)
		{
			for (int i = 0; i < ((SummonCard)card).numToSummon; i++)
			{
				GameObject prefab = ((SummonCard)card).summon;
				Vector3 spawnLoc = proj.transform.position + Vector3.right * i;
				GameObject spawned = (GameObject) GameObject.Instantiate(prefab, spawnLoc, prefab.transform.rotation);
				spawned.GetComponent<Character>().faction = faction;
			}

		}
		else if (card is DamageCard)
		{
			foreach (Character character in GetEffected(col, proj, faction))
				character.TakeDamage(((DamageCard)card).damage);
		}
		else if (card is StatusCard)
		{
			foreach (Character character in GetEffected(col, proj, faction))
				character.ApplyStatus(((StatusCard)card).status, ((StatusCard)card).value);
		}

		if (card.effectPrefab != null)
			GameObject.Instantiate(card.effectPrefab, proj.transform.position, card.effectPrefab.transform.rotation);
	}

	public List<Character> GetEffected(Collision col, GameObject proj, int faction)
	{
		List<Character> effected = new List<Character>();

		if (card.useExplosion)
		{
			foreach (Collider collider in Physics.OverlapSphere(proj.transform.position, card.explosionRadius, LOSManager.instance.characterLayer))
				if (EffectsCharacter(collider.gameObject, faction))
					effected.Add(collider.gameObject.GetComponent<Character>());

			Debug.Log ("explosion hit: " + effected.Count);
		}
		else 
		{
			if (EffectsCharacter(col.gameObject, faction))
				effected.Add(col.gameObject.GetComponent<Character>());
		}

		return effected;
	}

	bool EffectsCharacter(GameObject possibility, int faction)
	{
		if (possibility.GetComponent<Character>() == null)
			return false;

		if (possibility.GetComponent<Character>().faction != faction && card.affectsEnemies)
			return true;

		if (possibility.GetComponent<Character>().faction == faction && card.affectsFriendlies)
			return true;


		return false;
	}

	public bool CanAfford(PlayerCharacter playerCharacter)
	{
		return (playerCharacter.mana >= card.manaCost);
	}


}