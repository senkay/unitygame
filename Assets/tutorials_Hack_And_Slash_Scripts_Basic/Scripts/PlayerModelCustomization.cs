using UnityEngine;
using System.Collections;

public class PlayerModelCustomization : MonoBehaviour {
	#region vars
	private const string MALE_MODEL_PATH = "Character/Model/Prefab/Human/Male/";
	private const string FEMALE_MODEL_PATH = "Character/Meshes/Female/";
	private const string HEAD_TEXTURE_PATH = "Character/Faces/Human/Male/Textures/";

	public string[] maleModels;
	public string[] femaleModels;
	public float rotationSpeed = 100;
	
	private bool _usingMaleModel = true;
	private int _index = 0;

	
	private Material _headMaterial;
	public static GameObject characterMesh;
	

	public static Vector2 scale = Vector2.one;
	public static int skinColor = 1;
	public static int headIndex = 0;
	

	private bool _rotateMe = false;
	private bool _rotateClockwise = true;
	
	private static bool _update = false;
	private Hair _hair = new Hair();
	private bool _resetHair = true;

	#endregion	

	
	// Use this for initialization
	void Start() {
		if( maleModels.Length < 1 )
			Debug.LogWarning( "We have no male models" );
		
		if( femaleModels.Length < 1 )
			Debug.LogWarning( "We have no female models" );
		
		InstantiateCharacterModel();
	}
	
	public void Update() {
		if( _rotateMe ) {
			if ( _rotateClockwise )
				transform.Rotate( Vector3.up * Time.deltaTime * rotationSpeed);
			else
				transform.Rotate( Vector3.down * Time.deltaTime * rotationSpeed);
				
			_rotateMe = false;
		}
		
		//do we have a reference to a mesh
		if ( characterMesh == null )
			return;
		
		characterMesh.transform.localScale = new Vector3( scale.x, scale.y, scale.x );
		
		if( _update )
			UpdateHead();
	}
	
	public void OnEnable() {
		Messenger.AddListener( "ToggleGender", OnToggleGender );
		Messenger<bool>.AddListener( "RotatePlayerClockwise", OnRotateClockwise );
	}
	
	public void OnDisable() {
		Messenger.RemoveListener( "ToggleGender", OnToggleGender );
		Messenger<bool>.RemoveListener( "RotatePlayerClockwise", OnRotateClockwise );
	}
	
	private void InstantiateCharacterModel() {
		if( transform.childCount > 0 )
			for( int cnt = 0; cnt < transform.childCount; cnt++ )
				Destroy( transform.GetChild( cnt ).gameObject );

		
		if( _usingMaleModel )
			characterMesh = Instantiate( Resources.Load( MALE_MODEL_PATH + maleModels[ _index ] ), transform.position, transform.rotation ) as GameObject;
		else 
			characterMesh = Instantiate( Resources.Load( FEMALE_MODEL_PATH + femaleModels[ _index ] ), transform.position, transform.rotation ) as GameObject;

		characterMesh.transform.parent = transform;
		
		MeshOffset mo = characterMesh.GetComponent< MeshOffset >();
		
		if( mo != null )
			mo.transform.position = new Vector3(
												mo.transform.position.x + mo.positionOffset.x,
												mo.transform.position.y + mo.positionOffset.y,
												mo.transform.position.z + mo.positionOffset.z
			                                    );
			
		if( characterMesh.GetComponent<Animation>() != null ) {
			characterMesh.animation[ "idle1" ].wrapMode = WrapMode.Loop;
			characterMesh.animation.Play( "idle1" );
		}
	}
	
	private void OnRotateClockwise( bool clockwise ) {
		_rotateMe = true;
		_rotateClockwise = clockwise;
	}
	
	public void OnToggleGender() {
		_usingMaleModel = !_usingMaleModel;
		_index = 0;
		InstantiateCharacterModel();
	}
	
	public static void ChangePlayerSkinColor( int color ) {
		//store the color the player has selected
		skinColor = color;
		
		//change to proper head and hands for the color
	}
	
	public static void ChangeHeadIndex( int index ) {
		headIndex = index;
		
//		Debug.Log( "Head set to index: " + headIndex );
		_update = true;
		
	}
	
	public void UpdateHead() {
		_headMaterial = characterMesh.GetComponent<PlayerCharacter>().characterMaterialMesh.renderer.materials[4];	//4 is the index of the material for the head/face texture
		_headMaterial.mainTexture = Resources.Load( HEAD_TEXTURE_PATH + "head_" + headIndex + "_" + skinColor + ".human") as Texture;
	}
	
	public void OnGUI() {
		if ( _resetHair ) {
			_hair.LoadInitailHair();
			_resetHair = false;
		}
		
		_hair.OnGUI();
		
		if( GUI.Button( new Rect( Screen.width - 55, Screen.height - 30, 50, 25 ), "Next" ) ) {
			SaveCustomizations();
			Application.LoadLevel( GameSetting2.levelNames[3] );
		}
	}
	
	private void SaveCustomizations() {
		GameSetting2.SaveHair( _hair.hairMeshIndex, _hair.hairColorIndex );
		GameSetting2.SaveCharacterScale( scale.x, scale.y );
		
//		Debug.Log( GameSetting2.LoadCharacterScale()[0] + " : " + GameSetting2.LoadCharacterScale()[1] );
//		LoadSetting();
	}
	
//	private void LoadSetting() {
//		Debug.Log( "Saved Value; " + GameSetting2.LoadHairMesh() );
//	}
}
