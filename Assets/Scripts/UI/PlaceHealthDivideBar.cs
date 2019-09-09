using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceHealthDivideBar : MonoBehaviour
{
    static int hpBarWidth = 200;
    static int hp_max = 8;
    static int hp_chunks = 1;
    public Transform HPDivider;

    void Start()
    {
        hpBarWidth = (int)this.GetComponent<RectTransform>().rect.width;
        SetupHPBarDividers();
    }

    public void SetupHPBarDividers()
    {
        //removes all previous HPBar dividers
        foreach (Transform child in this.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        float pixelsPerHP = (float)hpBarWidth / hp_max;//~0.6f
        int everyHundredHPLineOffset = Mathf.RoundToInt(pixelsPerHP * hp_chunks);

        int numberOfLines = Mathf.RoundToInt((float)hp_max / hp_chunks);
        for (int i = 1; i < numberOfLines + 1; i++)
        {
            int offset_current = i * everyHundredHPLineOffset;
            Transform HPDivideLine = Instantiate(HPDivider, this.transform);
            HPDivideLine.GetComponent<RectTransform>().anchoredPosition = new Vector2(offset_current - (hpBarWidth / 2), this.GetComponent<RectTransform>().anchoredPosition.y);
        }
    }
}



