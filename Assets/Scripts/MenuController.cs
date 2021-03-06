﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public TMPro.TextMeshProUGUI ScoreText;
    void Start()
    {
        var score = PlayerPrefs.GetInt("HighScore", 0);
        if(ScoreText != null) {
            ScoreText.text = score.ToString();
        }
    }


    public void NewGame() {
        SceneManager.LoadScene("Game");
    }

    public void MainMenu() {
        SceneManager.LoadScene("Menu");
    }

    public void LoadScene(string name) {
        SceneManager.LoadScene(name);
    }
}
