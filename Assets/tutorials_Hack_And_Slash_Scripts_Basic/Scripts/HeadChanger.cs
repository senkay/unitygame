using UnityEngine;
using System.Collections;

public class HeadChanger : MonoBehaviour {
	private int _headIndex = 0;
	
	private int _maxTextureIndex = 5;

	public void OnMouseDown() {
		Debug.Log( "Head Index: " + _headIndex );
		_headIndex++;
		
		if( _headIndex > _maxTextureIndex )
			_headIndex = 0;
		
		PlayerModelCustomization.ChangeHeadIndex( _headIndex );
	}
}
