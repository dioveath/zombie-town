using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class LivingEntity : MonoBehaviour, IDamageable {

	public float startingHealth;
	protected float health;
	public bool isDead = false;

	public System.Action OnDeath;

	protected virtual void Start(){
		health = startingHealth;
	}

    public float GetHealth() {
        return health;
    }
		

	public virtual void TakeHit(float damage, Vector3 hitPoint, Vector3 hitDirection){
		TakeHit (damage);
	}

	public virtual void TakeHit(float damage){
		health -= damage;
		if (health <= 0 && !isDead) { //good thing to include isDead .. <= 0 
			isDead = true;
			Die ();
		}
	}

	private void Die(){
		isDead = true;
		if (OnDeath != null) {
			OnDeath ();
		}
		Destroy (gameObject, 10f);
	}

}
