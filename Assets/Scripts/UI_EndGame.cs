using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_EndGame : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI coins;
    [SerializeField] TextMeshProUGUI score;
    void Start()
    {
        Time.timeScale = 0;
        if(GameManager.instance.coins <= 0)
        {
            return;
        }
        if(GameManager.instance.score <= 0)
        {
            return;
        }
        coins.text = "Coins:   " + GameManager.instance.coins.ToString("#,#");
        score.text = "Score:   " + GameManager.instance.score.ToString("#,#");
    }
}
