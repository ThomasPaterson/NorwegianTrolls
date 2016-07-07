using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;



[RequireComponent(typeof (ThirdPersonCharacter))]
public class ThirdPersonUserControl : MonoBehaviour
{
	public int playerNum;
	public float cardPower;


    private ThirdPersonCharacter m_Character; // A reference to the ThirdPersonCharacter on the object
	private HandUIController handUI;
	private Character character;

    
    private void Start()
    {
		Camera.main.GetComponent<SmoothFollow>().AddTarget(this.gameObject);
        m_Character = GetComponent<ThirdPersonCharacter>();  
		character = GetComponent<Character>();
    }

    // Fixed update is called in sync with physics
    private void FixedUpdate()
    {
		float modifier = 1f;

		if (handUI != null && handUI.UsingCard())
		{
			modifier = 0.5f;
			GetComponent<Character>().FlipDirection(GetFireVector());
			GetComponent<Character>().ShowDirectionArrow(GetFireVector());
		}
		else
		{
			GetComponent<Character>().HideDirectionArrow();
			GetComponent<Character>().FlipDirection(GetMoveVector());
		}

		modifier += character.GetMoveModifiers();
		
		// pass all parameters to the character control script
		m_Character.Move(GetMoveVector(), modifier);

       
    }

	public void AssignHandController(HandUIController controller)
	{
		handUI = controller;
		handUI.SetCharacter(GetComponent<PlayerCharacter>());

		foreach (InputCommand command in handUI.inputs)
		{
			if (command.cardButton != null)
				command.cardButton.fireEvent += FireCard;
		}
	}

	void FireCard(CardInstance cardInstance, float chargePercent)
	{
		GameObject cardProj = (GameObject)Instantiate(CardConfig.instance.cardPrefab, GetPositionOfShot(), CardConfig.instance.cardPrefab.transform.rotation);
		cardProj.GetComponent<Projectile>().faction = GetComponent<Character>().faction;
		cardProj.GetComponent<Projectile>().cardInstance = cardInstance;
		cardProj.GetComponent<Rigidbody>().AddForce(GetForceOfShot() * cardInstance.CardForceModifier(chargePercent));
		GetComponent<PlayerCharacter>().SpendMana(cardInstance.card.manaCost);

	}

	Vector3 GetForceOfShot()
	{
		return GetFireVector().normalized * cardPower;
	}

	Vector3 GetPositionOfShot()
	{
		return GetFireVector().normalized * 0.5f + transform.position + Vector3.up;
	}


	Vector3 GetFireVector()
	{
		float x = CrossPlatformInputManager.GetAxis("FireHorizontal" + playerNum.ToString());
		float y = CrossPlatformInputManager.GetAxis("FireVertical" + playerNum.ToString()) * -1f;

        if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer)
        {
            Vector3 direction = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
            x = direction.x;
            y = direction.y;
        }
		
		return new Vector3(x, 0f, y);
	}

	Vector3 GetMoveVector()
	{
		float x = CrossPlatformInputManager.GetAxis("Horizontal" + playerNum.ToString());
		float y = CrossPlatformInputManager.GetAxis("Vertical" + playerNum.ToString()) * -1f;

		return new Vector3(x, 0f, y);
	}

	void OnDisable()
	{

		if (handUI != null)
		{

			Debug.Log ("Destroying hand UI");
			foreach (InputCommand command in handUI.inputs)
			{
				if (command.cardButton != null)
					command.cardButton.fireEvent -= FireCard;
			}

			Destroy (handUI.gameObject);
		}
	}




}

