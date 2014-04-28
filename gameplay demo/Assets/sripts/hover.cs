using UnityEngine;
using System.Collections;


public class hover : MonoBehaviour {
	
	public float force = 10f, tourqueX, tourqueY;
	
	Random varX, varZ;
	
	int localX, localZ;
	
	void OnTriggerStay (Collider other){
		
		other.rigidbody.AddForce(Vector3.up * force, ForceMode.Acceleration);
		
		other.rigidbody.AddTorque(Vector3.up * tourqueY,ForceMode.Acceleration);
			
		other.rigidbody.AddTorque(Vector3.right * tourqueX,ForceMode.Acceleration);
	}
	
}
