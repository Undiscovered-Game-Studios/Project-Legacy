using UnityEngine;
using System.Collections;

public class EnemySightTrigger : MonoBehaviour {
	
	public Transform myTrans, target;
	
	public Ray targetRay;
	public RaycastHit hit;
	public Vector3 targetDir;
	
	public bool canSee = false;
	
	void Awake(){
		myTrans = transform;
		GameObject go = GameObject.FindGameObjectWithTag("Player");
		target = go.transform;
	}
	
	public void CanSee(){
		
		Enemy_AI ea = (Enemy_AI) GetComponent ("Enemy_AI");
		
		targetDir = -myTrans.position + ea.target.transform.position;
		
		targetRay = new Ray(myTrans.position, targetDir);
		if(Physics.Raycast(targetRay, out hit)){
			if(hit.collider.tag == "Player"){
				canSee = true;
			}else{
				canSee = false;
			}
		}
			
		Debug.DrawRay(myTrans.position, targetDir, Color.red);
		
	}
	
	public void Trigger(){
		
		CanSee();
		
		Enemy_AI ea = (Enemy_AI) GetComponent ("Enemy_AI");
		
		Vector3 dir = (ea.target.transform.position - transform.position).normalized;
		
		if (Vector3.Distance(ea.target.transform.position, myTrans.position) < ea.maxDistance && Vector3.Dot(dir,transform.forward) > 0)
			if(myTrans.position.y < target.position.y + 5 && myTrans.position.y > target.position.y - 5 /*&& canSee == true*/){
				ea.isFollowing = true;
		}
		
		if (Vector3.Distance(ea.target.transform.position, myTrans.position) > ea.maxDistance){
			ea.isFollowing = false;
		}
	}
	
	void Update(){
		Trigger();	
	}
}
