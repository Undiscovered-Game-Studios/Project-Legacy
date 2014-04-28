using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class inclusionObjectCulling : MonoBehaviour {

	public GameObject parentObject;
	public List<GameObject> cullingObjects;
	//public List<Collider> otherCullingTriggers;

	//public int cullOutDelay = 100;

	public int index;

	void Start(){
		Transform[] allChildren = parentObject.transform.GetComponentsInChildren<Transform>();
		foreach (Transform child in allChildren) {
			if(child.gameObject.renderer != null){
				cullingObjects.Add(child.gameObject);
			}
		}
	}

	void OnTriggerStay(Collider col){
		if(col.gameObject.tag == "Player"){
			foreach(GameObject obj in cullingObjects){
				obj.renderer.enabled = true;
			}
		}
	}
	void OnTriggerExit(Collider col){
		if(col.gameObject.tag == "Player"){
			foreach(GameObject obj in cullingObjects){
				obj.renderer.enabled = false;
			}
		}
	}
}
