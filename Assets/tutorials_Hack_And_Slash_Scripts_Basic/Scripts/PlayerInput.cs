/// <summary>
/// PlayerInput.cs
/// Jan 27, 2011
/// Peter Laliberte
/// 
/// See tutorials #89 and #90 for how to use the Input Manger that comes with Unity 3
/// 
/// Input Manager Key/Axis:
	/// Toggle Inventory 			- Open and close the inventory window - Key
	/// Toggle Character Window		- Open and close the character information window - Key
	/// Move Forward				- Axis keys to move the character forward or backwards - Axis
	/// Rotate Player				- Axis keys to turn the player left and right - Axis
	/// Strafe						- Axis keys to have the character move side to side - Axis
	/// Jump						- Button to use to make the character jump - Key
	/// Run							- Toggle to use to have the character run or walk - Key
/// 
/// </summary>
using UnityEngine;
using System.Collections;

[AddComponentMenu("Hack And Slash Tutorial/Player/All Player Scripts")]
[RequireComponent (typeof(AdvancedMovement))]
[RequireComponent (typeof(PlayerCharacter))]
public class PlayerInput : MonoBehaviour {

	// Update is called once per frame
	void Update() {
		if(Input.GetButtonUp("Toggle Inventory")) {
			Messenger.Broadcast("ToggleInventory");
		}

		if(Input.GetButtonUp("Toggle Character Window")) {
			Messenger.Broadcast("ToggleCharacterWindow");
		}

		if(Input.GetButton("Move Forward")) {
			if(Input.GetAxis("Move Forward") > 0) {
				SendMessage("MoveMeForward", AdvancedMovement.Forward.forward);
			}
			else {
				SendMessage("MoveMeForward", AdvancedMovement.Forward.back);
			}
		}
		
		if(Input.GetButtonUp("Move Forward")) {
			SendMessage("MoveMeForward", AdvancedMovement.Forward.none);
		}
		
		if(Input.GetButton("Rotate Player")) {
			if(Input.GetAxis("Rotate Player") > 0) {
				SendMessage("RotateMe", AdvancedMovement.Turn.right);
			}
			else {
				SendMessage("RotateMe", AdvancedMovement.Turn.left);
			}
		}
		
		if(Input.GetButtonUp("Rotate Player")) {
			SendMessage("RotateMe", AdvancedMovement.Turn.none);
		}
		
		if(Input.GetButton("Strafe")) {
			if(Input.GetAxis("Strafe") > 0) {
				SendMessage("Strafe", AdvancedMovement.Turn.right);
			}
			else {
				SendMessage("Strafe", AdvancedMovement.Turn.left);
			}
		}
		
		if(Input.GetButtonUp("Strafe")) {
			SendMessage("Strafe", AdvancedMovement.Turn.none);
		}
		
		if(Input.GetButtonUp("Jump")) {
			SendMessage("JumpUp");
		}
		
		if(Input.GetButtonDown("Run")) {
			SendMessage("ToggleRun");
		}
	}
	
	public void OnTriggerEnter(Collider other) {
		if(other.CompareTag("Water"))
			SendMessage("IsSwimming", true);
	}

	public void OnTriggerExit(Collider other) {
		if(other.CompareTag("Water"))
			SendMessage("IsSwimming", false);
	}
}
