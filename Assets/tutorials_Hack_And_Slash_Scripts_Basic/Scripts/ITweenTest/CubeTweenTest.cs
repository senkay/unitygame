using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CubeTweenTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
		//移动
		//iTween.MoveTo(gameObject, new Vector3(20,gameObject.transform.position.y, gameObject.transform.position.z), 20);
		//振动
		//iTween.ShakePosition(gameObject, new Vector3(0,(0.1),0), 1);
		Vector3[]  paths = iTweenPath.GetPath("myPath");
		Debug.Log("paths="+paths);
		
		iTween.MoveTo(gameObject, iTween.Hash("positon",Vector3(0,0,0),
			"path",paths,
			"time", 20,
			"easetype","linear"));
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
