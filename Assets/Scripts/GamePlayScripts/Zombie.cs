using UnityEngine;
using UnityEngine.AI;

public class Zombie : Enemy {

	Animator animator;
	public Transform zombieModel;

	protected override void Awake(){
		base.Awake ();
		animator = zombieModel.GetComponent<Animator> ();
	}

	protected override void Start(){
		base.Start ();
	}

	protected override void Update(){
		base.Update ();
		/*if (currentEnemyState == EnemyState.IDLE) {
			animator.SetBool ("idle", true);
		} else {
			animator.SetBool ("idle", false);
		}

		if (currentEnemyState == EnemyState.ATTACKING) {
			animator.SetBool ("attack", true);
		} else {
			animator.SetBool ("attack", false);
		}

		if (currentEnemyState == EnemyState.CHASING) {
			animator.SetBool ("run", true);
		} else {
			animator.SetBool ("run", false);
		}*/
		if (currentEnemyState == EnemyState.ATTACKING) {
			animator.SetBool ("IsAttacking", true);
		} else {
			animator.SetBool ("IsAttacking", false);
		}
		if (currentEnemyState == EnemyState.CHASING) {
			animator.SetBool ("IsRunning", true);
		} else {
			animator.SetBool ("IsRunning", false);
		}

	}

	public override void TakeHit(float damage, Vector3 hitPoint, Vector3 hitDirection){
		if (health - damage <= 0) {
			Destroy ((Instantiate (deathEffect, hitPoint, Quaternion.FromToRotation (Vector3.forward, hitDirection))).gameObject, deathEffect.main.startLifetimeMultiplier);
			Destroy (this);
			if (agent != null) {
				Destroy (agent);
			} if (myBoxCollider != null) {
				Destroy (myBoxCollider);
			}
			AudioManager.instance.PlaySound ("ZombieDie", transform.position);
			animator.SetTrigger ("die");
		} else {
			animator.Play ("wound", -1, 0f);
			AudioManager.instance.PlaySound ("ZombieTakeHit", transform.position);
		}
		base.TakeHit (damage);
	}
}

/*public class Zombie : LivingEntity {
//	enum ZombieState {
//		IDLE, 
//		ATTACKING,
//		RUNNING,
//WOUND,
//		DIE
//	};

//	ZombieState currentState = ZombieState.IDLE;
//	Animator animator;
//	public Transform zombieModel;
//	NavMeshAgent navAgent;

//	void Start(){
//		animator = zombieModel.GetComponent<Animator> ();
//		navAgent = GetComponent<NavMeshAgent> ();
//	}
//
//	void Update(){
//	}
//}*/
