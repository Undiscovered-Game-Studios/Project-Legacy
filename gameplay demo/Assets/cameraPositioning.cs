using UnityEngine;
using System.Collections;

public class cameraPositioning : MonoBehaviour {

	public float distance, height;

	// Use this for initialization
	void Start () {
		distance = Vector3.Distance (transform.position, transform.parent.position);
		height = transform.position.y - transform.parent.position.y;
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = transform.parent.position;
		transform.position += height * transform.up;
		transform.position -= distance * transform.forward;
	}
}
