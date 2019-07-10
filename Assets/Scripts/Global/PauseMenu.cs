﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1;
        Player.Instance.enablePlayer(true);
        GameIsPaused = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0;
        Player.Instance.enablePlayer(false);
        GameIsPaused = true;
    }

    public void AbandonRun()
    {
        Time.timeScale = 1;
        SceneLoader.LoadHubWorld();
    }

    //Todo: Settings

    public void ReturnToStartScreen()
    {
        Debug.Log("Returning to start screen");
        Time.timeScale = 1f;
        SceneLoader.LoadStartScene();
    }
}
