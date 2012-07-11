using UnityEngine;
using System.Collections;

public class RotatePlayerArrow : MonoBehaviour {
	public bool rotateClockwise = true;
	
	private bool _arrowPressed = false;


	
	public void Update() {
		if( _arrowPressed )
			Messenger<bool>.Broadcast("RotatePlayerClockwise", rotateClockwise);
	}

	
	public void OnMouseEnter() {
		HighLight( true );
	}
	
	public void OnMouseExit() {
		HighLight( false );
	}
	
	public void OnMouseDown() {
		_arrowPressed = true;
	}
	
	public void OnMouseUp() {
		_arrowPressed = false;
	}
	
	private void HighLight( bool glow ) {
		Color color = Color.white;
		
		if( glow )
			color = Color.red;
		
		renderer.material.color = color;
	}
}
