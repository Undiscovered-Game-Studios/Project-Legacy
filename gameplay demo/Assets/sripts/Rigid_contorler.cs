using UnityEngine;
using System.Collections;

public class Rigid_contorler : MonoBehaviour {
	
	public float maxSpeedForward = 20, maxSpeedBack = 15, jumpSpeed = 7, vertSwimSpeed = 5, 
				horSwimSpeed = 10, climbSpeed = 2;

	public float maxSprintMultiplier = 3, maxSprintTime = 5, 
				maxJumpDuration = 5;

	public float curRunSpeed, curTurnSpeed, curJumpDuration, curSprintTime,	curSprintMultiplier;

	public bool canMove, isSprinting, isAirBorn, isJumping, isSwimming, isSliding, isClimbing,
				isWalkingRope, needToBeVertical;

	public Quaternion oldRot;

	public Vector3 startingTightRopeVector, endingTightRopeVector;

	private Transform myTrans;
	public Quaternion toRot;
	

	
	public Vector3 startingSpot;
	
	//no clue how it works, but these are needed to rotate
	private Quaternion deltaRotation;
	public Vector3 eulerAngleVelocity = new Vector3 (0, 100, 0);

	public Vector3 slope;
	
	// Use this for initialization
	void Start () {
		myTrans = transform;
		
		LoadNewScene lo = (LoadNewScene) GetComponent ("LoadNewScene");
		
		myTrans.position = lo.startingArea;
	}
	
	private void Move(){

		eulerAngleVelocity = new Vector3 (0, 100 * Input.GetAxis("buttonTurn"), 0);

		if(isClimbing == false){
			curRunSpeed = curSprintMultiplier * maxSpeedForward * Input.GetAxis("buttonForward");
			myTrans.position += myTrans.forward * curRunSpeed * Time.deltaTime;


			curTurnSpeed = eulerAngleVelocity.y;
			deltaRotation = Quaternion.Euler(eulerAngleVelocity * Time.deltaTime);	

			if(isWalkingRope == false){	
				rigidbody.MoveRotation(rigidbody.rotation * deltaRotation);
			
			
				Jump();
				
				Swim();
				
				if(isSwimming == true){
					if(isJumping == true){
						myTrans.position += myTrans.up * vertSwimSpeed * curSprintMultiplier * Time.deltaTime;
					}else{
						myTrans.position -= myTrans.up * vertSwimSpeed * curSprintMultiplier * Time.deltaTime;
					}
				}else{
					myTrans.position += myTrans.up * curJumpDuration * jumpSpeed * Time.deltaTime;
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
		
		groundRay = new Ray (myTrans.position,Vector3.down);
		
		wallRay = new Ray (myTrans.position,myTrans.forward);
		
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
		Vector3 slideDir = (slidePlace - myTrans.position);

		Debug.Log(slidePlace.ToString());
		Debug.Log(slideDir.ToString());
	}

	public void Climb(){
		ladderTrigger la = (ladderTrigger) GetComponent ("ladderTrigger");
		myTrans.rotation = Quaternion.Slerp(myTrans.rotation, toRot, Time.deltaTime * 5);

		myTrans.position += myTrans.up * climbSpeed * curSprintMultiplier * Input.GetAxis("buttonForward") * Time.deltaTime;

		myTrans.position += myTrans.right * climbSpeed * curSprintMultiplier * Input.GetAxis("buttonTurn") * Time.deltaTime;

		/*if(isTurningLeft == true){
			myTrans.position -= myTrans.right * climbSpeed * curSprintMultiplier * Time.deltaTime;
		}
		if(isTurningRight == true){
			myTrans.position += myTrans.right * climbSpeed * curSprintMultiplier * Time.deltaTime;
		}*/
	}

	public void WalkRope(){
		rigidbody.useGravity = false;
		myTrans.rotation = Quaternion.Slerp(myTrans.rotation,Quaternion.LookRotation(endingTightRopeVector - myTrans.position),Time.deltaTime);
		//myTrans.position = Vector3.Lerp(myTrans.position, startingTightRopeVector, Time.deltaTime);
		//myTrans.position = startingTightRopeVector;
	}
	
	// Update is called once per frame
	void FixedUpdate () {		
		CheckButtons();
		
		FindMovement();
		
		if(canMove == true){
			Move();
		}
		
		FindObstacles();
		
		SlopeCalc();
		
		if(isClimbing == false)if(isAirBorn == true || isJumping == true) isSprinting = false;
		
		rigidbody.isKinematic = false;	

		if(isSwimming == false && isClimbing == false && isWalkingRope == false)rigidbody.useGravity = true;

		if(myTrans.rotation.x == 0 && myTrans.rotation.z == 0) needToBeVertical = false;
		if(needToBeVertical == true){
			Quaternion temp = new Quaternion();
			temp = myTrans.rotation;

			if(temp.x - Time.deltaTime/2 > 0){
				temp.x -= Time.deltaTime/2;
			}else if(temp.x > 0 && temp.x - Time.deltaTime/2 < 0){
				temp.x = 0;
			}

			if(temp.x + Time.deltaTime/2 < 0){
				temp.x += Time.deltaTime/2;
			}else if (temp.x < 0 && temp.x + Time.deltaTime/2 > 0){
				temp.x = 0;
			}

			if(temp.z - Time.deltaTime/2 > 0){
				temp.z -= Time.deltaTime/2;
			}else if(temp.z > 0 && temp.z - Time.deltaTime/2 < 0){
				temp.z = 0;
			}
			
			if(temp.z + Time.deltaTime/2 < 0){
				temp.z += Time.deltaTime/2;
			}else if (temp.z < 0 && temp.z + Time.deltaTime/2 > 0){
				temp.z = 0;
			}

			myTrans.rotation = temp;
		}

	}
}
