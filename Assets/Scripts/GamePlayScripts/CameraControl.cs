using UnityEngine;



public class CameraControl : MonoBehaviour {

    public Transform player;
    Vector3 offsetVector;

    void Start() {
        if(player != null) {
            offsetVector = (player.position - transform.position);
        }
    }


    private void LateUpdate() {
        if(player != null) {
            transform.position = player.position - offsetVector;
        }
    }

}
