﻿using UnityEngine;
using System.Collections;

public class PauseMenu : MonoBehaviour
{
    private delegate void MenuDelegate();
	private MenuDelegate menuFunction;
	
	private float screenHeight;
	private float screenWidth;
	private float buttonHeight;
	private float buttonWidth;
	private float tempVolume;
	
	public float volume;
	public float gamma;
	
	public GameSaveLoad saveload;

	public bool inMenu;
	
	// Use this for initialization
	void Start ()
	{
		screenHeight = Screen.height;
		screenWidth = Screen.width;
	
		buttonHeight = screenHeight * 0.08f;
		buttonWidth = screenWidth * 0.3f;
		
	//	Screen.lockCursor = true;
		
		saveload = (GameSaveLoad) gameObject.GetComponent ("GameSaveLoad");
		
		gamma = 0.2f;
		volume = 0.5f;
	}
	
	// Update void used to determine when to open the menu and what to do when the menu is open.
	
	void Update()
	{
	//Opens menu on input.
		if (Input.GetKeyDown(KeyCode.Return))
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
	
	// The equivalent of "void Update()"
	
	void OnGUI()
	{
		if(menuFunction != null)	menuFunction();
	}

	// Shows the main menu and all of it's subcategories.
	
	void mainMenu()
	{
		inMenu = true;
		if(GUI.Button(new Rect((screenWidth - buttonWidth) * 0.5f, screenHeight * 0.35f, buttonWidth, buttonHeight), "Resume Game"))
		{
			menuFunction = null;
			inMenu = false;
		}

		if(GUI.Button(new Rect((screenWidth - buttonWidth) * 0.5f, screenHeight * 0.45f, buttonWidth, buttonHeight), "Load Game"))
		{
			menuFunction = loadGame;
		}	
	
		if(GUI.Button(new Rect((screenWidth - buttonWidth) * 0.5f, screenHeight * 0.55f, buttonWidth, buttonHeight), "Settings"))
		{
			menuFunction = settings;
		}
		
		if(GUI.Button(new Rect((screenWidth - buttonWidth) * 0.5f, screenHeight * 0.65f, buttonWidth, buttonHeight), "Options"))
		{
			menuFunction = options;
		}
		
		if(GUI.Button(new Rect((screenWidth - buttonWidth) * 0.5f, screenHeight * 0.75f, buttonWidth, buttonHeight), "Exit Game"))
		{
			Application.Quit();
		}
	}
	
	// Functions for loading up a previously saved game.

	void loadGame()
	{
		GUI.Label(new Rect((screenWidth - buttonWidth) * 0.625f, screenHeight * 0.3f, buttonWidth, buttonHeight), "Load Game");
		
		if(GUI.Button(new Rect((screenWidth - buttonWidth) * 0.5f, screenHeight * 0.35f, buttonWidth, buttonHeight), "01"))
		{
			saveload.Start();
			saveload.LoadXML();
			saveload.load();
		}

		if(GUI.Button(new Rect((screenWidth - buttonWidth) * 0.5f, screenHeight * 0.45f, buttonWidth, buttonHeight), "02"))
		{
			saveload.Start();
			saveload.LoadXML();
			saveload.load();
		}	
	
		if(GUI.Button(new Rect((screenWidth - buttonWidth) * 0.5f, screenHeight * 0.55f, buttonWidth, buttonHeight), "03"))
		{
			saveload.Start();
			saveload.LoadXML();
			saveload.load();
		}
		
		if(GUI.Button(new Rect((screenWidth - buttonWidth) * 0.5f, screenHeight * 0.75f, buttonWidth, buttonHeight), "Main Menu"))
		{
			menuFunction = mainMenu;
		}
	}
	
	// Options for miscellaneous viewing options.
	
	void options()
	{
		if(GUI.Button(new Rect((screenWidth - buttonWidth) * 0.5f, screenHeight * 0.5f, buttonWidth, buttonHeight), "Device Info"))
		{
			menuFunction = deviceInfo;
		}
		
		if(GUI.Button(new Rect((screenWidth - buttonWidth) * 0.5f, screenHeight * 0.75f, buttonWidth, buttonHeight), "Main Menu"))
		{
			menuFunction = mainMenu;
		}
	}
	
	// Sub-category of "Options" that displays basic info about the current device being used.
	
	void deviceInfo()
	{
		GUI.Label(new Rect((screenWidth - buttonWidth) * 0.55f, screenHeight * 0.375f, buttonWidth, buttonHeight), "Unity player version "+Application.unityVersion);
	    GUI.Label(new Rect((screenWidth - buttonWidth) * 0.55f, screenHeight * 0.4f, buttonWidth, buttonHeight * 1.7f), "Graphics: "+SystemInfo.graphicsDeviceName+" "+
	    SystemInfo.graphicsMemorySize+"MB\n"+
	    SystemInfo.graphicsDeviceVersion+"\n"+
	    SystemInfo.graphicsDeviceVendor);
	    GUI.Label(new Rect((screenWidth - buttonWidth) * 0.55f, screenHeight * 0.5f, buttonWidth, buttonHeight), "Shadows: "+SystemInfo.supportsShadows);
	    GUI.Label(new Rect((screenWidth - buttonWidth) * 0.55f, screenHeight * 0.525f, buttonWidth, buttonHeight), "Image Effects: "+SystemInfo.supportsImageEffects);
	    GUI.Label(new Rect((screenWidth - buttonWidth) * 0.55f, screenHeight * 0.55f, buttonWidth, buttonHeight), "Render Textures: "+SystemInfo.supportsRenderTextures);
		if(GUI.Button(new Rect((screenWidth - buttonWidth) * 0.5f, screenHeight * 0.75f, buttonWidth, buttonHeight), "Back to Options"))
		{
			menuFunction = options;
		}
	}
	
	// Menu that displays setting categories.
	
	void settings()
		{
		if(GUI.Button(new Rect((screenWidth - buttonWidth) * 0.5f, screenHeight * 0.45f, buttonWidth, buttonHeight), "Audio"))
		{
			menuFunction = audio;
		}
	
		if(GUI.Button(new Rect((screenWidth - buttonWidth) * 0.5f, screenHeight * 0.55f, buttonWidth, buttonHeight), "Display"))
		{
			menuFunction = display;
		}

		if(GUI.Button(new Rect((screenWidth - buttonWidth) * 0.5f, screenHeight * 0.65f, buttonWidth, buttonHeight), "Controls"))
		{
			menuFunction = controls;
		}
		
		if(GUI.Button(new Rect((screenWidth - buttonWidth) * 0.5f, screenHeight * 0.75f, buttonWidth, buttonHeight), "Main Menu"))
		{
			menuFunction = mainMenu;
		}		
	}
	
	// Sub-category of "Settings" that allows the user to modify the audio settings.
	
	void audio()
	{
		GUI.Label(new Rect((screenWidth - buttonWidth) * 0.625f, screenHeight * 0.3f, buttonWidth, buttonHeight), "Master Volume");
		volume = GUI.HorizontalSlider(new Rect((screenWidth - buttonWidth) * 0.5f, screenHeight * 0.5f, buttonWidth, buttonHeight), volume, 0.0f, 1.0f);
		AudioListener.volume = volume;
		if(GUI.Button(new Rect((screenWidth - buttonWidth) * 0.5f, screenHeight * 0.75f, buttonWidth, buttonHeight), "Back to Settings"))
		{
			menuFunction = settings;
		}	
	}
	
	// Sub-category of "Settings" the allows the user to modify the display settings.
	
	void display()
	{
		switch (QualitySettings.currentLevel) 
		{
	        case QualityLevel.Fastest:
	        	GUI.Label(new Rect((screenWidth - buttonWidth) * 0.665f, screenHeight * 0.2f, buttonWidth, buttonHeight), "Boring");
	        	break;
	        case QualityLevel.Fast:
	        	GUI.Label(new Rect((screenWidth - buttonWidth) * 0.67f, screenHeight * 0.2f, buttonWidth, buttonHeight), "Plain");
	        	break;
	        case QualityLevel.Simple:
		        GUI.Label(new Rect((screenWidth - buttonWidth) * 0.665f, screenHeight * 0.2f, buttonWidth, buttonHeight), "Simple");
		        break;
	        case QualityLevel.Good:
		        GUI.Label(new Rect((screenWidth - buttonWidth) * 0.66f, screenHeight * 0.2f, buttonWidth, buttonHeight), "Default");
		        break;
	        case QualityLevel.Beautiful:
		        GUI.Label(new Rect((screenWidth - buttonWidth) * 0.65f, screenHeight * 0.2f, buttonWidth, buttonHeight), "Beautiful");
		        break;
	        case QualityLevel.Fantastic:
	        	GUI.Label(new Rect((screenWidth - buttonWidth) * 0.65f, screenHeight * 0.2f, buttonWidth, buttonHeight), "Fantastic");
	        	break;
			}
	    if(GUI.Button(new Rect((screenWidth - buttonWidth) * 0.45f, screenHeight * 0.35f, buttonWidth * 0.5f, buttonHeight * 0.5f), "Decrease Quality"))
		{
	        QualitySettings.DecreaseLevel();
	    }
	    if(GUI.Button(new Rect((screenWidth - buttonWidth) * 0.75f, screenHeight * 0.35f, buttonWidth * 0.5f, buttonHeight * 0.5f), "Increase Quality"))
		{
	        QualitySettings.IncreaseLevel();
	    }
		GUI.Label(new Rect((screenWidth - buttonWidth) * 0.625f, screenHeight * 0.55f, buttonWidth, buttonHeight), "Gamma Control");
		gamma = GUI.HorizontalSlider(new Rect((screenWidth - buttonWidth) * 0.5f, screenHeight * 0.6f, buttonWidth, buttonHeight), gamma, 0.0f, 1.0f);
		RenderSettings.ambientLight = new Color(gamma, gamma, gamma, 1.0f);
		if(GUI.Button(new Rect((screenWidth - buttonWidth) * 0.5f, screenHeight * 0.75f, buttonWidth, buttonHeight), "Back to Settings"))
		{
			menuFunction = settings;
		}	
	}
	
	// Sub-category of "Settings" that allows the user to modify the controls of the game.
	
	void controls()
	{
		//Functions for control settings here.
	}
}