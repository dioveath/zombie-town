using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using System;



public class Enemy : LivingEntity {

	protected enum EnemyState {
		IDLE, ATTACKING, CHASING, ROAMING
	}

	protected EnemyState currentEnemyState = EnemyState.IDLE;
	public float damage = 1;

	[Header ("Navigation")]
	public Player target;
	protected NavMeshAgent agent;
	public float fieldOfView = 10f;

	[Header ("Attack")]
	public float attackDistanceThreshold = 1f;
	protected float animationTime = 1f;
	CapsuleCollider targetCollider;
	protected BoxCollider myBoxCollider;
	float targetRadius;
	float myRadius;

	/*
	Material hairMaterial;
	Color originalHairColor;
	*/
	[Header ("Death")]
	public ParticleSystem deathEffect;

	protected virtual void Awake(){
		target = FindObjectOfType<Player> ();
		agent = GetComponent<NavMeshAgent> ();


		//for distancing enemy within a range of player
		if (target != null && !target.isDead) {
			targetCollider = target.GetComponent<CapsuleCollider> ();
			myBoxCollider = GetComponent<BoxCollider> ();
			targetRadius = targetCollider.radius;
			myRadius = myBoxCollider.size.x / 2;
		}
	}

	protected override void Start(){
		base.Start ();


		//variables for changing hair color in attacking state
		//Transform myGraphicsT = transform.FindChild ("Graphics");
		//MeshRenderer hairRenderer = myGraphicsT.FindChild ("Hair").GetComponent<MeshRenderer>();
		//hairMaterial = hairRenderer.material;
		//originalHairColor = hairMaterial.color;
	}

	protected virtual void Update(){

		if (!target.isDead) {
			if ((target.transform.position - transform.position).sqrMagnitude <= Mathf.Pow (fieldOfView, 2)) {
				if ((target.transform.position - transform.position).sqrMagnitude <= (Mathf.Pow (attackDistanceThreshold + targetRadius + myRadius, 2))) {
					if (currentEnemyState == EnemyState.CHASING) {
						StopCoroutine ("UpdatePath");
						StartCoroutine ("Attack");
					}
					if (currentEnemyState == EnemyState.IDLE) {
						StartCoroutine ("Attack");
					}
				} else {
					if (currentEnemyState == EnemyState.IDLE) {
						StartCoroutine ("UpdatePath");
					}
				}
			} else {
				currentEnemyState = EnemyState.IDLE;
			}
		} else {
			if (currentEnemyState != EnemyState.IDLE) {
				currentEnemyState = EnemyState.IDLE;
			}
		}
	}

	IEnumerator UpdatePath(){
		currentEnemyState = EnemyState.CHASING;
		float checkRate = .25f;

		while(!target.isDead){
			Vector3 targetPosition = new Vector3 (target.transform.position.x, 0, target.transform.position.z);
			Vector3 normalizedTargetEnemy = (transform.position - targetPosition).normalized;
			Vector3 destinationPosition = targetPosition + (normalizedTargetEnemy * (targetRadius + myRadius + attackDistanceThreshold - .3f));

			if (!isDead && currentEnemyState != EnemyState.IDLE && agent != null) {
				agent.SetDestination (destinationPosition);
			}
			if (currentEnemyState == EnemyState.IDLE && agent != null) {
				agent.SetDestination (transform.position);
			}
			yield return new WaitForSeconds (checkRate);
		}
		currentEnemyState = EnemyState.IDLE;
	}

	IEnumerator Attack(){
		currentEnemyState = EnemyState.ATTACKING;


		float animationPercent = 0;
		float animationAngle = 0;
		float animationSpeed = 180 / animationTime;
		Vector3 originalPosition = transform.position;
		Vector3 targetPosition = new Vector3(target.transform.position.x, 0, target.transform.position.z);
		Vector3 normalizedTargetEnemy = (transform.position - targetPosition).normalized;
		Vector3 destinationPosition = targetPosition + (normalizedTargetEnemy * (targetRadius + myRadius) * 2);


		//Color attackColor = Color.red;
		//hairMaterial.color = attackColor;

		bool hasAppliedDamage = false;

		while (animationAngle <= 180) {
			animationPercent = Mathf.Sin ( animationAngle / 180 * (float) Math.PI);

            if (!target.isDead) {
                float distanceWithinAttack = (originalPosition - target.transform.position).magnitude;
                if (animationAngle >= 135 && !hasAppliedDamage && distanceWithinAttack <= (attackDistanceThreshold * 2)) {
                    hasAppliedDamage = true;
                    if (!target.isDead && currentEnemyState != EnemyState.IDLE) {
                        target.GetComponent<IDamageable>().TakeHit(damage);
                    }
                }
            }

			transform.position = Vector3.Lerp (originalPosition, destinationPosition, animationPercent);
			transform.LookAt (new Vector3 (targetPosition.x, transform.position.y, targetPosition.z));
			animationAngle += animationSpeed * Time.deltaTime;
			yield return null;
		}

		currentEnemyState = EnemyState.IDLE;
	}

	public void SetCharacteristics(float speed, float angularSpeed, float fieldOfView, float damage, float health){
		this.agent.speed = speed;
		this.agent.angularSpeed = angularSpeed;
		this.fieldOfView = fieldOfView;
		this.damage = damage;
		this.startingHealth = health;
	}


}
