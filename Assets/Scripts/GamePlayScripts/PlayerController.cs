using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(Rigidbody))]
public class PlayerController : MonoBehaviour {

	Rigidbody rigidBody;
	Vector3 velocity;

	void Start(){
		rigidBody = GetComponent<Rigidbody> ();
	}

	public void Move(Vector3 _velocity){
		velocity = _velocity;
	}

	// It needs to be executed at small regular speed so that it never goes through other object
	void FixedUpdate(){ 
		rigidBody.MovePosition (rigidBody.position + velocity * Time.fixedDeltaTime);
	}

	public void LookAt(Vector3 toPoint){
		Vector3 heightCorrectedPoint = new Vector3 (toPoint.x, transform.position.y, toPoint.z);
		transform.LookAt (heightCorrectedPoint);
	}
}
