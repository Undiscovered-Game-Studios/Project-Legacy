using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyAI : MonoBehaviour {

	public List<GameObject> targets;
	public GameObject targ;
	// Use this for initialization
	void Start () {
		FillTargets ();
	}

	public void FillTargets(){
		GameObject[] go = (GameObject.FindGameObjectsWithTag ("Player"));
		foreach (GameObject obj in go) {
			targets.Add(obj);
		}
	}

	// Update is called once per frame
	void Update () {
	
	}
}
