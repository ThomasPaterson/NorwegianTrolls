﻿using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour 
{

	public static MusicManager instance;

	void Awake()
	{
		if (instance == null)
		{
			instance = this;
			DontDestroyOnLoad(gameObject);

		}
		else
		{
			Destroy (gameObject);
		}
	}
}
