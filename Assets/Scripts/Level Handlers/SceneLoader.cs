using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static int hubWorldIndex = 1;
    public static bool LoadingNextScene = false;
    void Awake()
    {
        LoadingNextScene = false;
    }

    //Methods for buttons (Does not allow static method calls to be assigned to buttons)
    public void ButtonLoadSavedGame()
    {
        LoadingNextScene = true;
        SavePersistentData SavePersistentData = SaveSystem.LoadPersistentData();
        if (SavePersistentData != null)
        {
            SceneManager.LoadScene(SavePersistentData.level);
            if (SavePersistentData.level == hubWorldIndex)
            {
                if (PersistentGameData.Instance)
                {
                    PersistentGameData.Instance.resetPersistentGameData();
                }
            }
        }
        else
        {
            print("No Save data (Probably because you made it to win screen or Hub, then quit). Taking player to hub world.");
            SceneManager.LoadScene(hubWorldIndex);
            if (PersistentGameData.Instance != null)
            {
                PersistentGameData.Instance.resetPersistentGameData();
            }
        }
    }

    public void ButtonStartNewGame()
    {
        print("Starting new game");
        LoadingNextScene = true;
        SaveSystem.ResetGlobalMoneyData();
        SaveSystem.ResetPlayerLootPoolData(GetComponent<LootLedger>());
        SaveSystem.ResetVendorLootPoolData(GetComponent<LootLedger>());
        SceneManager.LoadScene(hubWorldIndex);
        if (PersistentGameData.Instance != null)
        {
            PersistentGameData.Instance.resetPersistentGameData();
        }
    }

    public void ButtonLoadStartScreen()
    {
        LoadingNextScene = true;
        SceneManager.LoadScene(0);
        if (PersistentGameData.Instance != null)
        {
            PersistentGameData.Instance.resetPersistentGameData();
        }
    }

    public void ButtonLoadHubWorld()
    {
        //Todo: add save coins and loot table logic
        LoadingNextScene = true;
        SceneManager.LoadScene(hubWorldIndex);
        if (PersistentGameData.Instance != null)
        {
            PersistentGameData.Instance.resetPersistentGameData();
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
        LoadingNextScene = true;
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextScene = currentSceneIndex + 1;
        int lastScene = SceneManager.sceneCountInBuildSettings - 2;
        if (PersistentGameData.Instance)
        {
            PersistentGameData.Instance.saveAndPersistGameData();
        }
        SceneManager.LoadScene(nextScene);

        //resets game data if you reach the win screen
        if (nextScene == lastScene)
        {
            if (PersistentGameData.Instance)
            {
                PersistentGameData.Instance.resetPersistentGameData();
            }
        }
    }

    public static void LoadStartScene()
    {
        LoadingNextScene = true;
        SceneManager.LoadScene(0);
        if (PersistentGameData.Instance != null)
        {
            PersistentGameData.Instance.resetPersistentGameDataRetainSave();
        }
    }

    public static void LoadHubWorld()
    {
        LoadingNextScene = true;
        SceneManager.LoadScene(hubWorldIndex);
        if (PersistentGameData.Instance != null)
        {
            PersistentGameData.Instance.resetPersistentGameData();
        }
    }

    public static void loadGameOverScene()
    {
        print("Going to game over screen");
        LoadingNextScene = true;
        SaveSystem.SaveGlobalMoneyData(Inventory.Instance.getMoneyCount());
        int gameOverScene = SceneManager.sceneCountInBuildSettings - 1;
        SceneManager.LoadScene(gameOverScene);
        if (PersistentGameData.Instance != null)
        {
            PersistentGameData.Instance.resetPersistentGameData();
        }
    }

    public static void QuitGame()
    {
        print("Quitting the game");
        Application.Quit();
    }
}
