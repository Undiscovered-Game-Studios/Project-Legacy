using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LoadNewScene : MonoBehaviour {
	
	public string Level;
	public Vector3 startingArea;
	public bool isLocked = false;
	public GameObject key;
	
	public GameObject playerHub;
	public int numberOfHubs;
	public List<GameObject> hubs;

	public void AddAllPlayersInScene(){
		GameObject[] go = GameObject.FindGameObjectsWithTag("playerHub");
		
		foreach(GameObject hub in go){
			hubs.Add(hub);
		}

		playerHub = GameObject.FindGameObjectWithTag ("playerHub");
	}

	void Start(){
		AddAllPlayersInScene ();
	}

	void OnTriggerStay(Collider col){
		if (col.gameObject.tag == "Player") {
			LoadScene ();
		}
	}

	void OnCollisionStay(Collision col){
		if (col.gameObject.tag == "Player") {
			LoadScene ();
		}
	}

	public void LoadScene(){
		LocateAllPlayers ();
		DontDestroyOnLoad (playerHub);
		Application.LoadLevel(Level);
	}

	public void LocateAllPlayers(){
		Transform[] allChildren = playerHub.transform.GetComponentsInChildren<Transform>();
		foreach (Transform child in allChildren) {
			child.transform.position = startingArea;
		}
	}

	void FixedUpdate(){
		if (hubs.Count > 1) {
			GameObject gos = hubs [1];
			hubs.Remove (gos);
			GameObject.Destroy(gos);
		}
	}
}