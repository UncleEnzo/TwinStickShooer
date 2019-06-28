using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeChest1 : TreasureChest
{
    void Update()
    {
        bool success = attemptToOpenChest();
        if (success)
        {
            OpenChest();
        }
    }

    protected override int OpenChest()
    {
        int chestID = base.OpenChest();
        spawnRecipe(LootListType.PhysicalRecipe, -3, -3, chestID);
        spawnRecipe(LootListType.GunpowderRecipe, 0, 3, chestID);
        spawnRecipe(LootListType.ExplosiveRecipe, 3, -3, chestID);
        return chestID;
    }
}
