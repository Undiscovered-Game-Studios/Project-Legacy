using UnityEngine;
using System.Collections;

public class itemUse : MonoBehaviour {
	
	public equipment eq;
	public int curWeaponPreset = 1, lastWeaponPreset = 0;
	public GameObject rightHandWeapon, leftHandWeapon;
	public Transform rightHand, leftHand;

	void Start(){
		eq = (equipment) gameObject.GetComponent ("equipment");
		WeaponReference();
		if (rightHandWeapon != null) {
			GameObject.Instantiate (rightHandWeapon);
			rightHandWeapon.transform.position = rightHand.position;
		}
		if (leftHandWeapon != null) {
			GameObject.Instantiate (leftHandWeapon);
			leftHandWeapon.transform.position = leftHand.position;
		}
	}

	public void WeaponReference(){
		if(curWeaponPreset == 1){
			rightHandWeapon = eq.weapons[0];
			leftHandWeapon = eq.weapons[1];
		}
		if(curWeaponPreset == 2){
			rightHandWeapon = eq.weapons[2];
			leftHandWeapon = eq.weapons[3];
		}
		if(curWeaponPreset == 3){
			rightHandWeapon = eq.weapons[4];
			leftHandWeapon = eq.weapons[5];
		}
		if(curWeaponPreset == 4){
			rightHandWeapon = eq.weapons[6];
			leftHandWeapon = eq.weapons[7];
		}
	}

	// Update is called once per frame
	void Update () {
		if(rightHandWeapon != null)	rightHandWeapon.transform.position = rightHand.position;
		if(leftHandWeapon != null)	leftHandWeapon.transform.position = leftHand.position;
		lastWeaponPreset = curWeaponPreset;
	}
}
