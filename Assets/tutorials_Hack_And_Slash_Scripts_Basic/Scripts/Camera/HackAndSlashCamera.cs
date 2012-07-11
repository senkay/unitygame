/// <summary>
/// HackAndSlashCamera.cs
/// December 6, 2010
/// Peter Laliberte
/// 
/// This camera is to act as most modern RPG style Camera.
/// 
/// Attach this script to your main camera.
/// 
/// Make sure your player character has the Tag: Player so the camera can find it
/// 
/// For this camera script to work properly, you will have to make sure the following axis exist the Input Manger (Tutorial 89):
	/// Rotate Camera Button				- the button we will press to allow us to rotate the camea with the mouse - Key / Mouse Button
	/// Mouse X								- rotate the camera horizontally with the mouse (included by default) - Mouse Movement
	/// Mouse Y								- rotate the camera vertically with the mouse (included by default) - Mouse Movement
	/// Rotate Camera Horizontal Buttons	- the keyboard buttons to rotate the camera on the x axis - Axis
	/// Rotate Camera Vertical Buttons		- the keyboard buttons to rotate the camera on the y axis - Axis
	/// Reset Camera						- the button to reset the camera to the default position - Button <TODO>
///
/// </summary>
using UnityEngine;
using System.Collections;

[AddComponentMenu("Hack And Slash Tutorial/Camera-Control/Hack And Slash Camera")]
public class HackAndSlashCamera : MonoBehaviour {
	public Transform target;
	public string cameraTagName = "Player";
	
	public float walkDistance = 4;
	public float runDisatance = 8;
	public float height = 1;
	public float xSpeed = 250.0f;
	public float ySpeed = 120.0f;
	public float heightDamping = 2.0f;
	public float rotationDamping = 3.0f;

		
	private Transform _myTransform;
	private float _x;
	private float _y;
	private bool _camButtonDown = false;
	private bool _rotateCameraKeyPressed = false;

	void Awake() {
		_myTransform = transform;		//cache our transform so we do not need to look it up all of the time
	}
	
	// Use this for initialization
	void Start () {
		//if we do not have a target, let them know, else set the camera up according to where our target it.
		if(target == null)
			Debug.LogWarning("We do not have a target for the camera");
		else
			CameraSetUp();
	}
	
	void Update() {
		//detect if the player has entered any input
//		if(Input.GetMouseButtonDown(1))		//Use the Input Manager to make this a user selectable button
		if(Input.GetButtonDown("Rotate Camera Button"))		//Use the Input Manager to make this a user selectable button
			_camButtonDown = true;

//		if(Input.GetMouseButtonUp(1))		//Use the Input Manager to make this a user selectable button
		if(Input.GetButtonUp("Rotate Camera Button")) {		//Use the Input Manager to make this a user selectable button
			_x = 0;		//reset the x value
			_y = 0;		//reset the y value
			
			_camButtonDown = false;
		}
		
		if(Input.GetButtonDown("Rotate Camera Horizontal Buttons") || Input.GetButtonDown("Rotate Camera Vertical Buttons"))
			_rotateCameraKeyPressed = true;

		if(Input.GetButtonUp("Rotate Camera Horizontal Buttons") || Input.GetButtonUp("Rotate Camera Vertical Buttons")) {
			_x = 0;		//reset the x value
			_y = 0;		//reset the y value

			_rotateCameraKeyPressed = false;
		}
	}
	

	//this function is called after all of the Update functions are done.
	void LateUpdate() {
//		_myTransform.position = new Vector3(target.position.x, target.position.y + height, target.position.z - walkDistance);
//		_myTransform.LookAt(target);

		if(target != null) {											//as long as we have a target, we can move the camera around them
			if(_rotateCameraKeyPressed) {
		        _x += Input.GetAxis("Rotate Camera Horizontal Buttons") * xSpeed * 0.02f;		//Use the Input Manager to make this a user selectable button
		        _y -= Input.GetAxis("Rotate Camera Vertical Buttons") * ySpeed * 0.02f;		//Use the Input Manager to make this a user selectable button
				
				RotateCamera();
			}
			else if(_camButtonDown) {										//if the button is being held down this frame
		        _x += Input.GetAxis("Mouse X") * xSpeed * 0.02f;		//Use the Input Manager to make this a user selectable button
		        _y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;		//Use the Input Manager to make this a user selectable button
				
				RotateCamera();
			}
			else {
//				_myTransform.position = new Vector3(target.position.x, target.position.y + height, target.position.z - walkDistance);
//				_myTransform.LookAt(target);
				// Calculate the current rotation angles
				float wantedRotationAngle = target.eulerAngles.y;
				float wantedHeight = target.position.y + height;
		
				float currentRotationAngle = _myTransform.eulerAngles.y;
				float currentHeight = _myTransform.position.y;
	
				// Damp the rotation around the y-axis
				currentRotationAngle = Mathf.LerpAngle(currentRotationAngle, wantedRotationAngle, rotationDamping * Time.deltaTime);

				// Damp the height
				currentHeight = Mathf.Lerp(currentHeight, wantedHeight, heightDamping * Time.deltaTime);

				// Convert the angle into a rotation
				Quaternion currentRotation = Quaternion.Euler(0, currentRotationAngle, 0);
	
				// Set the position of the camera on the x-z plane to:
				// distance meters behind the target
				_myTransform.position = target.position;
				_myTransform.position -= currentRotation * Vector3.forward * walkDistance;

				// Set the height of the camera
				_myTransform.position = new Vector3(_myTransform.position.x, currentHeight, _myTransform.position.z);
	
				// Always look at the target
				_myTransform.LookAt (target);
			}
		}
		else {//if we do not have a target, try to find it and assign it to the target variable
			GameObject go = GameObject.FindGameObjectWithTag(cameraTagName);
			
			if(go == null)
				return;
			
			target = go.transform;
		}
	}
	
	private void RotateCamera() {
		Quaternion rotation = Quaternion.Euler(_y, _x, 0);

        Vector3 position = rotation * new Vector3(0.0f, 0.0f, -walkDistance) + target.position;
		
        _myTransform.rotation = rotation;		//set the rotation of the camera
        _myTransform.position = position;		//set the position of the camera
	}
	
	//set the camera to a default position behind the player and facing them
	public void CameraSetUp() {
		_myTransform.position = new Vector3(target.position.x, target.position.y + height, target.position.z - walkDistance);
		_myTransform.LookAt(target);
	}
}
