/// <summary>
/// EnemyHealth.cs
/// Oct 20, 2010
/// Peter Laliberte
/// 
/// A basic script display the health of a mob in game
/// 
/// This script is ment to be attached to a mob, or a mob prefab
/// </summary>
using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour {
	public int maxHealth = 100;
	public int curHealth = 100;
	
	public float healthBarLength;

	// Use this for initialization
	void Start () {
		healthBarLength = Screen.width / 2;
	}
	
	// Update is called once per frame
	void Update () {
		AddjustCurrentHealth(0);
	}
	
	void OnGUI() {
		GUI.Box(new Rect(10, 40, healthBarLength, 20), curHealth + "/" + maxHealth);
	}
	
	public void AddjustCurrentHealth(int adj) {
		curHealth += adj;
		
		if(curHealth < 0)
			curHealth = 0;
		
		if(curHealth > maxHealth)
			curHealth = maxHealth;
		
		if(maxHealth < 1)
			maxHealth = 1;
		
		healthBarLength = (Screen.width / 2) * (curHealth / (float)maxHealth);
	}
}