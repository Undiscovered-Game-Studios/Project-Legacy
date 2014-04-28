using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnEnemy : MonoBehaviour {
	
	public int maxNumberOfEnemys, spawnDelay;
	
	public bool needToAddEnemy = false;
	
	public float timeSinceLastSpawn;
	
	public Transform enemyToSpawn;
	
	public Vector3 spawnPlace;
	
	public List<Transform> curEnemys;
	
	void Start(){
		AddAllEnemy();	
		
		spawnPlace = transform.position;
	}
	
	void Update () {
		
		GameObject go = GameObject.FindGameObjectWithTag("Player");
		
		ally_targeting at = (ally_targeting) go.GetComponent ("ally_targeting");
		
		Enemy_health_tracking eh = (Enemy_health_tracking) GetComponent ("Enemy_health_tracking");
		
		if (timeSinceLastSpawn == 0) //New time
    	{
       		timeSinceLastSpawn = Time.time; //Set the current time
   		}else{
		
			if(curEnemys.Count < maxNumberOfEnemys && (int)(Time.time - timeSinceLastSpawn) >= spawnDelay){
				Instantiate (enemyToSpawn, spawnPlace, Quaternion.identity);
				at.targets = new List<Transform>();
				curEnemys = new List<Transform>();
				AddAllEnemy();
				at.AddAllEnemy();
				timeSinceLastSpawn = 0;
			}	
			
		}
		timeSinceLastSpawn --;
		
		if(eh.curHealth == 0){
			curEnemys = new List<Transform>();
			AddAllEnemy();
		}
		
		//Debug.Log((int)(Time.time - timeSinceLastSpawn));
	}
	
	public void AddAllEnemy(){
		GameObject[] go = GameObject.FindGameObjectsWithTag("Enemy");
		
		foreach(GameObject enemy in go){
			AddTarget(enemy.transform);
		}
	}
	
	public void AddTarget(Transform enemy){
		curEnemys.Add(enemy);
	}
}
