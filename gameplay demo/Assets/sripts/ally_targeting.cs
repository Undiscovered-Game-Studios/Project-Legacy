using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ally_targeting : MonoBehaviour {
	
	public List<Transform> targets;
	public Transform activeTarget;
	private Transform myTransform;
	// Use this for initialization
	void Start () {
		targets = new List<Transform>();
		activeTarget = null;
		myTransform = transform;
		AddAllEnemy();
	}
	
	public void AddAllEnemy(){
		GameObject[] go = GameObject.FindGameObjectsWithTag("Enemy");
		
		foreach(GameObject enemy in go){
			AddTarget(enemy.transform);
		}
	}
	
	public void AddTarget(Transform enemy){
		targets.Add(enemy);
	}
		
	private void SortByDistance(){
		targets.Sort(delegate (Transform t1, Transform t2) {
			return (Vector3.Distance(t1.position,myTransform.position).CompareTo(Vector3.Distance(t2.position,myTransform.position)));	
		});
	}
	
	private void FindActive(){
		SortByDistance();	

		activeTarget = targets[0];
		
		Player_Combat pc = (Player_Combat) GetComponent ("Player_Combat");
		
		pc.target = activeTarget.gameObject;
	}
	
	// Update is called once per frame
	void Update () {		
		FindActive();
	}
}
