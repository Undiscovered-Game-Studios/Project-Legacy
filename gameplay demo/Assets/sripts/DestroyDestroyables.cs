using UnityEngine;
using System.Collections;

public class DestroyDestroyables : MonoBehaviour {
	void OnCollisionEnter(Collision col){
		if(col.gameObject.tag == "Destroy-able"){
			Destroy(col.gameObject);	
		}
	}
}
