using UnityEngine;
using System.Collections;

public enum CharacterMeshMaterial {
	Feet,
	Legs,
	Torso,
	Hands,
	Face,
	COUNT
}

public class ChangingRoom : MonoBehaviour {
	private CharacterAsset ca;
	private PlayerCharacter pc;
	
	private int _faceMaterialIndex = 0;
	private int _torsoMaterialIndex = 0;
	private int _handMaterialIndex = 0;
	private int _legMaterialIndex = 0;
	private int _feetMaterialIndex = 0;

	private int _weaponIndex = 0;
	private int _hairMeshIndex = 0;
	
	private int _charModelIndex = 0;
	private string _charModelName = "Muscular";

	private GameObject _characterMesh;
	
	// Use this for initialization
	void Start () {
		ca = GameObject.Find("Character Asset Manager").GetComponent<CharacterAsset>();

		InstantiateCharacterModel();
		RefreshCharacterMeshMaterials();

		InstantiateWeaponModel();
		InstantiateHairMesh();
	}
	

	void OnGUI() {
		ChangeCharacterMesh();
		ChangeHairMeshGUI();
		ChangeFaceMaterialGUI();
		ChangeTorsoMaterialGUI();
		ChangeHandMaterialGUI();
		ChangeLegsMaterialGUI();
		ChangeFeetMaterialGUI();

		ChangeWeaponMesh();

		RotateCharacterModel();
	}
	
	private void ChangeHairMeshGUI() {
		if(GUI.Button(new Rect(Screen.width / 2 - 60, Screen.height - 280, 120, 30), _hairMeshIndex.ToString())) {
			_hairMeshIndex++;
			InstantiateHairMesh();
		}
	}

	
	private void InstantiateHairMesh() {
		if(_hairMeshIndex > ca.hairMesh.Length - 1)
			_hairMeshIndex = 0;
		
		if( pc.hairMount.transform.childCount > 0)
			for(int cnt = 0; cnt < pc.hairMount.transform.childCount; cnt++)
				Destroy(pc.hairMount.transform.GetChild(cnt).gameObject);
		
		GameObject w = Instantiate(ca.hairMesh[_hairMeshIndex], pc.hairMount.transform.position, Quaternion.identity) as GameObject;
		w.transform.parent = pc.hairMount.transform;
		w.transform.rotation = new Quaternion(0, 0, 0, 0);
		
		MeshOffset mo = w.GetComponent<MeshOffset>();
		
		if(mo == null)
			return;
		
		Debug.Log("We have a mesh offset");
		
		w.transform.localPosition = mo.positionOffset;
	}

	
	private void ChangeFaceMaterialGUI() {
		if(GUI.Button(new Rect(Screen.width * .5f - 95, Screen.height - 175, 30, 30), "<"))
		{
			_faceMaterialIndex--;
			ChangeMeshMaterial(CharacterMeshMaterial.Face);
		}
		
		GUI.Box(new Rect(Screen.width / 2 - 60, Screen.height - 175, 120, 30), _faceMaterialIndex.ToString());
		
		if(GUI.Button(new Rect(Screen.width * .5f + 65, Screen.height - 175, 30, 30), ">"))
		{
			_faceMaterialIndex++;
			ChangeMeshMaterial(CharacterMeshMaterial.Face);
		}
	}

	private void ChangeTorsoMaterialGUI() {
		if(GUI.Button(new Rect(Screen.width * .5f - 95, Screen.height - 140, 30, 30), "<"))
		{
			_torsoMaterialIndex--;
			ChangeMeshMaterial(CharacterMeshMaterial.Torso);
		}
		
		GUI.Box(new Rect(Screen.width / 2 - 60, Screen.height - 140, 120, 30), _torsoMaterialIndex.ToString());
		
		if(GUI.Button(new Rect(Screen.width * .5f + 65, Screen.height - 140, 30, 30), ">"))
		{
			_torsoMaterialIndex++;
			ChangeMeshMaterial(CharacterMeshMaterial.Torso);
		}
	}

