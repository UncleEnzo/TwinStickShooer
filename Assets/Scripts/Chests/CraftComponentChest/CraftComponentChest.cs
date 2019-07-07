using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftComponentChest : TreasureChest
{
    void Start()
    {
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
        int chestItemDropCount = 0;
        if (chestRarityRange == 0)
        {
            chestItemDropCount = 5;
        }
        else
        {
            chestItemDropCount = chestRarityRange * 10;
        }
        for (int i = 0; i < chestItemDropCount; i++)
        {
            spawnItem(LootListType.CraftComponents, Random.Range(-3f, 3f), Random.Range(-3f, 3f), chestID);
        }
        return chestID;
    }
}
