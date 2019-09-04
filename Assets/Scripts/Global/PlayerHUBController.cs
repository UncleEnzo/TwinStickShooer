using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
    public Slider healthSlider;

    //Call to update health on UI
    //Ammo for each gun
    //crafting materials
    //recipies
    //timers
    //powerups
    public void updateDisplayHubHealth(float health, float totalHealth)
    {
        foreach (TextMeshProUGUI uiElement in GetComponentsInChildren<TextMeshProUGUI>())
        {
            if (uiElement.tag == TagsAndLabels.HealthUITag)
            {
                uiElement.text = health.ToString() + "/" + totalHealth.ToString();
                healthSlider.maxValue = totalHealth;
                healthSlider.value = health;
            }
        }
    }
    public void updateDisplayHubGun(Sprite gunImage)
    {
        foreach (Image uiElement in GetComponentsInChildren<Image>())
        {
            if (uiElement.tag == TagsAndLabels.GunUITag)
            {
                uiElement.sprite = gunImage;
            }
        }
    }
    public void updateDisplayHubAmmo(float ammo)
    {
        foreach (TextMeshProUGUI uiElement in GetComponentsInChildren<TextMeshProUGUI>())
        {
            if (uiElement.tag == TagsAndLabels.AmmoUITag)
            {
                uiElement.text = ammo.ToString();
            }
        }
    }
}
