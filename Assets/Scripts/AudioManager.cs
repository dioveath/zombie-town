using UnityEngine;
using System.Collections;


public class AudioManager : MonoBehaviour {

	public enum AudioChannel {
		MASTER, 
		MUSIC,
		SFX
	};

	public float masterVolumePercent = .5f;
	public float musicVolumePercent = .5f;
	public float sfxVolumePercent = 1f;

	AudioSource[] musicSources;
	AudioSource sound2DSource;
	int activeAudioIndex = 0;

	AudioAssets assets;

	//Singleton
	public static AudioManager instance;


	AudioListener audioListener;
	Transform playerTransform;

	void Awake(){
		if (instance != null) {
			Destroy (gameObject);
			return;
		}
		instance = this;
		DontDestroyOnLoad (gameObject);
		assets = GetComponent<AudioAssets>();

		GameObject audioListenerObject = new GameObject ("AudioListener");
		audioListener = audioListenerObject.AddComponent<AudioListener> ();
		audioListener.transform.parent = transform;

		musicSources = new AudioSource[2];
		for (int i = 0; i < 2; i++) {
			GameObject musicSourceObject = new GameObject ("Audio Source " + (i + 1));
			musicSources [i] = musicSourceObject.AddComponent<AudioSource> ();
			musicSourceObject.transform.parent = transform;
			musicSources [i].loop = true;
		}

		GameObject soundSourceObject = new GameObject ("Sound Source 2D");
		sound2DSource = soundSourceObject.AddComponent<AudioSource> ();
		sound2DSource.transform.parent = transform;

		masterVolumePercent = PlayerPrefs.GetFloat ("MasterVolume", masterVolumePercent);
		musicVolumePercent = PlayerPrefs.GetFloat ("MusicVolumePercent", musicVolumePercent);
		sfxVolumePercent = PlayerPrefs.GetFloat ("SoundVolumePercent", sfxVolumePercent);
	}

	void Start(){
	}

	void Update(){
		if (FindObjectOfType<Player> () != null) {
			playerTransform = FindObjectOfType<Player> ().transform;
			audioListener.transform.position = playerTransform.position;
		}
	}

	public void PlayMusic(AudioClip audioClip, float fadeDuration){
		activeAudioIndex = 1 - activeAudioIndex;
		musicSources [activeAudioIndex].clip = audioClip;
		musicSources [activeAudioIndex].Play ();
		StartCoroutine (AnimateMusicFade (fadeDuration));
	}

	IEnumerator AnimateMusicFade(float fadeDuration){
		float percent = 0;

		while (percent < 1) {
			percent += (1 / fadeDuration) * Time.deltaTime;
			musicSources [activeAudioIndex].volume = Mathf.Lerp (0, masterVolumePercent * musicVolumePercent, percent);
			musicSources [1 - activeAudioIndex].volume = Mathf.Lerp (masterVolumePercent * musicVolumePercent, 0, percent);
			yield return null;
		}
	}

	public void PlaySound(AudioClip audioClip, Vector3 pos){
		AudioSource.PlayClipAtPoint (audioClip, pos, masterVolumePercent * sfxVolumePercent);
	}

	public void PlaySound(string soundName, Vector3 pos){
		AudioSource.PlayClipAtPoint (assets.GetClipFromName (soundName), pos, masterVolumePercent * sfxVolumePercent);
	}

	public void Play2DSound(string soundName){
		sound2DSource.PlayOneShot (assets.GetClipFromName (soundName), masterVolumePercent * sfxVolumePercent);
	}

	public void SetVolume(float volume, AudioChannel channel){
		switch (channel) {
		case AudioChannel.MASTER:
			masterVolumePercent = volume;
			break;
		case AudioChannel.MUSIC:
			musicVolumePercent = volume;
			break;
		case AudioChannel.SFX:
			sfxVolumePercent = volume;
			break;
		}
		for (int i = 0; i < musicSources.GetLength (0); i++) {
			musicSources [i].volume = masterVolumePercent * musicVolumePercent;
		}

		PlayerPrefs.SetFloat ("MasterVolume", masterVolumePercent);
		PlayerPrefs.SetFloat ("MusicVolumePercent", musicVolumePercent);
		PlayerPrefs.SetFloat ("SoundVolumePercent", sfxVolumePercent);
		PlayerPrefs.Save ();
	}

}
