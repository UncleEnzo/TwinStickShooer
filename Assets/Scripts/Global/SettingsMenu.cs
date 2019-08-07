using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Dropdown resolutionDropdown;
    public Slider volumeSlider;
    public Dropdown qualityDropdown;
    public Toggle fullScreenToggle;
    Resolution[] resolutions;
    public GameObject pauseMenuUI;
    public GameObject SettingsMenuUI;
    public int currentResolutionIndex = 0;
    public float currentVolume = 0;
    public int currentQualityIndex = 0;
    public bool currentIsFullScreen = true;
    public int preservedResolutionIndex = 0;
    public float preservedVolume = 0;
    public int preservedQualityIndex = 0;
    public bool preservedIsFullScreen = true;

    void Start()
    {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);
            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        SaveSettingData saveSettingData = SaveSystem.LoadSettingsData();
        if (saveSettingData != null)
        {
            SetSettings(saveSettingData.isFullScreen, saveSettingData.ResolutionIndex, saveSettingData.QualityIndex, saveSettingData.Volume);
        }
    }

    private void SetSettings(bool isFullScreen, int ResolutionIndex, int QualityIndex, float Volume)
    {
        //load up the correct values
        currentIsFullScreen = isFullScreen;
        currentResolutionIndex = ResolutionIndex;
        currentQualityIndex = QualityIndex;
        currentVolume = Volume;

        //Set the values in the system
        SetFullscreen(currentIsFullScreen);
        SetResolution(currentResolutionIndex);
        SetQuality(currentQualityIndex);
        SetVolume(currentVolume);

        //Adjust the UI elements
        fullScreenToggle.isOn = currentIsFullScreen;
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
        qualityDropdown.value = currentQualityIndex;
        qualityDropdown.RefreshShownValue();
        volumeSlider.value = currentVolume;
    }


    private void ResetSettings()
    {
        //load up the correct values
        currentIsFullScreen = preservedIsFullScreen;
        currentResolutionIndex = preservedResolutionIndex;
        currentQualityIndex = preservedQualityIndex;
        currentVolume = preservedVolume;

        //Set the values in the system
        SetFullscreen(preservedIsFullScreen);
        SetResolution(preservedResolutionIndex);
        SetQuality(preservedQualityIndex);
        SetVolume(preservedVolume);

        //Adjust the UI elements
        fullScreenToggle.isOn = preservedIsFullScreen;
        resolutionDropdown.value = preservedResolutionIndex;
        resolutionDropdown.RefreshShownValue();
        qualityDropdown.value = preservedQualityIndex;
        qualityDropdown.RefreshShownValue();
        volumeSlider.value = preservedVolume;
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        currentResolutionIndex = resolutionIndex;
    }
    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume);
        currentVolume = volume;
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
        currentQualityIndex = qualityIndex;
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        currentIsFullScreen = isFullscreen;
    }

    public void Apply()
    {
        SaveSystem.SaveSettingsData(this);
        SettingsMenuUI.SetActive(false);
        pauseMenuUI.SetActive(true);
        PauseMenu.SettingsMenuOpen = false;
    }

    public void Cancel()
    {
        ResetSettings();
        SettingsMenuUI.SetActive(false);
        pauseMenuUI.SetActive(true);
        PauseMenu.SettingsMenuOpen = false;
    }
}
