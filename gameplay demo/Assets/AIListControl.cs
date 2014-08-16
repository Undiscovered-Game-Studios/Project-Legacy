using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AIListControl : MonoBehaviour {

	public List<GameObject> players, empties;
	public List<GameObject> cameras;
	public int activePlayer, index;

	// Use this for initialization
	void Start () {
		GameObject[] gos = GameObject.FindGameObjectsWithTag ("Player");

		foreach (GameObject go in gos) {
			players.Add(go);
		}

		GameObject[] ems = GameObject.FindGameObjectsWithTag ("PlayerToPoint");

		foreach (GameObject em in ems) {
			empties.Add(em);
		}
		activePlayer = 0;

		foreach (GameObject go in players) {
			allyAI ai = (allyAI)go.GetComponent ("allyAI");
			//ai.toPositionMarker = empties [players.IndexOf (go)];
			//ai.toPosition = empties[players.IndexOf (go)].transform.position;
		}
	}
	
	// Update is called once per frame
	void Update () {
		SelectActivePlayer ();
		SetActivePlayer ();
	}

	public void SetActivePlayer(){
		foreach (GameObject go in players) {
			Rigid_contorler ri = (Rigid_contorler) go.GetComponent ("Rigid_contorler");
			allyAI ai = (allyAI) go.GetComponent ("allyAI");
			if(go == players[activePlayer]){
				ri.isAIControlled = false;
				ai.isAIControlled = false;
			}else{
				ri.isAIControlled = true;
				ai.isAIControlled = true;
			}
		}
		foreach (GameObject cam in cameras) {
			cameraFollow cf = (cameraFollow)cam.GetComponent ("cameraFollow");
			cf.followedPlayer = players [activePlayer];
		}
	}

	public void SelectActivePlayer(){
		if(Input.GetKeyDown(KeyCode.Alpha1)){
			activePlayer = 0;
		}
		if(Input.GetKeyDown(KeyCode.Alpha2)){
			activePlayer = 1;
		}
		if(Input.GetKeyDown(KeyCode.Alpha3)){
			activePlayer = 2;
		}
		if(Input.GetKeyDown(KeyCode.Alpha4)){
			activePlayer = 3;
		}
	}
}