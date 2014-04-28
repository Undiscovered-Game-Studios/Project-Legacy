using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BombBehavior : MonoBehaviour {
	
	public float explosionDelay = 10, explosionRadius = 10;
	
	public int damage = 50;
	
	public bool isLit = true, isExploding = false;
	
	public List<Transform> bombables, players, enemys, moveables;
	
	
	// Use this for initialization
	void Start () {
		AddAllBombables();
	}
	
	public void AddAllBombables(){
		GameObject[] go = GameObject.FindGameObjectsWithTag("bomb-able");
		
		foreach(GameObject bombable in go){
			AddBombable(bombable.transform);
		}
		
		GameObject[] gb = GameObject.FindGameObjectsWithTag("Player");
		
		foreach(GameObject players in gb){
			AddPlayer(players.transform);
		}
		
		GameObject[] gt = GameObject.FindGameObjectsWithTag("Enemy");
		
		foreach(GameObject enemy in gt){
			AddEnemy(enemy.transform);
		}
		
		GameObject[] gj = GameObject.FindGameObjectsWithTag("bomb-move");
		
		foreach(GameObject moveable in gj){
			AddMoveable(moveable.transform);
		}
	}
	
	public void AddBombable(Transform bombable){
		bombables.Add(bombable);
	}
	
	public void AddPlayer(Transform player){
		players.Add(player);
	}
	
	public void AddEnemy(Transform enemy){
		enemys.Add(enemy);
	}
	
	public void AddMoveable(Transform moveable){
		moveables.Add(moveable);
	}
	
	public void Explode(){
		foreach(Transform trans in bombables){
			if(Vector3.Distance(transform.position, trans.position) <= explosionRadius && trans.gameObject.name != "audio insurance"){
				Destroy(trans.gameObject);
				audio.Play();
			}
			if(trans.gameObject.name == "audio insurance"){
				audio.Play();	
			}
		}
		foreach(Transform trans in enemys){
			if(Vector3.Distance(transform.position, trans.position) <= explosionRadius){
				Enemy_health_tracking eh = (Enemy_health_tracking) trans.gameObject.GetComponent ("Enemy_health_tracking");
				eh.AddjustCurentHealth(-damage);
				audio.Play();
			}
		}
		
		foreach(Transform trans in players){
			if(Vector3.Distance(transform.position, trans.position) <= explosionRadius){
				Ally_Health_Tracking al = (Ally_Health_Tracking) trans.gameObject.GetComponent ("Ally_Health_Tracking");
				al.AddjustCurentHealth(-damage);
				audio.Play();
			}
		}
		
		foreach(Transform trans in moveables){
			if(Vector3.Distance(transform.position, trans.position) <= explosionRadius){
				trans.rigidbody.isKinematic = false;
				trans.rigidbody.WakeUp();
				audio.Play();
			}
		}
		
		bombables = new List<Transform>();
		moveables = new List<Transform>();
	}
	
	// Update is called once per frame
	void Update () {
		if(isExploding == true){
			Explode();	
		}
		
		if(isLit == true){
			explosionDelay -= Time.deltaTime;
			if(explosionDelay + 5 <= 5){
				isExploding = true;
			}
			if(explosionDelay + 5 <= 0){
			Destroy(gameObject);
			}
		}
	}
}
