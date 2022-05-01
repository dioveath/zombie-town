using UnityEngine;
using System.Collections;


public class CollectibleSpeed : Collectible {

	public float effectTime = 1f;
	public float newSpeed = 10f;
	public float rotateAnglePerSec = 180;
	float previousSpeed;
    Player player;
    MeshRenderer myRenderer;

    private void Start() {
        player = FindObjectOfType<Player>();
    }

    void Update(){
		transform.Rotate ((Vector3.right + Vector3.up) * Time.deltaTime * rotateAnglePerSec);
        myRenderer = transform.FindChild("Cube").GetComponent<MeshRenderer>();
	}

	public override void EffectPlayer(){
		if (player != null) {
			previousSpeed = player.moveSpeed;
			player.moveSpeed = newSpeed;
            Invoke("ResetEffect", effectTime);
        }
	}

    public void ResetEffect() {
        if (player != null) {
            player.moveSpeed = previousSpeed;
        }
    }

    public void OnCollisionEnter(Collision collision) {
        if(collision.collider.tag == "Player") {
            print("oncollisionEnterPLayer");
        }
    }

    public void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            if (player != null && player.moveSpeed != newSpeed) {
                EffectPlayer();
                Destroy(GetComponent<BoxCollider>());
                Destroy(myRenderer);
				if (OnUsed != null) {
					OnUsed ();
				}
                Destroy(gameObject, effectTime);
            }
        }
    }

}
