/// <summary>
/// TargetMob.cs
/// Sept 25, 2010
/// Peter Laliberte
/// 
/// This script can be attached to any permanent gameobject, and is responsible for allowing the player to target different mobs that are with in range
/// </summary>
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("Hack And Slash Tutorial/Managers/Targeting")]
public class TargetMob : MonoBehaviour {
	public List<Transform> targets;
	public Transform selectedTarget;
	
	private Transform myTransform;
		
	// Use this for initialization
	void Start () {
		targets = new List<Transform>();
		selectedTarget = null;
		myTransform = transform;
		
		AddAllEnemies();
	}
	
	public void AddAllEnemies() {
		GameObject[] go = GameObject.FindGameObjectsWithTag("Enemy");
		
		foreach(GameObject enemy in go)
			AddTarget(enemy.transform);
	}
	
	public void AddTarget(Transform enemy) {
		targets.Add(enemy);
	}
	
	
	private void SortTargetsByDistance() {
		targets.Sort(delegate(Transform t1, Transform t2) {
				return Vector3.Distance(t1.position, myTransform.position).CompareTo(Vector3.Distance(t2.position, myTransform.position));
				});
	}
	
	
	//if we do not have an enemy targeted ywt, then find the clostest one and target him
	//if we do have an enemy targeted, then get the next target
	//if we have the last target in the list, then get then first target in the list
	private void TargetEnemy() {
		if(targets.Count == 0)
			AddAllEnemies();
		
		if(targets.Count > 0) {
			if(selectedTarget == null) {
				SortTargetsByDistance();
				selectedTarget = targets[0];
			}
			else {
				int index = targets.IndexOf(selectedTarget);
				
				if(index < targets.Count - 1) {
					index++;
				}
				else {
					index = 0;
				}
				DeselectTarget();
				selectedTarget = targets[index];
			}
			SelectTarget();
		}
	}
	
	private void SelectTarget() {
		Transform name = selectedTarget.FindChild("Name");
		
		if(name == null) {
			Debug.LogError("Could not find the Name on " + selectedTarget.name);
			return;
		}
		
		name.GetComponent<TextMesh>().text = selectedTarget.GetComponent<Mob>().Name;
		name.GetComponent<MeshRenderer>().enabled = true;
		selectedTarget.GetComponent<Mob>().DisplayHealth();

		Messenger<bool>.Broadcast("show mob vitalbars", true);
	}
	
	private void DeselectTarget() {
		selectedTarget.FindChild("Name").GetComponent<MeshRenderer>().enabled = false;

		selectedTarget = null;
		Messenger<bool>.Broadcast("show mob vitalbars", false);
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Tab))
			TargetEnemy();
	}
}
