using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Handler : MonoBehaviour
{
    private bool gamePaused;
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject inGameMenu;
    [SerializeField] GameObject settingsMenu;
    private GameObject resumeMenu;
    [SerializeField] GameObject endGame;
    [SerializeField] TextMeshProUGUI lastScoreText;
    [SerializeField] TextMeshProUGUI highScoreText;
    [SerializeField] TextMeshProUGUI coinsText;
    [SerializeField] UI_Volume[] sliders;
    [SerializeField] Image muteIcon;
    [SerializeField] Image inGameMuteIcon;
    private bool gameMuted;
    private void Start()
    {
        gameMuted = PlayerPrefs.GetInt("IsGameMuted", 0) == 1;
        if (gameMuted)
        {
            muteIcon.color = new Color(1, 1, 1, 0.5f);
            AudioListener.volume = 0;
        }
        else
        {
            muteIcon.color = Color.white;
            AudioListener.volume = 1;
        }
        for (int i = 0; i< sliders.Length; i++) 
        {
            sliders[i].SetupSlider();
        }
        SwitchMenuTo(mainMenu);
        lastScoreText.text = "Last Score:   " + PlayerPrefs.GetFloat("LastScore").ToString("#,#");
        highScoreText.text = "High Score:   " + PlayerPrefs.GetFloat("HighScore").ToString("#,#");
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            Mute();
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (resumeMenu == pauseMenu)
            {
                resumeMenu = inGameMenu;
                PauseGame();
                SwitchMenuTo(inGameMenu);
            }
            else if (resumeMenu == inGameMenu)
            {
                resumeMenu = pauseMenu;
                PauseGame();
                SwitchMenuTo(pauseMenu);
            }
        }
    }

    public void SwitchMenuTo(GameObject UIMenu)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
        UIMenu.SetActive(true);
        AudioManager.instance.PlayAudio(3);
        coinsText.text = PlayerPrefs.GetInt("Coins").ToString("#,#");
    }

    public void ExitGame() { 
        Application.Quit();
    }

    public void Mute()
    {
        gameMuted= !gameMuted;
        if(gameMuted) 
        {
            muteIcon.color = new Color(1, 1, 1, 0.5f);
            AudioListener.volume= 0;
        }
        else
        {
            muteIcon.color = Color.white;
            AudioListener.volume= 1;
        }
        PlayerPrefs.SetInt("IsGameMuted", gameMuted ? 1 : 0);
    }
    public void StartGame()
    {
        muteIcon = inGameMuteIcon;
        if (gameMuted) 
        {
            muteIcon.color = new Color(1, 1, 1, 0.5f);
        }
        GameManager.instance.UnlockPlayer();
        resumeMenu = inGameMenu;
    }

    public void PauseGame()
    {
        if(gamePaused)
        {
            Time.timeScale = 1;
            gamePaused = false;
        }
        else
        {
            Time.timeScale = 0;
            gamePaused = true;
        }
    }

    public void RestartGame()
    {
        GameManager.instance.RestartLevel();
    }

    public void OpenEndGameUI()
    {
        SwitchMenuTo(endGame);
        resumeMenu = null;
    }
}
