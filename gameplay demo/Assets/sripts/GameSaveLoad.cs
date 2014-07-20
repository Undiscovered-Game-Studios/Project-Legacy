using UnityEngine; 
using System.Collections; 
using System.Xml; 
using System.Xml.Serialization; 
using System.IO; 
using System.Text; 

public class GameSaveLoad : MonoBehaviour
{	
	// This is our local private members 
	private float screenHeight;
	private float screenWidth;
	private float buttonHeight;
	private float buttonWidth;
	bool _ShouldSave, _ShouldLoad,_SwitchSave,_SwitchLoad; 
	string _FileLocation,_FileName; 
	public GameObject _Player; 
	UserData myData; 
	string _PlayerName; 
	string _data;
	int savenumber;
	int saveSlot;



/**************************************************************************************************/



	// When the EGO is instantiated the Start will trigger 
	// so we set-up our initial values for our local members 
	public void Start () {
		screenHeight = Screen.height;
		screenWidth = Screen.width;
	
		buttonHeight = screenHeight * 0.08f;
		buttonWidth = screenWidth * 0.3f;
		// Determines what save slot and file number to use.
		FileInfo t = new FileInfo(_FileLocation+"\\"+ _FileName); 
		if(!t.Exists)
		{
			savenumber =  1;
		}
		else if(t.Exists == true && savenumber == 1)
		{
			savenumber = 2;
		}
		else if(t.Exists == true && savenumber == 2)
		{
			savenumber = 3;
		}
		else if(t.Exists == true && savenumber == 3)
		{
			//functions to handle file overwrite
		}
		
		// Where we want to save and load to and from 
		_FileLocation=Application.dataPath; 
		_FileName="SaveData_"; 
		
		// we need something to store the information into 
		myData=new UserData(); 
	}



/**************************************************************************************************/



	//*************************************************** 
	// Loading The Player... 
	// **************************************************       
	public void load() {
		if(_data.ToString() != "") 
		{ 
			// notice how I use a reference to type (UserData) here, you need this 
			// so that the returned object is converted into the correct type 
			myData = (UserData)DeserializeObject(_data); 
			// set the players position to the data we loaded			
			_Player.transform.position=myData._iUser.v; 
			_Player.transform.rotation=myData._iUser.q;
		} 
	} 

	//*************************************************** 
	// Saving The Player... 
	// **************************************************    
	public void save() {
		Debug.Log(saveSlot);
		Debug.Log(saveSlot);
		Debug.Log(saveSlot);
		Debug.Log(saveSlot);
		Debug.Log(saveSlot);
		myData._iUser.v=_Player.transform.position;
		myData._iUser.q=_Player.transform.rotation;  

		// Creates XML 
		_data = SerializeObject(myData); 
		// This is the final resulting XML from the serialization process 
		CreateXML();
	} 



/**************************************************************************************************/



	string UTF8ByteArrayToString(byte[] characters) 
	{      
		UTF8Encoding encoding = new UTF8Encoding(); 
		string constructedString = encoding.GetString(characters); 
		return (constructedString); 
	} 
	
	byte[] StringToUTF8ByteArray(string pXmlString) 
	{ 
		UTF8Encoding encoding = new UTF8Encoding(); 
		byte[] byteArray = encoding.GetBytes(pXmlString); 
		return byteArray; 
	} 
	
	// Here we serialize our UserData object of myData 
	string SerializeObject(object pObject) 
	{ 
		string XmlizedString = null; 
		MemoryStream memoryStream = new MemoryStream(); 
		XmlSerializer xs = new XmlSerializer(typeof(UserData)); 
		XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8); 
		xs.Serialize(xmlTextWriter, pObject); 
		memoryStream = (MemoryStream)xmlTextWriter.BaseStream; 
		XmlizedString = UTF8ByteArrayToString(memoryStream.ToArray()); 
		return XmlizedString; 
	} 
	
	// Here we de-serialize it back into its original form 
	object DeserializeObject(string pXmlizedString) 
	{ 
		XmlSerializer xs = new XmlSerializer(typeof(UserData)); 
		MemoryStream memoryStream = new MemoryStream(StringToUTF8ByteArray(pXmlizedString)); 
		XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8); 
		return xs.Deserialize(memoryStream); 
	} 



/**************************************************************************************************/



	// Finally our save and load methods for the file itself 
	void CreateXML() 
	{ 
		StreamWriter writer;
		FileInfo t = new FileInfo(_FileLocation+"\\"+ _FileName);
		if(!t.Exists)
		{
			writer = t.CreateText();
		}
		else if(t.Exists == true && savenumber == 1)
		{
			writer = t.CreateText();
		}
		else if(t.Exists == true && savenumber == 2)
		{
			writer = t.CreateText();
		}
		else if(t.Exists == true && savenumber == 3)
		{
			if(GUI.Button(new Rect((screenWidth - buttonWidth) * 0.5f, screenHeight * 0.35f, buttonWidth, buttonHeight), "Overwrite ---0"+savenumber+"---?"))
			{
				t.Delete();
				writer = t.CreateText();
			}
		}
	} 
	
	public void LoadXML() 
	{ 
		StreamReader reader = File.OpenText(_FileLocation+"\\"+ _FileName); 
		string _info = reader.ReadToEnd(); 
		reader.Close(); 
		_data=_info;
	} 
} 

// UserData is our custom class that holds our defined objects we want to store in XML format 
public class UserData 
{ 
	// We have to define a default instance of the structure 
	public DemoData _iUser; 
	// Default constructor doesn't really do anything at the moment 
	public UserData() { } 
	
	// Anything we want to store in the XML file, we define it here 
	public struct DemoData 
	{ 
		public Vector3 v;
		public Quaternion q;
	} 
}