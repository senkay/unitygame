/// <summary>
/// Mob.cs
/// Oct 20, 2010
/// Peter Laliberte
/// 
/// This script is responsible for controlling the mob
/// 
/// Attach this script to a mob, or a mob prefab
/// </summary>
using UnityEngine;
using System.Collections;

[AddComponentMenu("Hack And Slash Tutorial/Mob/All Mob Scripts")]
[RequireComponent (typeof(CharacterController))]
[RequireComponent (typeof(SphereCollider))]
[RequireComponent (typeof(AI))]
[RequireComponent (typeof(AdvancedMovement))]
public class Mob : BaseCharacter {
	public int curHealth;
	public int maxHealth;
	
	
	// Use this for initialization
	void Start () {
//		GetPrimaryAttribute((int)AttributeName.Constituion).BaseValue = 100;
//		GetVital((int)VitalName.Health).Update();
		
		Transform displayName = transform.FindChild("Name");
		
		if(displayName == null) {
			Debug.LogWarning("Please Add a 3DText to the mob.");
			return;
		}
		
		displayName.GetComponent<MeshRenderer>().enabled = false;
		
		Name = "Slug Mob";
	}
	
	// Update is called once per frame
	void Update () {
	}
	
	public void DisplayHealth() {
//		Messenger<int, int>.Broadcast("mob health update", curHealth, maxHealth);
	}
}
