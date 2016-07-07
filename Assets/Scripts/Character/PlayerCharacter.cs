using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PlayerCharacter : Character
{

	public int mana;
	public float manaRechargeRate;
	public int maxMana;
	public Deck deck;
	public PlayerInstance playerInstance;

	protected override void Start()
	{
		base.Start ();
		StartCoroutine(RechargeMana());

	}

	public void Init(PlayerInstance playerInstance)
	{
		this.playerInstance = playerInstance;
		deck = new Deck(CardConfig.instance.decks[playerInstance.chosenDeck].cards);
		faction = playerInstance.faction;
	}


	public void PickupMana(int value)
	{
		mana += value;

		if (mana > maxMana)
		{
			mana = maxMana;
		}
	}

	public void SpendMana(int value)
	{
		mana -= value;
	}

	IEnumerator RechargeMana()
	{

		while (true)
		{
			PickupMana(1);
			yield return new WaitForSeconds(manaRechargeRate);
		}
	}

	public Card DrawCard()
	{
		return deck.DrawCard();
	}




}
