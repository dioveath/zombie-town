using UnityEngine;


[RequireComponent (typeof(PlayerController))]
[RequireComponent (typeof(GunController))]
public class Player : LivingEntity {

	public float moveSpeed;
	public CrossHair crossHair;
	PlayerController playerController;
	GunController gunController;
	public System.Action OnTakeHit;
	Vector3 point;

	protected override void Start(){
		base.Start ();
		playerController = GetComponent<PlayerController> ();
		gunController = GetComponent<GunController> ();
	}

	void Update(){
		//movement
		Vector3 input = new Vector3 (Input.GetAxisRaw ("Horizontal"),0,  Input.GetAxis ("Vertical"));
		Vector3 moveVelocity = input.normalized * moveSpeed;
		playerController.Move (moveVelocity);

		//look
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		Plane groundPlane = new Plane (Vector3.up, Vector3.zero);
		float rayDistance;

		if(groundPlane.Raycast (ray,out rayDistance)){
			point = ray.GetPoint (rayDistance);
			//Debug.DrawLine (ray.origin, point, Color.blue);
			playerController.LookAt(point);
			crossHair.transform.position = new Vector3(point.x, gunController.GetHeight (), point.z);
			crossHair.DetectTargets (ray);
		}

		//gun
		if (Input.GetMouseButton (0)) {
			gunController.OnTriggerHold ();
		} else {
            gunController.OnTriggerReleased();
        }

		if (Input.GetKey (KeyCode.R)) {
			gunController.Reload ();
		}
	}

    public override void TakeHit(float damage) {
        if(health <= damage) {
            // animation 
            Destroy(this);
            Destroy(playerController);
            Destroy(gunController);
        }
		if (OnTakeHit != null) {
			OnTakeHit ();
		}
        base.TakeHit(damage);
    }

    public void SetHealth(float newHealth) {
        health = newHealth;
    }
		
}
