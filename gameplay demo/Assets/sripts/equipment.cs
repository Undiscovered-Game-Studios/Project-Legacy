using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class equipment : MonoBehaviour {
	public List<GameObject> armour, weapons, Items;
	public List<Texture2D> armourTex, weaponsTex, ItemsTex;
	public List<int> numberOfStacked;
	public List<string> corespondingItems;
	
	
	public void CheckPositions(){
		if(weapons[1] != null && weapons[0] == null){
			weapons[0] = weapons[1];
			weaponsTex[0] = weaponsTex[1];
			weapons[1] = new GameObject();
			weaponsTex[1] = null;
		}
		if(weapons[3] != null && weapons[2] == null){
			weapons[2] = weapons[3];
			weaponsTex[2] = weaponsTex[3];
			weapons[3] = new GameObject();
			weaponsTex[3] = null;
		}
		if(weapons[5] != null && weapons[4] == null){
			weapons[4] = weapons[5];
			weaponsTex[4] = weaponsTex[5];
			weapons[5] = new GameObject();
			weaponsTex[5] = null;
		}
		if(weapons[7] != null && weapons[6] == null){
			weapons[6] = weapons[7];
			weaponsTex[6] = weaponsTex[7];
			weapons[7] = new GameObject();
			weaponsTex[7] = null;
		}
	}
	
	void Update(){
		CheckPositions();
	}
}
