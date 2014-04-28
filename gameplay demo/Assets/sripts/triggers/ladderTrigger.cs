using UnityEngine;
using System.Collections;

public class ladderTrigger : MonoBehaviour {

	public bool canClimb;
	public int coolDown;
	public Rigid_contorler ri;

	void OnTriggerStay(Collider col){
		canClimb = true;
		ri = col.gameObject.GetComponent<Rigid_contorler>();
		if(ri != null){
			ri.toRot = transform.rotation;
		}
	}
	void OnTriggerExit(Collider col){
		canClimb = false;
		if(ri != null){
			ri.isClimbing = false;
			ri = null;
			ri.toRot = new Quaternion();
		}
	}

	public void Toggle(){
		if(Input.GetKeyDown(KeyCode.C)){
			if(ri != null){
				if(ri.isClimbing == false && coolDown <= 5){
					ri.isClimbing = true;
					coolDown = 15;
				}
				if(ri.isClimbing == true && coolDown <= 5){
					ri.isClimbing = false;
					coolDown = 15;
				}
			}
		}
	}

	// Update is called once per frame
	void Update() {
		if(coolDown < 0) coolDown = 0;
		if(coolDown == 0){
			Toggle();
		}
		coolDown --;
		if(canClimb == false && ri != null) ri.isClimbing = false;
	}
}
