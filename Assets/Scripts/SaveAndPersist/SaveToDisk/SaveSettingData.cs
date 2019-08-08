using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveSettingData
{
    public float Volume;
    public bool isFullScreen;
    public int QualityIndex;
    public int ResolutionIndex;
    public int VSync;
    public int AntiAliasing;

    public SaveSettingData(SettingsMenu SettingsMenu)
    {
        Volume = SettingsMenu.currentVolume;
        isFullScreen = SettingsMenu.currentIsFullScreen;
        QualityIndex = SettingsMenu.currentQualityIndex;
        ResolutionIndex = SettingsMenu.currentResolutionIndex;
        VSync = SettingsMenu.currentVSync;
        AntiAliasing = SettingsMenu.currentAntiAliasing;
    }
}

