using UnityEngine;
using System.Collections;

public class SwimTrigger : MonoBehaviour {
	void OnTriggerEnter(Collider col){
			Rigid_contorler ri = (Rigid_contorler)col.GetComponent("Rigid_contorler");
			
			ri.isSwimming = true;
	}
	void OnTriggerExit(Collider col){
			Rigid_contorler ri = (Rigid_contorler)col.GetComponent("Rigid_contorler");
			
			ri.isSwimming = false;
	}
}
