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
    private float barWidth;
    private GameObject healthbar;
    private Image barImage;
    private Transform damagedBarTemplate;
    private GameObject damagedBars;
    private TextMeshProUGUI healthText;


    void Start()
    {
        GameObject Canvas = GameObject.Find("Canvas");
        healthbar = Canvas.transform.Find("HealthBar").gameObject;

        if (SceneManager.GetActiveScene().buildIndex == SceneLoader.hubWorldIndex)
        {
            healthbar.gameObject.SetActive(false);
            Canvas.transform.Find("KeyPanel").gameObject.SetActive(false);
            Canvas.transform.Find("Gun").gameObject.SetActive(false);
            Canvas.transform.Find("Ammo").gameObject.SetActive(false);
        }

        barImage = healthbar.transform.Find("bar").GetComponent<Image>();
        damagedBarTemplate = healthbar.transform.Find("damagedBarTemplate");
        barWidth = damagedBarTemplate.GetComponent<RectTransform>().rect.width;
        damagedBars = healthbar.transform.Find("damagedBars").gameObject;
        healthText = healthbar.transform.Find("HealthText").gameObject.GetComponent<TextMeshProUGUI>();
    }

    private float getHealthNormalized(float healthAmount, float healthAmountMax)
    {
        return (float)healthAmount / healthAmountMax;
    }
    public void updateDisplayHubHealth(float healthAmount, float healthAmountMax)
    {
        healthText.text = healthAmount + "/" + healthAmountMax;
        float beforeDamagedBarFillAmount = barImage.fillAmount;
        barImage.fillAmount = getHealthNormalized(healthAmount, healthAmountMax);
        Transform damagedBar = Instantiate(damagedBarTemplate, damagedBars.transform);
        damagedBar.gameObject.SetActive(true);
        damagedBar.GetComponent<RectTransform>().anchoredPosition = new Vector2(barImage.fillAmount * barWidth,
            damagedBar.GetComponent<RectTransform>().anchoredPosition.y);
        damagedBar.GetComponent<Image>().fillAmount = beforeDamagedBarFillAmount - barImage.fillAmount;
        damagedBar.gameObject.AddComponent<HealthBarCutFallDown>();
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
