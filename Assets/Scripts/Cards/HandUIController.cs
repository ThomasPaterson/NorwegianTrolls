using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;
using System;
using UnityEngine.UI;

public class HandUIController : MonoBehaviour 
{
	public Action<CardButton> usedCard;
	public InputCommand[] inputs;
	public CardButton currentSelection;
	public Image manaDisplay;
	public string cancelInput;
	public string fireInput;
	public bool charging = false;

	public PlayerCharacter owner;
	private bool activated = false;
	private bool cancelling = false;

	void Update()
	{
		if (owner == null && activated)
			Destroy (gameObject);
		else if (owner != null)
		{

			if (!UsingCard())
				CheckPressed();

			CheckCancelled();

			DisplayMana();
		}
	}

	public void SetCharacter(PlayerCharacter playerCharacter)
	{
		owner = playerCharacter;
		activated = true;
		bool selectedFirst = false;

		foreach (InputCommand command in inputs)
		{
			if (command.cardButton != null)
			{

				if (!selectedFirst)
				{
					SelectCardButton(command.cardButton);
					selectedFirst = true;
				}

				command.cardButton.SetColor(PlayerConfig.instance.factionColor[playerCharacter.faction]);
				command.cardButton.AddCard(owner.DrawCard());
			}
				
		}

		manaDisplay.color = PlayerConfig.instance.factionColor[playerCharacter.faction];
	}

	public bool UsingCard()
	{
		return charging;
	}

	void CheckPressed()
	{
		if (!cancelling && !charging && CanAfford(currentSelection) && (Mathf.Abs(CrossPlatformInputManager.GetAxis(fireInput)) > 0.3f || Input.GetMouseButton(0)))
		{
			StartCoroutine(Charge(fireInput));
			return;
		}
			
		foreach (InputCommand command in inputs)
		{
			if (command.cardButton != null)
				if (CrossPlatformInputManager.GetButtonDown(command.inputName))
					SelectCardButton(command.cardButton);
		}
			
    }

	void CheckCancelled()
	{
		if (CrossPlatformInputManager.GetButtonDown(cancelInput))
		{
			currentSelection.Cancel(!charging);
			charging = false;
			StartCoroutine(WatchForCancel());
		}
	}

	IEnumerator WatchForCancel()
	{
		cancelling = true;

		while (Mathf.Abs(CrossPlatformInputManager.GetAxis(fireInput)) > 0.3f)
			yield return null;

		cancelling = false;
	}

	public void SelectCardButton(CardButton newSelection)
	{
		if (currentSelection != null)
			currentSelection.Deselect();

		currentSelection = newSelection;

		if (currentSelection != null)
			currentSelection.Select();
	}
	
	IEnumerator Charge(string inputName)
	{
		charging = true;
		float charged = 0f;
		GetComponent<AudioSource>().clip = AudioConfig.instance.chargeSound;
		GetComponent<AudioSource>().Play ();

		while ((Mathf.Abs(CrossPlatformInputManager.GetAxis(fireInput)) > 0.3f || Input.GetMouseButton(0)) && currentSelection != null && charging)
		{

			if (!GetComponent<AudioSource>().isPlaying)
			{
				GetComponent<AudioSource>().clip = AudioConfig.instance.chargeHoldSound;
				GetComponent<AudioSource>().Play ();
			}

			charged += Time.deltaTime;
			currentSelection.DisplayCharge(charged);
			yield return null;
		}

		if (currentSelection != null && charging)
			currentSelection.Release(charged);

		GetComponent<AudioSource>().Stop();

		charging = false;
	}

	void DisplayMana()
	{
		manaDisplay.GetComponent<RectTransform>().sizeDelta = new Vector2(owner.mana * 20f, 20f);
	}

	bool CanAfford(CardButton cardButton)
	{
		if (cardButton.currentCard == null)
			return false;

		return cardButton.currentCard.CanAfford(owner);
	}

	public Card DrawCard()
	{
		if (owner == null)
			return null;
		else
			return owner.DrawCard();
	}
}


[System.Serializable]
public class InputCommand
{
	public string inputName;
	public CardButton cardButton;


}
