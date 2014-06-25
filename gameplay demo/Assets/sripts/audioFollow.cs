using UnityEngine;
using System.Collections;

public class audioFollow : MonoBehaviour {

	public GameObject player, listener;
	public Vector3 startingSpot;
	public float range = 500, distance;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("MainCamera");
		startingSpot = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		transform.LookAt (player.transform.position);
		if (Vector3.Distance (startingSpot, transform.position) < range) {
			distance = Vector3.Distance (transform.position, player.transform.position);
			transform.position += transform.forward * distance;
		} else {
			transform.position += transform.forward;
		}
		if (Vector3.Distance (startingSpot, transform.position) >= range) {
			transform.LookAt (startingSpot);
			distance = (Vector3.Distance (startingSpot, transform.position) - range);
			transform.position += transform.forward * distance;
		}
	}
}
