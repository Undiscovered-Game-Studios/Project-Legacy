using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy_health_tracking : MonoBehaviour {
	
	public int maxHealth = 100, curHealth = 100;
	
	public float healthBarLength;
	
	public int healthBarPlace;
	
	public Texture2D horizontalHealthBar, verticalHeathBar, healthBarColor;
	
	void Start(){

	}
	
	// Update is called once per frame
	void Update () {
		AddjustCurentHealth(0);
		
		if(curHealth <= 0){
			EnemyAI ai = (EnemyAI) GetComponent ("EnemyAI");
			
			Player_Combat pc = (Player_Combat) ai.activeTarget.GetComponent ("Player_Combat");

			pc.targets.Remove(transform);
			Destroy(gameObject);		
		}
	}
	
	void OnGUI(){
		
		EnemyAI ai = (EnemyAI) GetComponent ("EnemyAI");
		
		Player_Combat pc = (Player_Combat) ai.activeTarget.GetComponent ("Player_Combat");

		int myIndex = pc.targets.IndexOf(gameObject.transform);

		if(Vector3.Distance(transform.position, pc.gameObject.transform.position) <= ai.detectionRange){

			if(myIndex == 0){
				healthBarLength = (Screen.width/2) * (curHealth / (float)maxHealth);
				healthBarPlace = 10;
			
				GUI.DrawTexture(new Rect((Screen.width - Screen.width / 2) + 90,healthBarPlace + 40, healthBarLength - 100, 20), healthBarColor);
				GUI.DrawTexture(new Rect((Screen.width - Screen.width / 2),healthBarPlace, Screen.width / 2, 60), horizontalHealthBar);
			}if(myIndex == 1){
				healthBarLength = (Screen.width / 3) * (curHealth / maxHealth);
				healthBarPlace = 70;
			
				GUI.DrawTexture(new Rect((Screen.width - Screen.width / 3) + 60,healthBarPlace + (40 - 15), healthBarLength - 65, 15), healthBarColor);
				GUI.DrawTexture(new Rect((Screen.width - Screen.width / 3),healthBarPlace, Screen.width / 3, 40), horizontalHealthBar);
			}if(myIndex == 2){
				healthBarLength = (Screen.width / 4) * (curHealth / maxHealth);
				healthBarPlace = 120;
				GUI.DrawTexture(new Rect((Screen.width - Screen.width / 4) + 45,healthBarPlace + 20, healthBarLength - 55, 10), healthBarColor);
				GUI.DrawTexture(new Rect((Screen.width - Screen.width / 4),healthBarPlace, Screen.width / 4, 30), horizontalHealthBar);
			}if(myIndex == 3){
			
				healthBarLength = Screen.height / 3;
				healthBarPlace = 155;
			
				GUI.DrawTexture(new Rect((Screen.width - (25 + 25/3) + 7),healthBarPlace + 25,15 + 15/3 - 3, healthBarLength - 35), healthBarColor);
				GUI.DrawTexture(new Rect((Screen.width - (35 + 35/3)),healthBarPlace,30 + 30/3, Screen.height / 3), verticalHeathBar);
			}
		}
	}
	
	public void AddjustCurentHealth(int adj){
		curHealth += adj;
		
		if(curHealth < 0){
			curHealth = 0;	
		}
		if(curHealth > maxHealth){
			curHealth = maxHealth;	
		}
		if(maxHealth < 1){
			maxHealth = 1;	
		}
		
		healthBarLength = (Screen.width/2) * (curHealth / (float)maxHealth);
	}
}
