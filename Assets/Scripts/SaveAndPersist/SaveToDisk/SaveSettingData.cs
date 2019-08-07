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

    public SaveSettingData(SettingsMenu SettingsMenu)
    {
        Volume = SettingsMenu.currentVolume;
        isFullScreen = SettingsMenu.currentIsFullScreen;
        QualityIndex = SettingsMenu.currentQualityIndex;
        ResolutionIndex = SettingsMenu.currentResolutionIndex;
    }
}

