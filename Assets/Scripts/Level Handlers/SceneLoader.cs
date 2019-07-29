using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static int hubWorldIndex = 1;

    //note: This will only be called when you make transactions, or some story element happens
    //Note 2: This cannot be a static due to coroutine. So need to call only in here, when transaction is made
    public void displaySaveIcon()
    {
        GameObject saveIcon = GameObject.Find("Canvas").transform.Find("SaveIcon").gameObject;
        if (!saveIcon.activeInHierarchy)
        {
            saveIcon.SetActive(true);
        }
        StartCoroutine(LateCall(saveIcon));
    }

    private IEnumerator LateCall(GameObject saveIcon)
    {
        yield return new WaitForSeconds(2f);
        saveIcon.SetActive(false);
    }

    //Methods for buttons (Does not allow static method calls to be assigned to buttons)
    public void ButtonLoadSavedGame()
    {
        SavePersistentData SavePersistentData = SaveSystem.LoadPersistentData();
        SceneManager.LoadScene(SavePersistentData.level);
        if (SavePersistentData.level == hubWorldIndex)
        {
            if (PersistentGameData.Instance)
            {
                PersistentGameData.Instance.resetPersistentGameData();
            }
        }
    }

    public void ButtonStartNewGame()
    {
        print("STARTING NEW GAME");
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
        SceneManager.LoadScene(0);
        if (PersistentGameData.Instance != null)
        {
            PersistentGameData.Instance.resetPersistentGameData();
        }
    }

    public void ButtonLoadHubWorld()
    {
        //Todo: add save coins and loot table logic
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
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextScene = currentSceneIndex + 1;
        int lastScene = SceneManager.sceneCountInBuildSettings - 2;
        if (PersistentGameData.Instance)
        {
            PersistentGameData.Instance.saveAndPersistGameData();
        }
        SceneManager.LoadScene(nextScene);
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
        PersistentGameData.Instance.saveAndPersistGameData();
        SceneManager.LoadScene(0);
        if (PersistentGameData.Instance != null)
        {
            PersistentGameData.Instance.resetPersistentGameDataRetainSave();
        }
    }

    public static void LoadHubWorld()
    {
        SceneManager.LoadScene(hubWorldIndex);
        if (PersistentGameData.Instance != null)
        {
            PersistentGameData.Instance.resetPersistentGameData();
        }
    }

    public static void loadGameOverScene()
    {
        print("Going to game over screen");
        //Todo: Add save global data 
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
