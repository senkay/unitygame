/// <summary>
/// Chest.cs
/// Jan 2, 2011
/// Peter Laliberte
/// 
/// 
/// </summary>
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("Hack And Slash Tutorial/Objects/Chest")]

[RequireComponent (typeof(BoxCollider))]
[RequireComponent (typeof(AudioSource))]

public class Chest : MonoBehaviour {
	public enum State {
		open,							//the chest is completely open
		close,							//the chest is completely closed
		inbetween						//the chest is being open or closed.
	}
	
	public string openAnimName;
	public string closeAnimName;
	
	public AudioClip openSound;			//sound to play when the chest is openned
	public AudioClip closeSound;		//the sound to play when the chest is being closed
	
	public GameObject particleEffect;	//link to a particle effect for when the chest is open
	
	public GameObject[] parts;			//the parts of the chest that you want to apply the highlight to it has focus
	private Color[] _defaultColors;		//the default colors of the parts that you are using for the highlight
	
	private State state;					//our current state
	
	public float maxDistance = 2;		//the max distance the player can be to open this chest
	
	private GameObject _player;
	private Transform _myTransform;
	private bool inUse = false;
	private bool _used = false;			//track if the chest has been used or not
	
	public List<Item> loot = new List<Item>();
	
	public static float defaultLifeTimer = 120;
	private float _lifeTimer = 0;
	
	
	// Use this for initialization
	void Start () {
		_myTransform = transform;
		
		state = Chest.State.close;
		
		if(particleEffect != null)
			particleEffect.active = false;
		
		_defaultColors = new Color[parts.Length];
		
		if(parts.Length > 0)
			for(int cnt = 0; cnt < _defaultColors.Length; cnt++)
				_defaultColors[cnt] = parts[cnt].renderer.material.GetColor("_Color");
	}
	
	void Update() {
		_lifeTimer += Time.deltaTime;
		
		if(_lifeTimer > defaultLifeTimer && state == Chest.State.close)
			DestroyChest();
		
		if(!inUse)
			return;
		
		if(_player == null)
			return;
		
		if(Vector3.Distance(transform.position, _player.transform.position) > maxDistance) {
			MyGUI.chest.ForceClose();
//			Messenger.Broadcast("CloseChest");
		}
	}
	
	public void OnMouseEnter() {
//		Debug.Log("Enter");
		HighLight(true);
	}

	public void OnMouseExit() {
//		Debug.Log("Exit");
		HighLight(false);
	}

	public void OnMouseUp() {
//		Debug.Log("Up");
		GameObject go = GameObject.FindGameObjectWithTag("Player");
		
		if(go == null)
			return;
		
		if(Vector3.Distance(_myTransform.position, go.transform.position) > maxDistance && !inUse)
			return;
		
		switch(state) {
		case State.open:
			state = Chest.State.inbetween;
//			StartCoroutine("Close");
			ForceClose();
			break;
		case State.close:
			if(MyGUI.chest != null) {
				MyGUI.chest.ForceClose();
			}
			
			state = Chest.State.inbetween;
			StartCoroutine("Open");
			break;
		}
//		if(state == Chest.State.close)
//			Open();
//		else 
//			Close();
	}
	
	/// <summary>
	/// Procedure for this chest while openning.
	/// </summary>
	private IEnumerator Open() {
		//set this script to be the one that is holding the items
		MyGUI.chest = this;
		
		//find the player so we can track his distance after openning the chest
		_player = GameObject.FindGameObjectWithTag("Player");
		
		//make this chest as being in use
		inUse = true;

		//play the open animation
		animation.Play(openAnimName);

		//quickly turn on the particle animation
		if(particleEffect != null)
			particleEffect.active = true;
		
		//play the chest open sound fx
		audio.PlayOneShot(openSound);
		
		if(!_used)
			PopulateChest(5);
		
		//wait until the chest is done openning
		yield return new WaitForSeconds(animation[openAnimName].length);

		//change the chest state to open
		state = Chest.State.open;

		//send a message to the GUI to create 5 items and display them in the loot window
//		Messenger<int>.Broadcast("PopulateChest", 5, MessengerMode.DONT_REQUIRE_LISTENER);
		Messenger.Broadcast("DisplayLoot");
	}

	private void PopulateChest(int x) {
		for(int cnt = 0; cnt < x; cnt++) {
			loot.Add( ItemGenerator.CreateItem() );
//			Debug.Log( loot[cnt].Name );
		}

		_used = true;
	}

	
	private IEnumerator Close() {
		_player = null;
		inUse = false;

		animation.Play(closeAnimName);
		if(particleEffect != null)
			particleEffect.active = false;
		audio.PlayOneShot(closeSound);
		
		float tempTimer = 0;
		
		if(closeAnimName != "")
			tempTimer = animation[closeAnimName].length;
		
		if(closeSound != null)
			if(closeSound.length > tempTimer)
				tempTimer = closeSound.length;
			
		yield return new WaitForSeconds(tempTimer);

		state = Chest.State.close;
		
		if(loot.Count == 0)
			DestroyChest();
	}
	
	private void DestroyChest() {
		loot = null;
		Destroy(gameObject);
	}
	
	public void ForceClose() {
		Messenger.Broadcast("CloseChest");
		
		StopCoroutine("Open");
		StartCoroutine("Close");
	}
	
	private void HighLight(bool glow) {
		if(glow) {
			if(parts.Length > 0)
				for(int cnt = 0; cnt < _defaultColors.Length; cnt++)
					for(int matCount = 0; matCount < parts[cnt].renderer.materials.Length; matCount++)
						parts[cnt].renderer.materials[matCount].SetColor("_Color", Color.yellow);
		}
		else {
			if(parts.Length > 0)
				for(int cnt = 0; cnt < _defaultColors.Length; cnt++)
					for(int matCount = 0; matCount < parts[cnt].renderer.materials.Length; matCount++)
						parts[cnt].renderer.materials[matCount].SetColor("_Color", _defaultColors[cnt]);
		}
	}
}
