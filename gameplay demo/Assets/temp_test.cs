using UnityEngine;
using System.Collections;

public class temp_test : MonoBehaviour {

	public GameObject probe;
	public EnemyAI ai;

	// Update is called once per frame
	void Update () {
		ai = (EnemyAI)probe.GetComponent ("EnemyAI");
		transform.position = ai.walkTo;
	}
}
