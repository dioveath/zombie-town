using UnityEngine;



public class CrossHair : MonoBehaviour {

	public LayerMask enemyMask;
	public SpriteRenderer nobRenderer;
	public SpriteRenderer aimRenderer;
	Color originalNobColor;
	Color originalaimColor;
	public float rotateSpeed;

	void Awake(){
		originalaimColor = aimRenderer.color;
		originalNobColor = nobRenderer.color;
	}

	void Start(){
		Cursor.visible = false;
	}

	void Update(){
		transform.Rotate (Vector3.forward * Time.deltaTime * rotateSpeed);
	}

	public void DetectTargets(Ray ray){
		if (Physics.Raycast (ray, 100, enemyMask)) {
			rotateSpeed = 360;
			transform.localScale = new Vector3 (0.3f, 0.3f, 0.3f);
			nobRenderer.color = Color.red;
			aimRenderer.color = Color.red;
		} else {
			rotateSpeed = 40;
			nobRenderer.color = originalNobColor;
			aimRenderer.color = originalaimColor;
			transform.localScale = new Vector3 (0.2f, 0.2f, 0.2f);
		}
	}

}
