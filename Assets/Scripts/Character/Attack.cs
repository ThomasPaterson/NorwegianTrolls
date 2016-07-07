using UnityEngine;
using System.Collections;

public class Attack : MonoBehaviour 
{

	public float range;
	public float force;
	public float offset;
	public float reloadSpeed;
	public float attackLockTime;
	public GameObject projectile;
	public GameObject effectPrefab;
	public AudioClip attackSound;


	private float timeToNextShot;


	public bool CanAttack(GameObject target)
	{
		if ( timeToNextShot > 0f)
			return false;

		if (Vector3.Distance(target.transform.position, transform.position) > range)
			return false;

		if (!LOSManager.CanSee(transform.position, target.transform.position))
			return false;

		return true;
	}

	public bool WithinAttackRange(GameObject target)
	{

		if (Vector3.Distance(target.transform.position, transform.position) > range)
			return false;

		if (!LOSManager.CanSee(transform.position, target.transform.position))
			return false;

		
		return true;
	}


	public void MakeAttack(GameObject target)
	{
		GameObject cardProj = (GameObject)Instantiate(CardConfig.instance.cardPrefab, GetPositionOfShot(target), CardConfig.instance.cardPrefab.transform.rotation);
		cardProj.GetComponent<Projectile>().faction = GetComponent<Character>().faction;
		cardProj.GetComponent<Rigidbody>().AddForce(GetDirectionOfShot(target) * force);
		timeToNextShot = reloadSpeed;

		if (effectPrefab != null)
			SetupEffect(GetPositionOfShot(target));

		if (attackSound != null)
			AudioSource.PlayClipAtPoint(attackSound, transform.position);

		GetComponentInChildren<AnimationHandler>().SetAttacking(target.transform.position);
	}

	void Update()
	{
		if (timeToNextShot > 0f)
			timeToNextShot -= Time.deltaTime;
	}

	void SetupEffect(Vector3 positionToSpawn)
	{
		GameObject effect = (GameObject)Instantiate(effectPrefab, positionToSpawn, effectPrefab.transform.rotation);

		Vector3 localScale = effectPrefab.transform.localScale;
		localScale.x *= Mathf.Sign (GetComponent<Character>().toFlip.transform.localScale.x);
		effectPrefab.transform.localScale = localScale;
	}


	
	Vector3 GetDirectionOfShot(GameObject target)
	{
		return (target.transform.position - transform.position).normalized;
	}
	
	Vector3 GetPositionOfShot(GameObject target)
	{
		return (target.transform.position - transform.position).normalized * offset + transform.position + Vector3.up;
	}
}
