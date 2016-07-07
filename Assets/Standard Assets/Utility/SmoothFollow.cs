using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class SmoothFollow : MonoBehaviour
{

	public Vector3 offset;
	public AnimationCurve cameraZoomValue;
	public float averageDistance; //at this distance, uses 
	
	public List<GameObject> targets = null;

	// The distance in the x-z plane to the target
	[SerializeField]
	private float distance = 10.0f;
	// the height we want the camera to be above the target
	[SerializeField]
	private float height = 5.0f;

	[SerializeField]
	private float rotationDamping;
	[SerializeField]
	private float heightDamping;

	public void AddTarget(GameObject newTarget)
	{
		if (targets == null)
			targets = new List<GameObject>();

		Debug.Log (newTarget.name);
		targets.Add(newTarget);
	}

	public void RemoveTarget(GameObject toRemove)
	{
		targets.Remove(toRemove);
	}



	// Update is called once per frame
	void LateUpdate()
	{
		// Early out if we don't have a target
		if (targets == null || targets.Count == 0)
			return;

		Vector3 averageLocation = GetAverageTarget();

		transform.position = averageLocation + CalculateOffset();

		transform.LookAt(averageLocation);
	}

	Vector3 GetAverageTarget()
	{
		Vector3 average = new Vector3();

		foreach (GameObject target in targets)
			average += target.transform.position;

		average /= targets.Count;

		return average;
	}

	Vector3 CalculateOffset()
	{


		if (targets.Count == 0)
			return new Vector3();

		Bounds bounds = new Bounds((targets[0].transform.position), Vector3.zero);
		
		foreach (GameObject target in targets)
				bounds.Encapsulate(target.transform.position);
			
		float maxDistance = Vector3.Distance(bounds.max, bounds.min);
		float percentZoom = cameraZoomValue.Evaluate(maxDistance/averageDistance);

		return offset * percentZoom;


	}
}
