using UnityEngine;
using System.Collections;
using System;

public class GUIBase : MonoBehaviour {
	public Texture2D rightSideBase, leftSideBase, compass;
	public Rect left, right, compassRect;
	public int elementHeight;

    public Texture enemyBlip, playerBlip;
    public Texture radarBG;
    public Texture N, S, E, W;

    public Transform centerObject;
    public float mapScale = 0.3f;
    public Vector2 mapCenter = new Vector2(50, 50), mapSize = new Vector2(64,64), letterSize = new Vector2(5,5);
    public string enemyFilter = "Enemy", playerFilter = "Player";
    public float maxDist = 200;
    public Rect radarRect;

	
	void Start(){
		elementHeight = 75;
		left = new Rect(0, Screen.height - elementHeight, Screen.width / 2.5f, elementHeight);
		right = new Rect(Screen.width - Screen.width / 2.5f, Screen.height - elementHeight, Screen.width / 2.5f, elementHeight);
		compassRect = new Rect(Screen.width - 120, Screen.height - 40, 40, 30);
        radarRect = new Rect(Screen.width - (mapCenter.x + mapSize.x / 2), Screen.height - (mapCenter.y + mapSize.y / 2), mapSize.x + 12, mapSize.y + 12);
	}
	
	void OnGUI(){
		//GUI.DrawTexture(left, leftSideBase);
		//GUI.DrawTexture(right, rightSideBase);
		//GUI.DrawTexture (compassRect,compass);
        GUI.DrawTexture(radarRect, radarBG);
        DrawBlipsFor(enemyFilter, enemyBlip);
        DrawBlipsFor(playerFilter, playerBlip);
        Compass(N, S, E, W);
	}

