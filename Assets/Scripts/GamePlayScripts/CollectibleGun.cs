using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CollectibleGun :  Collectible {

	public float rotateAnglePerSec = 360f;
	public Gun gun;
	Player player;
	GunController playerGunController;

	void Awake(){
		player = FindObjectOfType<Player> ();
	}

	void Start(){
		if (player != null) {
			playerGunController = player.GetComponent<GunController> ();
		}
	}

	void Update(){
		transform.Rotate ((Vector3.right + Vector3.up) * Time.deltaTime * rotateAnglePerSec);
	}

	public override void EffectPlayer(){
		if (player != null) {
			playerGunController.EquipGun (gun);
		}
	}

	public void OnTriggerEnter(Collider collider){
		if (collider.tag == "Player") {
			EffectPlayer ();
			if (OnUsed != null) {
				OnUsed ();
			}
			Destroy (gameObject);
		}
	}

}
