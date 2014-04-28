using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class itemControl : MonoBehaviour {
	
	public bool isStackable;
	public GameObject myObject;
	public Texture2D myTex;
	public string itemType = "misc", moreSpecific;
	public List<string> possibleItemTypes, possibleMoreSpecific;
	
	// Use this for initialization
	void Start () {
		possibleItemTypes = new List<string>();
		possibleMoreSpecific = new List<string>();
		
		possibleItemTypes.Add("misc");
		possibleItemTypes.Add("weapon");
		possibleItemTypes.Add("armour");
		
		if(itemType == "weapon"){
			possibleMoreSpecific.Add("heavyMelee");	
			possibleMoreSpecific.Add("heavyRanged");	
			possibleMoreSpecific.Add("lightMelee");	
			possibleMoreSpecific.Add("lightRanged");	
		}
		if(itemType == "armour"){
			possibleMoreSpecific.Add("helm");	
			possibleMoreSpecific.Add("chestPlate");	
			possibleMoreSpecific.Add("arms");	
			possibleMoreSpecific.Add("legs");	
		}
		if(itemType == "misc"){
			possibleMoreSpecific.Add("consumable");	
			possibleMoreSpecific.Add("amo");	
			possibleMoreSpecific.Add("explosive");	
			possibleMoreSpecific.Add("restorative");
		}
	}
}
