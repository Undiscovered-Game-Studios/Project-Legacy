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
	public bool isFollowing, isReturning;
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

	#region wander variables
		public Vector3 targetedVector, startingVector;
		public float wanderRange = 100;
	#endregion
	
	public void Start () {
		seeker = GetComponent<Seeker>();
		controller = GetComponent<CharacterController>();

		startingVector = transform.position;

		LoadTargets ();
		FindTarget ();

		targetedVector = new Vector3(
			Random.Range(-detectionRange,detectionRange)+transform.position.x,
			Random.Range(-detectionRange,detectionRange)+transform.position.y,
			transform.position.z
			);
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
			RefreshPath ();
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
			/* old path folling logic
			//Direction to the next waypoint
			Vector3 dir = (path.vectorPath[currentWaypoint]-transform.position).normalized;
			dir *= speed * Time.fixedDeltaTime;
			controller.SimpleMove (dir);
			//Check if we are close enough to the next waypoint
			//If we are, proceed to follow the next waypoint
			currentWaypoint++;
			return;
			*/
			
			if (Vector3.Distance (transform.position, path.vectorPath [currentWaypoint]) < 1) {
				currentWaypoint += 1;
			}
			transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation(path.vectorPath[currentWaypoint] - transform.position, Vector3.up), speed * Time.deltaTime);
			transform.position += transform.forward * speed * Time.deltaTime;
		}
		
		public void RefreshPath(){
			if (curDelay <= 0) {
				if (Vector3.Distance (transform.position, activeTarget.transform.position) < detectionRange) {
					//Start a new path to the targetPosition, return the result to the OnPathComplete function
					seeker.StartPath (transform.position, activeTarget.transform.position, OnPathComplete);
					isFollowing = true;
					isReturning = false;
				}else{
					isFollowing = false;
					Wander();
				}
				curDelay = maxDelay;
			} else {
				curDelay -= refreshFrequency * Time.deltaTime;
			}
		if (curDelay > maxDelay)
			curDelay = maxDelay;
		}
	#endregion

	public void Wander(){
		if(Vector3.Distance(transform.position, startingVector) > wanderRange){
			isReturning = true;
		}
		if(Vector3.Distance(transform.position, startingVector) < 5){
			isReturning = false;
		}
		
		if(isReturning == false){
			Vector3 temp;
			temp = new Vector3(
				Random.Range(-15f,15f),
				0,
				Random.Range(-15f,15f)
				);
			targetedVector += temp;
			targetedVector.y = transform.position.y +5;
			transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation(targetedVector - transform.position, Vector3.up), speed * Time.deltaTime);
			transform.position += transform.forward * speed * Time.deltaTime;
		}else{
			transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation(startingVector - transform.position, Vector3.up), speed * Time.deltaTime);
			transform.position += transform.forward * speed * Time.deltaTime;
			targetedVector = startingVector;
		}
	}
	
	public void FixedUpdate () {
		FollowPath();
		FindTarget();
		RefreshPath ();
	}
}