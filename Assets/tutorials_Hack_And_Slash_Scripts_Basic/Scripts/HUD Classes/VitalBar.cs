/// <summary>
/// VitalBar.cs
/// Sept 17, 2010
/// Peter Laliberte
/// 
/// This class is responsble for displaying a vital for the player character or a mob
/// 
/// Attach this script to a GUITexture that is being used for a vital bar.
/// </summary>
/// 
using UnityEngine;
using System.Collections;

public class VitalBar : MonoBehaviour {
	public bool _isPlayerHealthBar;			//this boolean value tells us if this is the player healthbar or the mob healthbar
	
	private int _maxBarLength;				//this is how large the vital bar can be if the target is at 100% health
	private int _curBarLength;				//this is the current length of the vital bar
	private GUITexture _display;
	
	
	void Awake() {
		_display = gameObject.GetComponent<GUITexture>();
	}
	
	// Use this for initialization
	void Start () {
		_maxBarLength = (int)_display.pixelInset.width;
		
		_curBarLength = _maxBarLength;
		_display.pixelInset = CalculatePosition();

		//This is commented out because we do not need to call it manually
//		OnEnable();
	}
	
	//This method is called when the gameobject is enabled
	public void OnEnable() {
		if(_isPlayerHealthBar) {
			Messenger<int, int>.AddListener("player health update", OnChangeHealthBarSize);
		}
		else {
			ToggleDisplay(false);
			Messenger<int, int>.AddListener("mob health update", OnChangeHealthBarSize);
			Messenger<bool>.AddListener("show mob vitalbars", ToggleDisplay);
		}
	}
	
	//this method is called when the gameobject is disabled
	public void OnDisable() {
		if(_isPlayerHealthBar)
			Messenger<int, int>.RemoveListener("player health update", OnChangeHealthBarSize);
		else {
			Messenger<int, int>.RemoveListener("mob health update", OnChangeHealthBarSize);
			Messenger<bool>.RemoveListener("show mob vitalbars", ToggleDisplay);
		}
	}
	
	

	//This method will calculate the total size of the healthbar in relation to the the % of health the target has left
	public void OnChangeHealthBarSize(int curHealth, int maxHealth) {
//		Debug.Log("We heard an event: curHealth = " + curHealth + " - maxHealth = " + maxHealth);
		
		_curBarLength = (int)((curHealth / (float)maxHealth) * _maxBarLength);		//this calculates the current bar length based on the players health %
		
//		_display.pixelInset = new Rect(_display.pixelInset.x, _display.pixelInset.y, _curBarLength, _display.pixelInset.height);
		_display.pixelInset = CalculatePosition();
		
	}
	

	//settig the healthbar to the player or mob
	public void SetPlayerHealth(bool b) {
		_isPlayerHealthBar = b;
	}
	
	
	//Calculate the position of the vitalbar on the screen
	private Rect CalculatePosition() {
		float yPos = _display.pixelInset.y / 2 - 10;
		
		if(!_isPlayerHealthBar){
			float xPos = (_maxBarLength - _curBarLength) - (_maxBarLength / 4 + 10);
			return new Rect(xPos, yPos, _curBarLength, _display.pixelInset.height);
		}
		
		return new Rect(_display.pixelInset.x, yPos, _curBarLength, _display.pixelInset.height);
	}
	
	
	//toggle the display of the vitalbar on and off according to the value passed in
	private void ToggleDisplay(bool show) {
			_display.enabled = show;
	}
}
