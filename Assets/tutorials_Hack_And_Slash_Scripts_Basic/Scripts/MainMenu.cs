//check to see if we have some saved data in the playerprefs
//check the version of the saved data
//if the saved version of the data is not the current version
//do something
//else if the saved version is the current version
//check to see if they have a character saved - check for a character name
//if they do not have a character saved, load the character generation scene
//else if they do have a character saved
//if they want to load the character, load the character and go to level 1
//if they want to delete the character, delete the character and go to the character generation scene

using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {
	public const float VERSION = .2f;
	public bool clearPrefs = false;
	
	private string _levelToLoad = "";
	private string _characterGeneration = GameSetting2.levelNames[1];
	private string _firstLevel = GameSetting2.levelNames[3];
	
	private bool _hasCharacter = false;
	private float _percentLoaded = 0;
	private bool _displayOptions = true;
	
	// Use this for initialization
	void Start () {
		if(clearPrefs)
			PlayerPrefs.DeleteAll();
		
		if(PlayerPrefs.HasKey("ver")) {
			Debug.Log("There is a ver key");
			if(PlayerPrefs.GetFloat("ver") != VERSION) {
				Debug.Log("Saved version is not the same as current version");
				/* Upgrade playerprefs here */
			}
			else {
				Debug.Log("Saved version is the same as the current version");
				if(PlayerPrefs.HasKey("Player Name")) {
					Debug.Log("There is a Player Name tag");
					if(PlayerPrefs.GetString("Player Name") == "") {
						Debug.Log("The Player Name key does not have anything in it.");
						PlayerPrefs.DeleteAll();
						_levelToLoad = _characterGeneration;
					}
					else {
						Debug.Log("The Player Name key has a value");
						_hasCharacter = true;
						_displayOptions = true;
//						_levelToLoad = _firstLevel;
					}
				}
				else {
					Debug.Log("There is no Player Name key");
					PlayerPrefs.DeleteAll();
					PlayerPrefs.SetFloat("ver", VERSION);
					_levelToLoad = _characterGeneration;
				}
			}
		}
		else {
			Debug.Log("There is no ver key");
			Debug.Log("Deleting Keys");
			PlayerPrefs.DeleteAll();
			Debug.Log("Saving ver");
			PlayerPrefs.SetFloat("ver", VERSION);
			_levelToLoad = _characterGeneration;
		}
	
	}
	
	// Update is called once per frame
	void Update () {
		if(_levelToLoad == "")
			return;
		
		if(Application.GetStreamProgressForLevel(_levelToLoad) == 1) {
			Debug.Log("Level Ready");
			_percentLoaded = 1;
			
			if(Application.CanStreamedLevelBeLoaded(_levelToLoad)) {
				Debug.Log("Level can be streamed, so lets load it up!");
				Application.LoadLevel(_levelToLoad);
			}
		}
		else {
			_percentLoaded = Application.GetStreamProgressForLevel(_levelToLoad);
		}
	}
	
	
	void OnGUI() {
		if(_displayOptions) {
			if(_hasCharacter) {
				if(GUI.Button(new Rect(10, 10, 110, 25), "Load Character")) {
					_levelToLoad = _firstLevel;
					_displayOptions = false;
				}
				if(GUI.Button(new Rect(10, 40, 110, 25), "Delete Character")) {
					PlayerPrefs.DeleteAll();
					PlayerPrefs.SetFloat("ver", VERSION);
					_levelToLoad = _characterGeneration;
					_displayOptions = false;
				}
			}
		}
		
		if(_levelToLoad == "")
			return;
		
		GUI.Label(new Rect(Screen.width / 2 - 50, Screen.height - 45, 100, 25), (_percentLoaded * 100) + "%");
		GUI.Box(new Rect(0, Screen.height - 20, Screen.width * _percentLoaded, 15), "");
	}
	
}
