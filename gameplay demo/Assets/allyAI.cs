using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pathfinding;

public class allyAI : MonoBehaviour {

	public GameObject toPositionMarker;
	public bool isAIControlled, isInCombat, isFollowing ;

	#region targeting variables
	public List<Transform> targets;
	public List<float> targetHate;
	public Transform activeTarget;
	public float detectionRange;
	#endregion

	#region A* Variables
	//The point to move to
	private Seeker seeker;
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

	#region Combat Variables
	public Player_Combat pc;
	public float AIattack1Delay = 6, AIattack2Delay = 13, dodgeFrequency;
	private float curAIattack1Delay, curAIattack2Delay;
	#endregion

	// Use this for initialization
	void Start () {
		pc = (Player_Combat)GetComponent ("Player_Combat");
		targets = pc.targets;
		foreach (Transform obj in targets) {
			targetHate.Add(detectionRange - Vector3.Distance(obj.transform.position,transform.position));
		}
		seeker = (Seeker) gameObject.GetComponent ("Seeker");
	}

	#region Path Find/Follow
	public void OnPathComplete (Path p) {
		if (!p.error) {
			path = p;
			//Reset the waypoint counter
			currentWaypoint = 0;
		}
	}
	
	public void FollowPath(){
		FindTowardPosition ();
		if (path == null) {
			//We have no path to move after yet
			return;
		}
		
		if (currentWaypoint >= path.vectorPath.Count) {
			return;
		}
		
		if (Vector3.Distance (transform.position, path.vectorPath [currentWaypoint]) < 1) {
			currentWaypoint += 1;
		}

	}
	
	public void RefreshPath(Vector3 pathToPosition){
		if (curDelay <= 0) {
			//Start a new path to the targetPosition, return the result to the OnPathComplete function
			seeker.StartPath (transform.position, pathToPosition, OnPathComplete);
			curDelay = maxDelay;
		} else {
			curDelay -= refreshFrequency * Time.deltaTime;
		}
		if (curDelay > maxDelay)
			curDelay = maxDelay;
	}

	public void MoveTowardPoint(Vector3 towardPosition){
		Quaternion tmprot, torot;
		transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation(towardPosition - transform.position, Vector3.up), speed * Time.deltaTime);
		tmprot = transform.rotation;
		transform.rotation = Quaternion.identity;
		torot = Quaternion.identity;
		torot.y = tmprot.y;
		torot.w = tmprot.w;
		transform.rotation = torot;
		transform.position += transform.forward * speed * Time.deltaTime;
	}

	public void FindTowardPosition(){
		Ray r = new Ray (transform.position, transform.forward);
		RaycastHit h = new RaycastHit ();
		float dist = 1;
		if (isInCombat != true) {
			if (Physics.Raycast (r, out h, .5f)) {
				RefreshPath (toPositionMarker.transform.position);
				if (Vector3.Distance (transform.position, path.vectorPath [currentWaypoint]) > dist) {
					MoveTowardPoint (path.vectorPath [currentWaypoint]);
				}
			} else {
				if (Vector3.Distance (transform.position, toPositionMarker.transform.position) > dist) {
					MoveTowardPoint (toPositionMarker.transform.position);
				}
			}
		} else { /*in combat else*/
			if (Physics.Raycast (r, out h, .5f)) {
				RefreshPath (activeTarget.transform.position);
				if(path.vectorPath[currentWaypoint] != null){
					if (Vector3.Distance (transform.position, path.vectorPath [currentWaypoint]) > dist) {
						MoveTowardPoint (path.vectorPath [currentWaypoint]);
					}
				}
			} else {
				if (Vector3.Distance (transform.position, activeTarget.transform.position) > dist) {
					MoveTowardPoint (activeTarget.transform.position);
				}
			}
		}
	}
	#endregion

	// Update is called once per frame
	void FixedUpdate () {
		if (isAIControlled) {
			FollowPath();
			FindTarget();
			if(isInCombat == true){
				CombatLoop();
			}
		}
	}

	#region targeting
	public void FindTarget(){
		List<Transform> tempTarg;
		List<float> tempHate;
		tempHate = new List<float> ();
		tempTarg = new List<Transform> ();
		foreach (Transform targ in targets) {
			if(Vector3.Distance(targ.position, transform.position) <= detectionRange){
				tempTarg.Add(targ);
				tempHate.Add(targetHate[targets.IndexOf(targ)]);
			}
		}
		if(tempHate.Count > 0 && tempTarg.Count > 0){
		float highestHate;
		int index;
		highestHate = tempHate.Max ();
		index = tempHate.IndexOf (highestHate);
		activeTarget = tempTarg [index];
		}

		tempHate = new List<float> ();
		tempTarg = new List<Transform> ();

		DetectCombat ();

		//remove old
		foreach (Transform temp in targets) {
			if(temp == null){
				int ind = targets.IndexOf(temp);
				float tmp = targetHate[ind];
				targetHate.Remove(tmp);
				targets.Remove(temp);
			}
		}
	}

	public void DetectCombat(){
		if(activeTarget != null)
		if (Vector3.Distance (transform.position, activeTarget.transform.position) <= detectionRange) {
			isInCombat = true;
		} else {
			isInCombat = false;
		}
	}
	#endregion

	#region AI combat

	public void CombatLoop(){
		if (curAIattack1Delay <= 0) {
			pc.attackP();
			curAIattack1Delay = AIattack1Delay;
		} else {
			curAIattack1Delay -= Time.deltaTime;
		}

		if (curAIattack2Delay <= 0) {
			pc.attackS();
			curAIattack2Delay = AIattack2Delay;
		}else{
			curAIattack2Delay -= Time.deltaTime;
		}

		enemyCombat ec = (enemyCombat)activeTarget.GetComponent ("enemyCombat");
		if (ec.attacking1 == true || ec.attacking2 == true) {
			//add random dodge and damage negation here
		}
	}

	#endregion
}











