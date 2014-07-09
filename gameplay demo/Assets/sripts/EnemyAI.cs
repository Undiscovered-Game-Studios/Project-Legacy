using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class EnemyAI : MonoBehaviour {

	public float moveSpeed;

	public List<GameObject> targets;
	public List<int> targetHate;
	public GameObject activeTarget;

	public bool isFollowing, isBlocked;
	public Ray testRay;
	public float testDist;

	public GameObject testTarget;



// Use this for initialization
void Start () {
	FillTargets ();
	InitiateHateIndex ();
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

		public void FindPath(){
			
		}
		
			public void TargetOrbit(){
				
			}

		public void checkBlockages(){
			Ray testRay = new Ray (transform.position + Vector3.up, activeTarget.transform.position - transform.position);
			RaycastHit hit;
			if (Physics.Raycast (testRay, out hit, testDist)) {
			Debug.Log(hit.collider.gameObject.name);
				if(hit.collider.tag != "Player") isBlocked = true;
			} else {
				isBlocked = false;
			}
			Debug.DrawRay (testRay.origin, testRay.direction);
		}
}
