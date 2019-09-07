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
    private const float DAMAGED_HEALTH_FADE_TIMER_MAX = 1f;
    private Image HealthBarImage;
    private Transform BgFadeFillImageTemplate;
    private Color bgFadeColor;
    private float bgFadeTimer;
    private GameObject healthSliderObj;
    private float HealthBarWidth;

    void Start()
    {
        GameObject Canvas = GameObject.Find("Canvas");
        healthSliderObj = Canvas.transform.Find("HealthSlider").gameObject;
        HealthBarImage = healthSliderObj.transform.Find("Fill").GetComponent<Image>();
        BgFadeFillImageTemplate = healthSliderObj.transform.Find("BackgroundFadeFill");
        HealthBarWidth = healthSliderObj.GetComponent<RectTransform>().rect.width;
        // bgFadeColor = BgFadeFillImageTemplate.color;
        // bgFadeColor.a = 0f;
        // BgFadeFillImageTemplate.color = bgFadeColor;

        if (SceneManager.GetActiveScene().buildIndex == SceneLoader.hubWorldIndex)
        {
            healthSliderObj.gameObject.SetActive(false);
            Canvas.transform.Find("KeyPanel").gameObject.SetActive(false);
            Canvas.transform.Find("Gun").gameObject.SetActive(false);
            Canvas.transform.Find("Ammo").gameObject.SetActive(false);
        }
    }
    private void Update()
    {
        // if (bgFadeColor.a > 0)
        // {
        //     bgFadeTimer -= Time.deltaTime;
        //     if (bgFadeTimer < 0)
        //     {
        //         float fadeAmount = 5f;
        //         bgFadeColor.a -= fadeAmount * Time.deltaTime;
        //         BgFadeFillImageTemplate.color = bgFadeColor;
        //     }
        // }
    }

    public void updateDisplayHubHealth(float healthAfterDamage, float totalHealth, float healthDifference = 0)
    {
        foreach (TextMeshProUGUI uiElement in GetComponentsInChildren<TextMeshProUGUI>())
        {
            if (uiElement.tag == TagsAndLabels.HealthUITag)
            {
                float beforeDamagedBarFillAmount = HealthBarImage.fillAmount;
                uiElement.text = healthAfterDamage.ToString() + "/" + totalHealth.ToString();
                HealthBarImage.fillAmount = (healthAfterDamage / totalHealth);
                Transform bgFadeFillBar = Instantiate(BgFadeFillImageTemplate, BgFadeFillImageTemplate.transform);
                // bgFadeFillBar.SetParent(BgFadeFillImageTemplate);
                bgFadeFillBar.gameObject.GetComponent<Image>().enabled = true;
                bgFadeFillBar.GetComponent<RectTransform>().anchoredPosition = new Vector2(((HealthBarImage.fillAmount * HealthBarWidth) + (HealthBarWidth / 2)), bgFadeFillBar.GetComponent<RectTransform>().anchoredPosition.y);
                bgFadeFillBar.GetComponent<Image>().fillAmount = beforeDamagedBarFillAmount - HealthBarImage.fillAmount;
                bgFadeFillBar.gameObject.AddComponent<HealthBarCutFallDown>();
                print(HealthBarImage.fillAmount);
                //     if (bgFadeColor.a <= 0)
                //     {
                //         //damaged bar image is invisible
                //         BgFadeFillImageTemplate.fillAmount = ((healthAfterDamage + healthDifference) / totalHealth);
                //     }
                //     //damaged bar is already visible
                //     bgFadeColor.a = 1;
                //     BgFadeFillImageTemplate.color = bgFadeColor;
                //     bgFadeTimer = DAMAGED_HEALTH_FADE_TIMER_MAX;
                // }
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
