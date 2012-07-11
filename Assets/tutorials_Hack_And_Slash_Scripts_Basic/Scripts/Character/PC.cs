using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("Hack And Slash Tutorial/Player/PC Stats")]
public class PC : BaseCharacter {
	public List<Item> inventory = new List<Item>();

	private Item _equipedWeapon;

	private static PC instance = null;
	public static PC Instance {
		get {
			if ( instance == null ) {
				Debug.Log( "Instancing a new PC" );
				GameObject go = Instantiate( Resources.Load( "Character/Model/Prefab/Human/Male/Muscular" ) ) as GameObject;
				go.name = "PC";
			}
			
			return instance;
		}
	}
	
	#region Unity functions
	public new void Awake() {
		base.Awake();
		
		instance = this;
	}
	
//	public override void Awake() {
//		base.Awake();
//		
//		/**************************
//		Check out tutorial #140 and #141 to see how we got this weaponMount
//		**************************/
//		Transform weaponMount = transform.Find("base/spine/spine_up/right_arm/right_foret_arm/right_hand/weaponSlot");
//		
//		if(weaponMount == null) {
//			Debug.LogWarning("We could not find the weapon mount");
//			return;
//		}
//		
//		int count = weaponMount.GetChildCount();
//		
//		_weaponMesh = new GameObject[count];
//		
//		for(int cnt = 0; cnt < count; cnt++) {
//			_weaponMesh[cnt] = weaponMount.GetChild(cnt).gameObject;
//		}
//
//		HideWeaponMeshes();
//	}
//	
	//we do not want to be sending messages out each frame. We will be moving this out when we get back in to combat
	void Update() {
		Messenger<int, int>.Broadcast("player health update", 80, 100, MessengerMode.DONT_REQUIRE_LISTENER);
	}
	#endregion


	public Item EquipedWeapon {
		get { return _equipedWeapon; }
		set {
			if( value == null )
				return;

			_equipedWeapon = value;
			
			if( weaponMount.transform.childCount > 0 )
				Destroy( weaponMount.transform.GetChild( 0 ).gameObject );
				        
//			Debug.Log( GameSetting2.MELEE_WEAPON_MESH_PATH + _equipedWeapon.Name );
			GameObject mesh = Instantiate( Resources.Load( GameSetting2.MELEE_WEAPON_MESH_PATH + _equipedWeapon.Name ), weaponMount.transform.position, weaponMount.transform.rotation ) as GameObject;
			mesh.transform.parent = weaponMount.transform;
		}
	}
	
	public void LoadCharacter() {
		LoadHair();
	}
	
	public void LoadHair() {
		LoadHairMesh();
		LoadHairColor();
	}

	public void LoadHairMesh() {
	}
	
	public void LoadHairColor() {
	}
	
	public void LoadHelmet() {
	}

	public void LoadShoulderPads() {
	}

	public void LoadTorsoArmor() {
	}
	
	public void LoadGloves() {
	}

	public void LoadLegArmor() {
	}

	public void LoadBoots() {
	}

	public void LoadBackItem() {
	}

}
