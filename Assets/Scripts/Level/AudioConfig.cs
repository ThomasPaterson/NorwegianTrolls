using UnityEngine;
using System.Collections;

public class AudioConfig : MonoBehaviour
{

	public static AudioConfig instance;

	public AudioClip drawCard;
	public AudioClip chargeSound;
	public AudioClip chargeHoldSound;

	void Awake()
	{
		instance = this;
	}


}
