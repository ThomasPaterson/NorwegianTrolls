using UnityEngine;
using System.Collections;

public class MainCameraController : MonoBehaviour 
{
	public static MainCameraController instance;

	public float speed;
	public float smoothRate;
	
	private Vector3 target;

	void Awake()
	{
		instance = this;
	}

	void LateUpdate()
	{
		transform.position = Vector3.Lerp(transform.position, target, smoothRate * Time.deltaTime);
	}

	public void Move(Vector2 move)
	{
		target += new Vector3(move.x * speed, 0f, move.y * speed);
	}


}
