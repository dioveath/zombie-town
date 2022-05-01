using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour {

    public enum FireMode {
        SINGLE, BURST, AUTO
    };

	public Transform[] muzzles;
	Muzzleflash muzzleFlash;

    [Header ("Fire Mode")]
    public FireMode gunFireMode;
	public int numOfAmmos;
    public int numOfAmmosPerMag;
    public int numOfAmmoPerBurst;
    public int currentNumAmmoInMag;
    int burstFired;

    bool isReloading = false;
    public float reloadTime = 3f;

	public Projectile projectile;
	public float msTimeBetweenShots = 500;
	public float muzzleVelocity = 35; 
	float nextShotTime;
    bool triggerReleasedSinceLastShot = true;

	[Header ("Gun Effect (Shell)")]
	public Transform shellEjectionPoint;
	public Shell shell;

	[Header ("Gun Sound Effects")]
	public AudioClip fireClip;
    public AudioClip reloadClip;

	public System.Action OnAmmoFinished;

	[Header ("Recoil")]
	public Vector2 recoilMinMax;
	Vector3 currentVelocity;

	void Start(){
		muzzleFlash = GetComponent<Muzzleflash> ();
		if (numOfAmmos > 0) {
			if (numOfAmmos >= numOfAmmosPerMag) {
				currentNumAmmoInMag += numOfAmmosPerMag;
				numOfAmmos -= numOfAmmosPerMag;
			} else {
				currentNumAmmoInMag += numOfAmmos;
				numOfAmmos = 0;
			}
			burstFired = 0;
		}
		if (numOfAmmos == -1) {
			currentNumAmmoInMag = numOfAmmosPerMag;
			burstFired = 0;
		}
	}

	void Update(){
		//Aimate Recoil
		transform.localPosition = Vector3.SmoothDamp (transform.localPosition, Vector3.zero, ref currentVelocity, msTimeBetweenShots/1000);
	}

	public void Reload() {
		if (!isReloading) {
			if (numOfAmmos > 0) {
				if (numOfAmmos >= numOfAmmosPerMag) {
					int toReload = numOfAmmosPerMag - currentNumAmmoInMag;
					currentNumAmmoInMag += toReload;
					numOfAmmos -= toReload;
				} else {
					currentNumAmmoInMag += numOfAmmos;
					numOfAmmos = 0;
				}
				burstFired = 0;
				if (!isReloading) {
					StartCoroutine ("AnimateReload");
				}
			}
			if (numOfAmmos == -1) {
				currentNumAmmoInMag = numOfAmmosPerMag;
				burstFired = 0;
				if (!isReloading) {
					StartCoroutine ("AnimateReload");
				}
			}
			if (numOfAmmos == 0 && currentNumAmmoInMag == 0) {
				if (OnAmmoFinished != null) {
					OnAmmoFinished ();
				}
			}
		}
    }

    IEnumerator AnimateReload() {
		isReloading = true;
		AudioManager.instance.PlaySound (reloadClip, transform.position);
		yield return new WaitForSeconds(reloadClip.length);
		isReloading = false;
    }

	private void Shoot(){
		if (!isReloading && Time.time > nextShotTime && currentNumAmmoInMag >= 1 ) {
            if(gunFireMode == FireMode.SINGLE && !triggerReleasedSinceLastShot){
                return;
            }

            if(gunFireMode == FireMode.BURST && !triggerReleasedSinceLastShot && burstFired >= numOfAmmoPerBurst) {
                return;
            } else {
                burstFired++;
            }
			for (int i = 0; i < muzzles.GetLength (0); i++) {
				Projectile newProjectile = Instantiate (projectile, muzzles[i].position, muzzles[i].rotation);
				newProjectile.SetSpeed (muzzleVelocity);
			}

			Instantiate (shell, shellEjectionPoint.position, shellEjectionPoint.rotation);
			AudioManager.instance.PlaySound (fireClip, transform.position);

			muzzleFlash.Activate ();
            currentNumAmmoInMag--;
			nextShotTime = Time.time + (msTimeBetweenShots / 1000);
			//Recoil
			transform.localPosition -= Vector3.forward * Random.Range (recoilMinMax.x, recoilMinMax.y);
		}
        if(currentNumAmmoInMag == 0) {
            Reload();
        }
	}

    public void OnTriggerHold() {

        Shoot();
        triggerReleasedSinceLastShot = false;
    }

    public void OnTriggerReleased() {
        burstFired = 0;
        triggerReleasedSinceLastShot = true;
    }
}
