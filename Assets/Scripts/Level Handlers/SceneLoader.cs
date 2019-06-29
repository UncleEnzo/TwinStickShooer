using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance;

    #region Singleton
    void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (Instance != this)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }
    #endregion
    public void LoadNextScene()
    {
        print("Going to next level");
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextScene = currentSceneIndex + 1;
        int lastScene = SceneManager.sceneCountInBuildSettings - 2;
        if (PersistentGameData.Instance)
        {
            PersistentGameData.Instance.savePlayerStats();
        }
        SceneManager.LoadScene(nextScene);
        if (nextScene == lastScene)
        {
            if (PersistentGameData.Instance)
            {
                PersistentGameData.Instance.resetPersistentStats();
            }
        }
    }

    public void LoadStartScene()
    {
        SceneManager.LoadScene(0);
        if (PersistentGameData.Instance != null)
        {
            PersistentGameData.Instance.resetPersistentStats();
        }

    }

    public void loadDevTestScene()
    {
        print("Going to dev test level");
        int devTestScene = SceneManager.sceneCountInBuildSettings - 2;
        print(devTestScene);
        SceneManager.LoadScene(devTestScene);
        if (PersistentGameData.Instance != null)
        {
            PersistentGameData.Instance.resetPersistentStats();
        }
    }

    public void loadGameOverScene()
    {
        print("Going to game over screen");
        int gameOverScene = SceneManager.sceneCountInBuildSettings - 1;
        SceneManager.LoadScene(gameOverScene);
        if (PersistentGameData.Instance != null)
        {
            PersistentGameData.Instance.resetPersistentStats();
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
