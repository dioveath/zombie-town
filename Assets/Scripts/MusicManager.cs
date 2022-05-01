using UnityEngine;
using UnityEngine.SceneManagement;


public class MusicManager : MonoBehaviour {

	public AudioClip mainMenuClip;
	public AudioClip gamePlayClip;

	void Start(){
		AudioManager.instance.PlayMusic (mainMenuClip, 2f);
		SceneManager.activeSceneChanged += SceneManager_activeSceneChanged;
	}

	void Update(){
	}

	void SceneManager_activeSceneChanged (Scene fromScene, Scene toScene)
	{
		if (toScene.buildIndex == 0) {
			AudioManager.instance.PlayMusic (mainMenuClip, 2f);
		} else if (toScene.buildIndex == 1) {
			AudioManager.instance.PlayMusic (gamePlayClip, 3f);
		}
	}


}
