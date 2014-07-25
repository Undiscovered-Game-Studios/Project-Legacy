using UnityEngine;
using System.Collections;

public class enemyCombat : MonoBehaviour {

	public GameObject target;
	public float attack1Damage, attack1Range, attack1Angle, attack1CoolDown;
	private float curAttack1CoolDown;
	public float attack2Damage, attack2Range, attack2Angle, attack2CoolDown;
	private float curAttack2CoolDown;
	public bool attacking1, attacking2;

	// Use this for initialization
	void Start () {

		curAttack1CoolDown = attack1CoolDown;
	}
	
	// Update is called once per frame
	void Update () {
		EnemyAI ai = (EnemyAI)GetComponent ("EnemyAI");
		target = ai.activeTarget;
		AttackTimer ();
	}

	public void AttackTimer(){
		if (curAttack1CoolDown > 0) {
			curAttack1CoolDown -= 1;
			attacking1 = false;
		} else {
			Attack1();
			attacking1 = true;
			curAttack1CoolDown = attack1CoolDown;
		}

		if (curAttack2CoolDown > 0) {
			curAttack2CoolDown -= 1;
			attacking2 = false;
		} else {
			Attack2();
			attacking2 = true;
			curAttack2CoolDown = attack2CoolDown;
		}
	}

	private void Attack1(){
		Ally_Health_Tracking ht = (Ally_Health_Tracking) target.GetComponent ("Ally_Health_Tracking");

		Vector3 dir = (target.transform.position - transform.position).normalized;
		float direction = Vector3.Dot (dir, transform.forward);
		float dist = Vector3.Distance (transform.position, target.transform.position);

		if (direction >= attack1Angle && dist <= attack1Range) {
			ht.AddjustCurentHealth ((int)attack1Damage * -1);
		}
	}

	private void Attack2(){
		Ally_Health_Tracking ht = (Ally_Health_Tracking)target.GetComponent ("Ally_Health_Tracking");
		
		Vector3 dir = (target.transform.position - transform.position).normalized;
		float direction = Vector3.Dot (dir, transform.forward);
		float dist = Vector3.Distance (transform.position, target.transform.position);
		
		if (direction >= attack2Angle && dist <= attack2Range) {
			ht.AddjustCurentHealth ((int)attack2Damage * -1);
		}
	}
}
