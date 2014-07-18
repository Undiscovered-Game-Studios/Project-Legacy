using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pathfinding;

public class EnemyAI : MonoBehaviour {

	#region targeting varibales
	public List<GameObject> targets;
	public List<float> targetHate;
	public GameObject activeTarget;
	public float detectionRange = 200;
	private Vector3 targetedVector;
	#endregion

	#region A* Variables
	//The point to move to
	private Seeker seeker;
	private CharacterController controller;
	//The calculated path
	public Path path;
	//The AI's speed per second
	public float speed = 100;
	//The max distance from the AI to a waypoint for it to continue to the next waypoint
	public float nextWaypointDistance = 3;
	//The waypoint we are currently moving towards
	private int currentWaypoint = 0;
	//delays path update
	public float maxDelay = 5, refreshFrequency = 1, curDelay;
	#endregion
	
	public void Start () {
		seeker = GetComponent<Seeker>();
		controller = GetComponent<CharacterController>();



		LoadTargets ();
		FindTarget ();
	}

	#region targeting
		public void LoadTargets(){
			GameObject[] temp = GameObject.FindGameObjectsWithTag ("Player");
			foreach (GameObject obj in temp) {
				targets.Add(obj);
			}
			foreach (GameObject obj in targets) {
				targetHate.Add(100 - Vector3.Distance(obj.transform.position,transform.position));
			}
		}

		public void FindTarget(){
			float highestHate;
			int index;
			highestHate = targetHate.Max ();
			index = targetHate.IndexOf (highestHate);
			activeTarget = targets [index];
		}
	#endregion

	#region PathFinding
	
		public void OnPathComplete (Path p) {
			Debug.Log ("Yay, we got a path back. Did it have an error? "+p.error);
			if (!p.error) {
				path = p;
				//Reset the waypoint counter
				currentWaypoint = 0;
			}
		}

		public void FollowPath(){
			if (path == null) {
				//We have no path to move after yet
				return;
			}
			
			if (currentWaypoint >= path.vectorPath.Count) {
				Debug.Log ("End Of Path Reached");
				return;
			}
			
			//Direction to the next waypoint
			Vector3 dir = (path.vectorPath[currentWaypoint]-transform.position).normalized;
			dir *= speed * Time.fixedDeltaTime;
			controller.SimpleMove (dir);
			//Check if we are close enough to the next waypoint
			//If we are, proceed to follow the next waypoint
			currentWaypoint++;
			return;
		}
		
		public void RefreshPath(){
			DetermineTargetVector ();
			if (curDelay <= 0) {
				//Start a new path to the targetPosition, return the result to the OnPathComplete function
				seeker.StartPath (transform.position, targetedVector, OnPathComplete);
				curDelay = maxDelay;
			} else {
				curDelay -= refreshFrequency * Time.deltaTime;
			}
		if (curDelay > maxDelay)
			curDelay = maxDelay;
		}
		
		public void DetermineTargetVector(){
			if (Vector3.Distance (transform.position, activeTarget.transform.position) > detectionRange) {
				targetedVector = new Vector3(
					Random.Range(-detectionRange,detectionRange)+transform.position.x,
					Random.Range(-detectionRange,detectionRange)+transform.position.y,
					transform.position.z
				);
			} else {
				targetedVector = activeTarget.transform.position;
			}
		}
	#endregion
	
	public void FixedUpdate () {
		FollowPath();
		FindTarget();
		RefreshPath ();
	}
}