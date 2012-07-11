/// <summary>
/// EnemyAI.cs
/// Oct 20, 2010
/// Peter Laliberte
/// 
/// This is some very basic Mob AI we are going to use to get use to coding in C# and Unity
/// 
/// This script is ment to be attached to a mob, or a mob prefab
/// </summary>
using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour {
	public Transform target;
	public int moveSpeed;
	public int rotationSpeed;
	public int maxDistance;
	
	private Transform myTransform;
	
	void Awake() {
		myTransform = transform;
	}

	// Use this for initialization
	void Start () {
		GameObject go = GameObject.FindGameObjectWithTag("Player");
		
		target = go.transform;
		
		maxDistance = 2;	
	}
	
	// Update is called once per frame
	void Update () {
		Debug.DrawLine(target.position, myTransform.position, Color.yellow);
		
		//Look at target
		myTransform.rotation = Quaternion.Slerp(myTransform.rotation, Quaternion.LookRotation(target.position - myTransform.position), rotationSpeed * Time.deltaTime);
		
		if(Vector3.Distance(target.position, myTransform.position) > maxDistance) {
			//Move towards target
			myTransform.position += myTransform.forward * moveSpeed * Time.deltaTime;
		}
	}
}
