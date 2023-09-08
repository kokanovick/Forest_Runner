using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public UI_Handler ui;
    public static GameManager instance;
    public int coins;
    public Player player;
    public float distance;
    public float score;
    private void Awake()
    {
        instance= this;
        Time.timeScale = 1;
    }
    private void Update()
    {
        if(player.transform.position.x > distance) 
        { 
            distance = player.transform.position.x;
        }
    }
    public void UnlockPlayer() 
    {
       player.runBegun = true;
    }
    public void RestartLevel()
    {
        SaveInfo();
        SceneManager.LoadScene(0);
    }
    public void SaveInfo()
    {
        int savedCoins = PlayerPrefs.GetInt("Coins");
        PlayerPrefs.SetInt("Coins", savedCoins + coins);
        score = distance * coins;
        PlayerPrefs.SetFloat("LastScore", score);
        if(PlayerPrefs.GetFloat("HighScore") < score) 
        {
            PlayerPrefs.SetFloat("HighScore", score);
        }
    }
    public void GameEnded()
    {
        SaveInfo();
        ui.OpenEndGameUI();
    }
}
