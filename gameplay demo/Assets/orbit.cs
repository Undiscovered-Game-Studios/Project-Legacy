using UnityEngine;
using System.Collections;

public class orbit : MonoBehaviour {

	public GameObject lookHere;
	public float orbitSpeed = 5;

	// Update is called once per frame
	void Update () {
		transform.LookAt (lookHere.transform.position);
		transform.position += transform.right * orbitSpeed * Time.deltaTime;
	}
}
