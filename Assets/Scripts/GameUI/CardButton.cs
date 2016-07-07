using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class CardButton : MonoBehaviour
{
	public Action<CardInstance, float> fireEvent = delegate{};


	public int cardNumber;
	public CardInstance currentCard;
	public Image cardImage;
	public GameObject selectedBackground;
	public bool pressed = false;
	public Image chargeDisplay;
	public Text costText;

	private HandUIController hand;

	void Start()
	{
		hand = GetComponentInParent<HandUIController>();
	}
	


	public void AddCard(Card card)
	{

		if (card == null)
		{
			currentCard = null;
			gameObject.SetActive(false);
		}
		else
		{
			StartCoroutine(DrawCard(card));

			currentCard = new CardInstance(card);
			cardImage.sprite = card.cardSprite;
			costText.text = card.manaCost.ToString();
		}
	}

	IEnumerator DrawCard(Card card)
	{
		cardImage.gameObject.SetActive(false);
		costText.gameObject.SetActive(false);

		yield return new WaitForSeconds(0.2f);

		cardImage.gameObject.SetActive(true);
		costText.gameObject.SetActive(true);
		currentCard = new CardInstance(card);
		cardImage.sprite = card.cardSprite;
		costText.text = card.manaCost.ToString();

		if (AudioConfig.instance.drawCard != null)
			AudioSource.PlayClipAtPoint(AudioConfig.instance.drawCard, Camera.main.transform.position);
	}

	public void SetColor(Color color)
	{

		Color tempColor = color;
		tempColor.a = 1f;
		selectedBackground.GetComponent<Graphic>().color = tempColor;
		//costText.color = tempColor;
	}

	public void Press()
	{
		ImageColorHelper.SetGraphicToColor(cardImage, Color.grey);
		pressed = true;
		chargeDisplay.gameObject.SetActive(true);
	}

	public void Select()
	{
		selectedBackground.SetActive(true);
	}

	public void Deselect()
	{
		selectedBackground.SetActive(false);
	}

	public void Release(float charge)
	{
		ImageColorHelper.SetGraphicToColor(cardImage, Color.white);
		pressed = false;
		chargeDisplay.gameObject.SetActive(false);
		float percent = Mathf.Min(1f, charge/currentCard.card.maxCharge);
		fireEvent(currentCard, percent);

		AddCard(hand.DrawCard());

	}

	public void Cancel(bool discard)
	{
		ImageColorHelper.SetGraphicToColor(cardImage, Color.white);

		if (discard)
		{
			AddCard(hand.DrawCard());
		}
			
		else
		{
			chargeDisplay.gameObject.SetActive(false);
			pressed = false;
		}
	}

	public void DisplayCharge(float chargeValue)
	{
		chargeDisplay.gameObject.SetActive(true);
		Vector2 sizeDelta = chargeDisplay.GetComponent<RectTransform>().sizeDelta;
		sizeDelta.y = CalculateChargeSize(chargeValue);
		chargeDisplay.GetComponent<RectTransform>().sizeDelta = sizeDelta;
    }

	float CalculateChargeSize(float chargeValue)
	{
		float percent = Mathf.Min(1f, chargeValue/currentCard.card.maxCharge);
		return percent * GetComponent<RectTransform>().sizeDelta.y;
	}
}
