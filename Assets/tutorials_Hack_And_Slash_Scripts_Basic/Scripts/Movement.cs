/// <summary>
/// Movement.cs
/// Dec 20, 2010
/// Peter Laliberte
/// 
/// Attach this class to your player preab. It will be responsible for all of your character movements and animations.
/// </summary>
using UnityEngine;
using System.Collections;

[RequireComponent (typeof(CharacterController))]
public class Movement : MonoBehaviour {
	public float moveSpeed = 5;					//the speed our character walks at
	public float runMultiplier = 2;				//how fast the player runs compared to the walk speed
	public float strafeSpeed = 2.5f;			//the speed our character strafes at
	public float rotateSpeed = 250;				//the speed our character turns at
	
	private Transform _myTransform;				//our cached transform
	private CharacterController _controller;	//our cached CharacterController
	
	
	//Called before the script starts
	public void Awake() {
		_myTransform = transform;
		_controller = GetComponent<CharacterController>();
	}
	
	// Use this for initialization
	void Start () {
		animation.wrapMode = WrapMode.Loop;
		animation["idle"].wrapMode = WrapMode.Loop;
	}
	
	// Update is called once per frame
	void Update () {
		if(!_controller.isGrounded) {
			_controller.Move(Vector3.down * Time.deltaTime);
		}

		Turn();
		Walk();
		Strafe();
	}
	
	private void Turn() {
		if(Mathf.Abs(Input.GetAxis("Rotate Player")) > 0) {
//			Debug.Log("Rotate: " + Input.GetAxis("Rotate Player"));
			_myTransform.Rotate(0, Input.GetAxis("Rotate Player") * Time.deltaTime * rotateSpeed, 0);
		}
	}
	
	private void Walk() {
		if(Mathf.Abs(Input.GetAxis("Move Forward")) > 0) {
//			Debug.Log("Move Forward: " + Input.GetAxis("Move Forward"));
			if(Input.GetButton("Run")) {
				animation.CrossFade("run");
				_controller.SimpleMove(_myTransform.TransformDirection(Vector3.forward) * Input.GetAxis("Move Forward") * moveSpeed * runMultiplier);
			}
			else {
				animation.CrossFade("walk");
				_controller.SimpleMove(_myTransform.TransformDirection(Vector3.forward) * Input.GetAxis("Move Forward") * moveSpeed);
			}
		}
		else {
			animation.CrossFade("idle");
		}
	}
	
	private void Strafe() {
		if(Mathf.Abs(Input.GetAxis("Strafe")) > 0) {
//			Debug.Log("Strafe: " + Input.GetAxis("Strafe"));
			animation.CrossFade("side");
			_controller.SimpleMove(_myTransform.TransformDirection(Vector3.right) * Input.GetAxis("Strafe") * strafeSpeed);
		}
	}
	
}
