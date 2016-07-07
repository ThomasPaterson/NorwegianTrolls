using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Deck  
{

	public string deckName;
	public List<Card> cards;


	public Deck(List<Card> available, int cardNum = 30)
	{
		int index = 0;
		cards = new List<Card>();

		for (int i = 0; i < cardNum; i++)
		{
			if (index >= available.Count)
				index = 0;

			cards.Add(available[index]);

			index++;
		}


	}

	public Card DrawCard()
	{
		if (cards.Count > 0)
		{
			int randomDraw = Mathf.FloorToInt(Random.Range(0, cards.Count));
			Card card = cards[randomDraw];
			//cards.RemoveAt(randomDraw);
			return card;
		}
		else
			return null;
	}
}
