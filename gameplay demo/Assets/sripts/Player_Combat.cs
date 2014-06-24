using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player_Combat : MonoBehaviour {
	public List<Transform> targets;
	public float timer, coolDown1 = 5, curCool1, coolDown2 = 5, curCool2, range1, tempDamage = 15;
	private float I;

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

				if (curCool1 < 0f) {
						curCool1 = 0;
				}
		curCool1--;
		}

		public void SortByDistance(){
			targets.Sort(delegate (Transform t1, Transform t2) {
				return (Vector3.Distance(t1.position,transform.position).CompareTo(Vector3.Distance(t2.position,transform.position)));	
			});
		}

		public void attackP(){
				curCool1 = coolDown1;

				foreach (Transform trans in targets) {
						Enemy_health_tracking eh = (Enemy_health_tracking)trans.GetComponent ("Enemy_health_tracking");
						float dist = Vector3.Distance(transform.position, trans.transform.position);

			Debug.Log(dist);

						if (dist <= range1) {
								eh.curHealth -= (int) tempDamage;
						} else if (tempDamage - (dist - range1) <= 0) {
								eh.curHealth -= (int) (tempDamage - (dist - range1));
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
				Enemy_health_tracking eh = (Enemy_health_tracking)target.GetComponent("Enemy_health_tracking");
				
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