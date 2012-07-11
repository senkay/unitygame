/// <summary>
/// EnemyAttack.cs
/// Oct 20, 2010
/// Peter Laliberte
/// 
/// This is a very basic Mob Attack script that we are going to use to get use to coding in C# and Unity
/// 
/// This script is ment to be attached to a mob, or a mob prefab
/// </summary>
using UnityEngine;
using System.Collections;

public class EnemyAttack : MonoBehaviour {
	public GameObject target;
	public float attackTimer;
	public float coolDown;

	// Use this for initialization
	void Start () {
		attackTimer = 0;
		coolDown = 2.0f;
	}
	
	// Update is called once per frame
	void Update () {
		if(attackTimer > 0)
			attackTimer -= Time.deltaTime;
		
		if(attackTimer < 0)
			attackTimer = 0;
		
		if(attackTimer == 0) {
			Attack();
			attackTimer = coolDown;
		}
	}
	
	private void Attack() {
		float distance = Vector3.Distance(target.transform.position, transform.position);
		
		Vector3 dir = (target.transform.position - transform.position).normalized;
		
		float direction = Vector3.Dot(dir, transform.forward);
		
		if(distance < 2.5f) {
			if(direction > 0) {
				PlayerHealth eh = (PlayerHealth)target.GetComponent("PlayerHealth");
				eh.AddjustCurrentHealth(-10);
			}
		}
	}
}
