/// <summary>
/// PlayerCharacter.cs
/// Oct 20, 2010
/// 
/// This script controls the users ingame character.
/// 
/// Make sure this script is attached to the character prefab
/// </summary>

using UnityEngine;
using System.Collections.Generic;

[AddComponentMenu("Hack And Slash Tutorial/Player/Player Character Stats")]
public class PlayerCharacter : BaseCharacter {
//	public static GameObject[] _weaponMesh;

	private static List<Item> _inventory = new List<Item>();
	public static List<Item> Inventory {
		get{ return _inventory; }
	}
	
	private static Item _equipedWeapon;
	public static Item EquipedWeapon {
		get { return _equipedWeapon; }
		set {
			_equipedWeapon = value;
			
//			HideWeaponMeshes();
			
			if(_equipedWeapon == null)
				return;
					
//			switch(_equipedWeapon.Name) {
//			case "Sword":
//				temp += "Sword";
//				_weaponMesh[2].active = true;
//				break;
//			case "Silifi":
//				temp += "Silifi";
//				_weaponMesh[1].active = true;
//				break;
//			case "Morningstar":
//				temp += "Morningstar";
//				_weaponMesh[0].active = true;
//				break;
//			default:
//				break;
//			}
			

			if( wm.transform.childCount > 0 )
				Destroy( wm.transform.GetChild( 0 ).gameObject );
				        
//			Debug.Log( GameSetting2.MELEE_WEAPON_MESH_PATH + _equipedWeapon.Name );
			GameObject mesh = Instantiate( Resources.Load( GameSetting2.MELEE_WEAPON_MESH_PATH + _equipedWeapon.Name ), wm.transform.position, wm.transform.rotation ) as GameObject;
			mesh.transform.parent = wm.transform;
			
		}
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
	
//	private static void HideWeaponMeshes() {
//		for(int cnt = 0; cnt < _weaponMesh.Length; cnt++) {
//			_weaponMesh[cnt].active = false;
//			Debug.Log(_weaponMesh[cnt].name);
//		}
//	}
	
	private static GameObject wm;
	public void Start() {
		wm = weaponMount;
	}
}
