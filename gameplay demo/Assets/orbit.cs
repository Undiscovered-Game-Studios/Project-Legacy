using UnityEngine;
using System.Collections;

public class orbit : MonoBehaviour {

	public GameObject lookHere;
	public float orbitSpeed = 100, orbitDist = 5, orbitHeight = 5;
	public bool isIntersecting;

	// Update is called once per frame
	void FixedUpdate () {
		transform.LookAt (lookHere.transform.position);
		transform.position += transform.right * orbitSpeed * Time.deltaTime;
		transform.position += transform.forward * (Vector3.Distance (transform.position, lookHere.transform.position) - orbitDist);
		if (transform.position.y + lookHere.transform.position.y < orbitHeight)
						transform.position += Vector3.up;
		if (transform.position.y + lookHere.transform.position.y > orbitHeight)
						transform.position -= Vector3.up;
	}
	void OnTriggerEnter(){
		isIntersecting = true;
	}
	void OnTriggerExit(){
		isIntersecting = false;
	}
}