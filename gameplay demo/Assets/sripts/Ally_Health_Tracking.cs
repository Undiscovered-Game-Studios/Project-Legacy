using UnityEngine;
using System.Collections;

public class Ally_Health_Tracking : MonoBehaviour {
	
	public int maxHealth = 100, curHealth = 100;
	
	public float healthBarLength, healthBarVerticalSpaceScale = 1, HealthBarDistanceFromTopOfScreen;
	public Vector2 healthBarPlace, healthBarSize;
	
	public Texture2D horizontalHealthBar, verticalHeathBar, healthBarColor;

	private float myIndex;
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		AddjustCurentHealth(0);
	}
	
	void OnGUI(){
		AIListControl ailc = (AIListControl)gameObject.GetComponent ("AIListControl");




		HealthBars ();
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

	public void HealthBars(){
		AIListControl ailc = (AIListControl)transform.parent.GetComponent ("AIListControl");
		int index,
			i1 = ailc.players.IndexOf(gameObject),
			i2 = ailc.activePlayer;
		index = i1 + i2;
		if (index >= (ailc.players.Count)) {
			index -= ailc.players.Count;
		}

		healthBarLength = (Screen.width/(index+2)) * (curHealth / (float)maxHealth);

		Rect healthBarColorRect = new Rect(healthBarPlace.x, 					//x-position
		                                   ((healthBarPlace.y * (index + 1)) * healthBarVerticalSpaceScale) 
		                                   - (healthBarPlace.y * healthBarVerticalSpaceScale) 
		                                   + HealthBarDistanceFromTopOfScreen, 	//y-position
		                                   healthBarLength * healthBarSize.x, 	//x-width
		                                   healthBarSize.y);					//y-height

		Rect healthBarFrameRect = new Rect(healthBarPlace.x, 					//x-position
		                                   ((healthBarPlace.y * (index + 1)) * healthBarVerticalSpaceScale) 
		                                   - (healthBarPlace.y * healthBarVerticalSpaceScale) 
		                                   + HealthBarDistanceFromTopOfScreen, 	//y-position
		                                   healthBarSize.x * (Screen.width/(index+2)), 	//x-width
		                                   healthBarSize.y);
		
		GUI.DrawTexture(healthBarColorRect, healthBarColor);

		//GUI.DrawTexture(healthBarFrameRect, horizontalHealthBar);
	}
}
