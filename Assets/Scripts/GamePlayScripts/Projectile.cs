using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Projectile : MonoBehaviour {

	public LayerMask enemyMask;

	float speed = 10f;
	float lifeTime = 3f;
	public float damage = 1;
	float skinWidth = .1f;

	void Start(){
		Destroy (gameObject, lifeTime);
		Collider[] colliders = Physics.OverlapSphere (transform.position, .1f, enemyMask);
		if (colliders.Length > 0) {
			OnHitObject (colliders[0], transform.position);
		}
	}

	void Update(){
		float moveDistance = speed * Time.deltaTime;
		CheckCollisions (moveDistance);
		transform.Translate (Vector3.forward  * moveDistance);
	}

	public void CheckCollisions(float moveDistance){
		Ray ray = new Ray (transform.position, transform.forward);
		RaycastHit hit;

		if(Physics.Raycast (ray, out hit, moveDistance + skinWidth, enemyMask, QueryTriggerInteraction.Collide)){
			OnHitObject (hit.collider, hit.point);
		}
	}
		
	public void OnHitObject(Collider collider, Vector3 hitPoint){
		IDamageable damageableObject = collider.GetComponent<IDamageable> ();
		if (damageableObject != null) {
			damageableObject.TakeHit (damage, hitPoint, transform.forward);
		}
		Destroy (gameObject);
	}

	public void SetSpeed(float newSpeed){ 
		this.speed = newSpeed;
	}
}
