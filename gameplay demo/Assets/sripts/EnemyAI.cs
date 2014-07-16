using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class EnemyAI : MonoBehaviour {

	public float moveSpeed;

	public List<GameObject> targets;
	public List<int> targetHate;
	public GameObject activeTarget, orbiter;

	public bool isFollowing, isBlocked;
	public Ray testRay;
	public float testDist;

	public Vector3 walkTo;

	private orbit orb;
	public Vector3 orbPlace;
	public bool lastFrame = new bool(), thisFrame = new bool();



// Use this for initialization
void Start () {
	FillTargets ();
	InitiateHateIndex ();
	orb = (orbit)orbiter.GetComponent ("orbit");
}

	public void FillTargets(){
		GameObject[] go = (GameObject.FindGameObjectsWithTag ("Player"));
		foreach (GameObject obj in go) {
			targets.Add(obj);
		}
	}

	public void InitiateHateIndex(){
		foreach (GameObject obj in targets) {
			targetHate.Add(1);
		}
	}

// Update is called once per frame
void FixedUpdate () {
	findHighestHate ();
	orbPlace = orb.gameObject.transform.position;
	Follow ();
}
	
	public void findHighestHate(){
		int max = targetHate.Max ();
		int index = targetHate.IndexOf (max);
		activeTarget = targets [index];
	}

	public void Follow(){
		checkBlockages ();
		if (isBlocked == false) {
			transform.LookAt (activeTarget.transform.position);
			transform.position += transform.forward * moveSpeed * Time.deltaTime;
		} else if (isBlocked == true) {
			FindPath();
		}
	}

	#region PathFinding
		public void FindPath(){
			CheckOrbiter ();
			transform.LookAt (walkTo);
			transform.position += transform.forward * moveSpeed * Time.deltaTime;
		}
		
			public void CheckOrbiter(){
				thisFrame = orb.isIntersecting;
				//false means way is clear
				if (thisFrame != lastFrame && thisFrame == false) {
					walkTo = orbPlace;
				}
				//Debug.Log ("lastFrame" + lastFrame.ToString ());
				//Debug.Log ("thisFrame" + thisFrame.ToString ());
				lastFrame = orb.isIntersecting;
			}
	#endregion

		public void checkBlockages(){
			Ray testRay = new Ray (transform.position + Vector3.up, activeTarget.transform.position - transform.position);
			RaycastHit hit;
			if (Physics.Raycast (testRay, out hit, testDist)) {
//			Debug.Log(hit.collider.gameObject.name);
				if(hit.collider.tag != "Player") isBlocked = true;
			} else {
				isBlocked = false;
			}
			Debug.DrawRay (testRay.origin, testRay.direction);
		}
}
