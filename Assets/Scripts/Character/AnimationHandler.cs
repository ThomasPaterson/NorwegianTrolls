using UnityEngine;
using System.Collections;

public class AnimationHandler : MonoBehaviour 
{
	public AnimationCurve walkY;
	public AnimationCurve attackMagnitude;
	public float attackLength;

	private bool walking = false;
	private bool attacking = false;
	private Vector3 offset;
	private Rigidbody rigidBody;

	void Awake()
	{
		offset = transform.localPosition;
		rigidBody = GetComponentInParent<Rigidbody>();
	}

	void FixedUpdate()
	{
		if (rigidBody.velocity.magnitude <= 0.2f)
			walking = false;
		else if (!walking)
			StartCoroutine(Walk ());
	}

	public void SetAttacking(Vector3 pos)
	{
		StartCoroutine(Attack(pos));
	}


	IEnumerator Walk()
	{
		walking = true;
		float walkTime = 0f;

		while (walking)
		{
			if (!attacking)
				transform.localPosition = new Vector3(offset.x, offset.y + walkY.Evaluate(walkTime), offset.z);

			walkTime += Time.deltaTime;

			yield return null;
		}

		if (!attacking)
			transform.localPosition = offset;
	}

	IEnumerator Attack(Vector3 targetPos)
	{
		attacking = true;
		float attackTime = 0f;
		Vector3 direction = targetPos - transform.position;
		direction.y = 0f;
		
		while (attacking && attackTime < attackLength)
		{
			transform.localPosition = (direction*attackMagnitude.Evaluate(attackTime) + offset);
			attackTime += Time.deltaTime;
			yield return null;
		}

		attacking = false;
		transform.localPosition = offset;
	}

}
