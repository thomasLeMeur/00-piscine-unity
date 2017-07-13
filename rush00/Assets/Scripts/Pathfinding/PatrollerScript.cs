using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrollerScript : MonoBehaviour {

	public List<Transform> 	waypoints = new List<Transform>();
	public bool 			invertedWay = false;

	bool 			backWay = false;
	int 			nextStep = 0;
	EnemyController controller;

	void Start()
	{
		controller = gameObject.GetComponent<EnemyController> ();
		if (waypoints.Count > 0) {
			controller.currentStatus = EnemyController.Status.patrol;
			transform.position = waypoints [0].position;
			nextStep = 1;
		}
	}

	void Update() {

		if (controller.currentStatus == EnemyController.Status.patrol) {

			controller.targetPosition = waypoints [nextStep].position;
			if (transform.position == waypoints [nextStep].position) {
				GetNextStep();
				controller.Lookat((Vector2)transform.position - (Vector2)waypoints [nextStep].position);
			}
		}
	}

	void GetNextStep()
	{
		if (!invertedWay)
			nextStep = (nextStep == waypoints.Count - 1) ? 0 : nextStep + 1;
		else if (nextStep > 0 && (nextStep == waypoints.Count - 1 || backWay)) {
			nextStep = nextStep - 1;
			backWay = true;

		} else {
			nextStep = nextStep + 1;
			backWay = false;
		}

		if (nextStep < 0)
			nextStep = 0;
		else if (nextStep > waypoints.Count - 1)
			nextStep = waypoints.Count - 1;
	}

	public bool IsOnPatrolWay()
	{
		foreach (Transform wayp in waypoints) {
			if (wayp.position == controller.transform.position)
				return true;
		}
		return false;
	}

	public void BackToPatrol()
	{
		controller.currentStatus = EnemyController.Status.patrol;

		int i = 0;
		foreach (Transform wayp in waypoints) {
			if (wayp.position == controller.transform.position) {
				nextStep = i;
				GetNextStep ();
				controller.Lookat((Vector2)transform.position - (Vector2)waypoints [nextStep].position);
				break;
			}
			i++;
		}
	}
}
