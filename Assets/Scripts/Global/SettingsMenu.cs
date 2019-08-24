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
    public Dropdown antiAliasingDropdown;
    public Dropdown vSyncDropdown;
    public Toggle fullScreenToggle;
    Resolution[] resolutions;
    public GameObject pauseMenuUI;
    public GameObject SettingsMenuUI;
    public int currentResolutionIndex = 0;
    public float currentVolume = 0;
    public int currentQualityIndex = 0;
    public bool currentIsFullScreen = true;
    public int currentVSync = 0;
    public int currentAntiAliasing = 0;
    public int preservedVSync = 0;
    public int preservedAntialiasing = 0;
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
            if (!options.Contains(option))
            {
                options.Add(option);
            }
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
            SetSettings(saveSettingData);
        }
        else
        {
            SystemDefaultSettings();
        }
    }

    private void SystemDefaultSettings()
    {
        //load up values from system default (Resolution is already done)
        currentIsFullScreen = Screen.fullScreen;
        currentQualityIndex = QualitySettings.GetQualityLevel();
        audioMixer.GetFloat("volume", out currentVolume);
        if (currentVolume != 0)
        {
            audioMixer.SetFloat("volume", 0);
            currentVolume = 0;
        }
        currentAntiAliasing = QualitySettings.antiAliasing;
        currentVSync = QualitySettings.vSyncCount;

        //Set the values in the system
        SetFullscreen(currentIsFullScreen);
        SetQuality(currentQualityIndex);
        SetVolume(currentVolume);
        SetVSync(currentVSync);
        SetAntiAliasing(currentAntiAliasing);

        //Adjust the UI elements
        fullScreenToggle.isOn = currentIsFullScreen;
        qualityDropdown.value = currentQualityIndex;
        qualityDropdown.RefreshShownValue();
        volumeSlider.value = currentVolume;
        antiAliasingDropdown.value = currentAntiAliasing;
        antiAliasingDropdown.RefreshShownValue();
        vSyncDropdown.value = currentVSync;
        vSyncDropdown.RefreshShownValue();
    }

    private void SetSettings(SaveSettingData saveSettingData)
    {
        //load up the correct values
        currentIsFullScreen = saveSettingData.isFullScreen;
        currentResolutionIndex = saveSettingData.ResolutionIndex;
        currentQualityIndex = saveSettingData.QualityIndex;
        currentVolume = saveSettingData.Volume;
        currentAntiAliasing = saveSettingData.AntiAliasing;
        currentVSync = saveSettingData.VSync;

        //Set the values in the system
        SetFullscreen(currentIsFullScreen);
        SetResolution(currentResolutionIndex);
        SetQuality(currentQualityIndex);
        SetVolume(currentVolume);
        SetVSync(currentVSync);
        SetAntiAliasing(currentAntiAliasing);

        //Adjust the UI elements
        fullScreenToggle.isOn = currentIsFullScreen;
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
        qualityDropdown.value = currentQualityIndex;
        qualityDropdown.RefreshShownValue();
        volumeSlider.value = currentVolume;
        antiAliasingDropdown.value = currentAntiAliasing;
        antiAliasingDropdown.RefreshShownValue();
        vSyncDropdown.value = currentVSync;
        vSyncDropdown.RefreshShownValue();
    }

    private void ResetSettings()
    {
        //load up the correct values
        currentIsFullScreen = preservedIsFullScreen;
        currentResolutionIndex = preservedResolutionIndex;
        currentQualityIndex = preservedQualityIndex;
        currentVolume = preservedVolume;
        currentAntiAliasing = preservedAntialiasing;
        currentVSync = preservedVSync;

        //Set the values in the system
        SetFullscreen(preservedIsFullScreen);
        SetResolution(preservedResolutionIndex);
        SetQuality(preservedQualityIndex);
        SetVolume(preservedVolume);
        SetVSync(preservedVSync);
        SetAntiAliasing(preservedVSync);

        //Adjust the UI elements
        fullScreenToggle.isOn = preservedIsFullScreen;
        resolutionDropdown.value = preservedResolutionIndex;
        resolutionDropdown.RefreshShownValue();
        qualityDropdown.value = preservedQualityIndex;
        qualityDropdown.RefreshShownValue();
        volumeSlider.value = preservedVolume;
        antiAliasingDropdown.value = currentAntiAliasing;
        antiAliasingDropdown.RefreshShownValue();
        vSyncDropdown.value = currentVSync;
        vSyncDropdown.RefreshShownValue();
    }

    public void SetVSync(int vSync)
    {
        QualitySettings.vSyncCount = vSync;
        currentVSync = vSync;
    }
    public void SetAntiAliasing(int antiAliasing)
    {
        QualitySettings.antiAliasing = (int)Mathf.Pow(2f, antiAliasing);
        currentAntiAliasing = antiAliasing;
    }

    public void SetResolution(int resolutionIndex)
    {
        if (resolutionIndex < 0 || resolutionIndex > (resolutions.Length - 1))
        {
            resolutionIndex = resolutions.Length - 1;
        }
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
