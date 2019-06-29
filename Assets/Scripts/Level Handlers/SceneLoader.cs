using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    //Methods for buttons (Does not allow static method calls to be assigned to buttons)
    public void ButtonLoadNextScene()
    {
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

    public void ButtonLoadStartScene()
    {
        SceneManager.LoadScene(0);
        if (PersistentGameData.Instance != null)
        {
            PersistentGameData.Instance.resetPersistentStats();
        }

    }

    public void ButtonLoadDevTestScene()
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

    public void ButtonGameOverScene()
    {
        print("Going to game over screen");
        int gameOverScene = SceneManager.sceneCountInBuildSettings - 1;
        SceneManager.LoadScene(gameOverScene);
        if (PersistentGameData.Instance != null)
        {
            PersistentGameData.Instance.resetPersistentStats();
        }
    }

    public void ButtonQuitGame()
    {
        print("Quitting the game");
        Application.Quit();
    }

    //In game methods
    public static void LoadNextScene()
    {
        //print("Going to next level");
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

    public static void LoadStartScene()
    {
        SceneManager.LoadScene(0);
        if (PersistentGameData.Instance != null)
        {
            PersistentGameData.Instance.resetPersistentStats();
        }

    }

    public static void loadDevTestScene()
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

    public static void loadGameOverScene()
    {
        print("Going to game over screen");
        int gameOverScene = SceneManager.sceneCountInBuildSettings - 1;
        SceneManager.LoadScene(gameOverScene);
        if (PersistentGameData.Instance != null)
        {
            PersistentGameData.Instance.resetPersistentStats();
        }
    }

    public static void QuitGame()
    {
        print("Quitting the game");
        Application.Quit();
    }
}
