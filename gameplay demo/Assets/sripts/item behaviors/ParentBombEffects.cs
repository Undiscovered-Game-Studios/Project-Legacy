using UnityEngine;
using System.Collections;

public class ParentBombEffects : MonoBehaviour {

	
	
	public void PlayEffects(){
		audio.Play();
	}
	
	// Update is called once per frame
	void Update () {
		BombBehavior bb = (BombBehavior) GetComponent ("BombBehavior");
		
		if(bb.explosionDelay <= 0){
			PlayEffects();	
		}	
		
		Debug.Log(bb.explosionDelay);
	}
}
