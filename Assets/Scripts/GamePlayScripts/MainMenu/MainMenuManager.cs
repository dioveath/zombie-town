using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenuManager : MonoBehaviour {

	[Header("MainMenu")]
	public GameObject mainMenuObject;
	public Button playButton;
	public Button optionsButton;
	public Button creditsButton;
	public Button exitButton;
	public Text highScoreText;

	[Header("OptionsMenu")]
	public Canvas optionsCanvas;
	public Toggle firstRes;
	public Toggle secondRes;
	public Toggle thirdRes;
	public Slider[] sliders;
	public int[] screenResolutions;
	int currentResolutionIndex;
	bool isFullscreen;


	[Header("CreditsMenu")]
	public Canvas creditsCanvas;
	public RectTransform creditTextT;
	public float creditMoveTime = 2f;
	public float finalCreditTextPositionY;
	Vector3 originalCreditTextPosition;

	[Header("ExitMenu")]
	public Canvas exitCanvas;

	void Start(){
		isFullscreen = PlayerPrefs.GetInt ("isFullscreen", 1) == 1 ? true : false;
		currentResolutionIndex = PlayerPrefs.GetInt ("resIndex", 0);
		OnFullScreen (isFullscreen);
		sliders [0].value = AudioManager.instance.masterVolumePercent;
		sliders [1].value = AudioManager.instance.musicVolumePercent;
		sliders [2].value = AudioManager.instance.sfxVolumePercent;

		int highScore = PlayerPrefs.GetInt ("HighScore", 0);
		highScoreText.text = "HighScore: " + highScore.ToString ("D6");

		originalCreditTextPosition = creditTextT.anchoredPosition;

		exitCanvas.gameObject.SetActive (false);
		optionsCanvas.gameObject.SetActive (false);
		creditsCanvas.gameObject.SetActive (false);
	}

	public void PlayPressed(){
		SceneManager.LoadScene (1);
	}

	public void OptionsPressed(){
		optionsCanvas.gameObject.SetActive (true);
		mainMenuObject.SetActive (false);
	}


	public void CreditsPressed(){
		creditsCanvas.gameObject.SetActive (true);
		mainMenuObject.SetActive (false);
		StopCoroutine ("AnimateCredit");
		StartCoroutine ("AnimateCredit");
	}

	IEnumerator AnimateCredit(){
		float percent = 0;

		creditTextT.anchoredPosition = originalCreditTextPosition;
		yield return new WaitForSeconds (5f);
		Vector3 finalCreditTextPosition = new Vector3 (originalCreditTextPosition.x, finalCreditTextPositionY, originalCreditTextPosition.z);

		while (percent < 1) {
			percent += (1/creditMoveTime) * Time.deltaTime;
			creditTextT.anchoredPosition = Vector3.Lerp (originalCreditTextPosition, finalCreditTextPosition, percent);
			yield return null;
		}
	}

	public void BackPressed(){
		mainMenuObject.SetActive (true);
		creditsCanvas.gameObject.SetActive (false);
		optionsCanvas.gameObject.SetActive (false);
	}

	public void ExitPressed(){
		exitCanvas.gameObject.SetActive (true);
		playButton.interactable = false;
		optionsButton.interactable = false;
		creditsButton.interactable = false;
		exitButton.interactable = false;
	}

	public void exitYesPressed(){
		Application.Quit ();
	}

	public void exitNoPressed(){
		exitCanvas.gameObject.SetActive (false);
		playButton.interactable = true;
		optionsButton.interactable = true;
		creditsButton.interactable = true;
		exitButton.interactable = true;
	}

	public void OnMasterVolumeChanged(float volume){
		AudioManager.instance.SetVolume (volume, AudioManager.AudioChannel.MASTER);
	}

	public void OnMusicVolumeChanged(float volume){
		AudioManager.instance.SetVolume (volume, AudioManager.AudioChannel.MUSIC); 
	}

	public void OnSoundVolmeChanged(float volume){
		AudioManager.instance.SetVolume (volume, AudioManager.AudioChannel.SFX);
	}

	public void OnScreenResolutionChanged(int index){
		if (currentResolutionIndex != index) {
			currentResolutionIndex = index;
		}
		Screen.SetResolution (screenResolutions[currentResolutionIndex], screenResolutions[currentResolutionIndex] * 16/9, false);
		PlayerPrefs.SetInt ("resIndex", index);
		PlayerPrefs.Save ();
	}

	public void OnFullScreen(bool isFull){
		Resolution[] resolutions = Screen.resolutions;
		Resolution maxRes = resolutions [resolutions.GetLength (0) - 1];
		if (isFull) {
			firstRes.interactable = false;
			secondRes.interactable = false;
			thirdRes.interactable = false;
			Screen.SetResolution (maxRes.width, maxRes.height, true);
			PlayerPrefs.SetInt ("isFullscreen", 1);
		} else {
			firstRes.interactable = true;
			secondRes.interactable = true;
			thirdRes.interactable = true;
			PlayerPrefs.SetInt ("isFullscreen", 0);
			OnScreenResolutionChanged (currentResolutionIndex);
		}
		PlayerPrefs.Save ();
	}
		
}
