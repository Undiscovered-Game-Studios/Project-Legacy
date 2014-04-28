using UnityEngine;
using System.Collections;
using System;

public class Enemy_AI : MonoBehaviour {
	
	public Transform target;
	public int moveSpeed = 10, rotSpeed = 30, maxDistance = 100, minDistance = 2;
	public float direction;
	
	private Transform myTrans;
	private Vector3 initialTrans, newLookAtSpot;
	public bool isFollowing;
	
	/*idle behavior variables*/
	public string idleBehavior = "BoundWander", traversionStyle = "walk";
	private float curDisRand, curRotRand, rotRandUpdate, updateRand, curFloatRand;
	
	/*Pathfinding variables*/
	public bool needFindPath;
	public float testDistance = 1000, testAngle;
	public Vector3 testVector, testDirection;
	public Ray testRay, targetRay;
	public RaycastHit testHit, targetHit;
	
	
	// Use this for initialization
	void Start () {
		
		myTrans = transform;
		initialTrans = myTrans.position;
		
		GameObject go = GameObject.FindGameObjectWithTag("Player");
		target = go.transform;
		
		curDisRand = UnityEngine.Random.Range(0,moveSpeed);
		rotRandUpdate = UnityEngine.Random.Range(-360, 360);
		
		newLookAtSpot = new Vector3(UnityEngine.Random.Range(myTrans.position.x - 5,myTrans.position.x + 5),UnityEngine.Random.Range(myTrans.position.y - 5,myTrans.position.y + 5),UnityEngine.Random.Range(myTrans.position.z - 5,myTrans.position.z + 5));
	}
	
	public void FindPath(){
				
		testVector = new Vector3(((float)Math.Cos(testAngle) * testDistance) + myTrans.position.x,
								myTrans.position.y,
								(float)(Math.Sin(testAngle) * testDistance + myTrans.position.z));
		
		testVector.Normalize();
		
		testDirection = (myTrans.position - testVector).normalized;
		
		testDirection.Normalize();
		
		
		
		targetRay = new Ray(myTrans.position,testDirection);
		
		if(collider.Raycast(testRay,out testHit, testDistance)){
			if(targetHit.collider.gameObject.tag != "Player"){
				needFindPath = true;
				Debug.Log("ray and tag work");
			}else{
				Debug.Log("Ray works");	
			}
		}else{
			Debug.Log("Ray doesn't work");	
		}
		
		
		
		Debug.DrawRay(myTrans.position,testDirection, Color.blue);
	}
	
	private void FreeWander(){
		
		updateRand = UnityEngine.Random.Range(0,5);
		
		if(traversionStyle == "walk"){
			if(updateRand == 0){
				curDisRand = UnityEngine.Random.Range(0,moveSpeed);
				rotRandUpdate = UnityEngine.Random.Range(-360, 360);
			}
			
			if(curDisRand < moveSpeed/5) curDisRand = 0;
			
			if(rotRandUpdate > 200 || rotRandUpdate < -200){
				curRotRand = rotRandUpdate;	
			}	
			
			myTrans.position += myTrans.forward * curDisRand* Time.deltaTime;
			
			myTrans.rotation = Quaternion.Slerp(myTrans.rotation, Quaternion.Euler(myTrans.up * curRotRand), Time.deltaTime);
		}
		if(traversionStyle == "float"){
			
			if(updateRand == 0){
				curDisRand = UnityEngine.Random.Range(0,moveSpeed);
				newLookAtSpot = new Vector3(
					(float)UnityEngine.Random.Range(-UnityEngine.Random.Range(myTrans.position.x - initialTrans.x, myTrans.position.x),UnityEngine.Random.Range(myTrans.position.x - initialTrans.x,myTrans.position.x)),
					(float)UnityEngine.Random.Range(-UnityEngine.Random.Range(myTrans.position.y - initialTrans.y, myTrans.position.y),UnityEngine.Random.Range(myTrans.position.y - initialTrans.y,myTrans.position.y)),
					(float)UnityEngine.Random.Range(-UnityEngine.Random.Range(myTrans.position.z - initialTrans.z, myTrans.position.z),UnityEngine.Random.Range(myTrans.position.z - initialTrans.z,myTrans.position.z))
					);
			}			
			myTrans.rotation = Quaternion.Slerp(myTrans.rotation,Quaternion.LookRotation(newLookAtSpot),Time.deltaTime);
			
			myTrans.position += myTrans.forward * curDisRand* Time.deltaTime;
		}
	}
	
	private void BoundWander(){
		
		bool isOutOfBounds = false;
		
		updateRand = UnityEngine.Random.Range(0,5);
		
		if(updateRand == 0){
			curDisRand = UnityEngine.Random.Range(0,moveSpeed);
			rotRandUpdate = UnityEngine.Random.Range(-360, 360);
		}
		
		if(curDisRand < moveSpeed/5) curDisRand = 0;
		
		if(rotRandUpdate > 200 || rotRandUpdate < -200){
			curRotRand = rotRandUpdate;	
		}
		if(Vector3.Distance(myTrans.position, initialTrans) > maxDistance){
			isOutOfBounds = true;
		}
		
		if(Vector3.Distance(myTrans.position, initialTrans) < maxDistance/2){
			isOutOfBounds = false;	
		}
		
		if(isOutOfBounds == true){
			
			myTrans.position += myTrans.forward * curDisRand* Time.deltaTime;
			
			myTrans.rotation = Quaternion.Slerp(myTrans.rotation, Quaternion.LookRotation(initialTrans - myTrans.position + new Vector3 (UnityEngine.Random.Range(-Vector3.Distance(myTrans.position,initialTrans),Vector3.Distance(myTrans.position,initialTrans))*2,0,UnityEngine.Random.Range(-Vector3.Distance(myTrans.position,initialTrans),Vector3.Distance(myTrans.position,initialTrans))*2)),Time.deltaTime);
			
		}else{	
			
			myTrans.position += myTrans.forward * curDisRand* Time.deltaTime;
		
			myTrans.rotation = Quaternion.Slerp(myTrans.rotation, Quaternion.Euler(myTrans.up * curRotRand), Time.deltaTime);
		}
		
	}
	
	// Update is called once per frame
	void Update () {
		
		//FindPath();
		
		if (isFollowing == true){
			if (Vector3.Distance(target.position, myTrans.position) > minDistance){
				//Look at target
				myTrans.rotation = Quaternion.Slerp (myTrans.rotation, Quaternion.LookRotation(target.position - myTrans.position), rotSpeed * Time.deltaTime);
				
				//move towards target
				myTrans.position += myTrans.forward * moveSpeed * Time.deltaTime;
			}	
		}else{
			if(idleBehavior == "BoundWander"){
				BoundWander();
			}
			if(idleBehavior == "FreeWander"){
				FreeWander();
			}
		}
	}
}
