using UnityEngine;
using System.Collections;

public class LOSManager : MonoBehaviour
{

	public LayerMask losBlockingTerrain;
	public LayerMask characterLayer;

	public static LOSManager instance;

	void Awake ()
	{
		instance = this;
	}

	public static bool CanSee (Vector3 start, Vector3 end)
	{
		return !Physics.Linecast (start, end, instance.losBlockingTerrain);
	}
}

