using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public static bool SettingsMenuOpen = false;
    public static bool otherMenuOpen = false;
    public GameObject pauseMenuUI;
    public GameObject SettingsMenuUI;

    void Update()
    {
        if (!SettingsMenuOpen)
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
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        if (SceneLoader.getCurrentLevel() != SceneLoader.hubWorldIndex)
        {
            Inventory.Instance.moneyIconPanel.SetActive(false);
            if (EnemyRoom.Instance.isMiniMapFaded == false)
            {
                EnemyRoom.Instance.MiniMapMask.SetActive(true);
            }
        }
        if (InventoryUI.UIOpen || otherMenuOpen)
        {
            Time.timeScale = InventoryUI.UITimeScale;
        }
        else
        {
            Time.timeScale = 1;
            Player.Instance.enablePlayer(true);
        }
        InventoryUI.canUseUI = true;
        GameIsPaused = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0;
        if (SceneLoader.getCurrentLevel() != SceneLoader.hubWorldIndex)
        {
            Inventory.Instance.moneyIconPanel.SetActive(true);
            if (EnemyRoom.Instance.isMiniMapFaded == false)
            {
                EnemyRoom.Instance.MiniMapMask.SetActive(false);
            }
        }
        if (!InventoryUI.UIOpen && !otherMenuOpen)
        {
            Player.Instance.enablePlayer(false);
        }
        InventoryUI.canUseUI = false;
        GameIsPaused = true;
    }

    public void AbandonRun()
    {
        Time.timeScale = 1;
        SceneLoader.LoadHubWorld();
    }

    public void OpenSettingsMenu()
    {
        SettingsMenuOpen = true;
        SettingsMenu settingsMenu = GetComponent<SettingsMenu>();
        settingsMenu.preservedIsFullScreen = settingsMenu.currentIsFullScreen;
        settingsMenu.preservedQualityIndex = settingsMenu.currentQualityIndex;
        settingsMenu.preservedResolutionIndex = settingsMenu.currentResolutionIndex;
        settingsMenu.preservedVolume = settingsMenu.currentVolume;
        pauseMenuUI.SetActive(false);
        SettingsMenuUI.SetActive(true);
    }

    public void ReturnToStartScreen()
    {
        Debug.Log("Returning to start screen");
        Time.timeScale = 1f;
        SceneLoader.LoadStartScene();
    }
}
