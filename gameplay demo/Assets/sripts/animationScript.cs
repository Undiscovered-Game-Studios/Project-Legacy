using UnityEngine;
using System.Collections;

public class animationScript : MonoBehaviour {
	
	public float dir, speed, jumpTime;
	
	public Animator anim;
	
	public Rigid_contorler ri;
	
	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();
		ri = (Rigid_contorler) GetComponent ("Rigid_contorler");
	}
	
	private void FindMovement(){
		speed = ri.curRunSpeed;
		dir = ri.curTurnSpeed;
	}
	
	private void Animate(){
		FindMovement();
		anim.SetFloat ("Dir",dir);
		anim.SetFloat ("Speed",speed);
		//if(ri.isJumping == true) anim.SetBool("Jumping", true);
		//if(ri.isJumping == false) anim.SetBool("Jumping", false);
	}
	
	// Update is called once per frame
	void Update () {
		Animate();
	}
}