	private void ChangeHandMaterialGUI() {
		if(GUI.Button(new Rect(Screen.width * .5f - 95, Screen.height - 105, 30, 30), "<"))
		{
			_handMaterialIndex--;
			ChangeMeshMaterial(CharacterMeshMaterial.Hands);
		}
		
		GUI.Box(new Rect(Screen.width / 2 - 60, Screen.height - 105, 120, 30), _handMaterialIndex.ToString());
		
		if(GUI.Button(new Rect(Screen.width * .5f + 65, Screen.height - 105, 30, 30), ">"))
		{
			_handMaterialIndex++;
			ChangeMeshMaterial(CharacterMeshMaterial.Hands);
		}
	}


	
		private void ChangeLegsMaterialGUI() {
		if(GUI.Button(new Rect(Screen.width * .5f - 95, Screen.height - 210, 30, 30), "<"))
		{
			_legMaterialIndex--;
			ChangeMeshMaterial(CharacterMeshMaterial.Legs);
		}
		
		GUI.Box(new Rect(Screen.width / 2 - 60, Screen.height - 210, 120, 30), _legMaterialIndex.ToString());
		
		if(GUI.Button(new Rect(Screen.width * .5f + 65, Screen.height - 210, 30, 30), ">"))
		{
			_legMaterialIndex++;
			ChangeMeshMaterial(CharacterMeshMaterial.Legs);
		}
	}
	private void ChangeFeetMaterialGUI() {
		if(GUI.Button(new Rect(Screen.width * .5f - 95, Screen.height - 245, 30, 30), "<"))
		{
			_feetMaterialIndex--;
			ChangeMeshMaterial(CharacterMeshMaterial.Feet);
		}
		
		GUI.Box(new Rect(Screen.width / 2 - 60, Screen.height - 245, 120, 30), _feetMaterialIndex.ToString());
		
		if(GUI.Button(new Rect(Screen.width * .5f + 65, Screen.height - 245, 30, 30), ">"))
		{
			_feetMaterialIndex++;
			ChangeMeshMaterial(CharacterMeshMaterial.Feet);
		}
	}

	
	
	
	
	
	
	
	
	
	private void RefreshCharacterMeshMaterials() {
		for(int cnt = 0; cnt < (int)CharacterMeshMaterial.COUNT; cnt++)
			ChangeMeshMaterial((CharacterMeshMaterial)cnt);
	}
	
	private void ChangeMeshMaterial(CharacterMeshMaterial cmm) {
		Material[] mats = pc.characterMaterialMesh.renderer.materials;

		for(int cnt = 0; cnt < pc.characterMaterialMesh.renderer.materials.Length; cnt++)
			mats[cnt] = pc.characterMaterialMesh.renderer.materials[cnt];

		switch(cmm) {
		case CharacterMeshMaterial.Feet:
			if(_feetMaterialIndex > ca.feetMaterial.Length - 1)
				_feetMaterialIndex = 0;
			else if(_feetMaterialIndex < 0)
				_feetMaterialIndex = ca.feetMaterial.Length - 1;

			mats[(int)cmm] = ca.feetMaterial[_feetMaterialIndex];
			break;
		case CharacterMeshMaterial.Legs:
			if(_legMaterialIndex > ca.legMaterial.Length - 1)
				_legMaterialIndex = 0;
			else if(_legMaterialIndex < 0)
				_legMaterialIndex = ca.legMaterial.Length - 1;

			mats[(int)cmm] = ca.legMaterial[_legMaterialIndex];
			break;
		case CharacterMeshMaterial.Hands:
			if(_handMaterialIndex > ca.handsMaterial.Length - 1)
				_handMaterialIndex = 0;
			else if(_handMaterialIndex < 0)
				_handMaterialIndex = ca.handsMaterial.Length - 1;

			mats[(int)cmm] = ca.handsMaterial[_handMaterialIndex];
			break;
		case CharacterMeshMaterial.Torso:
			if(_torsoMaterialIndex > ca.torsoMaterial.Length - 1)
				_torsoMaterialIndex = 0;
			else if(_torsoMaterialIndex < 0)
				_torsoMaterialIndex = ca.torsoMaterial.Length - 1;

			mats[(int)cmm] = ca.torsoMaterial[_torsoMaterialIndex];
			break;
		case CharacterMeshMaterial.Face:
			if(_faceMaterialIndex > ca.faceMaterial.Length - 1)
				_faceMaterialIndex = 0;
			else if(_faceMaterialIndex < 0)
				_faceMaterialIndex = ca.faceMaterial.Length - 1;

			mats[(int)cmm] = ca.faceMaterial[_faceMaterialIndex];
			break;
		}
		
		DestroyImmediate(pc.characterMaterialMesh.renderer.materials[(int)cmm]);

		pc.characterMaterialMesh.renderer.materials = mats;
			
//		Debug.Log(mats.Length);
//		Debug.Log("Wearing:" +pc.characterMaterialMesh.renderer.materials[2].name + " Should be wearing: " + ca.torsoMaterial[_torsoMaterialIndex].name );
//		Resources.UnloadUnusedAssets();
	}
	

	
	private void RotateCharacterModel() {
		if(GUI.RepeatButton(new Rect(Screen.width * .5f - 95, Screen.height - 35, 30, 30), "<"))
			_characterMesh.transform.Rotate(Vector3.up * Time.deltaTime * 100);
			
		if(GUI.RepeatButton(new Rect(Screen.width * .5f + 65, Screen.height - 35, 30, 30), ">"))
			_characterMesh.transform.Rotate(Vector3.down * Time.deltaTime * 100);
	}
	
