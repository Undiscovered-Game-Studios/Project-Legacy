using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnItems : MonoBehaviour {
	
	public List<GameObject> itemsPossibleToDrop;
	
	private GameObject me;
	
	void Start(){
		me = gameObject;	
	}
	
	// Update is called once per frame
	void Update () {
		Enemy_health_tracking eh = (Enemy_health_tracking) me.GetComponent ("Enemy_health_tracking");
		
		if(eh.curHealth == 0){
			Instantiate (itemsPossibleToDrop[Random.Range(0,itemsPossibleToDrop.Count)],me.transform.position, Quaternion.identity);	
		}
	}
}
