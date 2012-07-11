using UnityEngine;
using System.Collections;

public class SkinToneSelector : MonoBehaviour {
	public int colorCode = 1;
	
	public void OnMouseDown() {
		Debug.Log( "Skin tone: " + colorCode );
		
		PlayerModelCustomization.ChangePlayerSkinColor( colorCode );
	}

}
