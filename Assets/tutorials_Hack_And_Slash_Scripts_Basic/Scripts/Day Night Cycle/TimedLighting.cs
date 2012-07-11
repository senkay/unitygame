using UnityEngine;
using System.Collections;

[AddComponentMenu("Day Night Cycle/Timed Lighting")]
public class TimedLighting : MonoBehaviour {
	public void OnEnable() {
		Messenger<bool>.AddListener("Morning Light Time", OnToggleLight);
	}

	public void OnDisable() {
		Messenger<bool>.RemoveListener("Morning Light Time", OnToggleLight);
	}
	
	private void OnToggleLight(bool b) {
		if(b) {
			GetComponent<Light>().enabled = false;
		}
		else {
			GetComponent<Light>().enabled = true;
		}
	}
}
