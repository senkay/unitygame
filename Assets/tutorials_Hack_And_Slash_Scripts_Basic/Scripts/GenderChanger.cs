using UnityEngine;
using System.Collections;

public class GenderChanger : MonoBehaviour {
	public Texture2D[] symbol;

	private int _index = 0;

	// Use this for initialization
	void Start() {
		if( symbol.Length < 0 )
			Debug.LogWarning( "We do not have any gender symblos loaded..." );
		
		renderer.material.mainTexture = symbol[ _index ];
	}
	
	
	
	public void OnMouseEnter() {
		HighLight( true );
	}
	
	public void OnMouseExit() {
		HighLight( false );
	}
	
	public void OnMouseDown() {
		_index++;
		
		if ( _index > symbol.Length - 1 )
			_index = 0;
		
		renderer.material.mainTexture = symbol[ _index ];

		Messenger.Broadcast("ToggleGender");
	}
	
	
	private void HighLight( bool glow ) {
		Color color = Color.white;
		
		if( glow )
			color = Color.red;
		
		renderer.material.color = color;
	}
}
