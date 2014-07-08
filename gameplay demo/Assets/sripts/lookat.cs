using UnityEngine;
using System.Collections;

public class lookat : MonoBehaviour {
	public GameObject lookHere;

	// Update is called once per frame
	void Update () {
		transform.LookAt (lookHere.transform.position);
	}
}
