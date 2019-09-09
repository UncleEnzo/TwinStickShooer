using System.Numerics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarCutFallDown : MonoBehaviour
{
    private RectTransform rectTransform;
    private float fallDownTimer;
    private float fadeTimer;
    private Image image;
    private Color color;

    void Awake()
    {
        rectTransform = transform.GetComponent<RectTransform>();
        image = transform.GetComponent<Image>();
        color = image.color;
        fallDownTimer = 1f;
        fadeTimer = 1f;
    }

    void Update()
    {
        fallDownTimer -= Time.deltaTime;
        if (fallDownTimer < 0)
        {
            float fallSpeed = 30f;
            rectTransform.anchoredPosition += UnityEngine.Vector2.down * fallSpeed * Time.deltaTime;

            fadeTimer -= Time.deltaTime;
            if (fadeTimer < 0)
            {
                float alphaFadeSpeed = 5f;
                color.a -= alphaFadeSpeed * Time.deltaTime;
                image.color = color;
                if (color.a <= 0)
                {
                    // gameObject.SetActive(false);
                    Destroy(gameObject);
                }
            }
        }
    }
}
