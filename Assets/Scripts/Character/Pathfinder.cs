using UnityEngine;
using System.Collections;
using Pathfinding;

[RequireComponent(typeof(Seeker))]
public class Pathfinder : MonoBehaviour 
{
	private const float UPDATE_FREQUENCY = 1f;

	private Seeker seeker;

	//The point to move to
	public Vector3 targetPosition;

	public Path path;

	//The max distance from the AI to a waypoint for it to continue to the next waypoint
	public float nextWaypointDistance = 3;
	//The waypoint we are currently moving towards
	private int currentWaypoint = 0;

	private float timeUntilNextUpdate = 0f;

	private GameObject target;

	void Awake()
	{
		seeker = GetComponent<Seeker>();
	}
	
	public void MoveTowardsTarget(GameObject newTarget)
	{
		if (target != newTarget)
		{
			target = newTarget;

			if (target != null)
				seeker.StartPath (transform.position, target.transform.position, OnPathComplete);
		
		}
		else if (timeUntilNextUpdate <= 0f && target != null)
		{
			timeUntilNextUpdate = UPDATE_FREQUENCY;
			seeker.StartPath (transform.position, target.transform.position, OnPathComplete);
		}

		timeUntilNextUpdate -= Time.deltaTime;


		if (path == null || (currentWaypoint >= path.vectorPath.Count) )
			return;


		//Direction to the next waypoint
		Vector3 dir = (path.vectorPath[currentWaypoint]-transform.position).normalized;
		dir.y = 0;
		GetComponent<ThirdPersonCharacter>().Move(dir, 1f + GetComponent<Character>().GetMoveModifiers());

		if (Vector3.Distance (transform.position,path.vectorPath[currentWaypoint]) < nextWaypointDistance) 
		{
			currentWaypoint++;
			return;
		}

	}



	public void OnPathComplete (Path p) 
	{
		currentWaypoint = 0;
		path = p;
	}

}