using UnityEngine;
using System.Collections;

public class SaveMenu : MonoBehaviour
{
    private delegate void MenuDelegate();
	private MenuDelegate menuFunction;
	
	private float screenHeight;
	private float screenWidth;
	private float buttonHeight;
	private float buttonWidth;
	private float tempVolume;
	
	public float volume;
	
	public int saveSlot;

	public bool inMenu;
	
	public GameSaveLoad saveload;
	
	// Use this for initialization
	void Start ()
	{
		GameObject player = GameObject.Find("player");
		//GameSaveLoad saveloadvar = player.GetComponent<GameSaveLoad>();
		
		screenHeight = Screen.height;
		screenWidth = Screen.width;
	
		buttonHeight = screenHeight * 0.08f;
		buttonWidth = screenWidth * 0.3f;
		
	//	Screen.lockCursor = true;
		
		saveload = (GameSaveLoad) gameObject.GetComponent ("GameSaveLoad");
	}
	
	// Update void used to determine when to open the menu and what to do when the menu is open.
	
	void Update()
	{
	
		//Opens menu on input.
		if (Input.GetKeyDown(KeyCode.Backspace))
		{
			menuFunction = mainMenu;
		}
		
		//If Menu is open.
		if(inMenu)
		{
			Time.timeScale = 0f;
		//	Screen.lockCursor = false;
			tempVolume = volume;
			AudioListener.volume = 0f;
		}
		
		//If menu is not open.
		if(!inMenu)
		{
			Time.timeScale = 1f;
		//	Screen.lockCursor = true;
			//Determines if volume is pre-set.
			if(tempVolume > 0)
			{
				volume = tempVolume;
			}
			AudioListener.volume = volume;
		}
	}
	
	void OnGUI()
	{
		menuFunction();
	}
	
	void mainMenu()
	{
		inMenu = true;
		
		if(GUI.Button(new Rect((screenWidth - buttonWidth) * 0.5f, screenHeight * 0.5f, buttonWidth, buttonHeight), "Save Game"))
		{
			menuFunction = saveGame;
		}
		
		if(GUI.Button(new Rect((screenWidth - buttonWidth) * 0.5f, screenHeight * 0.8f, buttonWidth, buttonHeight), "Resume Game"))
		{
			menuFunction = null;
			inMenu = false;
		}
	}

	void saveGame()
	{
		GUI.Label(new Rect((screenWidth - buttonWidth) * 0.625f, screenHeight * 0.3f, buttonWidth, buttonHeight), "Save Game");
		
		if(GUI.Button(new Rect((screenWidth - buttonWidth) * 0.5f, screenHeight * 0.35f, buttonWidth, buttonHeight), "--- 01 ---"))
		{
		//	saveloadvar.saveSlot = 1;
			saveload.Start();
			saveload.save();
		}

		if(GUI.Button(new Rect((screenWidth - buttonWidth) * 0.5f, screenHeight * 0.45f, buttonWidth, buttonHeight), "--- 02 ---"))
		{
		//	saveloadvar.saveSlot = 2;
			saveload.Start();
			saveload.save();
		}	
	
		if(GUI.Button(new Rect((screenWidth - buttonWidth) * 0.5f, screenHeight * 0.55f, buttonWidth, buttonHeight), "--- 03 ---"))
		{
		//	saveloadvar.saveSlot = 3;
			saveload.Start();
			saveload.save();
		}
		
		if(GUI.Button(new Rect((screenWidth - buttonWidth) * 0.5f, screenHeight * 0.75f, buttonWidth, buttonHeight), "Main Menu"))
		{
			menuFunction = mainMenu;
		}
	}
}
