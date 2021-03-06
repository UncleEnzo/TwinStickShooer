﻿using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class RecipeChest : TreasureChest
{
    void Start()
    {
        GetRecipePanelUIButtons();
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
        base.OpenChest();
        GameObject physicalRecipe = spawnRecipe(LootListType.PhysicalRecipe, -100, -100, chestID);
        GameObject gunPowderRecipe = spawnRecipe(LootListType.GunpowderRecipe, 0, 100, chestID);
        GameObject explosiveRecipe = spawnRecipe(LootListType.ExplosiveRecipe, 100, -100, chestID);
        Time.timeScale = 0;
        Player.Instance.enablePlayer(false);
        InventoryUI.canUseUI = false;
        PauseMenu.otherMenuOpen = true;
        //physical
        updateRecipeUI(physicalRecipe, physicalIcon, damageTextPhysical,
            physicalEffectText, physicalCraftPhysText, physicalCraftGunpowderText,
            physicalCraftExplosiveText, physicalButton);
        //gunPowder
        updateRecipeUI(gunPowderRecipe, gunpowderIcon, damageTextGunPowder,
            gunPowderEffectText, gunpowderCraftPhysText, gunpowderCraftGunpowderText,
            gunpowderCraftExplosiveText, gunPowderButton);
        //explosive
        updateRecipeUI(explosiveRecipe, explosiveIcon, damageTextExplosive,
            explosiveEffectText, explosiveCraftPhysText, explosiveCraftGunpowderText,
            explosiveCraftExplosiveText, explosiveButton);
        RecipeUIPanel.SetActive(true);
        return chestID;
    }
}
