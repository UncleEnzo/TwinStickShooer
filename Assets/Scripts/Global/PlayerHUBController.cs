using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
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

    void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == SceneLoader.hubWorldIndex)
        {
            GameObject Canvas = GameObject.Find("Canvas");
            Canvas.transform.Find("HealthSlider").gameObject.SetActive(false);
            Canvas.transform.Find("KeyPanel").gameObject.SetActive(false);
            Canvas.transform.Find("Gun").gameObject.SetActive(false);
            Canvas.transform.Find("Ammo").gameObject.SetActive(false);
        }
    }
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
