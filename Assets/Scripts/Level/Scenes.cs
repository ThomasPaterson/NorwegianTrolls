using UnityEngine;
using System.Collections;

public class Scenes  
{

	public const int MAIN_MENU = 0;
	public const int VERSUS = 1;
	public const int SINGLE_PLAYER = 2;
	public const int END_GAME_SCREEN = 3;


	public static void LoadLevel(int level)
	{
		Application.LoadLevel(level);
	}
}
