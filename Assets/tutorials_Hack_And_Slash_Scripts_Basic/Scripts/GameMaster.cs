/// <summary>
/// GameMaster.cs
/// Oct 20, 2010
/// Peter Laiberte
/// 
/// This script will be used to run the first level of our game
/// 
/// Create a GameObject called what ever you want and attach this script to it
/// </summary>
using UnityEngine;
using System.Collections;

[AddComponentMenu("Hack And Slash Tutorial/Managers/Game Master")]
public class GameMaster : MonoBehaviour {
	public GameObject playerCharacter;
	public GameObject gameSettings;

//	public Camera mainCamera;
//	public float zOffset;
//	public float yOffset;
//	public float xRotOffset;
	
	private GameObject _pc;
//	private PlayerCharacter _pcScript;
	
	public Vector3 _playerSpawnPointPos;			//this is the place in 3d space where I want my player to spawn

	// Use this for initialization
	void Start () {
		//I have commented out the following line as I am using a public variable and the inspector to place the character
//		_playerSpawnPointPos = new Vector3(240, 1, 116);		//the default position for our player spawn point
		
		GameObject go = GameObject.Find(GameSettings.PLAYER_SPAWN_POINT);
		
		if(go == null) {
//			Debug.LogWarning("Can not find Player Spawn Point");
			
			go = new GameObject(GameSettings.PLAYER_SPAWN_POINT);
//			Debug.Log("Created Player Spawn Point");
			
			go.transform.position = _playerSpawnPointPos;
//			Debug.Log("Moved Player Spawn Point");
		}
		
		_pc = Instantiate(playerCharacter, go.transform.position, Quaternion.identity) as GameObject;
		_pc.name = "pc";
		
//		_pcScript = _pc.GetComponent<PlayerCharacter>();
		
//		zOffset = -2.5f;
//		yOffset = 2.5f;
//		xRotOffset = 22.5f;
		
		//in later tutorials, we have the camera position itself and follow the player. You can comment out these lines when you have done them
//		mainCamera.transform.position = new Vector3(_pc.transform.position.x, _pc.transform.position.y + yOffset, _pc.transform.position.z + zOffset);
//		mainCamera.transform.Rotate(xRotOffset, 0, 0);
		
		LoadCharacter();
	}
	
	
	/*
	 * Load the character data.
	 */
	public void LoadCharacter() {
		GameObject gs = GameObject.Find("__GameSettings");
		
		if(gs == null) {
			GameObject gs1 = Instantiate(gameSettings, Vector3.zero, Quaternion.identity) as GameObject;
			gs1.name = "__GameSettings";
		}

			GameSettings gsScript = GameObject.Find("__GameSettings").GetComponent<GameSettings>();
			
			//loading the character data
			gsScript.LoadCharacterData();
	}
}
