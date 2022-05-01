using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviour {

    public Spawner spawner;
    Player player;
    GunController playerGunController;

    [Header("GameOverUI")]
    public GameObject gameOverUI;
    public Image fadePlane;
    public float fadeTime;
    public Color fadeColor;
    public Text gameOverScoreText;
	public Text congratsText;

    [Header("GameUI")]
    public GameObject gameUI;
	public RectTransform waveBannerUI;
	public RectTransform comboRect;
	public Button pauseButton;
	public Text waveText;
    public Text scoreText;
    public Text healthText;
    public Text ammoText;
	public Text currentAmmoText;
	public Text comboText;
	public Text comboPraiseText;
	public float animateBannerTime = 4f;
	public float animateComboTime = .5f;
	public float animateTakeHitTime = .5f;
	public Color comboColorInitial;
	public Color coolColor;
	public Color excellentColor;
	public Color showTimeColor;
	public Color godColor;
	public Color fadePlaneOriginalColor;
	public Color fadePlanePlayerHitColor;

	[Header("GamePauseUI")]
	public GameObject gamePauseUI;

    [Header("ScoreManager")]
    public float scorePerKill = 10;
    public float streakExpiryTime = 2f;
    float lastKilledTime;
    int streakCount = 0;
    float score = 0;

	[Header ("AudioClips")]
	public AudioClip playerTakeHit;

    PlayerPrefs playerPrefs;


    private void Awake() {
        gameOverUI.SetActive(false);
        player = FindObjectOfType<Player>();
        player.OnDeath += OnPlayerDeath;
		player.OnTakeHit += OnPlayerTakeHit;
        spawner.OnEnemyKilled += KilledEnemy;
		spawner.OnNextWaveEvent += OnNextWave_Spawner;
        playerGunController = player.GetComponent<GunController>();
    }

    private void Start() {  
        score = 0;
		comboText.gameObject.SetActive (false);
		comboPraiseText.gameObject.SetActive (false);
    }

    private void Update() {
        scoreText.text = "Score: " + score.ToString("000000");
        healthText.text = "Health: " + player.GetHealth();
		ammoText.text = "Ammo: " + playerGunController.GetGun().numOfAmmos;
		comboText.text = "Combo X" + streakCount;
		currentAmmoText.text = "Current Ammo: " + playerGunController.GetGun ().currentNumAmmoInMag;
    }

	public void OnPausePressed(){
		Time.timeScale = 0f;
		Cursor.visible = true;
		gamePauseUI.SetActive (true);
		pauseButton.interactable = false;
	}

	public void OnResumePressed(){
		Time.timeScale = 1f;
		Cursor.visible = false;
		gamePauseUI.SetActive (false);
		pauseButton.interactable = true;
	}

    public void OnPlayerDeath() {
		Cursor.visible = true;
		AudioManager.instance.PlaySound ("PlayerDeath", player.transform.position);
		int highScore = (int)PlayerPrefs.GetInt ("HighScore", 0);
		if (score > highScore) {
			PlayerPrefs.SetInt ("HighScore", (int)score);
			congratsText.gameObject.SetActive (true);
		} else {
			congratsText.gameObject.SetActive (false);
		}
        StartCoroutine("Fade");
        gameUI.SetActive(false);
    }

    IEnumerator Fade() {

        float percent = 0;
        float fadeSpeed = 1 / fadeTime;
        Color originalColor = fadePlane.color;

        while(percent <= 1) {
            percent += fadeSpeed * Time.deltaTime;
            fadePlane.color = Color.Lerp(originalColor, fadeColor, percent);
            yield return null;
        }
        gameOverScoreText.text = "Score: " + score.ToString("000000");
        gameOverUI.SetActive(true);
    }

    public void PlayAgainPressed() {
        SceneManager.LoadScene(1);
    }

    public void MainMenuPressed() {
		Cursor.visible = true;
		Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void KilledEnemy() {
        if(Time.time < lastKilledTime + streakExpiryTime) {
            streakCount++;
        } else {
            streakCount = 1;
        }

        lastKilledTime = Time.time;

		score += (scorePerKill * streakCount);

		if (streakCount > 2) {
			comboText.color = comboColorInitial;
			comboPraiseText.color = comboColorInitial;
			comboPraiseText.text = "";
			StopCoroutine ("AnimateCombo");
			StartCoroutine ("AnimateCombo");
			switch (streakCount) {
			case 5:
			case 6:
			case 7:
				comboText.color = coolColor;
				comboPraiseText.color = coolColor;
				comboPraiseText.text = "Cool";
				break;
			case 8:
			case 9:
			case 10:
				comboText.color = excellentColor;
				comboPraiseText.color = excellentColor;
				comboPraiseText.text = "Excellent";
				break;
			case 11:
			case 12:
			case 13:
			case 14:
				comboText.color = showTimeColor;
				comboPraiseText.color = showTimeColor;
				comboPraiseText.text = "ShowTime";
				break;
			}
			if (streakCount >= 15) {
				comboPraiseText.text = "You're God";
				comboText.color = godColor;
				comboPraiseText.color = godColor;
			}
		}

    }

	IEnumerator AnimateCombo(){
		comboText.gameObject.SetActive (true);
		comboPraiseText.gameObject.SetActive (true);
		float percent = 0;
		float animateSpeed = 180 / animateComboTime;
		float animationAngle = 0;
		Vector3 newLocalScale = new Vector3 (1.2f, 1.3f, 0);

		while (animationAngle <= 180) {
			percent = Mathf.Sin ( animationAngle / 180 * (float) Math.PI);
			comboRect.localScale = Vector3.Lerp (Vector3.one, newLocalScale, percent);
			animationAngle += animateSpeed * Time.deltaTime;
			yield return null;
		}
		yield return new WaitForSeconds (streakExpiryTime);
		comboText.gameObject.SetActive (false);
		comboPraiseText.gameObject.SetActive (false);
	}

	public void OnNextWave_Spawner(){
		String waveNumberText = "";
		switch (spawner.GetCurrentWaveIndex ()) {
		case 0:
			waveNumberText = "One";
			break;
		case 1:
			waveNumberText = "Two";
			break;
		case 2:
			waveNumberText = "Three";
			break;
		case 3:
			waveNumberText = "Four";
			break;
		case 4:
			waveNumberText = "Five";
			break;

		}
		waveText.text = "Wave: " + waveNumberText;

		StartCoroutine ("AnimateBanner");
	}

	IEnumerator AnimateBanner(){
		float percent = 0;
		float animateSpeed = 180 / animateBannerTime;
		float animationAngle = 0;
		Vector3 originalPosition = new Vector3 (0, -100, 0);
		Vector3 finalPosition = new Vector3 (0, 100, 0);

		while (animationAngle <= 180) {
			percent = Mathf.Sin ( animationAngle / 180 * (float) Math.PI);

			waveBannerUI.anchoredPosition = Vector3.Lerp (originalPosition, finalPosition, percent);
			animationAngle += animateSpeed * Time.deltaTime;
			yield return null;
		}
	}

	public void OnPlayerTakeHit(){
		StopCoroutine ("AnimatePlayerTakeHit");
		StartCoroutine ("AnimatePlayerTakeHit");
		AudioManager.instance.PlaySound ("PlayerTakeHit", player.transform.position);
	}

	IEnumerator AnimatePlayerTakeHit(){
		float percent = 0;
		float animateSpeed = 180 / animateTakeHitTime;
		float animationAngle = 0;

		while (animationAngle <= 180) {
			percent = Mathf.Sin ( animationAngle / 180 * (float) Math.PI);
			fadePlane.color = Color.Lerp (fadePlaneOriginalColor, fadePlanePlayerHitColor, percent);
			animationAngle += animateSpeed * Time.deltaTime;
			yield return null;
		}

	}

}
