using UnityEngine;



public class GunController : MonoBehaviour {

	public Transform weaponHold;
	public Gun startingGun;
	Gun equippedGun;

	private void Start(){
		if (startingGun != null) {
			EquipGun (startingGun);
		}
	}

	public void EquipGun(Gun gun){
		if (equippedGun != null) {
			Destroy (equippedGun.gameObject);
		}
		equippedGun = Instantiate (gun, weaponHold, false);
		equippedGun.OnAmmoFinished += OnAmmoFinished;
	}

	public void OnAmmoFinished(){
		Destroy(equippedGun);
		EquipGun (startingGun);
	}

	public void OnTriggerHold(){
		if (equippedGun != null) {
            equippedGun.OnTriggerHold();
		}
	}

    public void OnTriggerReleased() {
        if (equippedGun != null) {
            equippedGun.OnTriggerReleased();
        }
    }

    public void Reload() {
		if(equippedGun != null){
			equippedGun.Reload();
		}
    }

    public Gun GetGun() {
        return equippedGun;
    }

	public float GetHeight(){
		return weaponHold.position.y;
	}

}
