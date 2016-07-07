using UnityEngine;
using System.Collections;

public class CardConfig : MonoBehaviour 
{
	public static CardConfig instance;
	
	public Deck[] decks;
	public GameObject cardPrefab;
	
	void Awake () 
	{
		instance = this;
	}
	
	public Card GetRandomCard()
	{
		return decks[0].cards[Mathf.FloorToInt(Random.Range(0f, decks[0].cards.Count))];
	}
}

