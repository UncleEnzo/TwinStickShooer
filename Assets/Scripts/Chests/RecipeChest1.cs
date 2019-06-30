using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RecipeChest1 : TreasureChest
{
    private GameObject physicalPanel;
    private Image physicalIcon;
    private TextMeshProUGUI damageTextPhysical;
    private TextMeshProUGUI physicalEffectText;
    private Button physicalButton;
    private GameObject gunpowderPanel;
    private Image gunpowderIcon;
    private TextMeshProUGUI damageTextGunPowder;
    private TextMeshProUGUI gunPowderEffectText;
    private Button gunPowderButton;
    private GameObject explosivePanel;
    private Image explosiveIcon;
    private TextMeshProUGUI damageTextExplosive;
    private TextMeshProUGUI explosiveEffectText;
    private Button explosiveButton;
    void Start()
    {
        physicalPanel = RecipeUIPanel.transform.GetChild(1).gameObject;
        physicalIcon = physicalPanel.transform.GetChild(0).GetComponent<Image>();
        damageTextPhysical = physicalPanel.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();
        physicalEffectText = physicalPanel.transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>();
        physicalButton = physicalPanel.transform.GetChild(3).GetComponent<Button>();
        gunpowderPanel = RecipeUIPanel.transform.GetChild(2).gameObject;
        gunpowderIcon = gunpowderPanel.transform.GetChild(0).GetComponent<Image>();
        damageTextGunPowder = gunpowderPanel.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();
        gunPowderEffectText = gunpowderPanel.transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>();
        gunPowderButton = gunpowderPanel.transform.GetChild(3).GetComponent<Button>();
        explosivePanel = RecipeUIPanel.transform.GetChild(3).gameObject;
        explosiveIcon = explosivePanel.transform.GetChild(0).GetComponent<Image>();
        damageTextExplosive = explosivePanel.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();
        explosiveEffectText = explosivePanel.transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>();
        explosiveButton = explosivePanel.transform.GetChild(3).GetComponent<Button>();
        RecipeUIPanel.SetActive(false);
    }

    void Update()
    {
        CheckDestroyChestHealth();
        bool success = attemptToOpenChest();
        if (success)
        {
            OpenChest();
        }
    }

    protected override int OpenChest()
    {
        int chestID = base.OpenChest();
        GameObject physicalRecipe = spawnRecipe(LootListType.PhysicalRecipe, -100, -100, chestID);
        GameObject gunPowderRecipe = spawnRecipe(LootListType.GunpowderRecipe, 0, 100, chestID);
        GameObject explosiveRecipe = spawnRecipe(LootListType.ExplosiveRecipe, 100, -100, chestID);
        Time.timeScale = 0;
        Player.Instance.enablePlayer(false);

        //physical
        updateRecipeUI(physicalRecipe, physicalIcon, damageTextPhysical, physicalEffectText, physicalButton);
        //GunPowder
        updateRecipeUI(gunPowderRecipe, gunpowderIcon, damageTextGunPowder, gunPowderEffectText, gunPowderButton);
        //Explosive
        updateRecipeUI(explosiveRecipe, explosiveIcon, damageTextExplosive, explosiveEffectText, explosiveButton);
        RecipeUIPanel.SetActive(true);
        return chestID;
    }
    private void updateRecipeUI(GameObject recipe, Image UISprite, TextMeshProUGUI UIDamage, TextMeshProUGUI UIEffect, Button button)
    {
        UISprite.sprite = recipe.GetComponent<RecipePickUp>().item.icon;
        UIDamage.text = recipe.GetComponent<RecipePickUp>().damageDescription;
        UIEffect.text = recipe.GetComponent<RecipePickUp>().effectDescription;
        button.onClick.AddListener(() => { recipe.GetComponent<RecipePickUp>().MakeSelection(); });
    }
}
