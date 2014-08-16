using UnityEngine;
using System.Collections;

public class cameraFollow : MonoBehaviour {

	public GameObject followedPlayer;
	public bool copyRotation;
	
	// Update is called once per frame
	void Update () {
		if (followedPlayer != null) {
			if (copyRotation == true) {
				transform.rotation = followedPlayer.transform.rotation;
			}

			transform.position = followedPlayer.transform.position;
		}
	}
}
