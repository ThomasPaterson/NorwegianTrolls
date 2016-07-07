using UnityEngine;
using System.Collections;

public class CharacterAI : MonoBehaviour 
{
	public const float WIDE_SWEEP_RANGE = 20f;

	public LayerMask targetSearchMask;
	public float detectionRange;
	public float aggroRange;
	public float searchFrequency;
	public GameObject target;
	public AudioClip foundSearchSound;

	private Character character;
	private Attack attack;
	private Pathfinder pathfinder;
	public bool searching = false;
	public bool attackLocked = false;


	void Awake()
	{
		attack = GetComponentInChildren<Attack>();
		character = GetComponentInChildren<Character>();
		pathfinder = GetComponentInChildren<Pathfinder>();
	}

	void Start()
	{
		StartCoroutine(CheckAggro());
	}

	void FixedUpdate()
	{
		if (!searching && !attackLocked)
		{

			if (target == null)
				StartCoroutine(SearchForTarget());
			else if (attack.CanAttack(target))
			{
				attack.MakeAttack(target);
				StartCoroutine(LockForAttack());
			}
            else if (WithinRange(target, detectionRange * 3f))
                MoveTowardsTarget();
			else
				target = null;
		}

		if (target != null)
		{
			character.FlipDirection(target.transform.position - transform.position);
			GetComponent<Character>().ShowDirectionArrow(target.transform.position - transform.position);
		}
		else
			GetComponent<Character>().HideDirectionArrow();



		         
	}

	IEnumerator SearchForTarget()
	{
		searching = true;

		while (target == null)
		{
	
			target = FindBestTarget(Physics.OverlapSphere(transform.position, detectionRange,targetSearchMask));

			yield return new WaitForSeconds(searchFrequency);
		}

		if (foundSearchSound != null)
			AudioSource.PlayClipAtPoint(foundSearchSound, transform.position);

		searching = false;

	}

	IEnumerator CheckAggro()
	{
		while (true)
		{

			if (target != null)
			{
				bool canSee = CanSee(target);
				bool withinAggro = WithinRange(target, aggroRange * 1.2f);

				if (!withinAggro || !canSee)
				{
					GameObject newPossibility = FindBestTarget(Physics.OverlapSphere(transform.position, detectionRange,targetSearchMask));

					if (newPossibility != null || (!withinAggro && !canSee))
						target = newPossibility;
				}

			}
			
			yield return new WaitForSeconds(searchFrequency);
		}
	}

	public void WideSweep()
	{
		if (target != null)
			return;

		target = FindBestTarget(Physics.OverlapSphere(transform.position, WIDE_SWEEP_RANGE,targetSearchMask));

		if (target != null)
			if (foundSearchSound != null)
				AudioSource.PlayClipAtPoint(foundSearchSound, transform.position);

	}

	IEnumerator LockForAttack()
	{
		attackLocked = true;

		yield return new WaitForSeconds(attack.attackLockTime);

		attackLocked = false;
		
	}

	void MoveTowardsTarget()
	{
		if (!attack.WithinAttackRange(target))
			pathfinder.MoveTowardsTarget(target);
	}

	GameObject FindBestTarget(Collider[] possibilities)
	{
		GameObject bestTarget = null;
		float bestScore = float.MaxValue;

		foreach (Collider col in possibilities)
		{
			if (col.gameObject.GetComponent<Character>() != null)
			{
				if (col.gameObject.GetComponent<Character>().faction != character.faction)
				{
					float distance = Vector3.Distance(col.transform.position, transform.position);

					if (distance < bestScore && CanSee(col.gameObject))
					{
						bestScore = distance;
						bestTarget = col.gameObject;
					}
				}
			}
		}

		return bestTarget;
	}



	bool WithinRange(GameObject toCheck, float range)
	{
		return Vector3.Distance(toCheck.transform.position, transform.position) <= range;
	}

	bool CanSee(GameObject target)
	{
		return LOSManager.CanSee(transform.position, target.transform.position);
	}



}
