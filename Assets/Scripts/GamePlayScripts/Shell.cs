using UnityEngine;
using System.Collections;



public class Shell : MonoBehaviour {

	Rigidbody myRigidBody;
	public Vector2 forceMinMax;

	float force;

	float fadeStartTime = 2f;
	float fadeTime = 4f;

	public AudioClip[] shellClips;
	bool soundPlayed = false;

	void Start(){
		myRigidBody = GetComponent<Rigidbody> ();
		force = Random.Range (forceMinMax.x, forceMinMax.y);
		myRigidBody.AddForce (transform.right * force);
		myRigidBody.AddTorque (Random.insideUnitSphere * force);
		StartCoroutine ("Fade");
	}

	void Update(){
	}

	IEnumerator Fade(){
		yield return new WaitForSeconds (fadeStartTime);

		float fadeSpeed = 1 / fadeTime;
		float percent = 0;
		Material shellGraphicMaterial = transform.FindChild ("Cube").GetComponent<Renderer> ().material;
		Color originalColor = shellGraphicMaterial.color;

		while (percent < 1) {
			percent += fadeSpeed * Time.deltaTime;
			shellGraphicMaterial.color = Color.Lerp (originalColor, Color.clear, percent);
			yield return null;
		}
		Destroy (gameObject);
	}

	void OnCollisionEnter(Collision collision){
		if (collision.collider.tag == "Floor" && !soundPlayed) {
			int randomShellIndex = Random.Range (0, shellClips.GetLength (0));
			AudioManager.instance.PlaySound (shellClips[randomShellIndex], transform.position);
			soundPlayed = true;
		}
	}

}
