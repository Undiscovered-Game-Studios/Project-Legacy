using UnityEngine;
using System.Collections;

public class TightRopeTrigger : MonoBehaviour {

	public bool isWalking;
	public int coolDown;
	public Rigid_contorler ri;
	public toggleTriggerScript Toggle1, Toggle2;
	public Vector3 vec1, vec2;

	void Start(){

	}

	void OnTriggerStay(Collider col){
		ri = col.gameObject.GetComponent<Rigid_contorler>();
	}

	void OnTriggerExit(Collider col){
		if(ri != null){
			if(col.gameObject.tag == "Player"){
				ri.isWalkingRope = false;
				ri.needToBeVertical = true;
			}
			ri = null;
			// = new Quaternion();
		}
	}

	public void Toggle(){
		if(Input.GetKeyDown(KeyCode.C)){
			if(ri != null){
				if(ri.isWalkingRope == false && coolDown <= 5){
					ri.isWalkingRope = true;
					ri.oldRot = ri.transform.rotation;
					coolDown = 15;
				}
				if(ri.isWalkingRope == true && coolDown <= 5){
					ri.needToBeVertical = true;
					ri.isWalkingRope = false;
					coolDown = 15;
				}
				if(Toggle1.isTriggered == true){
					ri.startingTightRopeVector = vec2;
					ri.endingTightRopeVector = vec1;
				}else{
					ri.startingTightRopeVector = vec1;
					ri.endingTightRopeVector = vec2;
				}

				/*if(ri.isWalkingRope == true){
					ri.isWalkingRope = false;
				}*/
			} 
		}
	}

	// Update is called once per frame
	void Update () {
		if(coolDown < 0) coolDown = 0;
		if(coolDown == 0){
			Toggle();
		}
		coolDown --;
		if(ri != null){
			if(Toggle2.partnerToggle == false && ri != null){
				ri.isWalkingRope = false;
				isWalking = false;
				ri.needToBeVertical = true;
			}
		}
	}
}
