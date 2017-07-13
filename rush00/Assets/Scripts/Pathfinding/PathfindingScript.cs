using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingScript : MonoBehaviour {

	List<Vector2> 		waypoints = new List<Vector2>();
	Vector2 			target = new Vector2(-1000f, 0f);

	int 				nextStep = -1;
	EnemyController 	controller;

	bool 				onTheBackWay = false;
	bool 				running = true;

	void Awake()
	{
		controller = gameObject.GetComponent<EnemyController> ();
	}

	void Update() {
//
		if (nextStep != -1 && controller.currentStatus == EnemyController.Status.backToStart) {
			if (waypoints != null && waypoints.Count > 0 && nextStep >= 0 && nextStep < waypoints.Count)
			{
				if ((Vector2)transform.position != waypoints [nextStep])
					controller.targetPosition = waypoints [nextStep];
				else
				{
					if (controller.startStatus == EnemyController.Status.patrol && controller.GetComponent<PatrollerScript> ().IsOnPatrolWay ()) {
						clearPath ();
						controller.GetComponent<PatrollerScript> ().BackToPatrol ();
					}
					else {
						nextStep++;
						if (nextStep > -1 && nextStep < waypoints.Count - 1)
							controller.Lookat ((Vector2)transform.position - (Vector2)waypoints [nextStep]);
					}
				}
			}
			else
			{
				if (controller.currentStatus == EnemyController.Status.backToStart) {
					controller.transform.rotation = controller.startRotation;
					controller.currentStatus = controller.startStatus;
				}
			}
		}
		else
			clearPath ();

	}

	void clearPath()
	{
		target.x = -1000.0f;
		nextStep = -1;
		clearWay ();
	}

	void clearWay()
	{
		waypoints.Clear ();
	}

	GameObject GetCloseWaypoint(Vector2 position)
	{
		GameObject[] waypoints = GameObject.FindGameObjectsWithTag("Waypoint");
		GameObject closestObject = null;
		foreach(GameObject obj in waypoints)
		{
			if(!closestObject)
				closestObject = obj;
			else if(Vector2.Distance(position, obj.transform.position) <= Vector2.Distance(position, closestObject.transform.position))
				closestObject = obj;
		}
		return closestObject;
		return null;
	}

	public void GoToTarget(Vector2 target)
	{
		if (this.target == target)
			return;
		clearPath ();
		this.target = target;
		controller.Lookat((Vector2)transform.position - target);
		WaypointScript closeWayp = GetCloseWaypoint (transform.position).GetComponent<WaypointScript>();

		List<WaypointScript> previous = new List<WaypointScript>();
		List<Vector2> path = closeWayp.getPath (GetCloseWaypoint(target).GetComponent<WaypointScript>(), previous);
		if (path != null) {
			path.Reverse ();
			foreach (Vector2 way in path)
				waypoints.Add (way);
			nextStep = 0;
		}
	}

}
