using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class cutTest : MonoBehaviour {

	public string filePath;
	public bool isLooped = false;
	public AudioClip sound;

	private int curFrame = 0;
	private double frameTime;
	private Rect rectangle1;
	public List<Texture2D> allPics;
	private Object[] pretext;
	
	// Use this for initialization
	void Start () {
		LoadFrames();
		frameTime = .03333333333333;
		rectangle1 = new Rect (0,0,Screen.width,Screen.height);
	}
	
	private void LoadFrames(){
		pretext = Resources.LoadAll(filePath);

		foreach(Texture2D texs in pretext){
			if(texs is Texture2D){
				allPics.Add((Texture2D)texs);
			}
		}
	}
	
	private void UpdateFrames(){
		if(frameTime <= 0) {
			if(curFrame >= allPics.Count -1){
				if(isLooped == true) curFrame = 0;

			}
			else curFrame++;
			frameTime = .03333333333333;
		}else frameTime -= .01;
	}

	// Update is called once per frame
	void OnGUI() {
		UpdateFrames();
		GUI.DrawTexture(rectangle1, allPics[curFrame]);
	}
}