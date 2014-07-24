using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player_Combat : MonoBehaviour {
	public List<Transform> targets;
	public float timer, coolDown1 = 5, coolDown2 = 5, range1, range2, angle1, angle2, tempDamage = 15;
	public Vector3 dir;
	private float direction, curCool1, curCool2;

		void Start(){
			GameObject[] go = GameObject.FindGameObjectsWithTag("Enemy");
		
			foreach(GameObject enemy in go){
				targets.Add(enemy.transform);
			}
		}

		void Update(){
			SortByDistance ();
			
			if (Input.GetKeyDown (KeyCode.F) && curCool1 <= 0f) {
				attackP ();
			}

			if (Input.GetKeyDown (KeyCode.C) && curCool1 <= 0f) {
				attackS ();
			}
		
			if (curCool1 < 0f) {
				curCool1 = 0;
			}

			curCool1--;
			curCool2--;
		

		}

		public void SortByDistance(){
			targets.Sort(delegate (Transform t1, Transform t2) {
				return (Vector3.Distance(t1.position,transform.position).CompareTo(Vector3.Distance(t2.position,transform.position)));	
			});
		}

		public void attackP(){
				curCool1 = coolDown1;

				foreach (Transform trans in targets) {
					enemyHealthTracking eh = (enemyHealthTracking)trans.GetComponent ("enemyHealthTracking");
					float dist = Vector3.Distance(transform.position, trans.transform.position);
		
					dir = (targets[0].transform.position - transform.position).normalized;
					direction = Vector3.Dot(dir, transform.forward);

					if(direction >= angle1){
						if (dist <= range1) {
								eh.curHealth -= (int) tempDamage;
						} else if (tempDamage - (dist - range1) <= 0) {
								eh.curHealth -= (int) (tempDamage - (dist - range1)/2);
						}
					}
				}
		}
		public void attackS(){
			curCool2 = coolDown2;
			
			foreach (Transform trans in targets) {
				enemyHealthTracking eh = (enemyHealthTracking)trans.GetComponent ("enemyHealthTracking");
				float dist = Vector3.Distance(transform.position, trans.transform.position);
				
				dir = (targets[0].transform.position - transform.position).normalized;
				direction = Vector3.Dot(dir, transform.forward);
				
				if(direction >= angle2){
					if (dist <= range2) {
						eh.curHealth -= (int) tempDamage;
					} else if (tempDamage - (dist - range2) <= 0) {
						eh.curHealth -= (int) (tempDamage - (dist - range2)/2);
					}
				}
			}
		}
	
	/*old combat
	 * 
	public GameObject target;
	public float timer, coolDown, range = 3;
	// Use this for initialization
	void Start () {
		timer = 0;
		coolDown = 2.0f;
	}
	
	// Update is called once per frame
	void Update () {
		if (timer > 0){
			timer -= Time.deltaTime;	
		}
		if (timer < 0) timer = 0;
		
		if(Input.GetKeyDown(KeyCode.F)){
			if(timer == 0){
				Attack();
				timer = coolDown;
			}
		}
		if(target == null){
			ally_targeting al = (ally_targeting) GetComponent ("ally_targeting");
			
			al.targets  = new System.Collections.Generic.List<UnityEngine.Transform>();
			
			al.AddAllEnemy();
		}
	}
	
	private void Attack(){
		Vector3 dir = (target.transform.position - transform.position).normalized;
		
		float distance = Vector3.Distance(target.transform.position, transform.position);
		
		float direction = Vector3.Dot(dir, transform.forward);
		
		if (distance < range){
			if (direction > 0){
				
				Enemy_AI ea = (Enemy_AI) target.GetComponent("Enemy_AI");
				enemyHealthTracking eh = (enemyHealthTracking)target.GetComponent("enemyHealthTracking");
				
				if(ea.isFollowing == false){
					eh.curHealth = 0;
				}else{
					eh.AddjustCurentHealth(-10);
				}
				
			}
		}
	}
	*/
}