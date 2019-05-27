using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PlayerHUBController : MonoBehaviour
{
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
            if (uiElement.tag == "HealthUI")
            {
                uiElement.text = "Health: " + health.ToString();
            }
        }
    }
    public void updateDisplayHubGun(WeaponType gunName)
    {
        foreach (TextMeshProUGUI uiElement in GetComponentsInChildren<TextMeshProUGUI>())
        {
            if (uiElement.tag == "GunUI")
            {
                uiElement.text = "Gun: " + gunName.ToString();
            }
        }
    }
    public void updateDisplayHubAmmo(float ammo)
    {
        foreach (TextMeshProUGUI uiElement in GetComponentsInChildren<TextMeshProUGUI>())
        {
            if (uiElement.tag == "AmmoUI")
            {
                uiElement.text = "Ammo: " + ammo.ToString();
            }
        }
    }
}
