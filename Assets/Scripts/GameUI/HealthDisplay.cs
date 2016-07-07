using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour 
{
	public Image topHealth;
	public Image bottomHealth;

	private HandUIController controller;
	private bool activated = false;
	private int baseHealth;
	private float baseLength;

	void Awake()
	{
		controller = GetComponentInParent<HandUIController>();
		baseLength = topHealth.GetComponent<RectTransform>().sizeDelta.x;
		topHealth.gameObject.SetActive(false);
		bottomHealth.gameObject.SetActive(false);
	}

	IEnumerator Start()
	{
		while (controller.owner == null)
			yield return null;

		Debug.Log("setting up health tracking");

		baseHealth = controller.owner.health;
		topHealth.gameObject.SetActive(true);
		bottomHealth.gameObject.SetActive(true);
		activated = true;


	}

	
	// Update is called once per frame
	void Update () 
	{
		if (activated && controller.owner != null)
		{
			Vector2 sizeDelta = topHealth.GetComponent<RectTransform>().sizeDelta;
			sizeDelta.x = baseLength * controller.owner.health/baseHealth;
			topHealth.GetComponent<RectTransform>().sizeDelta = sizeDelta;
		}
	}
}
