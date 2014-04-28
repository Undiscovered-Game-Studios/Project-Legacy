using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InventoryControl : MonoBehaviour {
	
	private equipment eq;
	
	public bool isDisplayingInventory = false;
	
	#region GUI Window Variables
	private int inventoryWidth, inventoryHeight;
	private Rect inventoryRect;
	
	public int inventoryX = 100, inventoryY = 50, inventoryYOffSet = 20, areaBuffer = 10, upperAreaX = 10, upperAreaY, upperAreaHeight = 200;
	
	private int charX, charWidth;
	public Texture2D charTex;
	private Rect charRect;
	
	public int ArmSlots = 4;
	private int armSize, armX;
	
	public int WeapPresets = 4, WeapPerSet = 2;
	private int weapX, weapSize;
	
	public int ItemSlotsX = 3, ItemSlotsY = 3;
	private int itemX, itemWidthAdded, itemSize;
	
	private int openSlotY = 300, openSlotSize = 50, Cols, Rows = 4;
	public int OpenSlots = 100;
	
	private Rect sliderRect, sliderView;
	private Vector2 sliderVector = Vector2.zero;
	#endregion
	
	#region variables for selector box
	public float selectedX, selectedY, selectedInList;
	public Rect selected, selector;
	public bool somethingSelected = false;
	public GameObject selectedItem;
	public int selectedCount, curDelay, maxDelay = 10;
	public Texture2D selectedTex;
	#endregion
	
	#region Lists for items
	public List<GameObject> itemsInInventory;
	public List<Texture2D> itemTextures;
	public List<GameObject> possibleItemsToHave;
	#endregion
	
	void Start(){
		
		selectedItem = itemsInInventory[0];
		selectedTex = itemTextures[0];
		
		eq = (equipment) GetComponent ("equipment");
		
		inventoryWidth = Screen.width - inventoryX * 2;
		inventoryHeight = Screen.height - inventoryY * 2;
		
		armSize = upperAreaHeight / ArmSlots;
		weapSize = upperAreaHeight / WeapPresets;
		itemSize = upperAreaHeight / ItemSlotsY;
		charWidth = upperAreaHeight / 2;
		
		armX = upperAreaX + areaBuffer;
		charX = armX + areaBuffer + armSize + inventoryX;
		weapX = charX + areaBuffer + charWidth;
		itemX = weapX + areaBuffer + weapSize * WeapPerSet;
		
		Cols = OpenSlots / Rows;
		
		inventoryRect = new Rect(inventoryX, inventoryY + inventoryYOffSet, inventoryWidth, inventoryHeight);
		charRect = new Rect (charX, upperAreaY + inventoryY + inventoryYOffSet, charWidth, upperAreaHeight);
		sliderRect = new Rect (inventoryX + openSlotSize / 4, inventoryY + openSlotY - openSlotSize / 2 + areaBuffer, inventoryWidth - openSlotSize / 2, openSlotSize * Rows + openSlotSize / 2);
		sliderView = new Rect (inventoryX, inventoryY + openSlotY, Cols * openSlotSize - openSlotSize * 1.25f - 5, Rows * openSlotSize);
		
	}
	
	void OnCollisionEnter(Collision col){
		if(col.gameObject.tag == "item"){	
			itemControl ic = (itemControl) col.gameObject.GetComponent ("itemControl");
			
			if(ic.isStackable == true){
				if(eq.corespondingItems.Contains(col.gameObject.name)){
					int place;
					place = eq.corespondingItems.IndexOf(col.gameObject.name);
					eq.numberOfStacked[place]+=1;
				}else{
					AddItem(ic.myTex, ic.myObject);
					eq.corespondingItems.Add(col.gameObject.name);
					eq.numberOfStacked.Add(1);
				}
			}else{
				AddItem(ic.myTex, ic.myObject);
			}
			
			Destroy(col.gameObject);
		}
	}
	
	public void AddItem(Texture2D tex, GameObject go){
		itemTextures.Add(tex);
		
		foreach(GameObject obj in possibleItemsToHave){
			if(obj.name == go.name){
				itemsInInventory.Add(obj);	
			}
		}
	}
	
	public void GUIBase(){
		
		GUI.Box(inventoryRect,"inventory");
			GUI.DrawTexture(charRect,charTex);
			for(int y = 0; y < ArmSlots; y++){
				GUI.Box(new Rect(armX + inventoryX, (upperAreaY + (armSize * y)) + inventoryY + inventoryYOffSet,  armSize, armSize),
					"arm");
				GUI.DrawTexture(new Rect(armX + inventoryX, (upperAreaY + (armSize * y)) + inventoryY + inventoryYOffSet, armSize, armSize),
					eq.armourTex[(y)]);
			}
		
		for(int x = 0; x < WeapPerSet; x++){
			for(int y = 0; y < WeapPresets; y++){
				GUI.Box(new Rect(weapX +  weapSize * x, (upperAreaY + (weapSize * y)) + inventoryYOffSet + inventoryY, weapSize , weapSize),
					"weapon");
				GUI.DrawTexture(new Rect (weapX + weapSize * x, (upperAreaY + (weapSize * y)) + inventoryYOffSet + inventoryY, weapSize , weapSize),
					eq.weaponsTex[x+(y*WeapPerSet)]);
			}
		}
		
		for(int x = 0; x < ItemSlotsX; x++){
			for(int y = 0; y < ItemSlotsY; y++){
				GameObject referenceObj;
				GUI.Box(new Rect(itemX + (itemWidthAdded + itemSize) * x, (upperAreaY + (itemSize * y)) + inventoryY + inventoryYOffSet, itemSize + itemWidthAdded, itemSize),
					"item");
				GUI.DrawTexture( new Rect(itemX + (itemWidthAdded + itemSize) * x, (upperAreaY + (itemSize * y)) + inventoryY + inventoryYOffSet, itemSize + itemWidthAdded, itemSize),
					eq.ItemsTex[(x+(y*ItemSlotsX))]);
				referenceObj = eq.Items[(x+(y*ItemSlotsX))];
				if(referenceObj != null){
					int referencePlace;
					referencePlace = eq.corespondingItems.IndexOf(referenceObj.name);
					GUI.Box(new Rect(itemX + (itemWidthAdded + itemSize) * x, (upperAreaY + (itemSize * y)) + inventoryY + inventoryYOffSet, (itemSize + itemWidthAdded)/4, itemSize/4),
					eq.numberOfStacked[referencePlace].ToString());	
				}
			}
		}
		sliderVector = GUI.BeginScrollView(sliderRect, sliderVector,sliderView);
			for(int x = 0; x < Cols; x++){
				for(int y = 0; y < Rows; y++){
					if((x+y*Cols) < itemsInInventory.Count){
						GameObject referenceObj;
						referenceObj = itemsInInventory[(x+y*Cols)];
						GUI.DrawTexture(new Rect(openSlotSize * x, openSlotSize * y + openSlotY + openSlotSize, openSlotSize, openSlotSize),
							itemTextures[(x+y*Cols)]);
						itemControl itco = (itemControl) referenceObj.GetComponent ("itemControl");
						if(itco.isStackable == true){
							int referencePlace;
							referencePlace = eq.corespondingItems.IndexOf(referenceObj.name);
							GUI.Box(new Rect(openSlotSize * x, openSlotSize * y + openSlotY + openSlotSize, openSlotSize / 2, openSlotSize / 2),
								eq.numberOfStacked[referencePlace].ToString());
						}
					}else{						
					GUI.Box(new Rect(openSlotSize * x, openSlotSize * y + openSlotY + openSlotSize, openSlotSize, openSlotSize),
						"open");
					}
						
				}
			}
		GUI.EndScrollView();

	}
	
	public void SelectBase(){
		if(Input.GetKeyDown(KeyCode.D) && curDelay == 0){
			selectedX += 1;	
			curDelay = maxDelay;
		}
		if(Input.GetKeyDown(KeyCode.A) && curDelay == 0){
			selectedX -= 1;	
			curDelay = maxDelay;
		}
		if(Input.GetKeyDown(KeyCode.W) && curDelay == 0){
			selectedY -= 1;
			curDelay = maxDelay;
		}
		if(Input.GetKeyDown(KeyCode.S) && curDelay == 0){
			selectedY += 1;
			curDelay = maxDelay;
		}
		
		if(selectedY < 0){
			if(selectedX < 1){
				GUI.Box(new Rect(armX + inventoryX, (upperAreaY + (armSize * (selectedY + 4))) + inventoryY + inventoryYOffSet, armSize, armSize),"select");
				if(selectedY < -4) selectedY = -4;
				selector = new Rect(armX + inventoryX, (upperAreaY + (armSize * (selectedY + 4))) + inventoryY + inventoryYOffSet, armSize, armSize);
				if(Input.GetKeyDown(KeyCode.F)){
					if(somethingSelected == true && curDelay == 0){
						somethingSelected = false;
						curDelay = maxDelay;
					}
					if(somethingSelected == false && curDelay == 0){
						selected = new Rect(armX + inventoryX, (upperAreaY + (armSize * (selectedY + 4))) + inventoryY + inventoryYOffSet, armSize, armSize);
						somethingSelected = true;
						curDelay = maxDelay;
					}
				}				
			}
			if(selectedX >=1 && selectedX <= 2){
				GUI.Box(new Rect(weapX + weapSize * (selectedX - 1), (upperAreaY + (weapSize * (selectedY + 4))) + inventoryY + inventoryYOffSet, weapSize , weapSize),"select");
				if(selectedY < -4) selectedY = -4;
				selector = new Rect(armX + inventoryX, (upperAreaY + (armSize * (selectedY + 4))) + inventoryY + inventoryYOffSet, armSize, armSize);
				if(Input.GetKeyDown(KeyCode.F)){
						if(somethingSelected == true && curDelay == 0){
							somethingSelected = false;
							curDelay = maxDelay;
						}
						if(somethingSelected == false && curDelay == 0){
						selected = new Rect(weapX + weapSize * (selectedX - 1), (upperAreaY + (weapSize * (selectedY + 4))) + inventoryY + inventoryYOffSet, weapSize , weapSize);
						somethingSelected = true;
						curDelay = maxDelay;
					}
				}
			}
			if(selectedX > 2){
					GUI.Box(new Rect(itemX + (itemWidthAdded + itemSize) * (selectedX - 3),	(upperAreaY + (itemSize * (selectedY + 3))) + inventoryY + inventoryYOffSet, itemSize + itemWidthAdded, itemSize), "select");
				if(selectedY < -3) selectedY = -3;
				selector = new Rect(armX + inventoryX, (upperAreaY + (armSize * (selectedY + 4))) + inventoryY + inventoryYOffSet, armSize, armSize);
				if(Input.GetKeyDown(KeyCode.F)){
					if(somethingSelected == true && curDelay == 0){
						somethingSelected = false;
						curDelay = maxDelay;
					}
					if(somethingSelected == false && curDelay == 0){
						selected = new Rect(itemX + (itemWidthAdded + itemSize) * (selectedX - 3),	(upperAreaY + (itemSize * (selectedY + 3))) + inventoryY + inventoryYOffSet, itemSize + itemWidthAdded, itemSize);
						somethingSelected = true;
						curDelay = maxDelay;
					}
				}
			}
			if(selectedX > 5) selectedX = 5;
			if(selectedX < 0) selectedX = 0;
		}else{
			GUI.Box(new Rect((selectedX * openSlotSize) + sliderRect.x, selectedY * openSlotSize + sliderRect.y, openSlotSize, openSlotSize),"select");
			if(selectedY > 3) selectedY = 3;
			if(selectedX > 9){
				selectedX = 9;
				sliderVector.x += openSlotSize;
			}if(selectedX < 0){
				selectedX = 0;
				sliderVector.x -= openSlotSize;
			}
			selector = new Rect(armX + inventoryX, (upperAreaY + (armSize * (selectedY + 4))) + inventoryY + inventoryYOffSet, armSize, armSize);
			if(Input.GetKeyDown(KeyCode.F)){
				if(somethingSelected == true && curDelay == 0){
					somethingSelected = false;
					curDelay = maxDelay;
				}
				if(somethingSelected == false && curDelay == 0){
					selected = new Rect((selectedX * openSlotSize) + sliderRect.x - (int)(sliderVector.x / openSlotSize), selectedY * openSlotSize + sliderRect.y, openSlotSize, openSlotSize);
					somethingSelected = true;
					curDelay = maxDelay;
				}
			}
			selectedInList = selectedX+selectedY*Cols;
		}
		if(somethingSelected == true){
			GUI.Box(selected,"selected");
		}

	}
	
	public void WorkSelected(){
		
		itemControl ic = (itemControl) selectedItem.GetComponent ("itemControl");
		if(Input.GetKeyDown(KeyCode.F) && curDelay == 0){
			if(selectedY >= 0){
				selectedItem = itemsInInventory[(int)((selectedX + (int)(sliderVector.x / openSlotSize))+selectedY*Cols) + 2];
				selectedTex = itemTextures[(int)((selectedX + (int)(sliderVector.x / openSlotSize))+selectedY*Cols) + 2];
				selectedCount = (int)((selectedX + (int)(sliderVector.x / openSlotSize))+selectedY*Cols) + 2;
				
			}else{
				#region armour logic
				if(selectedX < 1 && ic.itemType == "armour"){
					if(ic.moreSpecific == "arms" && selectedY == -4){
						if(eq.armour[0] != null){
							itemsInInventory[selectedCount] = eq.armour[0];
							itemTextures[selectedCount] = eq.armourTex[0];
						}else{
							itemsInInventory.Remove(itemsInInventory[selectedCount]);
							itemTextures.Remove(itemTextures[selectedCount]);
						}
						
						eq.armour[0] = selectedItem;
						eq.armourTex[0] = selectedTex;
						selectedItem = new GameObject();
					}
					if(ic.moreSpecific == "chestPlate" && selectedY == -3){
						if(eq.armour[1] != null){
							itemsInInventory[selectedCount] = eq.armour[1];
							itemTextures[selectedCount] = eq.armourTex[1];
						}else{
							itemsInInventory.Remove(itemsInInventory[selectedCount]);
							itemTextures.Remove(itemTextures[selectedCount]);
						}
						
						eq.armour[1] = selectedItem;
						eq.armourTex[1] = selectedTex;
						selectedItem = new GameObject();
					}
					if(ic.moreSpecific == "arms" && selectedY == -2){
						if(eq.armour[2] != null){
							itemsInInventory[selectedCount] = eq.armour[2];
							itemTextures[selectedCount] = eq.armourTex[2];
						}else{
							itemsInInventory.Remove(itemsInInventory[selectedCount]);
							itemTextures.Remove(itemTextures[selectedCount]);
						}
						
						eq.armour[2] = selectedItem;
						eq.armourTex[2] = selectedTex;
						selectedItem = new GameObject();
					}
					if(ic.moreSpecific == "legs" && selectedY == -1){
						if(eq.armour[3] != null){
							itemsInInventory[selectedCount] = eq.armour[3];
							itemTextures[selectedCount] = eq.armourTex[3];
						}else{
							itemsInInventory.Remove(itemsInInventory[selectedCount]);
							itemTextures.Remove(itemTextures[selectedCount]);
						}
						
						eq.armour[3] = selectedItem;
						eq.armourTex[3] = selectedTex;
						selectedItem = new GameObject();
					}
				}
				#endregion
				#region weapon logic
				if(ic.itemType == "weapon"){
					if(selectedX ==1 && selectedY == -4){
						if(eq.weapons[0] != null){
							itemsInInventory[selectedCount] = eq.weapons[0];
							itemTextures[selectedCount] = eq.weaponsTex[0];
						}else{
							itemsInInventory.Remove(itemsInInventory[selectedCount]);
							itemTextures.Remove(itemTextures[selectedCount]);
						}
						
						eq.weapons[0] = selectedItem;
						eq.weaponsTex[0] = selectedTex;
						selectedItem = new GameObject();
					}
					if(selectedX ==2 && selectedY == -4){
						if(eq.weapons[1] != null){
							itemsInInventory[selectedCount] = eq.weapons[1];
							itemTextures[selectedCount] = eq.weaponsTex[1];
						}else{
							itemsInInventory.Remove(itemsInInventory[selectedCount]);
							itemTextures.Remove(itemTextures[selectedCount]);
						}
						
						eq.weapons[1] = selectedItem;
						eq.weaponsTex[1] = selectedTex;
						selectedItem = new GameObject();
					}
					if(selectedX ==1 && selectedY == -3){
						if(eq.weapons[2] != null){
							itemsInInventory[selectedCount] = eq.weapons[2];
							itemTextures[selectedCount] = eq.weaponsTex[2];
						}else{
							itemsInInventory.Remove(itemsInInventory[selectedCount]);
							itemTextures.Remove(itemTextures[selectedCount]);
						}
						
						eq.weapons[2] = selectedItem;
						eq.weaponsTex[2] = selectedTex;
						selectedItem = new GameObject();
					}
					if(selectedX ==2 && selectedY == -3){
						if(eq.weapons[3] != null){
							itemsInInventory[selectedCount] = eq.weapons[3];
							itemTextures[selectedCount] = eq.weaponsTex[3];
						}else{
							itemsInInventory.Remove(itemsInInventory[selectedCount]);
							itemTextures.Remove(itemTextures[selectedCount]);
						}
						
						eq.weapons[3] = selectedItem;
						eq.weaponsTex[3] = selectedTex;
						selectedItem = new GameObject();
					}
					if(selectedX ==1 && selectedY == -2){
						if(eq.weapons[4] != null){
							itemsInInventory[selectedCount] = eq.weapons[4];
							itemTextures[selectedCount] = eq.weaponsTex[4];
						}else{
							itemsInInventory.Remove(itemsInInventory[selectedCount]);
							itemTextures.Remove(itemTextures[selectedCount]);
						}
						
						eq.weapons[4] = selectedItem;
						eq.weaponsTex[4] = selectedTex;
						selectedItem = new GameObject();
					}
					if(selectedX ==2 && selectedY == -2){
						if(eq.weapons[5] != null){
							itemsInInventory[selectedCount] = eq.weapons[5];
							itemTextures[selectedCount] = eq.weaponsTex[5];
						}else{
							itemsInInventory.Remove(itemsInInventory[selectedCount]);
							itemTextures.Remove(itemTextures[selectedCount]);
						}
						
						eq.weapons[5] = selectedItem;
						eq.weaponsTex[5] = selectedTex;
						selectedItem = new GameObject();
					}
					if(selectedX ==1 && selectedY == -1){
						if(eq.weapons[6] != null){
							itemsInInventory[selectedCount] = eq.weapons[6];
							itemTextures[selectedCount] = eq.weaponsTex[6];
						}else{
							itemsInInventory.Remove(itemsInInventory[selectedCount]);
							itemTextures.Remove(itemTextures[selectedCount]);
						}
						
						eq.weapons[6] = selectedItem;
						eq.weaponsTex[6] = selectedTex;
						selectedItem = new GameObject();
					}
					if(selectedX ==2 && selectedY == -1){
						if(eq.weapons[7] != null){
							itemsInInventory[selectedCount] = eq.weapons[7];
							itemTextures[selectedCount] = eq.weaponsTex[7];
						}else{
							itemsInInventory.Remove(itemsInInventory[selectedCount]);
							itemTextures.Remove(itemTextures[selectedCount]);
						}
						
						eq.weapons[7] = selectedItem;
						eq.weaponsTex[7] = selectedTex;
						selectedItem = new GameObject();
					}
				}
				#endregion
				#region item logic
				if(ic.itemType == "misc"){
					if(selectedX == 3 && selectedY == -3){
						if(eq.Items[0] != null){
							itemsInInventory[selectedCount] = eq.Items[0];
							itemTextures[selectedCount] = eq.ItemsTex[0];
						}else{
							itemsInInventory.Remove(itemsInInventory[selectedCount]);
							itemTextures.Remove(itemTextures[selectedCount]);
						}
						
						eq.Items[0] = selectedItem;
						eq.ItemsTex[0] = selectedTex;
						selectedItem = new GameObject();
					}
					if(selectedX == 4 && selectedY == -3){
						if(eq.Items[1] != null){
							itemsInInventory[selectedCount] = eq.Items[1];
							itemTextures[selectedCount] = eq.ItemsTex[1];
						}else{
							itemsInInventory.Remove(itemsInInventory[selectedCount]);
							itemTextures.Remove(itemTextures[selectedCount]);
						}
						
						eq.Items[1] = selectedItem;
						eq.ItemsTex[1] = selectedTex;
						selectedItem = new GameObject();
					}
					if(selectedX == 5 && selectedY == -3){
						if(eq.Items[2] != null){
							itemsInInventory[selectedCount] = eq.Items[2];
							itemTextures[selectedCount] = eq.ItemsTex[2];
						}else{
							itemsInInventory.Remove(itemsInInventory[selectedCount]);
							itemTextures.Remove(itemTextures[selectedCount]);
						}
						
						eq.Items[2] = selectedItem;
						eq.ItemsTex[2] = selectedTex;
						selectedItem = new GameObject();
					}
					if(selectedX == 3 && selectedY == -2){
						if(eq.Items[3] != null){
							itemsInInventory[selectedCount] = eq.Items[3];
							itemTextures[selectedCount] = eq.ItemsTex[3];
						}else{
							itemsInInventory.Remove(itemsInInventory[selectedCount]);
							itemTextures.Remove(itemTextures[selectedCount]);
						}
						
						eq.Items[3] = selectedItem;
						eq.ItemsTex[3] = selectedTex;
						selectedItem = new GameObject();
					}
					if(selectedX == 4 && selectedY == -2){
						if(eq.Items[4] != null){
							itemsInInventory[selectedCount] = eq.Items[4];
							itemTextures[selectedCount] = eq.ItemsTex[4];
						}else{
							itemsInInventory.Remove(itemsInInventory[selectedCount]);
							itemTextures.Remove(itemTextures[selectedCount]);
						}
						
						eq.Items[4] = selectedItem;
						eq.ItemsTex[4] = selectedTex;
						selectedItem = new GameObject();
					}
					if(selectedX == 5 && selectedY == -2){
						if(eq.Items[5] != null){
							itemsInInventory[selectedCount] = eq.Items[5];
							itemTextures[selectedCount] = eq.ItemsTex[5];
						}else{
							itemsInInventory.Remove(itemsInInventory[selectedCount]);
							itemTextures.Remove(itemTextures[selectedCount]);
						}
						
						eq.Items[5] = selectedItem;
						eq.ItemsTex[5] = selectedTex;
						selectedItem = new GameObject();
					}
					if(selectedX == 3 && selectedY == -1){
						if(eq.Items[6] != null){
							itemsInInventory[selectedCount] = eq.Items[6];
							itemTextures[selectedCount] = eq.ItemsTex[6];
						}else{
							itemsInInventory.Remove(itemsInInventory[selectedCount]);
							itemTextures.Remove(itemTextures[selectedCount]);
						}
						
						eq.Items[6] = selectedItem;
						eq.ItemsTex[6] = selectedTex;
						selectedItem = new GameObject();
					}
					if(selectedX == 4 && selectedY == -1){
						if(eq.Items[7] != null){
							itemsInInventory[selectedCount] = eq.Items[7];
							itemTextures[selectedCount] = eq.ItemsTex[7];
						}else{
							itemsInInventory.Remove(itemsInInventory[selectedCount]);
							itemTextures.Remove(itemTextures[selectedCount]);
						}
						
						eq.Items[7] = selectedItem;
						eq.ItemsTex[7] = selectedTex;
						selectedItem = new GameObject();
					}
					if(selectedX == 5 && selectedY == -1){
						if(eq.Items[8] != null){
							itemsInInventory[selectedCount] = eq.Items[8];
							itemTextures[selectedCount] = eq.ItemsTex[8];
						}else{
							itemsInInventory.Remove(itemsInInventory[selectedCount]);
							itemTextures.Remove(itemTextures[selectedCount]);
						}
						
						eq.Items[8] = selectedItem;
						eq.ItemsTex[8] = selectedTex;
						selectedItem = new GameObject();
					}
				}
				#endregion
			}
		}
	}
	
	void OnGUI(){		
		if(isDisplayingInventory == true){
			
			GUIBase();
			
			WorkSelected();
			
			SelectBase();
			
			curDelay--;
			if(curDelay <= 0) curDelay = 0;
		}

	}
	
	void Update(){
		Rigid_contorler ri = (Rigid_contorler) GetComponent ("Rigid_contorler");
		if(isDisplayingInventory == true){
			ri.canMove = false;
		}else{
			ri.canMove = true;	
		}
		if(Input.GetButtonDown("open inventory")){
			if(isDisplayingInventory == true){
				isDisplayingInventory = false;
			}else if(isDisplayingInventory == false){
				isDisplayingInventory = true;
			}
		}
	}
}