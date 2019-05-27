using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadNextScene()
    {
        print("Going to next level");
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextScene = currentSceneIndex + 1;
        int lastScene = SceneManager.sceneCountInBuildSettings - 2;
        if (FindObjectOfType<PersistentGameData>())
        {
            FindObjectOfType<PersistentGameData>().savePlayerStats();
        }
        SceneManager.LoadScene(nextScene);
        if (nextScene == lastScene)
        {
            if (FindObjectOfType<PersistentGameData>())
            {
                FindObjectOfType<PersistentGameData>().resetPersistentStats();
            }
        }
    }

    public void LoadStartScene()
    {
        SceneManager.LoadScene(0);
        if (FindObjectOfType<PersistentGameData>() != null)
        {
            FindObjectOfType<PersistentGameData>().resetPersistentStats();
        }

    }

    public void loadDevTestScene()
    {
        print("Going to dev test level");
        int devTestScene = SceneManager.sceneCountInBuildSettings - 2;
        print(devTestScene);
        SceneManager.LoadScene(devTestScene);
        if (FindObjectOfType<PersistentGameData>() != null)
        {
            FindObjectOfType<PersistentGameData>().resetPersistentStats();
        }
    }

    public void loadGameOverScene()
    {
        print("Going to game over screen");
        int gameOverScene = SceneManager.sceneCountInBuildSettings - 1;
        SceneManager.LoadScene(gameOverScene);
        if (FindObjectOfType<PersistentGameData>() != null)
        {
            FindObjectOfType<PersistentGameData>().resetPersistentStats();
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