	private void ChangeCharacterMesh() {
		if(GUI.Button(new Rect(Screen.width / 2 - 60, Screen.height - 35, 120, 30), _charModelName)) {
			_charModelIndex++;
			InstantiateCharacterModel();
		}
	}
	
	private void InstantiateWeaponModel() {
		if(_weaponIndex > ca.weaponMesh.Length - 1)
			_weaponIndex = 0;
		
		if( pc.weaponMount.transform.childCount > 0)
			for(int cnt = 0; cnt < pc.weaponMount.transform.childCount; cnt++) {
				Destroy(pc.weaponMount.transform.GetChild(cnt).gameObject);
			}
		
		GameObject w = Instantiate(ca.weaponMesh[_weaponIndex], pc.weaponMount.transform.position, Quaternion.identity) as GameObject;
		w.transform.parent = pc.weaponMount.transform;
		w.transform.rotation = new Quaternion(0, 0, 0, 0);
	}
	
	private void ChangeWeaponMesh() {
		if(GUI.Button(new Rect(Screen.width / 2 - 60, Screen.height - 70, 120, 30), _weaponIndex.ToString())) {
			_weaponIndex++;
			InstantiateWeaponModel();
		}
	}

	private void InstantiateCharacterModel() {
		switch(_charModelIndex) {
		case 1:
			_charModelName = "Fat";
			break;
		default:
			_charModelIndex = 0;
			_charModelName = "Muscular";
			break;
		}
		
		Quaternion oldRot;
		
		if(_characterMesh == null)
			oldRot = transform.rotation;
		else
			oldRot = _characterMesh.transform.rotation;
		
		if(transform.childCount > 0)
			for(int cnt = 0; cnt < transform.childCount; cnt++)
				Destroy(transform.GetChild(cnt).gameObject);
			
		_characterMesh = Instantiate(ca.characterMesh[_charModelIndex], transform.position, Quaternion.identity) as GameObject;
		
		_characterMesh.transform.parent = transform;
		_characterMesh.transform.rotation = oldRot;
		
		_characterMesh.animation["idle1"].wrapMode = WrapMode.Loop;
		_characterMesh.animation.Play("idle1");
		
		pc = _characterMesh.GetComponent<PlayerCharacter>();

		RefreshCharacterMeshMaterials();
		InstantiateWeaponModel();
		InstantiateHairMesh();
		
		Resources.UnloadUnusedAssets();
	}
}