    private void Compass(Texture n, Texture s, Texture e, Texture w)
    {
		int up = 4;
		int left = 6;

#region(north);
		Vector3 centerPosN = centerObject.position;
		Vector3 extPosN = new Vector3(centerPosN.x, centerPosN.y, centerPosN.z + 200);
		
		// first we need to get the distance of the enemy from the player
		float distN = Vector3.Distance(centerPosN, extPosN);
		
		float dxN = centerPosN.x - extPosN.x; // how far to the side of the player is the enemy?
		float dzN = centerPosN.z - extPosN.z; // how far in front or behind the player is the enemy?
		
		// what's the angle to turn to face the enemy - compensating for the player's turning?
		float deltayN = Mathf.Atan2(dxN, dzN) * Mathf.Rad2Deg - 270 - centerObject.eulerAngles.y;
		
		// just basic trigonometry to find the point x,y (enemy's location) given the angle deltay
		float bXN = distN * Mathf.Cos(deltayN * Mathf.Deg2Rad);
		float bYN = distN * Mathf.Sin(deltayN * Mathf.Deg2Rad);
		
		bXN = bXN * mapScale; // scales down the x-coordinate by half so that the plot stays within our radar
		bYN = bYN * mapScale; // scales down the y-coordinate by half so that the plot stays within our radar
		
		if (distN <= maxDist)
		{
			// this is the diameter of our largest radar circle
			GUI.DrawTexture(new Rect(radarRect.x + radarRect.width/2 + bXN - left, radarRect.y + radarRect.height/2 + bYN - up, letterSize.x, letterSize.y), n);
		}

#endregion();

#region(south);
		Vector3 centerPosS = centerObject.position;
		Vector3 extPosS = new Vector3(centerPosS.x, centerPosS.y, centerPosS.z - 200);
		
		// first we need to get the distance of the enemy from the player
		float distS = Vector3.Distance(centerPosS, extPosS);
		
		float dxS = centerPosS.x - extPosS.x; // how far to the side of the player is the enemy?
		float dzS = centerPosS.z - extPosS.z; // how far in front or behind the player is the enemy?
		
		// what's the angle to turn to face the enemy - compensating for the player's turning?
		float deltayS = Mathf.Atan2(dxS, dzS) * Mathf.Rad2Deg - 270 - centerObject.eulerAngles.y;
		
		// just basic trigonometry to find the point x,y (enemy's location) given the angle deltay
		float bXS = distS * Mathf.Cos(deltayS * Mathf.Deg2Rad);
		float bYS = distS * Mathf.Sin(deltayS * Mathf.Deg2Rad);
		
		bXS = bXS * mapScale; // scales down the x-coordinate by half so that the plot stays within our radar
		bYS = bYS * mapScale; // scales down the y-coordinate by half so that the plot stays within our radar
		
		if (distS <= maxDist)
		{
			// this is the diameter of our largest radar circle
			GUI.DrawTexture(new Rect(radarRect.x + radarRect.width/2 + bXS - left, radarRect.y + radarRect.height/2 + bYS - up, letterSize.x, letterSize.y), s);
		}
#endregion;

#region(east);
		Vector3 centerPosE = centerObject.position;
		Vector3 extPosE = new Vector3(centerPosE.x + 200, centerPosE.y, centerPosE.z);

		// first we need to get the distance of the enemy from the player
		float distE = Vector3.Distance(centerPosE, extPosE);
		
		float dxE = centerPosE.x - extPosE.x; // how far to the side of the player is the enemy?
		float dzE = centerPosE.z - extPosE.z; // how far in front or behind the player is the enemy?
		
		// what's the angle to turn to face the enemy - compensating for the player's turning?
		float deltayE = Mathf.Atan2(dxE, dzE) * Mathf.Rad2Deg - 270 - centerObject.eulerAngles.y;
		
		// just basic trigonometry to find the point x,y (enemy's location) given the angle deltay
		float bXE = distE * Mathf.Cos(deltayE * Mathf.Deg2Rad);
		float bYE = distE * Mathf.Sin(deltayE * Mathf.Deg2Rad);
		
		bXE = bXE * mapScale; // scales down the x-coordinate by half so that the plot stays within our radar
		bYE = bYE * mapScale; // scales down the y-coordinate by half so that the plot stays within our radar
		
		if (distE <= maxDist)
		{
			// this is the diameter of our largest radar circle
			GUI.DrawTexture(new Rect(radarRect.x + radarRect.width/2 + bXE - left, radarRect.y + radarRect.height/2 + bYE - up, (float)(letterSize.x), letterSize.y), e);
		}
#endregion;

#region(west);
		Vector3 centerPosW = centerObject.position;
		Vector3 extPosW = new Vector3(centerPosW.x - 200, centerPosW.y, centerPosW.z);
		
		// first we need to get the distance of the enemy from the player
		float distW = Vector3.Distance(centerPosW, extPosW);
		
		float dxW = centerPosW.x - extPosW.x; // how far to the side of the player is the enemy?
		float dzW = centerPosW.z - extPosW.z; // how far in front or behind the player is the enemy?
		
		// what's the angle to turn to face the enemy - compensating for the player's turning?
		float deltayW = Mathf.Atan2(dxW, dzW) * Mathf.Rad2Deg - 270 - centerObject.eulerAngles.y;
		
		// just basic trigonometry to find the point x,y (enemy's location) given the angle deltay
		float bXW = distW * Mathf.Cos(deltayW * Mathf.Deg2Rad);
		float bYW = distW * Mathf.Sin(deltayW * Mathf.Deg2Rad);
		
		bXW = bXW * mapScale; // scales down the x-coordinate by half so that the plot stays within our radar
		bYW = bYW * mapScale; // scales down the y-coordinate by half so that the plot stays within our radar
		
		if (distW <= maxDist)
		{
			// this is the diameter of our largest radar circle
			GUI.DrawTexture(new Rect(radarRect.x + radarRect.width/2 + bXW - left, radarRect.y + radarRect.height/2 + bYW - up,(float)(letterSize.x * 1.5f), letterSize.y), w);
		}
#endregion;
    }

    private void DrawBlipsFor(string tagName, Texture aTexture)
    {

        // Find all game objects with tag 
        GameObject[] gos = GameObject.FindGameObjectsWithTag(tagName);

        // Iterate through them
        foreach (GameObject go in gos)
        {
            drawBlip(go, aTexture);
        }
    }

    private void drawBlip(GameObject go, Texture aTexture)
    {
        Vector3 centerPos = centerObject.position;
        Vector3 extPos = go.transform.position;

        // first we need to get the distance of the enemy from the player
        float dist = Vector3.Distance(centerPos, extPos);

        float dx = centerPos.x - extPos.x; // how far to the side of the player is the enemy?
        float dz = centerPos.z - extPos.z; // how far in front or behind the player is the enemy?

        // what's the angle to turn to face the enemy - compensating for the player's turning?
        float deltay = Mathf.Atan2(dx, dz) * Mathf.Rad2Deg - 270 - centerObject.eulerAngles.y;

        // just basic trigonometry to find the point x,y (enemy's location) given the angle deltay
        float bX = dist * Mathf.Cos(deltay * Mathf.Deg2Rad);
        float bY = dist * Mathf.Sin(deltay * Mathf.Deg2Rad);

        bX = bX * mapScale; // scales down the x-coordinate by half so that the plot stays within our radar
        bY = bY * mapScale; // scales down the y-coordinate by half so that the plot stays within our radar

        if (dist <= maxDist)
        {
            // this is the diameter of our largest radar circle
            GUI.DrawTexture(new Rect(radarRect.x + radarRect.width/2 + bX, radarRect.y + radarRect.height/2 + bY, 4, 4), aTexture);
        }
    }
}
