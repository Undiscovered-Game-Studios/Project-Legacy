using UnityEngine;
using System.Collections;

public class toggleTriggerScript : MonoBehaviour {

	public bool isTriggered, partnerToggle;
	public toggleTriggerScript partner;

	void OnTriggerEnter(Collider col){
		if(col.gameObject.tag == "Player"){
			isTriggered = true;
			if(partnerToggle == true){
				partnerToggle = false;
				partner.partnerToggle = false;
			}else{
				partnerToggle = true;
				partner.partnerToggle = true;
			}
		}
	}
	void OnTriggerExit(Collider col){
		if(col.gameObject.tag == "Player"){
			isTriggered = false;
		}
	}
}
