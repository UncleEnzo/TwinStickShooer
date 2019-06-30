using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PlayerHUBController : MonoBehaviour
{
    #region Singleton
    void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning("More than one Instance of Inventory found.");
        }
        Instance = this;
    }
    #endregion

    public static PlayerHUBController Instance;

    //Call to update health on UI
    //Ammo for each gun
    //crafing materials
    //recipies
    //timers
    //powerups
    public void updateDisplayHubHealth(float health)
    {
        foreach (TextMeshProUGUI uiElement in GetComponentsInChildren<TextMeshProUGUI>())
        {
            if (uiElement.tag == TagsAndLabels.HealthUITag)
            {
                uiElement.text = "Health: " + health.ToString();
            }
        }
    }
    public void updateDisplayHubGun(WeaponType gunName)
    {
        foreach (TextMeshProUGUI uiElement in GetComponentsInChildren<TextMeshProUGUI>())
        {
            if (uiElement.tag == TagsAndLabels.GunUITag)
            {
                uiElement.text = "Gun: " + gunName.ToString();
            }
        }
    }
    public void updateDisplayHubAmmo(float ammo)
    {
        foreach (TextMeshProUGUI uiElement in GetComponentsInChildren<TextMeshProUGUI>())
        {
            if (uiElement.tag == TagsAndLabels.AmmoUITag)
            {
                uiElement.text = "Ammo: " + ammo.ToString();
            }
        }
    }
}
