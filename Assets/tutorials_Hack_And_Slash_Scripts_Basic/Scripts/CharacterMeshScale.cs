using UnityEngine;
using System.Collections;

public class CharacterMeshScale : MonoBehaviour {
	public float minHeight = .75f;
	public float maxheight = 1.1f;
	
	public float minWidth = .8f;
	public float maxWidth = 1.5f;
	
	private float _width = 1;
	private float _height = 1;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		PlayerModelCustomization.scale = new Vector2( _width, _height );
	}
	
	void OnGUI() {
		//adjust the width of our character
		_width = GUI.HorizontalSlider( new Rect( 310, Screen.height - 50, 300, 20 ), _width, minWidth, maxWidth );
		
		//adjust the height of the character
		_height = GUI.VerticalSlider( new Rect( Screen.width - 50, 10, 20, 300 ), _height, maxheight, minHeight );
		
	}
}
