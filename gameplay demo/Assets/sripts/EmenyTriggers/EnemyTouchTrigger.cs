using UnityEngine;
using System.Collections;

public class EnemyTouchTrigger : MonoBehaviour {
	
	public bool isTouched = false;
	
	void OnCollisionEnter(Collision col){		
		if(col.gameObject.tag == "Player"){
			isTouched = true;	
		}
	}
	
	void Update(){
		Enemy_AI ea = (Enemy_AI) GetComponent ("Enemy_AI");
		
		if(isTouched == true){
			ea.isFollowing = true;
		}
	}
}
