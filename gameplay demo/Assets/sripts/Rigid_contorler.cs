﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Rigid_contorler : MonoBehaviour {

	#region player-controlled vaiables
		public float maxSpeedForward = 20, maxSpeedBack = 15, jumpSpeed = 7, vertSwimSpeed = 5, 
					horSwimSpeed = 10, climbSpeed = 2, strafeSpeed = 50;

		public float maxSprintMultiplier = 3, maxSprintTime = 5, 
			maxJumpDuration = 5, maxStrafeTimer = 1.5f;

		public float curRunSpeed, curTurnSpeed, curJumpDuration, curSprintTime, curSprintMultiplier, curStrafeTimer;

		public bool canMove, isSprinting, isAirBorn, isJumping, isSwimming, isSliding, isClimbing,
					isWalkingRope, isStrafingRight, isStrafingLeft, needToBeVertical;

		public Quaternion oldRot, toRot;

		public Vector3 startingTightRopeVector, endingTightRopeVector;
	#endregion

	#region AI-controller variables
		public bool isAIControlled;
	#endregion
	
	public Vector3 startingSpot;
	
	//no clue how it works, but these are needed to rotate
	private Quaternion deltaRotation;
	public Vector3 eulerAngleVelocity = new Vector3 (0, 100, 0);

	public Vector3 slope;
	
	// Use this for initialization
	void Start () {
		LoadNewScene lo = (LoadNewScene) GetComponent ("LoadNewScene");

		if (lo != null) {
			transform.position = lo.startingArea;
		}
	}

    public void Strafe()
    {
		if (isStrafingLeft == true || isStrafingRight == true) {
			curStrafeTimer--;
		} else {
			curStrafeTimer = maxStrafeTimer;
		}
		if (curStrafeTimer <= 0f) {
			isStrafingLeft = false;
			isStrafingRight = false;
			canMove = true;
		}
		if (Input.GetKeyDown(KeyCode.E))
		{
			isStrafingLeft = false;
			isStrafingRight = true;
			canMove = false;
		}
		if (Input.GetKeyDown(KeyCode.Q))
		{
			isStrafingRight = false;
			isStrafingLeft = true;
			canMove = false;
		}
		if (isStrafingLeft == true) {
			transform.position -= transform.right * strafeSpeed;
		}
		if (isStrafingRight == true) {
			transform.position += transform.right * strafeSpeed;
		}
    }
	
	private void Move(){

		eulerAngleVelocity = new Vector3 (0, 100 * Input.GetAxis("buttonTurn"), 0);

		if(isClimbing == false){
			curRunSpeed = curSprintMultiplier * maxSpeedForward * Input.GetAxis("buttonForward");
			transform.position += transform.forward * curRunSpeed * Time.deltaTime;


			curTurnSpeed = eulerAngleVelocity.y;
			deltaRotation = Quaternion.Euler(eulerAngleVelocity * Time.deltaTime);	

			if(isWalkingRope == false){	
				rigidbody.MoveRotation(rigidbody.rotation * deltaRotation);
			
			
				Jump();
				
				Swim();
				
				if(isSwimming == true){
					if(isJumping == true){
						transform.position += transform.up * vertSwimSpeed * curSprintMultiplier * Time.deltaTime;
					}else{
						transform.position -= transform.up * vertSwimSpeed * curSprintMultiplier * Time.deltaTime;
					}
				}else{
					transform.position += transform.up * curJumpDuration * jumpSpeed * Time.deltaTime;
				}
			}else{
				WalkRope();
			}
		}else{
			rigidbody.useGravity = false;
			Climb();
		}
	}
	
	private void FindMovement(){
		  
		if(isSliding == true){
			SlideCalc();
		}else{

			
			if(isSprinting == true){
				curSprintMultiplier += Time.deltaTime;
				
				if(curSprintMultiplier > maxSprintMultiplier) curSprintMultiplier = maxSprintMultiplier;
				
				curSprintTime += Time.deltaTime;
				
				if(curSprintTime > maxSprintTime) isSprinting = false;
			}else{
				curSprintMultiplier -= Time.deltaTime;
				
				if(curSprintMultiplier < 1) curSprintMultiplier = 1;
				
				curSprintTime -= Time.deltaTime;
				
				if(curSprintTime < 0)curSprintTime = 0;
			}
			
			if(isJumping == true){
				curJumpDuration += Time.deltaTime;
				if(curJumpDuration > maxJumpDuration) curJumpDuration = maxJumpDuration;
			}else{
				curJumpDuration -= Time.deltaTime;	
				if(curJumpDuration < 0) curJumpDuration = 0;
			}
		}
	}
	
	private void CheckButtons(){

		
		if(Input.GetKeyDown(KeyCode.LeftShift) && curSprintTime == 0){
			isSprinting = true;	
		}
		if(Input.GetKeyUp(KeyCode.LeftShift)){
			isSprinting = false;	
		}
	}
	
	private void Jump(){
		if(Input.GetAxis("Jump") > 0){
			if(isAirBorn == false || isSwimming == true){
				isJumping = true;
				curJumpDuration = 0;
			}
			if(curJumpDuration < maxJumpDuration && isJumping == true){
				curJumpDuration ++;
			}
		}else{
			isJumping = false;
		}
	}
	
	private void Swim(){
		if(isSwimming == true){
			rigidbody.useGravity = false;
		}
	}
	
	public void FindObstacles(){
		
		RaycastHit groundOut, wallOut;
		
		Ray groundRay, wallRay;
		
		groundRay = new Ray (transform.position,Vector3.down);
		
		wallRay = new Ray (transform.position,transform.forward);
		
		if(Physics.Raycast(groundRay,out groundOut,2)){
			isAirBorn = false;
			slope = groundOut.normal;
		}else{
			isAirBorn = true;
		}
		
		if(Physics.Raycast(wallRay,out groundOut,2)){
		}
	}
	
	public void SlopeCalc(){	
		
		if(slope.x > .5 || slope.x < -.5){
			if(isSprinting == true){
				isSprinting = false	;
			}else if (curSprintMultiplier == 1){
				canMove = false;
				isSliding = true;
			}
		}

		if(slope.x > .6 || slope.x < -.6){
			isSprinting = false	;
			canMove = false;
		}
		
		if(slope.x < .3 || slope.x > -.3){
			isSliding = false;
		}
		
		
	}

	public void SlideCalc(){
		Vector3 slidePlace = transform.position;
		Vector3 slideDir = (slidePlace - transform.position);

		Debug.Log(slidePlace.ToString());
		Debug.Log(slideDir.ToString());
	}

	public void Climb(){
		ladderTrigger la = (ladderTrigger) GetComponent ("ladderTrigger");
		transform.rotation = Quaternion.Slerp(transform.rotation, toRot, Time.deltaTime * 5);

		transform.position += transform.up * climbSpeed * curSprintMultiplier * Input.GetAxis("buttonForward") * Time.deltaTime;

		transform.position += transform.right * climbSpeed * curSprintMultiplier * Input.GetAxis("buttonTurn") * Time.deltaTime;

		/*if(isTurningLeft == true){
			transform.position -= transform.right * climbSpeed * curSprintMultiplier * Time.deltaTime;
		}
		if(isTurningRight == true){
			transform.position += transform.right * climbSpeed * curSprintMultiplier * Time.deltaTime;
		}*/
	}

	public void WalkRope(){
		rigidbody.useGravity = false;
		transform.rotation = Quaternion.Slerp(transform.rotation,Quaternion.LookRotation(endingTightRopeVector - transform.position),Time.deltaTime);
		//transform.position = Vector3.Lerp(transform.position, startingTightRopeVector, Time.deltaTime);
		//transform.position = startingTightRopeVector;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (isAIControlled == false) {

			CheckButtons ();
		
			FindMovement ();

			Strafe ();
		
			if (canMove == true) {
				Move ();
			}
		
			FindObstacles ();
		
			SlopeCalc ();
		
			if (isClimbing == false)
			if (isAirBorn == true || isJumping == true)
				isSprinting = false;
		
			rigidbody.isKinematic = false;	

			if (isSwimming == false && isClimbing == false && isWalkingRope == false)
				rigidbody.useGravity = true;

			if (transform.rotation.x == 0 && transform.rotation.z == 0)
				needToBeVertical = false;

			if (needToBeVertical == true) {
				Quaternion temp = new Quaternion ();
				temp = transform.rotation;

				if (temp.x - Time.deltaTime / 2 > 0) {
					temp.x -= Time.deltaTime / 2;
				} else if (temp.x > 0 && temp.x - Time.deltaTime / 2 < 0) {
					temp.x = 0;
				}

				if (temp.x + Time.deltaTime / 2 < 0) {
					temp.x += Time.deltaTime / 2;
				} else if (temp.x < 0 && temp.x + Time.deltaTime / 2 > 0) {
					temp.x = 0;
				}

				if (temp.z - Time.deltaTime / 2 > 0) {
					temp.z -= Time.deltaTime / 2;
				} else if (temp.z > 0 && temp.z - Time.deltaTime / 2 < 0) {
					temp.z = 0;
				}
				
				if (temp.z + Time.deltaTime / 2 < 0) {
					temp.z += Time.deltaTime / 2;
				} else if (temp.z < 0 && temp.z + Time.deltaTime / 2 > 0) {
					temp.z = 0;
				}

				transform.rotation = temp;
			}
		}
	}
}
