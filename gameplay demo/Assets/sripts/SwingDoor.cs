using UnityEngine;
using System.Collections;

public class SwingDoor : MonoBehaviour {
	public float Toggle = 1;
	private float ExcelSpeed, SpeedLimit;
	public float MovePower = 200;
	public bool canOpen = false;
	void Update() {
		/*if(Toggle < 1) {
			rigidbody.isKinematic = false;
			rigidbody.WakeUp();
			MovePower = MovePower + ExcelSpeed;
			rigidbody.AddForce(-Vector3.forward * MovePower);
			if(MovePower > SpeedLimit){
				MovePower = MovePower - ExcelSpeed;
				rigidbody.AddForce(-Vector3.forward * MovePower);
			}
		}
		if(Input.GetAxis("attack/open doors") > 0 && Toggle >= 1) {
			Toggle--;
		}*/
		if(Input.GetAxis("attack/open doors") > 0 && Toggle > 1 && canOpen == true) {
			rigidbody.isKinematic = false;
			rigidbody.WakeUp();
			rigidbody.AddForce(-Vector3.forward * MovePower);
			Toggle--;
		}
		if(Input.GetAxis("attack/open doors") <= 0) {
			Toggle += 1;
			if(Toggle > 5) Toggle = 5;
		}
	}
	void OnCollisionStay (Collision col) {
		if(col.gameObject.tag == "Player"){
			canOpen = true;
		}
	}
	void OnCollisionExit (Collision col) {
		if(col.gameObject.tag == "Player"){
			canOpen = false;
		}
	}
}