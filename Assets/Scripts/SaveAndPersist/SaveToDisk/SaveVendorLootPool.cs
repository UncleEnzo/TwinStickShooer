using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveVendorLootPool
{

    // Later
    //Need to get a vendor working to he'll trade with you

    public Dictionary<string, List<string>> VendorLootPool = new Dictionary<string, List<string>>();

    //New Game method constructor, Overwrites all save game data
    public SaveVendorLootPool(LootLedger LootLedger)
    {
        VendorLootPool.Clear();
        foreach (KeyValuePair<string, List<string>> entry in LootLedger.NewGameVendorItemPool)
        {
            VendorLootPool.Add(entry.Key, entry.Value);
        }
    }

    //Should be saving this THROUGH PersistentGameData whenever you make a transaction with Vendor
    public SaveVendorLootPool(PersistentGameData PersistentGameData)
    {

    }
}
