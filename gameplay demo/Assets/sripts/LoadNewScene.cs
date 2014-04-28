using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LoadNewScene : MonoBehaviour {
	
	public string Level;
	public Vector3 startingArea;
	public List<GameObject> players;
	public int numberOfPlayers;
	public bool isLocked = false;
	public GameObject key;
	public GameObject mainPlayer;
	public float delay = 10;
	public bool needToChangeScene = false;
	
	void Start(){
		AddAllPlayer();	
		
		numberOfPlayers = players.Count;
		
		if(numberOfPlayers > 1){
			EliminateExtraPlayers();	
		}
	}

	void OnTriggerStay (Collider col) {
		if(col.gameObject.tag == "Player"){
			mainPlayer = col.gameObject;
			Rigid_contorler ri = (Rigid_contorler) mainPlayer.GetComponent ("Rigid_contorler");
			ri.canMove = false;
			needToChangeScene = true;
		}
	}
	
	void OnCollisionStay (Collision col) {
		if(col.gameObject.tag == "Player" && Input.GetAxis("attack/open doors") > 0){
			mainPlayer = col.gameObject;
			Rigid_contorler ri = (Rigid_contorler) mainPlayer.GetComponent ("Rigid_contorler");
			ri.canMove = false;
			needToChangeScene = true;
		}
	}
	
	public void AddAllPlayer(){
		GameObject[] go = GameObject.FindGameObjectsWithTag("Player");
		
		foreach(GameObject player in go){
			AddPlayer(player);
		}
	}
	
	public void AddPlayer(GameObject player){
		players.Add(player);
	}
	
	public void EliminateExtraPlayers(){
		
		Destroy(players[1]);
			
	}

	public void Load(GameObject colplayer){
		if(isLocked == false){
			DontDestroyOnLoad(colplayer);
			
			Application.LoadLevel(Level);
			
			Rigid_contorler ri = (Rigid_contorler) colplayer.GetComponent ("Rigid_contorler");
			
			ri.transform.position = startingArea;
		}else if(isLocked == true){
			
			DontDestroyOnLoad(colplayer);
			
			Application.LoadLevel(Level);
			
			Rigid_contorler ri = (Rigid_contorler) colplayer.GetComponent ("Rigid_contorler");
			
			ri.transform.position = startingArea;
		}
	}

	public void DelayTimer(){
		if(needToChangeScene == true && delay > 0){
			delay--;
		}else if(needToChangeScene == true && delay <= 0){
			Rigid_contorler ri = (Rigid_contorler) mainPlayer.GetComponent ("Rigid_contorler");
			//ri.canMove = true;
			Load(mainPlayer);
		}
	}
	
	void Update(){
		DelayTimer();
		
	}
}