using System.Collections;
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

    public void LoadMenu()
    {
        Time.timeScale = 1;
        SceneLoader.LoadStartScene();
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game");
        SceneLoader.QuitGame();
    }
}
