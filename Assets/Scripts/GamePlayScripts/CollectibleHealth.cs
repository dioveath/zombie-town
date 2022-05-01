using UnityEngine;
using System;

public class CollectibleHealth : Collectible {

	public float rotateAnglePerSec = 180;
    Player player;

    private void Start() {
        player = FindObjectOfType<Player>();
    }

    void Update(){
		transform.Rotate ((Vector3.right + Vector3.up) * Time.deltaTime * rotateAnglePerSec);
	}

	public override void EffectPlayer(){
        if (!player.isDead) {
            player.SetHealth(player.startingHealth);
		}
	}
		

    private void OnTriggerEnter(Collider other) {
        if(other.tag == "Player") {
            if (player != null && player.GetHealth() != player.startingHealth) {
                EffectPlayer();
				if (OnUsed != null) {
					OnUsed ();
				}
                Destroy(gameObject);
            }
        }
    }
}
