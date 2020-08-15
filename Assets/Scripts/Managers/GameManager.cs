using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public Text scoreBlack, scoreWhite, bestScoreCount;
    public GameObject tapToStart, tapToContinue, bestScore;
    private int score;
    private bool playerWhite = true;

    private bool startGame = false, endGame = false;

    //other scripts
    public Movement playermovement;
    public Spawner spawner;
    
    // sound
    public Sprite soundOn, soundOff;
    private bool sound;
    public GameObject soundButton;

    public GameObject trailToOn;

    private GoogleInterstitialScript googleAds;

    private void Awake()
    {
        Physics2D.gravity = new Vector3(0, -1, 0);
        CheckPlayerPrefs();

        googleAds = GetComponent<GoogleInterstitialScript>();
    }

    private void Update()
    {
        if (!startGame) WaitActing();
        if (endGame) WaitActing();
    }


    //initial and gameEnding functions
    private void WaitActing()
    {
#if UNITY_ANDROID
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Ended && touch.position.y < Screen.height/2)
            {
                startGame = true;
                if (endGame) SceneManager.LoadScene(0);
            }

        }
#endif
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0) && Input.mousePosition.y < Screen.height/2)
        {
            startGame = true;
            if (endGame) SceneManager.LoadScene(0);
        }
#endif

        if (startGame) StartGame();

    }

    private void StartGame()
    {
        soundButton.SetActive(false);
        bestScore.GetComponent<Animator>().SetBool("Fading", true);
        tapToStart.GetComponent<Animator>().SetBool("Fading", true);
        StartCoroutine(CoroutineHelper.WaitTime(0.5f, delegate () {
            bestScore.SetActive(false);
            tapToStart.SetActive(false);
        }));

        scoreWhite.enabled = true;
        if (playermovement != null) playermovement.enabled = true;
        spawner.enabled = true;
        trailToOn.SetActive(true);
    }

    public void EndGame()
    {
        if (score > PlayerPrefs.GetInt("BestScore")) 
        { 
            PlayerPrefs.SetInt("BestScore", score);
            bestScoreCount.text = Convert.ToString(score);
        }
        googleAds.ShowInterstitial();
        endGame = true;
        tapToContinue.SetActive(true);
    }


    // score 
    public void IncreaseScore()
    {
        score += 1;
        if (playerWhite) scoreWhite.text = Convert.ToString(score);
        else scoreBlack.text = Convert.ToString(score);
    }

    public void ChangeColor()
    {
        playerWhite = !playerWhite;
        if (playerWhite)
        {
            scoreWhite.text = Convert.ToString(score);
            scoreBlack.enabled = false;
            scoreWhite.enabled = true;
        }
        else
        {
            scoreBlack.text = Convert.ToString(score);
            scoreWhite.enabled = false;
            scoreBlack.enabled = true;
        }
    }


    //playerPrefs
    private void CheckPlayerPrefs()
    {
        if (!PlayerPrefs.HasKey("BestScore")) PlayerPrefs.SetInt("BestScore", 0);
        bestScoreCount.text = Convert.ToString(PlayerPrefs.GetInt("BestScore"));

        if (PlayerPrefs.HasKey("Sound"))
        {
            if (Convert.ToBoolean(PlayerPrefs.GetInt("Sound"))) SwitchSoundOn();
            else SwitchSoundOff();
        }
        else SwitchSoundOn();
    }


    //sound
    public void SwitchSoundButton()
    {
        if (sound) SwitchSoundOff();
        else SwitchSoundOn();
    }

    private void SwitchSoundOn()
    {
        sound = true;
        PlayerPrefs.SetInt("Sound", 1);

        soundButton.GetComponent<Button>().image.sprite = soundOn;

        AudioListener.pause = false;
        AudioListener.volume = 1;
    }

    private void SwitchSoundOff()
    {
        sound = false;
        PlayerPrefs.SetInt("Sound", 0);

        soundButton.GetComponent<Button>().image.sprite = soundOff;

        AudioListener.pause = false;
        AudioListener.volume = 0;

    }
}

public class CoroutineHelper : MonoBehaviour
{
    public delegate void AwaitableCallback();

    public static IEnumerator WaitTime(float seconds, AwaitableCallback callback)
    {
        yield return new WaitForSeconds(seconds);
        callback();
    }
}
