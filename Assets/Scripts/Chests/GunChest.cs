using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunChest : TreasureChest
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
        spawnItem(LootListType.Weapon, -3, -3, chestID);
        return chestID;
    }
}
