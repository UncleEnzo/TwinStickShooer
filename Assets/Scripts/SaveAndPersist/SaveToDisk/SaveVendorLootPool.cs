using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveVendorLootPool
{
    public Dictionary<VendorType, List<string>> VendorLootPool = new Dictionary<VendorType, List<string>>();

    //New Game method constructor, Overwrites all save game data
    public SaveVendorLootPool(LootLedger LootLedger)
    {
        VendorLootPool.Clear();
        foreach (KeyValuePair<VendorType, List<string>> entry in LootLedger.NewGameVendorItemPool)
        {
            VendorLootPool.Add(entry.Key, entry.Value);
        }
    }

    public SaveVendorLootPool(Dictionary<VendorType, List<string>> SaveData)
    {

    }
}
