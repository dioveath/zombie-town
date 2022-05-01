using UnityEngine;



public class Muzzleflash : MonoBehaviour {

	public GameObject muzzleFlashObject;
	public SpriteRenderer[] spriteRenderer;
	public Sprite[] muzzleFlashSprites;
	public float flashTime;

	public void Activate(){
		int randomIndex = Random.Range (0, muzzleFlashSprites.GetLength (0) - 1);
		for (int i = 0; i < spriteRenderer.GetLength (0); i++) {
			spriteRenderer [i].sprite = muzzleFlashSprites [randomIndex];
		}
		muzzleFlashObject.SetActive (true);
		Invoke ("Deactivate", flashTime);
	}

	public void Deactivate(){
		muzzleFlashObject.SetActive (false);
	}

}
