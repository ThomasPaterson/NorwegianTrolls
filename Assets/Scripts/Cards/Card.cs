using UnityEngine;
using System.Collections;

public class Card : ScriptableObject 
{

	public string cardName;
	public Sprite cardSprite;
	public float maxCharge;
	public AnimationCurve chargeForce;
	public int manaCost;
	public float explosionRadius;
	public bool useExplosion;
	public bool affectsFriendlies = false;
	public bool affectsEnemies = true;
	public GameObject effectPrefab;
	public AudioClip impactSound;
	
}
