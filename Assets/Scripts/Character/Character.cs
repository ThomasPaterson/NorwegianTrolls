using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour
{
	public const float HASTE_MOD = 0.8f;
	public const float SLOW_MOD = -0.6f;

	public int health = 3;
	public int faction;
	public SpriteRenderer factionRing;
	public GameObject toFlip;
	public GameObject directionArrow;
	public AudioClip deathSound;

	public bool slowed;
	private float slowDuration;

	public bool hasted;
	private float hasteDuration;

	private int maxHealth;


	protected virtual void Start()
	{
		maxHealth = health;
		factionRing.color = PlayerConfig.instance.factionColor[faction];

		if (directionArrow != null)
			directionArrow.GetComponent<SpriteRenderer>().color = PlayerConfig.instance.factionColor[faction];
	}


	public void TakeDamage(int damage)
	{
		health -= damage;

		if (health <= 0)
			Die();

		if (GetComponent<CharacterAI>() != null)
			GetComponent<CharacterAI>().WideSweep();
	}

	public void FlipDirection(Vector3 direction)
	{

		if (toFlip == null)
			return;

		Vector3 scale = toFlip.transform.localScale;


		if (direction.x < 0)
			scale.x = Mathf.Abs(scale.x);
		else
			scale.x = Mathf.Abs(scale.x) * -1f;

		toFlip.transform.localScale = scale;
			
	}

	public void ShowDirectionArrow(Vector3 direction)
	{
		if (directionArrow == null)
			return;

		directionArrow.SetActive(true);
		
		float angle = Mathf.Atan2(direction.z * -1f, direction.x) * Mathf.Rad2Deg;
		
		Quaternion temp  = (Quaternion.AngleAxis(angle, Vector3.up));
		temp.eulerAngles = new Vector3(temp.eulerAngles.x + 90f, temp.eulerAngles.y, temp.eulerAngles.z);
		directionArrow.transform.rotation = temp;
	}


	public void HideDirectionArrow()
	{
		if (directionArrow != null)
			directionArrow.SetActive(false);
	}
	void Update()
	{
		if (transform.position.y < -1f)
			Die ();
	}

	void Die()
	{
		Camera.main.GetComponent<SmoothFollow>().RemoveTarget(gameObject);

		if (deathSound != null)
			AudioSource.PlayClipAtPoint(deathSound, transform.position);

		Destroy (gameObject);
	}

	public void Heal(int healAmount)
	{
		health = Mathf.Min(health + healAmount, maxHealth);
	}


	public void ApplyHaste(float hasteTime)
	{
		if (!hasted)
			StartCoroutine(HandleHaste(hasteTime));
		else
			this.hasteDuration += hasteTime;
	}

	IEnumerator HandleHaste(float hasteTime)
	{
		hasted = true;
		hasteDuration = hasteTime;

		while (hasteDuration >= 0f)
		{
			hasteDuration -= Time.deltaTime;
			yield return null;
		}

		hasted = false;
	}

	public void ApplySlow(float slowTime)
	{
		if (!slowed)
			StartCoroutine(HandleSlow(slowTime));
		else
			this.slowDuration += slowTime;
	}
	
	IEnumerator HandleSlow(float slowTime)
	{
		slowed = true;
		slowDuration = slowTime;
		
		while (slowDuration >= 0f)
		{
			slowDuration -= Time.deltaTime;
			yield return null;
		}
		
		slowed = false;
	}

	public float GetMoveModifiers()
	{
		float mod = 0f;

		if (hasted)
			mod += HASTE_MOD;

		if (slowed)
			mod += SLOW_MOD;

		return mod;
	}

	public void ApplyStatus(StatusCard.Status type, float value)
	{
		switch (type)
		{
		case StatusCard.Status.Haste:
			ApplyHaste(value);
			break;
		case StatusCard.Status.Heal:
			Heal((int)value);
			break;
		case StatusCard.Status.Slow:
			ApplySlow(value);
			break;
		}
	}

}
