using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointScript : MonoBehaviour {

	public List<Transform> nearWayp = new List<Transform>();
	float overlapRadius;	

	void Awake()
	{
		overlapRadius = GetComponent<CircleCollider2D> ().radius;
		Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, overlapRadius);
		int i = 0;
		while (i < hits.Length) {
			if (hits[i].gameObject.tag == "Waypoint" && hits[i].transform.position != transform.position)
				nearWayp.Add(hits[i].transform);
			i++;
		}	
	}

	public List<Vector2> getPath (WaypointScript target, List<WaypointScript> previous) {
		if (previous.Contains (this) || (previous.Count > 50)) {
			return null;
		}

		previous = new List<WaypointScript> (previous);
		previous.Add (this);
		List<Vector2> path = null;
		if (this == target) {
			path = new List<Vector2> ();
			path.Add (this.transform.position);
			return path;
		}

		List<Vector2>[] paths = new List<Vector2>[nearWayp.Count];
		int i = 0;
		foreach (Transform link in nearWayp) {
			paths[i] = link.gameObject.GetComponent<WaypointScript>().getPath (target, previous);
			i++;
		}
		path = null;
		int pathCount = int.MaxValue;
		foreach (List<Vector2> p in paths) {
			if (p != null && p.Count < pathCount) {
				path = p;
				pathCount = p.Count;
			}
		}
		if (path != null)
			path.Add (this.transform.position);

		return path;
	}

	void OnDrawGizmosSelected() {

		Gizmos.color = Color.yellow;
		foreach (Transform wayp in nearWayp)
			Gizmos.DrawLine(transform.position, wayp.position);
	}
}
