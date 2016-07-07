using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ImageColorHelper
{

	public static void SetGraphicsToColor(Graphic[] graphics, Color color)
	{
		foreach (Graphic graphic in graphics)
			SetGraphicToColor (graphic, color);
	}

	public static void SetGraphicToColor(Graphic graphic, Color color)
	{
		graphic.color = color;
	}
}
