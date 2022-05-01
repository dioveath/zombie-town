using UnityEngine;

public abstract class Collectible : MonoBehaviour{

	public System.Action OnUsed;

	public abstract void EffectPlayer();
}
