using UnityEngine;
using System.Collections;

public class Enemy_combat : MonoBehaviour {
	
	public GameObject target;
	public float timer, coolDown;
	// Use this for initialization
	void Start () {
		
		target = GameObject.FindGameObjectWithTag("Player");
		
		timer = 0;
		coolDown = 2.0f;
	}
	
	// Update is called once per frame
	void Update () {
		if (timer > 0){
			timer -= Time.deltaTime;	
		}
		if (timer < 0) timer = 0;
		
		if(timer == 0){
			Attack();
			timer = coolDown;
		}
	}
	
	private void Attack(){
		Vector3 dir = (target.transform.position - transform.position).normalized;
		
		float distance = Vector3.Distance(target.transform.position, transform.position);
		
		float direction = Vector3.Dot(dir, transform.forward);
		
		if (distance < 3){
			if (direction > 0){
				Ally_Health_Tracking eh = (Ally_Health_Tracking)target.GetComponent("Ally_Health_Tracking");
				
				eh.AddjustCurentHealth(-10);
			}
		}
	}
}
