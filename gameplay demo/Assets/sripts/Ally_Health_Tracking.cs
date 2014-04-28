using UnityEngine;
using System.Collections;

public class Ally_Health_Tracking : MonoBehaviour {
	
	public int maxHealth = 100, curHealth = 100;
	
	public float healthBarLength, healthBarPlace = 10;
	
	public Texture2D horizontalHealthBar, verticalHeathBar, healthBarColor;
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		AddjustCurentHealth(0);
	}
	
	void OnGUI(){
		healthBarLength = (Screen.width/2) * (curHealth / (float)maxHealth);
		healthBarPlace = 10;
		
		GUI.DrawTexture(new Rect((Screen.width/2) - healthBarLength + 10,healthBarPlace + 40, healthBarLength - 100, 20), healthBarColor);
		GUI.DrawTexture(new Rect(0,healthBarPlace, Screen.width / 2, 60), horizontalHealthBar);
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
	
	void OnCollisionStay(Collision col){
		if(col.gameObject.tag == "Fire"){
			curHealth --;
		}
	}
}
